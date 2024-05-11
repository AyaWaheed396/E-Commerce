using Infrastructure.BasketReposatory.BasketEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.BasketServices.Services.Dto
{
    public class CustomerBasketDto
    {
        public string Id { get; set; }

        public List<BasketItemDto> BasketItems { get; set; } = new List<BasketItemDto>();

        public int? DeliveryMethodId { get; set; }

        public decimal ShippingPrice { get; set; }
        public string? PaymentIntentId { get; set; }

        public string? ClientSecret { get; set; }
    }
}
