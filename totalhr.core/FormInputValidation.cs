using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using totalhr.Resources;

namespace totalhr.core
{
    public class ValidationResult
    {
        public bool StateIsValid { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class FormInputValidation
    {
        public string ErrorMessage { get; set; }

        public virtual ValidationResult Validate(string input, string inputName)
        {
            return new ValidationResult {StateIsValid = !string.IsNullOrEmpty(input)};
        } 
    }

    public class RequiredFormInputValidator : FormInputValidation
    {
        public override ValidationResult Validate(string input, string inputName)
        {
            return !string.IsNullOrEmpty(input) ? 
                new ValidationResult { StateIsValid = true } 
                : 
                new ValidationResult
                    {
                        StateIsValid = false, 
                        ErrorMessage = string.Format(Error.Error_Validation_Field_X_IsRequired, inputName)
                    };
        }
    }

    public class MinLengthFormInputValidator : FormInputValidation
    {
        public int Minimum { get; set; }

        public MinLengthFormInputValidator(int len)
        {
            Minimum = len;
        }

        public override ValidationResult Validate(string input, string inputName)
        {
            var bEmptyString = string.IsNullOrEmpty(input);

            if (bEmptyString || input.Length < Minimum)
            {
                return new ValidationResult
                {
                    StateIsValid = false,
                    ErrorMessage = string.Format(Error.Error_Validation_Field_X_MinReq, Minimum, inputName)
                };
            }

            return new ValidationResult { StateIsValid = true } ;
        }
    }

    public class MaxLengthFormInputValidator : FormInputValidation
    {
        public int Maximum { get; set; }

        public MaxLengthFormInputValidator(int len)
        {
            Maximum = len;
        }

        public override ValidationResult Validate(string input, string inputName)
        {
            if (input.Length > Maximum)
            {
                return new ValidationResult
                {
                    StateIsValid = false,
                    ErrorMessage = string.Format(Error.Error_Validation_Field_X_MinReq, Maximum, inputName)
                };
            }

            return new ValidationResult { StateIsValid = true };
        }
    }

    public class PatternFormInputValidator : FormInputValidation
    {
        public string Pattern { get; set; }

        public PatternFormInputValidator(string regex)
        {
            this.Pattern = regex;
        }

        public override ValidationResult Validate(string input, string inputName)
        {
            var regex = new Regex(Pattern);
            var match = regex.Match(input);
            if (match.Success)
            {
                return new ValidationResult { StateIsValid = true };
            }
            else
            {
                return new ValidationResult
                {
                    StateIsValid = false,
                    ErrorMessage = string.Format(Error.Error_Validation_Pattern_Not_Matched, inputName)
                };
            }
        }
    }
}
