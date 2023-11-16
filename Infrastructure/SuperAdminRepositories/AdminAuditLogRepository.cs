using Domain.SuperAdminModels.Logger;
using Entity.Logger;
using Helper.Common;
using Helper.Enum;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SuperAdminRepositories;

public class AdminAuditLogRepository : AuditLogRepositoryBase, IAdminAuditLogRepository
{
    public AdminAuditLogRepository(ITenantProvider tenantProvider) : base(tenantProvider)
    {
    }

    public List<AuditLogModel> GetAuditLogList(int tenantId, AuditLogSearchModel requestModel, Dictionary<AuditLogEnum, int> sortDictionary, int skip, int take)
    {
        List<AuditLogModel> result;
        IQueryable<NewAuditLog> query = NoTrackingDataContext.NewAuditLogs.Where(item => item.TenantId == tenantId);

        if (!requestModel.IsEmptyModel)
        {
            query = FilterData(query, requestModel);
        }

        var querySortList = SortQuery(query, sortDictionary);

        querySortList = (IOrderedQueryable<NewAuditLog>)querySortList.Skip(skip).Take(take);

        result = querySortList.Select(item => new AuditLogModel(
                                                  item.LogId,
                                                  item.TenantId,
                                                  item.Domain ?? string.Empty,
                                                  item.ThreadId ?? string.Empty,
                                                  item.LogType ?? string.Empty,
                                                  item.HpId,
                                                  item.UserId,
                                                  item.LoginKey ?? string.Empty,
                                                  item.DepartmentId,
                                                  item.LogDate,
                                                  item.EventCd ?? string.Empty,
                                                  item.PtId,
                                                  item.SinDay,
                                                  item.RaiinNo,
                                                  item.Path ?? string.Empty,
                                                  item.RequestInfo ?? string.Empty,
                                                  item.ClientIP ?? string.Empty,
                                                  item.Desciption ?? string.Empty
                             )).ToList();
        return result;
    }

    private IQueryable<NewAuditLog> FilterData(IQueryable<NewAuditLog> query, AuditLogSearchModel requestModel)
    {
        if (!string.IsNullOrEmpty(requestModel.Domain))
        {
            query = query.Where(p => EF.Functions.ToTsVector("english", p.Domain)
                         .Matches(requestModel.Domain));
        }
        if (!string.IsNullOrEmpty(requestModel.ThreadId))
        {
            query = query.Where(p => EF.Functions.ToTsVector("english", p.ThreadId)
                         .Matches(requestModel.ThreadId));
        }
        if (!string.IsNullOrEmpty(requestModel.LogType))
        {
            query = query.Where(p => EF.Functions.ToTsVector("english", p.LogType)
                         .Matches(requestModel.LogType));
        }
        if (!string.IsNullOrEmpty(requestModel.LoginKey))
        {
            query = query.Where(p => EF.Functions.ToTsVector("english", p.LoginKey)
                         .Matches(requestModel.LoginKey));
        }
        if (!string.IsNullOrEmpty(requestModel.EventCd))
        {
            query = query.Where(p => EF.Functions.ToTsVector("english", p.EventCd ?? string.Empty)
                         .Matches(requestModel.EventCd));
        }
        if (!string.IsNullOrEmpty(requestModel.Path))
        {
            query = query.Where(p => EF.Functions.ToTsVector("english", p.Path ?? string.Empty)
                         .Matches(requestModel.Path));
        }
        if (!string.IsNullOrEmpty(requestModel.RequestInfo))
        {
            query = query.Where(p => EF.Functions.ToTsVector("english", p.RequestInfo)
                         .Matches(requestModel.RequestInfo));
        }
        if (!string.IsNullOrEmpty(requestModel.ClientIP))
        {
            query = query.Where(p => EF.Functions.ToTsVector("english", p.ClientIP)
                         .Matches(requestModel.ClientIP));
        }
        if (!string.IsNullOrEmpty(requestModel.Desciption))
        {
            query = query.Where(p => EF.Functions.ToTsVector("english", p.Desciption)
                         .Matches(requestModel.Desciption));
        }
        if (requestModel.HpId > 0)
        {
            query = query.Where(item => item.HpId == requestModel.HpId);
        }
        if (requestModel.UserId > 0)
        {
            query = query.Where(item => item.UserId == requestModel.UserId);
        }
        if (requestModel.DepartmentId > 0)
        {
            query = query.Where(item => item.DepartmentId == requestModel.DepartmentId);
        }
        if (requestModel.PtId > 0)
        {
            query = query.Where(item => item.PtId == requestModel.PtId);
        }
        if (requestModel.SinDay > 0)
        {
            query = query.Where(item => item.SinDay == requestModel.SinDay);
        }
        if (requestModel.RaiinNo > 0)
        {
            query = query.Where(item => item.RaiinNo == requestModel.RaiinNo);
        }
        if (requestModel.LogId > 0)
        {
            query = query.Where(item => item.LogId == requestModel.LogId);
        }
        if (requestModel.StartDate != null)
        {
            query = query.Where(item => item.LogDate >= requestModel.StartDate);
        }
        if (requestModel.EndDate != null)
        {
            query = query.Where(item => item.LogDate <= requestModel.EndDate);
        }
        return query;
    }

    private IOrderedQueryable<NewAuditLog> SortQuery(IQueryable<NewAuditLog> query, Dictionary<AuditLogEnum, int> sortDictionary)
    {
        bool firstSort = true;
        IOrderedQueryable<NewAuditLog> querySortList = query.OrderByDescending(item => item.LogId);
        foreach (var sortItem in sortDictionary)
        {
            switch (sortItem.Value)
            {
                // DESC
                case 1:
                    switch (sortItem.Key)
                    {
                        case AuditLogEnum.LogType:
                            if (firstSort)
                            {
                                querySortList = querySortList.OrderByDescending(item => item.LogType);
                                continue;
                            }
                            querySortList = querySortList.ThenByDescending(item => item.LogType);
                            break;
                        case AuditLogEnum.UserId:
                            if (firstSort)
                            {
                                querySortList = querySortList.OrderByDescending(item => item.UserId);
                                continue;
                            }
                            querySortList = querySortList.ThenByDescending(item => item.UserId);
                            break;
                        case AuditLogEnum.LoginKey:
                            if (firstSort)
                            {
                                querySortList = querySortList.OrderByDescending(item => item.LoginKey);
                                continue;
                            }
                            querySortList = querySortList.ThenByDescending(item => item.LoginKey);
                            break;
                        case AuditLogEnum.LogDate:
                            if (firstSort)
                            {
                                querySortList = querySortList.OrderByDescending(item => item.LogDate);
                                continue;
                            }
                            querySortList = querySortList.ThenByDescending(item => item.LogDate);
                            break;
                        case AuditLogEnum.EventCd:
                            if (firstSort)
                            {
                                querySortList = querySortList.OrderByDescending(item => item.EventCd);
                                continue;
                            }
                            querySortList = querySortList.ThenByDescending(item => item.EventCd);
                            break;
                        case AuditLogEnum.PtId:
                            if (firstSort)
                            {
                                querySortList = querySortList.OrderByDescending(item => item.PtId);
                                continue;
                            }
                            querySortList = querySortList.ThenByDescending(item => item.PtId);
                            break;
                        case AuditLogEnum.SinDay:
                            if (firstSort)
                            {
                                querySortList = querySortList.OrderByDescending(item => item.SinDay);
                                continue;
                            }
                            querySortList = querySortList.ThenByDescending(item => item.SinDay);
                            break;
                        case AuditLogEnum.RequestInfo:
                            if (firstSort)
                            {
                                querySortList = querySortList.OrderByDescending(item => item.RequestInfo);
                                continue;
                            }
                            querySortList = querySortList.ThenByDescending(item => item.RequestInfo);
                            break;
                        case AuditLogEnum.Desciption:
                            if (firstSort)
                            {
                                querySortList = querySortList.OrderByDescending(item => item.Desciption);
                                continue;
                            }
                            querySortList = querySortList.ThenByDescending(item => item.Desciption);
                            break;
                    }
                    break;
                // ASC
                default:
                    switch (sortItem.Key)
                    {
                        case AuditLogEnum.LogType:
                            if (firstSort)
                            {
                                querySortList = querySortList.OrderBy(item => item.LogType);
                                continue;
                            }
                            querySortList = querySortList.ThenBy(item => item.LogType);
                            break;
                        case AuditLogEnum.UserId:
                            if (firstSort)
                            {
                                querySortList = querySortList.OrderBy(item => item.UserId);
                                continue;
                            }
                            querySortList = querySortList.ThenBy(item => item.UserId);
                            break;
                        case AuditLogEnum.LoginKey:
                            if (firstSort)
                            {
                                querySortList = querySortList.OrderBy(item => item.LoginKey);
                                continue;
                            }
                            querySortList = querySortList.ThenBy(item => item.LoginKey);
                            break;
                        case AuditLogEnum.LogDate:
                            if (firstSort)
                            {
                                querySortList = querySortList.OrderBy(item => item.LogDate);
                                continue;
                            }
                            querySortList = querySortList.ThenBy(item => item.LogDate);
                            break;
                        case AuditLogEnum.EventCd:
                            if (firstSort)
                            {
                                querySortList = querySortList.OrderBy(item => item.EventCd);
                                continue;
                            }
                            querySortList = querySortList.ThenBy(item => item.EventCd);
                            break;
                        case AuditLogEnum.PtId:
                            if (firstSort)
                            {
                                querySortList = querySortList.OrderBy(item => item.PtId);
                                continue;
                            }
                            querySortList = querySortList.ThenBy(item => item.PtId);
                            break;
                        case AuditLogEnum.SinDay:
                            if (firstSort)
                            {
                                querySortList = querySortList.OrderBy(item => item.SinDay);
                                continue;
                            }
                            querySortList = querySortList.ThenBy(item => item.SinDay);
                            break;
                        case AuditLogEnum.RequestInfo:
                            if (firstSort)
                            {
                                querySortList = querySortList.OrderBy(item => item.RequestInfo);
                                continue;
                            }
                            querySortList = querySortList.ThenBy(item => item.RequestInfo);
                            break;
                        case AuditLogEnum.Desciption:
                            if (firstSort)
                            {
                                querySortList = querySortList.OrderBy(item => item.Desciption);
                                continue;
                            }
                            querySortList = querySortList.ThenBy(item => item.Desciption);
                            break;
                    }
                    break;
            }
            firstSort = false;
        }
        return querySortList;
    }

    public void ReleaseResource()
    {
        DisposeDataContext();
    }
}
