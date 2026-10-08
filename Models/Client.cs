using System.ComponentModel.DataAnnotations;

namespace Lab_3.Models;

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

    public List<Order> Orders { get; set; } = new();
}