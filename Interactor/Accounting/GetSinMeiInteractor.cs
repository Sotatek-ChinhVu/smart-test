using Domain.Models.Accounting;
using EmrCalculateApi.Interface;
using EmrCalculateApi.Receipt.Constants;
using EmrCalculateApi.Receipt.ViewModels;
using Helper.Extension;
using Infrastructure.Interfaces;
using UseCase.Accounting.GetSinMei;

namespace Interactor.Accounting
{
    public class GetSinMeiInteractor : IGetSinMeiInputPort
    {
        private readonly IAccountingRepository _accountingRepository;
        private readonly ITenantProvider _tenantProvider;
        private readonly ISystemConfigProvider _systemConfigProvider;
        private readonly IEmrLogger _emrLogger;

        public GetSinMeiInteractor(IAccountingRepository accountingRepository, ITenantProvider tenantProvider, ISystemConfigProvider systemConfigProvider, IEmrLogger emrLogger)
        {
            _accountingRepository = accountingRepository;
            _tenantProvider = tenantProvider;
            _systemConfigProvider = systemConfigProvider;
            _emrLogger = emrLogger;
        }

        public GetSinMeiOutputData Handle(GetSinMeiInputData inputData)
        {
            try
            {
                var raiinNos = _accountingRepository.GetRaiinNos(inputData.HpId, inputData.PtId, inputData.RaiinNo);

                if (!raiinNos.Any()) { return new GetSinMeiOutputData(new(), GetSinMeiStatus.NoData); }

                var sinMeiVm = new SinMeiViewModel(SinMeiMode.Kaikei, includeOutDrg: false, inputData.HpId, inputData.PtId,
                                                    inputData.SinDate, raiinNos, _tenantProvider, _systemConfigProvider, _emrLogger);

                var sinMei = sinMeiVm.SinMei.Select(item => new SinMeiModel(
                                                                            item.SinId,
                                                                            string.Empty,
                                                                            item.ItemName,
                                                                            item.Suryo,
                                                                            item.UnitName,
                                                                            item.TenKai,
                                                                            item.TotalTen,
                                                                            item.TotalKingaku,
                                                                            item.Kingaku,
                                                                            item.FutanS,
                                                                            item.FutanK1,
                                                                            item.FutanK2,
                                                                            item.FutanK3,
                                                                            item.FutanK4,
                                                                            item.CdKbn,
                                                                            item.JihiSbt,
                                                                            item.EnTenKbn,
                                                                            item.SanteiKbn,
                                                                            item.InOutKbn,
                                                                            false
                                                                )).ToList();

                var editSinMei = EditSinMei(sinMei);

                return new GetSinMeiOutputData(editSinMei, GetSinMeiStatus.Successed);

            }
            finally
            {
                _accountingRepository.ReleaseResource();
            }
        }

        private List<SinMeiModel> EditSinMei(List<SinMeiModel> listSinMei)
        {
            if (listSinMei == null || listSinMei.Count <= 0) return new();

            int oldSinId = 0;

            for (int i = 0; i < listSinMei.Count; i++)
            {
                if (listSinMei[i].SinId != 0 && listSinMei[i].SinId != oldSinId)
                {
                    oldSinId = listSinMei[i].SinId;
                    listSinMei[i].SinIdBinding = oldSinId.AsString();
                }

                if (i == listSinMei.Count - 1) continue;

                if (listSinMei[i + 1].SinId != 0 && listSinMei[i + 1].SinId != oldSinId)
                {
                    listSinMei.Insert(i + 1, new SinMeiModel(listSinMei[i].SinId,
                                                            listSinMei[i].SinIdBinding,
                                                            listSinMei[i].ItemName,
                                                            listSinMei[i].Suryo,
                                                            listSinMei[i].UnitName,
                                                            listSinMei[i].TenKai,
                                                            listSinMei[i].TotalTen,
                                                            listSinMei[i].TotalKingaku,
                                                            listSinMei[i].Kingaku,
                                                            listSinMei[i].FutanS,
                                                            listSinMei[i].FutanK1,
                                                            listSinMei[i].FutanK2,
                                                            listSinMei[i].FutanK3,
                                                            listSinMei[i].FutanK4,
                                                            listSinMei[i].CdKbn,
                                                            listSinMei[i].JihiSbt,
                                                            listSinMei[i].EnTenKbn,
                                                            listSinMei[i].SanteiKbn,
                                                            listSinMei[i].InOutKbn,
                                                            true));
                    i++;
                }
            }

            return listSinMei;
        }


    }
}
