using Domain.Models.Yousiki;
using UseCase.Yousiki.GetYousiki1InfDetailsByCodeNo;

namespace Interactor.Yousiki
{
    public class GetYousiki1InfDetailsByCodeNoInteractor : IGetYousiki1InfDetailsByCodeNoInputPort
    {
        private readonly IYousikiRepository _yousikiRepository;

        public GetYousiki1InfDetailsByCodeNoInteractor(IYousikiRepository yousikiRepository)
        {
            _yousikiRepository = yousikiRepository;
        }

        public GetYousiki1InfDetailsByCodeNoOutputData Handle(GetYousiki1InfDetailsByCodeNoInputData inputData)
        {
            try
            {
                var result = _yousikiRepository.GetYousiki1InfDetailsByCodeNo(inputData.HpId, inputData.SinYm, inputData.PtId, inputData.DataType, inputData.SeqNo, inputData.CodeNo);
                return new GetYousiki1InfDetailsByCodeNoOutputData(result, GetYousiki1InfDetailsByCodeNoStatus.Successed);
            }
            finally
            {
                _yousikiRepository.ReleaseResource();
            }
        }
    }
}
