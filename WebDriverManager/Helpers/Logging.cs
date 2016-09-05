namespace WebDriverManager.Helpers
{
    using NLog;

    public class Logging
    {
        protected Logger Log { get; private set; }

        protected Logging()
        {
            Log = LogManager.GetLogger(GetType().FullName);
        }
    }
}