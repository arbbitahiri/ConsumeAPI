using System.ComponentModel.DataAnnotations;

namespace ConsumeAPI.Models
{
    public class Lendet
    {
        public int LendetId { get; set; }

        [Required(ErrorMessage = "Shkruani emrin e lendes!")]
        [Display(Name = "Emri i Lendes")]
        public string EmriLendes { get; set; }

        [Required(ErrorMessage = "Shkruani semestrin!")]
        public int Semestri { get; set; }

        [Required(ErrorMessage = "Zgjedheni drejtimin!")]
        [Display(Name = "Drejtimi")]
        public int DrejtimiId { get; set; }

        public virtual Drejtimet Drejtimi { get; set; }
    }
}
