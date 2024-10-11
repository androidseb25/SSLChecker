using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;

namespace SSLChecker.Controllers;

[ApiExplorerSettings(IgnoreApi = true)]
[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{

    public WeatherForecastController() { }

    [HttpGet(Name = "Test1234")]
    public async Task<IActionResult> Get()
    {
        string url = "example.com";  // Replace with your website URL
        var certificate = await CertificateDownloader.GetCertificateAsync(url);
        Console.WriteLine($"Subject:   {certificate.Subject}");
        Console.WriteLine($"Issuer:    {certificate.Issuer}");
        Console.WriteLine($"NotBefore: {certificate.NotBefore}");
        Console.WriteLine($"NotAfter:  {certificate.NotAfter}");
        Console.WriteLine($"Algorithm: {certificate.SignatureAlgorithm.FriendlyName}");
        return Ok();
    }
}
