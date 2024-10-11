using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;

namespace SSLChecker;

static class CertificateDownloader
{
    // Accept any certificate, even if the certificate is invalid
    // We don't care about security here. The only goal is to get the certificate, not to transmit data.
    private static readonly RemoteCertificateValidationCallback s_certificateCallback = (_, _, _, _) => true;

    public static async Task<X509Certificate2?> GetCertificateAsync(string domain, int port = 443)
    {
        using var client = new TcpClient(domain, port);
        using var sslStream = new SslStream(client.GetStream(), leaveInnerStreamOpen: true, s_certificateCallback);

        // Initiate the connection, so it will download the server certificate
        await sslStream.AuthenticateAsClientAsync(domain).ConfigureAwait(false);

        // Duplicate the certificate because "serverCertificate" won't be accessible
        // after disposing the stream, so not accessible outsite this method
        var serverCertificate = sslStream.RemoteCertificate;
        if (serverCertificate != null)
            return new X509Certificate2(serverCertificate);

        return null;
    }
}