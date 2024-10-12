namespace SSLChecker.Services;

public class StartUpBuilder : IStartupFilter
{
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return builder =>
        {
            Console.Clear();
            if (Environments.GotifyServerUrl.Length == 0)
            {
                Console.WriteLine("Gotify Server isn't configured!");
            }

            if (Environments.GotifyAppToken.Length == 0)
            {
                Console.WriteLine("Gotify App token isn't configured!");
            }

            if (Environments.GotifyPriority < 0)
            {
                Console.WriteLine("Gotify priority isn't supported! Only 0 - 10 is supported!");
            }

            if (Environments.DomainList.Count == 0)
            {
                Console.WriteLine("Domain list isn't configured!");
            }

            next(builder);
        };
    }
}