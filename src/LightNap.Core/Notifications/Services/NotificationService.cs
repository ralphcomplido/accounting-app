using LightNap.Core.Api;
using LightNap.Core.Data;
using LightNap.Core.Data.Entities;
using LightNap.Core.Notifications.Dto.Request;
using LightNap.Core.Notifications.Dto.Response;
using LightNap.Core.Notifications.Extensions;
using LightNap.Core.Notifications.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LightNap.Core.Notifications.Services
{
    /// <summary>  
    /// Service for managing user notifications.
    /// </summary>  
    public class NotificationService(ApplicationDbContext db, UserManager<ApplicationUser> userManager) : INotificationService
    {
        /// <summary>
        /// Creates a notification for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user to notify.</param>
        /// <param name="requestDto">The notification data transfer object containing the notification details.</param>
        public async Task CreateUserNotificationAsync(string userId, CreateNotificationDto requestDto)
        {
            Notification notification = requestDto.ToCreate(userId);
            db.Notifications.Add(notification);
            await db.SaveChangesAsync();

            // TODO: Send notification to SignalR
        }

        /// <summary>
        /// Creates a notification for all users in a specified role.
        /// </summary>
        /// <param name="role">The role for which to create notifications.</param>
        /// <param name="requestDto">The notification data transfer object containing the notification details.</param>
        public async Task CreateRoleNotificationAsync(string role, CreateNotificationDto requestDto)
        {
            foreach (var user in await userManager.GetUsersInRoleAsync(role))
            {
                await this.CreateUserNotificationAsync(user.Id, requestDto);
            }
        }

        /// <summary>
        /// Retrieves a notification by its ID.
        /// </summary>
        /// <param name="id">The ID of the notification to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the notification data transfer object.</returns>
        public async Task<NotificationDto?> GetNotificationAsync(int id)
        {
            var notification = await db.Notifications.FindAsync(id);
            return notification?.ToDto();
        }

        /// <summary>
        /// Marks all notifications as read for a specific user.
        /// </summary>
        /// <param name="userId">The ID of the user whose notifications should be marked as read.</param>
        public async Task MarkAllAsReadAsync(string userId)
        {
            // ExecuteUpdateAsync is not universally supported.
            foreach (var notification in await db.Notifications.Where(n => n.UserId == userId).ToListAsync())
            {
                notification.Status = NotificationStatus.Read;
            }
            await db.SaveChangesAsync();
        }

        /// <summary>
        /// Marks a specific notification as read.
        /// </summary>
        /// <param name="id">The ID of the notification to mark as read.</param>
        public async Task MarkAsReadAsync(int id)
        {
            Notification notification = db.Notifications.Find(id) ?? throw new UserFriendlyApiException("Notification not found");
            notification.Status = NotificationStatus.Read;
            await db.SaveChangesAsync();
        }

        /// <summary>
        /// Searches for notifications based on the specified criteria.
        /// </summary>
        /// <param name="userId">The ID of the user whose notifications to search.</param>
        /// <param name="requestDto">The search criteria for the notifications.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the search results data transfer object.</returns>
        public async Task<NotificationSearchResultsDto> SearchNotificationsAsync(string userId, SearchNotificationsDto requestDto)
        {
            IQueryable<Notification> query = db.Notifications.Where(n => n.UserId == userId);

            if (requestDto.SinceId is not null)
            {
                query = query.Where(n => n.Id > requestDto.SinceId);
            }

            if (requestDto.PriorToId is not null)
            {
                query = query.Where(n => n.Id < requestDto.PriorToId);
            }

            if (requestDto.Status is not null)
            {
                query = query.Where(n => n.Status == requestDto.Status);
            }
            else
            {
                query = query.Where(n => n.Status != NotificationStatus.Archived);
            }

            if (requestDto.Type is not null)
            {
                query = query.Where(n => n.Type == requestDto.Type);
            }

            query = query.OrderByDescending(item => item.Id);

            int totalCount = await query.CountAsync();
            if (requestDto.PageNumber > 1)
            {
                query = query.Skip((requestDto.PageNumber - 1) * requestDto.PageSize);
            }

            var items = await query.Take(requestDto.PageSize).Select(item => item.ToDto()).ToListAsync();

            return new NotificationSearchResultsDto(items, requestDto.PageNumber, requestDto.PageSize, totalCount)
            {
                UnreadCount = await db.Notifications.CountAsync(n => n.UserId == userId && n.Status == NotificationStatus.Unread)
            };
        }
    }
}