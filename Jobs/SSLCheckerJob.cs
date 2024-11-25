using System.Text;
using Microsoft.Extensions.Options;
using Quartz;

namespace SSLChecker.Jobs;

[DisallowConcurrentExecution]
public class SSLCheckerJob : IJob
{
    private readonly ILogger<SSLCheckerJob> _logger;

    public SSLCheckerJob(ILogger<SSLCheckerJob> logger)
    {
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await Task.Run(() => new CheckSSL(_logger));
    }
}

public class CheckSSL
{
    private ILogger<SSLCheckerJob> logger;
    
    public CheckSSL(ILogger<SSLCheckerJob> logger)
    {
        this.logger = logger;
        if (Environments.DomainList.Count > 0)
            CheckCertificates();
    }

    private async void CheckCertificates()
    {
        Console.Clear();
        foreach (string domain in Environments.DomainList)
        {
            var certificate = await CertificateDownloader.GetCertificateAsync(domain);
            if (certificate == null)
            {
                logger.LogInformation($"Certificate not found for domain: {domain}");
                continue;
            }

            DateTime expireDateIn30 = certificate.NotAfter.Date.AddDays(-30);
            DateTime expireDateIn20 = certificate.NotAfter.Date.AddDays(-20);
            DateTime expireDateIn10 = certificate.NotAfter.Date.AddDays(-10);
            DateTime expireDateIn5 = certificate.NotAfter.Date.AddDays(-5);
            DateTime expireDateIn3 = certificate.NotAfter.Date.AddDays(-3);
            DateTime expireDateIn2 = certificate.NotAfter.Date.AddDays(-2);
            DateTime expireDateIn1 = certificate.NotAfter.Date.AddDays(-1);

            if (DateTime.Today >= expireDateIn1)
            {
                string title = $"[{domain}] Certificate expire in 1 day";
                string message =
                    $"Certificate at domain {domain} expires in 1 day at {certificate.NotAfter:dd.MM.yyyy HH:mm:ss}";
                logger.LogInformation(message);
                _ = await SendMessageToGotify(title, message);
            }
            else if (DateTime.Today == expireDateIn2)
            {
                string title = $"[{domain}] Certificate expire in 2 days";
                string message =
                    $"Certificate at domain {domain} expires in 2 days at {certificate.NotAfter:dd.MM.yyyy HH:mm:ss}";
                logger.LogInformation(message);
                bool result = await SendMessageToGotify(title, message);
            }
            else if (DateTime.Today == expireDateIn3)
            {
                string title = $"[{domain}] Certificate expire in 3 days";
                string message =
                    $"Certificate at domain {domain} expires in 3 days at {certificate.NotAfter:dd.MM.yyyy HH:mm:ss}";
                logger.LogInformation(message);
                bool result = await SendMessageToGotify(title, message);
            }
            else if (DateTime.Today == expireDateIn5)
            {
                string title = $"[{domain}] Certificate expire in 5 days";
                string message =
                    $"Certificate at domain {domain} expires in 5 days at {certificate.NotAfter:dd.MM.yyyy HH:mm:ss}";
                logger.LogInformation(message);
                _ = await SendMessageToGotify(title, message);
            }
            else if (DateTime.Today == expireDateIn10)
            {
                string title = $"[{domain}] Certificate expire in 10 days";
                string message =
                    $"Certificate at domain {domain} expires in 10 days at {certificate.NotAfter:dd.MM.yyyy HH:mm:ss}";
                logger.LogInformation(message);
                _ = await SendMessageToGotify(title, message);
            }
            else if (DateTime.Today == expireDateIn20)
            {
                string title = $"[{domain}] Certificate expire in 20 days";
                string message =
                    $"Certificate at domain {domain} expires in 20 days at {certificate.NotAfter:dd.MM.yyyy HH:mm:ss}";
                logger.LogInformation(message);
                _ = await SendMessageToGotify(title, message);
            }
            else if (DateTime.Today == expireDateIn30)
            {
                string title = $"[{domain}] Certificate expire in 30 days";
                string message =
                    $"Certificate at domain {domain} expires in 30 days at {certificate.NotAfter:dd.MM.yyyy HH:mm:ss}";
                logger.LogInformation(message);
                _ = await SendMessageToGotify(title, message);
            }
            else
            {
                string title = $"[{domain}] Certificate is still valid";
                string message = $"Certificate is valid until {certificate.NotAfter:dd.MM.yyyy HH:mm:ss}";
                logger.LogInformation(message);

                if (Environments.SendGotifyMessageAtSuccess)
                {
                    _ = await SendMessageToGotify(title, message);
                }
            }
        }
    }
    
    private async Task<bool> SendMessageToGotify(string title, string message)
    {
        if (Environments.GotifyServerUrl.Length == 0 ||  Environments.GotifyAppToken.Length == 0)
            return false;
        
        using (HttpClient client = new HttpClient())
        {
            // Prepare the message payload
            var content = new StringContent(
                $"{{\"title\": \"{title}\", \"message\": \"{message}\", \"priority\": {Environments.GotifyPriority}}}", 
                Encoding.UTF8, 
                "application/json"
            );

            // Set the authorization header with the app token
            client.DefaultRequestHeaders.Add("X-Gotify-Key", Environments.GotifyAppToken);

            // Send the POST request to the Gotify /message API endpoint
            HttpResponseMessage response = await client.PostAsync($"{Environments.GotifyServerUrl}/message", content);

            // Check if the request was successful
            return response.IsSuccessStatusCode;
        }
    }
}