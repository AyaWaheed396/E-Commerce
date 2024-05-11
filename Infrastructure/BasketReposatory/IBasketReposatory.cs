using Infrastructure.BasketReposatory.BasketEntities;

namespace Infrastructure.BasketReposatory
{
    public interface IBasketReposatory
    {
        Task<CustomerBasket> GetBasketAsync(string BasketId);
        Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
        Task<bool> DeleteBasketAsync(string basketId);
    }
}
