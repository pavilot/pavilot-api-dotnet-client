using Microsoft.Extensions.Configuration;
using System;
using Xunit;

namespace Pavilot.Api.Client.Tests
{
    public class UnitTests
    {
        [Fact]
        public void PavilotService_Valid_Settings()
        {
            var settings = new PavilotSettings
            {
                ApiEndpoint = "http://v1.pavilot.com",
                ApiKey = "PavilotSubscriptionKey"
            };

            var service = new PavilotService(settings);
            Assert.True(service.IsValid());
        }


        [Fact]
        public void PavilotService_Valid_Configuration()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();

            var service = new PavilotService(configuration);
            Assert.True(service.IsValid());
        }

        [Fact]
        public void PavilotService_InValid_ApiEndpoint_ThrowsArgumentException()
        {
            var settings = new PavilotSettings
            {
                ApiEndpoint = "httpv1pavilotcom",
                ApiKey = "PavilotSubscriptionKey"
            };

            var service = new PavilotService(settings);
            Assert.Throws<ArgumentException>(() => service.IsValid());
        }

        [Fact]
        public void PavilotService_InValid_MissingApiEndpoint_ThrowsArgumentNullException()
        {
            var settings = new PavilotSettings
            {
                ApiKey = "PavilotSubscriptionKey"
            };

            var service = new PavilotService(settings);
            Assert.Throws<ArgumentNullException>(() => service.IsValid());
        }

        [Fact]
        public void PavilotService_InValid_MissingApiKey_ThrowsArgumentNullException()
        {
            var settings = new PavilotSettings
            {
                ApiEndpoint = "http://v1.pavilot.com"
            };

            var service = new PavilotService(settings);
            Assert.Throws<ArgumentNullException>(() => service.IsValid());
        }

        [Fact]
        public void GetProjects_MissingSettings_ThrowsException()
        {
            PavilotSettings settings = null;
            var service = new PavilotService(settings);
            Assert.ThrowsAsync<ArgumentNullException>(() => service.GetProjectsAsync());
        }

        [Fact]
        public void GetProjects_MissingConfiguration_ThrowsException()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.missing.json", optional: false, reloadOnChange: true);
            var configuration = builder.Build();

            var service = new PavilotService(configuration);

            Assert.ThrowsAsync<ArgumentNullException>(() => service.GetProjectsAsync());
        }

    }
}
