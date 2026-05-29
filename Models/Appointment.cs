using System;
using System.ComponentModel.DataAnnotations;

namespace ManicureBooking.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "Imię i nazwisko są wymagane.")]
        [Display(Name = "Imię i nazwisko klientki")]
        public string ClientName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Adres e-mail jest wymagany.")]
        [EmailAddress(ErrorMessage = "Niepoprawny adres e-mail.")]
        [Display(Name = "Adres e-mail")]
        public string ClientEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Numer telefonu jest wymagany.")]
        [Phone(ErrorMessage = "Niepoprawny numer telefonu.")]
        [Display(Name = "Numer telefonu")]
        public string ClientPhone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Wybór daty i godziny jest wymagany.")]
        [Display(Name = "Data i godzina wizyty")]
        public DateTime AppointmentTime { get; set; }

        [Required(ErrorMessage = "Wybór usługi jest wymagany.")]
        [Display(Name = "Usługa")]
        public int ServiceId { get; set; }
        public Service? Service { get; set; }

        [Display(Name = "Status wizyty")]
        public string Status { get; set; } = "Oczekująca";
    }
}