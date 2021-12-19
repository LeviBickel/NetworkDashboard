using System.Net.NetworkInformation;

namespace NDDevicePoller
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task<bool> PingHost(string hostInfo)
        {
            //Remake this. It should be updating the database. 

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
    }
}