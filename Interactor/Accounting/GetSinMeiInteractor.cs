using Domain.Models.Accounting;
using Domain.Models.MstItem;
using EmrCalculateApi.Interface;
using EmrCalculateApi.Receipt.Constants;
using EmrCalculateApi.Receipt.ViewModels;
using Helper.Constants;
using Helper.Enum;
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

                if (!raiinNos.Any()) { return new GetSinMeiOutputData(new(), new(), new(), GetSinMeiStatus.NoData); }

                var sinMei = GetSinMei(inputData.HpId, inputData.PtId, inputData.SinDate, raiinNos);

                var sinHo = GetSinHo(sinMei);

                var sinGai = GetSinGai(inputData.HpId, inputData.PtId, raiinNos, sinMei);

                return new GetSinMeiOutputData(sinMei, sinHo, sinGai, GetSinMeiStatus.Successed);

            }
            finally
            {
                _accountingRepository.ReleaseResource();
            }
        }

        private List<SinMeiModel> GetSinMei(int hpId, long ptId, int sinDate, List<long> raiinNos)
        {
            var sinMeiVm = new SinMeiViewModel(SinMeiMode.Kaikei, includeOutDrg: false, hpId, ptId,
                                                    sinDate, raiinNos, _tenantProvider, _systemConfigProvider, _emrLogger);

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

            return EditSinMei(sinMei);
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

        private List<SinHoModel> GetSinHo(List<SinMeiModel> sinMeiModels)
        {
            var sinHo = new List<SinHoModel>();
            var codeCount = Enum.GetNames(typeof(CodeHoEnum)).Length;

            for (int i = 0; i < codeCount - 4; i++)
            {
                var codeHo = (CodeHoEnum)i;
                var sinMeiCode = sinMeiModels.Where(item => item.CdKbn == codeHo.AsString());
                sinHo.Add(new SinHoModel(
                                        codeHo.AsString(),
                                        sinMeiCode.Sum(item => item.SinHoTotalTen).AsDouble(),
                                        sinMeiCode.Where(item => item.FutanK1 == 1).Sum(item => item.SinHoTotalTen).AsDouble(),
                                        sinMeiCode.Where(item => item.FutanK2 == 1).Sum(item => item.SinHoTotalTen).AsDouble(),
                                        sinMeiCode.Where(item => item.FutanK3 == 1).Sum(item => item.SinHoTotalTen).AsDouble(),
                                        sinMeiCode.Where(item => item.FutanK4 == 1).Sum(item => item.SinHoTotalTen).AsDouble(),
                                        false));
            }

            var hoAll = new SinHoModel(
                                        CodeHoEnum.ALL.AsString(),
                                        sinHo.Sum(item => item.Point).AsDouble(),
                                        sinHo.Sum(item => item.PointKohi1).AsDouble(),
                                        sinHo.Sum(item => item.PointKohi2).AsDouble(),
                                        sinHo.Sum(item => item.PointKohi3).AsDouble(),
                                        sinHo.Sum(item => item.PointKohi4).AsDouble(),
                                        true);
            sinHo.Add(hoAll);

            return sinHo;
        }

        private List<SinGaiModel> GetSinGai(int hpId, long ptId, List<long> raiinNos, List<SinMeiModel> sinMeiModels)
        {
            var sinGai = new List<SinGaiModel>();
            var jihis = GetListJihiSbtMst(hpId);

            if (jihis != null && jihis.Count > 0)
            {
                for (int i = 0; i < jihis.Count; i++)
                {
                    sinGai.Add(new SinGaiModel(jihis[i].Name,
                                                sinMeiModels.Where(item => item.JihiSbt == jihis[i].JihiSbt)
                                                .Sum(item => item.TotalKingaku).AsInteger(),
                                                false));
                }
            }

            sinGai.Add(new SinGaiModel("自費",
                                        sinMeiModels.Where(item => item.JihiSbt == 0 && item.SanteiKbn == 2).Sum(item => item.TotalKingaku).AsInteger(),
                                        false));

            sinGai.Add(new SinGaiModel("合計金額",
                                        //自費項目 --> item.JihiSbt = 0
                                        //自費算定項目 --> item.JihiSbt == 0 && item.SanteiKbn == 2
                                        sinMeiModels.Where(item => item.JihiSbt > 0 || (item.JihiSbt == 0 && item.SanteiKbn == 2)).Sum(item => item.TotalKingaku)
                                            .AsInteger(),
                                        true));

            int point = _accountingRepository.GetJihiOuttaxPoint(hpId, ptId, raiinNos);

            sinGai.Add(new SinGaiModel(SinHoConstant.CodeHoDic[nameof(CodeHoEnum.SZ)],
                                       point,
                                       true));

            return sinGai;
        }

        private List<JihiSbtMstModel> GetListJihiSbtMst(int hpId)
        {
            return _accountingRepository.GetListJihiSbtMst(hpId);
        }
    }
}
