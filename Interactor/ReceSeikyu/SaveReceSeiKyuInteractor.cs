using Domain.Models.ReceSeikyu;
using UseCase.ReceSeikyu.GetList;
using UseCase.ReceSeikyu.Save;

namespace Interactor.ReceSeikyu
{
    public class SaveReceSeiKyuInteractor : ISaveReceSeiKyuInputPort
    {
        private readonly IReceSeikyuRepository _receSeikyuRepository;

        public SaveReceSeiKyuInteractor(IReceSeikyuRepository receptionRepository)
        {
            _receSeikyuRepository = receptionRepository;
        }

        public SaveReceSeiKyuOutputData Handle(SaveReceSeiKyuInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                    return new SaveReceSeiKyuOutputData(SaveReceSeiKyuStatus.InvalidHpId, string.Empty);

                if(inputData.UserAct <= 0)
                    return new SaveReceSeiKyuOutputData(SaveReceSeiKyuStatus.InvalidUserId, string.Empty);


            }
            finally
            {
                _receSeikyuRepository.ReleaseResource();
            }
        }
    }
}
