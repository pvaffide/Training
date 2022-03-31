using Radzen;
using System.Collections.Generic;


namespace Training.Services
{
    public interface ITrainingNotificationService
    {
        const double DEFAULT_DURATION = 7000.0;
        void Notify(IEnumerable<string> details, NotificationSeverity severity = NotificationSeverity.Info, string summary = "", double duration = DEFAULT_DURATION);
        void Notify(NotificationSeverity severity = NotificationSeverity.Info, string summary = "", string detail = "", double duration = DEFAULT_DURATION);

    }
}
