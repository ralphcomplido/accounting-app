using LightNap.Core.Data.Entities;

namespace LightNap.Core.Notifications.Dto.Response
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public NotificationStatus Status { get; set; }
        public NotificationType Type { get; set; }
        public Dictionary<string, object> Data { get; set; } = [];
    }
}