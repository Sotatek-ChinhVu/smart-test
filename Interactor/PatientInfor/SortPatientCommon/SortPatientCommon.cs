using Helper.Enum;
using UseCase.PatientInfor;

namespace Interactor.PatientInfor.SortPatientCommon;

public class SortPatientCommon : ISortPatientCommon
{
    private const string startGroupOrderKey = "group_";

    public List<PatientInfoWithGroup> SortData(IEnumerable<PatientInfoWithGroup> ptInfList, Dictionary<string, string> sortData, int pageIndex, int pageSize)
    {
        if (!sortData.Any())
        {
            return ptInfList
                   .Skip((pageIndex - 1) * pageSize)
                   .Take(pageSize)
                   .ToList();
        }
        int index = 1;
        IOrderedEnumerable<PatientInfoWithGroup> sortQuery = ptInfList.OrderBy(item => item.PtId);
        foreach (var item in sortData)
        {
            string typeSort = item.Value.Replace(" ", string.Empty).ToLower();
            if (item.Key.StartsWith(startGroupOrderKey))
            {
                int groupId = 0;
                int.TryParse(item.Key.Replace(startGroupOrderKey, string.Empty), out groupId);
                if (index == 1)
                {
                    sortQuery = OrderByDynamicAction(groupId, typeSort, sortQuery);
                    index++;
                    continue;
                }
                sortQuery = ThenOrderByDynamicAction(groupId, typeSort, sortQuery);
                continue;
            }
            int field = 0;
            int.TryParse(item.Key, out field);
            if (index == 1)
            {
                sortQuery = OrderByAction((FieldSortPatientEnum)field, typeSort, sortQuery);
                index++;
                continue;
            }
            sortQuery = ThenOrderByAction((FieldSortPatientEnum)field, typeSort, sortQuery);
        }

        var result = sortQuery
                     .Skip((pageIndex - 1) * pageSize)
                     .Take(pageSize)
                     .ToList();
        return result;
    }

    private IOrderedEnumerable<PatientInfoWithGroup> OrderByDynamicAction(int groupId, string typeSort, IOrderedEnumerable<PatientInfoWithGroup> sortQuery)
    {
        if (typeSort.Equals("desc"))
        {
            sortQuery = sortQuery.OrderByDescending(item => item.GroupInfList.FirstOrDefault(item => item.Key == groupId).Value?.GroupCodeName ?? string.Empty);
        }
        else
        {
            sortQuery = sortQuery.OrderBy(item => item.GroupInfList.FirstOrDefault(item => item.Key == groupId).Value?.GroupCodeName ?? string.Empty);
        }
        return sortQuery;
    }

    private IOrderedEnumerable<PatientInfoWithGroup> ThenOrderByDynamicAction(int groupId, string typeSort, IOrderedEnumerable<PatientInfoWithGroup> sortQuery)
    {
        if (typeSort.Equals("desc"))
        {
            sortQuery = sortQuery.ThenByDescending(item => item.GroupInfList.FirstOrDefault(item => item.Key == groupId).Value?.GroupCodeName ?? string.Empty);
        }
        else
        {
            sortQuery = sortQuery.ThenBy(item => item.GroupInfList.FirstOrDefault(item => item.Key == groupId).Value?.GroupCodeName ?? string.Empty);
        }
        return sortQuery;
    }

    private IOrderedEnumerable<PatientInfoWithGroup> OrderByAction(FieldSortPatientEnum field, string typeSort, IOrderedEnumerable<PatientInfoWithGroup> sortQuery)
    {
        switch (field)
        {
            case FieldSortPatientEnum.PtId:
                if (typeSort.Equals("desc"))
                {
                    sortQuery = sortQuery.OrderByDescending(item => item.PtId);
                }
                else
                {
                    sortQuery = sortQuery.OrderBy(item => item.PtId);
                }
                break;
            case FieldSortPatientEnum.PtNum:
                if (typeSort.Equals("desc"))
                {
                    sortQuery = sortQuery.OrderByDescending(item => item.PtNum);
                }
                else
                {
                    sortQuery = sortQuery.OrderBy(item => item.PtNum);
                }
                break;
            case FieldSortPatientEnum.KanaName:
                if (typeSort.Equals("desc"))
                {
                    sortQuery = sortQuery.OrderByDescending(item => item.KanaName);
                }
                else
                {
                    sortQuery = sortQuery.OrderBy(item => item.KanaName);
                }
                break;
            case FieldSortPatientEnum.Name:
                if (typeSort.Equals("desc"))
                {
                    sortQuery = sortQuery.OrderByDescending(item => item.Name);
                }
                else
                {
                    sortQuery = sortQuery.OrderBy(item => item.Name);
                }
                break;
            case FieldSortPatientEnum.Birthday:
                if (typeSort.Equals("desc"))
                {
                    sortQuery = sortQuery.OrderByDescending(item => item.Birthday);
                }
                else
                {
                    sortQuery = sortQuery.OrderBy(item => item.Birthday);
                }
                break;
            case FieldSortPatientEnum.Sex:
                if (typeSort.Equals("desc"))
                {
                    sortQuery = sortQuery.OrderByDescending(item => item.Sex);
                }
                else
                {
                    sortQuery = sortQuery.OrderBy(item => item.Sex);
                }
                break;
            case FieldSortPatientEnum.Age:
                if (typeSort.Equals("desc"))
                {
                    sortQuery = sortQuery.OrderByDescending(item => item.Age);
                }
                else
                {
                    sortQuery = sortQuery.OrderBy(item => item.Age);
                }
                break;
            case FieldSortPatientEnum.Tel1:
                if (typeSort.Equals("desc"))
                {
                    sortQuery = sortQuery.OrderByDescending(item => item.Tel1);
                }
                else
                {
                    sortQuery = sortQuery.OrderBy(item => item.Tel1);
                }
                break;
            case FieldSortPatientEnum.Tel2:
                if (typeSort.Equals("desc"))
                {
                    sortQuery = sortQuery.OrderByDescending(item => item.Tel2);
                }
                else
                {
                    sortQuery = sortQuery.OrderBy(item => item.Tel2);
                }
                break;
            case FieldSortPatientEnum.RenrakuTel:
                if (typeSort.Equals("desc"))
                {
                    sortQuery = sortQuery.OrderByDescending(item => item.RenrakuTel);
                }
                else
                {
                    sortQuery = sortQuery.OrderBy(item => item.RenrakuTel);
                }
                break;
            case FieldSortPatientEnum.HomePost:
                if (typeSort.Equals("desc"))
                {
                    sortQuery = sortQuery.OrderByDescending(item => item.HomePost);
                }
                else
                {
                    sortQuery = sortQuery.OrderBy(item => item.HomePost);
                }
                break;
            case FieldSortPatientEnum.HomeAddress:
                if (typeSort.Equals("desc"))
                {
                    sortQuery = sortQuery.OrderByDescending(item => item.HomeAddress);
                }
                else
                {
                    sortQuery = sortQuery.OrderBy(item => item.HomeAddress);
                }
                break;
            case FieldSortPatientEnum.LastVisitDate:
                if (typeSort.Equals("desc"))
                {
                    sortQuery = sortQuery.OrderByDescending(item => item.LastVisitDate);
                }
                else
                {
                    sortQuery = sortQuery.OrderBy(item => item.LastVisitDate);
                }
                break;
        }
        return sortQuery;
    }

    private IOrderedEnumerable<PatientInfoWithGroup> ThenOrderByAction(FieldSortPatientEnum field, string typeSort, IOrderedEnumerable<PatientInfoWithGroup> sortQuery)
    {
        switch (field)
        {
            case FieldSortPatientEnum.PtId:
                if (typeSort.Equals("desc"))
                {
                    sortQuery = sortQuery.ThenByDescending(item => item.PtId);
                }
                else
                {
                    sortQuery = sortQuery.ThenBy(item => item.PtId);
                }
                break;
            case FieldSortPatientEnum.PtNum:
                if (typeSort.Equals("desc"))
                {
                    sortQuery = sortQuery.ThenByDescending(item => item.PtNum);
                }
                else
                {
                    sortQuery = sortQuery.ThenBy(item => item.PtNum);
                }
                break;
            case FieldSortPatientEnum.KanaName:
                if (typeSort.Equals("desc"))
                {
                    sortQuery = sortQuery.ThenByDescending(item => item.KanaName);
                }
                else
                {
                    sortQuery = sortQuery.ThenBy(item => item.KanaName);
                }
                break;
            case FieldSortPatientEnum.Name:
                if (typeSort.Equals("desc"))
                {
                    sortQuery = sortQuery.ThenByDescending(item => item.Name);
                }
                else
                {
                    sortQuery = sortQuery.ThenBy(item => item.Name);
                }
                break;
            case FieldSortPatientEnum.Birthday:
                if (typeSort.Equals("desc"))
                {
                    sortQuery = sortQuery.ThenByDescending(item => item.Birthday);
                }
                else
                {
                    sortQuery = sortQuery.ThenBy(item => item.Birthday);
                }
                break;
            case FieldSortPatientEnum.Sex:
                if (typeSort.Equals("desc"))
                {
                    sortQuery = sortQuery.ThenByDescending(item => item.Sex);
                }
                else
                {
                    sortQuery = sortQuery.ThenBy(item => item.Sex);
                }
                break;
            case FieldSortPatientEnum.Age:
                if (typeSort.Equals("desc"))
                {
                    sortQuery = sortQuery.ThenByDescending(item => item.Age);
                }
                else
                {
                    sortQuery = sortQuery.ThenBy(item => item.Age);
                }
                break;
            case FieldSortPatientEnum.Tel1:
                if (typeSort.Equals("desc"))
                {
                    sortQuery = sortQuery.ThenByDescending(item => item.Tel1);
                }
                else
                {
                    sortQuery = sortQuery.ThenBy(item => item.Tel1);
                }
                break;
            case FieldSortPatientEnum.Tel2:
                if (typeSort.Equals("desc"))
                {
                    sortQuery = sortQuery.ThenByDescending(item => item.Tel2);
                }
                else
                {
                    sortQuery = sortQuery.ThenBy(item => item.Tel2);
                }
                break;
            case FieldSortPatientEnum.RenrakuTel:
                if (typeSort.Equals("desc"))
                {
                    sortQuery = sortQuery.ThenByDescending(item => item.RenrakuTel);
                }
                else
                {
                    sortQuery = sortQuery.ThenBy(item => item.RenrakuTel);
                }
                break;
            case FieldSortPatientEnum.HomePost:
                if (typeSort.Equals("desc"))
                {
                    sortQuery = sortQuery.ThenByDescending(item => item.HomePost);
                }
                else
                {
                    sortQuery = sortQuery.ThenBy(item => item.HomePost);
                }
                break;
            case FieldSortPatientEnum.HomeAddress:
                if (typeSort.Equals("desc"))
                {
                    sortQuery = sortQuery.ThenByDescending(item => item.HomeAddress);
                }
                else
                {
                    sortQuery = sortQuery.ThenBy(item => item.HomeAddress);
                }
                break;
            case FieldSortPatientEnum.LastVisitDate:
                if (typeSort.Equals("desc"))
                {
                    sortQuery = sortQuery.ThenByDescending(item => item.LastVisitDate);
                }
                else
                {
                    sortQuery = sortQuery.ThenBy(item => item.LastVisitDate);
                }
                break;
        }
        return sortQuery;
    }

}
