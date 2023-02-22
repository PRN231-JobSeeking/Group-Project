using Microsoft.AspNetCore.Http;

namespace JobSeekingClient.Models
{
    public class ApplyRequest
    {
        public int PostId { get; set; }
        public IFormFile CvFile { get; set; }
    }
}
