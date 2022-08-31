namespace Atm.Application.Interfaces
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for data storage related operations.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Adds an entity to the datasource.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The added item.</returns>
        T Add(T item);

        void AddRange(IEnumerable<T> items);

        /// <summary>
        /// Reads entities from the datasource.
        /// </summary>
        /// <returns>List of entities.</returns>
        IQueryable<T> Read();

        /// <summary>
        /// Updates the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The updated item.</returns>
        T Update(T item);

        /// <summary>
        /// Deletes the specified item from the datasource.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The deleted item.</returns>
        T Delete(T item);

        /// <summary>
        /// Deletes multiple items from the datasource.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>The deleted items.</returns>
        IEnumerable<T> DeleteRange(IEnumerable<T> items);

        Task<List<T>> ReadAsListAsync();
    }
}