using Domain.Models.Accounting;
using Domain.Models.Reception;
using UseCase.Accounting.GetAccountingHeader;

namespace Interactor.Accounting
{
    public class GetAccountingHeaderInteractor : IGetAccountingHeaderInputPort
    {
        private readonly IAccountingRepository _accountingRepository;

        public GetAccountingHeaderInteractor(IAccountingRepository accountingRepository)
        {
            _accountingRepository = accountingRepository;
        }

        public GetAccountingHeaderOutputData Handle(GetAccountingHeaderInputData inputData)
        {
            try
            {
                var raiinInf = _accountingRepository.GetListRaiinInf(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo, true).ToList();

                if (raiinInf.Any())
                {
                    var personNumber = raiinInf.FirstOrDefault(x => x.PersonNumber != null)?.PersonNumber ?? 0;
                    return new GetAccountingHeaderOutputData(personNumber, ConvertToDto(raiinInf), GetAccountingHeaderStatus.Successed);
                }

                return new GetAccountingHeaderOutputData(0, new(), GetAccountingHeaderStatus.NoData);
            }
            finally
            {
                _accountingRepository.ReleaseResource();
            }
        }

        private List<HeaderDto> ConvertToDto(List<ReceptionDto> models)
        {
            List<HeaderDto> headerDtos = new List<HeaderDto>();
            foreach (var item in models)
            {
                headerDtos.Add(new HeaderDto(item.RaiinNo, item.RaiinBinding, item.PatternName));
            }
            return headerDtos;
        }
    }
}
