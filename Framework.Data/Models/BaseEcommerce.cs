
namespace Data.Models
{
    public abstract class BaseEcommerce : Base
    {
        public string Titulo { get; set; }
        public string Tags { get; set; }
        public string Url { get; set; }
        public string Slug { get; set; }
    }
}
