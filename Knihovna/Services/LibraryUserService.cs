using Knihovna.DTO;
using Knihovna.Models;
using Knihovna.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Knihovna.Services
{
    public class LibraryUserService
    {
        private ApplicationDbContext _dbContext;
        private UserManager<AppUser> _userManager;

        public LibraryUserService(ApplicationDbContext dbContext, UserManager<AppUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        //*******************************
        //********* CREATE   ************
        //*******************************
        public async Task CreateAsync(LibraryUser libraryUser)
        {
            await _dbContext.LibraryUsers.AddAsync(libraryUser);
            await _dbContext.SaveChangesAsync();
        }
        //*******************************
        //********* READ  ************
        //*******************************
        public async Task<IEnumerable<UserVM>> GetAllAsync()
        {
            var allLibraryUsers = await _dbContext.LibraryUsers.Include(x => x.AppUser).ToListAsync();

            var result = new List<UserVM>();
            foreach (LibraryUser libraryUser in allLibraryUsers)
            {
                if (libraryUser.AppUser != null)
                {
                    AppUser? user = await _userManager.FindByIdAsync(libraryUser.AppUser.Id);
                    if (user != null)
                    {
                        var roleNames = await _userManager.GetRolesAsync(user);
                        string roles = string.Join(", ", roleNames);
                        result.Add(libraryUserToUserVM(libraryUser, roles));
                    }
                }
            }
            return result;
        }
        //*******************************
        //********* UPDATE  ************
        //*******************************
        public async Task<LibraryUser> EditAsync(LibraryUser libraryUser)
        {
            _dbContext.Update(libraryUser);
            await _dbContext.SaveChangesAsync();
            return libraryUser;
        }
        //*******************************
        //********* DELETE  ************
        //*******************************
        public async Task<string> DeleteAsync(int id)
        {
            var libraryUserToDelete = await _dbContext.LibraryUsers.Include(x => x.AppUser).FirstOrDefaultAsync(x => x.Id == id);
            if (libraryUserToDelete?.AppUser != null)
            {
                string appUserId = libraryUserToDelete.AppUser.Id;
                _dbContext.Remove(libraryUserToDelete);
                await _dbContext.SaveChangesAsync();
                return appUserId;
            }
            return "";
        }
        //*******************************
        //********* GET BY ID  ************
        //*******************************
        public async Task<LibraryUser> GetByIdAsync(int id)
        {
            var libraryUserToDelete = await _dbContext.LibraryUsers.FirstOrDefaultAsync(x => x.Id == id);
            if (libraryUserToDelete != null)
            {
                return libraryUserToDelete;
            }
            return new LibraryUser();
        }
        //*******************************
        //********* LIBRARY USER TO USERVM  ************
        //*******************************
        public UserVM libraryUserToUserVM(LibraryUser libraryUser, string roleNames)
        {

            return new UserVM()
            {
                Id = libraryUser.Id,
                AppUserId =   libraryUser?.AppUser?.Id ?? default,
                UserName = libraryUser?.AppUser?.UserName ?? "",
                Email = libraryUser?.AppUser?.Email ?? "",
                FirstName = libraryUser?.FirstName ?? "",
                LastName = libraryUser?.LastName ?? "",
                DateOfBirth = libraryUser?.DateOfBirth ?? default,
                RoleNames = roleNames,
                Password = ""
            };

        }
    }
}
