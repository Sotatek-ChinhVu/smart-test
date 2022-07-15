using Domain.CommonObject;
using Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.OrdInfs
{
    public class OrdInfMst
    {
        public HpId HpId { get; private set; }
        public RaiinNo RaiinNo { get; private set; }
        public RpNo RpNo { get; private set; }
        public RpEdaNo RpEdaNo { get; private set; }
        public PtId PtId { get; set; }
        public SinDate SinDate { get; private set; }
        public HokenPid HokenPid { get; private set; }
        public OdrKouiKbn OdrKouiKbn { get; private set; }
        public RpName RpName { get; private set; }
        public InoutKbn InoutKbn { get; private set; }
        public SikyuKbn SikyuKbn { get; private set; }
        public SyohoSbt SyohoSbt { get; private set; }
        public SanteiKbn SanteiKbn { get; private set; }
        public TosekiKbn TosekiKbn { get; private set; }
        public int DaysCnt { get; private set; }
        public int SortNo { get; private set; }
        public int IsDeleted { get; private set; }
        public DateTime CreateDate { get; private set; }
        public UserId CreateId { get; private set; }
        public string? CreateMachine { get; private set; }
        public DateTime UpdateDate { get; private set; }
        public UserId UpdateId { get; private set; }
        public string? UpdateMachine { get; private set; }
        public OrderId Id { get; private set; }
        public GroupKoui GroupKoui { get; private set; }

        public OrdInfMst(int hpId, long raiinNo, long rpNo, long rpEdaNo, long ptId, int sinDate, int hokenPid, int odrKouiKbn, string? rpName, int inoutKbn, int sikyuKbn, int syohoSbt, int santeiKbn, int tosekiKbn, int daysCnt, int sortNo, int isDeleted, DateTime createDate, int createId, string? createMachine, DateTime updateDate, int updateId, string? updateMachine, long id)
        {

            HpId = HpId.From(hpId);
            RaiinNo = RaiinNo.From(raiinNo);
            RpNo = RpNo.From(rpNo);
            RpEdaNo = RpEdaNo.From(rpEdaNo);
            PtId = PtId.From(ptId);
            SinDate = SinDate.From(sinDate);
            HokenPid = HokenPid.From(hokenPid);
            OdrKouiKbn = OdrKouiKbn.From(odrKouiKbn);
            RpName = RpName.From(rpName);
            InoutKbn = InoutKbn.From(inoutKbn);
            SikyuKbn = SikyuKbn.From(sikyuKbn);
            SyohoSbt = SyohoSbt.From(syohoSbt);
            SanteiKbn = SanteiKbn.From(santeiKbn);
            TosekiKbn = TosekiKbn.From(tosekiKbn);
            DaysCnt = daysCnt;
            SortNo = sortNo;
            IsDeleted = isDeleted;
            CreateDate = createDate;
            CreateId = UserId.From(createId);
            CreateMachine = createMachine;
            UpdateDate = updateDate;
            UpdateId = UserId.From(updateId);
            UpdateMachine = updateMachine;
            Id = OrderId.From(id);
            GroupKoui = GroupKoui.From(OdrKouiKbn.From(odrKouiKbn));
        }
    }
}
