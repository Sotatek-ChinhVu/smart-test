using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Tenant
{
    public class TestReport
    {
        public long PtId { get; set; }

        public long PtNum { get; set; }

        public string PtKanaName { get; set; } = string.Empty;

        public string PtName { get; set; } = string.Empty;

        public int Sex { get; set; }

        public int BirthDay { get; set; }

        public long RaiinNo { get; set; }

        public int SinYm { get; set; }

        public int SinDate { get; set; }

        public string SinId {get; set;}

        public double Suryo { get; set; }

        public string UnitName { get; set; }

        public int Count1 { get; set; }

        public double TotalSuryo { get; set; }

        public string ItemCd { get; set; }

        public string ItemCdCmt { get; set; }

        public string SrcItemCd { get; set; }

        public string ItemName { get; set; }

        public string ItemKanaName1 { get; set; }
        public string ItemKanaName2 { get; set; }
        public string ItemKanaName3 { get; set; }
        public string ItemKanaName4 { get; set; }
        public string ItemKanaName5 { get; set; }
        public string ItemKanaName6 { get; set; }
        public string ItemKanaName7 { get; set; }
        public int  KaId { get; set; }
        public string KaSname { get; set;}
        public int TantoId { get; set; }
        public string TantoSname { get; set; }
        public string SinKouiKbn { get; set; }
        public int MadokuKbn { get; set; }
        public int DrugKbn { get; set; }
        public int KouseisinKbn { get; set; }
        public int KazeiKbn { get; set; }
        public int EntenKbn { get; set; }
        public double Ten { get; set; }
        public int SyosaisinKbn { get; set; }
        public int HokenPid { get; set; }
        public int HokenKbn { get; set; }
        public string Houbetu { get; set; }
        public int HokenSbtCd { get; set; }
        public int InoutKbn { get; set; }
        public int KohatuKbn { get; set; }
        public int IsAdopted { get; set; }
        public int KizamiId { get; set; }
        public double TenDetail { get; set; }
    }
}
