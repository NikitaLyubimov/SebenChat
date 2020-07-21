using System;
using System.Collections.Generic;
using System.Text;

using Core.DTO;

namespace Core.DTO.GatewayResponces
{
    public abstract class BaseGateWayResponce
    {
        public bool Success { get; }
        public IEnumerable<Error> Errors { get; }

        protected BaseGateWayResponce(bool success = false, IEnumerable<Error> errors = null)
        {
            Success = success;
            Errors = errors;
        }


    }
}
