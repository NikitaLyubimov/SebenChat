using System.Net;

using Core.Interfaces;
using Core.DTO.UseCaseResponses;
using API.Serializer;


namespace API.Presenters
{
    
    public class RefreshTokenPresenter : IOutputPort<RefreshTokenResponce>
    {
        public JsonContentResult ContentResult { get; }

        public RefreshTokenPresenter()
        {
            ContentResult = new JsonContentResult();
        }

        public void Handle(RefreshTokenResponce responce)
        {
            ContentResult.StatusCode = (int)(responce.Success ? HttpStatusCode.OK : HttpStatusCode.BadRequest);
            ContentResult.Content = responce.Success ? ApiJsonSerializer.SerializeObject(responce) : ApiJsonSerializer.SerializeObject(responce.Message);
        }
    }
}
