namespace Atm.Application.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Amount can't be covered exception.
    /// </summary>
    /// <seealso cref="Atm.Application.Exceptions.StatusCodeRelatedException" />
    public class AmountCantBeCoveredException : StatusCodeRelatedException
    {
        public AmountCantBeCoveredException(string message) : base(message)
        {
            this.StatusCode = 503;
        }
    }
}
