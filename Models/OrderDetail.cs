using System.ComponentModel.DataAnnotations;

namespace Lab_3.Models;

public class OrderDetail
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

    public Order Order { get; set; } = null!;
    public Service Service { get; set; } = null!;
}