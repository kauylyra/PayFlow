using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayFlow.Infra.Messaging.Interface;

public interface IEventPublisher
{
    Task PublishAsync<T>(
        T message,
        string routingKey,
        CancellationToken cancellationToken = default);
}
