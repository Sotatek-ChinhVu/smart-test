using Domain.Models.Yousiki;
using UseCase.Yousiki.GetYousiki1InfModel;

namespace Interactor.Yousiki.GetYousiki1InfModel
{
    public class GetYousiki1InfModelInteractor : IGetYousiki1InfModelInputPort
    {
        private readonly IYousikiRepository _yousikiRepository;

        public GetYousiki1InfModelInteractor(IYousikiRepository yousikiRepository)
        {
            _yousikiRepository = yousikiRepository;
        }

        public GetYousiki1InfModelOutputData Handle(GetYousiki1InfModelInputData inputData)
        {
            try
            {
                var result = _yousikiRepository.GetYousiki1InfModel(inputData.HpId, inputData.SinYm, inputData.PtNum, inputData.DataType);
                return new GetYousiki1InfModelOutputData(result, GetYousiki1InfModelStatus.Successed);
            }
            finally
            {
                _yousikiRepository.ReleaseResource();
            }
        }
    }
}
