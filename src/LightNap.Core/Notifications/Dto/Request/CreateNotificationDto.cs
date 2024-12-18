using LightNap.Core.Data.Entities;

namespace LightNap.Core.Notifications.Dto.Request
{
    public class CreateNotificationDto
    {
        public NotificationType Type { get; set; }
        public Dictionary<string, object> Data { get; set; } = [];
    }
}