using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Brands
    {
        [Key]
        public int brand_id { get; set; }
        public string brand_name { get; set; }
        public Brands()
        {
            this.Products = new HashSet<Products>();
        }

        public virtual ICollection<Products> Products { get; set; }
    }
}
