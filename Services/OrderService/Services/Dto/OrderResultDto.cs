using Core.Entities.OrderEntities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Services.OrderService.Services.Dto
{
    public class OrderResultDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }

        public DateTimeOffset OrderDate { get; set; }

        public AddressDto ShippingAddress { get; set; }

        public string DeliveryMethod { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public IReadOnlyList<OrderItemDto> OrderItems { get; set; }

        [Column(TypeName = "money")]
        public decimal SubTotal { get; set; }

        [Column(TypeName = "money")]
        public decimal ShippingPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal Total { get; set; }

        public string? PaymentIntentId { get; set; }
    }
}
