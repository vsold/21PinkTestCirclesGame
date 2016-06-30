using System;
using UnityEngine;

namespace CirclesGame
{
    [Serializable]
    public class Notification
    {
        public NotificationName name;
        public Component sender;
        public Component reciever;
        public NotificationArgs args;

        public Notification(Component componentSender, NotificationName notificationName,
            NotificationArgs extraArgs = null, Component componentReciever = null)
        {
            sender = componentSender;
            reciever = componentReciever;
            name = notificationName;
            args = extraArgs;
        }

        public T GetArgs<T>() where T : NotificationArgs
        {
            if (args != null)
            {
                if (!(args is T))
                {
                    throw new InvalidCastException(string.Format("Try cast {0} to {1} for {2}", args.GetType(),
                        typeof (T), name));
                }
                return args as T;
            }
            else
            {
                return null;
            }
        }
    }

    [Serializable]
    public class Observer : IEquatable<Observer>
    {
        public Component component;
        public Action<Notification> action;

        public Observer(Component component, Action<Notification> action)
        {
            this.component = component;
            this.action = action;
        }

        public bool Equals(Observer other)
        {
            if (other == null)
            {
                return false;
            }
            return other.component == component && other.action == action;
        }
    }

    public enum NotificationName
    {
        NOT_SET,
        ON_CIRCLE_CLICK,
        ON_NEW_LEVEl,
        ON_SCORE_INC,
        ON_CIRCLE_HIT_GROUND,
    }
}
