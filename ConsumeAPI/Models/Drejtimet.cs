using System.ComponentModel.DataAnnotations;


namespace ConsumeAPI.Models
{
    public class Drejtimet
    {
        public int DrejtimetId { get; set; }

        [Required(ErrorMessage = "Shkruani emrin e drejtimit!")]
        [Display(Name = "Emri i Drejtimit")]
        public string EmriDrejtimit { get; set; }

        [Required]
        public string Koment { get; set; }
    }
}
