using Core.Entities.OrderEntities;

namespace Infrastructure.Specification
{
    public class OrderWIthPaymentIntentSpecification : BaseSpecification<Order>
    {
        public OrderWIthPaymentIntentSpecification(string paymentIntentId) 
            : base(order => order.PaymentIntentId == paymentIntentId)
        {
        }
    }
}
