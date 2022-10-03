using Domain.Models.Reception;
using UseCase.Reception.Get;

namespace Interactor.Reception
{
    public class GetReceptionInteractor : IGetReceptionInputPort
    {
        private readonly IReceptionRepository _receptionRepository;
        public GetReceptionInteractor(IReceptionRepository receptionRepository)
        {
            _receptionRepository = receptionRepository;
        }

        public GetReceptionOutputData Handle(GetReceptionInputData inputData)
        {
            if (inputData.RaiinNo <= 0)
            {
                return new GetReceptionOutputData(new ReceptionModel(), GetReceptionStatus.InvalidRaiinNo);
            }

            var receptionModel = _receptionRepository.Get(inputData.RaiinNo);
            if (receptionModel.HpId == 0 && receptionModel.PtId == 0 && receptionModel.SinDate == 0 && receptionModel.RaiinNo == 0)
            {
                return new GetReceptionOutputData(new ReceptionModel(), GetReceptionStatus.ReceptionNotExisted);
            }

            return new GetReceptionOutputData(receptionModel, GetReceptionStatus.Successed);
        }
    }
}
