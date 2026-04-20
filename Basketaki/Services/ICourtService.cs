using Basketaki.Models;

namespace Basketaki.Services
{
    public interface ICourtService
    {
        Task<List<Court>> GetAllAsync();          // Returns a list of all Courts

        Task<Court?> GetByIdAsync(int id);        // Returns a specific Court by its ID, or null if not found
         
        Task<SimpleResult> CreateAsync(Court court);      // Creates a new Court and returns true if successful, false if not

        Task<SimpleResult> UpdateAsync(Court court);      // Updates an existing Court and returns true if successful, false if not

        Task<SimpleResult> DeleteAsync(int id);           // Deletes a Court by its ID and returns true if successful, false if not 

        
    }
}