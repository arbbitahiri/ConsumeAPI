using ConsumeAPI.DataValidation;
using System.ComponentModel.DataAnnotations;

namespace ConsumeAPI.Models
{
    public class Provimet
    {
        public int ProvimetId { get; set; }

        [Required(ErrorMessage = "Zgjedheni studentin!")]
        [Display(Name = "Studenti")]
        public int StudentiId { get; set; }

        [Required(ErrorMessage = "Zgjedheni lenden!")]
        [Display(Name = "Lenda")]
        public int LendaId { get; set; }

        [Required(ErrorMessage = "Zgjedheni profesorin!")]
        [Display(Name = "Profesori")]
        public int ProfesoriId { get; set; }

        [PiketProvimit]
        [Required(ErrorMessage = "Shkruani piket!")]
        public int Piket { get; set; }

        [NotaStudentit]
        [Required(ErrorMessage = "Shkruani noten!")]
        public int Nota { get; set; }

        public virtual Lendet Lenda { get; set; }
        public virtual Profesoret Profesori { get; set; }
        public virtual Studenti Studenti { get; set; }
    }
}
