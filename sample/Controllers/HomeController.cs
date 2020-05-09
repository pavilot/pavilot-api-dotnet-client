using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pavilot.Api.Client;
using Pavilot.Client.Sample.Models;

namespace Pavilot.Client.Sample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPavilotService _pavilotService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(
            IPavilotService pavilotService,
            ILogger<HomeController> logger)
        {
            _pavilotService = pavilotService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string projectId = "", string animationId = "")
        {
            var projects = await _pavilotService.GetProjectsAsync();
            IEnumerable<Animation> animations = null;
            IEnumerable<Video> videos = null;
            if (!string.IsNullOrEmpty(projectId))
            {
                animations = await _pavilotService.GetAnimationsAsync(projectId);

                if (!string.IsNullOrEmpty(animationId))
                {
                    videos = await _pavilotService.GetVideosAsync(projectId, animationId, 0);
                }
            }
            var data = new PavilotItems
            {
                Projects = projects,
                Animations = animations,
                Videos = videos
            };

            ViewBag.ProjectId = projectId;
            ViewBag.AnimationId = animationId;

            return View(data);
        }

        public async Task NewVideo(string projectId, string animationId)
        {
            var request = new ExportRequest
            {
                Name = DateTime.Now.ToShortDateString(),
                Data = new List<Mapping>
                {
                    new Mapping
                    {
                        Key = "Time",
                        Value = DateTime.Now.ToLongDateString()
                    }
                },
                Distributions = new List<Distribution>
                {
                    new Distribution
                    {
                        Message = "Message to social media",
                        Platform = Platform.Twitter
                    }
                }
            };

            await _pavilotService.ExportAsync(projectId, animationId, request);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
