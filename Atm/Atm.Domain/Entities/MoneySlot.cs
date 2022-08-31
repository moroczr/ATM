namespace Atm.Domain.Entities
{
    /// <summary>
    /// Representation of the money slot storage model.
    /// </summary>
    public class MoneySlot
    {
        public virtual int Id { get; set; }
        public virtual int BankNote { get; set; }
        public virtual int Qty { get; set; }
    }
}