using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Products
    {
        [Key]
        public int product_id { get; set; }
        public string product_name { get; set; }

        public int brand_id { get; set; }
        public int category_id { get; set; }

        public short model_year { get; set; }
        public decimal list_price { get; set; }
        public virtual Brands Brands { get; set; }
        public virtual Categories Categories { get; set; }
    }
}
