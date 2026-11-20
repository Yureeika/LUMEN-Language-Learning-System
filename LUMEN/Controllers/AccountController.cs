namespace LUMEN.Controllers;

using LUMEN.Models;
using LUMEN.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

public class AccountController(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IEmailSender emailSender,
    ILogger<AccountController> logger,
    LUMEN.Data.ApplicationDbContext context) : Controller
{
    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string Email, string Password, bool RememberMe)
    {
        if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
        {
            ViewBag.Error = "Email and Password are required";
            return View();
        }

        var result = await signInManager.PasswordSignInAsync(Email, Password, RememberMe, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            // Track login time
            var user = await userManager.FindByEmailAsync(Email);
            if (user != null)
            {
                user.LastLoginAt = DateTime.UtcNow;
                await context.SaveChangesAsync();
            }

            logger.LogInformation("User logged in.");

            if (User.IsInRole("SuperAdmin"))
            {
                return RedirectToAction("AdminDashboard", "Home");
            }
            return RedirectToAction("Dashboard", "Home");
        }

        ViewBag.Error = "Invalid login attempt.";
        return View();
    }

    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string ConfirmPassword)
    {
        if (string.IsNullOrEmpty(FirstName) ||
            string.IsNullOrEmpty(LastName) ||
            string.IsNullOrEmpty(Email) ||
            string.IsNullOrEmpty(Password))
        {
            ViewBag.Error = "All fields are required";
            return View();
        }

        if (Password != ConfirmPassword)
        {
            ViewBag.Error = "Passwords do not match";
            return View();
        }

        var user = new ApplicationUser
        {
            UserName = Email,
            Email = Email,
            FirstName = FirstName,
            LastName = LastName,

            // No category is selected during registration
            Categories = "",

            EmailConfirmed = true,
            TotalXP = 0,
            CreatedAt = DateTime.UtcNow
        };

        var result = await userManager.CreateAsync(user, Password);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Student");

            logger.LogInformation(
                "User created a new account.");

            await signInManager.SignInAsync(
                user,
                isPersistent: false);

            return RedirectToAction(
                "Index",
                "Home");
        }

        ViewBag.Error = string.Join(
            ", ",
            result.Errors.Select(e => e.Description));

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        logger.LogInformation("User logged out.");
        return RedirectToAction("Index", "Home");
    }

    // ===== FORGOT PASSWORD =====
    public IActionResult ForgotPassword() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(string Email)
    {
        if (string.IsNullOrEmpty(Email))
        {
            ViewBag.Error = "Please enter your email address";
            return View();
        }

        var user = await userManager.FindByEmailAsync(Email);

        // Always show success message (security - don't reveal if email exists)
        if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
        {
            ViewBag.Success = "If that email is registered, a password reset link has been sent. Please check your inbox.";
            return View();
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(user);
        var callbackUrl = Url.Action(
            "ResetPassword",
            "Account",
            new { userId = user.Id, code = token },
            protocol: Request.Scheme);

        var htmlMessage = $@"
        <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; padding: 20px;'>
            <div style='background: #1a1a1a; padding: 20px; text-align: center; border-radius: 8px 8px 0 0;'>
                <h1 style='color: #C65D3E; margin: 0; font-size: 28px;'>LUMEN</h1>
                <p style='color: white; margin: 5px 0 0;'>Learning Platform</p>
            </div>
            <div style='background: #FAF6ED; padding: 30px; border-radius: 0 0 8px 8px;'>
                <h2 style='color: #1a1a1a;'>Password Reset Request</h2>
                <p>Hello {user.FirstName},</p>
                <p>We received a request to reset your password. Click the button below to create a new password:</p>
                <div style='text-align: center; margin: 30px 0;'>
                    <a href='{System.Net.WebUtility.HtmlEncode(callbackUrl)}' 
                       style='background: #C65D3E; color: white; padding: 12px 30px; 
                              text-decoration: none; border-radius: 4px; font-weight: bold;
                              display: inline-block;'>
                        Reset Password
                    </a>
                </div>
                <p style='color: #6b6b6b; font-size: 14px;'>
                    If you didn't request this, please ignore this email. 
                    This link will expire in 24 hours.
                </p>
                <p style='color: #6b6b6b; font-size: 12px; margin-top: 20px;'>
                    If the button doesn't work, copy this link:<br/>
                    <a href='{System.Net.WebUtility.HtmlEncode(callbackUrl)}' style='color: #C65D3E; word-break: break-all;'>
                        {System.Net.WebUtility.HtmlEncode(callbackUrl)}
                    </a>
                </p>
            </div>
            <div style='text-align: center; padding: 20px; color: #6b6b6b; font-size: 12px;'>
                &copy; 2026 LUMEN Learning Platform. All rights reserved.
            </div>
        </div>";

        try
        {
            await emailSender.SendEmailAsync(Email, "LUMEN - Reset Your Password", htmlMessage);
            ViewBag.Success = "If that email is registered, a password reset link has been sent. Please check your inbox and spam folder.";
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to send reset email to {Email}", Email);
            ViewBag.Error = "Failed to send email. Please check your email configuration or try again later.";
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(string UserId, string Code, string Password, string ConfirmPassword)
    {
        if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ConfirmPassword))
        {
            ViewBag.Error = "Please fill in all fields";
            ViewBag.UserId = UserId;
            ViewBag.Code = Code;
            return View();
        }

        if (Password != ConfirmPassword)
        {
            ViewBag.Error = "Passwords do not match";
            ViewBag.UserId = UserId;
            ViewBag.Code = Code;
            return View();
        }

        var user = await userManager.FindByIdAsync(UserId);
        if (user == null)
        {
            ViewBag.Error = "User not found";
            return View("Error");
        }

        var result = await userManager.ResetPasswordAsync(user, Code, Password);

        if (result.Succeeded)
        {
            return RedirectToAction("ResetConfirmation");
        }

        ViewBag.Error = string.Join(", ", result.Errors.Select(e => e.Description));
        ViewBag.UserId = UserId;
        ViewBag.Code = Code;
        return View();
    }

    public IActionResult ResetConfirmation() => View();

    public IActionResult AccessDenied() => View();


    // TEMPORARY: Reset admin password - REMOVE AFTER USE
    [HttpGet]
    public async Task<IActionResult> ResetAdmin()
    {
        var admin = await userManager.FindByEmailAsync("superadmin@lumen.com");
        if (admin == null)
        {
            // Create admin if doesn't exist
            admin = new ApplicationUser
            {
                UserName = "superadmin@lumen.com",
                Email = "superadmin@lumen.com",
                FirstName = "Super",
                LastName = "Admin",
                Category = "Coding",
                Categories = "Coding",
                EmailConfirmed = true,
                TotalXP = 0,
                CreatedAt = DateTime.UtcNow
            };
            var createResult = await userManager.CreateAsync(admin, "Admin@123");
            if (createResult.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "SuperAdmin");
                return Content("Admin created with password: Admin@123");
            }
            return Content("Failed: " + string.Join(", ", createResult.Errors.Select(e => e.Description)));
        }

        // Reset password
        var token = await userManager.GeneratePasswordResetTokenAsync(admin);
        var result = await userManager.ResetPasswordAsync(admin, token, "Admin@123");

        if (result.Succeeded)
        {
            // Ensure roles
            if (await userManager.IsInRoleAsync(admin, "Admin"))
            {
                await userManager.RemoveFromRoleAsync(admin, "Admin");
            }

            if (await userManager.IsInRoleAsync(admin, "Student"))
            {
                await userManager.RemoveFromRoleAsync(admin, "Student");
            }

            if (!await userManager.IsInRoleAsync(admin, "SuperAdmin"))
            {
                await userManager.AddToRoleAsync(admin, "SuperAdmin");
            }

            return Content("Password reset to: Admin@123 - Roles assigned");
        }

        return Content("Reset failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));
    }
}