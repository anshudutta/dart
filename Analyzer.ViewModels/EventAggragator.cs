using System;
using Microsoft.Practices.Composite.Presentation.Events;

namespace Analyzer.ViewModels
{
    public class EventAggregator : IEventAggregator
    {
        private static Microsoft.Practices.Composite.Events.IEventAggregator _eventAggregator = CreateAggregator();

        private static Microsoft.Practices.Composite.Events.IEventAggregator CreateAggregator()
        {
            return new Microsoft.Practices.Composite.Events.EventAggregator();
        }

        #region IEventAggregator Members

        private CompositePresentationEvent<T> GetEventBus<T>()
        {
            var bus = _eventAggregator.GetEvent<CompositePresentationEvent<T>>();
            return bus;
        }

        public void SendMessage<TEvent, TPayload>(TEvent message) where TEvent : PresentationEvent<TPayload>
        {
            var bus = GetEventBus<TEvent>();
            bus.Publish(message);
        }

        public void Subscribe<TEvent, TPayload>(Action<TEvent> action) where TEvent : PresentationEvent<TPayload>
        {
            var bus = GetEventBus<TEvent>();
            bus.Subscribe(action);
        }

        public void Subscribe<TEvent, TPayload>(Action<TEvent> action, bool keepSubscriberReferenceAlive) where TEvent : PresentationEvent<TPayload>
        {
            var bus = GetEventBus<TEvent>();
            bus.Subscribe(action, keepSubscriberReferenceAlive);
        }

        public void Subscribe<TEvent, TPayload>(Action<TEvent> action, ThreadOption threadOption, bool keepSubscriberReferenceAlive, Predicate<TEvent> where) where TEvent : PresentationEvent<TPayload>
        {
            var bus = GetEventBus<TEvent>();
            bus.Subscribe(action, threadOption, keepSubscriberReferenceAlive, where);
        }

        public void Unsubscribe<TEvent, TPayload>(Action<TEvent> action) where TEvent : PresentationEvent<TPayload>
        {
            var bus = GetEventBus<TEvent>();
            bus.Unsubscribe(action);
        }

        #endregion
    }
}
