using Domain.Models.HokenMst;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class HokenMstRepository : IHokenMstRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public HokenMstRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public HokenMasterModel GetHokenMaster(int hpId, int hokenNo, int hokenEdaNo, int prefNo, int sinDate)
        {
            var hokenMaster = _tenantDataContext.HokenMsts.FirstOrDefault(u => u.HpId == hpId &&
                                                                 u.HokenNo == hokenNo &&
                                                                 u.HokenEdaNo == hokenEdaNo &&
                                                                 (u.PrefNo == prefNo
                                                                 || u.IsOtherPrefValid == 1) &&
                                                                 u.StartDate <= sinDate &&
                                                                 u.EndDate >= sinDate);

            if (hokenMaster == null)
            {
                hokenMaster = _tenantDataContext.HokenMsts.FirstOrDefault(u => u.HpId == hpId &&
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

            string? roudou = _tenantDataContext.RoudouMsts.FirstOrDefault(u => u.RoudouCd == result.PrefNo.ToString())?.RoudouName;
            if (!string.IsNullOrEmpty(roudou))
                result.Roudou = "(" + roudou + ")";

            return result;
        }
        public List<HokenMasterModel> CheckExistHokenEdaNo(int hokenNo)
        {
            var existHokenEdaNo = _tenantDataContext.HokenMsts
               .Where(x => x.HpId == TempIdentity.HpId && x.HokenNo == hokenNo)
               .Select(x => new HokenMasterModel(
                   x.HpId,
                   x.HokenNo,
                   x.HokenEdaNo,
                   0, 0, 0, String.Empty, String.Empty, String.Empty, String.Empty,
                   0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, String.Empty
                   ))
               .ToList();

            return existHokenEdaNo;
        }
    }
}
