using UseCase.Online;

namespace EmrCloudApi.Responses.Online.Dto;

public class PatientInfoDto
{
    public PatientInfoDto(PatientInfoItem item)
    {
        SinDate = item.SinDate;
        Id = item.Id;
        PtId = item.PtId;
        PtNum = item.PtNum;
        RefNo = item.RefNo;
        KanaName = item.KanaName;
        Name = item.Name;
        RawBirthday = item.RawBirthday;
        Gender1 = item.Gender1;
        Gender2 = item.Gender2;
        Address = item.Address;
        OfficePost = item.OfficePost;
        Setanusi = item.Setanusi;
        ProcessingTime = item.ProcessingTime;
        InsuranceNumber = item.InsuranceNumber;
        BranchNumber = item.BranchNumber;
        KigoBango = item.KigoBango;
        Validity = item.Validity;
        UketukeStatus = item.UketukeStatus;
        ConfirmationResult = item.ConfirmationResult;
        SegmentOfResult = item.SegmentOfResult;
        ProcessingResultStatus = item.ProcessingResultStatus;
        Sex1 = item.Sex1;
        Sex2 = item.Sex2;
        DisplayedValidity = item.DisplayedValidity;
        DisplayedUketukeStatus = item.DisplayedUketukeStatus;
        Result = item.Result;
        BirthDate = item.BirthDate;
        BirthDay = item.BirthDay;
        AgeYmd = item.AgeYmd;
        IsDefault = item.IsDefault;
    }

    public int SinDate { get; private set; }

    public long Id { get; private set; }

    public long PtId { get; private set; }

    public long PtNum { get; private set; }

    public long RefNo { get; private set; }

    public string KanaName { get; private set; }

    public string Name { get; private set; }

    public int RawBirthday { get; private set; }

    public int Gender1 { get; private set; }

    public int Gender2 { get; private set; }

    public string Address { get; private set; }

    public string OfficePost { get; private set; }

    public string Setanusi { get; private set; }

    public string ProcessingTime { get; private set; }

    public string InsuranceNumber { get; private set; }

    public string BranchNumber { get; private set; }

    public string KigoBango { get; private set; }

    public string Validity { get; private set; }

    public int UketukeStatus { get; private set; }

    public string ConfirmationResult { get; private set; }

    public int SegmentOfResult { get; private set; }

    public int ProcessingResultStatus { get; private set; }

    public string Sex1 { get; private set; }

    public string Sex2 { get; private set; }

    public string DisplayedValidity { get; private set; }

    public string DisplayedUketukeStatus { get; private set; }

    public string Result { get; private set; }

    public string BirthDate { get; private set; }

    public string BirthDay { get; private set; }

    public string AgeYmd { get; private set; }

    public bool IsDefault { get; private set; }
}
