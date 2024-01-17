using Domain.Models.Yousiki;
using UseCase.Yousiki.GetKacodeYousikiMstDict;

namespace Interactor.Yousiki.GetKacodeYousikiMstDict
{
    public class GetKacodeYousikiMstDictInteractor : IGetKacodeYousikiMstDictInputPort
    {
        private readonly IYousikiRepository _yousikiRepository;

        public GetKacodeYousikiMstDictInteractor(IYousikiRepository yousikiRepository)
        {
            _yousikiRepository = yousikiRepository;
        }
        public GetKacodeYousikiMstDictOutputData Handle(GetKacodeYousikiMstDictInputData inputData)
        {
            try
            {
                var result = _yousikiRepository.GetKacodeYousikiMstDict();

                return new GetKacodeYousikiMstDictOutputData(result, GetKacodeYousikiMstDictStatus.Successed);
            }
            finally
            {
                _yousikiRepository.ReleaseResource();
            }
        }
    }
}
