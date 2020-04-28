using System;
using System.Collections.Generic;
using System.Text;

namespace DataBase.DTO
{
    public class CreateUserResponce : BaseResponce
    {
        public string Id { get; set; }

        public CreateUserResponce(string id, bool success = false, IEnumerable<Error> errors = null) : base(success, errors)
        {
            Id = id;
        }
    }
}
