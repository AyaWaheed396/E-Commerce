﻿using Core.Context;
using Core.IdentityContext;
using Core.IdentityEntities;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Helper
{
    public class ApplySeeding
    {
        public static async Task StoreSeeding(WebApplication app)
        {

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var context = services.GetRequiredService<StoreDbContext>();

                    var identityContext = services.GetRequiredService<AppIdentityDbContext>();
                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    
                    await identityContext.Database.MigrateAsync();
                    await context.Database.MigrateAsync();
                    await StoreSeedContext.SeedAsync(context, loggerFactory);
                    await AppIdentityContextSeed.SeedUserAsync(userManager);

                }
                catch (Exception ex)
                {

                    var logger = loggerFactory.CreateLogger<StoreSeedContext>();
                    logger.LogError(ex.Message);
                }
            }
        }
    }
}
