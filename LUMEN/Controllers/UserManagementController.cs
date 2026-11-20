namespace LUMEN.Controllers;

using LUMEN.Data;
using LUMEN.Models;
using LUMEN.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "SuperAdmin")]
public class UserManagementController(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager,
    ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var users = await userManager.Users.ToListAsync();
        var userRoles = new List<UserRoleViewModel>();

        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);
            userRoles.Add(new UserRoleViewModel
            {
                UserId = user.Id,
                UserName = $"{user.FirstName} {user.LastName}",
                Email = user.Email ?? "",
                CurrentRole = roles.FirstOrDefault() ?? "None",
                AvailableRoles = new List<string>
{
    "SuperAdmin",
    "Student"
}
            });
        }

        return View(userRoles);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateRole(string userId, string newRole)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null) return NotFound();

        var currentRoles = await userManager.GetRolesAsync(user);
        await userManager.RemoveFromRolesAsync(user, currentRoles);

        var result = await userManager.AddToRoleAsync(user, newRole);
        if (result.Succeeded)
        {
            TempData["SuccessMessage"] = "User role updated successfully.";
        }
        else
        {
            TempData["ErrorMessage"] = "Failed to update user role.";
        }

        return RedirectToAction(nameof(Index));
    }
}