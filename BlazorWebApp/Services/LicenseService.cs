using BlazorWebApp.Data;
using BlazorWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorWebApp.Services
{
    public class LicenseService
    {
        private readonly ApplicationDbContext _dbContext;

        public LicenseService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task UpdateExpiredLicensesAsync()
        {
            var expiredLicenses = await _dbContext.Licenses
                .Where(l => l.ExpiryDate < DateTime.Now && l.Status == "Active")
                .ToListAsync();

            foreach (var license in expiredLicenses)
            {
                license.Status = "Expired";
            }

            if (expiredLicenses.Any())
            {
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}