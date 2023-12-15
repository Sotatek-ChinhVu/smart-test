using Entity.Tenant;
using Helper.Common;
using Helper.Extension;

namespace Reporting.Statistics.Sta9000.Models;

public class CoRaiinInfModel
{
    public RaiinInf RaiinInf { get; private set; }
    public UketukeSbtMst UketukeSbtMst { get; private set; }
    public KaMst KaMst { get; private set; }
    public UserMst UserMst { get; private set; }

    private readonly string _raiinCmt;
    private readonly string _raiinBiko;
    private readonly int _firstVisitDate;

    public CoRaiinInfModel(RaiinInf raiinInf, UketukeSbtMst uketukeSbtMst, KaMst kaMst, UserMst userMst, string raiinCmt, string raiinBiko, int firstVisitDate)
    {
        RaiinInf = raiinInf;
        UketukeSbtMst = uketukeSbtMst;
        KaMst = kaMst;
        UserMst = userMst;
        _raiinCmt = raiinCmt;
        _raiinBiko = raiinBiko;
        _firstVisitDate = firstVisitDate;
        RaiinKbns = new();
    }

    public CoRaiinInfModel()
    {
        RaiinInf = new();
        UketukeSbtMst = new();
        KaMst = new();
        UserMst = new();
        _raiinCmt = string.Empty;
        _raiinBiko = string.Empty;
        _firstVisitDate = new();
        RaiinKbns = new();
    }

    /// <summary>
    /// 患者ID
    /// </summary>
    public long PtId
    {
        get => RaiinInf.PtId;
    }

    public string SinDate
    {
        get => CIUtil.SDateToShowSDate(RaiinInf.SinDate);
    }

    public long RaiinNo
    {
        get => RaiinInf.RaiinNo;
    }

    public long OyaRaiinNo
    {
        get => RaiinInf.OyaRaiinNo;
    }

    public string Status
    {
        get
        {
            switch (RaiinInf.Status)
            {
                case 0: return "予約";
                case 1: return "受付";
                case 3: return "一時保存";
                case 5: return "計算";
                case 7: return "精算待ち";
                case 9: return "済み";
            }
            return string.Empty;
        }
    }

    public int IsYoyaku
    {
        get => RaiinInf.IsYoyaku;
    }

    public string YoyakuTime
    {
        get
        {
            if (RaiinInf.YoyakuTime?.AsString() == string.Empty) return string.Empty;

            string value = RaiinInf.YoyakuTime ?? string.Empty.PadLeft(4, '0');
            return value.Length >= 4 ? (value.Substring(0, 2) + ":" + value.Substring(2, 2)) : string.Empty;
        }
    }

    public int YoyakuId
    {
        get => RaiinInf.YoyakuId;
    }

    public string UketukeSbt
    {
        get => UketukeSbtMst?.KbnName ?? RaiinInf.UketukeSbt.ToString();
    }

    public string UketukeTime
    {
        get
        {
            if (RaiinInf.UketukeTime?.AsString() == string.Empty) return string.Empty;

            string value = RaiinInf.UketukeTime ?? string.Empty.PadLeft(4, '0');
            value = value.PadRight(6, '0');
            return value.Length >= 6 ? (value.Substring(0, 2) + ":" + value.Substring(2, 2) + ":" + value.Substring(4, 2)) : string.Empty;
        }
    }

    public int UketukeId
    {
        get => RaiinInf.UketukeId;
    }

    public int UketukeNo
    {
        get => RaiinInf.UketukeNo;
    }

    public string SinStartTime
    {
        get
        {
            if (RaiinInf.SinStartTime?.AsString() == string.Empty) return string.Empty;

            string value = RaiinInf.SinStartTime ?? string.Empty.PadLeft(4, '0');
            value = value.PadRight(6, '0');
            return value.Length >= 6 ? (value.Substring(0, 2) + ":" + value.Substring(2, 2) + ":" + value.Substring(4, 2)) : string.Empty;
        }
    }

    public string SinEndTime
    {
        get
        {
            if (RaiinInf.SinEndTime?.AsString() == string.Empty) return string.Empty;

            string value = RaiinInf.SinEndTime ?? string.Empty.PadLeft(4, '0');
            value = value.PadRight(6, '0');
            return value.Length >= 6 ? (value.Substring(0, 2) + ":" + value.Substring(2, 2) + ":" + value.Substring(4, 2)) : string.Empty;
        }
    }

    public string KaikeiTime
    {
        get
        {
            if (RaiinInf.KaikeiTime?.AsString() == string.Empty) return string.Empty;

            string value = RaiinInf.KaikeiTime ?? string.Empty.PadLeft(4, '0');
            value = value.PadRight(6, '0');
            return value.Length >= 6 ? (value.Substring(0, 2) + ":" + value.Substring(2, 2) + ":" + value.Substring(4, 2)) : string.Empty;
        }
    }

    public int KaikeiId
    {
        get => RaiinInf.KaikeiId;
    }

    public string KaName
    {
        get => KaMst?.KaSname ?? RaiinInf.KaId.ToString();
    }

    public int TantoId
    {
        get => RaiinInf.TantoId;
    }

    public string TantoName
    {
        get => UserMst?.Sname ?? RaiinInf.TantoId.ToString();
    }

    public int HokenPid
    {
        get => RaiinInf.HokenPid;
    }

    public string SyosaisinKbn
    {
        get
        {
            switch (RaiinInf.SyosaisinKbn)
            {
                case 0:
                case 2: return "なし";
                case 1: return "初診";
                case 3: return "再診";
                case 4: return "電話再診";
                case 5: return "自費";
                case 6: return "同日初診";
                case 7: return "再診(２科目)";
                case 8: return "電話再診(２科目)";
            }
            return string.Empty;
        }
    }

    public string SyosaisinKbnS
    {
        get
        {
            switch (RaiinInf.SyosaisinKbn)
            {
                case 0:
                case 2: return "なし";
                case 1: return "初診";
                case 3: return "再診";
                case 4: return "電話";
                case 5: return "自費";
                case 6: return "同初";
                case 7: return "再２";
                case 8: return "電２";
            }
            return string.Empty;
        }
    }

    public int JikanKbnCd
    {
        get => RaiinInf.JikanKbn;
    }

    public string JikanKbn
    {
        get
        {
            switch (RaiinInf.JikanKbn)
            {
                case 0: return "時間内";
                case 1: return "時間外";
                case 2: return "休日";
                case 3: return "深夜";
                case 4: return "夜間早朝";
                case 5: return "夜間小特";
                case 6: return "休日小特";
                case 7: return "深夜小特";
            }
            return string.Empty;
        }
    }

    public string RaiinCmt
    {
        get => _raiinCmt.AsString().Replace(Environment.NewLine, "⏎");
    }

    public string RaiinBiko
    {
        get => _raiinBiko.AsString().Replace(Environment.NewLine, "⏎");
    }

    /// <summary>
    /// 来院区分
    /// </summary>
    public struct RaiinKbn
    {
        public int GrpId { get; set; }
        public string GrpName { get; set; }
        public string KbnName { get; set; }

        public RaiinKbn(int grpId, string grpName, string kbnName)
        {
            GrpId = grpId;
            GrpName = grpName;
            KbnName = kbnName;
        }
    };

    /// <summary>
    /// 来院区分
    /// </summary>
    public List<RaiinKbn> RaiinKbns { get; set; }

    /// <summary>
    /// 初回来院日
    /// </summary>
    public string FirstVisitDate
    {
        get => CIUtil.SDateToShowSDate(_firstVisitDate);
    }

    /// <summary>
    /// 初回来院日フラグ
    /// </summary>
    public string IsFirstVisitDate
    {
        get => RaiinInf.SinDate == _firstVisitDate ? "*" : "";
    }
}
