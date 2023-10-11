using Domain.Models.PatientInfor;
using Helper.Extension;
using UseCase.PatientInfor.UpdateVisitTimesManagement;

namespace Interactor.PatientInfor;

public class UpdateVisitTimesManagementInteractor : IUpdateVisitTimesManagementInputPort
{
    private readonly IPatientInforRepository _patientInforRepository;
    private string defaultTimer = "000000";
    private string defaultRaiiNo = "0000000000";
    private string defaultKohiPriorityNumber = "99999999" + "99999999" + "99999999" + "99999999";
    private string defaultHokenPId = "0000";
    private string defaultSeqNo = "0";

    public UpdateVisitTimesManagementInteractor(IPatientInforRepository patientInforRepository)
    {
        _patientInforRepository = patientInforRepository;
    }

    public UpdateVisitTimesManagementOutputData Handle(UpdateVisitTimesManagementInputData inputData)
    {
        try
        {
            if (!_patientInforRepository.CheckExistIdList(new List<long>() { inputData.PtId }))
            {
                return new UpdateVisitTimesManagementOutputData(UpdateVisitTimesManagementStatus.InvalidPtId);
            }
            var visitTimeDBList = _patientInforRepository.GetVisitTimesManagementModels(inputData.HpId, inputData.SinYm, inputData.PtId, inputData.KohiId);
            if (inputData.VisitTimesManagementList.Any(item => item.IsDeleted && !item.IsOutHospital))
            {
                return new UpdateVisitTimesManagementOutputData(UpdateVisitTimesManagementStatus.CanNotDeleted);
            }
            else if (!inputData.VisitTimesManagementList.Any() && visitTimeDBList.Any(item => !item.IsOutHospital))
            {
                return new UpdateVisitTimesManagementOutputData(UpdateVisitTimesManagementStatus.CanNotDeleted);
            }
            var visitTimesList = ResetSortKey(inputData, visitTimeDBList);
            if (_patientInforRepository.UpdateVisitTimesManagement(inputData.HpId, inputData.UserId, inputData.PtId, inputData.KohiId, visitTimesList))
            {
                return new UpdateVisitTimesManagementOutputData(UpdateVisitTimesManagementStatus.Successed);
            }
            return new UpdateVisitTimesManagementOutputData(UpdateVisitTimesManagementStatus.Failed);
        }
        finally
        {
            _patientInforRepository.ReleaseResource();
        }
    }

    private List<VisitTimesManagementModel> ResetSortKey(UpdateVisitTimesManagementInputData inputData, List<VisitTimesManagementModel> visitTimeDBList)
    {
        var visitTimesList = inputData.VisitTimesManagementList;
        var firstItem = visitTimesList[0];
        int sinYM = firstItem.SinDate / 100;

        Dictionary<int, int> seqNoDic = new();

        int indexItem = 0;
        foreach (var item in visitTimeDBList)
        {
            if (item.SeqNo > 0)
            {
                seqNoDic.Add(item.SeqNo, indexItem);
            }
            indexItem++;
        }

        int nextIndex = 1;
        if (firstItem.IsOutHospital)
        {
            int sinDate = (sinYM + "01").AsInteger();
            string sortKey = sinDate + defaultTimer + defaultRaiiNo + defaultKohiPriorityNumber + defaultHokenPId + defaultSeqNo;
            firstItem.ChangeSortKey(sinDate, sortKey);

            int seqNo = 1;
            while (nextIndex < visitTimesList.Count)
            {
                var nextItem = visitTimesList[nextIndex++];
                if (!nextItem.IsOutHospital) break;

                sinDate = firstItem.SinDate;
                sortKey = firstItem.SortKey.Substring(0, 24) + defaultKohiPriorityNumber + defaultHokenPId + (seqNo++).AsString();
                nextItem.ChangeSortKey(sinDate, sortKey);
            }
        }

        int index = 0;
        foreach (var item in visitTimesList)
        {
            if (!item.IsOutHospital)
            {
                nextIndex = index + 1;
                int seqNo = 0;
                while (nextIndex < visitTimesList.Count)
                {
                    var nextItem = visitTimesList[nextIndex++];
                    if (!nextItem.IsOutHospital) break;

                    int sinDate = item.SinDate;
                    string sortKey = item.SortKey.Substring(0, 24) + defaultKohiPriorityNumber + defaultHokenPId + (seqNo++).AsString();
                    nextItem.ChangeSortKey(sinDate, sortKey);
                }
            }

            // change seqNo = 0 if resort
            if (seqNoDic.ContainsKey(item.SeqNo) && seqNoDic[item.SeqNo] != index && item.IsOutHospital)
            {
                item.ChangeSeqNo(0);
            }

            index++;
        }
        return visitTimesList;
    }
}