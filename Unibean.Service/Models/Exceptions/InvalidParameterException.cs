using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Unibean.Service.Models.Exceptions;

[Serializable]
public class InvalidParameterException : Exception
{
    public ModelStateDictionary ModelState { get; }

    public InvalidParameterException() { }

    public InvalidParameterException(string message)
        : base(String.Format(message))
    {
    }

    public InvalidParameterException(ModelStateDictionary ModelState)
    {
        this.ModelState = ModelState;
    }
}
