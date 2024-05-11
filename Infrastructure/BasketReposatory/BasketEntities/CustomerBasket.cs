using System.ComponentModel.DataAnnotations;

namespace Infrastructure.BasketReposatory.BasketEntities
{
    public class CustomerBasket
    {
        [Required]
        public string Id { get; set; }

        public List<BasketItem> BasketItems { get; set; } = new List<BasketItem>();

        public int? DeliveryMethodId { get; set; }

        public decimal ShippingPrice { get; set; }

        public string? PaymentIntentId { get; set; }

        public string? ClientSecret { get; set; }

    }
}
