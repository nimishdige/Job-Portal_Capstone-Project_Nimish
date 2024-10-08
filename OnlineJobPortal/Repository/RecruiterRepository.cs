﻿using Microsoft.EntityFrameworkCore;
using OnlineJobPortal.Data;
using OnlineJobPortal.Models;
namespace OnlineJobPortal.Repository
{
    public class RecruiterRepository :IRecruiterRepository
    {
        private readonly ApplicationDbContext _context;

        public RecruiterRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //public RecruiterRepository() { }

        public async Task<List<Recruiter>> GetAllRecruitersAsync()
        {
            var records = await _context.Recruiters.Select(u=> new Recruiter()
            {
                RID = u.RID,
                Name = u.Name,
                CityID = u.CityID,
                Email = u.Email,
                Contact = u.Contact,
            }).ToListAsync();
            return records;
        }

        public async Task<string> CheckEmailExistsAsync(string email)
        {
            // Validate input parameter
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException("Email must be provided.");
            }

            // Query the database to find the recruiter with the provided email and return the name if found
            var recruiterName = await _context.Recruiters
                                               .Where(r => r.Email == email)
                                               .Select(r => r.Name) // Assuming 'Name' is the property for the recruiter's name
                                               .FirstOrDefaultAsync();

            return recruiterName; // Will return null if no recruiter is found
        }



        public async Task<Recruiter> GetRecruiterByIdAsync(int RID)
        {
            var records = _context.Recruiters.Where(u => u.RID == RID).Select(u => new Recruiter()
            {
                RID = u.RID,
                Name = u.Name,
                CityID = u.CityID,
                Email = u.Email,
                Contact = u.Contact,
            }).FirstOrDefault();
            return records;
        }

        public async Task<int> AddRecruiterAsync(Recruiter recruiter)
        {
            var newrecruiter = new Recruiter()
            {
                //RID = recruiter.RID,
                Name = recruiter.Name,
                CityID = recruiter.CityID,
                Email = recruiter.Email,
                Contact = recruiter.Contact,
            };
            _context.Recruiters.Add(newrecruiter);
            await _context.SaveChangesAsync();

            return newrecruiter.RID;
        }

        public async Task UpdateRecruiterByIDAsync(int RID, Recruiter recruiter)
        {
            var updatedRecruiter = await _context.Recruiters.FindAsync(RID);

            if(updatedRecruiter != null)
            {
                updatedRecruiter.Name = recruiter.Name;
                updatedRecruiter.Email = recruiter.Email;
                updatedRecruiter.CityID = recruiter.CityID;
                updatedRecruiter.Contact = recruiter.Contact;
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteRecruiterByIDAsync(int RID)
        {
            var deletedRecruiter = new Recruiter()
            {
                RID = RID,
            };

            _context.Recruiters.Remove(deletedRecruiter);
            await _context.SaveChangesAsync();
        }
    }
}
