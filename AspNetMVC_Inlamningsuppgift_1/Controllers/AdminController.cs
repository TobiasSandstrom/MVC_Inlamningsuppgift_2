using AspNetMVC_Inlamningsuppgift_1.Models;
using AspNetMVC_Inlamningsuppgift_2.Data;
using AspNetMVC_Inlamningsuppgift_2.Managers;
using AspNetMVC_Inlamningsuppgift_2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetMVC_Inlamningsuppgift_1.Controllers
{
    public class AdminController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserProfileManager _userprofileManager;
        private readonly AppDbContext _context;

        public AdminController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IUserProfileManager userprofileManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userprofileManager = userprofileManager;
            _context = context;
        }

        [HttpGet]
        [Route("admin")]
        public IActionResult Index()
        {
            if (User.IsInRole("admin"))
            {
                return View();
            }
            
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        [Route("editusers")]
        public async Task<IActionResult> Users()
        {

            if (User.IsInRole("admin"))
            {
                var users = await _context.UserProfiles.Include(x => x.User).ToListAsync();
                var _users = new List<UserProfileAdminViewModel>();
                
                foreach (var i in users)
                {
                    var u = new UserProfileAdminViewModel();
                    var roleId = await _context.UserRoles.FirstOrDefaultAsync(x => x.UserId == i.UserId);
                    var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == roleId.RoleId);

                    u.Id = i.UserId;
                    u.FirstName = i.FirstName;
                    u.LastName = i.LastName;
                    u.Email = i.User.Email;
                    u.Street = i.Street;
                    u.City = i.City;
                    u.Zipcode = i.Zipcode;
                    u.Country = i.Country;
                    u.ProfilePicture = "";
                    u.PageRole = role.Name;

                    _users.Add(u);
                }
                return View(_users);
            }
            return RedirectToAction("Index", "Home");
        }


        [HttpGet("{id}")]
        [Route("delete")]
        public async Task<IActionResult> Delete(string id)
        {
            if (User.IsInRole("admin"))
            {
                var response = new AdminResponse();
                if (User.FindFirst("UserId").Value != id)
                {
                    var profileUser = await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == id);
                    if (profileUser != null)
                    {
                        _context.UserProfiles.Remove(profileUser);

                        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
                        if (user != null)
                        {
                            _context.Users.Remove(user);

                            await _context.SaveChangesAsync();
                            response.Response = "User Deleted";
                            return View(response);
                        }

                        response.Response = "User could not be deleted";
                        return View(response);
                    }

                    response.Response = "User could not be deleted";
                    return View(response);
                }

                response.Response = "Cant remove your own account";
                return View(response);

            }

            return RedirectToAction("Index", "Home");
        }


        [HttpGet("{id}")]
        [Route("edit")]
        public async Task<IActionResult> Edit(string id)
        {
            var response = new AdminResponse();
            var profileView = new AdminEditViewModel();

            if (User.IsInRole("admin"))
            {
                    
                var profileUser = await _context.UserProfiles.Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == id);
                if (profileUser != null)
                {
                    profileView.Id = profileUser.UserId;
                    profileView.FirstName = profileUser.FirstName;
                    profileView.LastName = profileUser.LastName;
                    profileView.Email = profileUser.User.Email;
                    profileView.Street = profileUser.Street;
                    profileView.Zipcode = profileUser.Zipcode;
                    profileView.Country = profileUser.Country;
                    profileView.City = profileUser.City;


                    var roleId = await _context.UserRoles.FirstOrDefaultAsync(x => x.UserId == profileUser.UserId);
                    var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == roleId.RoleId);


                    return View(profileView);
                }

                response.Response = "Something went wrong";
                return View(profileView);
            }


            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> Edit(AdminEditViewModel model)
        {

            if (User.IsInRole("admin"))
            {

                if (ModelState.IsValid)
                {


                    // Kollar så ingen annan har emailen användaren angiver
                    var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == model.Id);
                    var emailCheck = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);
                    if (emailCheck != null && emailCheck.Id != user.Id)
                    {
                        model.Response = "User with that email already exists";
                        return View(model);
                    }

                    // Ändrar User
                    if (user.Email != model.Email)
                    {
                        user.Email = model.Email;
                        user.NormalizedEmail = model.Email.ToUpper();
                        user.UserName = model.Email;
                        user.NormalizedUserName = model.Email.ToUpper();
                        _context.Users.Update(user);
                    }




                    var profileUser = await _context.UserProfiles.FirstOrDefaultAsync(x => x.UserId == model.Id);
                    if (profileUser != null)
                    {
                        // Ändrar ProfileUser
                        profileUser.FirstName = model.FirstName;
                        profileUser.LastName = model.LastName;
                        profileUser.Street = model.Street;
                        profileUser.Zipcode = model.Zipcode;
                        profileUser.Country = model.Country;
                        profileUser.City = model.City;


                        _context.UserProfiles.Update(profileUser);

                    }


                    try
                    {
                        await _context.SaveChangesAsync();
                        model.Response = "Saved successfully";
                        return View(model);
                    }
                    catch (Exception)
                    {
                        model.Response = "Something went wrong";
                        return View(model);

                    }


                }

                model.Response = "Form is not correct";
                return View(model);

            }
                
            return RedirectToAction("Index", "Home");

                
        }

    }
}
