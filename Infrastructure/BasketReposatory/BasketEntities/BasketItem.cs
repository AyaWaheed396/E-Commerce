namespace Infrastructure.BasketReposatory.BasketEntities
{
    public class BasketItem
    {
        public int Id { get; set; }
        public string ProductName { get; set; }

        public decimal Price { get; set; }
        public int Qty { get; set; }

        public string PictureUrl { get; set; }

        public string Brand { get; set; }
        public string Type { get; set; }
    }
}