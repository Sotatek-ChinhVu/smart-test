using Domain.Models.Diseases;
using Helper.Common;
using Helper.Constants;

namespace EmrCloudApi.Responses.Diseases;

public class ByomeiByPtIdDto
{
    public ByomeiByPtIdDto(PtDiseaseModel model)
    {
        SeqNo = model.SeqNo;
        Id = model.Id;
        PtId = model.PtId;
        HosokuCmt = model.HosokuCmt;
        CreateUser = model.CreateUser;
        UpdateUser = model.UpdateUser;
        IsDeleted = model.IsDeleted == 1;
        HokenPid = model.HokenPid;
        IsImportant = model.IsImportant;
        StartDate = model.StartDate;
        Byomei = model.Byomei;
        SyobyoKbn = model.SyubyoKbn;
        NanByoCd = model.NanbyoCd;
        SikkanKbn = model.SikkanKbn;
        TenkiKbn = model.TenkiKbn;
        TenkiDate = model.TenkiDate;
        IsNodspRece = model.IsNodspRece;
        IsNodspKarte = model.IsNodspKarte;
        PrefixSuffixList = model.PrefixSuffixList;
        CreateDate = model.CreateDate;
    }

    public long SeqNo { get; private set; }

    public long Id { get; private set; }

    public long PtId { get; private set; }

    public string HosokuCmt { get; private set; }

    public string CreateUser { get; private set; }

    public string UpdateUser { get; private set; }

    public bool IsDeleted { get; private set; }

    public int HokenPid { get; private set; }

    public int IsImportant { get; private set; }

    public int StartDate { get; private set; }

    public string Byomei { get; private set; }

    public int SyobyoKbn { get; private set; }

    public int NanByoCd { get; private set; }

    public int SikkanKbn { get; private set; }

    public int TenkiKbn { get; private set; }

    public int TenkiDate { get; private set; }

    public int IsNodspRece { get; private set; }

    public int IsNodspKarte { get; private set; }

    public List<PrefixSuffixModel> PrefixSuffixList { get; private set; }

    public string CreateDate { get; private set; }

    public string StartDateDisplay
    {
        get => CIUtil.SDateToShowSDate(StartDate);
    }

    public string IsDspImportant
    {
        get => IsImportant == 1 ? "✓" : string.Empty;
    }

    public string MainByomei
    {
        get => SyobyoKbn == 1 ? "✓" : string.Empty;
    }

    public string ImportanceFlag
    {
        get => string.Empty;
    }

    public string Dangyue
    {
        get => string.Empty;
    }

    public string HokenByomei
    {
        get => string.Empty;
    }

    public string Kaiji
    {
        get => string.Empty;
    }

    public string DisplayNanByo => NanByoCd == NanbyoConst.Gairai ? "難病" : string.Empty;

    public string TenkiDateDisplay
    {
        get => CIUtil.SDateToShowSDate(TenkiDate);
    }

    public string IsDspRece
    {
        get => IsNodspRece == 0 && Id != 0 ? "✓" : string.Empty;
    }

    public string IsDspKarte
    {
        get => IsNodspKarte == 0 && Id != 0 ? "✓" : string.Empty;
    }
}
