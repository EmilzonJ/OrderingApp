using System;
using System.Net;

namespace Domain.CustomExceptions
{
    public class ApiException : Exception
    {
        public readonly HttpStatusCode Code;
        public readonly object Errors;

        public ApiException(HttpStatusCode code, object errors = null)
        {
            Code = code;
            Errors = errors;
        }
    }
}