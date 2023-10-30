using Domain.Models.Online;
using UseCase.MainMenu.GetListQualification;

namespace Interactor.MainMenu
{
    public class GetListQualificationInfInteractor : IGetListQualificationInfInputPort
    {
        private readonly IOnlineRepository _onlineRepository;

        public GetListQualificationInfInteractor(IOnlineRepository onlineRepository)
        {
            _onlineRepository = onlineRepository;
        }

        public GetListQualificationInfOutputData Handle(GetListQualificationInfInputData inputData)
        {
            try
            {
                var result = _onlineRepository.GetListQualificationInf();
                return new GetListQualificationInfOutputData(result, result.Any() ? GetListQualificationInfStatus.Successed
                                                                                  : GetListQualificationInfStatus.NoData);
            }
            finally
            {
                _onlineRepository.ReleaseResource();
            }
        }
    }
}
