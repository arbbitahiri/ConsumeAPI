using System;
using System.ComponentModel.DataAnnotations;

namespace ConsumeAPI.Models
{
    public class Users
    {
        public int UsersId { get; set; }

        [Required(ErrorMessage = "Shkruani username!")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Shkruani fjalekalimin!")]
        public string Fjalekalimi { get; set; }

        [Required(ErrorMessage = "Zgjedheni rolin!")]
        [Display(Name = "Roli")]
        public int RoleId { get; set; }
        public DateTime LastLoginDate { get; set; }

        public virtual Rolet Role { get; set; }
    }
}
