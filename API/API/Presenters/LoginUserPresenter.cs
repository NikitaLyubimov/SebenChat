using System.Net;


using Core.Interfaces;
using Core.DTO.UseCaseResponses;
using API.Serializer;


namespace API.Presenters
{
    public class LoginUserPresenter : IOutputPort<LoginResponce>
    {
        public JsonContentResult ContentResult { get; }

        public LoginUserPresenter()
        {
            ContentResult = new JsonContentResult();
        }

        public void Handle(LoginResponce responce)
        {
            ContentResult.StatusCode = (int)(responce.Success ? HttpStatusCode.OK : HttpStatusCode.Unauthorized);
            ContentResult.Content = responce.Success ? ApiJsonSerializer.SerializeObject(responce) : ApiJsonSerializer.SerializeObject(responce.Errors);
        }
    }
}
