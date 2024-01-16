using Domain.Models.Yousiki;
using Infrastructure.Base;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class YousikiRepository : RepositoryBase, IYousikiRepository
{
    public YousikiRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
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

    /// <summary>
    /// Get Yousiki1InfDetail list
    /// </summary>
    /// <param name="sinYm"></param>
    /// <param name="ptId"></param>
    /// <param name="dataType"></param>
    /// <param name="seqNo"></param>
    /// <returns></returns>
    public List<Yousiki1InfDetailModel> GetYousiki1InfDetails(int hpId, int sinYm, long ptId, int dataType, int seqNo)
    {
        var result = NoTrackingDataContext.Yousiki1InfDetails.Where(item => item.SinYm == sinYm
                                                                            && item.PtId == ptId
                                                                            && item.DataType == dataType
                                                                            && item.SeqNo == seqNo
                                                                            && item.HpId == hpId)
                                                             .Select(item => new Yousiki1InfDetailModel(
                                                                                 item.PtId,
                                                                                 item.SinYm,
                                                                                 item.DataType,
                                                                                 item.SeqNo,
                                                                                 item.CodeNo ?? string.Empty,
                                                                                 item.RowNo,
                                                                                 item.Payload,
                                                                                 item.Value ?? string.Empty))
                                                             .ToList();
        return result;
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
