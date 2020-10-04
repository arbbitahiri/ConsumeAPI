using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ConsumeAPI.DataValidation
{
    public class StudentIndex : ValidationAttribute
    {
        public int MaxValue { get; set; }

        public StudentIndex() : base("{0} eshte gabim. Shkruani ne formen 123456 dhe nje shkronje ne fund.")
        {
            MaxValue = 7;
        }

        public override bool IsValid(object value)
        {
            string stringValue = value as string;

            if (!string.IsNullOrEmpty(stringValue))
            {
                int stringLength = stringValue.Length;
                if (stringLength == MaxValue)
                {
                    bool strHasInt = stringValue.All(char.IsLetterOrDigit);
                    if (strHasInt)
                    {
                        for (int i = 0; i < stringLength - 1; i++)
                        {
                            if (char.IsDigit(stringValue[i]))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }

                        for (int i = stringLength - 1; i <= stringLength; i++)
                        {
                            if (char.IsLetter(stringValue[i]))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return false;
        }
    }
}
