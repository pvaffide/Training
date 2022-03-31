using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Training.Services
{
    public class TrainingNotificationService : ITrainingNotificationService
    {

        private readonly NotificationService _notificationService;

        public TrainingNotificationService(NotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        public void Notify(IEnumerable<string> details, NotificationSeverity severity = NotificationSeverity.Info, string summary = "", double duration = ITrainingNotificationService.DEFAULT_DURATION)
        {
            foreach(var detail in details)
                _notificationService.Notify(severity, summary, detail, duration);
        }

        public void Notify(NotificationSeverity severity = NotificationSeverity.Info, string summary = "", string detail = "", double duration = ITrainingNotificationService.DEFAULT_DURATION)
        {
            _notificationService.Notify(severity, summary, detail, duration);
        }
    }
}
