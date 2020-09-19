using System;
using System.Net;
using WebDriverManager.DriverConfigs.Impl;
using Xunit;

[assembly: CollectionBehavior(CollectionBehavior.CollectionPerClass, MaxParallelThreads = 2)]
namespace WebDriverManager.Tests
{
    public class DriverManagerTests
    {
        [Fact]
        public void ChromeTest()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
        }

        [Fact]
        public void DownloadZipFileWithProxy()
        {
            var webProxyStub = new WebProxyStub();
            new DriverManager().WithProxy(webProxyStub).SetUpDriver(new ChromeConfig());
            Assert.True(webProxyStub.IsBypassed(webProxyStub.RequestedUri));
        }

        private class WebProxyStub : IWebProxy
        {
            public Uri RequestedUri { get; private set; }
            public ICredentials Credentials { get; set; }
            public Uri GetProxy(Uri destination) => new Uri("http://localhost");

            public bool IsBypassed(Uri host)
            {
                RequestedUri = host;
                return true;
            }
        }
    }

    public class ParallelDriverManagerTests
    {
        [Fact]
        public void ParallelChromeTest()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());
        }
    }
}
