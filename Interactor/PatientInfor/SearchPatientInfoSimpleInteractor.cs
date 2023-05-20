using Domain.Models.GroupInf;
using Domain.Models.PatientInfor;
using Helper.Common;
using Helper.Enum;
using Helper.Extension;
using System.Text.RegularExpressions;
using UseCase.PatientInfor;
using UseCase.PatientInfor.SearchSimple;

namespace Interactor.PatientInfor
{
    public class SearchPatientInfoSimpleInteractor : ISearchPatientInfoSimpleInputPort
    {
        private readonly IPatientInforRepository _patientInforRepository;
        private readonly IGroupInfRepository _groupInfRepository;
        private const string _regPhone = @"[^0-9^\-]";
        private const string startGroupOrderKey = "group_";

        public SearchPatientInfoSimpleInteractor(IPatientInforRepository patientInforRepository, IGroupInfRepository groupInfRepository)
        {
            _patientInforRepository = patientInforRepository;
            _groupInfRepository = groupInfRepository;
        }

        public SearchPatientInfoSimpleOutputData Handle(SearchPatientInfoSimpleInputData inputData)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(inputData.Keyword))
                {
                    return new SearchPatientInfoSimpleOutputData(new List<PatientInfoWithGroup>(), SearchPatientInfoSimpleStatus.InvalidKeyword);
                }

                string halfSizekeyword = CIUtil.ToHalfsize(inputData.Keyword);
                bool isContainMode = inputData.ContainMode;
                bool isNum = int.TryParse(halfSizekeyword, out int ptNum);

                if (!inputData.ContainMode)
                {
                    if (!isNum)
                    {
                        return new SearchPatientInfoSimpleOutputData(new List<PatientInfoWithGroup>(), SearchPatientInfoSimpleStatus.NotFound);
                    }

                    (PatientInforModel ptInfModel, bool isFound) = _patientInforRepository.SearchExactlyPtNum(ptNum, inputData.HpId);
                    if (!isFound)
                    {
                        return new SearchPatientInfoSimpleOutputData(new List<PatientInfoWithGroup>(), SearchPatientInfoSimpleStatus.NotFound);
                    }
                    return new SearchPatientInfoSimpleOutputData(AppendGroupInfo(new List<PatientInforModel> { ptInfModel }), SearchPatientInfoSimpleStatus.Success);
                }

                int searchType = DetectSearchType(inputData.Keyword);
                List<PatientInforModel> patientInfoList = new();
                bool sortGroup = inputData.SortData.Select(item => item.Key).ToList().Exists(item => item.StartsWith(startGroupOrderKey));
                switch (searchType)
                {
                    case 0:
                        patientInfoList = _patientInforRepository.SearchContainPtNum(ptNum, halfSizekeyword, inputData.HpId, inputData.PageIndex, inputData.PageSize, inputData.SortData);
                        if (patientInfoList.Count == 0)
                        {
                            return new SearchPatientInfoSimpleOutputData(new List<PatientInfoWithGroup>(), SearchPatientInfoSimpleStatus.NotFound);
                        }
                        break;
                    case 1:
                        int sindate = halfSizekeyword.AsInteger();
                        patientInfoList = _patientInforRepository.SearchBySindate(sindate, inputData.HpId, inputData.PageIndex, inputData.PageSize, inputData.SortData);
                        if (patientInfoList.Count == 0)
                        {
                            return new SearchPatientInfoSimpleOutputData(new List<PatientInfoWithGroup>(), SearchPatientInfoSimpleStatus.NotFound);
                        }
                        break;
                    case 2:
                        patientInfoList = _patientInforRepository.SearchPhone(inputData.Keyword, isContainMode, inputData.HpId, inputData.PageIndex, inputData.PageSize, inputData.SortData);
                        if (patientInfoList.Count == 0)
                        {
                            return new SearchPatientInfoSimpleOutputData(new List<PatientInfoWithGroup>(), SearchPatientInfoSimpleStatus.NotFound);
                        }
                        break;
                    case 3:
                        patientInfoList = _patientInforRepository.SearchName(inputData.Keyword, halfSizekeyword, isContainMode, inputData.HpId, inputData.PageIndex, inputData.PageSize, inputData.SortData);
                        if (patientInfoList.Count == 0)
                        {
                            return new SearchPatientInfoSimpleOutputData(new List<PatientInfoWithGroup>(), SearchPatientInfoSimpleStatus.NotFound);
                        }
                        break;
                }
                var result = AppendGroupInfo(patientInfoList);
                if (sortGroup)
                {
                    result = SortData(result, inputData.SortData, inputData.PageIndex, inputData.PageSize);
                }
                return new SearchPatientInfoSimpleOutputData(result, SearchPatientInfoSimpleStatus.Success);
            }
            finally
            {
                _patientInforRepository.ReleaseResource();
                _groupInfRepository.ReleaseResource();
            }
        }

        public List<PatientInfoWithGroup> AppendGroupInfo(List<PatientInforModel> patientInfList)
        {
            if (patientInfList.Count == 0)
            {
                return new List<PatientInfoWithGroup>();
            }

            var ptIdList = patientInfList.Select(p => p.PtId).ToList();
            var patientGroupInfList = _groupInfRepository.GetAllByPtIdList(ptIdList);

            List<PatientInfoWithGroup> patientInfoListWithGroup = new List<PatientInfoWithGroup>();
            foreach (var patientInfo in patientInfList)
            {
                long ptId = patientInfo.PtId;
                var groupInfList = patientGroupInfList.Where(p => p.PtId == ptId).ToList();

                patientInfoListWithGroup.Add(new PatientInfoWithGroup(patientInfo, groupInfList));
            }
            return patientInfoListWithGroup;
        }

        /// <summary>
        /// return 0 => search ptId
        /// return 1 => search sindate
        /// return 2 => search phone
        /// return 3 => search name
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public int DetectSearchType(string keyword)
        {
            bool isNum = int.TryParse(keyword, out _);
            if (isNum)
            {
                if (CIUtil.CheckSDate(keyword))
                {
                    return 1;
                }
                return 0;
            }

            Regex regex = new Regex(_regPhone);
            MatchCollection matches = regex.Matches(keyword);
            if (keyword.Length > 0 && matches.Count == 0)
            {
                return 2;
            }

            return 3;
        }

        private List<PatientInfoWithGroup> SortData(List<PatientInfoWithGroup> ptInfList, Dictionary<string, string> sortData, int pageIndex, int pageSize)
        {
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
}
