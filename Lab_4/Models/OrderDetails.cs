using Lab_4.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Lab_4.Models;

    public class OrderDetails
    {
        public int ID { get; set; }

        public int OrderID { get; set; }

        [Required(ErrorMessage = "Оберіть послугу")]
        [Display(Name = "Послуга")]
        public int ServiceID { get; set; }

        [Required]
        [Range(1, 1000, ErrorMessage = "Кількість має бути більше 0")]
        [Display(Name = "Кількість")]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, 1000000, ErrorMessage = "Ціна має бути більше 0")]
        [Display(Name = "Ціна за одиницю")]
        [DataType(DataType.Currency)]
        public decimal UnitPrice { get; set; }

    [ValidateNever] public Orders Order { get; set; } = null!;
    [ValidateNever] public Services Service { get; set; } = null!;
}




