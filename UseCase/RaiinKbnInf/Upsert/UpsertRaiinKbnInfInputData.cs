using UseCase.Core.Sync.Core;

namespace UseCase.RaiinKbnInf.Upsert;

public class UpsertRaiinKbnInfInputData : IInputData<UpsertRaiinKbnInfOutputData>
{
    public int HpId { get; set; }
    public int SinDate { get; set; }
    public long RaiinNo { get; set; }
    public long PtId { get; set; }
    public int GrpId { get; set; }
    public int KbnCd { get; set; }
}
