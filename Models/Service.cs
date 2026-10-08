using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab_3.Models;

public class Service
{
    public int ID { get; set; }

    [Required]
    [Display(Name = "Назва")]
    public string Name { get; set; } = null!;

    [Display(Name = "Опис")]
    public string? Description { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
