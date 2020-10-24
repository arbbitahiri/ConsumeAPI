using System.ComponentModel.DataAnnotations;

namespace ConsumeAPI.DataValidation
{
    public class PiketProvimit : ValidationAttribute
    {
        public int MaxLength { get; set; }

        public PiketProvimit() : base("{0} jane shtypur gabim. Shkruani piket perseri!")
        {
            MaxLength = 3;
        }

        public override bool IsValid(object value)
        {
            string val = value.ToString();
            int valueLength = val.Length;

            int intValue = (int)(value);

            if (valueLength <= MaxLength)
            {
                if (intValue >= 101 || intValue < 0)
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
