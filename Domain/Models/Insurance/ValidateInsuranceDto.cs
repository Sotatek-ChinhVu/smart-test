using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Insurance
{
    public class ValidateInsuranceDto
    {
        public int HpId { get; set; }

        public long PtId { get; set; }

        public int PtBirthday { get; set; }

        public long SeqNo { get; set; }

        public int HokenSbtCd { get; set; }

        public int HokenPid { get; set; }

        public int HokenKbn { get; set; }

        public string HokenMemo { get; set; } = string.Empty;

        public int IsDeleted { get; set; }

        public int StartDate { get; set; }

        public int EndDate { get; set; }

        public bool IsAddNew { get; set; }

        public HokenInDtoRequest HokenInf { get; set; } = new HokenInDtoRequest();

        public KohiInfDtoRequest Kohi1 { get; set; } = new KohiInfDtoRequest();

        public KohiInfDtoRequest Kohi2 { get; set; } = new KohiInfDtoRequest();

        public KohiInfDtoRequest Kohi3 { get; set; } = new KohiInfDtoRequest();

        public KohiInfDtoRequest Kohi4 { get; set; } = new KohiInfDtoRequest();

    }

    public class KohiInfDtoRequest
    {
        public List<ConfirmDateModel> ConfirmDateList { get; set; } = new List<ConfirmDateModel>();

        public string FutansyaNo { get; set; } = string.Empty;

        public string JyukyusyaNo { get; set; } = string.Empty;

        public int HokenId { get; set; }

        public int StartDate { get; set; }

        public int EndDate { get; set; }

        public int ConfirmDate { get; set; }

        public int Rate { get; set; }

        public int GendoGaku { get; set; }

        public int SikakuDate { get; set; }

        public int KofuDate { get; set; }

        public string TokusyuNo { get; set; } = string.Empty;

        public int HokenSbtKbn { get; set; }

        public string Houbetu { get; set; } = string.Empty;

        public int HokenNo { get; set; }

        public int HokenEdaNo { get; set; }

        public int PrefNo { get; set; }

        public int SinDate { get; set; }

        public bool IsHaveKohiMst { get; set; }

        public int IsDeleted { get; set; }

        public bool IsAddNew { get; set; }
    }


    public class HokenInDtoRequest
    {
        public List<ConfirmDateModel> ConfirmDateList { get; set; } = new List<ConfirmDateModel>();

        public int HpId { get; set; }

        public long PtId { get; set; }

        public int HokenId { get; set; }

        public long SeqNo { get; set; }

        public int HokenNo { get; set; }

        public int HokenEdaNo { get; set; }

        public int HokenKbn { get; set; }

        public string HokensyaNo { get; set; } = string.Empty;

        public string Kigo { get; set; } = string.Empty;

        public string Bango { get; set; } = string.Empty;

        public string EdaNo { get; set; } = string.Empty;

        public int HonkeKbn { get; set; }

        public int StartDate { get; set; }

        public int EndDate { get; set; }

        public int SikakuDate { get; set; }

        public int KofuDate { get; set; }

        public int ConfirmDate { get; set; }

        // detail 1
        public int KogakuKbn { get; set; }

        public int TasukaiYm { get; set; }

        public int TokureiYm1 { get; set; }

        public int TokureiYm2 { get; set; }

        public int GenmenKbn { get; set; }

        public int GenmenRate { get; set; }

        public int GenmenGaku { get; set; }

        public int SyokumuKbn { get; set; }

        public int KeizokuKbn { get; set; }

        public string Tokki1 { get; set; } = string.Empty;

        public string Tokki2 { get; set; } = string.Empty;

        public string Tokki3 { get; set; } = string.Empty;

        public string Tokki4 { get; set; } = string.Empty;

        public string Tokki5 { get; set; } = string.Empty;

        //2
        public string RousaiKofuNo { get; set; } = string.Empty;

        public string NenkinBango { get; set; } = string.Empty;

        public string RousaiRoudouCd { get; set; } = string.Empty;

        public string KenkoKanriBango { get; set; } = string.Empty;

        public int RousaiSaigaiKbn { get; set; }

        public string RousaiKantokuCd { get; set; } = string.Empty;

        public int RousaiSyobyoDate { get; set; }

        public int RyoyoStartDate { get; set; }

        public int RyoyoEndDate { get; set; }

        public string RousaiSyobyoCd { get; set; } = string.Empty;

        public string RousaiJigyosyoName { get; set; } = string.Empty;

        public string RousaiPrefName { get; set; } = string.Empty;

        public string RousaiCityName { get; set; } = string.Empty;

        public int RousaiReceCount { get; set; }

        public int SinDate { get; set; }

        public string JibaiHokenName { get; set; } = string.Empty;

        public string JibaiHokenTanto { get; set; } = string.Empty;

        public string JibaiHokenTel { get; set; } = string.Empty;

        public int JibaiJyusyouDate { get; set; }

        public string Houbetu { get; set; } = string.Empty;

        public string HokensyaName { get; set; } = string.Empty;

        public string HokensyaAddress { get; set; } = string.Empty;

        public string HokensyaTel { get; set; } = string.Empty;

        public List<RousaiTenkiModel> ListRousaiTenki { get; set; } = new List<RousaiTenkiModel>();

        public bool IsReceKisaiOrNoHoken { get; set; }

        public int IsDeleted { get; set; }

        public bool IsAddNew { get; set; }
    }
}
