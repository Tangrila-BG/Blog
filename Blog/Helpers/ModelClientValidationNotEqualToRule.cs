using System.Web.Mvc;

namespace Blog.Helpers
{
    public class ModelClientValidationNotEqualToRule : ModelClientValidationRule
    {
        public ModelClientValidationNotEqualToRule(string errorMessage, string otherProperties)
        {
            ErrorMessage = errorMessage;
            ValidationType = "notequalto";
            ValidationParameters.Add("otherproperties", otherProperties);
        }
    }
}
