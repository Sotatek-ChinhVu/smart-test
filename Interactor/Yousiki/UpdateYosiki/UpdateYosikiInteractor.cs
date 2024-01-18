using Domain.Models.Yousiki;
using UseCase.Yousiki.UpdateYosiki;

namespace Interactor.Yousiki.UpdateYosiki
{
    public class UpdateYosikiInteractor : IUpdateYosikiInputPort
    {
        private readonly IYousikiRepository _yousikiRepository;

        public UpdateYosikiInteractor(IYousikiRepository yousikiRepository)
        {
            _yousikiRepository = yousikiRepository;
        }
        public UpdateYosikiOutputData Handle(UpdateYosikiInputData inputData)
        {
            throw new NotImplementedException();
        }
    }
}
