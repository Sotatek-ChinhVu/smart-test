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
                var raiinInfs = _accountingRepository.GetListRaiinInf(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.RaiinNo, true).ToList();

                if (raiinInfs.Any())
                {
                    var personNumber = raiinInfs.FirstOrDefault()?.PersonNumber ?? 0;

                    if (personNumber < 0)
                    {
                        return new GetAccountingHeaderOutputData(0, new(), GetAccountingHeaderStatus.Failed);
                    }

                    return new GetAccountingHeaderOutputData(personNumber, ConvertToDto(raiinInfs), GetAccountingHeaderStatus.Successed);
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
            var headerDtos = new List<HeaderDto>();
            foreach (var item in models)
            {
                headerDtos.Add(new HeaderDto(item.RaiinNo, item.RaiinBinding, item.PatternName));
            }
            return headerDtos;
        }
    }
}
