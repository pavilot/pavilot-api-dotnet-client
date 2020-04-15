using Pavilot.Api.Client;
using System.Collections.Generic;

namespace Pavilot.Client.Sample.Models
{
    public class PavilotItems
    {
        public IEnumerable<Project> Projects { get; set; }
        public IEnumerable<Animation> Animations { get; set; }
        public IEnumerable<Video> Videos { get; set; }
    }
}
