using System.Collections.Generic;

namespace HelteOgHulerClient.Interfaces;

public interface ISubscriber<T>
{
    void Message(T message);
}
