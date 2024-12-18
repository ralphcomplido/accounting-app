using LightNap.Core.Api;
using LightNap.Core.Notifications.Dto.Request;
using LightNap.Core.Notifications.Dto.Response;

namespace LightNap.Core.Notifications.Interfaces
{
    public interface INotificationService
    {
        Task<NotificationDto?> GetNotificationAsync(int id);
        Task<PagedResponse<NotificationDto>> SearchNotificationsAsync(string userId, SearchNotificationsDto requestDto);
        Task CreateUserNotificationAsync(string userId, CreateNotificationDto requestDto);
        Task CreateRoleNotificationAsync(string role, CreateNotificationDto requestDto);
        Task MarkAsReadAsync(int id);
        Task MarkAllAsReadAsync(string userId);
    }
}