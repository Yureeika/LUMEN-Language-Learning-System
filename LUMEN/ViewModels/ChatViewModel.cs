using LUMEN.Models;

namespace LUMEN.ViewModels;

public class ChatViewModel
{
    public required ChatRoom CurrentRoom { get; set; }

    public List<ChatRoom> AvailableRooms { get; set; } = [];

    public required List<ChatMessage> Messages { get; set; } = [];

    public List<ApplicationUser> OnlineUsers { get; set; } = [];

    public required string CurrentUserId { get; set; }
}