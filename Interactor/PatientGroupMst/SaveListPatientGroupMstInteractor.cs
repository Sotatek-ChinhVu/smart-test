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
        if (inputData.HpId <= 0)
        {
            return new SaveListPatientGroupMstOutputData(SaveListPatientGroupMstStatus.InvalidHpId);
        }
        else if (inputData.UserId < 0)
        {
            return new SaveListPatientGroupMstOutputData(SaveListPatientGroupMstStatus.InvalidUserId);
        }
        try
        {
            if (_patientGroupMstRepository.SaveListPatientGroup(inputData.HpId, inputData.UserId, ConvertToListModel(inputData.SaveListPatientGroupMstInputs)))
            {
                return new SaveListPatientGroupMstOutputData(SaveListPatientGroupMstStatus.Successed);
            }
            return new SaveListPatientGroupMstOutputData(SaveListPatientGroupMstStatus.Failed);
        }
        catch (Exception)
        {
            return new SaveListPatientGroupMstOutputData(SaveListPatientGroupMstStatus.Failed);
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
                                    item.SortNo,
                                    item.GroupDetailName
                                ));
            }
            model.Add(
                    new PatientGroupMstModel(
                        input.GroupId,
                        input.SortNo,
                        input.GroupName,
                        listDetails
                ));
        }

        return model;
    }
}
