
[![Build status](https://dissperltd.visualstudio.com/Pavilot/_apis/build/status/pavilot-dotnet-client-library)](https://dissperltd.visualstudio.com/Pavilot/_build/latest?definitionId=30)
[![Version](https://img.shields.io/nuget/v/Pavilot.Api.Client.svg?style=flat)](https://www.nuget.org/packages/Pavilot.Api.Client)

# Pavilot API .Net Client

This library will help you to easily integrate with Pavilot API and focus on your work

To learn more about video automation, please visit [pavilot.com](https://pavilot.com)

## Setup

### 1. Startup.cs

Define configuration in appsettings.json as below

```json
    {
      "PavilotSettings": {
        "ApiEndpoint": "http://v1.pavilot.com",
        "ApiKey": "PavilotSubscriptionKey"
      }
    }
```

Add **services.AddPavilot()** in Startup.ConfigureServices method

```csharp
    ...
    using Pavilot.Api.Client;

    namespace Your.Namespace
    {
        public class Startup
        {
            public Startup(IConfiguration configuration)
            {
                Configuration = configuration;
            }

            public IConfiguration Configuration { get; }

            // This method gets called by the runtime. Use this method to add services to the container.
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddPavilot();
                ...
            }
        }
    }
```

### 2. Manual Initialize

```csharp
    var pavilotSettings = new PavilotSettings
    {
        ApiEndpoint = "http://v1.pavilot.com",
        ApiKey = "PavilotSubscriptionKey"
    };
    var pavilotService = new PavilotService(pavilotSettings);
```

## Usage

*pavilotService* in below code is a reference of *IPavilotService*

```csharp

    // Basic GET methods
    var projects = pavilotService.GetProjectsAsync();

    var animations = pavilotService.GetAnimationsAsync(projectId);

    var videos = pavilotService.GetVideosAsync(projectId, animationId, page: 0);
    
    // Request New Export
    var request = new ExportRequest
    {
        Name = DateTime.Now.ToShortDateString(),
        Message = "Default message to hooks and social media",
        Mappings = new Dictionary<string, string>
        {
          // Retrieve list of keys from Pavilot Portal
          { "Time", DateTime.Now.ToLongDateString() }
        },
        Distributions = new Distributions
        {
          Twitter = "Message to twitter"
        }
    };
    pavilotService.ExportAsync(projectId, animationId, request);
```
