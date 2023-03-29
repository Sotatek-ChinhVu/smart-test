using Domain.Constant;
using Helper.Common;
using Reporting.Karte1.Model;

namespace Reporting.Mappers
{
    public class Karte1Mapper
    {
        public Information information { get; private set; }

        public List<Item> table { get; private set; }

        public Karte1Mapper(CoKarte1Model coModel)
        {
            information = new Information();
            information.dfSysDateTimeS = CIUtil.GetJapanDateTimeNow().ToString("yyyy/MM/dd HH:mm");
            information.dfPtNum = coModel.PtNum.ToString();
            information.dfFutansyaNo_K1 = coModel.KohiFutansyaNo(1).PadLeft(8, ' ');
            information.dfFutansyaNo_K2 = coModel.KohiFutansyaNo(2).PadLeft(8, ' ');
            information.dfJyukyusyaNo_K1 = coModel.KohiJyukyusyaNo(1).PadLeft(7, ' ');
            information.dfJyukyusyaNo_K2 = coModel.KohiJyukyusyaNo(2).PadLeft(7, ' ');
            information.dfHokensyaNo = coModel.HokensyaNo.PadLeft(8, ' ');
            information.dfKigoBango = coModel.KigoBango;
            information.dfPtKanaName = coModel.PtKanaName;
            information.dfPtName = coModel.PtName;
            information.dfSetainusi = coModel.SetaiNusi;
            information.dfAge = coModel.Age.ToString();
            information.sex = coModel.Sex.ToString();
            information.dfPtPostCode = coModel.PtPostCd.ToString();
            information.dfPtAddress1 = coModel.PtHomeAddress1;
            information.dfPtAddress2 = coModel.PtHomeAddress2;
            information.dfOfficeAddress = coModel.OfficeAddress;
            information.dfOfficeTel = coModel.OfficeTel;
            information.dfPtTel = coModel.PtTel;
            information.dfPtRenrakuTel = coModel.PtRenrakuTel;
            information.dfOffice = coModel.OfficeName;
            information.dfHokensyaAddress = coModel.HokensyaAddress;
            information.dfJob = coModel.Job;
            information.dfZokugara = coModel.Zokugara;
            information.dfHokensyaTel = coModel.HokensyaTel;
            information.dfHokensyaName = coModel.HokensyaName;

            CIUtil.WarekiYmd wareki = CIUtil.SDateToShowWDate3(coModel.HokenEndDate);
            information.dfHokenKigenW = wareki.Ymd;

            wareki = CIUtil.SDateToShowWDate3(coModel.BirthDay);
            information.dfBirthDateW = wareki.Ymd;

            wareki = CIUtil.SDateToShowWDate3(coModel.HokenSyutokuDate);
            information.dfHokenSyutokuW = wareki.Ymd;

            table = coModel.PtByomeiModels
                .Select(c => new Item(c))
                .ToList();
        }
        public Karte1Mapper()
        {
            information = new();
            table = new();

        }
    }

    public class Information
    {
        public string dfSysDateTimeS { get; set; } = string.Empty;
        public string dfPtNum { get; set; } = string.Empty;
        public string dfFutansyaNo_K1 { get; set; } = string.Empty;
        public string dfFutansyaNo_K2 { get; set; } = string.Empty;
        public string dfHokensyaNo { get; set; } = string.Empty;
        public string dfJyukyusyaNo_K1 { get; set; } = string.Empty;
        public string dfJyukyusyaNo_K2 { get; set; } = string.Empty;
        public string dfKigoBango { get; set; } = string.Empty;
        public string dfPtKanaName { get; set; } = string.Empty;
        public string dfPtName { get; set; } = string.Empty;
        public string dfHokenKigenW { get; set; } = string.Empty;
        public string dfSetainusi { get; set; } = string.Empty;
        public string dfBirthDateW { get; set; } = string.Empty;
        public string dfAge { get; set; } = string.Empty;
        public string sex { get; set; } = string.Empty;
        public string dfHokenSyutokuW { get; set; } = string.Empty;
        public string dfPtPostCode { get; set; } = string.Empty;
        public string dfPtAddress1 { get; set; } = string.Empty;
        public string dfPtAddress2 { get; set; } = string.Empty;
        public string dfOfficeAddress { get; set; } = string.Empty;
        public string dfOfficeTel { get; set; } = string.Empty;
        public string dfPtTel { get; set; } = string.Empty;
        public string dfPtRenrakuTel { get; set; } = string.Empty;
        public string dfOffice { get; set; } = string.Empty;
        public string dfHokensyaAddress { get; set; } = string.Empty;
        public string dfJob { get; set; } = string.Empty;
        public string dfZokugara { get; set; } = string.Empty;
        public string dfHokensyaTel { get; set; } = string.Empty;
        public string dfHokensyaName { get; set; } = string.Empty;
    }

    public class Item
    {
        public Item(CoPtByomeiModel model)
        {
            byomei = model.Byomei;
            byomeiStartDateW = _getWDate(model.StartDate);
            byomeiEndDateW = _getWDate(model.TenkiDate);
            switch (model.TenkiKbn)
            {
                case TenkiKbnConst.Cured:
                    isTenkiChusiMaru = true;
                    break;
                case TenkiKbnConst.Dead:
                    isTenkiSiboMaru = true;
                    break;
                case TenkiKbnConst.Canceled:
                    isTenkiSonota = true;
                    break;
                case TenkiKbnConst.Other:
                    isTenkiTiyuMaru = true;
                    break;
            }
        }

        private string _getWDate(int date)
        {
            string ret = "";

            if (date > 0)
            {
                CIUtil.WarekiYmd wareki = CIUtil.SDateToShowWDate3(date);

                ret = $"{wareki.Year,2} {wareki.Month,2} {wareki.Day,2}";
            }

            return ret;
        }

        public string byomei { get; set; }

        public string byomeiStartDateW { get; set; }

        public string byomeiEndDateW { get; set; }

        public bool isTenkiChusiMaru { get; set; }

        public bool isTenkiSiboMaru { get; set; }

        public bool isTenkiSonota { get; set; }

        public bool isTenkiTiyuMaru { get; set; }
    }
}
