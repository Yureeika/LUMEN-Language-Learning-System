namespace LUMEN.Controllers;

using LUMEN.Data;
using LUMEN.Models;
using LUMEN.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class ChatController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Account");

        // Get or create default chat room
        var room = await context.ChatRooms.FirstOrDefaultAsync(r => r.Name == "General");
        if (room == null)
        {
            room = new ChatRoom { Name = "General", IsPublic = true, Description = "General discussion" };
            context.ChatRooms.Add(room);
            await context.SaveChangesAsync();
        }

        // Get messages for this room (last 100)
        var messages = await context.ChatMessages
            .Include(m => m.User)
            .Where(m => m.ChatRoomId == room.Id)
            .OrderByDescending(m => m.SentAt)
            .Take(100)
            .ToListAsync();
        messages.Reverse();

        // Get all public rooms
        var rooms = await context.ChatRooms.Where(r => r.IsPublic).ToListAsync();

        var viewModel = new ChatViewModel
        {
            CurrentRoom = room,
            AvailableRooms = rooms,
            Messages = messages,
            CurrentUserId = user.Id
        };

        return View(viewModel);
    }
}