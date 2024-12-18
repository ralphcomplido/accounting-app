using LightNap.Core.Api;
using LightNap.Core.Data.Entities;

namespace LightNap.Core.Notifications.Dto.Request
{
    public class SearchNotificationsDto : PaginationRequestDtoBase
    {
        public int? SinceId { get; set; }
        public NotificationStatus? Status { get; set; }
        public NotificationType? Type { get; set; }
    }
}