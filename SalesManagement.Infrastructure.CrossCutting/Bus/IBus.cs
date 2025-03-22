using System.Threading.Tasks;
using MediatR;

namespace SalesManagement.Infrastructure.CrossCutting.Bus
{
    public interface IBus
    {
        Task SendCommand<T>(T command) where T : IRequest;
        Task<TResponse> SendCommand<T, TResponse>(T command) where T : IRequest<TResponse>;
        Task RaiseEvent<T>(T @event) where T : INotification;
    }
}