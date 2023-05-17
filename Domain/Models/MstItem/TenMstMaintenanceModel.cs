namespace Domain.Models.MstItem
{
    public class TenMstMaintenanceModel
    {
        public TenMstMaintenanceModel(int hpId, string itemCd, int startDate, int endDate, string masterSbt, int sinKouiKbn, string name, string kanaName1, string kanaName2, string kanaName3, string kanaName4, string kanaName5, string kanaName6, string kanaName7, string ryosyuName, string receName, int tenId, double ten, string receUnitCd, string receUnitName, string odrUnitName, string cnvUnitName, double odrTermVal, double cnvTermVal, double defaultVal, int isAdopted, int koukiKbn, string santeiItemCd, string itemType)
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
            SanteiItemCd = santeiItemCd;
            ItemType = itemType;
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

        /// <summary>
        /// 有効終了年月日
        ///     yyyymmdd
        /// </summary>
        public int EndDate { get; private set; }

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

        /// <summary>
        /// 診療行為区分
        /// </summary>
        public int SinKouiKbn { get; private set; }

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
        /// 算定診療行為コード
        ///     算定時の診療行為コード（自己結合でデータ取得）
        /// </summary>
        public string SanteiItemCd { get; private set; }

        public string ItemType { get; set; }

        public bool CheckDefaultValue()
        {
            return string.IsNullOrEmpty(ItemCd);
        }
    }
}
