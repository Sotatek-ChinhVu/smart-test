using Domain.Models.RsvInf;
using UseCase.MainMenu.RsvInfToConfirm;

namespace Interactor.MainMenu
{
    public class GetRsvInfToConfirmInteractor : IGetRsvInfToConfirmInputPort
    {
        private readonly IRsvInfRepository _rsvInfRepository;

        public GetRsvInfToConfirmInteractor(IRsvInfRepository rsvInfRepository)
        {
            _rsvInfRepository = rsvInfRepository;
        }

        public GetRsvInfToConfirmOutputData Handle(GetRsvInfToConfirmInputData inputData)
        {
            try
            {
                var rsvToConfirms = _rsvInfRepository.GetListRsvInfToConfirmModel(inputData.HpId, inputData.SinDate);

                return new GetRsvInfToConfirmOutputData(rsvToConfirms, rsvToConfirms.Any()
                                                        ? GetRsvInfToConfirmStatus.Successed
                                                        : GetRsvInfToConfirmStatus.NoData);
            }
            finally
            {
                _rsvInfRepository.ReleaseResource();
            }
        }
    }
}
