using EventProcessor.Model;

namespace EventProcessor.Interfaces;

public interface IEventProcessorService
{
    bool AddListAuditTrailLog(List<ArgumentModel> listAuditTraiLogModels);

    bool BundleExecute(BundleArgumentModel bundleArg);

    CommonDataModel GetCommonData(ArgumentModel auditTraiLogModel);

    bool RunCommonProgram(RenkeiModel renkeiModel, CommonDataModel common);

    Renkei040DataModel GetRenkei040Data(long ptId, int sinDate, long raiinNo);

    Renkei050DataModel GetRenkei050Data(long ptId, int sinDate, long raiinNo);

    Renkei060DataModel GetRenkei060Data(long ptId);

    Renkei070DataModel GetRenkei070Data(long ptId);

    Renkei080DataModel GetRenkei080Data(long ptId, int sinDate, long raiinNo);

    Renkei100DataModel GetRenkei100Data(long ptId, int sinDate, long raiinNo);

    List<Renkei130DataModel> GetRenkei130Data(ArgumentModel arg);

    List<Renkei260OdrInfModel> GetRenkei260Data(long ptId, int sinDate);

    List<Renkei270KarteInfModel> GetRenkei270Data(long ptId, int sinDate);

    Renkei280DataModel GetRenkei280Data(long ptId, int sinDate, long raiinNo);

    Renkei330DataModel GetRenkei330Data(long ptId, int sinDate, long raiinNo, string eventCd, string hosoku);

    Renkei350DataModel GetRenkei350Data(long ptId, int sinDate);

    Renkei360DataModel GetRenkei360Data(long ptId, int sinDate, long raiinNo);

    bool CheckOdrItem(long ptId, int sinDate, long raiinNo, string biko);
}
