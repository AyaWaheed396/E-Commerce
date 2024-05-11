using Core.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Specification
{
    public class OrderWithItemsSpecification : BaseSpecification<Order>
    {
        public OrderWithItemsSpecification(string buyerEmail) 
            : base(order => order.BuyerEmail == buyerEmail)
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod); 

            AddOrderByDesc(order => order.OrderDate);
        }

        public OrderWithItemsSpecification(int id,string buyerEmail)
            : base(order =>
                    (order.Id == id)&&
                    (order.BuyerEmail == buyerEmail)
                  )
        {
            AddInclude(order => order.OrderItems);
            AddInclude(order => order.DeliveryMethod);

        }
    }
}
