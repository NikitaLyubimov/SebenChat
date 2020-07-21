using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTO.GatewayResponces.Repositories
{
    public sealed class CreateUserResponce : BaseGateWayResponce
    {
        public string Id { get; }
        public CreateUserResponce(string id, bool success = false, IEnumerable<Error> errors = null) : base(success, errors)
        {
            Id = id;
        }
    }
}
