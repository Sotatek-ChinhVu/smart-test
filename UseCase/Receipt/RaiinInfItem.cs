using Domain.Models.Receipt;
using Helper.Common;
using Helper.Extension;

namespace UseCase.Receipt;

public class RaiinInfItem
{
    public RaiinInfItem(RaiinInfModel model)
    {
        PtId = model.PtId;
        SinDate = model.SinDate;
        RaiinNo = model.RaiinNo;
        UketukeTime = model.UketukeTime;
        SinEndTime = model.SinEndTime;
        Status = model.Status;
    }

    public long PtId { get; private set; }

    public int SinDate { get; private set; }

    public long RaiinNo { get; private set; }

    public string RaiinNoBinding => RaiinNo > 0 ? RaiinNo.AsString() : string.Empty;

    public string UketukeTime { get; private set; }

    public string UketukeTimeBinding => HourAndMinuteFormat(UketukeTime);

    public string SinEndTime { get; private set; }

    public string SinEndTimeBinding => HourAndMinuteFormat(SinEndTime);

    public int Status { get; private set; }

    private string HourAndMinuteFormat(string value)
    {
        string sResult = string.Empty;
        if (!string.IsNullOrEmpty(value) && value.AsInteger() != 0)
        {
            if (value.Length > 4)
            {
                sResult = CIUtil.Copy(value, 1, 4);
            }
            else
            {
                sResult = value;
            }
            sResult = sResult.PadLeft(4, '0');
            sResult = sResult.Insert(2, ":");
        }
        return sResult;
    }
}
