using System.Threading.Tasks;
using MediatR;
using SalesManagement.Infrastructure.CrossCutting.Logging;

namespace SalesManagement.Infrastructure.CrossCutting.Bus
{
    public class InMemoryBus : IBus
    {
        private readonly IMediator _mediator;
        private readonly ILogService _logService;

        public InMemoryBus(IMediator mediator, ILogService logService)
        {
            _mediator = mediator;
            _logService = logService;
        }

        public async Task SendCommand<T>(T command) where T : IRequest
        {
            await _mediator.Send(command);
        }

        public async Task<TResponse> SendCommand<T, TResponse>(T command) where T : IRequest<TResponse>
        {
            return await _mediator.Send(command);
        }

        public async Task RaiseEvent<T>(T @event) where T : INotification
        {
            await _mediator.Publish(@event);
            await LogEvent(@event);
        }

        private Task LogEvent<T>(T @event) where T : INotification
        {
            var eventName = @event.GetType().Name;
            _logService.LogInformation($"Domain Event: {eventName} - {System.Text.Json.JsonSerializer.Serialize(@event)}");
            return Task.CompletedTask;
        }
    }
}