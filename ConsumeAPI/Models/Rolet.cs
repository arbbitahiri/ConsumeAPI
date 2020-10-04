using System.ComponentModel.DataAnnotations;

namespace ConsumeAPI.Models
{
    public class Rolet
    {
        public int RoletId { get; set; }

        [Required(ErrorMessage = "Shkruani emrin e rolit!")]
        [Display(Name = "Emri i Rolit")]
        public string RoleName { get; set; }
    }
}
