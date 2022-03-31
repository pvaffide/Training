using Training.Resources;
using System.ComponentModel.DataAnnotations;

namespace Training.Helpers
{
    public class RequiredAAttribute : RequiredAttribute
    {
        public RequiredAAttribute()
        {
            ErrorMessageResourceName = "RequiredAttribute";
            ErrorMessageResourceType = typeof(ValidationResources);
        }
    }

    public class StringLengthAAttribute : StringLengthAttribute
    {
        public StringLengthAAttribute(int maximumLenght) : base(maximumLenght)
        {
            ErrorMessageResourceName = "StringLengthAttribute";
            ErrorMessageResourceType = typeof(ValidationResources);
        }
    }
}
