using System.ComponentModel.DataAnnotations;

namespace Lab_5.Models
{
    public class Orders
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Оберіть клієнта")]
        [Display(Name = "Клієнт")]
        public int ClientID { get; set; }

        [Required]
        [Display(Name = "Дата прийому")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Today;

        [Required]
        [Display(Name = "Дата видачі")]
        [DataType(DataType.Date)]
        public DateTime DeliveryDate { get; set; } = DateTime.Today.AddDays(1);

        [Display(Name = "Клієнт")]
        public Clients? Client { get; set; }
        public List<OrderDetails> OrderDetails { get; set; } = new();
    }
}
