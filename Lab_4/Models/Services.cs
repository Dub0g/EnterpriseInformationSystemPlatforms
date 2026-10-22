
using System.ComponentModel.DataAnnotations;

namespace Lab_4.Models;
public class Services
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Назва")]
        public string Name { get; set; } = null!;

        [Display(Name = "Опис")]
        public string? Description { get; set; }

        public ICollection<OrderDetails> OrderDetails { get; set; } = new List<OrderDetails>();
    }


