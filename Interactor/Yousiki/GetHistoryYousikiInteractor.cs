using Domain.Models.Yousiki;
using UseCase.Yousiki.GetHistoryYousiki;

namespace Interactor.Yousiki
{
    public class GetHistoryYousikiInteractor : IGetHistoryYousikiInputPort
    {
        private readonly IYousikiRepository _yousikiRepository;

        public GetHistoryYousikiInteractor(IYousikiRepository yousikiRepository)
        {
            _yousikiRepository = yousikiRepository;
        }
        public GetHistoryYousikiOutputData Handle(GetHistoryYousikiInputData inputData)
        {
            try
            {
                var result = _yousikiRepository.GetHistoryYousiki(inputData.HpId, inputData.SinYm, inputData.PtId, inputData.DataType);

                return new GetHistoryYousikiOutputData(result, GetHistoryYousikiStatus.Successed);
            }
            finally
            {
                _yousikiRepository.ReleaseResource();
            }
        }
    }
}
