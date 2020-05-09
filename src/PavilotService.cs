using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Pavilot.Api.Client
{
    /// <summary>
    /// Pavilot Service
    /// Methods for api communication
    /// </summary>
    public interface IPavilotService
    {
        /// <summary>
        /// Generated Client
        /// </summary>
        PavilotClient PavilotClient { get; }

        /// <summary>
        /// Verify settings and initialize Pavilot Client
        /// </summary>
        void VerifyAndSetupClient();

        /// <summary>
        /// Validates Pavilot communication configuration.
        /// Throws ArgumentException if any of the settings is not valid. 
        /// Throws ArgumentNullException if any of the settings is missing.
        /// </summary>
        bool IsValid();

        /// <summary>
        /// Retrieves all projects
        /// </summary>
        Task<IEnumerable<Project>> GetProjectsAsync();

        /// <summary>
        /// Retrieves animations of a project
        /// </summary>
        /// <param name="projectId">Project Id</param>
        Task<IEnumerable<Animation>> GetAnimationsAsync(string projectId);

        /// <summary>
        /// Retrieves videos of an animation
        /// Order is reversed, page 0 retrieves latest generated videos
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <param name="animationId">Animation Id</param>
        /// <param name="page">Page</param>
        Task<IEnumerable<Video>> GetVideosAsync(string projectId, string animationId, int page = 0);

        /// <summary>
        /// Retrieve video to have status details
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <param name="animationId">Animation Id</param>
        /// <param name="videoId">Video Id</param>
        Task<Video> GetVideoAsync(string projectId, string animationId, string videoId);

        /// <summary>
        /// Export a new video with updated data
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <param name="animationId">Animation Id</param>
        /// <param name="request">Mappings and distribution details</param>
        Task<Video> ExportAsync(string projectId, string animationId, ExportRequest request);

        /// <summary>
        /// Subscribe rest hook to receive updates
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <param name="request">Hook filter</param>
        Task<Hook> HookSubscribeAsync(string projectId, HookRequest request);

        /// <summary>
        /// Unsubscribe rest hook to stop receiving updates
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <param name="hookId">Hook Id</param>
        Task<Hook> HookUnsubscribeAsync(string projectId, int hookId);
    }

    /// <summary>
    /// Pavilot Service
    /// </summary>
    public class PavilotService : IPavilotService
    {
        IPavilotSettings Settings { get; }
        HttpClient HttpClient { get; set; }
        IEnumerable<Project> Projects { get; set; }

        /// <summary>
        /// Generated Client
        /// </summary>
        public PavilotClient PavilotClient { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration">Configuration reference to read settings</param>
        public PavilotService(IConfiguration configuration)
        {
            var section = configuration.GetSection(nameof(PavilotSettings));
            if (section.Exists())
            {
                Settings = new PavilotSettings
                {
                    ApiEndpoint = section[nameof(PavilotSettings.ApiEndpoint)],
                    ApiKey = section[nameof(PavilotSettings.ApiKey)]
                };
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="settings">Pavilot settings</param>
        public PavilotService(IPavilotSettings settings)
        {
            Settings = settings;
        }

        /// <summary>
        /// Verify settings and initialize Pavilot Client
        /// </summary>
        public void VerifyAndSetupClient()
        {
            if (IsValid())
            {
                HttpClient = new HttpClient();
                HttpClient.DefaultRequestHeaders.Add("X-Pavilot-Key", Settings.ApiKey);
                PavilotClient = new PavilotClient(Settings.ApiEndpoint, HttpClient);
            }
        }

        /// <summary>
        /// Validates Pavilot communication configuration.
        /// Throws ArgumentException if any of the settings is not valid. 
        /// Throws ArgumentNullException if any of the settings is missing.
        /// </summary>
        public bool IsValid()
        {
            if (Settings == null)
            {
                throw new ArgumentNullException($"{nameof(Settings)} is not defined");
            }

            if (string.IsNullOrWhiteSpace(Settings.ApiEndpoint))
            {
                throw new ArgumentNullException($"{nameof(Settings.ApiEndpoint)} setting is not defined");
            }

            if (!Uri.IsWellFormedUriString(Settings.ApiEndpoint, UriKind.Absolute))
            {
                throw new ArgumentException($"{nameof(Settings.ApiEndpoint)} setting is not valid url");
            }

            if (string.IsNullOrWhiteSpace(Settings.ApiKey))
            {
                throw new ArgumentNullException($"{nameof(Settings.ApiKey)} setting is not defined");
            }

            return true;
        }

        /// <summary>
        /// Retrieves all projects
        /// </summary>
        public async Task<IEnumerable<Project>> GetProjectsAsync()
        {
            VerifyAndSetupClient();
            if (Projects == null)
            {
                Projects = await PavilotClient.V1ProjectsGetAsync();
            }
            return Projects;
        }

        /// <summary>
        /// Retrieves animations of a project
        /// </summary>
        /// <param name="projectId">Project Id</param>
        public Task<IEnumerable<Animation>> GetAnimationsAsync(string projectId)
        {
            VerifyAndSetupClient();
            return PavilotClient.V1ProjectsAnimationsGetAsync(projectId);
        }

        /// <summary>
        /// Retrieves videos of an animation
        /// Order is reversed, page 0 retrieves latest generated videos
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <param name="animationId">Animation Id</param>
        /// <param name="page">Page</param>
        public Task<IEnumerable<Video>> GetVideosAsync(string projectId, string animationId, int page = 0)
        {
            VerifyAndSetupClient();
            return PavilotClient.V1ProjectsAnimationsVideosGetAsync(projectId, animationId, page);
        }

        /// <summary>
        /// Retrieve video to have status details
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <param name="animationId">Animation Id</param>
        /// <param name="videoId">Video Id</param>
        public Task<Video> GetVideoAsync(string projectId, string animationId, string videoId)
        {
            VerifyAndSetupClient();
            return PavilotClient.V1ProjectsAnimationsVideosGetAsync(projectId, animationId, videoId);
        }

        /// <summary>
        /// Export a new video with updated data
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <param name="animationId">Animation Id</param>
        /// <param name="request">Mappings and distribution details</param>
        public Task<Video> ExportAsync(string projectId, string animationId, ExportRequest request)
        {
            VerifyAndSetupClient();
            return PavilotClient.V1ProjectsAnimationsVideosPostAsync(projectId, animationId, request);
        }

        /// <summary>
        /// Subscribe rest hook to receive updates
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <param name="request">Hook filter</param>
        public Task<Hook> HookSubscribeAsync(string projectId, HookRequest request)
        {
            VerifyAndSetupClient();
            return PavilotClient.V1ProjectsHooksPostAsync(projectId, request);
        }

        /// <summary>
        /// Unsubscribe rest hook to stop receiving updates
        /// </summary>
        /// <param name="projectId">Project Id</param>
        /// <param name="hookId">Hook Id</param>
        public Task<Hook> HookUnsubscribeAsync(string projectId, int hookId)
        {
            VerifyAndSetupClient();
            return PavilotClient.V1ProjectsHooksDeleteAsync(projectId, hookId);
        }
    }
}
