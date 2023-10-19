using Domain.Models.PatientGroupMst;
using UseCase.PatientGroupMst.SaveList;

namespace Interactor.PatientGroupMst;

public class SaveListPatientGroupMstInteractor : ISaveListPatientGroupMstInputPort
{
    private readonly IPatientGroupMstRepository _patientGroupMstRepository;
    public SaveListPatientGroupMstInteractor(IPatientGroupMstRepository patientGroupMstRepository)
    {
        _patientGroupMstRepository = patientGroupMstRepository;
    }
    public SaveListPatientGroupMstOutputData Handle(SaveListPatientGroupMstInputData inputData)
    {
        if (inputData == null)
        {
            return new SaveListPatientGroupMstOutputData(SaveListPatientGroupMstStatus.Failed);
        }
        else if (inputData.HpId <= 0)
        {
            return new SaveListPatientGroupMstOutputData(SaveListPatientGroupMstStatus.InvalidHpId);
        }
        else if (inputData.UserId < 0)
        {
            return new SaveListPatientGroupMstOutputData(SaveListPatientGroupMstStatus.InvalidUserId);
        }

        var listDataInputs = ConvertToListModel(inputData.SaveListPatientGroupMstInputs);
        foreach (var model in listDataInputs)
        {
            if (model.GroupId <= 0)
            {
                return new SaveListPatientGroupMstOutputData(SaveListPatientGroupMstStatus.InvalidGroupId);
            }
            else if (model.GroupName.Length == 0 || model.GroupName.Length > 20)
            {
                return new SaveListPatientGroupMstOutputData(SaveListPatientGroupMstStatus.InvalidGroupName);
            }
            else if (listDataInputs.Count(x => x.GroupId == model.GroupId) > 1)
            {
                return new SaveListPatientGroupMstOutputData(SaveListPatientGroupMstStatus.DuplicateGroupId);
            }
            else if (listDataInputs.Count(x => x.GroupName.Equals(model.GroupName)) > 1)
            {
                return new SaveListPatientGroupMstOutputData(SaveListPatientGroupMstStatus.DuplicateGroupName);
            }
            foreach (var detail in model.Details)
            {
                if (detail.GroupCode.Length == 0 || detail.GroupCode.Length > 2)
                {
                    return new SaveListPatientGroupMstOutputData(SaveListPatientGroupMstStatus.InvalidDetailGroupCode);
                }
                else if (detail.GroupDetailName.Length == 0 || detail.GroupDetailName.Length > 30)
                {
                    return new SaveListPatientGroupMstOutputData(SaveListPatientGroupMstStatus.InvalidGroupDetailName);
                }
                else if (model.Details.Count(x => x.SeqNo == detail.SeqNo) > 1)
                {
                    return new SaveListPatientGroupMstOutputData(SaveListPatientGroupMstStatus.DuplicateGroupDetailSeqNo);
                }
                else if (model.Details.Count(x => x.GroupCode.Equals(detail.GroupCode)) > 1)
                {
                    return new SaveListPatientGroupMstOutputData(SaveListPatientGroupMstStatus.DuplicateGroupDetailCode);
                }
                else if (model.Details.Count(x => x.GroupDetailName.Equals(detail.GroupDetailName)) > 1)
                {
                    return new SaveListPatientGroupMstOutputData(SaveListPatientGroupMstStatus.DuplicateGroupDetailName);
                }
            }
        }

        try
        {
            if (_patientGroupMstRepository.SaveListPatientGroup(inputData.HpId, inputData.UserId, listDataInputs))
            {
                return new SaveListPatientGroupMstOutputData(SaveListPatientGroupMstStatus.Successed);
            }
            return new SaveListPatientGroupMstOutputData(SaveListPatientGroupMstStatus.Failed);
        }
        catch (Exception)
        {
            return new SaveListPatientGroupMstOutputData(SaveListPatientGroupMstStatus.Failed);
        }
        finally
        {
            _patientGroupMstRepository.ReleaseResource();
        }
    }
    private List<PatientGroupMstModel> ConvertToListModel(List<SaveListPatientGroupMstInputItem> inputItem)
    {
        List<PatientGroupMstModel> model = new();
        foreach (var input in inputItem)
        {
            List<PatientGroupDetailModel> listDetails = new();
            foreach (var item in input.Details)
            {
                listDetails.Add(new PatientGroupDetailModel(
                                    item.GroupId,
                                    item.GroupCode,
                                    item.SeqNo,
                                    0,
                                    item.GroupDetailName
                                ));
            }
            model.Add(
                    new PatientGroupMstModel(
                        input.GroupId,
                        input.GroupId,
                        input.GroupName,
                        listDetails
                ));
        }

        return model;
    }
}
