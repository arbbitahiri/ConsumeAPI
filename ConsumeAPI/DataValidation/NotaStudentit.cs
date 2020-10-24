using System.ComponentModel.DataAnnotations;

namespace ConsumeAPI.DataValidation
{
    public class NotaStudentit : ValidationAttribute
    {
        public int MaxLength { get; set; }

        public NotaStudentit() : base ("{0} eshte shtypur gabim. Shkruani noten perseri!")
        {
            MaxLength = 2;
        }

        public override bool IsValid(object value)
        {
            string val = value.ToString();
            int valueLength = val.Length;

            int intValue = (int)(value);

            if (valueLength <= MaxLength)
            {
                if (intValue >= 11 || intValue <= 4)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
