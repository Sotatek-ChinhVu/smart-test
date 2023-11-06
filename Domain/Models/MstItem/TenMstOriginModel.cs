using Helper.Enum;

namespace Domain.Models.MstItem
{
    public class TenMstOriginModel
    {
        public TenMstOriginModel(int hpId, string itemCd, int startDate, int endDate, string masterSbt, int sinKouiKbn, string name, string kanaName1, string kanaName2, string kanaName3, string kanaName4, string kanaName5, string kanaName6, string kanaName7, string ryosyuName, string receName, int tenId, double ten, string receUnitCd, string receUnitName, string odrUnitName, string cnvUnitName, double odrTermVal, double cnvTermVal, double defaultVal, int isAdopted, int koukiKbn, int hokatuKensa, int byomeiKbn, int igakukanri, int jitudayCount, int jituday, int dayCount, int drugKanrenKbn, int kizamiId, int kizamiMin, int kizamiMax, int kizamiVal, double kizamiTen, int kizamiErr, int maxCount, int maxCountErr, string tyuCd, string tyuSeq, int tusokuAge, string minAge, string maxAge, int ageCheck, int timeKasanKbn, int futekiKbn, int futekiSisetuKbn, int syotiNyuyojiKbn, int lowWeightKbn, int handanKbn, int handanGrpKbn, int teigenKbn, int sekituiKbn, int keibuKbn, int autoHougouKbn, int gairaiKanriKbn, int tusokuTargetKbn, int hokatuKbn, int tyoonpaNaisiKbn, int autoFungoKbn, int tyoonpaGyokoKbn, int gazoKasan, int kansatuKbn, int masuiKbn, int fukubikuNaisiKasan, int fukubikuKotunanKasan, int masuiKasan, int moniterKasan, int toketuKasan, string tenKbnNo, int shortstayOpe, int buiKbn, int sisetucd1, int sisetucd2, int sisetucd3, int sisetucd4, int sisetucd5, int sisetucd6, int sisetucd7, int sisetucd8, int sisetucd9, int sisetucd10, string agekasanMin1, string agekasanMax1, string agekasanCd1, string agekasanMin2, string agekasanMax2, string agekasanCd2, string agekasanMin3, string agekasanMax3, string agekasanCd3, string agekasanMin4, string agekasanMax4, string agekasanCd4, int kensaCmt, int madokuKbn, int sinkeiKbn, int seibutuKbn, int zoueiKbn, int drugKbn, int zaiKbn, double zaikeiPoint, int capacity, int kohatuKbn, int tokuzaiAgeKbn, int sansoKbn, int tokuzaiSbt, int maxPrice, int maxTen, string syukeiSaki, string cdKbn, int cdSyo, int cdBu, int cdKbnno, int cdEdano, int cdKouno, string kokujiKbn, int kokujiSyo, int kokujiBu, int kokujiKbnNo, int kokujiEdaNo, int kokujiKouNo, string kokuji1, string kokuji2, int kohyoJun, string yjCd, string yakkaCd, int syusaiSbt, string syohinKanren, int updDate, int delDate, int keikaDate, int rousaiKbn, int sisiKbn, int shotCnt, int isNosearch, int isNodspPaperRece, int isNodspRece, int isNodspRyosyu, int isNodspKarte, int isNodspYakutai, int jihiSbt, int kazeiKbn, int yohoKbn, string ipnNameCd, int fukuyoRise, int fukuyoMorning, int fukuyoDaytime, int fukuyoNight, int fukuyoSleep, int suryoRoundupKbn, int kouseisinKbn, int chusyaDrugSbt, int kensaFukusuSantei, string santeiItemCd, int santeigaiKbn, string kensaItemCd, int kensaItemSeqNo, string renkeiCd1, string renkeiCd2, int saiketuKbn, int cmtKbn, int cmtCol1, int cmtColKeta1, int cmtCol2, int cmtColKeta2, int cmtCol3, int cmtColKeta3, int cmtCol4, int cmtColKeta4, int selectCmtId, int kensaLabel, bool isUpdated, bool isAddNew, int isDeleted, bool isStartDateKeyUpdated, int originStartDate, bool isSelected)
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
            AgeCheck = ageCheck;
            TimeKasanKbn = timeKasanKbn;
            FutekiKbn = futekiKbn;
            FutekiSisetuKbn = futekiSisetuKbn;
            SyotiNyuyojiKbn = syotiNyuyojiKbn;
            LowWeightKbn = lowWeightKbn;
            HandanKbn = handanKbn;
            JitudayCount = jitudayCount;
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
            AgekasanCd1Note = string.Empty;
            AgekasanMin2 = agekasanMin2;
            AgekasanMax2 = agekasanMax2;
            AgekasanCd2 = agekasanCd2;
            AgekasanCd2Note = string.Empty;
            AgekasanMin3 = agekasanMin3;
            AgekasanMax3 = agekasanMax3;
            AgekasanCd3 = agekasanCd3;
            AgekasanCd3Note = string.Empty;
            AgekasanMin4 = agekasanMin4;
            AgekasanMax4 = agekasanMax4;
            AgekasanCd4 = agekasanCd4;
            AgekasanCd4Note = string.Empty;
            KensaCmt = kensaCmt;
            MadokuKbn = madokuKbn;
            SinkeiKbn = sinkeiKbn;
            SeibutuKbn = seibutuKbn;
            ZoueiKbn = zoueiKbn;
            DrugKbn = drugKbn;
            ZaiKbn = zaiKbn;
            ZaikeiPoint = zaikeiPoint;
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
            IsNodspYakutai = isNodspYakutai;
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
            ChusyaDrugSbt = chusyaDrugSbt;
            KensaFukusuSantei = kensaFukusuSantei;
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
            KensaLabel = kensaLabel;
            IsUpdated = isUpdated;
            IsAddNew = isAddNew;
            IsDeleted = isDeleted;
            IsLastItem = false;
            IsStartDateKeyUpdated = isStartDateKeyUpdated;
            OriginStartDate = originStartDate;
            IsSelected = isSelected;
            CreateId = 0;
        }

        public TenMstOriginModel()
        {
            ItemCd = string.Empty;
            Name = string.Empty;
            KanaName1 = string.Empty;
            KanaName2 = string.Empty;
            KanaName3 = string.Empty;
            KanaName4 = string.Empty;
            KanaName5 = string.Empty;
            KanaName6 = string.Empty;
            KanaName7 = string.Empty;
            RyosyuName = string.Empty;
            ReceName = string.Empty;
            ReceUnitCd = string.Empty;
            ReceUnitName = string.Empty;
            OdrUnitName = string.Empty;
            CnvUnitName = string.Empty;
            TyuCd = string.Empty;
            TyuSeq = string.Empty;
            MinAge = string.Empty;
            MaxAge = string.Empty;
            TenKbnNo = string.Empty;
            AgekasanMin1 = string.Empty;
            AgekasanMax1 = string.Empty;
            AgekasanCd1 = string.Empty;
            AgekasanCd1Note = string.Empty;
            AgekasanMin2 = string.Empty;
            AgekasanMax2 = string.Empty;
            AgekasanCd2 = string.Empty;
            AgekasanCd2Note = string.Empty;
            AgekasanMin3 = string.Empty;
            AgekasanMax3 = string.Empty;
            AgekasanCd3 = string.Empty;
            AgekasanCd3Note = string.Empty;
            AgekasanMin4 = string.Empty;
            AgekasanMax4 = string.Empty;
            AgekasanCd4 = string.Empty;
            AgekasanCd4Note = string.Empty;
            SyukeiSaki = string.Empty;
            CdKbn = string.Empty;
            KokujiKbn = string.Empty;
            Kokuji1 = string.Empty;
            Kokuji2 = string.Empty;
            YjCd = string.Empty;
            YakkaCd = string.Empty;
            SyohinKanren = string.Empty;
            IpnNameCd = string.Empty;
            SanteiItemCd = string.Empty;
            KensaItemCd = string.Empty;
            RenkeiCd1 = string.Empty;
            RenkeiCd2 = string.Empty;
            MasterSbt = string.Empty;
            IsSelected = IsSelected;
            CreateId = 0;
        }

        public TenMstOriginModel(int hpId, int startDate, int endDate, int isAdopted, string santeiItemCd, int createId, bool isAddNew,  int sinKouiKbn, string kanaName1, string kanaName2, string syukeiSaki, int tenId, int isNodspRece, int isNodspPaperRece, string masterSbt, string receUnitName, string odrUnitName, int cmtKbn, int defaultVal, string kokuji1, string kokuji2, string itemCd, int jihiSbt)
        {
            HpId = hpId;
            ItemCd = itemCd;
            StartDate = startDate;
            EndDate = endDate;
            IsAdopted = isAdopted;
            SanteiItemCd = santeiItemCd;
            IsAddNew = isAddNew;
            SinKouiKbn = sinKouiKbn;
            KanaName1 = kanaName1;
            KanaName2 = kanaName2;
            SyukeiSaki = syukeiSaki;
            TenId = tenId;
            IsNodspRece = isNodspRece;
            IsNodspPaperRece = isNodspPaperRece;
            MasterSbt = masterSbt;
            ReceUnitName = receUnitName;
            OdrUnitName = odrUnitName;
            CmtKbn = cmtKbn;
            DefaultVal = defaultVal;
            Kokuji1 = kokuji1;
            Kokuji2 = kokuji2;
            CreateId = createId;
            JihiSbt = jihiSbt;
            Name = string.Empty;
            KanaName3 = string.Empty;
            KanaName4 = string.Empty;
            KanaName5 = string.Empty;
            KanaName6 = string.Empty;
            KanaName7 = string.Empty;
            RyosyuName = string.Empty;
            ReceName = string.Empty;
            ReceUnitCd = string.Empty;
            CnvUnitName = string.Empty;
            TyuCd = string.Empty;
            TyuSeq = string.Empty;
            MinAge = string.Empty;
            MaxAge = string.Empty;
            TenKbnNo = string.Empty;
            AgekasanMin1 = string.Empty;
            AgekasanMax1 = string.Empty;
            AgekasanCd1 = string.Empty;
            AgekasanCd1Note = string.Empty;
            AgekasanMin2 = string.Empty;
            AgekasanMax2 = string.Empty;
            AgekasanCd2 = string.Empty;
            AgekasanCd2Note = string.Empty;
            AgekasanMin3 = string.Empty;
            AgekasanMax3 = string.Empty;
            AgekasanCd3 = string.Empty;
            AgekasanCd3Note = string.Empty;
            AgekasanMin4 = string.Empty;
            AgekasanMax4 = string.Empty;
            AgekasanCd4 = string.Empty;
            AgekasanCd4Note = string.Empty;
            CdKbn = string.Empty;
            KokujiKbn = string.Empty;
            YjCd = string.Empty;
            YakkaCd = string.Empty;
            SyohinKanren = string.Empty;
            IpnNameCd = string.Empty;
            KensaItemCd = string.Empty;
            RenkeiCd1 = string.Empty;
            RenkeiCd2 = string.Empty;
            IsSelected = IsSelected;
        }

        /// <summary>
        /// 点数マスタ
        /// </summary>
        /// <summary>
        /// 医療機関識別ID
        /// </summary>
        public int HpId { get; private set; }

        /// <summary>
        /// 診療行為コード
        /// </summary>
        public string ItemCd { get; private set; }

        /// <summary>
        /// 有効開始年月日
        ///     yyyymmdd
        /// </summary>
        public int StartDate { get; private set; }

        public void SetStartDate(int value)
        {
            if (StartDate == value) return;
            IsStartDateKeyUpdated = true;
            StartDate = value;
        }

        public int EndDate { get; private set; }
        /// <summary>
        /// 有効終了年月日
        ///     yyyymmdd
        /// </summary>
        public void SetEndDate(int value)
        {
            if (value == 0)
            {
                if (IsLastItem || EndDate == 99999999)
                {
                    EndDate = 99999999;
                }
                return;
            }
            if (EndDate == value) return;
            EndDate = value;
        }

        /// <summary>
        /// マスター種別
        ///     S: 診療行為
        ///     Y: 医薬品
        ///     T: 特材
        ///     C: コメント
        ///     R: 労災
        ///     U: 労災特定器材
        ///     D: 労災コメントマスタ
        /// </summary>
        public string MasterSbt { get; private set; }


        public int SinKouiKbn { get; private set; }
        /// <summary>
        /// 診療行為区分
        /// </summary>
        public void setSinKouiKbn(int value)
        {
            if (SinKouiKbn == value) return;
            SinKouiKbn = value;
            switch (value)
            {
                case 21:
                    OdrUnitName = "日分";
                    ReceUnitName = "日分";
                    break;
                case 22:
                    OdrUnitName = "回分";
                    ReceUnitName = "回分";
                    break;
                case 23:
                    OdrUnitName = "調剤";
                    ReceUnitName = "調剤";
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 漢字名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// カナ名称１
        /// </summary>
        public string KanaName1 { get; private set; }

        /// <summary>
        /// カナ名称２
        /// </summary>
        public string KanaName2 { get; private set; }

        /// <summary>
        /// カナ名称３
        /// </summary>
        public string KanaName3 { get; private set; }

        /// <summary>
        /// カナ名称４
        /// </summary>
        public string KanaName4 { get; private set; }

        /// <summary>
        /// カナ名称５
        /// </summary>
        public string KanaName5 { get; private set; }

        /// <summary>
        /// カナ名称６
        /// </summary>
        public string KanaName6 { get; private set; }

        /// <summary>
        /// カナ名称７
        /// </summary>
        public string KanaName7 { get; private set; }

        /// <summary>
        /// 領収証用名称
        ///     漢字名称以外を領収証に印字する場合、設定する
        /// </summary>
        public string RyosyuName { get; private set; }

        /// <summary>
        /// 請求用名称
        ///     計算後、請求用の名称
        /// </summary>
        public string ReceName { get; private set; }

        /// <summary>
        /// 点数識別
        ///     1: 金額（整数部7桁、小数部2桁）
        ///     2: 都道府県購入価格
        ///     3: 点数（プラス）
        ///     4: 都道府県購入価格（点数）、金額（整数部のみ）
        ///     5: %加算
        ///     6: %減算
        ///     7: 減点診療行為
        ///     8: 点数（マイナス）
        ///     9: 乗算割合
        ///     10: 除算金額（金額を１０で除す。） ※ベントナイト用
        ///     11: 乗算金額（金額を10で乗ずる。） ※ステミラック注用
        ///     99: 労災円項目
        /// </summary>
        public int TenId { get; private set; }

        /// <summary>
        /// 点数
        /// </summary>
        public double Ten { get; private set; }

        /// <summary>
        /// レセ単位コード
        /// </summary>
        public string ReceUnitCd { get; private set; }

        /// <summary>
        /// レセ単位名称
        /// </summary>
        public string ReceUnitName { get; private set; }

        /// <summary>
        /// オーダー単位名称
        ///     オーダー時に使用する単位
        /// </summary>
        public string OdrUnitName { get; private set; }

        /// <summary>
        /// 数量換算単位名称
        ///     薬剤情報提供書の全数量に換算した値を表示する場合の当該医薬品の換算単位名称を表す。
        /// </summary>
        public string CnvUnitName { get; private set; }

        /// <summary>
        /// オーダー単位換算値
        ///     薬剤情報提供書の全数量に換算した値を表示する場合、当該医薬品の保険請求上の単位からオーダー単位へ換算するための値を表す。
        public double OdrTermVal { get; private set; }

        /// <summary>
        /// 数量換算単位換算値
        ///     薬剤情報提供書の全数量に換算した値を表示する場合、当該医薬品の保険請求上の単位から数量換算単位へ換算するための値を表す。
        /// </summary>
        public double CnvTermVal { get; private set; }

        /// <summary>
        /// 既定数量
        ///     0は未設定
        /// </summary>
        public double DefaultVal { get; private set; }

        /// <summary>
        /// 採用区分
        ///     0: 未採用
        ///     1: 採用
        /// </summary>
        public int IsAdopted { get; private set; }

        /// <summary>
        /// 後期高齢者医療適用区分
        ///     0: 社保・後期高齢者ともに適用される診療行為
        ///     1: 社保のみに適用される診療行為
        ///     2: 後期高齢者のみに適用される診療行為
        /// </summary>
        public int KoukiKbn { get; private set; }

        /// <summary>
        /// 包括対象検査
        ///     0: 1～12以外の診療行為 
        ///     1: 血液化学検査の包括項目 
        ///     2: 内分泌学的検査の包括項目 
        ///     3: 肝炎ウイルス関連検査の包括項目 
        ///     5: 腫瘍マーカーの包括項目 
        ///     6: 出血・凝固検査の包括項目 
        ///     7: 自己抗体検査の包括項目 
        ///     8: 内分泌負荷試験の包括項目 
        ///     9: 感染症免疫学的検査のうち、ウイルス抗体価（定性・半定量・定量） 
        ///     10: 感染症免疫学的検査のうち、グロブリンクラス別ウイルス抗体価 
        ///     11:血漿蛋白免疫学的検査のうち、特異的ＩｇＥ半定量・定量及びアレルゲン刺激性遊離ヒスタミン（ＨＲＴ） 
        ///     12: 悪性腫瘍遺伝子検査の包括項目
        /// </summary>
        public int HokatuKensa { get; private set; }

        /// <summary>
        /// 傷病名関連区分
        ///     0: 3～9以外の診療行為 
        ///     3: 皮膚科特定疾患指導管理料（Ⅰ） 
        ///     4: 皮膚科特定疾患指導管理料（Ⅱ） 
        ///     5: 特定疾患療養管理料、特定疾患処方管理加算１（処方料）、特定疾患処方管理加算１（処方箋料）、特定疾患処方管理加算２（処方料）、特定疾患処方管理加算２（処方箋料） 
        ///     7: てんかん指導料 
        ///     9: 難病外来指導管理料
        /// </summary>
        public int ByomeiKbn { get; private set; }

        /// <summary>
        /// 医学管理料
        ///     2以上の医学管理等を行った場合に、主たる医学管理等の所定点数を算定する背反関係がある診療行為に限り、コードを設定する。
        /// </summary>
        public int Igakukanri { get; private set; }

        /// <summary>
        /// 実日数カウント
        ///     0: 実日数に含めない
        ///     1: 実日数に含める
        /// </summary>
        public int JitudayCount { get; private set; }

        /// <summary>
        /// 実日数
        ///     0: 1～4以外の診療行為 
        ///     1: 算定回数が診療実日数以下の診療行為 
        ///     2: 初診料、再診料、外来診療料等 
        ///     3: 入院基本料、特定入院料 
        ///     4: 外泊
        /// </summary>
        public int Jituday { get; private set; }

        /// <summary>
        /// 日数・回数
        ///     実日数=0, 日数回数=0 - 算定回数と実日数の確認を要しない
        ///     実日数=1, 日数回数=0 - 算定回数が実日数以下である確認を要する
        ///     実日数 = 2, 日数回数 = 1 - 初診料
        ///     実日数=2, 日数回数=2 - 再診料、外来診療料自体、又は再診料、外来診療料が含まれる診療行為
        ///     実日数 = 3, 日数回数 = 3 - 入院基本料、特定入院料
        ///     実日数 = 4, 日数回数 = 0 - 外泊
        /// </summary>
        public int DayCount { get; private set; }

        /// <summary>
        /// 医薬品関連区分
        ///     医薬品の種類を算定要件とする診療行為であるか否かを表す。
        ///     0: 1～4以外の診療行為 
        ///     1: 麻薬加算、毒薬加算、覚醒剤加算、向精神薬加算、麻薬注射加算 
        ///     3: 神経ブロック（神経破壊剤使用） 
        ///     4: 生物学的製剤加算
        /// </summary>
        public int DrugKanrenKbn { get; private set; }

        /// <summary>
        /// きざみ値計算識別
        ///     0: きざみ値により算定しない診療行為（項番１２「新又は現点数」により算定する。） 
        ///     1: きざみ値により算定する診療行為
        /// </summary>
        public int KizamiId { get; private set; }

        /// <summary>
        /// きざみ下限値
        ///     きざみ値により算定する診療行為において「数量データ」の下限値を表す。
        ///     下限値の制限がない場合は「0」である。
        /// </summary>
        public int KizamiMin { get; private set; }

        /// <summary>
        /// きざみ上限値
        ///     きざみ値により算定する診療行為において「数量データ」の上限値を表す。
        ///     上限値の制限がない場合は「99999999」である。
        /// </summary>
        public int KizamiMax { get; private set; }

        /// <summary>
        /// きざみ値
        ///     きざみ値により算定する診療行為において点数のきざみ単位を表す。
        /// </summary>
        public int KizamiVal { get; private set; }

        /// <summary>
        /// きざみ点数
        ///     きざみ値により算定する診療行為においてきざみ点数を表す。
        /// </summary>
        public double KizamiTen { get; private set; }

        /// <summary>
        /// きざみ値上下限エラー処理
        ///     当該診療行為に係る「数量データ」が「下限値－きざみ値」以下又は「上限値」を超えた場合の対処方法を表す。
        ///     上下限エラー処理は「0」～「3」の4つの値を持ち、「下限値－きざみ値」以下の場合の条件、
        ///     及び「上限値」を超えた場合の条件を両方共に満たす値を設定する。
        /// </summary>
        public int KizamiErr { get; private set; }

        /// <summary>
        /// 上限回数
        ///     0: 上限未設定
        /// </summary>
        public int MaxCount { get; private set; }

        /// <summary>
        /// 上限回数エラー処理
        ///     当該診療行為の算定可能回数が上限回数を超えた場合の処理方法を表す。
        ///     0: 上限回数を確認する。
        ///     1: 上限回数にて算定する。
        /// </summary>
        public int MaxCountErr { get; private set; }

        /// <summary>
        /// 注加算コード
        ///     注加算が算定可能な診療行為（基本項目、合成項目及び準用項目）と注加算を関連付ける任意の同一番号を設定する。
        ///    「告示等識別区分（１）」に「７：加算項目」を設定している診療行為のうち、
        ///     注加算コードを設定せずに専用の項目を設定して算定可否を判定する診療行為は「別紙７－８」のとおりである。 
        /// </summary>
        public string TyuCd { get; private set; }

        /// <summary>
        /// 注加算通番
        ///     １つの診療行為に対して同時に算定が可能な注加算に、異なる番号を設定する。 
        ///     注加算が算定可能な診療行為（基本項目、合成項目及び準用項目）に「0」を、
        ///     注加算である診療行為に「1」から「9」及び「A」から「Z」（昇順、アルファベット順）を設定する。 
        ///     注加算コードと注加算通番の関連は「別紙７－９」のとおりである。 
        /// </summary>
        public string TyuSeq { get; private set; }

        /// <summary>
        /// 通則年齢
        ///     当該診療行為が年齢の通則加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: １以外の診療行為
        ///     　1: 通則年齢加算が算定可能な診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: １以外の診療行為
        ///     　1: 通則年齢加算自体
        /// </summary>
        public int TusokuAge { get; private set; }

        /// <summary>
        /// 上下限年齢下限年齢
        ///     当該診療行為が算定可能な年齢の下限値を表す。
        ///     算定可能な年齢 ≧ 下限年齢
        ///     下限年齢に制限のない場合は「00」である。
        ///     数字２桁以外の取り扱いは以下のとおり
        ///     AA：生後２８日
        ///     B3：３歳に達した日の翌月の１日
        ///     B6：６歳に達した日の翌月の１日
        ///     BF：１５歳に達した日の翌月の１日
        ///     BK：２０歳に達した日の翌月の１日
        ///     MG：未就学
        /// </summary>
        public string MinAge { get; private set; }

        /// <summary>
        /// 上下限年齢上限年齢
        ///     当該診療行為が算定可能な年齢の「上限値＋１」を表す。
        ///     算定可能な年齢 ＜ 上限年齢
        ///     上限年齢に制限のない場合は「00」である。
        ///     数字２桁以外の取り扱いは以下のとおり
        ///     AA：生後２８日
        ///     B3：３歳に達した日の翌月の１日
        ///     B6：６歳に達した日の翌月の１日
        ///     BF：１５歳に達した日の翌月の１日
        ///     BK：２０歳に達した日の翌月の１日
        ///     MG：未就学
        /// </summary>
        public string MaxAge { get; private set; }

        /// <summary>
        /// 上下限年齢チェック
        ///     MIN_AGE, MAX_AGEの設定がある場合のみ有効
        ///     0: 年齢範囲外の時、算定不可にする
        ///     1: 年齢範囲外の時、警告扱いにする
        ///     2: チェックしない
        /// </summary>
        public int AgeCheck { get; private set; }

        /// <summary>
        /// 時間加算区分
        ///     当該診療行為が時間加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: 1, 3以外の診療行為
        ///     　1: 時間加算が算定可能な診療行為（含む合成項目）
        ///     　3: 初診料の休日加算に係る診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: 下記以外の診療行為
        ///     　1: 時間外加算自体
        ///     　2: 休日加算自体
        ///     　3: 初診料の休日加算自体
        ///     　4: 深夜加算自体
        ///     　5: 時間外特例加算自体
        ///     　6: 夜間・早朝加算自体
        ///     　7: 夜間加算自体
        ///     　8: 時間外、深夜、時間外特例加算（手術又は、1000 点以上の処置）（注加算又は通則加算）自体
        ///     　9: 休日加算（手術又は、1000 点以上の処置）（注加算又は通則加算）自体
        /// </summary>
        public int TimeKasanKbn { get; private set; }

        /// <summary>
        /// 基準不適合逓減区分
        ///     当該診療行為が施設基準不適合の場合点数を逓減して算定できる診療行為であるか否かを表す。
        ///     　0: 点数逓減して算定できる診療行為以外
        ///     　1: 逓減コード自体
        ///     　2: 点数逓減して算定できる診療行為
        ///     （削）3: 年齢が１歳未満のとき、点数逓減して算定できる診療行為
        /// </summary>
        public int FutekiKbn { get; private set; }

        /// <summary>
        /// 基準不適合逓減対象施設区分
        ///     当該診療行為が施設基準不適合の場合点数を逓減して算定できる診療行為について設定した施設基準コードを表す。
        ///     基準不適合逓減対象施設区分（施設基準コード）については「別紙５」を参照。
        /// </summary>
        public int FutekiSisetuKbn { get; private set; }

        /// <summary>
        /// 処置乳幼児加算区分
        ///     当該診療行為が処置乳幼児加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: １～５以外の診療行為
        ///     　1: ３歳未満乳幼児加算（処置）（１００点）が算定できる診療行為
        ///     　2: ３歳未満乳幼児加算（処置）（５０点）が算定できる診療行為
        ///     　3: ６歳未満乳幼児加算（処置）（１００点）が算定できる診療行為
        ///     　4: ６歳未満乳幼児加算（処置）（７５点）が算定できる診療行為
        ///     　5: ６歳未満乳幼児加算（処置）（５０点）が算定できる診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: １～５以外の診療行為
        ///     　1: ３歳未満乳幼児加算（処置）（１００点）自体
        ///     　2: ３歳未満乳幼児加算（処置）（５０点）自体
        ///     　3: ６歳未満乳幼児加算（処置）（１００点）自体
        ///     　4: ６歳未満乳幼児加算（処置）（７５点）自体
        ///     　5: ６歳未満乳幼児加算（処置）（５０点）自体
        /// </summary>
        public int SyotiNyuyojiKbn { get; private set; }

        /// <summary>
        /// 極低出生体重児加算区分
        ///     当該診療行為が極低出生体重児加算（手術）（４００％）又は新生児加算（手術）（３００％）が算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: １以外の診療行為
        ///     　1: 極低出生体重児加算（手術）（４００％）、新生児加算（手術）（３００％）が算定できる診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: １以外の診療行為
        ///     　1: 極低出生体重児加算（手術）（４００％）、新生児加算（手術）（３００％）自体
        /// </summary>
        public int LowWeightKbn { get; private set; }

        /// <summary>
        /// 検査等実施判断区分
        ///     当該診療行為が検査等の実施料又は判断料に関するものであるか否かを表す。
        ///     　0: 1, 2以外の診療行為
        ///     　1: 検体検査実施料、生体検査実施料、核医学撮影料、コンピューター断層撮影料、病理標本作製料に係る診療行為
        ///     　2: 検体検査判断料、生体検査判断料、核医学診断料、コンピューター断層診断料、病理診断料、病理判断料に係る診療行為
        /// </summary>
        public int HandanKbn { get; private set; }

        /// <summary>
        /// 検査等実施判断グループ区分
        ///     当該診療行為が検査等の場合、判断料・診断料又は判断料･診断料を算定できるグループ区分を表す。
        ///     　0: 1～42以外の診療行為
        ///     (検体検査)
        ///     　　1: 尿・糞便等検査　2: 血液学的検査　3: 生化学的検査（Ⅰ）　4: 生化学的検査（Ⅱ）　5: 免疫学的検査
        ///     (細菌検査)
        ///     　　6: 微生物学的検査
        ///     (検体検査)
        ///     　　8: 基本的検体検査
        ///     (生理検査)
        ///     　　11: 呼吸機能検査　13: 脳波検査　14: 神経・筋検査　15: ラジオアイソトープ検査 16: 眼科学的検査
        ///     (その他検査)
        ///     　　31: 核医学診断（Ｅ１０１－２～Ｅ１０１－５）
        ///     　　32: 核医学診断（それ以外） 33: コンピューター断層診断
        ///     (病理検査)
        ///     　　40: 病理診断 ※
        ///     　　41: 病理診断（組織診断）
        ///     　　42: 病理診断（細胞診断）
        ///     　　※40: 病理診断は41: 病理診断（組織診断）、42: 病理診断（細胞診断）を含む。
        /// </summary>
        public int HandanGrpKbn { get; private set; }

        /// <summary>
        /// 逓減対象区分
        ///     当該診療行為が算定回数による逓減計算の対象となるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: 1以外の診療行為
        ///     　1: 逓減計算の対象となる診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: 1以外の診療行為
        ///     　1: 逓減コード自体
        /// </summary>
        public int TeigenKbn { get; private set; }

        /// <summary>
        /// 脊髄誘発電位測定等加算区分
        ///     当該診療行為が脊髄誘発電位測定等加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: 1, 2以外の診療行為
        ///     　1: 脊髄誘発電位測定等加算（１ 脳、脊椎、脊髄又は大動脈瘤の手術に用いた場合）が算定可能な診療行為
        ///     　2: 脊髄誘発電位測定等加算（２ 甲状腺又は副甲状腺の手術に用いた場合）が算定可能な診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: 1, 2以外の診療行為
        ///     　1: 脊髄誘発電位測定等加算（１ 脳、脊椎、脊髄又は大動脈瘤の手術に用いた場合）自体
        ///     　2: 脊髄誘発電位測定等加算（２ 甲状腺又は副甲状腺の手術に用いた場合）自体
        /// </summary>
        public int SekituiKbn { get; private set; }

        /// <summary>
        /// 頸部郭清術併施加算区分
        ///     当該診療行為が頸部郭清術併施加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: 1以外の診療行為
        ///     　1: 頸部郭清術併施加算が算定可能な診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: 頸部郭清術併施加算が算定できない診療行為
        ///     　1: 頸部郭清術併施加算自体
        /// </summary>
        public int KeibuKbn { get; private set; }

        /// <summary>
        /// 自動縫合器加算区分
        ///     当該診療行為が自動縫合器加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: １以外の診療行為
        ///     　1: 自動縫合器加算（2500点）が算定可能な診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: １以外の診療行為
        ///     　1: 自動縫合器加算（2500点）自体
        /// </summary>
        public int AutoHougouKbn { get; private set; }

        /// <summary>
        /// 外来管理加算区分
        ///     当該診療行為が外来管理加算を算定できないものであるか否かを表す。
        ///     　0: １，２以外の診療行為
        ///     　1: 算定した場合に外来管理加算が算定できない診療行為
        ///     　2: 外来管理加算自体
        /// </summary>
        public int GairaiKanriKbn { get; private set; }

        /// <summary>
        /// 通則加算所定点数対象区分
        ///     通則加算を行う場合において、所定点数として取扱うか否かを表す。
        ///     　0: 所定点数として取扱う診療行為及び通則加算
        ///     　1: 所定点数として取扱わない基本診療行為
        /// </summary>
        public int TusokuTargetKbn { get; private set; }

        /// <summary>
        /// 包括逓減区分
        ///     逓減対象検査等のグループ区分を表す。
        /// </summary>
        public int HokatuKbn { get; private set; }

        /// <summary>
        /// 超音波内視鏡加算区分
        ///     当該診療行為が超音波内視鏡加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: １以外の診療行為
        ///     　1: 超音波内視鏡加算が算定可能な診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: １以外の診療行為
        ///     　1: 超音波内視鏡加算自体
        /// </summary>
        public int TyoonpaNaisiKbn { get; private set; }

        /// <summary>
        /// 自動吻合器加算区分
        ///     通則加算を行う場合において、所定点数として取扱うか否かを表す。
        ///     　0: 所定点数として取扱う診療行為及び通則加算
        ///     　1: 所定点数として取扱わない基本診療行為
        /// </summary>
        public int AutoFungoKbn { get; private set; }

        /// <summary>
        /// 超音波凝固切開装置等加算区分
        ///     当該診療行為が超音波凝固切開装置等加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: 1以外の診療行為
        ///     　1: 超音波凝固切開装置等加算が算定可能な診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: 1以外の診療行為
        ///     　1: 超音波凝固切開装置等加算自体
        /// </summary>
        public int TyoonpaGyokoKbn { get; private set; }

        /// <summary>
        /// 画像等手術支援加算
        ///     当該診療行為が画像等手術支援加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: 1～5以外の診療行為
        ///     　1: ナビゲーションによる支援加算（２０００点）が算定できる診療行為
        ///     　2: 実物大臓器立体モデルによる支援加算（２０００点）が算定できる診療行為
        ///     　3: ナビゲーション又は実物大臓器立体モデルによる支援加算（共に２０００点）が算定できる診療行為
        ///     　4: 患者適合型手術支援ガイドによる支援加算（２０００点）が算定できる診療行為
        ///     　5: ナビゲーション又は患者適合型手術支援ガイドによる支援加算（共に２０００点）が算定できる診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: 1,2,4以外の診療行為
        ///     　1: ナビゲーションによる支援加算自体
        ///     　2: 実物大臓器立体モデルによる支援加算自体
        ///     　4: 患者適合型手術支援ガイドによる支援加算自体
        /// </summary>
        public int GazoKasan { get; private set; }

        /// <summary>
        /// 医療観察法対象区分
        ///     当該診療行為が医療観察法において算定可能であるか否かを表す。
        ///     　0: 1～4以外の診療行為
        ///     　1: 入院のみに出来高部分で算定可能な診療行為
        ///     　2: 外来（通院）のみに出来高部分で算定可能な診療行為
        ///     　3: 入院、外来（通院）共に出来高部分で算定可能な診療行為
        ///     　4: 医療観察法専用の診療行為
        /// </summary>
        public int KansatuKbn { get; private set; }

        /// <summary>
        /// 麻酔識別区分
        ///     当該診療行為がマスク又は気管内挿管による閉鎖循環式全身麻酔であるか否かを表す。
        ///     　0: １～９以外の診療行為
        ///     　1: マスク又は気管内挿管による閉鎖循環式全身麻酔１
        ///     　2: マスク又は気管内挿管による閉鎖循環式全身麻酔２
        ///     　3: マスク又は気管内挿管による閉鎖循環式全身麻酔３
        ///     　4: マスク又は気管内挿管による閉鎖循環式全身麻酔４
        ///     　5: マスク又は気管内挿管による閉鎖循環式全身麻酔５
        ///     　8: マスク又は気管内挿管による閉鎖循環式全身麻酔の加算（硬膜外麻酔併施加算以外）
        ///     　9: 硬膜外麻酔併施加算
        /// </summary>
        public int MasuiKbn { get; private set; }

        /// <summary>
        /// 副鼻腔手術用内視鏡加算
        ///     当該診療行為が副鼻腔手術用内視鏡加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: 1以外の診療行為
        ///     　1: 副鼻腔手術用内視鏡加算が算定可能な診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: 1以外の診療行為
        ///     　1: 副鼻腔手術用内視鏡加算自体
        /// </summary>
        public int FukubikuNaisiKasan { get; private set; }

        /// <summary>
        /// 副鼻腔手術用骨軟部組織切除機器加算
        ///     当該診療行為が副鼻腔手術用骨軟部組織切除機器加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: 1以外の診療行為
        ///     　1: 副鼻腔手術用骨軟部組織切除機器加算が算定可能な診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: 1以外の診療行為
        ///     　1: 副鼻腔手術用骨軟部組織切除機器加算自体
        /// </summary>
        public int FukubikuKotunanKasan { get; private set; }

        /// <summary>
        /// 長時間麻酔管理加算
        ///     当該診療行為が長時間麻酔管理加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: 1,2以外の診療行為
        ///     　1: 長時間麻酔管理加算が算定可能な診療行為
        ///     　2: L008に掲げるマスク又は気管内挿管による閉鎖循環式全身麻酔の実施時間が８時間を超え、長時間麻酔管理加算を算定する場合に実施している必要がある手術
        ///     ＜加算項目、通則加算項目＞
        ///     　0: 1以外の診療行為
        ///     　1: 長時間麻酔管理加算自体
        /// </summary>
        public int MasuiKasan { get; private set; }

        /// <summary>
        /// 非侵襲的血行動態モニタリング加算
        ///     当該診療行為が非侵襲的血行動態モニタリング加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: １、２以外の診療行為
        ///     　1: 非侵襲的血行動態モニタリング加算が算定可能な診療行為
        ///     　2: 非侵襲的血行動態モニタリング加算を算定する場合に実施している必要がある手術
        ///     ＜加算項目、通則加算項目＞
        ///     　0: １以外の診療行為
        ///     　1: 非侵襲的血行動態モニタリング加算自体
        /// </summary>
        public int MoniterKasan { get; private set; }

        /// <summary>
        /// 凍結保存同種組織加算
        ///     当該診療行為が凍結保存同種組織加算を算定できるものであるか否かを表す。
        ///     ＜基本項目、合成項目、準用項目＞
        ///     　0: １以外の診療行為
        ///     　1: 凍結保存同種組織加算が算定可能な診療行為
        ///     ＜加算項目、通則加算項目＞
        ///     　0: 1以外の診療行為
        ///     　1: 凍結保存同種組織加算自体
        /// </summary>
        public int ToketuKasan { get; private set; }

        /// <summary>
        /// 点数表区分番号
        ///     医科点数表の「第２章 特掲診療料」「第１０部 手術」に規定する診療行為（通則及び注に掲げる加算等を除く。）の区分番号及び項番等を表す。
        /// </summary>
        public string TenKbnNo { get; private set; }

        /// <summary>
        /// 短期滞在手術
        ///     当該診療行為が短期滞在手術等基本料を算定できるものであるか否かを表す。
        ///     　０：１～４以外の診療行為
        ///     　１：短期滞在手術等基本料１
        ///     　２：短期滞在手術等基本料２
        ///     　３：短期滞在手術等基本料１が算定可能な診療行為（手術）
        ///     　４：短期滞在手術等基本料２が算定可能な診療行為（手術）
        /// </summary>
        public int ShortstayOpe { get; private set; }

        /// <summary>
        /// 部位区分
        ///     画像診断撮影部位マスターにおいて撮影部位を表す。
        ///     　0: 部位以外
        ///     　1: 頭部
        ///     　2: 躯幹
        ///     　3: 四肢
        ///     　5: 胸部
        ///     　6: 腹部
        ///     　7: 脊椎
        ///     　8: 消化管
        ///      10: 指
        ///      99: その他部位（撮影部位マスターでない場合も含む。）
        /// </summary>
        public int BuiKbn { get; private set; }

        /// <summary>
        /// 施設基準コード１
        ///     ＜診療行為＞
        ///        当該診療行為が施設基準に関するものであるか否かを表す。
        ///     　「施設基準コード１」から使用し最大１０項目（「施設基準コード１０」）まで使用可能である。
        ///     　施設基準コードについては「別紙５」を参照。
        ///     ＜医薬品＞
        ///     　当該医薬品について薬価基準の規格単位数を表す。
        ///     　ただし、規格単位数が１の場合は省略し０を収容する。
        /// </summary>
        public int Sisetucd1 { get; private set; }

        /// <summary>
        /// 施設基準コード２
        ///     ＜診療行為＞
        ///     施設基準コード１を参照。
        ///     ＜医薬品＞
        ///     　当該医薬品が湿布薬で単位が「ｇ」の場合は膏体量を収容する。
        ///             /// </summary>
        public int Sisetucd2 { get; private set; }

        /// <summary>
        /// 施設基準コード３
        /// </summary>
        public int Sisetucd3 { get; private set; }

        /// <summary>
        /// 施設基準コード４
        /// </summary>
        public int Sisetucd4 { get; private set; }

        /// <summary>
        /// 施設基準コード５
        /// </summary>
        public int Sisetucd5 { get; private set; }

        /// <summary>
        /// 施設基準コード６
        /// </summary>
        public int Sisetucd6 { get; private set; }

        /// <summary>
        /// 施設基準コード７
        /// </summary>
        public int Sisetucd7 { get; private set; }

        /// <summary>
        /// 施設基準コード８
        /// </summary>
        public int Sisetucd8 { get; private set; }

        /// <summary>
        /// 施設基準コード９
        /// </summary>
        public int Sisetucd9 { get; private set; }

        /// <summary>
        /// 施設基準コード１０
        /// </summary>
        public int Sisetucd10 { get; private set; }

        /// <summary>
        /// 年齢加算下限年齢１
        ///     当該診療行為に算定可能な年齢注加算の診療行為コードを表し、最大４つの年齢範囲まで記録する。
        ///     未使用部分には、下限年齢、上限年齢及び注加算診療行為コードに「ゼロ」を記録する。
        ///     数字２桁以外の取り扱いは以下のとおり
        ///     AA: 生後28日
        ///     B3: 3歳に達した日の翌月の１日
        ///     BF: 15歳に達した日の翌月の１日
        ///     BK: 20歳に達した日の翌月の１日
        ///     年齢加算下限年齢：
        ///     当該診療行為に注加算の算定が可能な場合、記録された注加算診療行為コードの下限年齢を表す。
        ///     年齢加算上限年齢：
        ///     　当該診療行為に注加算の算定が可能な場合、記録された注加算診療行為コードの上限年齢を表す。
        ///     注加算診療行為コード：
        ///     　年齢注加算の診療行為コードを表す。
        /// </summary>
        public string AgekasanMin1 { get; private set; }
        /// <summary>
        /// 年齢加算上限年齢１
        ///     年齢加算下限年齢１を参照。
        /// </summary>
        public string AgekasanMax1 { get; private set; }

        /// <summary>
        /// 年齢加算注加算診療行為コード１
        ///      年齢加算下限年齢１を参照。
        /// </summary>
        public string AgekasanCd1 { get; private set; }


        public string AgekasanCd1Note { get; private set; }

        public void SetAgekasanCd1Note(string value)
        {
            AgekasanCd1Note = value;
        }

        /// <summary>
        /// 年齢加算下限年齢２
        ///      年齢加算下限年齢１を参照。
        /// </summary>
        public string AgekasanMin2 { get; private set; }
        /// <summary>
        /// 年齢加算上限年齢２
        ///      年齢加算下限年齢１を参照。
        /// </summary>
        public string AgekasanMax2 { get; private set; }

        /// <summary>
        /// 年齢加算注加算診療行為コード２
        ///      年齢加算下限年齢１を参照。
        /// </summary>
        public string AgekasanCd2 { get; private set; }

        public string AgekasanCd2Note { get; private set; }

        public void SetAgekasanCd2Note(string value)
        {
            AgekasanCd2Note = value;
        }

        /// <summary>
        /// 年齢加算下限年齢３
        ///      年齢加算下限年齢１を参照。
        /// </summary>
        public string AgekasanMin3 { get; private set; }

        /// <summary>
        /// 年齢加算上限年齢３
        ///      年齢加算下限年齢１を参照。
        /// </summary>
        public string AgekasanMax3 { get; private set; }

        /// <summary>
        /// 年齢加算注加算診療行為コード３
        ///      年齢加算下限年齢１を参照。
        /// </summary>
        public string AgekasanCd3 { get; private set; }

        public string AgekasanCd3Note { get; private set; }

        public void SetAgekasanCd3Note(string value)
        {
            AgekasanCd3Note = value;
        }

        /// <summary>
        /// 年齢加算下限年齢４
        ///      年齢加算下限年齢１を参照。
        /// </summary>
        public string AgekasanMin4 { get; private set; }

        /// <summary>
        /// 年齢加算上限年齢４
        ///      年齢加算下限年齢１を参照。
        /// </summary>
        public string AgekasanMax4 { get; private set; }

        /// <summary>
        /// 年齢加算注加算診療行為コード４
        ///      年齢加算下限年齢１を参照。
        /// </summary>
        public string AgekasanCd4 { get; private set; }

        public string AgekasanCd4Note { get; private set; }

        public void SetAgekasanCd4Note(string value)
        {
            AgekasanCd4Note = value;
        }

        /// <summary>
        /// 検体検査コメント
        ///     当該診療行為が、検体検査の検体コメントであるか否かを表す。
        ///     　0: 検体コメント以外
        ///     　1: 検体コメント
        /// </summary>
        public int KensaCmt { get; private set; }

        /// <summary>
        /// 麻毒区分
        ///     当該医薬品が麻薬、毒薬、覚せい剤原料又は向精神薬であるか否かを表す。
        ///     　0: 麻薬、毒薬、覚せい剤原料又は向精神薬以外
        ///     　1: 麻薬
        ///     　2: 毒薬
        ///     　3: 覚せい剤原料
        ///     　5: 向精神薬
        /// </summary>
        public int MadokuKbn { get; private set; }

        /// <summary>
        /// 神経破壊剤区分
        ///     当該医薬品が神経破壊剤であるか否かを表す。
        ///     　0: 神経破壊剤以外
        ///     　1: 神経破壊剤
        /// </summary>
        public int SinkeiKbn { get; private set; }

        /// <summary>
        /// 生物学的製剤区分
        ///     当該医薬品が生物学的製剤加算対象品目であるか否かを表す。
        ///     　0: 生物学的製剤加算対象品目以外
        ///     　1: 生物学的製剤加算対象品目
        /// </summary>
        public int SeibutuKbn { get; private set; }

        /// <summary>
        /// 造影剤区分
        ///     当該医薬品が造影剤又は造影補助剤であるか否かを表す。
        ///     　0: 造影剤、造影補助剤以外
        ///     　1: 造影剤
        ///     　2: 造影補助剤
        /// </summary>
        public int ZoueiKbn { get; private set; }

        /// <summary>
        /// 薬剤区分
        ///     当該医薬品の薬剤区分を表す。
        ///       0: 薬剤以外
        ///     　1: 内用薬
        ///     　3: その他
        ///     　4: 注射薬
        ///     　6: 外用薬
        ///     　8: 歯科用薬剤
        ///     （削）9: 歯科特定薬剤
        ///     ※レセプト電算マスターの項目「剤型」を収容する。
        /// </summary>
        public int DrugKbn { get; private set; }

        /// <summary>
        /// 剤型区分
        ///     当該医薬品の剤型区分を表す。
        ///     　0: 下記以外
        ///     　1: 散剤
        ///     　2: 顆粒剤（細粒剤）
        ///     　3: 液剤
        ///     ※レセプト電算マスターの項目「剤型」とは異なる。
        /// </summary>
        public int ZaiKbn { get; private set; }

        /// <summary>
        /// 剤形ポイント
        /// 薬袋フォーム切替条件判定の際、数量を換算するのに使用
        /// </summary>
        public double ZaikeiPoint { get; private set; }

        /// <summary>
        /// 注射容量
        ///     当該医薬品が注射薬の場合、その容量（単位はｍＬ）を表す。
        /// </summary>
        public int Capacity { get; private set; }

        /// <summary>
        /// 後発医薬品区分
        ///     当該医薬品が後発医薬品に該当するか否かを表す。
        ///     　0: 後発医薬品でない
        ///     　1: 先発医薬品がある後発医薬品である
        ///     ※基金マスタの設定、オーダー時はKOHATU_KBN_MSTを見るようにすること
        /// </summary>
        public int KohatuKbn { get; private set; }

        /// <summary>
        /// 特定器材年齢加算区分
        ///     当該特定器材が年齢加算に関係があるか否かを表す。
        ///     　0: 年齢加算に関係のない特定器材
        ///     　1: 年齢加算又は年齢加算が算定可能な特定器材
        ///     　　　＊胸部又は腹部単純撮影の乳幼児加算、及びフィルム料
        /// </summary>
        public int TokuzaiAgeKbn { get; private set; }

        /// <summary>
        /// 酸素等区分
        ///     当該特定器材が酸素又は窒素に関するものであるか否かを表す。
        ///     　0: 酸素、窒素、酸素補正率及び高気圧酸素加算以外
        ///     　1: 酸素補正率及び高気圧酸素加算
        ///     　2: 定置式液化酸素貯槽（ＣＥ）
        ///     　3: 可搬式液化酸素容器（ＬＧＣ）
        ///     　4: 大型ボンベ
        ///     　5: 小型ボンベ
        ///     　9: 窒素
        /// </summary>
        public int SansoKbn { get; private set; }

        /// <summary>
        /// 特定器材種別１
        ///     当該特定器材の点数算定方法の種別を表す。
        ///     　0:  ↑購入価格｜
        ///     　　　｜－－－－｜
        ///     　　　｜　１０円↓により算定する特定器材
        ///     　2:  ↑↑購入価格↓｜
        ///     　　　｜－－－－－－｜
        ///     　　　｜　　１０円　↓により算定する特定器材
        ///     （酸素、窒素）
        ///     　3:  ↑購入価格｜
        ///     　　　｜－－－－｜
        ///     　　　｜　５０円↓により算定する特定器材（高線量率イリジウム）
        ///     　4:  ↑購入価格　｜
        ///     　　　｜－－－－－｜
        ///     　　　｜１０００円↓により算定する特定器材（コバルト）
        ///     　　　↑↓：四捨五入
        /// </summary>
        public int TokuzaiSbt { get; private set; }

        /// <summary>
        /// 上限価格
        ///     当該特定器材の金額に酸素の上限価格の設定がされていることを表す。
        ///     　0: 下記以外
        ///     　1: 上限価格の設定がされている場合
        /// </summary>
        public int MaxPrice { get; private set; }

        /// <summary>
        /// 上限点数
        ///     当該特定器材（眼底カメラ検査用インスタントフィルム）が算定可能な上限点数を表す。上限点数の設定されない場合は「０」である。
        /// </summary>
        public int MaxTen { get; private set; }

        public string SyukeiSaki { get; private set; }

        /// <summary>
        /// 点数欄集計先識別（外来）
        ///     当該診療行為の入院外レセプトにおける点数欄への集計先を表す。
        ///     点数欄集計先識別については「別紙９」を参照。
        ///     入院外レセプトで使用不可の診療行為は「０」である。
        /// </summary>
        public void SetSyukeiSaki(string value)
        {
            if (SyukeiSaki == value) return;
            SyukeiSaki = value;
            switch (value)
            {
                case "ZZ0":
                    KanaName1 = "ｼﾝﾀﾞﾝｼｮﾘｮｳ";
                    ReceUnitName = "通";
                    OdrUnitName = "通";
                    break;
                case "ZZ1":
                    KanaName1 = "ﾒｲｻｲｼｮﾘｮｳ";
                    ReceUnitName = "通";
                    OdrUnitName = "通";
                    break;
                case "A18":
                    KanaName1 = "ｿﾉﾀ";
                    ReceUnitName = string.Empty;
                    OdrUnitName = string.Empty;
                    break;
            }
        }

        /// <summary>
        /// コード表用区分－区分
        ///     当該診療行為について医科点数表の章、部、区分番号及び項番を記録する。
        ///             区分（アルファベット部）：
        ///     　点数表の区分番号のアルファベット部を記録する。
        ///     　なお、介護老人保健施設入所者に係る診療料、医療観察法、入院時食事療養、入院時生活療養及び標準負担額については
        ///     　「－」（ハイホン）を、点数表に区分設定がないものは「＊」を記録する。
        ///     章：
        ///     部：
        ///     区分番号：
        ///     枝番：
        ///     項番：
        /// </summary>
        public string CdKbn { get; private set; }

        /// <summary>
        /// コード表用区分－章
        ///     コード表用区分－区分を参照。
        /// </summary>
        public int CdSyo { get; private set; }

        /// <summary>
        /// コード表用区分－部
        ///     コード表用区分－区分を参照。
        /// </summary>
        public int CdBu { get; private set; }

        /// <summary>
        /// コード表用区分－区分番号
        ///     コード表用区分－区分を参照。
        /// </summary>
        public int CdKbnno { get; private set; }

        /// <summary>
        /// コード表用区分－区分番号－枝番
        ///     コード表用区分－区分を参照。
        /// </summary>
        public int CdEdano { get; private set; }

        /// <summary>
        /// コード表用区分－項番
        ///     コード表用区分－区分を参照。
        /// </summary>
        public int CdKouno { get; private set; }

        /// <summary>
        /// 告知・通知関連番号－区分
        ///     当該診療行為が準用項目の場合、準用元の医科点数表の章、部、区分番号及び項番を記録する。
        ///             区分（アルファベット部）：
        ///     　点数表の区分番号のアルファベット部を記録する。準用項目以外は未使用。
        ///     章：
        ///     部：
        ///     区分番号：
        ///     枝番：
        ///     項番：
        /// </summary>
        public string KokujiKbn { get; private set; }

        /// <summary>
        /// 告知・通知関連番号－章
        ///     告知・通知関連番号－区分を参照。
        /// </summary>
        public int KokujiSyo { get; private set; }

        /// <summary>
        /// 告知・通知関連番号－部
        ///     告知・通知関連番号－区分を参照。
        /// </summary>
        public int KokujiBu { get; private set; }

        /// <summary>
        /// 告知・通知関連番号－区分番号
        ///     告知・通知関連番号－区分を参照。
        /// </summary>
        public int KokujiKbnNo { get; private set; }

        /// <summary>
        /// 告知・通知関連番号－区分番号－枝番
        ///     告知・通知関連番号－区分を参照。
        /// </summary>
        public int KokujiEdaNo { get; private set; }

        /// <summary>
        /// 告知・通知関連番号－項番
        ///     告知・通知関連番号－区分を参照。
        /// </summary>
        public int KokujiKouNo { get; private set; }

        /// <summary>
        /// 告示等識別区分（１）
        ///     当該診療行為についてコンピューター運用上の取扱い（磁気媒体に記録する際の取扱い）を表す。
        ///     　1: 基本項目（告示）　※基本項目
        ///     　3: 合成項目　　　　　※基本項目
        ///     　5: 準用項目（通知）　※基本項目
        ///     　7: 加算項目　　　　　※加算項目
        ///     　9: 通則加算項目　　　※加算項目
        ///       0: 診療行為以外（薬剤、特材等）
        ///       A: 入院基本料労災乗数項目又は四肢加算（手術）項目
        /// </summary>
        public string Kokuji1 { get; private set; }

        /// <summary>
        /// 告示等識別区分（２）
        ///     当該診療行為について点数表上の取扱いを表す。
        ///     　1: 基本項目（告示）
        ///     　3: 合成項目
        ///     （削）5: 準用項目（通知）
        ///     　7: 加算項目（告示）
        ///     （削）9: 通則加算項目
        ///       0: 診療行為以外（薬剤、特材等）
        /// </summary>
        public string Kokuji2 { get; private set; }

        /// <summary>
        /// 公表順序番号
        ///     コード表用番号による順序番号を記録する。
        /// </summary>
        public int KohyoJun { get; private set; }

        /// <summary>
        /// 個別医薬品コード
        ///     薬価基準収載医薬品コードと同様に英数12桁のコードですが、統一名収載品目の個々の商品に対して別々のコードが付与されます。
        ///     銘柄別収載品目（商品名で官報に収載されるもの）については、薬価基準収載医薬品コードと同じコードです。
        /// </summary>
        public string YjCd { get; private set; }

        /// <summary>
        /// 薬価基準収載医薬品コード
        ///     当該医薬品に係る薬価基準収載医薬品コードを表す。
        /// </summary>
        public string YakkaCd { get; private set; }

        /// <summary>
        /// 収載方式等識別
        ///     当該医薬品の薬価基準収載方式の分類を表す。
        ///     　0: 1～8以外の医薬品
        ///     　1: 日本薬局方収載医薬品（局方品）
        ///     　2: 局方品で生物学的製剤基準医薬品
        ///     　3: 局方品で生薬
        ///     　6: 生物学的製剤基準医薬品
        ///     　7: 生薬
        ///     　8: 1～7以外の統一名収載品
        /// </summary>
        public int SyusaiSbt { get; private set; }

        /// <summary>
        /// 商品名等関連
        ///     当該医薬品が商品名医薬品(非告示品)の場合、その統一名収載品(告示品)の医薬品コードを記録する。
        ///     なお、商品名医薬品でない場合は「0000000000」である。
        /// </summary>
        public string SyohinKanren { get; private set; }

        /// <summary>
        /// 変更年月日
        ///     yyyymmdd
        /// </summary>
        public int UpdDate { get; private set; }

        /// <summary>
        /// 廃止年月日
        ///     yyyymmdd
        /// </summary>
        public int DelDate { get; private set; }

        /// <summary>
        /// 経過措置年月日
        ///     yyyymmdd
        /// </summary>
        public int KeikaDate { get; private set; }

        /// <summary>
        /// 労災区分
        ///     当該診療行為が労災保険で算定可能かを表す。
        ///     　0: 健保・労災において算定可能
        ///     　1: 労災のみ算定可能
        ///     　2: 健保のみ算定可能
        /// </summary>
        public int RousaiKbn { get; private set; }

        /// <summary>
        /// 四肢加算区分（労災）
        ///     当該診療行為の四肢に対する特例の取扱い（1.5倍・2.0倍）を表す。
        ///     　0: 1～5以外の診療行為
        ///     　1: 1.5倍又は2.0倍の対象
        ///     　2: 1.5倍のみ対象
        ///     　3: 2.0倍のみ対象
        ///     　4: 1.5倍の加算自体
        ///     　5: 2.0倍の加算自体
        /// </summary>
        public int SisiKbn { get; private set; }

        /// <summary>
        /// フィルム撮影回数
        ///     フィルム1枚あたりの撮影回数
        ///     0: フィルム以外
        /// </summary>
        public int ShotCnt { get; private set; }

        /// <summary>
        /// 検索不可区分
        ///     0: 検索可
        ///     1: 検索不可
        /// </summary>
        public int IsNosearch { get; private set; }

        /// <summary>
        /// 紙レセ非表示区分
        ///     0: 表示
        ///     1: 非表示（摘要欄に表示しない、点数欄には表示する）
        /// </summary>
        public int IsNodspPaperRece { get; private set; }

        /// <summary>
        /// レセ非表示区分
        ///     0: 表示
        ///     1: 非表示（レセプト自体に表示しない）
        /// </summary>
        public int IsNodspRece { get; private set; }

        /// <summary>
        /// 領収証非表示区分
        ///     0: 表示
        ///     1: 非表示
        /// </summary>
        public int IsNodspRyosyu { get; private set; }

        /// <summary>
        /// カルテ非表示区分
        ///     0: 表示
        ///     1: 非表示
        /// </summary>
        public int IsNodspKarte { get; private set; }

        /// <summary>
        /// 薬袋表示
        ///     0: 薬袋に表示する
        ///     1: 薬袋に表示しない
        /// </summary>
        public int IsNodspYakutai { get; private set; }

        /// <summary>
        /// 自費種別コード
        ///     0: 自費項目以外
        ///     >0: 自費項目
        ///     JIHI_SBT_MST.自費種別
        /// </summary>
        public int JihiSbt { get; private set; }

        /// <summary>
        /// 課税区分
        ///     0: 非課税
        ///     1: 外税
        ///     2: 内税
        /// </summary>
        public int KazeiKbn { get; private set; }

        /// <summary>
        /// 用法区分
        ///     0: 用法以外
        ///     1: 基本用法
        ///     2: 補助用法
        /// </summary>
        public int YohoKbn { get; private set; }

        /// <summary>
        /// 一般名コード
        ///     YJ_CDの頭9桁（例外あり）
        /// </summary>
        public string IpnNameCd { get; private set; }

        /// <summary>
        /// 服用時設定-起床時
        ///     0: 服用しない
        ///     1: 服用する
        /// </summary>
        public int FukuyoRise { get; private set; }

        /// <summary>
        /// 服用時設定-朝
        ///     0: 服用しない
        ///     1: 服用する
        /// </summary>
        public int FukuyoMorning { get; private set; }

        /// <summary>
        /// 服用時設定-昼
        ///     0: 服用しない
        ///     1: 服用する
        /// </summary>
        public int FukuyoDaytime { get; private set; }

        /// <summary>
        /// 服用時設定-夜
        ///     0: 服用しない
        ///     1: 服用する
        /// </summary>
        public int FukuyoNight { get; private set; }

        /// <summary>
        /// 服用時設定-眠前
        ///     0: 服用しない
        ///     1: 服用する
        /// </summary>
        public int FukuyoSleep { get; private set; }

        /// <summary>
        /// 数量端数切り上げ区分
        ///     1: 端数切り上げ
        /// </summary>
        public int SuryoRoundupKbn { get; private set; }

        /// <summary>
        /// 向精神薬区分
        ///     0: 向精神薬以外
        ///     1:抗不安薬
        ///     2:睡眠薬
        ///     3:抗うつ薬
        ///     4:抗精神病薬
        /// </summary>
        public int KouseisinKbn { get; private set; }

        /// <summary>
        /// 注射薬剤種別
        ///     ※1～5は、人工腎臓に包括される
        ///     0:1～5以外
        ///     1:透析液
        ///     2:血液凝固阻止剤
        ///     3:エリスロポエチン
        ///     4:ダルベポエチン
        ///     5:生理食塩水
        /// </summary>
        public int ChusyaDrugSbt { get; private set; }

        /// <summary>
        /// 検査1来院複数回算定
        ///     1: 複数回算定可
        /// </summary>
        public int KensaFukusuSantei { get; private set; }

        /// <summary>
        /// 算定診療行為コード
        ///     算定時の診療行為コード（自己結合でデータ取得）
        /// </summary>
        public string SanteiItemCd { get; private set; }

        /// <summary>
        /// 算定外区分
        ///     0: 算定する
        ///     1: 算定外
        /// </summary>
        public int SanteigaiKbn { get; private set; }

        /// <summary>
        /// 検査項目コード
        ///     KENSA_MST.KENSA_ITEM_CD
        /// </summary>
        public string KensaItemCd { get; private set; }

        /// <summary>
        /// 検査項目コード連番
        ///     KENSA_MST.SEQ_NO
        /// </summary>
        public int KensaItemSeqNo { get; private set; }

        /// <summary>
        /// 連携コード１
        ///     外部システムとの連携用コード
        /// </summary>
        public string RenkeiCd1 { get; private set; }

        /// <summary>
        /// 連携コード２
        ///     外部システムとの連携用コード
        /// </summary>
        public string RenkeiCd2 { get; private set; }

        /// <summary>
        /// 採血料区分
        ///     当該診療行為を算定した場合に血液採取料を自動発生させるか否かを表す。
        ///     　0: 下記以外
        ///     　1: 末梢採血料（6点）
        ///     　2: 静脈採血料（12点）
        ///     　3: 動脈血採取料（40点）
        /// </summary>
        public int SaiketuKbn { get; private set; }

        /// <summary>
        /// コメント区分
        ///     計算時、コメントを自動で付与する場合、付与するコメントの種類を設定
        ///     0: 自動付与なし
        ///     1: 実施日
        ///     2: 前回日（840000087 :前回実施 月日）
        ///     3: 初回日（840000085 :初回実施 月日）
        ///     4: 前回日 or 初回日
        ///     5: 初回算定日（840000085 :初回算定 月日）
        ///     6: 実施日（列挙）
        ///     7: 実施日（列挙：項目名あり）
        ///     8: 実施日数（840000096 :実施日数 日）
        ///     9: 前回日 or 初回日（項目名あり）
        /// </summary>
        public int CmtKbn { get; private set; }

        /// <summary>
        /// カラム位置１
        ///     記録した数字情報の編集位置を表す。
        /// </summary>
        public int CmtCol1 { get; private set; }

        /// <summary>
        /// 桁数１
        ///     カラム位置から編集する数字情報の文字数を表す。
        /// </summary>
        public int CmtColKeta1 { get; private set; }

        /// <summary>
        /// カラム位置２
        ///     記録した数字情報の編集位置を表す。
        /// </summary>
        public int CmtCol2 { get; private set; }

        /// <summary>
        /// 桁数２
        ///     カラム位置から編集する数字情報の文字数を表す。
        /// </summary>
        public int CmtColKeta2 { get; private set; }

        /// <summary>
        /// カラム位置３
        ///     記録した数字情報の編集位置を表す。
        /// </summary>
        public int CmtCol3 { get; private set; }

        /// <summary>
        /// 桁数３
        ///     カラム位置から編集する数字情報の文字数を表す。
        /// </summary>
        public int CmtColKeta3 { get; private set; }

        /// <summary>
        /// カラム位置４
        ///     記録した数字情報の編集位置を表す。
        /// </summary>
        public int CmtCol4 { get; private set; }

        /// <summary>
        /// 桁数４
        ///     カラム位置から編集する数字情報の文字数を表す。
        /// </summary>
        public int CmtColKeta4 { get; private set; }

        /// <summary>
        /// 選択式コメント識別
        ///     選択式コメントであるか否かを表す。  
        ///     0: 選択式以外のコメント 
        ///     1: 選択式コメント
        /// </summary>
        public int SelectCmtId { get; private set; }

        /// <summary>
        /// 検査ラベル発行枚数
        ///     >=1 の場合、検査ラベル発行対象項目
        /// </summary>
        public int KensaLabel { get; private set; }

        public bool IsUpdated { get; private set; }

        public bool IsAddNew { get; private set; }

        public int IsDeleted { get; private set; }

        public bool IsUsage
        {
            get => ItemCd.StartsWith("Y");
        }

        public bool IsNotUsage
        {
            get => !IsUsage;
        }

        public bool IsLastItem { get; private set; }

        public void SetIsLastItem()
        {
            IsLastItem = true;
        }

        public bool IsStartDateKeyUpdated { get; private set; }

        public int OriginStartDate { get; private set; }

        /// <summary>
        /// Specifies the currently selected version of tenMst in client
        /// </summary>
        public bool IsSelected { get; private set; }

        public int CreateId { get; private set; }

        public ItemTypeEnums GetItemType()
        {
            if (ItemCd.StartsWith("J"))
            {
                return ItemTypeEnums.JihiItem;
            }
            else if (ItemCd.StartsWith("Z"))
            {
                return ItemTypeEnums.SpecificMedicalMeterialItem;
            }
            else if (ItemCd.StartsWith("Y"))
            {
                return ItemTypeEnums.UsageItem;
            }
            else if (ItemCd.StartsWith("W"))
            {
                return ItemTypeEnums.SpecialMedicineCommentItem;
            }
            else if (ItemCd.StartsWith("CO"))
            {
                return ItemTypeEnums.COCommentItem;
            }
            else if (ItemCd.StartsWith("KONI"))
            {
                return ItemTypeEnums.KonikaItem;
            }
            else if (ItemCd.StartsWith("FCR"))
            {
                return ItemTypeEnums.FCRItem;
            }
            else if (ItemCd.StartsWith("KN"))
            {
                return ItemTypeEnums.KensaItem;
            }
            else if (ItemCd.StartsWith("IGE"))
            {
                return ItemTypeEnums.TokuiTeki;
            }
            else if (ItemCd.StartsWith("SMZ"))
            {
                return ItemTypeEnums.Shimadzu;
            }
            else if (ItemCd.StartsWith("S"))
            {
                return ItemTypeEnums.Jibaiseki;
            }
            else if (ItemCd.StartsWith("X"))
            {
                return ItemTypeEnums.Dami;
            }
            else if (ItemCd.StartsWith("1") && ItemCd.Length == 9)
            {
                return ItemTypeEnums.ShinryoKoi;
            }
            else if (ItemCd.StartsWith("6") && ItemCd.Length == 9)
            {
                return ItemTypeEnums.Yakuzai;
            }
            else if (ItemCd.StartsWith("7") && ItemCd.Length == 9)
            {
                return ItemTypeEnums.Tokuzai;
            }
            else if (ItemCd.Length == 4)
            {
                return ItemTypeEnums.Bui;
            }
            else if (ItemCd.StartsWith("8") && ItemCd.Length == 9)
            {
                return ItemTypeEnums.CommentItem;
            }
            return ItemTypeEnums.Other;
        }

        public TenMstOriginModel ChangeHpId(int hpId)
        {
            HpId = hpId;
            return this;
        }
    }
}
