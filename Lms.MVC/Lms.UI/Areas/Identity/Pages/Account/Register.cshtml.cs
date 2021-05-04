using Lms.MVC.Core.Entities;
using Lms.MVC.Core.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Lms.MVC.UI.Areas.Identity.Pages.Account
{
    [Authorize(Roles = "Admin , Teacher")]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IUoW uoW;

       public RegisterModel(
           IUoW uoW,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            this.uoW = uoW;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "First Name")]
            [StringLength(50, MinimumLength = 3)]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            [StringLength(50, MinimumLength = 3)]
            public string LastName { get; set; }

            public string Name => $"{FirstName} {LastName}";

            //[Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            [Display(Name = "Role :")]
            public string Role { get; set; }
           
           
            
        }
       
        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var password = "password";

                Input.Password = password;
                Input.ConfirmPassword = password;
                if (!User.IsInRole("Admin"))
                {
                    Input.Role = "Student";
                }
                if (Input.Role is null)
                {
                    ModelState.AddModelError("Role", "Please Choose a role");
                }


                var user = GetUserByRole(Input.Role); 
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                  await  uoW.UserRepository.ChangeRoleAsync(user);
                    var role = await _userManager.AddToRoleAsync(user, Input.Role);
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");


                    var passwordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    passwordToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(passwordToken));
                    var resetPasswordCallbackUrl = Url.Page(
                        "/Account/ResetPassword",
                        pageHandler: null,
                        values: new { area = "Identity", passwordToken },
                        protocol: Request.Scheme);

                    if (!_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                    await _emailSender.SendEmailAsync(
                        Input.Email,
                        "You are registered in Lms",
                        $"Your password is {Input.Password} \n Please reset your password by <a href='{HtmlEncoder.Default.Encode(resetPasswordCallbackUrl)}'>clicking here</a>.");
                    }


                    return LocalRedirect(returnUrl);


                    //if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    //{
                    //    return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    //}
                    //else
                    //{
                    //    await _signInManager.SignInAsync(user, isPersistent: false);
                    //    return LocalRedirect(returnUrl);
                    //}
                }
                foreach (var error in result.Errors)
                {
                
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        public async Task GetUsersAsync()
        {
            var users = User.Identities.ToList();
            var teachers = await _userManager.GetUsersInRoleAsync("Teacher");
            var students = await _userManager.GetUsersInRoleAsync("Student");
        }
        private ApplicationUser GetUserByRole(string role)
        {


            if (role == "Teacher" || role == "Admin")
            {
                var appUser = new ApplicationUser { UserName = $"{Input.FirstName}.{Input.LastName}", Email = Input.Email, Name = Input.Name, Role = role };
                return appUser;
            }
            else 
            {
                var appUser = new ApplicationUser { UserName = $"{Input.FirstName}.{Input.LastName}", Email = Input.Email, Name = Input.Name, Role = "Student"};
                return appUser;
            }

          
        }
    }
}