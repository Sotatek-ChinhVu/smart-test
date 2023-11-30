using Domain.Common;
using Reporting.MedicalRecordWebId.Model;

namespace Reporting.MedicalRecordWebId.DB;

public interface ICoMedicalRecordWebIdFinder : IRepositoryBase
{
    CoHpInfModel GetHpInf(int hpId, int sinDate);

    CoPtInfModel GetPtInf(int hpId, long ptId);

    CoPtJibkarModel GetPtJibkar(int hpId, long ptId);
}
