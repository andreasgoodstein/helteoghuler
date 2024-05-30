using System.Collections.Generic;

namespace HelteOgHulerClient.Interfaces;

public interface ISubscriber<T>
{
    string GetId();

    void Message(T message);
}
