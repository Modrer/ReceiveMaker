using Microsoft.AspNetCore.Http;

namespace ReceiveMaker.Models
{
    public class LoadTemplateModel
    {
        public string id { get; set; }
        public string RepeatingFields { get; set; }
        public string Fields { get; set; }
        public string Name { get; set; }
        public IFormFile file { get; set; }
    }
}
