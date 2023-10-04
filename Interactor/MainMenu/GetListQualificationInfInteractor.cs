using Domain.Models.RsvInf;
using UseCase.MainMenu.GetListQualification;

namespace Interactor.MainMenu
{
    public class GetListQualificationInfInteractor : IGetListQualificationInfInputPort
    {
        private readonly IRsvInfRepository _rsvInfRepository;

        public GetListQualificationInfInteractor(IRsvInfRepository rsvInfRepository)
        {
            _rsvInfRepository = rsvInfRepository;
        }

        public GetListQualificationInfOutputData Handle(GetListQualificationInfInputData inputData)
        {
            try
            {
                var result = _rsvInfRepository.GetListQualificationInf();
                return new GetListQualificationInfOutputData(result, result.Any() ? GetListQualificationInfStatus.Successed
                                                                                  : GetListQualificationInfStatus.NoData);
            }
            finally
            {
                _rsvInfRepository.ReleaseResource();
            }
        }
    }
}
