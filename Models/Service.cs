using System.ComponentModel.DataAnnotations;

namespace ManicureBooking.Models
{
    public class Service
    {
        [Key]
        public int ServiceId { get; set; }

        [Required(ErrorMessage = "Nazwa usługi jest wymagana.")]
        [Display(Name = "Nazwa usługi")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Opis usługi")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Cena jest wymagana.")]
        [Display(Name = "Cena (zł)")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Czas trwania jest wymagany.")]
        [Display(Name = "Czas trwania (min)")]
        public int DurationInMinutes { get; set; }

        [Display(Name = "Zdjęcie usługi")]
        public string? ImagePath { get; set; }
    }
}