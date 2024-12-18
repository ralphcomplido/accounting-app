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
    public class NotificationService(ApplicationDbContext db, UserManager<ApplicationUser> userManager) : INotificationService
    {
        public async Task CreateUserNotificationAsync(string userId, CreateNotificationDto requestDto)
        {
            Notification notification = requestDto.ToCreate(userId);
            db.Notifications.Add(notification);
            await db.SaveChangesAsync();

            // TODO: Send notification to SignalR
        }

        public async Task CreateRoleNotificationAsync(string role, CreateNotificationDto requestDto)
        {
            foreach (var user in await userManager.GetUsersInRoleAsync(role))
            {
                await this.CreateUserNotificationAsync(user.Id, requestDto);
            }
        }

        public async Task<NotificationDto?> GetNotificationAsync(int id)
        {
            var notification = await db.Notifications.FindAsync(id);
            return notification?.ToDto();
        }

        public async Task MarkAllAsReadAsync(string userId)
        {
            // ExecuteUpdateAsync is not universally supported.
            foreach (var notification in await db.Notifications.Where(n => n.UserId == userId).ToListAsync())
            {
                notification.Status = NotificationStatus.Read;
            }
            await db.SaveChangesAsync();
        }

        public async Task MarkAsReadAsync(int id)
        {
            Notification notification = db.Notifications.Find(id) ?? throw new UserFriendlyApiException("Notification not found");
            notification.Status = NotificationStatus.Read;
            await db.SaveChangesAsync();
        }

        public async Task<PagedResponse<NotificationDto>> SearchNotificationsAsync(string userId, SearchNotificationsDto requestDto)
        {
            IQueryable<Notification> query = db.Notifications.Where(n => n.UserId == userId);

            if (requestDto.SinceId is not null)
            {
                query = query.Where(n => n.Id > requestDto.SinceId);
            }

            if (requestDto.Status is not null)
            {
                query = query.Where(n => n.Status == requestDto.Status);
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

            return new PagedResponse<NotificationDto>(items, requestDto.PageNumber, requestDto.PageSize, totalCount);
        }
    }
}