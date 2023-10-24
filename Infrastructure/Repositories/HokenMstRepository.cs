using Domain.Models.HokenMst;
using Domain.Models.InsuranceMst;
using Entity.Tenant;
using Helper.Common;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories
{
    public class HokenMstRepository : RepositoryBase, IHokenMstRepository
    {
        public HokenMstRepository(ITenantProvider tenantProvider) : base(tenantProvider)
        {
        }

        public HokenMasterModel GetHokenMaster(int hpId, int hokenNo, int hokenEdaNo, int prefNo, int sinDate)
        {
            var hokenMaster = NoTrackingDataContext.HokenMsts.FirstOrDefault(u => u.HpId == hpId &&
                                                                 u.HokenNo == hokenNo &&
                                                                 u.HokenEdaNo == hokenEdaNo &&
                                                                 (u.PrefNo == prefNo
                                                                 || u.IsOtherPrefValid == 1) &&
                                                                 u.StartDate <= sinDate &&
                                                                 u.EndDate >= sinDate);

            if (hokenMaster == null)
            {
                hokenMaster = NoTrackingDataContext.HokenMsts.FirstOrDefault(u => u.HpId == hpId &&
                                                                                u.HokenNo == hokenNo &&
                                                                                u.HokenEdaNo == hokenEdaNo &&
                                                                                (u.PrefNo == prefNo
                                                                                || u.IsOtherPrefValid == 1));
            }

            var result = new HokenMasterModel(hpId,
                                        hokenMaster?.HokenNo ?? 0,
                                        hokenMaster?.HokenEdaNo ?? 0,
                                        hokenMaster?.StartDate ?? 0,
                                        hokenMaster?.EndDate ?? 0,
                                        hokenMaster?.PrefNo ?? 0,
                                        hokenMaster?.Houbetu ?? string.Empty,
                                        hokenMaster?.HokenName ?? string.Empty,
                                        hokenMaster?.HokenNameCd ?? string.Empty,
                                        hokenMaster?.HokenSname ?? string.Empty,
                                        hokenMaster?.HokenSbtKbn ?? 0,
                                        hokenMaster?.HokenKohiKbn ?? 0,
                                        hokenMaster?.IsLimitList ?? 0,
                                        hokenMaster?.IsLimitListSum ?? 0,
                                        hokenMaster?.CheckDigit ?? 0,
                                        hokenMaster?.JyukyuCheckDigit ?? 0,
                                        hokenMaster?.IsFutansyaNoCheck ?? 0,
                                        hokenMaster?.IsJyukyusyaNoCheck ?? 0,
                                        hokenMaster?.IsTokusyuNoCheck ?? 0,
                                        hokenMaster?.AgeStart ?? 0,
                                        hokenMaster?.AgeEnd ?? 0,
                                        hokenMaster?.IsOtherPrefValid ?? 0,
                                        hokenMaster?.EnTen ?? 0,
                                        hokenMaster?.FutanKbn ?? 0,
                                        hokenMaster?.FutanRate ?? 0,
                                        hokenMaster?.KaiLimitFutan ?? 0,
                                        hokenMaster?.DayLimitFutan ?? 0,
                                        hokenMaster?.DayLimitCount ?? 0,
                                        hokenMaster?.MonthLimitFutan ?? 0,
                                        hokenMaster?.MonthLimitCount ?? 0,
                                        hokenMaster?.LimitKbn ?? 0,
                                        hokenMaster?.CountKbn ?? 0,
                                        hokenMaster?.CalcSpKbn ?? 0,
                                        hokenMaster?.MonthSpLimit ?? 0,
                                        hokenMaster?.KogakuTekiyo ?? 0,
                                        hokenMaster?.KogakuTotalKbn ?? 0,
                                        hokenMaster?.FutanYusen ?? 0,
                                        hokenMaster?.ReceSeikyuKbn ?? 0,
                                        hokenMaster?.ReceKisai ?? 0,
                                        hokenMaster?.ReceKisai2 ?? 0,
                                        hokenMaster?.ReceTenKisai ?? 0,
                                        hokenMaster?.ReceFutanHide ?? 0,
                                        hokenMaster?.ReceFutanRound ?? 0,
                                        hokenMaster?.ReceZeroKisai ?? 0,
                                        hokenMaster?.ReceSpKbn ?? 0,
                                        string.Empty);

            string? roudou = NoTrackingDataContext.RoudouMsts.FirstOrDefault(u => u.RoudouCd == result.PrefNo.ToString())?.RoudouName;
            if (!string.IsNullOrEmpty(roudou))
                result.Roudou = "(" + roudou + ")";

            return result;
        }
        public List<HokenMasterModel> CheckExistHokenEdaNo(int hokenNo, int hpId)
        {
            var existHokenEdaNo = NoTrackingDataContext.HokenMsts
               .Where(x => x.HpId == hpId && x.HokenNo == hokenNo)
               .Select(x => new HokenMasterModel(
                       x.HokenNo,
                       x.HokenEdaNo
               )).ToList();
            return existHokenEdaNo;
        }

        public List<HokenMstModel> FindHokenMst(int hpId)
        {
            List<HokenMstModel> result = new List<HokenMstModel>();
            int dateTimeNow = CIUtil.DateTimeToInt(CIUtil.GetJapanDateTimeNow());
            var hospitalInfo = NoTrackingDataContext.HpInfs.Where(x => x.HpId == hpId).OrderByDescending(x => x.StartDate).FirstOrDefault(x => x.StartDate <= dateTimeNow);
            if (hospitalInfo == null)
            {
                hospitalInfo = NoTrackingDataContext.HpInfs.Where(x => x.HpId == hpId).OrderByDescending(x => x.StartDate).FirstOrDefault();
            }

            int PrefCd = hospitalInfo == null ? 0 : hospitalInfo.PrefNo;

            List<HokenMst> entities = NoTrackingDataContext.HokenMsts
                .Where(
                    entity => entity.HpId == hpId
                    && (entity.PrefNo == PrefCd
                        || entity.PrefNo == 0
                        || entity.IsOtherPrefValid == 1))
                .OrderBy(e => e.HpId)
                .ThenBy(e => e.HokenNo)
                .ThenByDescending(e => e.PrefNo)
                .ThenBy(e => e.SortNo)
            .ThenByDescending(e => e.StartDate)
            .ToList();

            List<RoudouMst> roudouMsts = NoTrackingDataContext.RoudouMsts.ToList();
            entities?.ForEach(entity =>
            {
                string? prefName = roudouMsts.FirstOrDefault(roudou => roudou.RoudouCd == entity.PrefNo.ToString())?.RoudouName;
                result.Add(new HokenMstModel(
                          entity.FutanKbn,
                          entity.FutanRate,
                          entity.StartDate,
                          entity.EndDate,
                          entity.HokenNo,
                          entity.HokenEdaNo,
                          entity.HokenSname ?? string.Empty,
                          entity.Houbetu ?? string.Empty,
                          entity.HokenSbtKbn,
                          entity.CheckDigit,
                          entity.AgeStart,
                          entity.AgeEnd,
                          entity.IsFutansyaNoCheck,
                          entity.IsJyukyusyaNoCheck,
                          entity.JyukyuCheckDigit,
                          entity.IsTokusyuNoCheck,
                          entity.HokenName ?? string.Empty,
                          entity.HokenNameCd ?? string.Empty,
                          entity.HokenKohiKbn,
                          entity.IsOtherPrefValid,
                          entity.ReceKisai,
                          entity.IsLimitList,
                          entity.IsLimitListSum,
                          entity.EnTen,
                          entity.KaiLimitFutan,
                          entity.DayLimitFutan,
                          entity.MonthLimitFutan,
                          entity.MonthLimitCount,
                          entity.LimitKbn,
                          entity.CountKbn,
                          entity.FutanYusen,
                          entity.CalcSpKbn,
                          entity.MonthSpLimit,
                          entity.KogakuTekiyo,
                          entity.KogakuTotalKbn,
                          entity.KogakuHairyoKbn,
                          entity.ReceSeikyuKbn,
                          entity.ReceKisaiKokho,
                          entity.ReceKisai2,
                          entity.ReceTenKisai,
                          entity.ReceFutanRound,
                          entity.ReceZeroKisai,
                          entity.ReceSpKbn,
                          prefName ?? string.Empty,
                          entity.PrefNo,
                          entity.SortNo,
                          entity.SeikyuYm,
                          entity.ReceFutanHide,
                          entity.ReceFutanKbn,
                          entity.KogakuTotalAll,
                          true,
                          entity.DayLimitCount,
                          new List<ExceptHokensyaModel>()
                    ));
            });

            return result;
        }

        public void ReleaseResource()
        {
            DisposeDataContext();
        }

    }
}
