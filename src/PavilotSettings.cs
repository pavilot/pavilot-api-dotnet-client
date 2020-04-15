namespace Pavilot.Api.Client
{
    /// <summary>
    /// Pavilot settings interface
    /// </summary>
    public interface IPavilotSettings
    {
        /// <summary>
        /// Base url address for api
        /// e.g: https://v1.pavilot.com
        /// </summary>
        string ApiEndpoint { get; }

        /// <summary>
        /// Api Subscription Key
        /// </summary>
        string ApiKey { get; }
    }

    /// <summary>
    /// Pavilot settings
    /// </summary>
    public class PavilotSettings : IPavilotSettings
    {
        /// <summary>
        /// Base url address for api
        /// e.g: https://v1.pavilot.com
        /// </summary>
        public string ApiEndpoint { get; set; }

        /// <summary>
        /// Api Subscription Key
        /// </summary>
        public string ApiKey { get; set; }
    }
}
