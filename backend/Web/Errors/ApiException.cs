using System;
using System.Net;

namespace Web.Errors
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