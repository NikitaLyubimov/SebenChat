using System.Net;


using Core.Interfaces;
using Core.DTO.UseCaseResponses;
using API.Serializer;


namespace API.Presenters
{
    public sealed class RegisterUserPresenter : IOutputPort<RegisterUserResponce>
    {
        public JsonContentResult ContentResult { get; }

        public RegisterUserPresenter()
        {
            ContentResult = new JsonContentResult();
        }
        public void Handle(RegisterUserResponce responce)
        {
            ContentResult.StatusCode = (int)(responce.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            ContentResult.Content = ApiJsonSerializer.SerializeObject(responce);
        }


    }
}
