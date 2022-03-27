using AspNetMVC_Inlamningsuppgift_2.Data;
using AspNetMVC_Inlamningsuppgift_2.Entities;
using AspNetMVC_Inlamningsuppgift_2.Managers;
using AspNetMVC_Inlamningsuppgift_2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace AspNetMVC_Inlamningsuppgift_2.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IUserProfileManager _userProfileManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _host;


        public ProfileController(IUserProfileManager userProfileManager, SignInManager<IdentityUser> signInManager, AppDbContext context, IWebHostEnvironment host)
        {
            _userProfileManager = userProfileManager;
            _signInManager = signInManager;
            _context = context;
            _host = host;
        }

        [HttpGet("{id}")]
        [Route("profile/{id}")]
        public async Task<IActionResult> Index(string id)
        {


            if (_signInManager.IsSignedIn(User))
            {

                var userID = User.FindFirst("UserId").Value;
                var entity = await _context.UserProfiles.Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == userID);

                var model = new UserProfileViewModel();


                model.FirstName = entity.FirstName;
                model.LastName = entity.LastName;
                model.Email = entity.User.Email;
                model.Street = entity.Street;
                model.City = entity.City;
                model.Zipcode = entity.Zipcode;
                model.Country = entity.Country;


                //Hämtar användarrollen
                var roleId = await _context.UserRoles.FirstOrDefaultAsync(x => x.UserId == userID);
                var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == roleId.RoleId);

                if (role != null)
                    model.PageRole = role.Name;
                else
                    model.PageRole = "";

                //Hämtar profilbilden

                var picture = await _context.ProfilePictures.FirstOrDefaultAsync(x => x.UserId == userID);
                if (picture != null)
                {
                    model.ProfilePicture = picture.Picture;
                }

                else
                    model.ProfilePicture = "";

                return View(model);

            }

            return RedirectToAction("signin", "Authentication");

        }
        
        [HttpGet]
        [Route("Upload")]
        public IActionResult UploadPicture()
        {
            if (_signInManager.IsSignedIn(User))
            {
                var model = new AssignProfilePictureModel();
                return View(model);

            }
            return RedirectToAction("signin", "Authentication");
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> UploadPicture(AssignProfilePictureModel model)
        {

            if (_signInManager.IsSignedIn(User))
            {
                var _userID = User.FindFirst("UserId").ToString();
                var userID = _userID.Split(' ')[1];

                if (ModelState.IsValid)
                {
                    var pictureEntity = new ProfilePictureEntity();

                    pictureEntity.Picture = $"{userID}_{Path.GetFileName(model.FormFile.FileName)}";

                    if (userID != null)
                    {
                        pictureEntity.UserId = userID.ToString();
                        string picturePath = Path.Combine($"{_host.WebRootPath}/images/1_Profile_images", pictureEntity.Picture);


                        // Tar bort den gamla profilbilden ifall det finns någon
                        if (pictureEntity.UserId != null || pictureEntity.Picture != null)
                        {
                            var oldPictureEntity = await _context.ProfilePictures.FirstOrDefaultAsync(x => x.UserId == userID);
                            if (oldPictureEntity != null)
                            {
                                var oldPicturePath = Path.Combine($"{_host.WebRootPath}/images/1_Profile_images/", oldPictureEntity.Picture);

                                if (System.IO.File.Exists(oldPicturePath))
                                    System.IO.File.Delete(oldPicturePath);

                                _context.ProfilePictures.Remove(oldPictureEntity);
                                await _context.SaveChangesAsync();
                            }
                        }

                        using (var fs = new FileStream(picturePath, FileMode.Create))
                        {
                            await model.FormFile.CopyToAsync(fs);
                        }

                        _context.ProfilePictures.Add(pictureEntity);
                        await _context.SaveChangesAsync();
                        model.SuccessMessage = "Upladdning lyckades";
                        return View(model);

                    }

                    model.ErrorMessage = "Cant find User id, try again or contact an admin for help";
                    return View(model);

                }

                model.ErrorMessage = "Something went wrong when trying to upload your picture, please try agaín";
                return View(model);
            }


            return RedirectToAction("signin", "Authentication");

        }



        //Redigera profilinformationen
        [HttpGet("{id}")]
        [Route("profile/edit/{id}")]
        public async Task<IActionResult> EditProfile()
        {

            if (_signInManager.IsSignedIn(User))
            {
                var id = User.FindFirst("UserId").Value;
                var userProfile = await _context.UserProfiles.Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == id);
                var model = new UserProfileModel();

                if (userProfile != null)
                {
                    model.FirstName = userProfile.FirstName;
                    model.LastName = userProfile.LastName;
                    model.Email = userProfile.User.Email;
                    model.Street = userProfile.Street;
                    model.Zipcode = userProfile.Zipcode;
                    model.City = userProfile.City;
                    model.Country = userProfile.Country;
                    return View(model);
                }

                model.ErrorMessage = "Something went wrong, please try again or contact admin";
                return View(model);
            }
            
            return RedirectToAction("signin", "Authentication");
        }

        [HttpPost]
        [Route("profile/edit/{id}")]
        public async Task<IActionResult> EditProfile(UserProfileModel model)
        {

            if (_signInManager.IsSignedIn(User))
            {
                if (ModelState.IsValid)
                {

                    // Kollar så ingen annan har emailen användaren angiver
                    var id = User.FindFirst("UserId").Value;
                    var emailCheck = await _context.Users.FirstOrDefaultAsync(x => x.Email == model.Email);
                    if (emailCheck != null && emailCheck.Id != id)
                    {
                        model.ErrorMessage = "User with that email already exists";
                        return View(model);
                    }

                    //Letar upp userprofilen, sätter de nya värdena och sparar
                    var userProfile = await _context.UserProfiles.Include(x => x.User).FirstOrDefaultAsync(x => x.UserId == id);
                    userProfile.FirstName = model.FirstName;
                    userProfile.LastName = model.LastName;
                    userProfile.Street = model.Street;
                    userProfile.Zipcode = model.Zipcode;
                    userProfile.City = model.City;
                    userProfile.Country = model.Country;

                    try
                    {
                        _context.Update(userProfile);
                        await _context.SaveChangesAsync();

                    }
                    catch (Exception)
                    {
                        model.ErrorMessage = "Something went wrong, please try again or contact admin";
                        return View(model);

                    }


                    //Letar upp usern, sätter de nya värdena och sparar
                    var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
                    user.Email = model.Email;
                    user.NormalizedEmail = model.Email.ToUpper();
                    user.UserName = model.Email;
                    user.NormalizedUserName = model.Email.ToUpper();

                    try
                    {
                        _context.Update(user);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {

                        model.ErrorMessage = "Something went wrong, please try again or contact admin";
                        return View(model);
                    }

                    model.SuccessMessage = "Changes saved successfully";
                    return View(model);
                }

                model.ErrorMessage = "Something went wrong, please try again or contact admin";
                return View(model);
            }

            return RedirectToAction("signin", "Authentication");

        }

    }
}
