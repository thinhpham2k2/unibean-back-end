﻿using System.ComponentModel.DataAnnotations;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Unibean.Service.Validations;

public class ValidVerificationCode : ValidationAttribute
{
    public string OtherProperty { get; }

    public ValidVerificationCode(string otherProperty)
    {
        OtherProperty = otherProperty;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var property = validationContext.ObjectType.GetProperty(OtherProperty);
        if (property == null)
            return new ValidationResult(OtherProperty + " is an invalid property of the model");

        string verifyCode = value.ToString();
        string hashCode = property.GetValue(validationContext.ObjectInstance).ToString();

        try
        {
            if (BCryptNet.Verify(verifyCode, hashCode))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Invalid verification code");
        }
        catch
        {
            return new ValidationResult("Invalid verification code");
        }
    }
}
