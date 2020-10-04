using ConsumeAPI.DataValidation;
using System;
using System.ComponentModel.DataAnnotations;

namespace ConsumeAPI.Models
{
    public class Studenti
    {
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Shkruani emrin!")]
        public string Emri { get; set; }

        [Required(ErrorMessage = "Shkruani mbiemrin!")]
        public string Mbiemri { get; set; }

        [Required(ErrorMessage = "Zgjedheni daten e lindjes!")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMMM yyyy}")]
        [Display(Name = "Ditelindja")]
        public DateTime DataLindjes { get; set; }

        [Required(ErrorMessage = "Shkruani indeksin!")]
        [StudentIndex]
        public string Indeksi { get; set; }

        [Required(ErrorMessage = "Zgjedheni drejtimin!")]
        [Display(Name = "Drejtimi")]
        public int DrejtimiId { get; set; }

        [Required(ErrorMessage = "Zgjedheni statusin!")]
        [Display(Name = "Statusi")]
        public int StatusiId { get; set; }

        public virtual Drejtimet Drejtimi { get; set; }
        public virtual Statuset Statusi { get; set; }

        [Display(Name = "Emri i Studentit")]
        public string FullName
        {
            get
            {
                return Emri + " " + Mbiemri;
            }
        }
    }
}
