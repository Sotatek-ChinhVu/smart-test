using Domain.Models.ReceSeikyu;
using UseCase.ReceSeikyu.GetList;

namespace Interactor.ReceSeikyu
{
    public class GetListReceSeikyuInteractor : IGetListReceSeikyuInputPort
    {
        private readonly IReceSeikyuRepository _receSeikyuRepository;

        public GetListReceSeikyuInteractor(IReceSeikyuRepository receptionRepository)
        {
            _receSeikyuRepository = receptionRepository;
        }

        public GetListReceSeikyuOutputData Handle(GetListReceSeikyuInputData inputData)
        {
            try
            {
                var result = new List<ReceSeikyuModel>();
                if (inputData.HpId <= 0)
                    return new GetListReceSeikyuOutputData(GetListReceSeikyuStatus.InvalidHpId, result);

                var data = _receSeikyuRepository.GetListReceSeikyModel(inputData.HpId, 
                                                                       inputData.SinDate, 
                                                                       inputData.SinYm,
                                                                       inputData.IsIncludingUnConfirmed, 
                                                                       inputData.PtNumSearch, 
                                                                       inputData.NoFilter, 
                                                                       inputData.IsFilterMonthlyDelay,
                                                                       inputData.IsFilterReturn, 
                                                                       inputData.IsFilterOnlineReturn);

                if(data.Any())
                    return new GetListReceSeikyuOutputData(GetListReceSeikyuStatus.Successful, data);
                else
                    return new GetListReceSeikyuOutputData(GetListReceSeikyuStatus.NoData, data);
            }
            finally
            {
                _receSeikyuRepository.ReleaseResource();
            }
        }
    }
}
