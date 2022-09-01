namespace Atm.Application.Exceptions
{
    using System;

    /// <summary>
    /// Status code related exception.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class StatusCodeRelatedException : Exception
    {
        public StatusCodeRelatedException(string message) : base(message)
        {
        }

        public int StatusCode { get; set; }
    }
}