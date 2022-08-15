﻿using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
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
            var allOdrInf = _tenantDataContext.OdrInfs.Where(odr => odr.PtId == ptId && odr.RaiinNo == raiinNo && odr.SinDate == sinDate && odr.OdrKouiKbn != 10 && (isDeleted || odr.IsDeleted == 0)).Select(o => new OrdInfModel(
                o.HpId, o.RaiinNo, o.RpNo, o.RpEdaNo, o.PtId, o.SinDate, o.HokenPid, o.OdrKouiKbn, o.RpName, o.InoutKbn, o.SikyuKbn, o.SyohoSbt, o.SanteiKbn, o.TosekiKbn, o.DaysCnt, o.SortNo, o.IsDeleted, o.Id, new List<OrdInfDetailModel>()
                  )).ToList();

            foreach (OrdInfModel rpOdrInf in allOdrInf)
            {
                // Find OdrInfDetail
                var odrInfDetails = allOdrInfDetails?.Where(detail => detail.RpNo == rpOdrInf.RpNo && detail.RpEdaNo == rpOdrInf.RpEdaNo)
                    .ToList();
                var ordInf = new OrdInfModel(rpOdrInf.HpId, rpOdrInf.RaiinNo, rpOdrInf.RpNo, rpOdrInf.RpEdaNo, rpOdrInf.PtId, rpOdrInf.SinDate, rpOdrInf.HokenPid, rpOdrInf.OdrKouiKbn, rpOdrInf.RpName, rpOdrInf.InoutKbn, rpOdrInf.SikyuKbn, rpOdrInf.SyohoSbt, rpOdrInf.SanteiKbn, rpOdrInf.TosekiKbn, rpOdrInf.DaysCnt, rpOdrInf.SortNo, rpOdrInf.IsDeleted, rpOdrInf.Id,
                  odrInfDetails == null ? new List<OrdInfDetailModel>() : odrInfDetails.Select(od => new OrdInfDetailModel(od.HpId, od.RaiinNo, od.RpNo, od.RpEdaNo, od.RowNo, od.PtId, od.SinDate, od.SinKouiKbn, od.ItemCd, od.ItemName, od.Suryo, od.UnitName, od.UnitSBT, od.TermVal, od.KohatuKbn, od.SyohoKbn, od.SyohoLimitKbn, od.DrugKbn, od.YohoKbn, od.Kokuji1, od.Kokiji2, od.IsNodspRece, od.IpnCd, od.IpnName, od.JissiKbn, od.JissiDate, od.JissiId, od.JissiMachine, od.ReqCd, od.Bunkatu, od.CmtName, od.CmtOpt, od.FontColor, od.CommentNewline
                  )).ToList()
                    );
                result.Add(ordInf);
            }
            return result;
        }

        public IEnumerable<OrdInfModel> GetList(long ptId, int hpId, int sinDate, bool isDeleted)
        {
            var result = new List<OrdInfModel>();

            var allOdrInf = _tenantDataContext.OdrInfs.Where(odr => odr.PtId == ptId && odr.HpId == hpId && odr.SinDate < sinDate && (isDeleted || odr.IsDeleted == 0)).Select(o => new OrdInfModel(
                o.HpId, o.RaiinNo, o.RpNo, o.RpEdaNo, o.PtId, o.SinDate, o.HokenPid, o.OdrKouiKbn, o.RpName, o.InoutKbn, o.SikyuKbn, o.SyohoSbt, o.SanteiKbn, o.TosekiKbn, o.DaysCnt, o.SortNo, o.IsDeleted, o.Id, new List<OrdInfDetailModel>()
                  )).ToList();

            return allOdrInf;
        }

        public IEnumerable<OrdInfDetailModel> GetList(long ptId, int hpId)
        {

            var allOdrInfDetail = _tenantDataContext.OdrInfDetails.Where(odr => odr.PtId == ptId && odr.HpId == hpId).Select(o => new OrdInfDetailModel(
                     o.HpId,
                     o.RaiinNo,
                     o.RpNo,
                     o.RpEdaNo,
                     o.RowNo,
                     o.PtId,
                     o.SinDate,
                     o.SinKouiKbn,
                     o.ItemCd,
                     o.ItemName,
                     o.Suryo,
                     o.UnitName,
                     o.UnitSBT,
                     o.TermVal,
                     o.KohatuKbn,
                     o.SyohoKbn,
                     o.SyohoLimitKbn,
                     o.DrugKbn,
                     o.YohoKbn,
                     o.Kokuji1,
                     o.Kokiji2,
                     o.IsNodspRece,
                     o.IpnCd,
                     o.IpnName,
                     o.JissiKbn,
                     o.JissiDate,
                     o.JissiId,
                     o.JissiMachine,
                     o.ReqCd,
                     o.Bunkatu,
                     o.CmtName,
                     o.CmtOpt,
                     o.FontColor,
                     o.CommentNewline
                  )).ToList();

            return allOdrInfDetail;
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
