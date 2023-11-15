using Helper.Common;
using Helper.Extension;

namespace UseCase.Online;

public class PatientInfoItem
{
    public PatientInfoItem()
    {
        KanaName = string.Empty;
        Name = string.Empty;
        Address = string.Empty;
        OfficePost = string.Empty;
        Setanusi = string.Empty;
        ProcessingTime = string.Empty;
        InsuranceNumber = string.Empty;
        BranchNumber = string.Empty;
        KigoBango = string.Empty;
        Validity = string.Empty;
        ConfirmationResult = string.Empty;
    }

    public PatientInfoItem(int sinDate, long id, long ptId, long ptNum, long refNo, string kanaName, string name, int rawBirthday, int gender1, int gender2, string address, string officePost, string setanusi, string processingTime, string insuranceNumber, string branchNumber, string kigoBango, string validity, int uketukeStatus, string confirmationResult, int segmentOfResult, int processingResultStatus)
    {
        SinDate = sinDate;
        Id = id;
        PtId = ptId;
        PtNum = ptNum;
        RefNo = refNo;
        KanaName = kanaName;
        Name = name;
        RawBirthday = rawBirthday;
        Gender1 = gender1;
        Gender2 = gender2;
        Address = address;
        OfficePost = officePost;
        Setanusi = setanusi;
        ProcessingTime = processingTime;
        InsuranceNumber = insuranceNumber;
        BranchNumber = branchNumber;
        KigoBango = kigoBango;
        Validity = validity;
        UketukeStatus = uketukeStatus;
        ConfirmationResult = confirmationResult;
        SegmentOfResult = segmentOfResult;
        ProcessingResultStatus = processingResultStatus;
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

    public string Sex1 => IsDefault ? string.Empty : CIUtil.GetDisplayGender(Gender1);

    public string Sex2 => IsDefault ? string.Empty : CIUtil.GetDisplayGender(Gender2);

    public string DisplayedValidity => Validity.AsInteger() == 1 ? "有効" : string.Empty;

    public string DisplayedUketukeStatus
    {
        get
        {
            switch (UketukeStatus)
            {
                case 1:
                    return "済";
                case 9:
                    return "削除";
                default:
                    return string.Empty;
            }
        }
    }

    public string Result
    {
        get
        {
            if (SegmentOfResult == 1 && ProcessingResultStatus == 1)
            {
                return "正常";
            }
            if (SegmentOfResult == 9 && ProcessingResultStatus == 2)
            {
                return "エラー";
            }
            return string.Empty;
        }
    }

    public string BirthDate => CIUtil.SDateToShowWSDate(RawBirthday);

    public string BirthDay => CIUtil.SDateToShowWDate(RawBirthday);

    public string AgeYmd => IsDefault ? string.Empty : CIUtil.SDateToDecodeAge(RawBirthday.AsString(), SinDate.AsString());

    public bool IsDefault => Id == 0;
}
