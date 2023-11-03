using Domain.Common;
using Domain.Models.Diseases;
using Domain.Models.Family;
using Domain.Models.FlowSheet;
using Domain.Models.KarteInfs;
using Domain.Models.MonshinInf;
using Domain.Models.NextOrder;
using Domain.Models.OrdInfs;
using Domain.Models.SpecialNote.ImportantNote;
using Domain.Models.SpecialNote.PatientInfo;
using Domain.Models.SpecialNote.SummaryInf;

namespace Domain.Models.Medical;

public interface ISaveMedicalRepository : IRepositoryBase
{
    bool Upsert(int hpId, long ptId, long raiinNo, int sinDate, int syosaiKbn, int jikanKbn, int hokenPid, int santeiKbn, int tantoId, int kaId, string uketukeTime, string sinStartTime, string sinEndTime, byte status, List<OrdInfModel> odrInfs, KarteInfModel karteInfModel, int userId, List<FamilyModel> familyList, List<NextOrderModel> rsvkrtOrderInfModels, SummaryInfModel summaryInfModel, ImportantNoteModel importantNoteModel, PatientInfoModel patientInfoModel, List<PtDiseaseModel> ptDiseaseModels, List<FlowSheetModel> flowSheetData, MonshinInforModel monshin);
}
