using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUseCaseRequestHandler<in TUseCaseRequest, out TUseCaseResponce>
        where TUseCaseRequest : IUseCaseRequest<TUseCaseResponce>
    {
        Task<bool> Handle(TUseCaseRequest message, IOutputPort<TUseCaseResponce> outputPort);
    }
}
