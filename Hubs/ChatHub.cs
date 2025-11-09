using System;
using BlazorSignalRApp.Data;
using BlazorSignalRApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BlazorWebAppMovies.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;

    public ChatHub(IDbContextFactory<ApplicationDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task SendMessage(int userId, string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        await using var context = await _dbFactory.CreateDbContextAsync();
        var user = await context.ChatUsers.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
        if (user is null)
        {
            return;
        }

        var chatMessage = new ChatMessage
        {
            UserName = user.Name ?? "Unknown",
            Message = message.Trim(),
            SentAt = DateTime.UtcNow
        };

        context.ChatMessages.Add(chatMessage);
        await context.SaveChangesAsync();

        await Clients.All.SendAsync("ReceiveMessage", chatMessage.UserName, chatMessage.Message, chatMessage.SentAt);
    }
}
