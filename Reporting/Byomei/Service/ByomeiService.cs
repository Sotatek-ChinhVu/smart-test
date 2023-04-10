using Entity.Tenant;
using Infrastructure.Interfaces;
using Reporting.Byomei.DB;
using Reporting.Byomei.Mapper;
using Reporting.Mappers.Common;
using ByomeiCoPtByomeiModel = Reporting.Byomei.Model.CoPtByomeiModel;
using ByomeiCoPtHokenInfModel = Reporting.Byomei.Model.CoPtHokenInfModel;

namespace Reporting.Byomei.Service;

public class ByomeiService
{
    private readonly ITenantProvider _tenantProvider;

    public ByomeiService(ITenantProvider tenantProvider)
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

            if (ptHokenInfs == null || !ptHokenInfs.Any())
            {
                if (ptByomeis.Any())
                {
                    results.Add(new ByomeiCoPtByomeiModel(fromDay, toDay, ptInf, new(), ptByomeis));
                }
            }
            else if (ptHokenInfs.Count == 1)
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

                foreach (var ptHokenInf in from ByomeiCoPtHokenInfModel ptHokenInf in ptHokenInfs
                                           where ptByomeis.Any(p => p.HokenPid == ptHokenInf.HokenId)
                                           select ptHokenInf)
                {
                    emByomeis = ptByomeis.FindAll(p => p.HokenPid == ptHokenInf.HokenId);
                    results.Add(new ByomeiCoPtByomeiModel(fromDay, toDay, ptInf, ptHokenInf, emByomeis));
                }
            }

            return new ByomeiMapper(results).GetData();
        }
    }
}
