namespace LUMEN.Data;

using LUMEN.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services, IWebHostEnvironment env)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        await context.Database.MigrateAsync();

        // Create roles
        string[] roles = { "SuperAdmin", "Student" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // Create superadmin if not exists
        var adminEmail = "superadmin@lumen.com";
        if (await userManager.FindByEmailAsync(adminEmail) == null)
        {
            var admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Super",
                LastName = "Admin",
                Category = "Coding",
                Categories = "Coding",
                EmailConfirmed = true,
                TotalXP = 0,
                CreatedAt = DateTime.UtcNow
            };
            var result = await userManager.CreateAsync(admin, "Admin@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "SuperAdmin");
            }
        }

        // Remove Intermediate and Advanced subjects
        var unwantedSubjects = await context.Subjects
            .Where(s => s.Level == "Intermediate" || s.Level == "Advanced")
            .ToListAsync();

        if (unwantedSubjects.Any())
        {
            context.Subjects.RemoveRange(unwantedSubjects);
            await context.SaveChangesAsync();
        }

        // Seed subjects if empty
        if (!await context.Subjects.AnyAsync())
        {
            var subjects = GetSeedSubjects();
            context.Subjects.AddRange(subjects);
            await context.SaveChangesAsync();
        }
    }

    private static List<Subject> GetSeedSubjects()
    {
        return new List<Subject>
        {
            // ===== CODING - BASIC (FREE) - 5 courses =====
            new Subject { Title = "HTML & CSS Basics", Category = "Coding", Level = "Basic", Price = 0, Description = "Learn web fundamentals with HTML5 and CSS3 from scratch", ImageUrl = "https://images.unsplash.com/photo-1621839673705-6617adf9e890?w=800&q=80", IsActive = true },
            new Subject { Title = "Python Fundamentals", Category = "Coding", Level = "Basic", Price = 0, Description = "Start your coding journey with Python basics and syntax", ImageUrl = "https://images.unsplash.com/photo-1526379095098-d400fd0bf935?w=800&q=80", IsActive = true },
            new Subject { Title = "JavaScript Essentials", Category = "Coding", Level = "Basic", Price = 0, Description = "Core JavaScript concepts for absolute beginners", ImageUrl = "https://images.unsplash.com/photo-1627398242454-45a1465c2479?w=800&q=80", IsActive = true },
            new Subject { Title = "Git & Version Control", Category = "Coding", Level = "Basic", Price = 0, Description = "Master Git basics and collaboration workflows", ImageUrl = "https://images.unsplash.com/photo-1618401471353-b98afee0b2eb?w=800&q=80", IsActive = true },
            new Subject { Title = "Command Line Basics", Category = "Coding", Level = "Basic", Price = 0, Description = "Navigate and work with terminal efficiently", ImageUrl = "https://images.unsplash.com/photo-1629654297299-c8506221ca97?w=800&q=80", IsActive = true },

            // ===== LANGUAGES - BASIC (FREE) - 5 courses =====
            new Subject { Title = "Spanish Basics", Category = "Languages", Level = "Basic", Price = 0, Description = "Start your Spanish journey with fundamentals", ImageUrl = "https://images.unsplash.com/photo-1543783207-ec64e4d95325?w=800&q=80", IsActive = true },
            new Subject { Title = "French Fundamentals", Category = "Languages", Level = "Basic", Price = 0, Description = "Learn French alphabet, greetings, and basics", ImageUrl = "https://images.unsplash.com/photo-1502602898657-3e91760cbb34?w=800&q=80", IsActive = true },
            new Subject { Title = "German Essentials", Category = "Languages", Level = "Basic", Price = 0, Description = "German language fundamentals and pronunciation", ImageUrl = "https://images.unsplash.com/photo-1467269204594-9661b134dd2b?w=800&q=80", IsActive = true },
            new Subject { Title = "Italian Starter", Category = "Languages", Level = "Basic", Price = 0, Description = "Italian language basics and culture introduction", ImageUrl = "https://images.unsplash.com/photo-1516483638261-f4dbaf036963?w=800&q=80", IsActive = true },
            new Subject { Title = "Japanese Hiragana", Category = "Languages", Level = "Basic", Price = 0, Description = "Master Japanese writing system basics", ImageUrl = "https://images.unsplash.com/photo-1528360983277-13d401cdc186?w=800&q=80", IsActive = true },

        };
    }
}