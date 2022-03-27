using AspNetMVC_Inlamningsuppgift_2.Data;
using AspNetMVC_Inlamningsuppgift_2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspNetMVC_Inlamningsuppgift_2.Managers
{
    public interface IUserProfileManager
    {
        Task<UserProfileResult> CreateAsync(IdentityUser user, UserProfileEntity model);
        Task<UserProfileModel> ReadModelAsync(string userId);
        Task<string> DisplayNameAsync(string userId);

    }
    public class UserProfileManager : IUserProfileManager
    {
        private readonly AppDbContext _context;
        public UserProfileManager(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserProfileResult> CreateAsync(IdentityUser user, UserProfileEntity model)
        {
            
            if (await _context.Users.AnyAsync(x => x.Id == user.Id))
            {
                var UserProfileEntity = new UserProfileEntity
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Street = model.Street,
                    Zipcode = model.Zipcode,
                    City = model.City,
                    Country = model.Country,
                    UserId = user.Id
                };

                _context.UserProfiles.Add(UserProfileEntity);
                await _context.SaveChangesAsync();

                return new UserProfileResult() { Succeded = true };
            }
            else { return new UserProfileResult() { Succeded = false };}
        }

        public async Task<UserProfileModel> ReadModelAsync(string userId)
        {
            var model = new UserProfileModel();
            var _userprofile = await _context.UserProfiles.Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == userId);

            if (_userprofile != null)
            {
                model.FirstName = _userprofile.FirstName;
                model.LastName = _userprofile.LastName;
                model.Email = _userprofile.User.Email;
                model.Street = _userprofile.Street;
                model.Zipcode = _userprofile.Zipcode;
                model.City = _userprofile.City;
                model.Country = _userprofile.Country;
            }

            return model;
        }

        public async Task<string> DisplayNameAsync(string userId)
        {
            var _result = await ReadModelAsync(userId);
            return $"{_result.FirstName} {_result.LastName}";
        }
    }
        public class UserProfileResult
        {
            public bool Succeded { get; set; } = false;
        }
}
