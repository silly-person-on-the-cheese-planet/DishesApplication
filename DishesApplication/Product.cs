using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DishesApplication
{
    public class Product
    {
        public string? ProductArticleNumber { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductCategory { get; set; }
        public string? ProductPhoto { get; set; }
        public string? ProductManufacturer { get; set; }
        public decimal ProductCost { get; set; }
        public int ProductQuantityInStock { get; set; }
        public string? ProductStatus { get; set; }
    }
}
