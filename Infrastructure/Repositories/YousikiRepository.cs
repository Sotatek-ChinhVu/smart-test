using Domain.Models.Yousiki;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Infrastructure.Services;

namespace Infrastructure.Repositories;

public class YousikiRepository : RepositoryBase, IYousikiRepository
{
    public YousikiRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public List<Yousiki1InfModel> GetHistoryYousiki(int hpId, int sinYm, long ptId, int dataType)
    {
        return NoTrackingDataContext.Yousiki1Infs.Where(x => x.HpId == hpId && x.PtId == ptId && x.DataType == dataType && x.IsDeleted == 0 && (x.Status == 1 || x.Status == 2) &&
                                x.SinYm < sinYm)
            .OrderByDescending(x => x.SinYm)
            .AsEnumerable()
            .Select(x => new Yousiki1InfModel(
                    x.PtId,
                    x.SinYm,
                    x.DataType,
                    x.SeqNo,
                    x.IsDeleted,
                    x.Status)).ToList();
    }

    public List<Yousiki1InfModel> GetYousiki1InfModel(int hpId, int sinYm, long ptNumber, int dataTypes)
    {
        var ptInfs = NoTrackingDataContext.PtInfs.Where(x => x.HpId == hpId &&
                                x.IsDelete == 0 &&
                                (ptNumber == 0 ? true : x.PtNum == ptNumber));
        var yousiki1Infs = NoTrackingDataContext.Yousiki1Infs.Where(x => x.HpId == hpId &&
                            (dataTypes == 0 ? true : x.DataType == dataTypes) &&
                            x.IsDeleted == 0 &&
                            x.SinYm == sinYm);
        var query = from yousikiInf in yousiki1Infs
                    join ptInf in ptInfs on
                    yousikiInf.PtId equals ptInf.PtId
                    select new
                    {
                        yousikiInf,
                        ptInf
                    };
        return query.AsEnumerable()
                    .Select(x => new Yousiki1InfModel(
                            x.yousikiInf.PtId,
                            x.yousikiInf.SinYm,
                            x.yousikiInf.DataType,
                            x.yousikiInf.SeqNo,
                            x.yousikiInf.IsDeleted,
                            x.yousikiInf.Status, 
                            x.ptInf.PtNum,
                            x.ptInf.Name ?? string.Empty))
                    .ToList();
    }

    /// <summary>
    /// Get Yousiki1Inf List, default param when query all is status = -1
    /// </summary>
    /// <param name="hpId"></param>
    /// <param name="sinYm"></param>
    /// <param name="ptNum"></param>
    /// <param name="dataTypes"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    public List<Yousiki1InfModel> GetYousiki1InfModelWithCommonInf(int hpId, int sinYm, long ptNum, int dataTypes, int status = -1)
    {
        List<Yousiki1InfModel> compoundedResultList = new();
        var ptInfs = NoTrackingDataContext.PtInfs.Where(item => item.HpId == hpId
                                                                && item.IsDelete == 0
                                                                && (ptNum == 0 || item.PtNum == ptNum));
        var yousiki1Infs = NoTrackingDataContext.Yousiki1Infs.Where(item => item.HpId == hpId
                                                                            && item.IsDeleted == 0
                                                                            && (sinYm == 0 || item.SinYm == sinYm));
        var yousiki1InfResultList = (from yousikiInf in yousiki1Infs
                                     join ptInf in ptInfs on
                                     yousikiInf.PtId equals ptInf.PtId
                                     select new Yousiki1InfModel(
                                                ptInf.PtNum,
                                                ptInf.Name ?? string.Empty,
                                                yousikiInf.PtId,
                                                yousikiInf.SinYm,
                                                yousikiInf.DataType,
                                                yousikiInf.Status,
                                                new(),
                                                yousikiInf.SeqNo,
                                                new()))
                                     .ToList();

        var groups = yousiki1InfResultList.GroupBy(x => new { x.PtId, x.SinYm }).ToList();
        foreach (var group in groups)
        {
            var orderGroup = group.OrderBy(x => x.DataType).ToList();
            var yousiki = orderGroup.FirstOrDefault(x => (dataTypes == 0 || x.DataType == dataTypes) && (status == -1 || x.Status == status));
            if (yousiki == null)
            {
                continue;
            }

            Dictionary<int, int> statusDic = orderGroup.ToDictionary(x => x.DataType, x => x.Status);
            yousiki.ChangeStatusDic(statusDic);
            compoundedResultList.Add(yousiki);
        }

        return compoundedResultList;
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
