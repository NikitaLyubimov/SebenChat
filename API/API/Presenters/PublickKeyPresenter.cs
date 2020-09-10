using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Interfaces;
using Core.DTO.UseCaseResponses;
using API.Serializer;
using System.Net;

namespace API.Presenters
{
    public class PublickKeyPresenter : IOutputPort<PublickKeyResponce>
    {

        public JsonContentResult ContentResult { get; }
        public PublickKeyPresenter()
        {
            ContentResult = new JsonContentResult();
        }
        public void Handle(PublickKeyResponce responce)
        {
            ContentResult.StatusCode = (int)(responce.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            ContentResult.Content = ApiJsonSerializer.SerializeObject(responce);
        }
    }
}
