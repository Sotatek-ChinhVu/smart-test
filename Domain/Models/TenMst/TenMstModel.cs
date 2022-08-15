using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.TenMst
{
    public class TenMstModel
    {
        public TenMstModel(int hpId, string itemCd, int startDate, int endDate, string masterSbt, int sinKouiKbn, string name, string kanaName1, string kanaName2, string kanaName3, string kanaName4, string kanaName5, string kanaName6, string kanaName7, string ryosyuName, string receName, int tenId, double ten, string receUnitCd, string receUnitName, string odrUnitName, string cnvUnitName, double odrTermVal, double cnvTermVal, double defaultVal, int isAdopted, int koukiKbn, int hokatuKensa, int byomeiKbn, int igakukanri, int jitudayCount, int jituday, int dayCount, int drugKanrenKbn, int kizamiId, int kizamiMin, int kizamiMax, int kizamiVal, double kizamiTen, int kizamiErr, int maxCount, int maxCountErr, string tyuCd, string tyuSeq, int tusokuAge, string minAge, string maxAge, int timeKasanKbn, int futekiKbn, int futekiSisetuKbn, int syotiNyuyojiKbn, int lowWeightKbn, int handanKbn, int handanGrpKbn, int teigenKbn, int sekituiKbn, int keibuKbn, int autoHougouKbn, int gairaiKanriKbn, int tusokuTargetKbn, int hokatuKbn, int tyoonpaNaisiKbn, int autoFungoKbn, int tyoonpaGyokoKbn, int gazoKasan, int kansatuKbn, int masuiKbn, int fukubikuNaisiKasan, int fukubikuKotunanKasan, int masuiKasan, int moniterKasan, int toketuKasan, string tenKbnNo, int shortstayOpe, int buiKbn, int sisetucd1, int sisetucd2, int sisetucd3, int sisetucd4, int sisetucd5, int sisetucd6, int sisetucd7, int sisetucd8, int sisetucd9, int sisetucd10, string agekasanMin1, string agekasanMax1, string agekasanCd1, string agekasanMin2, string agekasanMax2, string agekasanCd2, string agekasanMin3, string agekasanMax3, string agekasanCd3, string agekasanMin4, string agekasanMax4, string agekasanCd4, int kensaCmt, int madokuKbn, int sinkeiKbn, int seibutuKbn, int zoueiKbn, int drugKbn, int zaiKbn, int capacity, int kohatuKbn, int tokuzaiAgeKbn, int sansoKbn, int tokuzaiSbt, int maxPrice, int maxTen, string syukeiSaki, string cdKbn, int cdSyo, int cdBu, int cdKbnno, int cdEdano, int cdKouno, string kokujiKbn, int kokujiSyo, int kokujiBu, int kokujiKbnNo, int kokujiEdaNo, int kokujiKouNo, string kokuji1, string kokuji2, int kohyoJun, string yjCd, string yakkaCd, int syusaiSbt, string syohinKanren, int updDate, int delDate, int keikaDate, int rousaiKbn, int sisiKbn, int shotCnt, int isNosearch, int isNodspPaperRece, int isNodspRece, int isNodspRyosyu, int isNodspKarte, int jihiSbt, int kazeiKbn, int yohoKbn, string ipnNameCd, int fukuyoRise, int fukuyoMorning, int fukuyoDaytime, int fukuyoNight, int fukuyoSleep, int suryoRoundupKbn, int kouseisinKbn, string santeiItemCd, int santeigaiKbn, string kensaItemCd, int kensaItemSeqNo, string renkeiCd1, string renkeiCd2, int saiketuKbn, int cmtKbn, int cmtCol1, int cmtColKeta1, int cmtCol2, int cmtColKeta2, int cmtCol3, int cmtColKeta3, int cmtCol4, int cmtColKeta4, int selectCmtId, int chusyaDrugSbt, int kensaFukusuSantei, int ageCheck, int kokujiBetuno, int kokujiKbnno1, int cmtSbt, int isNodspYakutai, double zaikeiPoint, int kensaLabel)
        {
            HpId = hpId;
            ItemCd = itemCd;
            StartDate = startDate;
            EndDate = endDate;
            MasterSbt = masterSbt;
            SinKouiKbn = sinKouiKbn;
            Name = name;
            KanaName1 = kanaName1;
            KanaName2 = kanaName2;
            KanaName3 = kanaName3;
            KanaName4 = kanaName4;
            KanaName5 = kanaName5;
            KanaName6 = kanaName6;
            KanaName7 = kanaName7;
            RyosyuName = ryosyuName;
            ReceName = receName;
            TenId = tenId;
            Ten = ten;
            ReceUnitCd = receUnitCd;
            ReceUnitName = receUnitName;
            OdrUnitName = odrUnitName;
            CnvUnitName = cnvUnitName;
            OdrTermVal = odrTermVal;
            CnvTermVal = cnvTermVal;
            DefaultVal = defaultVal;
            IsAdopted = isAdopted;
            KoukiKbn = koukiKbn;
            HokatuKensa = hokatuKensa;
            ByomeiKbn = byomeiKbn;
            Igakukanri = igakukanri;
            JitudayCount = jitudayCount;
            Jituday = jituday;
            DayCount = dayCount;
            DrugKanrenKbn = drugKanrenKbn;
            KizamiId = kizamiId;
            KizamiMin = kizamiMin;
            KizamiMax = kizamiMax;
            KizamiVal = kizamiVal;
            KizamiTen = kizamiTen;
            KizamiErr = kizamiErr;
            MaxCount = maxCount;
            MaxCountErr = maxCountErr;
            TyuCd = tyuCd;
            TyuSeq = tyuSeq;
            TusokuAge = tusokuAge;
            MinAge = minAge;
            MaxAge = maxAge;
            TimeKasanKbn = timeKasanKbn;
            FutekiKbn = futekiKbn;
            FutekiSisetuKbn = futekiSisetuKbn;
            SyotiNyuyojiKbn = syotiNyuyojiKbn;
            LowWeightKbn = lowWeightKbn;
            HandanKbn = handanKbn;
            HandanGrpKbn = handanGrpKbn;
            TeigenKbn = teigenKbn;
            SekituiKbn = sekituiKbn;
            KeibuKbn = keibuKbn;
            AutoHougouKbn = autoHougouKbn;
            GairaiKanriKbn = gairaiKanriKbn;
            TusokuTargetKbn = tusokuTargetKbn;
            HokatuKbn = hokatuKbn;
            TyoonpaNaisiKbn = tyoonpaNaisiKbn;
            AutoFungoKbn = autoFungoKbn;
            TyoonpaGyokoKbn = tyoonpaGyokoKbn;
            GazoKasan = gazoKasan;
            KansatuKbn = kansatuKbn;
            MasuiKbn = masuiKbn;
            FukubikuNaisiKasan = fukubikuNaisiKasan;
            FukubikuKotunanKasan = fukubikuKotunanKasan;
            MasuiKasan = masuiKasan;
            MoniterKasan = moniterKasan;
            ToketuKasan = toketuKasan;
            TenKbnNo = tenKbnNo;
            ShortstayOpe = shortstayOpe;
            BuiKbn = buiKbn;
            Sisetucd1 = sisetucd1;
            Sisetucd2 = sisetucd2;
            Sisetucd3 = sisetucd3;
            Sisetucd4 = sisetucd4;
            Sisetucd5 = sisetucd5;
            Sisetucd6 = sisetucd6;
            Sisetucd7 = sisetucd7;
            Sisetucd8 = sisetucd8;
            Sisetucd9 = sisetucd9;
            Sisetucd10 = sisetucd10;
            AgekasanMin1 = agekasanMin1;
            AgekasanMax1 = agekasanMax1;
            AgekasanCd1 = agekasanCd1;
            AgekasanMin2 = agekasanMin2;
            AgekasanMax2 = agekasanMax2;
            AgekasanCd2 = agekasanCd2;
            AgekasanMin3 = agekasanMin3;
            AgekasanMax3 = agekasanMax3;
            AgekasanCd3 = agekasanCd3;
            AgekasanMin4 = agekasanMin4;
            AgekasanMax4 = agekasanMax4;
            AgekasanCd4 = agekasanCd4;
            KensaCmt = kensaCmt;
            MadokuKbn = madokuKbn;
            SinkeiKbn = sinkeiKbn;
            SeibutuKbn = seibutuKbn;
            ZoueiKbn = zoueiKbn;
            DrugKbn = drugKbn;
            ZaiKbn = zaiKbn;
            Capacity = capacity;
            KohatuKbn = kohatuKbn;
            TokuzaiAgeKbn = tokuzaiAgeKbn;
            SansoKbn = sansoKbn;
            TokuzaiSbt = tokuzaiSbt;
            MaxPrice = maxPrice;
            MaxTen = maxTen;
            SyukeiSaki = syukeiSaki;
            CdKbn = cdKbn;
            CdSyo = cdSyo;
            CdBu = cdBu;
            CdKbnno = cdKbnno;
            CdEdano = cdEdano;
            CdKouno = cdKouno;
            KokujiKbn = kokujiKbn;
            KokujiSyo = kokujiSyo;
            KokujiBu = kokujiBu;
            KokujiKbnNo = kokujiKbnNo;
            KokujiEdaNo = kokujiEdaNo;
            KokujiKouNo = kokujiKouNo;
            Kokuji1 = kokuji1;
            Kokuji2 = kokuji2;
            KohyoJun = kohyoJun;
            YjCd = yjCd;
            YakkaCd = yakkaCd;
            SyusaiSbt = syusaiSbt;
            SyohinKanren = syohinKanren;
            UpdDate = updDate;
            DelDate = delDate;
            KeikaDate = keikaDate;
            RousaiKbn = rousaiKbn;
            SisiKbn = sisiKbn;
            ShotCnt = shotCnt;
            IsNosearch = isNosearch;
            IsNodspPaperRece = isNodspPaperRece;
            IsNodspRece = isNodspRece;
            IsNodspRyosyu = isNodspRyosyu;
            IsNodspKarte = isNodspKarte;
            JihiSbt = jihiSbt;
            KazeiKbn = kazeiKbn;
            YohoKbn = yohoKbn;
            IpnNameCd = ipnNameCd;
            FukuyoRise = fukuyoRise;
            FukuyoMorning = fukuyoMorning;
            FukuyoDaytime = fukuyoDaytime;
            FukuyoNight = fukuyoNight;
            FukuyoSleep = fukuyoSleep;
            SuryoRoundupKbn = suryoRoundupKbn;
            KouseisinKbn = kouseisinKbn;
            SanteiItemCd = santeiItemCd;
            SanteigaiKbn = santeigaiKbn;
            KensaItemCd = kensaItemCd;
            KensaItemSeqNo = kensaItemSeqNo;
            RenkeiCd1 = renkeiCd1;
            RenkeiCd2 = renkeiCd2;
            SaiketuKbn = saiketuKbn;
            CmtKbn = cmtKbn;
            CmtCol1 = cmtCol1;
            CmtColKeta1 = cmtColKeta1;
            CmtCol2 = cmtCol2;
            CmtColKeta2 = cmtColKeta2;
            CmtCol3 = cmtCol3;
            CmtColKeta3 = cmtColKeta3;
            CmtCol4 = cmtCol4;
            CmtColKeta4 = cmtColKeta4;
            SelectCmtId = selectCmtId;
            ChusyaDrugSbt = chusyaDrugSbt;
            KensaFukusuSantei = kensaFukusuSantei;
            AgeCheck = ageCheck;
            KokujiBetuno = kokujiBetuno;
            KokujiKbnno1 = kokujiKbnno1;
            CmtSbt = cmtSbt;
            IsNodspYakutai = isNodspYakutai;
            ZaikeiPoint = zaikeiPoint;
            KensaLabel = kensaLabel;
        }

        public int HpId { get; private set; }
        public string ItemCd { get; private set; }
        public int StartDate { get; private set; }
        public int EndDate { get; private set; }
        public string MasterSbt { get; private set; }
        public int SinKouiKbn { get; private set; }
        public string Name { get; private set; }
        public string KanaName1 { get; private set; }
        public string KanaName2 { get; private set; }
        public string KanaName3 { get; private set; }
        public string KanaName4 { get; private set; }
        public string KanaName5 { get; private set; }
        public string KanaName6 { get; private set; }
        public string KanaName7 { get; private set; }
        public string RyosyuName { get; private set; }
        public string ReceName { get; private set; }
        public int TenId { get; private set; }
        public double Ten { get; private set; }
        public string ReceUnitCd { get; private set; }
        public string ReceUnitName { get; private set; }
        public string OdrUnitName { get; private set; }
        public string CnvUnitName { get; private set; }
        public double OdrTermVal { get; private set; }
        public double CnvTermVal { get; private set; }
        public double DefaultVal { get; private set; }
        public int IsAdopted { get; private set; }
        public int KoukiKbn { get; private set; }
        public int HokatuKensa { get; private set; }
        public int ByomeiKbn { get; private set; }
        public int Igakukanri { get; private set; }
        public int JitudayCount { get; private set; }
        public int Jituday { get; private set; }
        public int DayCount { get; private set; }
        public int DrugKanrenKbn { get; private set; }
        public int KizamiId { get; private set; }
        public int KizamiMin { get; private set; }
        public int KizamiMax { get; private set; }
        public int KizamiVal { get; private set; }
        public double KizamiTen { get; private set; }
        public int KizamiErr { get; private set; }
        public int MaxCount { get; private set; }
        public int MaxCountErr { get; private set; }
        public string TyuCd { get; private set; }
        public string TyuSeq { get; private set; }
        public int TusokuAge { get; private set; }
        public string MinAge { get; private set; }
        public string MaxAge { get; private set; }
        public int TimeKasanKbn { get; private set; }
        public int FutekiKbn { get; private set; }
        public int FutekiSisetuKbn { get; private set; }
        public int SyotiNyuyojiKbn { get; private set; }
        public int LowWeightKbn { get; private set; }
        public int HandanKbn { get; private set; }
        public int HandanGrpKbn { get; private set; }
        public int TeigenKbn { get; private set; }
        public int SekituiKbn { get; private set; }
        public int KeibuKbn { get; private set; }
        public int AutoHougouKbn { get; private set; }
        public int GairaiKanriKbn { get; private set; }
        public int TusokuTargetKbn { get; private set; }
        public int HokatuKbn { get; private set; }
        public int TyoonpaNaisiKbn { get; private set; }
        public int AutoFungoKbn { get; private set; }
        public int TyoonpaGyokoKbn { get; private set; }
        public int GazoKasan { get; private set; }
        public int KansatuKbn { get; private set; }
        public int MasuiKbn { get; private set; }
        public int FukubikuNaisiKasan { get; private set; }
        public int FukubikuKotunanKasan { get; private set; }
        public int MasuiKasan { get; private set; }
        public int MoniterKasan { get; private set; }
        public int ToketuKasan { get; private set; }
        public string TenKbnNo { get; private set; }
        public int ShortstayOpe { get; private set; }
        public int BuiKbn { get; private set; }
        public int Sisetucd1 { get; private set; }
        public int Sisetucd2 { get; private set; }
        public int Sisetucd3 { get; private set; }
        public int Sisetucd4 { get; private set; }
        public int Sisetucd5 { get; private set; }
        public int Sisetucd6 { get; private set; }
        public int Sisetucd7 { get; private set; }
        public int Sisetucd8 { get; private set; }
        public int Sisetucd9 { get; private set; }
        public int Sisetucd10 { get; private set; }
        public string AgekasanMin1 { get; private set; }
        public string AgekasanMax1 { get; private set; }
        public string AgekasanCd1 { get; private set; }
        public string AgekasanMin2 { get; private set; }
        public string AgekasanMax2 { get; private set; }
        public string AgekasanCd2 { get; private set; }
        public string AgekasanMin3 { get; private set; }
        public string AgekasanMax3 { get; private set; }
        public string AgekasanCd3 { get; private set; }
        public string AgekasanMin4 { get; private set; }
        public string AgekasanMax4 { get; private set; }
        public string AgekasanCd4 { get; private set; }
        public int KensaCmt { get; private set; }
        public int MadokuKbn { get; private set; }
        public int SinkeiKbn { get; private set; }
        public int SeibutuKbn { get; private set; }
        public int ZoueiKbn { get; private set; }
        public int DrugKbn { get; private set; }
        public int ZaiKbn { get; private set; }
        public int Capacity { get; private set; }
        public int KohatuKbn { get; private set; }
        public int TokuzaiAgeKbn { get; private set; }
        public int SansoKbn { get; private set; }
        public int TokuzaiSbt { get; private set; }
        public int MaxPrice { get; private set; }
        public int MaxTen { get; private set; }
        public string SyukeiSaki { get; private set; }
        public string CdKbn { get; private set; }
        public int CdSyo { get; private set; }
        public int CdBu { get; private set; }
        public int CdKbnno { get; private set; }
        public int CdEdano { get; private set; }
        public int CdKouno { get; private set; }
        public string KokujiKbn { get; private set; }
        public int KokujiSyo { get; private set; }
        public int KokujiBu { get; private set; }
        public int KokujiKbnNo { get; private set; }
        public int KokujiEdaNo { get; private set; }
        public int KokujiKouNo { get; private set; }
        public string Kokuji1 { get; private set; }
        public string Kokuji2 { get; private set; }
        public int KohyoJun { get; private set; }
        public string YjCd { get; private set; }
        public string YakkaCd { get; private set; }
        public int SyusaiSbt { get; private set; }
        public string SyohinKanren { get; private set; }
        public int UpdDate { get; private set; }
        public int DelDate { get; private set; }
        public int KeikaDate { get; private set; }
        public int RousaiKbn { get; private set; }
        public int SisiKbn { get; private set; }
        public int ShotCnt { get; private set; }
        public int IsNosearch { get; private set; }
        public int IsNodspPaperRece { get; private set; }
        public int IsNodspRece { get; private set; }
        public int IsNodspRyosyu { get; private set; }
        public int IsNodspKarte { get; private set; }
        public int JihiSbt { get; private set; }
        public int KazeiKbn { get; private set; }
        public int YohoKbn { get; private set; }
        public string IpnNameCd { get; private set; }
        public int FukuyoRise { get; private set; }
        public int FukuyoMorning { get; private set; }
        public int FukuyoDaytime { get; private set; }
        public int FukuyoNight { get; private set; }
        public int FukuyoSleep { get; private set; }
        public int SuryoRoundupKbn { get; private set; }
        public int KouseisinKbn { get; private set; }
        public string SanteiItemCd { get; private set; }
        public int SanteigaiKbn { get; private set; }
        public string KensaItemCd { get; private set; }
        public int KensaItemSeqNo { get; private set; }
        public string RenkeiCd1 { get; private set; }
        public string RenkeiCd2 { get; private set; }
        public int SaiketuKbn { get; private set; }
        public int CmtKbn { get; private set; }
        public int CmtCol1 { get; private set; }
        public int CmtColKeta1 { get; private set; }
        public int CmtCol2 { get; private set; }
        public int CmtColKeta2 { get; private set; }
        public int CmtCol3 { get; private set; }
        public int CmtColKeta3 { get; private set; }
        public int CmtCol4 { get; private set; }
        public int CmtColKeta4 { get; private set; }
        public int SelectCmtId { get; private set; }
        public int ChusyaDrugSbt { get; private set; }
        public int KensaFukusuSantei { get; private set; }
        public int AgeCheck { get; private set; }
        public int KokujiBetuno { get; private set; }
        public int KokujiKbnno1 { get; private set; }
        public int CmtSbt { get; private set; }
        public int IsNodspYakutai { get; private set; }
        public double ZaikeiPoint { get; private set; }
        public int KensaLabel { get; private set; }
    }
}
