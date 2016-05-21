namespace WebDriverManager.Helpers
{
    using System;
    using System.Text;

    public class WebDriverManagerException : Exception
    {
        protected StringBuilder _sb = new StringBuilder();

        public WebDriverManagerException(string message, Exception innerException)
            : base(message, innerException)
        {
            _sb.AppendLine($"Message: {message}");
            _sb.AppendLine($"Exception message: {base.Message}");
            _sb.AppendLine($"Exception stack trace: {base.StackTrace}");
            _sb.AppendLine($"InnerException: {innerException.ToString()}");
        }

        public override string ToString()
        {
            return _sb.ToString();
        }
    }
}
