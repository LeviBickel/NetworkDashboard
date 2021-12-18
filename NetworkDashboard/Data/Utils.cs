using NetworkDashboard.Models;
using System.Net.NetworkInformation;

namespace NetworkDashboard.Data
{
    public class Utils
    {

        public bool PingHost(string hostInfo)
        {
            bool isPingable = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(hostInfo);
                isPingable = reply.Status == IPStatus.Success;
            }
            catch (PingException ex)
            {
                return false;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }


            return isPingable;
        }
        public async Task<bool> HTTPCheckHostAsync(NetworkDevice host)
        {
            HttpClient client = new HttpClient();
            var checkingResponse = await client.GetAsync(host.DeviceName);
            if (!checkingResponse.IsSuccessStatusCode)
            {
                checkingResponse = await client.GetAsync(host.IPAddress);
                if (!checkingResponse.IsSuccessStatusCode)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            else
            {
                return true;
            }
        }

    }
}
