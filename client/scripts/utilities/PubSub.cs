using Godot;
using HelteOgHulerClient.Interfaces;
using System.Collections.Generic;

namespace HelteOgHulerClient.Utilities;

public class PubSub<T>
{
	private Dictionary<string, ISubscriber<T>> subscriberMap;

	public PubSub()
	{
		subscriberMap = new Dictionary<string, ISubscriber<T>>();
	}

	public void Register(ISubscriber<T> subscriber)
	{
		subscriberMap.Add(subscriber.GetId(), subscriber);
	}

	public void Publish(T message)
	{
		foreach (var entry in subscriberMap)
		{
			entry.Value.Message(message);
		}
	}

	public void Unregister(ISubscriber<T> subscriber)
	{
		subscriberMap.Remove(subscriber.GetId());
	}
}
