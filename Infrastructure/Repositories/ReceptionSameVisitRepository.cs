using Domain.Models.Insurance;
using Domain.Models.InsuranceMst;
using Domain.Models.ReceptionSameVisit;
using Entity.Tenant;
using Helper.Common;
using Helper.Constants;
using Infrastructure.Interfaces;
using PostgreDataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ReceptionSameVisitRepository: IReceptionSameVisitRepository
    {
        private readonly TenantNoTrackingDataContext _tenantDataContext;
        public ReceptionSameVisitRepository(ITenantProvider tenantProvider)
        {
            _tenantDataContext = tenantProvider.GetNoTrackingDataContext();
        }

        public IEnumerable<ReceptionSameVisitModel> GetReceptionSameVisit(int hpId, long ptId, int sinDate)
        {
            var listDataRaiinInf = _tenantDataContext.RaiinInfs.Where(x => x.HpId == hpId && x.PtId == ptId && x.SinDate == sinDate && x.IsDeleted == DeleteTypes.None).ToList();
            
            var _doctors = _tenantDataContext.UserMsts.Where(p => p.StartDate <= sinDate && p.EndDate >= sinDate && p.JobCd == 1).OrderBy(p => p.SortNo).ToList();
            
            var _departments = _tenantDataContext.KaMsts.Where(p => p.HpId == hpId && p.IsDeleted == DeleteTypes.None).ToList();
            
            var _comments = _tenantDataContext.RaiinCmtInfs.Where(p => p.HpId == hpId && p.PtId == ptId && p.SinDate == sinDate && p.IsDelete == DeleteTypes.None).ToList();
            
            var _timePeriodModels = _tenantDataContext.UketukeSbtMsts.Where(p => p.HpId == hpId && p.IsDeleted == DeleteTypes.None).OrderBy(p => p.SortNo).ToList();


            var ptInfRespo = _tenantDataContext.PtInfs.Where(item =>
                    item.HpId == hpId && item.PtId == ptId &&
                    item.IsDelete == 0).ToList();

            var ptHokenPatternRepos = _tenantDataContext.PtHokenPatterns
                .Where(hokenPattern =>
                    hokenPattern.HpId == hpId && hokenPattern.PtId == ptId &&
                    hokenPattern.IsDeleted == DeleteTypes.None).ToList();
            var dataHokenInf = _tenantDataContext.PtHokenInfs.Where(x => x.HpId == hpId && x.PtId == ptId).FirstOrDefault();

            var listDorai =
                    (
                      from raiinInf in listDataRaiinInf
                      join ptInf in ptInfRespo on
                          new { raiinInf.HpId, raiinInf.PtId } equals
                          new { ptInf.HpId, ptInf.PtId }
                      join ptHokenPattern in ptHokenPatternRepos on
                                new { raiinInf.HokenPid, raiinInf.PtId } equals
                                new { ptHokenPattern.HokenPid, ptHokenPattern.PtId } into
                                ptHokenPatternList
                      from ptHokenPatternItem in ptHokenPatternList.DefaultIfEmpty()
                      where
                          raiinInf.HpId == hpId &&
                          (raiinInf.PtId == ptId) &&
                          raiinInf.SinDate == sinDate &&
                          raiinInf.IsDeleted == DeleteTypes.None &&
                          ptInf.IsDelete == DeleteTypes.None
                      select new
                      {
                          RaiinInf = raiinInf,
                          PtHokenPatternItem = ptHokenPatternItem,
                          PtId = raiinInf.PtId,
                          PtInf = ptInf,
                          Kohi1Id = ptHokenPatternItem == null ? 0 : ptHokenPatternItem.Kohi1Id,
                          Kohi2Id = ptHokenPatternItem == null ? 0 : ptHokenPatternItem.Kohi2Id,
                          Kohi3Id = ptHokenPatternItem == null ? 0 : ptHokenPatternItem.Kohi3Id,
                          Kohi4Id = ptHokenPatternItem == null ? 0 : ptHokenPatternItem.Kohi4Id,
                      }

                ).ToList();

            var listPtKohi = _tenantDataContext.PtKohis.Where(x => x.PtId == ptId && x.IsDeleted == DeleteTypes.None).ToList();

            var query = from doraiItem in listDorai
                        join ptKohi1 in listPtKohi on
                            new { doraiItem.PtId, doraiItem.Kohi1Id } equals
                            new { PtId = ptKohi1.PtId, Kohi1Id = ptKohi1.HokenId } into ptKohi1List
                        from ptKohi1Item in ptKohi1List.DefaultIfEmpty()
                        join ptKohi2 in listPtKohi on
                            new { doraiItem.PtId, doraiItem.Kohi2Id } equals
                            new { PtId = ptKohi2.PtId, Kohi2Id = ptKohi2.HokenId } into ptKohi2List
                        from ptKohi2Item in ptKohi2List.DefaultIfEmpty()
                        join ptKohi3 in listPtKohi on
                            new { doraiItem.PtId, doraiItem.Kohi3Id } equals
                            new { PtId = ptKohi3.PtId, Kohi3Id = ptKohi3.HokenId } into ptKohi3List
                        from ptKohi3Item in ptKohi3List.DefaultIfEmpty()
                        join ptKohi4 in listPtKohi on
                            new { doraiItem.PtId, doraiItem.Kohi4Id } equals
                            new { PtId = ptKohi4.PtId, Kohi4Id = ptKohi4.HokenId } into ptKohi4List
                        from ptKohi4Item in ptKohi4List.DefaultIfEmpty()
                        select new
                        {
                            RaiinInf = doraiItem.RaiinInf,
                            PtHokenPatternItem = doraiItem.PtHokenPatternItem,
                            PtInfor = doraiItem.PtInf,
                            Kohi1 = ptKohi1Item != null ? new KohiInfModel(
                                        ptKohi1Item.FutansyaNo ?? string.Empty,
                                        ptKohi1Item.JyukyusyaNo ?? string.Empty,
                                        ptKohi1Item.HokenId,
                                        ptKohi1Item.StartDate,
                                        ptKohi1Item.EndDate,
                                        0,
                                        ptKohi1Item.Rate,
                                        ptKohi1Item.GendoGaku,
                                        ptKohi1Item.SikakuDate,
                                        ptKohi1Item.KofuDate,
                                        ptKohi1Item.TokusyuNo ?? string.Empty,
                                        ptKohi1Item.HokenSbtKbn,
                                        ptKohi1Item.Houbetu ?? string.Empty,
                                        ptKohi1Item.HokenNo,
                                        ptKohi1Item.HokenEdaNo,
                                        ptKohi1Item.PrefNo,
                                        new HokenMstModel(),
                                        sinDate,
                                        new List<ConfirmDateModel>(),
                                        false,
                                        ptKohi1Item.IsDeleted,
                                        false,
                                        ptKohi1Item.SeqNo
                                    ) : null,
                            Kohi2 = ptKohi2Item != null ? new KohiInfModel(
                                        ptKohi2Item.FutansyaNo ?? string.Empty,
                                        ptKohi2Item.JyukyusyaNo ?? string.Empty,
                                        ptKohi2Item.HokenId,
                                        ptKohi2Item.StartDate,
                                        ptKohi2Item.EndDate,
                                        0,
                                        ptKohi2Item.Rate,
                                        ptKohi2Item.GendoGaku,
                                        ptKohi2Item.SikakuDate,
                                        ptKohi2Item.KofuDate,
                                        ptKohi2Item.TokusyuNo ?? string.Empty,
                                        ptKohi2Item.HokenSbtKbn,
                                        ptKohi2Item.Houbetu ?? string.Empty,
                                        ptKohi1Item.HokenNo,
                                        ptKohi1Item.HokenEdaNo,
                                        ptKohi1Item.PrefNo,
                                        new HokenMstModel(),
                                        sinDate,
                                        new List<ConfirmDateModel>(),
                                        false,
                                        ptKohi1Item.IsDeleted,
                                        false,
                                        ptKohi1Item.SeqNo
                                    ) : null,
                            Kohi3 = ptKohi3Item != null ? new KohiInfModel(
                                        ptKohi3Item.FutansyaNo ?? string.Empty,
                                        ptKohi3Item.JyukyusyaNo ?? string.Empty,
                                        ptKohi3Item.HokenId,
                                        ptKohi3Item.StartDate,
                                        ptKohi3Item.EndDate,
                                        0,
                                        ptKohi3Item.Rate,
                                        ptKohi3Item.GendoGaku,
                                        ptKohi3Item.SikakuDate,
                                        ptKohi3Item.KofuDate,
                                        ptKohi3Item.TokusyuNo ?? string.Empty,
                                        ptKohi3Item.HokenSbtKbn,
                                        ptKohi3Item.Houbetu ?? string.Empty,
                                        ptKohi1Item.HokenNo,
                                        ptKohi1Item.HokenEdaNo,
                                        ptKohi1Item.PrefNo,
                                        new HokenMstModel(),
                                        sinDate,
                                        new List<ConfirmDateModel>(),
                                        false,
                                        ptKohi1Item.IsDeleted,
                                        false,
                                        ptKohi1Item.SeqNo
                                    ) : null,
                            Kohi4 = ptKohi4Item != null ? new KohiInfModel(
                                        ptKohi4Item.FutansyaNo ?? string.Empty,
                                        ptKohi4Item.JyukyusyaNo ?? string.Empty,
                                        ptKohi4Item.HokenId,
                                        ptKohi4Item.StartDate,
                                        ptKohi4Item.EndDate,
                                        0,
                                        ptKohi4Item.Rate,
                                        ptKohi4Item.GendoGaku,
                                        ptKohi4Item.SikakuDate,
                                        ptKohi4Item.KofuDate,
                                        ptKohi4Item.TokusyuNo ?? string.Empty,
                                        ptKohi4Item.HokenSbtKbn,
                                        ptKohi4Item.Houbetu ?? string.Empty,
                                        ptKohi1Item.HokenNo,
                                        ptKohi1Item.HokenEdaNo,
                                        ptKohi1Item.PrefNo,
                                        new HokenMstModel(),
                                        sinDate,
                                        new List<ConfirmDateModel>(),
                                        false,
                                        ptKohi1Item.IsDeleted,
                                        false,
                                        ptKohi1Item.SeqNo
                                    ) : null,
                        };
            var listHokenData = new List<HokenPatternModel>();

            var listDataJoin = query.AsEnumerable().ToList();

            if (listDataJoin.Any())
            {
                foreach (var item in listDataJoin)
                {
                    string houbetu = string.Empty;
                    int futanRate = 0;
                    long raiinNo = 0;
                    int syosaisinKbn = 0;
                    int jikanKbn = 0;
                    int santeiKbn = 0;
                    var hokenMst = _tenantDataContext.HokenMsts.FirstOrDefault(x => x.HpId == hpId &&  (dataHokenInf == null || x.HokenNo == dataHokenInf.HokenNo && x.HokenEdaNo == dataHokenInf.HokenEdaNo));
                    if (hokenMst != null)
                    {
                        houbetu = hokenMst.Houbetu;
                        futanRate = hokenMst.FutanRate;
                    }
                    if(item.RaiinInf != null)
                    {
                        raiinNo = item.RaiinInf.RaiinNo;
                        syosaisinKbn = item.RaiinInf.SyosaisinKbn;
                        jikanKbn = item.RaiinInf.JikanKbn;
                        santeiKbn = item.RaiinInf.SanteiKbn;
                    }    

                    if (item.PtHokenPatternItem != null)
                    {
                        var itemHokenData = new HokenPatternModel(
                                            ptId,
                                            item.PtHokenPatternItem.HokenPid,
                                            item.PtHokenPatternItem.StartDate,
                                            item.PtHokenPatternItem.EndDate,
                                            item.PtHokenPatternItem.HokenSbtCd,
                                            item.PtHokenPatternItem.HokenKbn,
                                            item.PtHokenPatternItem.Kohi1Id,
                                            item.PtHokenPatternItem.Kohi2Id,
                                            item.PtHokenPatternItem.Kohi3Id,
                                            item.PtHokenPatternItem.Kohi4Id,
                                            item.Kohi1,
                                            item.Kohi2,
                                            item.Kohi3,
                                            item.Kohi4,
                                            sinDate,
                                            houbetu,
                                            futanRate,
                                            raiinNo,
                                            syosaisinKbn,
                                            jikanKbn,
                                            santeiKbn
                                            );

                        listHokenData.Add(itemHokenData);
                    }    
                }
            }
            var listSameVisitModel = new List<ReceptionSameVisitModel>();
            if (listDataRaiinInf.Count > 0)
            {
                foreach (var item in listDataRaiinInf)
                {
                    string hokenPidName = "";
                    string kaName = "";
                    string comment = "";
                    string doctorName = "";
                    string timePeriod = "";
                    string yoyakuInfo = "";
                    int kaId = 0;
                    int doctorId = 0;


                    if (listHokenData.Count > 0)
                    {
                        var hokenModel = listHokenData.FirstOrDefault(u => u.PtId == item.PtId && u.RaiinNo == item.RaiinNo);
                        hokenPidName = hokenModel?.HokenName ?? string.Empty;
                    }

                    if (_departments != null)
                    {
                        var tempKaMst = _departments.Find(p => p.KaId == item.KaId);
                        kaName = tempKaMst == null ? string.Empty : (tempKaMst.KaName ?? string.Empty);
                        kaId = tempKaMst == null ? 0 : tempKaMst.KaId;
                    }

                    if (_comments != null)
                    {
                        var tempComment = _comments.Find(cmt => cmt.RaiinNo == item.RaiinNo);
                        comment = tempComment == null ? string.Empty : tempComment.Text;
                    }

                    if (_doctors != null)
                    {
                        var tempDoctor = _doctors.Find(dr => dr.UserId == item.TantoId);
                        doctorName = tempDoctor == null ? string.Empty : (tempDoctor.Name ?? string.Empty);
                        doctorId = tempDoctor == null ? 0 : tempDoctor.UserId;
                    }

                    if (_timePeriodModels != null)
                    {
                        var timePeriodModel = _timePeriodModels.Find(tp => tp.KbnId == item.UketukeSbt);
                        timePeriod = timePeriodModel == null ? string.Empty : timePeriodModel.KbnName;
                    }
                    yoyakuInfo = GetYoyaku(listDataRaiinInf, item.OyaRaiinNo, kaName) ?? string.Empty;

                    var itemModelDorai = new ReceptionSameVisitModel(
                                            item.HpId,
                                            item.PtId,
                                            item.UketukeNo,
                                            kaName,
                                            hokenPidName,
                                            item.UketukeTime ?? String.Empty,
                                            item.Status,
                                            timePeriod,
                                            yoyakuInfo,
                                            doctorName,
                                            comment,
                                            item.OyaRaiinNo,
                                            item.HokenPid,
                                            kaId,
                                            doctorId,
                                            item.SyosaisinKbn,
                                            item.JikanKbn,
                                            item.SanteiKbn,
                                            item.RaiinNo
                                         );

                    listSameVisitModel.Add(itemModelDorai);
                }

            }    


            return listSameVisitModel;
        }

        private string GetYoyaku(List<RaiinInf> listDoraiModel, long yoyakuNo, string kaName)
        {
            string ConvertTimeToString(string uketsukeTime)
            {
                return CIUtil.HourAndMinuteFormat(uketsukeTime);
            }
            if (listDoraiModel == null)
            {
                return string.Empty;
            }

            if (yoyakuNo == 0)
            {
                return string.Empty;
            }
            var tempModel = listDoraiModel.Find(x => x.OyaRaiinNo == yoyakuNo);
            if (tempModel == null)
            {
                return string.Empty;
            }
            return tempModel.UketukeNo.ToString() + ". " + kaName + " " + ConvertTimeToString(tempModel.UketukeTime ?? string.Empty);
        }
    }
}
