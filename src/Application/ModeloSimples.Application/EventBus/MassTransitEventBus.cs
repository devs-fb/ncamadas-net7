﻿namespace ModeloSimples.Application.EventBus;

using MassTransit;
using ModeloSimples.Infrastructure.Shared.Interfaces;
using System.Reflection;

public class MassTransitEventBus : IEventBus
{
    private const string MethodPublish = "Publish";

    private readonly IPublishEndpoint _publishEndpoint;

    public MassTransitEventBus(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task Publish<T>(T message) where T : class
    {
        await _publishEndpoint.Publish(message);
    }

    public async Task PublishRange(IEnumerable<IEvent> messages)
    {
        foreach (var message in messages)
        {
            var publishMethod = typeof(MassTransitEventBus)
                .GetMethod(MethodPublish, BindingFlags.Instance | BindingFlags.Public)
                .MakeGenericMethod(message.GetType());

            await (Task)publishMethod.Invoke(this, new[] { message });
        }
    }
}
