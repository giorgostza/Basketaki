using Basketaki.Data;
using Basketaki.Models;
using Microsoft.EntityFrameworkCore;

namespace Basketaki.Services
{
    public class MatchPhotoService : IMatchPhotoService
    {
        private readonly ApplicationDbContext _context;

        public MatchPhotoService(ApplicationDbContext context)
        {

            _context = context;

        }

        public async Task<List<MatchPhoto>> GetByMatchAsync(int matchId)
        {

            return await _context.MatchPhotos.Where(mp => mp.MatchId == matchId).ToListAsync();  // Get Photos for a specific Match

        }

        public async Task<MatchPhoto?> GetByIdAsync(int id)
        {

            return await _context.MatchPhotos.FirstOrDefaultAsync(mp => mp.Id == id);  // Get a specific Photo by its ID

        }

        public async Task<bool> CreateAsync(MatchPhoto photo)
        {
            bool matchExists = await _context.Matches.AnyAsync(m => m.Id == photo.MatchId);

            if (!matchExists)  // Ensure the Match exist before adding a Photo to it
            {

                return false;

            }

            if (string.IsNullOrWhiteSpace(photo.Url)) // Ensure the URL is not empty or whitespace
            {

                return false;

            }

            
            var exists = await _context.MatchPhotos.AnyAsync(mp => mp.MatchId == photo.MatchId && mp.Url == photo.Url);
            // Prevent adding duplicate photos for the same Match (same URL)

            if (exists)
            {

                return false;

            }

            _context.MatchPhotos.Add(photo);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var photo = await _context.MatchPhotos.FindAsync(id);  // Find the Photo by its ID

            if (photo == null)
            {

                return false;

            }

            _context.MatchPhotos.Remove(photo);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.MatchPhotos.AnyAsync(mp => mp.Id == id);  // Check if a Photo with the specified ID exists in the database
        }
    }
}
