using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicQueue.Models
{
    static class NotificationManager
    {
        private static Notifications.Wpf.NotificationManager notificationManager = new Notifications.Wpf.NotificationManager();

        public static void ShowError(string message, string title = "Ошибка") => ShowNotification(title, message, NotificationType.Error);
        public static void ShowSuccess(string message, string title = "Успех") => ShowNotification(title, message, NotificationType.Success);
        public static void ShowInformation(string message, string title = "Информация") => ShowNotification(title, message, NotificationType.Information);
        public static void ShowWarning(string message, string title = "Внимание") => ShowNotification(title, message, NotificationType.Warning);

        private static void ShowNotification(string title, string message, NotificationType type)
        {
            notificationManager.Show(new NotificationContent
            {
                Title = title,
                Message = message,
                Type = type
            },
            areaName: "WindowArea");
        }
    }
}
