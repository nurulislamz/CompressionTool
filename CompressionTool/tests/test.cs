using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace CompressionTool.Tests
{
    public class CompressionToolTests : IDisposable
    {
        private readonly IHost _host;

        public CompressionToolTests()
        {
            var builder = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    // Register your services here
                    services.AddSingleton<IFileOpener, FileOpener>();
                    services.AddSingleton<IParser, Parser>();
                });

            _host = builder.Build();
        }

        [Fact]
        public void Parser_Should_Count_Characters_Correctly()
        {
            // Arrange
            var parser = _host.Services.GetRequiredService<IParser>();
            string text = "hello";

            // Act
            var result = parser.Parse(text);

            // Assert
            Assert.Equal(1, result['h']);
            Assert.Equal(1, result['e']);
            Assert.Equal(2, result['l']);
            Assert.Equal(1, result['o']);
        }

        public void Dispose()
        {
            _host?.Dispose();
        }
    }
}