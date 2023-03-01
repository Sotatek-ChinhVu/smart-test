﻿using Domain.Models.Accounting;
using Domain.Models.Diseases;
using UseCase.Accounting.GetPtByoMei;

namespace Interactor.Accounting
{
    public class GetPtByoMeiInteractor : IGetPtByoMeiInputPort
    {
        private readonly IAccountingRepository _accountingRepository;

        public GetPtByoMeiInteractor(IAccountingRepository accountingRepository)
        {
            _accountingRepository = accountingRepository;
        }

        public GetPtByoMeiOutputData Handle(GetPtByoMeiInputData inputData)
        {
            try
            {
                //Get GetPtByoMeiList
                var ptByoMei = _accountingRepository.GetPtByoMeiList(inputData.HpId, inputData.PtId, inputData.SinDate);

                if (!ptByoMei.Any())
                {
                    return new GetPtByoMeiOutputData(new(), GetPtByoMeiStatus.NoData);
                }
                return new GetPtByoMeiOutputData(ConvertToPtDiseaseDto(ptByoMei), GetPtByoMeiStatus.Successed);

            }
            finally
            {
                _accountingRepository.ReleaseResource();
            }
        }

        private List<PtDiseaseDto> ConvertToPtDiseaseDto(List<PtDiseaseModel> models)
        {
            var ptDiseaseDto = new List<PtDiseaseDto>();

            foreach (var item in models)
            {
                ptDiseaseDto.Add(new PtDiseaseDto(item.PtId, item.ByomeiCd, item.SeqNo, item.SyubyoKbn, item.SikkanKbn, item.FullByomei, item.StartDate, item.TenKiBinding, item.TenkiDate));
            }
            return ptDiseaseDto;
        }
    }
}
