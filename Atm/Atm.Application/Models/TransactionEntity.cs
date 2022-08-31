namespace Atm.Application.Models
{
    /// <summary>
    /// Describes the details of the transaction.
    /// </summary>
    public class TransactionEntity
    {
        /// <summary>
        /// Gets or sets the bank note.
        /// </summary>
        /// <value>
        /// The bank note.
        /// </value>
        public int BankNote { get; set; }

        /// <summary>
        /// Gets or sets the qty.
        /// </summary>
        /// <value>
        /// The qty.
        /// </value>
        public int Qty { get; set; }
    }
}