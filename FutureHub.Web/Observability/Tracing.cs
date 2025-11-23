using System.Diagnostics;

namespace FutureHub.Web.Observability;

public static class Tracing
{
    public static readonly ActivitySource ActivitySource = new("FutureHub.Api");

    public static Activity? StartActivity(string name)
    {
        return ActivitySource.StartActivity(name);
    }

    public static void AddTag(Activity? activity, string key, string? value)
    {
        activity?.SetTag(key, value);
    }

    public static void AddEvent(Activity? activity, string eventName)
    {
        activity?.AddEvent(new ActivityEvent(eventName));
    }

    public static void RecordException(Activity? activity, Exception exception)
    {
        if (activity == null) return;
        
        activity.SetStatus(ActivityStatusCode.Error, exception.Message);
        activity.AddEvent(new ActivityEvent("exception", 
            tags: new ActivityTagsCollection
            {
                { "exception.type", exception.GetType().FullName },
                { "exception.message", exception.Message },
                { "exception.stacktrace", exception.StackTrace }
            }));
    }
}
