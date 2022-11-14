using Domain.Models.NextOrder;
using Entity.Tenant;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System.Text;

namespace Infrastructure.Repositories
{
    public class NextOrderRepository : INextOrder
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        private readonly TenantDataContext _tenantDataContextTracking;
        public NextOrderRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
            _tenantDataContextTracking = tenantProvider.GetTrackingTenantDataContext();
        }

        public (List<RsvkrtByomeiModel> byomeis, RsvkrtKarteInfModel karteInf, List<RsvkrtOrderInfModel> orderInfs) Get(int hpId, long ptId, long rsvkrtNo, int type)
        {
            var byomeis = new List<RsvkrtByomei>();
            if (type == 2)
            {
                byomeis = _tenantDataContextTracking.RsvkrtByomeis.Where(b => b.HpId == hpId && b.PtId == ptId && b.RsvkrtNo == rsvkrtNo && b.IsDeleted == DeleteTypes.None).ToList();
            }
            var karteInf = _tenantDataContextTracking.RsvkrtKarteInfs.FirstOrDefault(k => k.HpId == hpId && k.PtId == ptId && k.RsvkrtNo == rsvkrtNo && k.IsDeleted == DeleteTypes.None);
            var orderInfs = _tenantDataContextTracking.RsvkrtOdrInfs.Where(o => o.HpId == hpId && o.PtId == ptId && o.RsvkrtNo == rsvkrtNo && o.IsDeleted == DeleteTypes.None).ToList();
            var orderInfDetails = _tenantDataContextTracking.RsvkrtOdrInfDetails.Where(o => o.HpId == hpId && o.PtId == ptId && o.RsvkrtNo == rsvkrtNo).ToList();

            var byomeiModels = byomeis.Select(b => ConvertByomeiToModel(b)).ToList();

            var karteModel = ConvertKarteInfToModel(karteInf ?? new());

            var oderInfModels = orderInfs.Select(o => ConvertOrderInfToModel(o, orderInfDetails)).ToList();

            return (byomeiModels, karteModel, oderInfModels);
        }

        private RsvkrtByomeiModel ConvertByomeiToModel(RsvkrtByomei byomei)
        {
            return new RsvkrtByomeiModel(
                    byomei.Id,
                    byomei.HpId,
                    byomei.PtId,
                    byomei.RsvkrtNo,
                    byomei.SeqNo,
                    byomei.ByomeiCd ?? string.Empty,
                    byomei.Byomei ?? string.Empty,
                    byomei.SyobyoKbn,
                    byomei.SikkanKbn,
                    byomei.NanbyoCd,
                    byomei.HosokuCmt ?? string.Empty,
                    byomei.IsNodspRece,
                    byomei.IsNodspKarte,
                    byomei.IsDeleted,
                    GetCodeLists(byomei)

                );
        }

        private RsvkrtKarteInfModel ConvertKarteInfToModel(RsvkrtKarteInf karteInf)
        {
            return new RsvkrtKarteInfModel(
                    karteInf.HpId,
                    karteInf.PtId,
                    karteInf.RsvDate,
                    karteInf.RsvkrtNo,
                    karteInf.KarteKbn,
                    karteInf.SeqNo,
                    karteInf.Text ?? string.Empty,
                    karteInf.RichText == null ? string.Empty : Encoding.UTF8.GetString(karteInf.RichText),
                    karteInf.IsDeleted
                );
        }

        private RsvkrtOrderInfModel ConvertOrderInfToModel(RsvkrtOdrInf odrInf, List<RsvkrtOdrInfDetail> odrInfDetails)
        {
            odrInfDetails = odrInfDetails.Where(od => od.RpNo == odrInf.RpNo && od.RpEdaNo == odrInf.RpEdaNo).ToList();
            return new RsvkrtOrderInfModel(
                    odrInf.HpId,
                    odrInf.PtId,
                    odrInf.RsvDate,
                    odrInf.RsvkrtNo,
                    odrInf.RpNo,
                    odrInf.RpEdaNo,
                    odrInf.Id,
                    odrInf.HokenPid,
                    odrInf.OdrKouiKbn,
                    odrInf.RpName,
                    odrInf.InoutKbn,
                    odrInf.SikyuKbn,
                    odrInf.SyohoSbt,
                    odrInf.SanteiKbn,
                    odrInf.TosekiKbn,
                    odrInf.DaysCnt,
                    odrInf.IsDeleted,
                    odrInf.SortNo,
                    odrInfDetails.Select(od => new RsvKrtOrderInfDetailModel(
                            od.HpId,
                            od.PtId,
                            od.RsvkrtNo,
                            od.RpNo,
                            od.RpEdaNo,
                            od.RowNo,
                            od.RsvDate,
                            od.SinKouiKbn,
                            od.ItemCd ?? string.Empty,
                            od.ItemName ?? string.Empty,
                            od.Suryo,
                            od.UnitName ?? string.Empty,
                            od.UnitSbt,
                            od.TermVal,
                            od.KohatuKbn,
                            od.SyohoKbn,
                            od.SyohoLimitKbn,
                            od.DrugKbn,
                            od.YohoKbn,
                            od.Kokuji1 ?? string.Empty,
                            od.Kokuji2 ?? string.Empty,
                            od.IsNodspRece,
                            od.IpnCd ?? string.Empty,
                            od.IpnName ?? string.Empty,
                            od.Bunkatu ?? string.Empty,
                            od.CmtName ?? string.Empty,
                            od.CmtOpt ?? string.Empty,
                            od.FontColor ?? string.Empty,
                            od.CommentNewline
                        )
                    ).ToList()
                );
        }

        private List<string> GetCodeLists(RsvkrtByomei mst)
        {
            var codeLists = new List<string>()
            {
                mst.SyusyokuCd1 ?? string.Empty,
                mst.SyusyokuCd2 ?? string.Empty,
                mst.SyusyokuCd3 ?? string.Empty,
                mst.SyusyokuCd4 ?? string.Empty,
                mst.SyusyokuCd5 ?? string.Empty,
                mst.SyusyokuCd6 ?? string.Empty,
                mst.SyusyokuCd7 ?? string.Empty,
                mst.SyusyokuCd8 ?? string.Empty,
                mst.SyusyokuCd9 ?? string.Empty,
                mst.SyusyokuCd10 ?? string.Empty,
                mst.SyusyokuCd11 ?? string.Empty,
                mst.SyusyokuCd12 ?? string.Empty,
                mst.SyusyokuCd13 ?? string.Empty,
                mst.SyusyokuCd14 ?? string.Empty,
                mst.SyusyokuCd15 ?? string.Empty,
                mst.SyusyokuCd16 ?? string.Empty,
                mst.SyusyokuCd17 ?? string.Empty,
                mst.SyusyokuCd18 ?? string.Empty,
                mst.SyusyokuCd19 ?? string.Empty,
                mst.SyusyokuCd20 ?? string.Empty,
                mst.SyusyokuCd21 ?? string.Empty
            };
            return codeLists?.Where(c => c != string.Empty).ToList() ?? new List<string>();
        }
    }
}
