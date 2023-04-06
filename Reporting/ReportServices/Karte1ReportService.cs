using Entity.Tenant;
using Infrastructure.Interfaces;
using Reporting.Byomei.DB;
using Reporting.Interface;
using Reporting.Karte1.DB;
using Reporting.Karte1.Model;
using Reporting.Mappers;
using Reporting.NameLabel.DB;
using Reporting.NameLabel.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Linq.Dynamic.Core.Tokenizer;
using CoPtInfModel = Reporting.Karte1.Model.CoPtInfModel;
using ByomeiCoPtByomeiModel = Reporting.Byomei.Model.CoPtByomeiModel;
using ByomeiCoPtHokenInfModel = Reporting.Byomei.Model.CoPtHokenInfModel;
using LabelCoPtInfModel = Reporting.NameLabel.Models.CoPtInfModel;
using Reporting.Mappers.Common;

namespace Reporting.ReportServices
{
    public class Karte1ReportService : IReportService
    {
        private readonly ITenantProvider _tenantProvider;

        public Karte1ReportService(ITenantProvider tenantProvider)
        {
            _tenantProvider = tenantProvider;
        }

        public CommonReportingRequestModel GetByomeiReportingData(long ptId, int fromDay, int toDay, bool tenkiIn, List<int> hokenIds)
        {
            using (var noTrackingDataContext = _tenantProvider.GetNoTrackingDataContext())
            {
                var finder = new CoPtByomeiFinder(noTrackingDataContext);

                var ptByomeis = finder.GetPtByomei(ptId, fromDay, toDay, tenkiIn, hokenIds);
                var ptInf = finder.FindPtInf(ptId);

                List<int> tempHokenIds = new List<int>();
                if (ptByomeis.Any())
                {
                    tempHokenIds = ptByomeis.GroupBy(p => p.HokenPid).Select(p => p.Key).ToList();
                }

                var ptHokenInfs = finder.GetPtHokenInf(ptId, tempHokenIds, toDay);

                List<ByomeiCoPtByomeiModel> results = new List<ByomeiCoPtByomeiModel>();

                if (ptHokenInfs == null || ptHokenInfs.Any() == false)
                {
                    if (ptByomeis.Any())
                    {
                        results.Add(new ByomeiCoPtByomeiModel(fromDay, toDay, ptInf, new(), ptByomeis));
                    }
                }
                else if (ptHokenInfs.Count() == 1)
                {
                    // 使用されている保険が1つの場合、共通(0)とその保険分をまとめて出力
                    if (ptByomeis.Any())
                    {
                        results.Add(new ByomeiCoPtByomeiModel(fromDay, toDay, ptInf, ptHokenInfs.First(), ptByomeis));
                    }
                }
                else
                {
                    List<PtByomei> emByomeis;

                    if (ptByomeis.Any(p => p.HokenPid == 0))
                    {
                        emByomeis = ptByomeis.FindAll(p => p.HokenPid == 0);
                        results.Add(new ByomeiCoPtByomeiModel(fromDay, toDay, ptInf, new(), emByomeis));
                    }

                    foreach (ByomeiCoPtHokenInfModel ptHokenInf in ptHokenInfs)
                    {
                        if (ptByomeis.Any(p => p.HokenPid == ptHokenInf.HokenId))
                        {
                            emByomeis = ptByomeis.FindAll(p => p.HokenPid == ptHokenInf.HokenId);
                            results.Add(new ByomeiCoPtByomeiModel(fromDay, toDay, ptInf, ptHokenInf, emByomeis));
                        }
                    }
                }

                return new ByomeiMapper(results).GetData();
            }
        }

        public Karte1Mapper GetKarte1ReportingData(int hpId, long ptId, int sinDate, int hokenPid, bool tenkiByomei, bool syuByomei)
        {
            using (var noTrackingDataContext = _tenantProvider.GetNoTrackingDataContext())
            {
                var finder = new CoKarte1Finder(noTrackingDataContext);
                // 白紙印刷の場合、データ取得しない
                if (ptId == 0) return new();

                // 患者情報
                CoPtInfModel ptInf = finder.FindPtInf(ptId, sinDate);

                // 病名情報
                List<CoPtByomeiModel> ptByomeis = finder.FindPtByomei(ptId, hokenPid, tenkiByomei);

                //ToDo: DuongLe need to update entity to uncomment below code
                //if (syuByomei)
                //{
                //    foreach (var item in ptByomeis)
                //    {
                //        if (item.SyobyoKbn == 1)
                //        {
                //            item.Byomei = "（主）" + item.Byomei;
                //        }
                //    }
                //}

                // 患者保険情報
                CoPtHokenInfModel ptHokenInf = finder.FindPtHokenInf(ptId, hokenPid, sinDate);

                CoKarte1Model coKarte1Model = new CoKarte1Model(ptInf, ptByomeis, ptHokenInf);

                return new Karte1Mapper(coKarte1Model);
            }
        }

        public CoNameLabelModel GetNameLabelReportingData(long ptId, string kanjiName, int sinDate)
        {
            using (var noTrackingDataContext = _tenantProvider.GetNoTrackingDataContext())
            {
                var finder = new CoNameLabelFinder(noTrackingDataContext);

                // 患者情報
                LabelCoPtInfModel ptInf = finder.FindPtInf(ptId);

                return new CoNameLabelModel(ptInf, kanjiName, sinDate);
            }
        }
    }
}
