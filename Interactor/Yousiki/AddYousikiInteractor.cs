using Domain.Models.PatientInfor;
using Domain.Models.Yousiki;
using UseCase.Yousiki.AddYousiki;

namespace Interactor.Yousiki;

public class AddYousikiInteractor : IAddYousikiInputPort
{
    private readonly IYousikiRepository _yousikiRepository;
    private readonly IPatientInforRepository _patientInforRepository;
    private const string mFree00030 = "mFree00030";
    private const string mFree00010 = "mFree00010";

    public AddYousikiInteractor(IYousikiRepository yousikiRepository, IPatientInforRepository patientInforRepository)
    {
        _yousikiRepository = yousikiRepository;
        _patientInforRepository = patientInforRepository;
    }

    public AddYousikiOutputData Handle(AddYousikiInputData inputData)
    {
        try
        {
            return new AddYousikiOutputData(string.Empty, AddYousikiStatus.Successed);
        }
        finally
        {
            _yousikiRepository.ReleaseResource();
        }
    }

    private AddYousikiOutputData ValidateData(AddYousikiInputData inputData)
    {
        if (inputData.SinYm == 0)
        {
            return new AddYousikiOutputData(mFree00030, AddYousikiStatus.InvalidYousikiSinYm);
        }
        else if (inputData.SelectDataType > 3 || inputData.SelectDataType < 1)
        {
            return new AddYousikiOutputData(mFree00030, AddYousikiStatus.InvalidYousikiSelectDataType0);
        }
        var ptId = _patientInforRepository.IsPatientExist(inputData.HpId, inputData.PtNum);
        if (inputData.PtNum == 0 || ptId == 0)
        {
            switch (inputData.SelectDataType)
            {
                case 1:
                    return new AddYousikiOutputData(mFree00010, AddYousikiStatus.InvalidYousikiSelectDataType1);
                case 2:
                    return new AddYousikiOutputData(mFree00010, AddYousikiStatus.InvalidYousikiSelectDataType2);
                case 3:
                    return new AddYousikiOutputData(mFree00010, AddYousikiStatus.InvalidYousikiSelectDataType3);
            }
        }
        else
        {

        }
        return new AddYousikiOutputData(string.Empty, AddYousikiStatus.ValidateSuccessed);
    }
}
