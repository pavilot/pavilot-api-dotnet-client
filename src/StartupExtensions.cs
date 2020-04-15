using Microsoft.Extensions.DependencyInjection;

namespace Pavilot.Api.Client
{
    /// <summary>
    /// Startup Extension for easy integration
    /// </summary>
    public static class StartupExtensions
    {
        /// <summary>
        /// Register PavilotService. Please use IPavilotService 
        /// </summary>
        /// <param name="services"></param>
        /// <param name="settings">Pavilot Settings</param>
        /// <returns></returns>
        public static IServiceCollection AddPavilot(this IServiceCollection services, PavilotSettings settings = null)
        {
            if (settings == null)
            {
                return services.AddSingleton<IPavilotService, PavilotService>();
            }

            return services.AddSingleton<IPavilotService>(new PavilotService(settings));
        }
    }
}
