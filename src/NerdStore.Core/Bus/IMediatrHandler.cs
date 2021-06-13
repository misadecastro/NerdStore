using NerdStore.Core.Messages;
using System;
using System.Threading.Tasks;

namespace NerdStore.Core.Bus
{
    public interface IMediatrHandler
    {
        Task PublicarEvento<T>(T Evento) where T : Event;
    }
}
