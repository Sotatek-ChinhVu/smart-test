using UseCase.PatientInfor;

namespace Interactor.PatientInfor.SortPatientCommon;

public interface ISortPatientCommon
{
    List<PatientInfoWithGroup> SortData(List<PatientInfoWithGroup> ptInfList, Dictionary<string, string> sortData, int pageIndex, int pageSize);
}
