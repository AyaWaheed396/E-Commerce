using Core.Context;
using Core.Entities;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Infrastructure
{
    public class StoreSeedContext
    {
        public static async Task SeedAsync(StoreDbContext context, ILoggerFactory loggerFactory)
        {
			try
			{
                if (context.ProductBrands != null && !context.ProductBrands.Any())
                    await SeedProductBrandAsync(context);
                if (context.ProductTypes != null && !context.ProductTypes.Any())
                    await SeedProductTypesAsync(context);
                if (context.Products != null && !context.Products.Any())
                    await SeedProductAsync(context);
                if (context.Deliveries != null && !context.Deliveries.Any())
                    await SeedDeliveriesAsync(context);




            }
			catch (Exception ex)
			{

                var logger = loggerFactory.CreateLogger<StoreSeedContext>();
                logger.LogError(ex.Message);
			}
        }
		private static async Task SeedProductBrandAsync(StoreDbContext context)
		{
            if (context.ProductBrands != null && !context.ProductBrands.Any())
            {
                var brands = File.ReadAllText("../Infrastructure/SeedData/brands.json");
                var productBrands = JsonSerializer.Deserialize<List<ProductBrand>>(brands);
                foreach (var productBrand in productBrands)
                    await context.ProductBrands.AddAsync(productBrand);

                await context.SaveChangesAsync();
            }

        }
        private static async Task SeedProductTypesAsync(StoreDbContext context)
        {
            if (context.ProductTypes != null && !context.ProductTypes.Any())
            {
                var types = File.ReadAllText("../Infrastructure/SeedData/types.json");
                var productTypes = JsonSerializer.Deserialize<List<ProductType>>(types);
                foreach (var productType in productTypes)
                    await context.ProductTypes.AddAsync(productType);

                await context.SaveChangesAsync();
            }

        }

        private static async Task SeedProductAsync(StoreDbContext context)
        {
            if (context.Products != null && !context.Products.Any())
            {
                var productsData = File.ReadAllText("../Infrastructure/SeedData/products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsData);
                foreach (var product in products)
                    await context.Products.AddAsync(product);

                await context.SaveChangesAsync();
            }

        }

        private static async Task SeedDeliveriesAsync(StoreDbContext context)
        {
            if (context.Deliveries != null && !context.Deliveries.Any())
            {
                var deliveryData = File.ReadAllText("../Infrastructure/SeedData/delivery.json");
                var deliveries = JsonSerializer.Deserialize<List<Delivery>>(deliveryData);
                foreach (var delivery in deliveries)
                    await context.Deliveries.AddAsync(delivery);

                await context.SaveChangesAsync();
            }

        }
    }
}
