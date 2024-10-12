namespace SSLChecker;

public class Environments
{
    public static List<string> DomainList
    {
        get
        {
            string domains = Environment.GetEnvironmentVariable("DOMAINS") ?? "";
            if (domains.Length == 0)
                return new List<string>();
            return domains.Split(';').ToList();
        }
    }

    public static string GotifyServerUrl
    {
        get
        {
            return Environment.GetEnvironmentVariable("GOTIFY_URL") ?? "";
        }
    }

    public static string GotifyAppToken
    {
        get
        {
            return Environment.GetEnvironmentVariable("GOTIFY_APP_TOKEN") ?? "";
        }
    }

    public static int GotifyPriority
    {
        get
        {
            return int.Parse(Environment.GetEnvironmentVariable("GOTIFY_PRIORITY") ?? "0");
        }
    }

    public static bool SendGotifyMessageAtSuccess
    {
        get
        {
            return bool.Parse(Environment.GetEnvironmentVariable("SEND_MESSAGE_ON_SUCCESS") ?? "false");
        }
    }

    public static int CheckInterval
    {
        get
        {
            return int.Parse(Environment.GetEnvironmentVariable("INTERVAL") ?? "24");
        }
    }

    public static int StartHour
    {
        get
        {
            string startTime = Environment.GetEnvironmentVariable("START_TIME") ?? "03:00";
            List<string> timeParts = startTime.Split(':').ToList();
            if (timeParts.Count == 2)
            {
                if (timeParts.Last().Length == 2)
                {
                    StartMinute = int.Parse(timeParts.Last());
                }
                else
                {
                    StartMinute = 0;
                }
                if (timeParts.First().Length == 2)
                {
                    return int.Parse(timeParts.First());
                }
            }
            else
            {
                StartMinute = 0;
            }

            return 3;
        }
    }

    public static int StartMinute
    {
        get;
        set;
    }
}