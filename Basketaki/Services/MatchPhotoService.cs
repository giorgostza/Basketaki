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
            return await _context.MatchPhotos.AsNoTracking().Where(mp => mp.MatchId == matchId)
                                                            .OrderByDescending(mp => mp.CreatedAt)
                                                            .ToListAsync();

        }


        public async Task<MatchPhoto?> GetByIdAsync(int id)
        {

            return await _context.MatchPhotos.AsNoTracking().Include(mp => mp.Match).FirstOrDefaultAsync(mp => mp.Id == id);

        }


        public async Task<SimpleResult> CreateAsync(MatchPhoto photo)
        {
            if (photo == null)
            {

                return SimpleResult.Fail("Photo data is required.");

            }


            var matchExists = await _context.Matches.AnyAsync(m => m.Id == photo.MatchId);

            if (!matchExists)
            {

                return SimpleResult.Fail("Associated match not found.");

            }


            if (string.IsNullOrWhiteSpace(photo.ImageUrl))
            {

                return SimpleResult.Fail("Photo URL is required.");

            }


            var trimmedImageUrl = photo.ImageUrl.Trim();


            var exists = await _context.MatchPhotos.AnyAsync(mp => mp.MatchId == photo.MatchId && mp.ImageUrl == trimmedImageUrl);

            if (exists)
            {

                return SimpleResult.Fail("Photo with the same URL already exists for this match.");

            }


            photo.ImageUrl = trimmedImageUrl;

            _context.MatchPhotos.Add(photo);


            try
            {

                await _context.SaveChangesAsync();
                return SimpleResult.Ok("Photo created successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to create photo. A photo with the same URL may already exist for this match.");

            }

        }


        public async Task<SimpleResult> DeleteAsync(int id)
        {

            var photo = await _context.MatchPhotos.FindAsync(id);

            if (photo == null)
            {

                return SimpleResult.Fail("Photo not found.");

            }



            try
            {

                _context.MatchPhotos.Remove(photo);
                await _context.SaveChangesAsync();

                return SimpleResult.Ok("Photo deleted successfully.");

            }
            catch (DbUpdateException)
            {

                return SimpleResult.Fail("Unable to delete photo because it is used by other data.");

            }


        }

    }
}