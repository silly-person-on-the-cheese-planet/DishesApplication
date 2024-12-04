using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DishesApplication
{
    public class BasketItem
    {
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductPhoto { get; set; }
        public string? ProductManufacturer { get; set; }
        public decimal ProductCost { get; set; }
    }
}
