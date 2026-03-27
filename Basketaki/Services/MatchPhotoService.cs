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

            return await _context.MatchPhotos.AsNoTracking().Where(mp => mp.MatchId == matchId).ToListAsync();  // Get Photos for a specific Match

        }

        public async Task<MatchPhoto?> GetByIdAsync(int id)
        {

            return await _context.MatchPhotos.AsNoTracking().FirstOrDefaultAsync(mp => mp.Id == id);  // Get a specific Photo by its ID

        }

        public async Task<SimpleResult> CreateAsync(MatchPhoto photo)
        {

            if (!await _context.Matches.AnyAsync(m => m.Id == photo.MatchId))
            {

                return new SimpleResult { Success = false, Message = "Associated match not found" };

            }

            if (string.IsNullOrWhiteSpace(photo.Url))
            {

                return new SimpleResult { Success = false, Message = "Photo URL is required" };

            }


            var normalizedUrl = photo.Url.Trim();
            var exists = await _context.MatchPhotos.AnyAsync(mp => mp.MatchId == photo.MatchId && mp.Url == normalizedUrl);



            if (exists)  
            {

                return new SimpleResult { Success = false, Message = "Photo with same URL already exists for this match" };
            }

            photo.Url = normalizedUrl;

            _context.MatchPhotos.Add(photo);
            await _context.SaveChangesAsync();

            return new SimpleResult { Success = true };

        }

         
        public async Task<SimpleResult> DeleteAsync(int id)
        {
            var photo = await _context.MatchPhotos.FindAsync(id);  // Find the Photo by its ID

            if (photo == null)
            {

                return new SimpleResult { Success = false, Message = "Photo not found" };

            }

            _context.MatchPhotos.Remove(photo);
            await _context.SaveChangesAsync();

            return new SimpleResult { Success = true };
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.MatchPhotos.AnyAsync(mp => mp.Id == id);  // Check if a Photo with the specified ID exists in the database
        }
    }
}
