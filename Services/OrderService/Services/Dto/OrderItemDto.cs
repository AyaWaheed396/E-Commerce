using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.OrderService.Services.Dto
{
    public class OrderItemDto
    {
        public int ProductItemId { get; set; }

        public string ProductName { get; set; }

        public string PictureUrl { get; set; }


        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public int Quantity { get; set; }

    }
}
