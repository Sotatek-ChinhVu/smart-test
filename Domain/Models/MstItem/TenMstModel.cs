using Entity.Tenant;

namespace Domain.Models.MstItem
{
    public class TenMstModel
    {
        public TenMst TenMst { get; }

        public TenMstModel(TenMst tenMst)
        {
            TenMst = tenMst;
        }

        public int HpId { get; private set; }
        public string ItemCd { get; private set; } = string.Empty;
        public int SinKouiKbn { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string OdrUnitName { get; private set; } = string.Empty;
        public string CnvUnitName { get; private set; } = string.Empty;
        public int IsNodspRece { get; private set; }
        public int YohoKbn { get; private set; }
        public double OdrTermVal { get; private set; }
        public double CnvTermVal { get; private set; }
        public string YjCd { get; private set; } = string.Empty;
        public string KensaItemCd { get; private set; } = string.Empty;
        public int KensaItemSeqNo { get; private set; }
        public int KohatuKbn { get; private set; }
        public double Ten { get; private set; }
        public int HandanGrpKbn { get; private set; }
        public string IpnNameCd { get; private set; } = string.Empty;
        public int CmtCol1 { get; private set; }
        public int CmtCol2 { get; private set; }
        public int CmtCol3 { get; private set; }
        public int CmtCol4 { get; private set; }
        public int CmtColKeta1 { get; private set; }
        public int CmtColKeta2 { get; private set; }
        public int CmtColKeta3 { get; private set; }
        public int CmtColKeta4 { get; private set; }
        public string MinAge { get; private set; } = string.Empty;
        public string MaxAge { get; private set; } = string.Empty;
        public int StartDate { get; private set; }
        public int EndDate { get; private set; }
        public string MasterSbt { get; private set; } = string.Empty;
        public int BuiKbn { get; private set; }
        public string CdKbn { get; private set; } = string.Empty;
        public int CdKbnNo { get; private set; }
        public int CdEdano { get; private set; }
        public string Kokuji1 { get; private set; } = string.Empty;
        public string Kokuji2 { get; private set; } = string.Empty;
        public string DrugKbn { get; private set; } = string.Empty;
        public string ReceName { get; private set; } = string.Empty;
        public string SanteiItemCd { get; private set; } = string.Empty;
        public int JihiSbt { get; private set; }

    }
}
