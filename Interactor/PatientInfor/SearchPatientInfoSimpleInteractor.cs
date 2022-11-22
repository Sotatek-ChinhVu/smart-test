using Domain.Models.GroupInf;
using Domain.Models.PatientInfor;
using Helper.Common;
using Helper.Extension;
using System.Text.RegularExpressions;
using UseCase.PatientInfor.SearchSimple;

namespace Interactor.PatientInfor
{
    public class SearchPatientInfoSimpleInteractor : ISearchPatientInfoSimpleInputPort
    {
        private readonly IPatientInforRepository _patientInforRepository;
        private readonly IGroupInfRepository _groupInfRepository;
        private const string _regPhone = @"[^0-9\-]";

        public SearchPatientInfoSimpleInteractor(IPatientInforRepository patientInforRepository, IGroupInfRepository groupInfRepository)
        {
            _patientInforRepository = patientInforRepository;
            _groupInfRepository = groupInfRepository;
        }

        public SearchPatientInfoSimpleOutputData Handle(SearchPatientInfoSimpleInputData inputData)
        {
            if (string.IsNullOrWhiteSpace(inputData.Keyword))
            {
                return new SearchPatientInfoSimpleOutputData(new List<PatientInfoWithGroup>(), SearchPatientInfoSimpleStatus.InvalidKeyword);
            }

            string keyword = CIUtil.ToHalfsize(inputData.Keyword);
            bool isContainMode = inputData.ContainMode;
            bool isNum = int.TryParse(keyword, out int ptNum);

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
            List<PatientInforModel> patientInfoList;
            switch (searchType)
            {
                case 0:
                    patientInfoList = _patientInforRepository.SearchContainPtNum(ptNum, keyword, inputData.HpId);
                    if (patientInfoList.Count == 0)
                    {
                        return new SearchPatientInfoSimpleOutputData(new List<PatientInfoWithGroup>(), SearchPatientInfoSimpleStatus.NotFound);
                    }

                    return new SearchPatientInfoSimpleOutputData(AppendGroupInfo(patientInfoList), SearchPatientInfoSimpleStatus.Success);
                case 1:
                    int sindate = keyword.AsInteger();
                    patientInfoList = _patientInforRepository.SearchBySindate(sindate, inputData.HpId);
                    if (patientInfoList.Count == 0)
                    {
                        return new SearchPatientInfoSimpleOutputData(new List<PatientInfoWithGroup>(), SearchPatientInfoSimpleStatus.NotFound);
                    }

                    return new SearchPatientInfoSimpleOutputData(AppendGroupInfo(patientInfoList), SearchPatientInfoSimpleStatus.Success);
                case 2:
                    patientInfoList = _patientInforRepository.SearchPhone(inputData.Keyword, isContainMode, inputData.HpId);
                    if (patientInfoList.Count == 0)
                    {
                        return new SearchPatientInfoSimpleOutputData(new List<PatientInfoWithGroup>(), SearchPatientInfoSimpleStatus.NotFound);
                    }
                    return new SearchPatientInfoSimpleOutputData(AppendGroupInfo(patientInfoList), SearchPatientInfoSimpleStatus.Success);
                case 3:
                    patientInfoList = _patientInforRepository.SearchName(keyword, isContainMode, inputData.HpId);
                    if (patientInfoList.Count == 0)
                    {
                        return new SearchPatientInfoSimpleOutputData(new List<PatientInfoWithGroup>(), SearchPatientInfoSimpleStatus.NotFound);
                    }
                    return new SearchPatientInfoSimpleOutputData(AppendGroupInfo(patientInfoList), SearchPatientInfoSimpleStatus.Success);
            }

            return new SearchPatientInfoSimpleOutputData(new List<PatientInfoWithGroup>(), SearchPatientInfoSimpleStatus.NotFound);
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
    }
}
