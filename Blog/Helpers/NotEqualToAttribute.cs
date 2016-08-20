using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Blog.Helpers
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class NotEqualToAttribute : ValidationAttribute, IClientValidatable
    {
        private const string DefaultErrorMessage = "{0} cannot be the same as {1}.";

        public string OtherProperties { get; private set; }

        public NotEqualToAttribute(string otherProperties)
            : base(DefaultErrorMessage)
        {
            if (string.IsNullOrEmpty(otherProperties))
            {
                throw new ArgumentNullException("otherProperties");
            }

            OtherProperties = otherProperties.Replace(" ", "");
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(ErrorMessageString, name, OtherProperties.Replace(",", " or "));
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var otherPropertiesNames = OtherProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var otherPropertyName in otherPropertiesNames)
                {
                    var otherProperty = validationContext.ObjectInstance.GetType().GetProperty(otherPropertyName);

                    var otherPropertyValue = otherProperty.GetValue(validationContext.ObjectInstance, null);

                    if (value.Equals(otherPropertyValue))
                    {
                        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                    }
                }
            }
            return ValidationResult.Success;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            return new[] { new ModelClientValidationNotEqualToRule(FormatErrorMessage(metadata.GetDisplayName()), OtherProperties) };
        }
    }
}