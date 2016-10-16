using System;

namespace Sarwa.Core.Common.Exceptions
{
    public class AuthorizationValidationException : ApplicationException
    {
        public AuthorizationValidationException(string message)
            : base(message)
        {
        }

        public AuthorizationValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
