namespace WebDriverManager.Helpers
{
    using System;
    using System.Text;

    public sealed class WebDriverManagerException : Exception
    {
        private readonly StringBuilder _sb = new StringBuilder();

        public WebDriverManagerException(string message, Exception innerException)
            : base(message, innerException)
        {
            _sb.AppendLine($"Message: {message}");
            _sb.AppendLine($"Exception message: {Message}");
            _sb.AppendLine($"Exception stack trace: {StackTrace}");
            _sb.AppendLine($"InnerException: {innerException}");
        }

        public override string ToString()
        {
            return _sb.ToString();
        }
    }
}