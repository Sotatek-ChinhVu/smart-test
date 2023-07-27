using Domain.Models.DrugInfor;
using Interactor.DrugInfor.CommonDrugInf;
using UseCase.DrugInfor.Get;

namespace Interactor.DrugInfor;

public class GetDrugInforInteractor : IGetDrugInforInputPort
{
    private readonly IGetCommonDrugInf _getCommonDrugInf;

    public GetDrugInforInteractor(IGetCommonDrugInf getCommonDrugInf)
    {
        _getCommonDrugInf = getCommonDrugInf;
    }

    public GetDrugInforOutputData Handle(GetDrugInforInputData inputData)
    {
        try
        {
            if (inputData.HpId <= 0)
            {
                return new GetDrugInforOutputData(new DrugInforModel(), GetDrugInforStatus.InValidHpId);
            }

            if (inputData.SinDate <= 0)
            {
                return new GetDrugInforOutputData(new DrugInforModel(), GetDrugInforStatus.InValidSindate);
            }

            if (string.IsNullOrEmpty(inputData.ItemCd))
            {
                return new GetDrugInforOutputData(new DrugInforModel(), GetDrugInforStatus.InValidItemCd);
            }
            var data = _getCommonDrugInf.GetDrugInforModel(inputData.HpId, inputData.SinDate, inputData.ItemCd);
            return new GetDrugInforOutputData(data, GetDrugInforStatus.Successed);
        }
        finally
        {
            _getCommonDrugInf.ReleaseResources();
        }
    }
}
