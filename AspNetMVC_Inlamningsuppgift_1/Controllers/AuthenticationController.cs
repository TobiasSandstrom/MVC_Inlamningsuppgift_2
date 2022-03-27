using AspNetMVC_Inlamningsuppgift_2.Managers;
using AspNetMVC_Inlamningsuppgift_2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetMVC_Inlamningsuppgift_2.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserProfileManager _userprofileManager;

        public AuthenticationController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IUserProfileManager userprofileManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _userprofileManager = userprofileManager;
        }



        // Registrera

        [Route("register")]
        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {

            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new RegisterModel();

            if (returnUrl != null)
                model.ReturnUrl = returnUrl;

            return View(model);
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                if (!await _roleManager.Roles.AnyAsync())
                {
                    await _roleManager.CreateAsync(new IdentityRole("admin"));
                    await _roleManager.CreateAsync(new IdentityRole("user"));
                }

                if (!await _userManager.Users.AnyAsync())
                {
                    model.RoleName = "admin";
                }


                var _user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == model.Email);
                
                // Kontrollerar så det inte finns en användare med samma email registrerad
                if (_user == null)
                {
                    var _identityUser = new IdentityUser()
                    {
                        UserName = model.Email,
                        Email = model.Email
                    };

                    var res = await _userManager.CreateAsync(_identityUser, model.Password);

                    if (res.Succeeded)
                    {

                        await _userManager.AddToRoleAsync(_identityUser, model.RoleName);

                        var _userProfile = new UserProfileEntity
                        {
                            FirstName = model.FirstName,
                            LastName = model.LastName,
                            Street = model.Street,
                            Zipcode = model.Zipcode,
                            City = model.City,
                            Country = model.Country
                        };

                        var _profileResult = await _userprofileManager.CreateAsync(_identityUser, _userProfile);

                        if (_profileResult.Succeded)
                        {
                            await _signInManager.SignInAsync(_identityUser, isPersistent: false);

                            if (model.ReturnUrl == null || model.ReturnUrl == "/")
                            { return RedirectToAction("Index", "Home"); }
                            else
                            { return LocalRedirect(model.ReturnUrl); }
                        }

                    }
                    else
                    { model.ErrorMessage = "An error accured while trying to create your account, please log in to your account and complete the registration";   }
                    
                }
                else
                { model.ErrorMessage = "User with that email already exists"; }
            }

            return View(model);
        }

        // ------------------------------------------------------------------------------------------------------------------------------------


        //Logga in

        [Route("Signin")]
        [HttpGet]
        public IActionResult SignIn(string returnUrl = null)
        {

            if (_signInManager.IsSignedIn(User))
            { return RedirectToAction("Index", "Home"); }

            var model = new SignInModel();

            if (returnUrl != null)
            { model.ReturnUrl = returnUrl; }

            return View(model);
        }

        [Route("Signin")]
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInModel model)
        {

            if (ModelState.IsValid)
            {
                var res = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, false);

                if (res.Succeeded)
                {
                    if (model.ReturnUrl == null || model.ReturnUrl == "/")
                    { return RedirectToAction("Index", "Home"); }
                    else
                    { return LocalRedirect(model.ReturnUrl); }

                }
            }

            model.ErrorMessage = "Password or Email was not correct";
            return View(model);
        }

        // ------------------------------------------------------------------------------------------------------------------------------------


        //Logga ut

        [Route("Signout")]
        [HttpGet]
        public async Task<IActionResult> SignOut()
        {
            if (_signInManager.IsSignedIn(User))
            { await _signInManager.SignOutAsync(); }
            return RedirectToAction("Index", "Home");
        }
    }
}
