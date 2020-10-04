using System.ComponentModel.DataAnnotations;

namespace ConsumeAPI.Models
{
    public class Profesoret
    {
        public int ProfesoretId { get; set; }

        [Required(ErrorMessage = "Shkruani emrin e profesorit!")]
        [Display(Name = "Emri i Profesorit")]
        public string EmriProfesorit { get; set; }

        [Required(ErrorMessage = "Zgjedheni lenden!")]
        [Display(Name = "Lenda")]
        public int LendaId { get; set; }

        public virtual Lendet Lenda { get; set; }
    }
}
