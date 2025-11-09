using System;
using System.Collections.Concurrent;
using BlazorSignalRApp.Data;
using BlazorSignalRApp.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace BlazorWebAppMovies.Hubs;

public class ChatHub : Hub
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
    private static readonly ConcurrentDictionary<int, HashSet<string>> Connections = new();

    public ChatHub(IDbContextFactory<ApplicationDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public Task RegisterUser(int userId)
    {
        var connections = Connections.GetOrAdd(userId, _ => new HashSet<string>());
        lock (connections)
        {
            connections.Add(Context.ConnectionId);
        }
        return Task.CompletedTask;
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        foreach (var (userId, ids) in Connections.ToArray())
        {
            lock (ids)
            {
                if (ids.Remove(Context.ConnectionId) && ids.Count == 0)
                {
                    Connections.TryRemove(userId, out _);
                }
            }
        }

        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(int userId, int? recipientUserId, string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        await using var context = await _dbFactory.CreateDbContextAsync();
        var sender = await context.ChatUsers.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
        if (sender is null)
        {
            return;
        }

        User? recipient = null;
        if (recipientUserId.HasValue)
        {
            recipient = await context.ChatUsers.AsNoTracking().FirstOrDefaultAsync(u => u.Id == recipientUserId.Value);
            if (recipient is null)
            {
                return;
            }
        }

        var chatMessage = new ChatMessage
        {
            UserName = sender.Name ?? "Unknown",
            Message = message.Trim(),
            SentAt = DateTime.UtcNow,
            IsPrivate = recipient is not null,
            RecipientUserId = recipient?.Id,
            RecipientUserName = recipient?.Name
        };

        context.ChatMessages.Add(chatMessage);
        await context.SaveChangesAsync();

        if (recipient is null)
        {
            await Clients.All.SendAsync("ReceiveMessage",
                chatMessage.UserName,
                chatMessage.Message,
                chatMessage.SentAt,
                false,
                null);
        }
        else
        {
            var senderConnections = GetConnections(sender.Id);
            var recipientConnections = GetConnections(recipient.Id);
            var targets = senderConnections.Concat(recipientConnections).Distinct().ToList();

            if (targets.Count == 0)
            {
                return;
            }

            await Clients.Clients(targets).SendAsync("ReceiveMessage",
                chatMessage.UserName,
                chatMessage.Message,
                chatMessage.SentAt,
                true,
                chatMessage.RecipientUserName);
        }
    }

    private static IEnumerable<string> GetConnections(int userId)
    {
        if (Connections.TryGetValue(userId, out var set))
        {
            lock (set)
            {
                return set.ToArray();
            }
        }

        return Array.Empty<string>();
    }
}
