using AutoMapper;
using Core.Entities;
using Core.Entities.OrderEntities;
using Microsoft.Extensions.Configuration;
using Services.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.OrderService.Services.Dto
{
    public class OrderItemUrlResolver : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly IConfiguration _Configuration;

        public OrderItemUrlResolver(IConfiguration configuration)
        {
            _Configuration = configuration;
        }

        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (!string.IsNullOrEmpty(source.ItemOrdered.PictureUrl))
                return _Configuration["BaseUrl"] + source.ItemOrdered.PictureUrl;

            return null;
        }
    }
  
}
