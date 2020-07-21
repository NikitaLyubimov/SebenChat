
namespace Core.Interfaces
{
    public interface IOutputPort<in TUseCaseResponce>
    {
        void Handle(TUseCaseResponce responce);
    }
}
