using System.Collections.Generic;
using HelteOgHulerClient.Interfaces;

namespace HelteOgHulerClient;

public class PubSub<T>
{
    private List<ISubscriber<T>> subscriberList;

    public PubSub()
    {
        subscriberList = new List<ISubscriber<T>>();
    }

    public void Register(ISubscriber<T> subscriber)
    {
        subscriberList.Add(subscriber);
    }

    public void Publish(T message)
    {
        subscriberList.ForEach(subscriber =>
        {
            subscriber.Message(message);
        });
    }
}
