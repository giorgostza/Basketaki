using Basketaki.Models;

namespace Basketaki.Services
{
    public interface ICourtService
    {
        Task<List<Court>> GetAllAsync();          // Returns a list of all Courts

        Task<Court?> GetByIdAsync(int id);        // Returns a specific Court by its ID, or null if not found
         
        Task<bool> CreateAsync(Court court);      // Creates a new Court and returns true if successful, false if not

        Task<bool> UpdateAsync(Court court);      // Updates an existing Court and returns true if successful, false if not

        Task<bool> DeleteAsync(int id);           // Deletes a Court by its ID and returns true if successful, false if not 

        Task<bool> ExistsAsync(int id);  // Checks if a Court with the specified ID exists and returns true if it does, false otherwise
    }
}