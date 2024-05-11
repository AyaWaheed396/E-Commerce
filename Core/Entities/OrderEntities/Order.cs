using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.OrderEntities
{
    public enum OrderStatus
    {
        Pending,
        PaymentRecived,
        PaymentFailed
    }

    public class Order : BaseEntity
    {
        public Order()
        {
            
        }

        public Order(string buyerEmail, ShippingAddress shippingAddress, Delivery deliveryMethod, IReadOnlyList<OrderItem> orderItems, decimal subTotal, string? paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            SubTotal = subTotal;
            PaymentIntentId = paymentIntentId;
        }

        public string BuyerEmail { get; set; }

        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

        public ShippingAddress ShippingAddress { get; set; }

        public Delivery DeliveryMethod { get; set; }

        public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;

        public IReadOnlyList<OrderItem> OrderItems { get; set; }

        [Column(TypeName ="money")]
        public decimal SubTotal { get; set; }

        public string PaymentIntentId { get; set; }

        public decimal GetTotal()
            => SubTotal + DeliveryMethod.Price;


    }
}
