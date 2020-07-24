using Core.Interfaces;
using Core.DTO.UseCaseResponses;
using API.Serializer;
using System.Net;

namespace API.Presenters
{
    public class VerifyEmailTokenPresenter : IOutputPort<VerifyEmailTokenResponce>
    {
        public JsonContentResult ContentResult { get; }

        public VerifyEmailTokenPresenter()
        {
            ContentResult = new JsonContentResult();
        }
        public void Handle(VerifyEmailTokenResponce responce)
        {
            ContentResult.StatusCode = (int)(responce.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            ContentResult.Content = ApiJsonSerializer.SerializeObject(responce);
        }


    }
}
