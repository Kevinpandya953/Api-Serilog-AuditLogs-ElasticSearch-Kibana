using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Categories
    {
        [Key]
        public int category_id { get; set; }
        public string category_name { get; set; }
        public Categories()
        {
            this.Products = new HashSet<Products>();
        }
        public virtual ICollection<Products> Products { get; set; }
    }
}
