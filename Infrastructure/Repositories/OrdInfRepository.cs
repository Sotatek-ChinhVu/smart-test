using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Entity.Tenant;
using Infrastructure.Interfaces;
using PostgreDataContext;

namespace Infrastructure.Repositories
{
    public class OrdInfRepository : IOrdInfRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public OrdInfRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public void Create(OrdInfModel ord)
        {
            throw new NotImplementedException();
        }

        public void Delete(int ordId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<OrdInfModel> GetList(long ptId, long raiinNo, int sinDate, bool isDeleted)
        {
            var result = new List<OrdInfModel>();
            var allOdrInfDetails = _tenantDataContext.OdrInfDetails.Where(o => o.PtId == ptId && o.RaiinNo == raiinNo && o.SinDate == sinDate)?.ToList();
            var allOdrInf = _tenantDataContext.OdrInfs.Where(odr => odr.PtId == ptId && odr.RaiinNo == raiinNo && odr.SinDate == sinDate && odr.OdrKouiKbn != 10 && (isDeleted || odr.IsDeleted == 0)).ToList();

            foreach (var rpOdrInf in allOdrInf)
            {
                var odrInfDetails = allOdrInfDetails?.Where(detail => detail.RpNo == rpOdrInf.RpNo && detail.RpEdaNo == rpOdrInf.RpEdaNo)
                    .ToList();
                var ordInf = ConvertToModel(rpOdrInf, odrInfDetails);
                result.Add(ordInf);
            }
            return result;
        }

        public IEnumerable<OrdInfModel> GetList(long ptId, int hpId, long raiinNo)
        {
            var result = new List<OrdInfModel>();
            var allOdrInfDetails = _tenantDataContext.OdrInfDetails.Where(o => o.PtId == ptId && o.HpId == hpId && o.RaiinNo == raiinNo)?.ToList();
            var allOdrInf = _tenantDataContext.OdrInfs.Where(odr => odr.PtId == ptId && odr.HpId == hpId && odr.OdrKouiKbn != 10 && odr.RaiinNo == raiinNo).ToList();

            foreach (var rpOdrInf in allOdrInf)
            {
                var odrInfDetails = allOdrInfDetails?.Where(detail => detail.RpNo == rpOdrInf.RpNo && detail.RpEdaNo == rpOdrInf.RpEdaNo)
                    .ToList();
                var ordInf = ConvertToModel(rpOdrInf, odrInfDetails);
                result.Add(ordInf);
            }
            return result;
        }

        private OrdInfModel ConvertToModel(OdrInf ordInf, List<OdrInfDetail>? ordInfDetails)
        {
            return new OrdInfModel(ordInf.HpId,
                        ordInf.RaiinNo,
                        ordInf.RpNo,
                        ordInf.RpEdaNo,
                        ordInf.PtId,
                        ordInf.SinDate,
                        ordInf.HokenPid,
                        ordInf.OdrKouiKbn,
                        ordInf.RpName ?? string.Empty,
                        ordInf.InoutKbn,
                        ordInf.SikyuKbn,
                        ordInf.SyohoSbt,
                        ordInf.SanteiKbn,
                        ordInf.TosekiKbn,
                        ordInf.DaysCnt,
                        ordInf.SortNo,
                        ordInf.IsDeleted,
                        ordInf.Id,
                        ordInfDetails == null ? new List<OrdInfDetailModel>()
                        : ordInfDetails.Select(od => ConvertToDetailModel(od)).ToList()
                   );
        }

        private OrdInfDetailModel ConvertToDetailModel(OdrInfDetail ordInfDetail)
        {
            return new OrdInfDetailModel(
                            ordInfDetail.HpId,
                            ordInfDetail.RaiinNo,
                            ordInfDetail.RpNo,
                            ordInfDetail.RpEdaNo,
                            ordInfDetail.RowNo,
                            ordInfDetail.PtId,
                            ordInfDetail.SinDate,
                            ordInfDetail.SinKouiKbn,
                            ordInfDetail.ItemCd ?? string.Empty,
                            ordInfDetail.ItemName ?? string.Empty,
                            ordInfDetail.Suryo,
                            ordInfDetail.UnitName ?? string.Empty,
                            ordInfDetail.UnitSBT,
                            ordInfDetail.TermVal,
                            ordInfDetail.KohatuKbn,
                            ordInfDetail.SyohoKbn,
                            ordInfDetail.SyohoLimitKbn,
                            ordInfDetail.DrugKbn,
                            ordInfDetail.YohoKbn,
                            ordInfDetail.Kokuji1 ?? string.Empty,
                            ordInfDetail.Kokiji2 ?? string.Empty,
                            ordInfDetail.IsNodspRece,
                            ordInfDetail.IpnCd ?? string.Empty,
                            ordInfDetail.IpnName ?? string.Empty,
                            ordInfDetail.JissiKbn,
                            ordInfDetail.JissiDate ?? DateTime.MinValue,
                            ordInfDetail.JissiId,
                            ordInfDetail.JissiMachine ?? string.Empty,
                            ordInfDetail.ReqCd ?? string.Empty,
                            ordInfDetail.Bunkatu ?? string.Empty,
                            ordInfDetail.CmtName ?? string.Empty,
                            ordInfDetail.CmtOpt ?? string.Empty,
                            ordInfDetail.FontColor ?? string.Empty,
                            ordInfDetail.CommentNewline,
                            "",
                            0,
                            0,
                            false,
                            0,
                            0
                );
        }

        public OrdInfModel Read(int ordId)
        {
            throw new NotImplementedException();
        }

        public void Update(OrdInfModel ord)
        {
            throw new NotImplementedException();
        }
    }
}
