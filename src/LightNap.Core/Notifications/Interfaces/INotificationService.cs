using LightNap.Core.Notifications.Dto.Request;
using LightNap.Core.Notifications.Dto.Response;

namespace LightNap.Core.Notifications.Interfaces
{
    public interface INotificationService
    {
        /// <summary>
        /// Retrieves a notification by its ID.
        /// </summary>
        /// <param name="id">The ID of the notification.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the notification DTO.</returns>
        Task<NotificationDto?> GetNotificationAsync(int id);

        /// <summary>
        /// Searches for notifications based on the specified criteria.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="requestDto">The search criteria.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the search results DTO.</returns>
        Task<NotificationSearchResultsDto> SearchNotificationsAsync(string userId, SearchNotificationsDto requestDto);

        /// <summary>
        /// Creates a new notification for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="requestDto">The notification creation details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task CreateUserNotificationAsync(string userId, CreateNotificationDto requestDto);

        /// <summary>
        /// Creates a new notification for a role.
        /// </summary>
        /// <param name="role">The role for which the notification is created.</param>
        /// <param name="requestDto">The notification creation details.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task CreateRoleNotificationAsync(string role, CreateNotificationDto requestDto);

        /// <summary>
        /// Marks a notification as read by its ID.
        /// </summary>
        /// <param name="id">The ID of the notification.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task MarkAsReadAsync(int id);

        /// <summary>
        /// Marks all notifications as read for a user.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task MarkAllAsReadAsync(string userId);
    }
}