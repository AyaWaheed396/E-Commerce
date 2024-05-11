using System.ComponentModel.DataAnnotations;

namespace Services.BasketServices.Services.Dto
{
    public class BasketItemDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage ="Price Must Be Greater Than Zero ")]
        public decimal Price { get; set; }
        [Required]
        [Range(1, 10, ErrorMessage ="Quantity Must be between 1 and 10 pieces")]
        public int Qty { get; set; }
        [Required]

        public string PictureUrl { get; set; }
        [Required]

        public string Brand { get; set; }
        [Required]

        public string Type { get; set; }
    }
}