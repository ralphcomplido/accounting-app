using LightNap.Core.Data.Entities;
using LightNap.Core.Notifications.Dto.Request;
using LightNap.Core.Notifications.Dto.Response;

namespace LightNap.Core.Notifications.Extensions
{
    public static class NotificationExtensions
    {
        public static Notification ToCreate(this CreateNotificationDto dto, string userId)
        {
            return new Notification()
            {
                Data = dto.Data,
                Status = NotificationStatus.Unread,
                Timestamp = DateTime.UtcNow,
                Type = dto.Type,
                UserId = userId,
            };
        }

        public static NotificationDto ToDto(this Notification notification)
        {
            return new NotificationDto()
            {
                Data = notification.Data,
                Id = notification.Id,
                Status = notification.Status,
                Type = notification.Type,
                Timestamp = notification.Timestamp,
            };
        }
    }
}