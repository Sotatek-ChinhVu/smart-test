namespace Helper.Constants
{
    public static class StaConfId
    {
        #region 患者情報
        /// <summary>
        /// 出力順
        /// 0:患者番号 1:氏名
        /// </summary>
        public const int OutputOrder = 1;

        public const int OutputOrder2 = 3;

        public const int OutputOrder3 = 4;

        /// <summary>
        /// 帳票の種類
        /// 0:患者一覧 1:患者一覧（処方・病名リスト） 2:宛名ラベル
        /// </summary>
        public const int ReportType = 2;

        /// <summary>
        /// 患者番号From
        /// </summary>
        public const int PtNumFrom = 10;

        /// <summary>
        /// 患者番号To
        /// </summary>
        public const int PtNumTo = 11;

        /// <summary>
        /// カナ氏名
        /// </summary>
        public const int KanaName = 12;

        /// <summary>
        /// 氏名
        /// </summary>
        public const int Name = 13;

        /// <summary>
        /// 生年月日From
        /// </summary>
        public const int BirthDayFrom = 14;

        /// <summary>
        /// 生年月日To
        /// </summary>
        public const int BirthDayTo = 15;

        /// <summary>
        /// 年齢From
        /// </summary>
        public const int AgeFrom = 16;

        /// <summary>
        /// 年齢To
        /// </summary>
        public const int AgeTo = 17;

        /// <summary>
        /// 年齢基準日
        /// </summary>
        public const int AgeRefDate = 18;

        /// <summary>
        /// 性別
        /// 1:男性 2:女性
        /// </summary>
        public const int Sex = 19;

        /// <summary>
        /// 郵便番号
        /// </summary>
        public const int HomePost = 20;

        /// <summary>
        /// 住所
        /// </summary>
        public const int Address = 21;

        /// <summary>
        /// 電話
        /// </summary>
        public const int PhoneNumber = 22;

        /// <summary>
        /// 登録日From
        /// </summary>
        public const int RegistrationDateFrom = 23;

        /// <summary>
        /// 登録日To
        /// </summary>
        public const int RegistrationDateTo = 24;

        /// <summary>
        /// テスト患者を含む
        /// </summary>
        public const int IncludeTestPt = 25;

        /// <summary>
        /// グループ
        /// </summary>
        public const int GroupSelected = 26;
        #endregion

        #region 保険情報
        /// <summary>
        /// 保険者番号From
        /// </summary>
        public const int HokensyaNoFrom = 100;

        /// <summary>
        /// 保険者番号To
        /// </summary>
        public const int HokensyaNoTo = 101;

        /// 記号
        /// </summary>
        public const int Kigo = 102;

        /// <summary>
        /// 番号
        /// </summary>
        public const int Bango = 103;

        /// <summary>
        /// 本人家族区分
        /// </summary>
        public const int HokenKbn = 104;

        /// <summary>
        /// 公費負担者番号From
        /// </summary>
        public const int KohiFutansyaNoFrom = 105;

        /// <summary>
        /// 公費負担者番号To
        /// </summary>
        public const int KohiFutansyaNoTo = 106;

        /// <summary>
        /// 公費特殊番号From
        /// </summary>
        public const int KohiTokusyuNoFrom = 107;

        /// <summary>
        /// 公費特殊番号To
        /// </summary>
        public const int KohiTokusyuNoTo = 108;

        /// <summary>
        /// 有効期限From
        /// </summary>
        public const int ExpireDateFrom = 109;

        /// <summary>
        /// 有効期限To
        /// </summary>
        public const int ExpireDateTo = 110;

        /// <summary>
        /// 保険種別
        /// </summary>
        public const int HokenSbt = 111;

        /// <summary>
        /// 法別番号1
        /// </summary>
        public const int Houbetu1 = 112;

        /// <summary>
        /// 法別番号2
        /// </summary>
        public const int Houbetu2 = 113;

        /// <summary>
        /// 法別番号3
        /// </summary>
        public const int Houbetu3 = 114;

        /// <summary>
        /// 法別番号4
        /// </summary>
        public const int Houbetu4 = 115;

        /// <summary>
        /// 法別番号5
        /// </summary>
        public const int Houbetu5 = 116;

        /// <summary>
        /// 高額療養費
        /// </summary>
        public const int Kogaku = 117;

        /// <summary>
        /// 公費保険番号From
        /// </summary>
        public const int KohiHokenNoFrom = 118;

        /// <summary>
        /// 公費保険番号枝番From
        /// </summary>
        public const int KohiHokenEdaNoFrom = 119;

        /// <summary>
        /// 公費保険番号To
        /// </summary>
        public const int KohiHokenNoTo = 120;

        /// <summary>
        /// 公費保険番号枝番To
        /// </summary>
        public const int KohiHokenEdaNoTo = 121;
        #endregion

        #region 病名情報
        /// <summary>
        /// 開始日From
        /// </summary>
        public const int StartDateFrom = 200;

        /// <summary>
        /// 開始日To
        /// </summary>
        public const int StartDateTo = 201;

        /// <summary>
        /// 転帰日From
        /// </summary>
        public const int TenkiDateFrom = 202;

        /// <summary>
        /// 転帰日To
        /// </summary>
        public const int TenkiDateTo = 203;

        /// <summary>
        /// 転帰区分
        /// 1:継続 2:治ゆ 3:死亡 4:中止
        /// </summary>
        public const int TenkiKbn = 204;

        /// <summary>
        /// 疾患区分
        /// 3:皮(1) 4:皮(2) 5:特疾 7:てんかん 8:特疾又はてんかん
        /// </summary>
        public const int SikkanKbn = 205;

        /// <summary>
        /// 疑い病名
        /// 1:疑い 2:疑い以外
        /// </summary>
        public const int IsDoubt = 206;

        /// <summary>
        /// 検索ワード
        /// </summary>
        public const int SearchWord = 207;

        /// <summary>
        /// （or, and）
        /// and:1
        /// </summary>
        public const int SearchWordMode = 208;

        /// <summary>
        /// 病名検索
        /// ByomeiCdをカンマ区切り
        /// </summary>
        public const int ByomeiCd = 209;

        /// <summary>
        /// （or, and）
        /// and:1
        /// </summary>
        public const int ByomeiCdOpt = 210;

        /// <summary>
        ///  検索病名（未コード化病名）
        /// </summary>
        public const int FreeByomei = 211;

        /// <summary>
        /// 難病外来
        /// </summary>
        public const int NanbyoCds = 212;
        #endregion

        #region 来院情報
        /// <summary>
        /// 来院日From
        /// </summary>
        public const int SindateFrom = 300;

        /// <summary>
        /// 来院日To
        /// </summary>
        public const int SindateTo = 301;

        /// <summary>
        /// 最終来院日From
        /// </summary>
        public const int LastVisitDateFrom = 302;

        /// <summary>
        /// 最終来院日To
        /// </summary>
        public const int LastVisitDateTo = 303;

        /// <summary>
        /// 状態
        /// 「0:予約 1:受付 3:一時保存 5:計算 7:精算待ち 9:済み」　をカンマ区切り
        /// </summary>
        public const int Status = 304;

        /// <summary>
        /// 受付種別
        /// UKETUKE_SBT_MST.KBN_ID をカンマ区切り
        /// </summary>
        public const int UketukeSbtId = 305;

        /// <summary>
        /// KA_MST.KA_ID をカンマ区切り
        /// </summary>
        public const int KaMstId = 306;

        /// <summary>
        /// 担当医
        /// USER_MST.USER_ID をカンマ区切り
        /// </summary>
        public const int UserMstId = 307;

        /// <summary>
        /// 新患
        /// </summary>
        public const int IsSinkan = 308;

        /// <summary>
        /// 来院日時点の年齢From
        /// </summary>
        public const int RaiinAgeFrom = 309;

        /// <summary>
        /// 来院日時点の年齢To
        /// </summary>
        public const int RaiinAgeTo = 310;

        /// <summary>
        /// 時間枠区分
        /// </summary>
        public const int JikanKbn = 311;
        #endregion

        #region 診療情報
        /// <summary>
        /// 対象データ
        ///     0:算定 1:オーダー
        /// </summary>
        public const int DataKind = 400;

        /// <summary>
        /// 検索項目
        ///     ItemCd,回数下限,回数上限,..
        /// </summary>
        public const int ItemCds = 403;

        /// <summary>
        /// 検索項目のオプション
        ///     0:or 1:and
        /// </summary>
        public const int ItemCdOpt = 404;

        /// <summary>
        /// 検索ワード
        /// </summary>
        public const int MedicalSearchWord = 401;

        /// <summary>
        /// 検索ワードの検索オプション
        ///     0:or 1:and
        /// </summary>
        public const int WordOpt = 402;

        /// <summary>
        /// 検索項目（未コード／コメント）
        /// </summary>
        public const int ItemCmt = 405;
        #endregion

        #region カルテ情報
        /// <summary>
        /// カルテ区分
        /// </summary>
        public const int KarteKbns = 500;

        /// <summary>
        /// 文字列検索
        /// </summary>
        public const int KarteSearchWords = 501;

        /// <summary>
        /// 検索ワードの検索オプション
        ///     0:or 1:and
        /// </summary>
        public const int KarteWordOpt = 502;
        #endregion

        # region 検査情報
        /// <summary>
        /// 依頼日Start
        /// </summary>
        public const int StartIraiDate = 600;

        /// <summary>
        /// 依頼日End
        /// </summary>
        public const int EndIraiDate = 601;

        /// <summary>
        /// 検査項目
        ///     項目コード,結果値下限,結果値上限,異常値<1:H 2:L 3:HorL>,… の繰り返し
        /// </summary>
        public const int KensaItemCds = 602;

        /// <summary>
        /// 検査項目の検索オプション
        ///     0:or 1:and
        /// </summary>
        public const int KensaItemCdOpt = 603;
        #endregion
    }
}
