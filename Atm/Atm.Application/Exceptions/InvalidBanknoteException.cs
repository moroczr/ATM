namespace Atm.Application.Exceptions
{
    using System;

    /// <summary>
    /// Invalid banknote exception.
    /// </summary>
    /// <seealso cref="Atm.Application.Exceptions.StatusCodeRelatedException" />
    public class InvalidBanknoteException : StatusCodeRelatedException
    {
        public InvalidBanknoteException(string message) : base(message)
        {
            this.StatusCode = 400;
        }
        
    }
}