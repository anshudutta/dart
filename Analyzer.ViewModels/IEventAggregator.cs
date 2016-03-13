using System;

namespace Analyzer.ViewModels
{
    public interface IEventAggregator
    {
        void SendMessage<TEvent, TPayload>(TEvent message) where TEvent : PresentationEvent<TPayload>;
        void Subscribe<TEvent, TPayload>(Action<TEvent> action) where TEvent : PresentationEvent<TPayload>;
        void Subscribe<TEvent, TPayload>(Action<TEvent> action, bool keepSubscriberReferenceAlive) where TEvent : PresentationEvent<TPayload>;
        void Subscribe<TEvent, TPayload>(Action<TEvent> action, Microsoft.Practices.Composite.Presentation.Events.ThreadOption threadOption, bool keepSubscriberReferenceAlive, Predicate<TEvent> where) where TEvent : PresentationEvent<TPayload>;
        void Unsubscribe<TEvent, TPayload>(Action<TEvent> action) where TEvent : PresentationEvent<TPayload>;
    }
}