using Domain.Models.GroupInf;
using Domain.Models.PatientInfor;
using Helper.Common;
using Helper.Extension;
using Interactor.PatientInfor.SortPatientCommon;
using System.Text.RegularExpressions;
using UseCase.PatientInfor;
using UseCase.PatientInfor.SearchSimple;

namespace Interactor.PatientInfor
{
    public class SearchPatientInfoSimpleInteractor : ISearchPatientInfoSimpleInputPort
    {
        private readonly IPatientInforRepository _patientInforRepository;
        private readonly IGroupInfRepository _groupInfRepository;
        private readonly ISortPatientCommon _sortPatientCommon;
        private const string _regPhone = @"[^0-9^\-]";
        private const string startGroupOrderKey = "group_";

        public SearchPatientInfoSimpleInteractor(IPatientInforRepository patientInforRepository, IGroupInfRepository groupInfRepository, ISortPatientCommon sortPatientCommon)
        {
            _patientInforRepository = patientInforRepository;
            _groupInfRepository = groupInfRepository;
            _sortPatientCommon = sortPatientCommon;
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

                    (PatientInforModel ptInfModel, bool isFound) = _patientInforRepository.SearchExactlyPtNum(ptNum, inputData.HpId, 0);
                    if (!isFound)
                    {
                        return new SearchPatientInfoSimpleOutputData(new List<PatientInfoWithGroup>(), SearchPatientInfoSimpleStatus.NotFound);
                    }
                    return new SearchPatientInfoSimpleOutputData(AppendGroupInfo(new List<PatientInforModel> { ptInfModel }, inputData.HpId), SearchPatientInfoSimpleStatus.Success);
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
                var result = AppendGroupInfo(patientInfoList, inputData.HpId);
                if (sortGroup)
                {
                    result = _sortPatientCommon.SortData(result, inputData.SortData, inputData.PageIndex, inputData.PageSize);
                }
                return new SearchPatientInfoSimpleOutputData(result, SearchPatientInfoSimpleStatus.Success);
            }
            finally
            {
                _patientInforRepository.ReleaseResource();
                _groupInfRepository.ReleaseResource();
            }
        }

        public List<PatientInfoWithGroup> AppendGroupInfo(List<PatientInforModel> patientInfList, int hpId)
        {
            if (patientInfList.Count == 0)
            {
                return new List<PatientInfoWithGroup>();
            }

            var ptIdList = patientInfList.Select(p => p.PtId).ToList();
            var patientGroupInfList = _groupInfRepository.GetAllByPtIdList(ptIdList, hpId);

            List<PatientInfoWithGroup> patientInfoListWithGroup = new();
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
    }
}
