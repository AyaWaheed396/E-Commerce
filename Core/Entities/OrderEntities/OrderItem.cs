using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.OrderEntities
{
    public class OrderItem: BaseEntity
    {
        public OrderItem()
        {
            
        }

        public OrderItem(decimal price, int quantity, ProductItemOrdered itemOrdered)
        {
            Price = price;
            Quantity = quantity;
            ItemOrdered = itemOrdered;
        }

        [Column(TypeName ="money")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public ProductItemOrdered ItemOrdered { get; set; }
    }
}
