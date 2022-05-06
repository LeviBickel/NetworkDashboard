using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib.Security;
using NDDevicePoller.Data;
using NDDevicePoller.Models;

namespace NDDevicePoller
{

    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly DBHelper _dbHelper;

        List<NetworkDevice> PingDevices = new List<NetworkDevice>();
        List<NetworkDevice> HttpDevices = new List<NetworkDevice>();
        List<NetworkDevice> SNMPDevices = new List<NetworkDevice>();
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            _dbHelper = new DBHelper();

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Tasker();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during tasker");
                }
                finally
                {
                    await Task.Delay(_dbHelper.GetPollInterval(), stoppingToken); 
                }
            }
        }

        private async Task Tasker()
        {
            var Tasks = new List<Task>();
            SortDevices();
            foreach (var device in PingDevices)
            {
                Tasks.Add(PingHost(device));
            }
            foreach (var device in HttpDevices)
            {
                Tasks.Add(HttpCheck(device));
            }
            //SNMP Check here
            await Task.WhenAll(Tasks);
        }

        private void SortDevices()
        {
            PingDevices.Clear();
            HttpDevices.Clear();
            SNMPDevices.Clear();

            var networkDevices = _dbHelper.GetAllDevices();
            var monitorTypes = _dbHelper.GetAllMonitorTypes();

            foreach (var networkDevice in networkDevices)
            {
                if (monitorTypes.Where(m => m.ping == false).Where(m => m.TypeTitle == networkDevice.DeviceType).Count() >= 1)
                {
                    //Add the SNMP List here
                    SNMPDevices.Add(networkDevice);

                    HttpDevices.Add(networkDevice);
                }
                else if (monitorTypes.Where(m=>m.ping == true).Where(m=>m.TypeTitle == networkDevice.DeviceType).Count() >= 1)
                {
                    PingDevices.Add(networkDevice);
                }
                else
                {
                    _logger.LogError($"Device {networkDevice} does not match ping or http service check.");
                }
            }
        }

        public async Task PingHost(NetworkDevice hostInfo)
        {
            bool isPingable = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(hostInfo.IPAddress); 
                if(reply.Status != IPStatus.Success)
                {
                    PingReply replyName = pinger.Send(hostInfo.DeviceName);
                    isPingable = replyName.Status == IPStatus.Success;
                }
                else
                {
                    isPingable = reply.Status == IPStatus.Success;
                }
            }
            catch (PingException ex)
            {
                _logger.LogError(ex.Message);
                isPingable = false;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }
            hostInfo.IsConnected = isPingable;
            await _dbHelper.UpdateConnectionStatus(hostInfo);
            _logger.LogInformation($"Device: {hostInfo.DisplayName} Connection Status: {hostInfo.IsConnected}");
            //return isPingable;
        }

        private async Task HttpCheck(NetworkDevice hostInfo)
        {
            bool connected = false;
            HttpClient client = new HttpClient();
            var checkingResponse = await client.GetAsync(hostInfo.DeviceName);
            if (!checkingResponse.IsSuccessStatusCode)
            {
                checkingResponse = await client.GetAsync(hostInfo.IPAddress);
                if (!checkingResponse.IsSuccessStatusCode)
                {
                    connected = false;
                }
                else
                {
                    connected = true;
                }

            }
            else
            {
                connected = true;
            }

            hostInfo.IsConnected = connected;
            await _dbHelper.UpdateConnectionStatus(hostInfo);
            _logger.LogInformation($"Device: {hostInfo.DisplayName} Connection Status: {hostInfo.IsConnected}");
        }

        private async Task SNMPListener(NetworkDevice hostInfo)
        {
            //Discovery Phase of SNMPv3
            
            Discovery discovery = Messenger.GetNextDiscovery(SnmpType.GetRequestPdu);
            ReportMessage report = discovery.GetResponse(60000, new IPEndPoint(IPAddress.Parse(hostInfo.IPAddress), 161));

            //Authentication
            var credentials = _dbHelper.GetSNMPv3Credentials();
            var userName = new OctetString(credentials.UserName);
            var auth = new SHA256AuthenticationProvider(new OctetString(credentials.AuthPassword));
            var aesPriv = new AESPrivacyProvider(new OctetString(credentials.PrivPassword), auth);
            
            
            
            //Get SNMP
            
            GetRequestMessage request = new GetRequestMessage(VersionCode.V3, Messenger.NextMessageId, Messenger.NextRequestId, userName, new List<Variable> { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.1.0")) }, aesPriv, Messenger.MaxMessageSize, report);
            ISnmpMessage reply = request.GetResponse(60000, new IPEndPoint(IPAddress.Parse(hostInfo.IPAddress), 161));
            if (reply.Pdu().ErrorStatus.ToInt32() != 0)
            {
                _logger.LogError($"Error in response {IPAddress.Parse(hostInfo.IPAddress)}, {reply}");  //log the error
                hostInfo.IsConnected = false;
            }
            else
            {
                _logger.LogInformation($"{hostInfo.DisplayName} polled with SNMPv3. Response: {reply}");
                hostInfo.IsConnected = true;
            }
            await _dbHelper.UpdateConnectionStatus(hostInfo);
        }
       
    }
}