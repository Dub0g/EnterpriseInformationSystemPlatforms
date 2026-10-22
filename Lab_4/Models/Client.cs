using Lab_4.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab_4.Models;

        [Table("Clients")]
public class Client
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Вкажіть прізвище")]
        [Display(Name = "Прізвище")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вкажіть ім'я")]
        [Display(Name = "Ім'я")]
        public string FirstName { get; set; } = string.Empty;

        [Display(Name = "По батькові")]
        public string? MiddleName { get; set; }

        [Display(Name = "Адреса")]
        public string? Address { get; set; }

        [Display(Name = "Телефон")]
        public string? Phone { get; set; }

        public List<Orders> Orders { get; set; } = new();
    }




