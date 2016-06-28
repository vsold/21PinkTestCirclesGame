using System;
using System.Collections.Generic;
using UnityEngine;

namespace CirclesGame
{
    public class NotificationCenter
    {
        private static NotificationCenter instance;

        public static NotificationCenter Instance
        {
            get { return instance ?? (instance = new NotificationCenter()); }
        }

        private readonly Dictionary<NotificationName, List<Observer>> notifications =
            new Dictionary<NotificationName, List<Observer>>();

        public void AddObserver(Component component, Action<Notification> action, NotificationName notificationName)
        {
            if (notificationName == NotificationName.NOT_SET)
            {
                Debug.LogWarning("Null name specified for notification in AddObserver.");
                return;
            }
            var observer = new Observer(component, action);

            if (!notifications.ContainsKey(notificationName))
            {
                notifications.Add(notificationName, new List<Observer>());
            }

            var notifyList = notifications[notificationName];

            if (notifyList != null)
            {
                notifyList.Add(observer);
            }
        }

        public void RemoveObserver(Component component, NotificationName name)
        {
            List<Observer> notifyList;
            notifications.TryGetValue(name, out notifyList);

            if (notifyList == null || notifyList.Count == 0)
            {
                notifications.Remove(name);
                return;
            }

            var observer = notifyList.Find(n => n.component = component);

            if (observer != null)
            {
                notifyList.Remove(observer);
            }
        }

        public void RemoveObserver(Component component)
        {
            foreach (var notificationName in notifications.Keys)
            {
                RemoveObserver(component, notificationName);
            }
        }

        public void PostNotification(Component sender, NotificationName notificationName, Component reciever)
        {
            PostNotification(sender, notificationName, null, reciever);
        }

        public void PostNotification(Component sender, NotificationName notificationName)
        {
            PostNotification(sender, notificationName, null, null);
        }

        public void PostNotification(NotificationName notificationName, NotificationArgs args)
        {
            PostNotification(null, notificationName, args, null);
        }

        public void PostNotification(Component sender, NotificationName notificationName, NotificationArgs args)
        {
            PostNotification(new Notification(sender, notificationName, args));
        }

        public void PostNotification(Component sender, NotificationName notificationName, NotificationArgs args,
            Component reciever)
        {
            PostNotification(new Notification(sender, notificationName, args, reciever));
        }

        public void PostNotification(Notification newNotification)
        {
            if (newNotification.name == NotificationName.NOT_SET)
            {
                Debug.LogWarning("Null name sent to PostNotification.");
                return;
            }

            List<Observer> notifyList;
            notifications.TryGetValue(newNotification.name, out notifyList);

            if (notifyList == null)
            {
                notifications.Remove(newNotification.name);
                return;
            }

            var observersToRemove = new List<Observer>();

            foreach (var observer in notifyList)
            {
                if (observer != null && observer.component != null)
                {
                    observer.action.Invoke(newNotification);
                }
                else
                {
                    observersToRemove.Add(observer);
                }
            }

            foreach (var observer in observersToRemove)
            {
                notifyList.Remove(observer);
            }
        }

        public void Destroy()
        {
            instance = null;
        }
    }
}

