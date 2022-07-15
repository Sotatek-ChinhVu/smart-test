using Domain.CommonObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.OrdInfDetails
{
    public class OrdInfDetailMst
    {
        public HpId HpId { get; set; }
        public RaiinNo RaiinNo { get; set; }
        public RpNo RpNo { get; set; }
        public RpEdaNo RpEdaNo { get; set; }
        public RowNo RowNo { get; set; }
        public PtId PtId { get; set; }
        public SinDate SinDate { get; set; }
        public SinKouiKbn SinKouiKbn { get; set; }
        public ItemCd ItemCd { get; set; }
        public ItemName ItemName { get; set; }
        public double Suryo { get; set; }
        public UnitName UnitName { get; set; }
        public UnitSbt UnitSbt { get; set; }
        public double TermVal { get; set; }
        public KohatuKbn KohatuKbn { get; set; }
        public int SyohoKbn { get; set; }
        public int SyohoLimitKbn { get; set; }
        public int DrugKbn { get; set; }
        public int YohoKbn { get; set; }
        public Kokuji Kokuji1 { get; set; }
        public Kokuji Kokuji2 { get; set; }
        public int IsNodspRece { get; set; }
        public IpnCd IpnCd { get; set; }
        public ItemName IpnName { get; set; }
        public int JissiKbn { get; set; }
        public DateTime? JissiDate { get; set; }
        public int JissiId { get; set; }
        public string? JissiMachine { get; set; }
        public ReqCd ReqCd { get; set; }
        public Bunkatu Bunkatu { get; set; }
        public CmtName CmtName { get; set; }
        public CmtOpt CmtOpt { get; set; }
        public string? FontColor { get; set; }
        public int CommentNewline { get; set; }

        public OrdInfDetailMst(int hpId, long raiinNo, long rpNo, long rpEdaNo, int rowNo, long ptId, int sinDate, int sinKouiKbn, string? itemCd, string? itemName, double suryo, string? unitName, int unitSbt, double termVal, int kohatuKbn, int syohoKbn, int syohoLimitKbn, int drugKbn, int yohoKbn, string? kokuji1, string? kokuji2, int isNodspRece, string? ipnCd, string? ipnName, int jissiKbn, DateTime? jissiDate, int jissiId, string? jissiMachine, string? reqCd,  string? bunkatu, string? cmtName, string? cmtOpt, string? fontColor, int commentNewline)
        {
            HpId = HpId.From(hpId);
            RaiinNo = RaiinNo.From(raiinNo);
            RpNo = RpNo.From(rpNo);
            RpEdaNo = RpEdaNo.From(rpEdaNo);
            RowNo = RowNo.From(rowNo);
            PtId = PtId.From(ptId);
            SinDate = SinDate.From(sinDate);
            SyohoLimitKbn = syohoLimitKbn;
            SinKouiKbn = SinKouiKbn.From(sinKouiKbn);
            ItemCd = ItemCd.From(itemCd);
            ItemName = ItemName.From(itemName);
            Suryo = suryo;
            UnitName = UnitName.From(unitName);
            UnitSbt = UnitSbt.From(unitSbt);
            TermVal = termVal;
            KohatuKbn = KohatuKbn.From(kohatuKbn);
            SyohoKbn = syohoKbn;
            SyohoLimitKbn = syohoKbn;
            DrugKbn = drugKbn;
            YohoKbn = yohoKbn;
            Kokuji1 = Kokuji.From(kokuji1);
            Kokuji2 = Kokuji.From(kokuji2);
            IsNodspRece = isNodspRece;
            IpnCd = IpnCd.From(ipnCd);
            IpnName = ItemName.From(ipnName);
            JissiKbn = jissiKbn;
            JissiDate = jissiDate;
            JissiId = jissiId;
            JissiMachine = jissiMachine;
            ReqCd = ReqCd.From(reqCd);
            Bunkatu = Bunkatu.From(bunkatu);
            CmtName = CmtName.From(cmtName);
            CmtOpt = CmtOpt.From(ipnCd);
            CmtOpt = CmtOpt.From(cmtOpt);
            FontColor = fontColor;
            CommentNewline = commentNewline;
        }
    }
}
