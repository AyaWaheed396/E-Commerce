using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Delivery : BaseEntity
    {
        
        [Required]

        public string ShortName { get; set; }
        [Required]

        public string Description { get; set; }
        [Required]

        public string DeliveryTime { get; set; }
        [Required]
        [Column(TypeName = "money")]

        public decimal Price { get; set; }


    }
}
