using Entity.Tenant;
using Helper.Constants;
using Helper.Extension;
using PostgreDataContext;

namespace CommonCheckers
{
    public class SystemConfig
    {
        // private DBContextFactory dbService;
        private readonly TenantNoTrackingDataContext _tenantNoTrackingDataContext;
        private List<SystemConf> _systemConfigs = new List<SystemConf>();
        private static SystemConfig? _instance;

        private static readonly object _threadsafelock = new object();

        public static SystemConfig Instance
        {
            get
            {
                // Double-Checked Locking
                // Check if instance needs to be created to avoid unnecessary lock
                // everytime you request an instance of the service
                if (_instance == null)
                {
                    // Lock thread so only one thread can create the first instance
                    lock (_threadsafelock)
                    {
                        // Check if instance needs to be created
                        // This is to avoid initial initialization by two threads.
                        if (_instance == null)
                        {
                            _instance = new SystemConfig();
                        }
                    }
                }
                return _instance;
            }
        }

        private SystemConfig(TenantNoTrackingDataContext tenantNoTrackingDataContext)
        {
            _tenantNoTrackingDataContext = tenantNoTrackingDataContext;
        }

        public SystemConfig()
        {
            RefreshData();
        }

        public void RefreshData()
        {
            _systemConfigs = _tenantNoTrackingDataContext.SystemConfs.Where(p => p.HpId == TempIdentity.HpId).ToList();
        }

        public double GetSettingValue(int groupCd, int grpEdaNo = 0, int defaultValue = 0, bool fromLastestDb = false)
        {
            lock (_threadsafelock)
            {
                SystemConf systemConf = new SystemConf();
                if (!fromLastestDb)
                {
                    systemConf = _systemConfigs.FirstOrDefault(p => p.GrpCd == groupCd && p.GrpEdaNo == grpEdaNo) ?? new SystemConf();
                }
                else
                {
                    systemConf = _tenantNoTrackingDataContext.SystemConfs.Where(p =>
                        p.HpId == TempIdentity.HpId && p.GrpCd == groupCd && p.GrpEdaNo == grpEdaNo).FirstOrDefault() ?? new SystemConf();
                }
                return systemConf != null ? systemConf.Val : defaultValue;
            }
        }

        public bool CheckContainKey(int groupCd, int grpEdaNo = 0, int defaultValue = 0, bool fromLastestDb = false)
        {
            SystemConf systemConf = new SystemConf();
            if (!fromLastestDb)
            {
                systemConf = _systemConfigs.FirstOrDefault(p => p.GrpCd == groupCd && p.GrpEdaNo == grpEdaNo) ?? new SystemConf();
            }
            else
            {
                systemConf = _tenantNoTrackingDataContext.SystemConfs.Where(p =>
                    p.HpId == TempIdentity.HpId && p.GrpCd == groupCd && p.GrpEdaNo == grpEdaNo).FirstOrDefault() ?? new SystemConf();
            }
            return systemConf != null ? true : false;
        }

        public string GetSettingParam(int groupCd, int grpEdaNo = 0, string defaultParam = "", bool fromLastestDb = false)
        {
            lock (_threadsafelock)
            {
                SystemConf systemConf = new SystemConf();
                if (!fromLastestDb)
                {
                    systemConf = _systemConfigs.FirstOrDefault(p => p.GrpCd == groupCd && p.GrpEdaNo == grpEdaNo) ?? new SystemConf();
                }
                else
                {
                    systemConf = _tenantNoTrackingDataContext.SystemConfs.Where(p =>
                        p.HpId == TempIdentity.HpId && p.GrpCd == groupCd && p.GrpEdaNo == grpEdaNo).FirstOrDefault() ?? new SystemConf();
                }
                //Fix comment 894 (duong.vu)
                //Return value in DB if and only if Param is not null or white space
                if (systemConf != null && !string.IsNullOrWhiteSpace(systemConf.Param))
                {
                    return systemConf.Param;
                }

                return defaultParam;
            }
        }

        private List<SystemConf> GetListGroupCd(int groupCd, bool fromLastestDb = false)
        {
            lock (_threadsafelock)
            {
                List<SystemConf> systemConfs = new List<SystemConf>();
                if (!fromLastestDb)
                {
                    systemConfs = _systemConfigs.FindAll(p => p.GrpCd == groupCd);
                }
                else
                {
                    systemConfs = _tenantNoTrackingDataContext.SystemConfs.Where(p =>
                        p.HpId == TempIdentity.HpId && p.GrpCd == groupCd).ToList();
                }
                return systemConfs != null ? systemConfs : new List<SystemConf>();
            }
        }


        /// <summary>
        /// // 0: 削除患者の番号を再利用しない
        // 1: 削除患者の番号を再利用する
        /// </summary>
        public int EmptyPtIdSetting { get => (int)GetSettingValue(1014, 0); }

        public long AutoPtNumStartValue { get => (long)GetSettingValue(1014, 1); }

        public int IsFirstVisitTime { get => (int)GetSettingValue(1007, 0); }

        public int UketukeNoMode { get => (int)GetSettingValue(1008, 0); }

        public int UketukeNoStart { get => GetSettingParam(1008, defaultParam: "1").AsInteger(); }

        public int DefaultDoctorSetting { get => (int)GetSettingValue(1009, 0); }

        public int SameVisitSetting { get => (int)GetSettingValue(1010, 0); }

        public int FirstTimeConfirmDays { get => (int)GetSettingValue(1013, 0, 90); }
        /// <summary>
        /// 労災レセプト電算 0-なし、1-あり
        /// </summary>
        public int RosaiReceden { get => (int)GetSettingValue(100003, 0); }
        /// <summary>
        /// 労災レセプト電算 開始年月(yyyyMM)
        /// </summary>
        public string RosaiRecedenTerm { get => GetSettingParam(100003, 0); }

        public int IsRyoSyoDetail { get => (int)GetSettingValue(93002, 0); }

        public int PtNumCheckDigit { get => (int)GetSettingValue(1001); }

        public int FKanNmChkJIS { get => (int)GetSettingValue(1003); }

        public int TorokuJiYoyaku { get => (int)GetSettingValue(1004); }

        public int ExistPediatrics { get => (int)GetSettingValue(3004); }
        ///// <summary>
        ///// 小児科標榜有無
        /////     0:標榜なし（既定値）
        /////     1:標榜あり
        ///// </summary>
        //public int IsHyoboSyounika { get => (int)GetSettingValue(3004, 0); }
        ///// <summary>
        ///// 産科標榜有無
        /////     0:標榜なし（既定値）
        /////     1:標榜あり
        ///// </summary>
        //public int IsHyoboSanka { get => (int)GetSettingValue(3004, 1); }
        /// <summary>
        /// 自賠準拠区分
        ///     0:健保準拠（既定値）
        ///     1:労災準拠
        /// </summary>
        public int JibaiJunkyo { get => (int)GetSettingValue(3001, 0); }
        /// <summary>
        /// 自賠労災準拠加算率
        /// </summary>
        public double JibaiRousaiRate { get => GetSettingValue(3001, 1); }
        /// <summary>
        /// 多剤投与の臨時判断日数
        ///     既定値は14
        /// </summary>
        public int SyohoRinjiDays { get => (int)GetSettingValue(3002, 0, 14); }

        /// <summary>
        /// 自動入金
        ///     0:しない 1:する
        /// </summary>
        public int AutoNyukin { get => (int)GetSettingValue(3003, 0); }
        /// <summary>
        /// レセプト院外処方のコメント／用法出力
        ///     0:しない
        ///     1:する（既定値）
        /// </summary>
        public int OutDrugYohoDsp { get => (int)GetSettingValue(3005, 0, 1); }
        /// <summary>
        /// 処方料・調剤料・調基・処方箋料　自動算定時の保険組み合わせID
        ///     0:公費を優先
        ///     1:上位にオーダーされている内容に合わせる
        /// </summary>
        public int DrugPid { get => (int)GetSettingValue(3007, 0, 0); }

        public int HokenKohiKbn { get => (int)GetSettingValue(3006, 0); }

        /// <summary>
        /// マル長の特記事項記載
        ///     0:上限未満は記載しない
        ///     1:上限未満でも記載する
        /// </summary>
        public int ChokiTokki { get => (int)GetSettingValue(3006, 1, 0); }

        /// <summary>
        /// 15更生があり異点数の場合のマル長の負担額
        ///     0:公費負担額を含む
        ///     1:社保/公費負担額を含む　　　国保/公費負担額を含まない
        ///     2:社保/公費負担額を含まない　国保/公費負担額を含む
        ///     3:公費負担額を含まない
        /// </summary>
        public int ChokiFutan { get => (int)GetSettingValue(3006, 2, 0); }

        /// <summary>
        /// マル長計算オプション
        ///     0:月単位
        ///     1:日単位（公費負担額を含むのみ）
        ///     2:社保/日単位　国保/月単位
        ///     3:社保/月単位　国保/日単位
        /// </summary>
        /// <remarks>
        ///     公１が5000円上限、マル長10000円で、1日目にマル長上限に達して公1上限未満だった場合に、
        ///     2日目以降に公1上限まで患者負担させるかどうか（月単位の場合は公1上限まで患者負担させる）
        /// </remarks>
        public int ChokiDateRange { get => (int)GetSettingValue(3006, 3, 0); }

        /// <summary>
        /// 包括背反チェックモード
        ///     0:1項目につき、すべての設定についてチェック
        ///     1:1項目につき、1つ該当する設定があれば次の項目をチェック
        /// </summary>
        public int HoukatuHaihanCheckMode { get => (int)GetSettingValue(3008, 0, 0); }
        /// <summary>
        /// 包括背反ログ出力モード
        ///     0:すべて出力
        ///     1:自動発生項目は追加しない
        /// </summary>
        public int HoukatuHaihanLogputMode { get => (int)GetSettingValue(3009, 0, 0); }
        /// <summary>
        /// 包括背反ログで条件付きのログを出力するかどうか
        ///     0:出力しない
        ///     1:出力する
        /// </summary>  
        public int HoukatuHaihanSPJyokenLogputMode { get => (int)GetSettingValue(3009, 1, 0); }

        /// <summary>
        /// 公費給付対象額
        ///     0:記載する（異点数のみ）
        ///     1:記載する（社保/異点数のみ）
        ///     2:記載する（国保/異点数のみ）
        ///     3:記載する（すべて）
        ///     9:記載しない
        /// </summary>
        public int ReceKyufuKisai { get => (int)GetSettingValue(3010, 0, 0); }

        /// <summary>
        /// 公費給付対象額（公２以降）
        ///     0:記載する
        ///     1:社保/マル長の場合は記載しない
        ///     2:国保/マル長の場合は記載しない
        ///     3:マル長の場合は記載しない
        /// </summary>
        public int ReceKyufuKisai2 { get => (int)GetSettingValue(3010, 1, 0); }

        /// <summary>
        /// 奈良県社保＋福祉のレセコメント出力
        ///     1: 出力する
        /// Paramに開始請求年月(YYYYMM)
        /// </summary>
        public int NaraFukusiReceCmt { get => (int)GetSettingValue(3011, 0, 0); }
        public string NaraFukusiReceCmtStartDate { get => (string)GetSettingParam(3011, 0, ""); }

        /// <summary>
        /// 高額療養費の窓口負担まるめ設定
        ///     0:1円単位
        ///     1:10円単位(四捨五入)
        ///     2:10円単位(切り捨て)
        /// </summary>
        public int RoundKogakuPtFutan { get => (int)GetSettingValue(3016, 0); }
        /// <summary>
        /// 検査まるめ分点（社保）
        /// 0 同一Rpにまとめる
        /// 1 別Rpに分ける
        /// </summary>
        public int KensaMarumeBuntenSyaho { get => (int)GetSettingValue(3017, 0); }
        /// <summary>
        /// 検査まるめ分点（国保）
        /// 0 同一Rpにまとめる
        /// 1 別Rpに分ける
        /// </summary>
        public int KensaMarumeBuntenKokuho { get => (int)GetSettingValue(3017, 1); }
        /// <summary>
        /// 自動発生コメント
        /// 0 算定情報優先
        /// 1 コメント区分優先(その他行為以外)
        /// </summary>
        public int CalcAutoComment { get => (int)GetSettingValue(3018, 0); }
        /// <summary>
        /// 検査重複チェックのログ出力
        /// 0 する
        /// 1 しない
        /// </summary>
        public int CalcCheckKensaDuplicateLog { get => (int)GetSettingValue(3019, 0); }
        /// <summary>
        /// レセプト院内処方の用法コメント出力
        /// </summary>
        public int InDrugYohoComment { get => (int)GetSettingValue(3022, 0, 0); }
        /// <summary>
        /// 訪問看護指導料が再診料を包括するかどうか
        /// 0 する
        /// 1 しない
        /// </summary>
        public int HoumonKangoSaisinHokatu { get => (int)GetSettingValue(3023, 0, 0); }
        /// <summary>
        /// 小児特定疾患カウンセリング料初回算定チェックを行うかどうか
        /// 0 初回算定日から2年まで
        /// 1 しない
        /// </summary>
        public int SyouniCounselingCheck { get => (int)GetSettingValue(3024, 0, 0); }
        public int AutoShowMonshinSheet { get => 1; }

        /// <summary>
        /// 0: あり（確認なし）
        /// 1: あり（確認あり）
        /// 2: なし
        /// </summary>
        public int CheckItemName => (int)GetSettingValue(2007, 0);

        /// <summary>
        /// 履歴からオーダーしたとき、保険パターンを引き継ぐかどうか
        /// 0: 初再診の保険パターンを設定
        /// 1: 履歴で使用した保険パターンを参考に決定
        /// </summary>
        public int AutoUseHokenPatternFromHistory { get => (int)GetSettingValue(2008, 0); }

        /// <summary>
        /// 保険パターンの期限切チェックを行うタイミング
        /// 0: オーダーされる度に毎回チェックする
        /// 1: 保険パターンごとにチェックする
        /// </summary>
        public int HokenPatternExpiredCheckTiming { get => (int)GetSettingValue(2008, 2); }

        /// <summary>
        /// 履歴からオーダーしたとき、自費算定を引き継ぐかどうか
        /// 0: 初再診の自費算定を設定
        /// 1: 履歴で使用した自費算定を参考に決定
        /// </summary>
        public int AutoUseJihiSanteiFromHistory { get => (int)GetSettingValue(2008, 1); }

        /// <summary>
        /// 外来リハビリテーション診療料
        /// 0: なし
        /// 1: あり
        /// </summary>
        public int CheckGairaiRiha => (int)GetSettingValue(2016);

        /// <summary>
        /// 予防注射再診チェック
        /// 0: しない
        /// 1: する（確認あり）
        /// 2: する（確認なし）
        /// </summary>
        public int CheckJihiYobo => (int)GetSettingValue(2016, 2);

        /// <summary>
        /// 年齢制限
        /// 0: なし
        /// 1: あり
        /// </summary>
        public int CheckAge => (int)GetSettingValue(2017);

        /// <summary>
        /// 検査項目の院内院外初期値
        /// 0: 院内
        /// 1: 院外
        /// </summary>
        public int DefaultInOutKensa { get => (int)GetSettingValue(2018, 0); }

        /// <summary>
        /// 検査依頼可否チェック
        /// 0: チェックしない
        /// 1: 外注依頼不可
        /// 2: 外注コード未設定
        /// </summary>
        public int CheckKensaIrai { get => (int)GetSettingValue(2019, 0); }

        /// <summary>
        /// 検査依頼可否チェック　（対象）
        /// 0: 検体・病理
        /// 1: 検体（微生物検査除く）
        /// </summary>
        public int CheckKensaIraiCondition { get => (int)GetSettingValue(2019, 1); }

        /// <summary>
        /// 後発品オーダー時の他銘柄変更・一般名処方可否
        /// 0: 変更不可
        /// 1: 他銘柄変更
        /// 2: 一般名処方
        /// </summary>
        public int AutoSetSyohoKbnKohatuDrug { get => (int)GetSettingValue(2020, 0); }
        /// <summary>
        /// 後発品オーダー時の変更の制限
        /// 0: 制限なし
        /// 1: 剤型変更不可
        /// 2: 含量規格変更不可
        /// 3: 含量規格・剤型変更不可
        /// </summary>
        public int AutoSetSyohoLimitKohatuDrug { get => (int)GetSettingValue(2020, 1); }
        /// <summary>
        /// 後発品オーダー時の処方箋記載自動設定　セット優先区分
        /// 0: セットの設定を無視する
        /// 1: セットの設定を優先する
        /// </summary>
        public int AutoSetSyohoKisaiIgnoreSetKohatuDrug { get => (int)GetSettingValue(2020, 2); }
        /// <summary>
        /// 後発品のある先発品オーダー時の他銘柄変更・一般名処方可否
        /// 0: 変更不可
        /// 1: 他銘柄変更
        /// 2: 一般名処方
        /// </summary>
        public int AutoSetSyohoKbnSenpatuDrug { get => (int)GetSettingValue(2021, 0); }
        /// <summary>
        /// 後発品のある先発品オーダー時の変更の制限
        /// 0: 制限なし
        /// 1: 剤型変更不可
        /// 2: 含量規格変更不可
        /// 3: 含量規格・剤型変更不可
        /// </summary>
        public int AutoSetSyohoLimitSenpatuDrug { get => (int)GetSettingValue(2021, 1); }
        /// <summary>
        /// 後発品のある先発品オーダー時の処方箋記載自動設定　セット優先区分
        /// 0: セットの設定を無視する
        /// 1: セットの設定を優先する
        /// </summary>
        public int AutoSetSyohoKisaiIgnoreSetSenpatuDrug { get => (int)GetSettingValue(2021, 2); }

        public int FoodAllergyLevelSetting { get => (int)GetSettingValue(2027, 0, 3); }

        public int AgeLevelSetting { get => (int)GetSettingValue(2027, 3, 8); }

        public int AgeTypeCheckSetting { get => (int)GetSettingValue(2030, 0, 0); }

        public int CheckDupicatedSetting { get => (int)GetSettingValue(2027, 4, 1); }

        public int DiseaseLevelSetting { get => (int)GetSettingValue(2027, 2, 3); }

        public int KinkiLevelSetting { get => (int)GetSettingValue(2027, 1, 4); }

        public double DosageRatioSetting { get => GetSettingValue(2023, 0, 1); }

        public bool DosageMinCheckSetting { get => GetSettingValue(2023, 1, 1) != 0; }

        public bool DosageDrinkingDrugSetting { get => GetSettingValue(2023, 2, 1) != 0; }

        public bool DosageDrugAsOrderSetting { get => GetSettingValue(2023, 3, 1) != 0; }

        public bool DosageOtherDrugSetting { get => GetSettingValue(2023, 4, 1) != 0; }

        public int DoExcludeItemFromHistory { get => (int)GetSettingValue(2033, 0, 0); }

        /// <summary>
        ///0 : チェックしない
        ///1 : 疑い病名の場合、疾患区分を設定しない
        ///2 : 疑い病名の場合、確認メッセージを表示（既定値）												
        /// </summary>
        public int SuspectedByomeiCheckMode { get => (int)GetSettingValue(2014, 0, 2); }

        /// <summary>
        ///0 : チェックしない									
        ///1 : 重複時、登録しない
        ///2 : 重複時、確認メッセージを表示（既定値）									
        /// </summary>
        public int DuplicatedByomeiCheckMode { get => (int)GetSettingValue(2015, 0, 2); }

        /// <summary>
        /// 体重別セットの体重確認日数
        /// 0 : 表示しない
        /// >=1	
        /// D 最終測定日から指定日後の初回来院時								
        /// M 最終測定日から指定月後の初回来院時
        /// </summary>
        public int WeightConfirmDays { get => GetSettingParam(2010, 0).AsInteger(); }

        public int WeightCheckType { get => (int)GetSettingValue(2010, 0); }

        /// <summary>
        /// 0 : 小数点以下第2位で四捨五入
        /// 1～4 : 小数点以下第N位で四捨五入（Nは設定値）
        /// </summary>
        public int WeightRound { get => (int)GetSettingValue(2011, 0); }

        public bool IsDuplicatedComponentChecked { get => (int)GetSettingValue(2026, 0, 1) == 1; }

        public bool IsProDrugChecked { get => (int)GetSettingValue(2026, 1, 1) == 1; }

        public bool IsSameComponentChecked { get => (int)GetSettingValue(2026, 2, 1) == 1; }

        public bool IsDuplicatedClassChecked { get => (int)GetSettingValue(2026, 3, 1) == 1; }

        public bool IsDuplicatedComponentForDuplication { get => (int)GetSettingValue(2024, 0, 1) == 1; }

        public bool IsProDrugForDuplication { get => (int)GetSettingValue(2024, 1, 1) == 1; }

        public bool IsSameComponentForDuplication { get => (int)GetSettingValue(2024, 2, 1) == 1; }

        public bool IsDuplicatedClassForDuplication { get => (int)GetSettingValue(2024, 3, 1) == 1; }

        public int GetHaigouSetting { get => (int)GetSettingValue(2025, 0, 3); }

        public int ReceNoDspComment { get => (int)GetSettingValue(3012, 0, 0); }

        public int UpdateIgnoreOption { get => (int)GetSettingValue(8040, 0, 0); }

        public string SortPrinterSetting { get => GetSettingParam(93005, 0, defaultParam: "領収証,明細,院外処方箋,薬袋ラベル,薬情,お薬手帳シール"); }

        /// <summary>
        /// 承認機能	
        　      ///（未承認カルテの通知）
        /// 0:使用しない
        /// 1:使用する
        /// </summary>
        public int ApprovalFunc { get => (int)GetSettingValue(2022); }

        public string ApprovalFuncParam { get => GetSettingParam(2022); }

        public int ApprovalNumberOfStartDateAgo { get => (int)GetSettingValue(2022, 1, 0); }

        //Dictionary<int, SolidColorBrush> _statusColorsHex;
        //public Dictionary<int, SolidColorBrush> StatusColorsHex(bool lastValue = false)
        //{
        //    if (lastValue == true || _statusColorsHex == null)
        //    {
        //        _statusColorsHex = new Dictionary<int, SolidColorBrush>();
        //        _statusColorsHex.Add(0, CIUtil.ColorBrush(GetSettingParam(5003, 0)));
        //        _statusColorsHex.Add(1, CIUtil.ColorBrush(GetSettingParam(5003, 1)));
        //        _statusColorsHex.Add(2, CIUtil.ColorBrush(GetSettingParam(5003, 2)));
        //        _statusColorsHex.Add(3, CIUtil.ColorBrush(GetSettingParam(5003, 3)));
        //        _statusColorsHex.Add(5, CIUtil.ColorBrush(GetSettingParam(5003, 5)));
        //        _statusColorsHex.Add(7, CIUtil.ColorBrush(GetSettingParam(5003, 7)));
        //        _statusColorsHex.Add(9, CIUtil.ColorBrush(GetSettingParam(5003, 9)));
        //    }
        //    return _statusColorsHex;
        //}

        //Dictionary<int, SolidColorBrush> _elapsedTime;
        //public Dictionary<int, SolidColorBrush> ElapsedTime(bool lastValue = false)
        //{
        //    lock (_threadsafelock)
        //    {
        //        if (lastValue == true || _elapsedTime == null)
        //        {
        //            _elapsedTime = new Dictionary<int, SolidColorBrush>();
        //            List<SystemConf> listTime = GetListGroupCd(5002).OrderBy(item => item.GrpEdaNo).ToList();
        //            if (listTime == null || listTime.Count <= 0)
        //            {
        //                return _elapsedTime;
        //            }

        //            foreach (var item in listTime)
        //            {
        //                _elapsedTime.Add(item.GrpEdaNo, CIUtil.ColorBrush(item.Param));
        //            }
        //        }
        //    }
        //    return _elapsedTime;
        //}
        /// <summary>
        /// 受診票　Rp名称印字
        ///     0:しない
        ///     1:する
        /// </summary>
        public int JyusinHyoRpName { get => (int)GetSettingValue(91001, 1); }
        /// <summary>
        /// 受診票　検査容器/材料情報印字
        ///     0:しない
        ///     1:する
        /// </summary>
        public int JyusinHyoKensaYokiZairyo { get => (int)GetSettingValue(91001, 2); }
        /// <summary>
        /// 受診票　印刷対象来院区分設定
        /// </summary>
        public string JyusinHyoRaiinKbn { get => GetSettingParam(91001, 3); }
        /// <summary>
        /// 受診票　患者コメント
        ///     0:印字する
        ///     1:印字しない
        /// </summary>
        public int JyusinHyoPtCmt { get => (int)GetSettingValue(91001, 4); }
        /// <summary>
        /// 受診票　アレルギー情報
        ///     0:印字する
        ///     1:印字しない
        /// </summary>
        public int JyusinHyoAlrgy { get => (int)GetSettingValue(91001, 5); }
        /// <summary>
        /// オーダーラベル　予約オーダー印字
        ///     0:変更のあった予約オーダーの来院すべてを印字
        ///     1:変更のあった予約オーダーの項目を印字
        ///     2:印字しない
        /// </summary>
        public int OrderLabelYoyakuOrderDsp { get => (int)GetSettingValue(92001, 1); }
        /// <summary>
        /// オーダーラベル　検査項目表示
        ///     0:1行複数項目
        ///     1:1行1項目
        /// </summary>
        public int OrderLabelKensaDsp { get => (int)GetSettingValue(92001, 3); }
        /// <summary>
        /// オーダーラベル　次回予約日の印字
        ///     0:印字しない
        ///     1:印字する
        /// </summary>
        public int OrderLabelYoyakuDateDsp { get => (int)GetSettingValue(92001, 4); }
        /// <summary>
        /// オーダーラベル　ヘッダー印字パターン
        ///     0:1印刷ごとに1つ印字
        ///     1:Rpごとに1つ印字
        /// </summary>
        public int OrderLabelHeaderPrint { get => (int)GetSettingValue(92001, 5); }
        /// <summary>
        /// オーダーラベル　算定外マーク表示
        ///     0:表示しない
        ///     1:表示する
        /// </summary>
        public int OrderLabelSanteiGaiDsp { get => (int)GetSettingValue(92001, 6); }
        /// <summary>
        /// オーダーラベル　診療科の印字
        /// 　　0:印字しない
        /// 　　1:印字する
        /// </summary>
        public int OrderLabelKaPrint { get => (int)GetSettingValue(92001, 7); }
        /// <summary>
        /// オーダーラベル　初再診の印字
        /// 　　0:印字しない
        /// 　　1:印字する
        /// </summary>
        public int OrderLabelSyosaiPrint { get => (int)GetSettingValue(92001, 8); }
        /// <summary>
        /// オーダーラベル　入力者の印字
        /// 　　0:印字しない
        /// 　　1:印字する
        /// </summary>
        public int OrderLabelCreateNamePrint { get => (int)GetSettingValue(92001, 10); }

        public int OrderLabelOdrKouiKbn { get => (int)GetSettingValue(92001, 11); }
        /// <summary>
        /// 院外処方箋　臨時のRpに臨時のコメントを自動記載するかどうか
        ///     0:しない
        ///     1:する
        /// </summary>
        public int SyohosenRinjiKisai { get => (int)GetSettingValue(92003, 1); }
        /// <summary>
        /// 院外処方箋　地域包括診療料等算定時、処方箋備考欄にコメント自動印字
        ///     0:しない
        ///     1:する
        /// </summary>
        public int SyohosenChiikiHoukatu { get => (int)GetSettingValue(92003, 2); }
        /// <summary>
        /// 院外処方箋　QRコード印字
        ///     0:しない
        ///     1:する
        /// </summary>
        public int SyohosenQRKbn { get => (int)GetSettingValue(92003, 3); }
        /// <summary>
        /// 院外処方箋　処方箋単位
        ///     0:オーダー単位
        ///     1:レセ単位
        /// </summary>
        public int SyohosenTani { get => (int)GetSettingValue(92003, 4); }
        /// <summary>
        /// 院外処方箋　控え印刷
        ///     0:しない
        ///     1:する
        /// </summary>
        public int SyohosenHikae { get => (int)GetSettingValue(92003, 5); }
        /// <summary>
        /// 院外処方箋　負担率印字
        ///     0:しない
        ///     1:する
        /// </summary>
        public int SyohosenFutanRate { get => (int)GetSettingValue(92003, 6); }
        /// <summary>
        /// 院外処方箋　QRコードのバージョン
        ///     0:JAHIS5
        ///     1:JAHIS7
        ///     2:JAHIS8
        ///     3:JAHIS9
        /// </summary>
        public int SyohosenQRVersion { get => (int)GetSettingValue(92003, 7); }
        /// <summary>
        /// リフィル処方0回の時の印字文字
        /// </summary>
        public string SyohosenRefillZero { get => GetSettingParam(92003, 9); }
        /// <summary>
        /// リフィル処方でない場合のリフィル可欄の取り繕い
        ///     1:取り消し線を引く
        /// </summary>
        public int SyohosenRefillStrikeLine { get => (int)GetSettingValue(92003, 9); }
        /// <summary>
        /// 指示箋　Rp名称印字
        ///     0:しない
        ///     1:する
        /// </summary>
        public int SijisenRpName { get => (int)GetSettingValue(92008, 1); }
        /// <summary>
        /// 指示箋　検査容器/材料情報印字
        ///     0:しない
        ///     1:する
        /// </summary>
        public int SijisenKensaYokiZairyo { get => (int)GetSettingValue(92008, 2); }
        /// <summary>
        /// 指示箋　患者コメント
        ///     0:印字する
        ///     1:印字しない
        /// </summary>
        public int SijisenPtCmt { get => (int)GetSettingValue(92008, 3); }
        /// <summary>
        /// 指示箋　アレルギー情報
        ///     0:印字する
        ///     1:印字しない
        /// </summary>
        public int SijisenAlrgy { get => (int)GetSettingValue(92008, 4); }

        public int SijisenCheckMachine { get => (int)GetSettingValue(92008, 5); }

        public string SijisenCheckMachineParam { get => GetSettingParam(92008, 5, "KrtRenkei,TKImport"); }
        /// <summary>
        /// 領収証タイプ
        /// </summary>
        public int AccountingFormType { get => (int)GetSettingValue(93001, 1); }
        /// <summary>
        /// 月間領収証タイプ
        /// </summary>
        public int AccountingMonthFormType { get => (int)GetSettingValue(93001, 2); }
        /// <summary>
        /// 領収証明細タイプ
        /// </summary>
        public int AccountingDetailFormType { get => (int)GetSettingValue(93002, 1); }
        /// <summary>
        /// 月間領収証明細タイプ
        /// </summary>
        public int AccountingDetailMonthFormType { get => (int)GetSettingValue(93002, 2); }
        /// <summary>
        /// 領収証明細　院外処方印字
        ///     0:印字しない　1:印字する
        /// </summary>
        public int AccountingDetailIncludeOutDrug { get => (int)GetSettingValue(93002, 4); }
        /// <summary>
        /// 領収証明細　コメント印字
        ///     0:印字しない　1:印字する
        /// </summary>
        public int AccountingDetailIncludeComment { get => (int)GetSettingValue(93002, 5); }
        /// <summary>
        /// 下位互換フィールドの使用
        ///     0:使用しない　1:使用する（既定値）
        /// </summary>
        public int AccountingUseBackwardFields { get => (int)GetSettingValue(93003, 0, 1); }
        /// <summary>
        /// 領収証定型文
        ///     0:印字しない　1:印字する
        /// </summary>
        public int AccountingTeikeibunPrint { get => (int)GetSettingValue(93004, 0); }
        /// <summary>
        /// 領収証定型文 1行目
        /// </summary>
        public string AccountingTeikeibun1 => GetSettingParam(93004, 1);
        /// <summary>
        /// 領収証定型文 2行目
        /// </summary>
        public string AccountingTeikeibun2 => GetSettingParam(93004, 2);
        /// <summary>
        /// 領収証定型文 3行目
        /// </summary>
        public string AccountingTeikeibun3 => GetSettingParam(93004, 3);
        /// <summary>
        /// 光ディスク送付書備考欄
        ///     0:総件数を記載しない
        ///     1:総件数を記載する
        /// </summary>
        public int HikariDiskIsTotalCnt { get => (int)GetSettingValue(94004, 0); }
        /// <summary>
        /// レセプト病名記載
        ///     0: 1行複数
        ///     1: 1行1病名
        /// </summary>
        public int ReceiptByomeiWordWrap { get => (int)GetSettingValue(94001, 0); }
        /// <summary>
        /// レセプト病名転帰日記載
        ///     0: 記載しない
        ///     1: 記載する
        /// </summary>
        public int ReceiptByomeiTenkiDate { get => (int)GetSettingValue(94001, 1); }
        /// <summary>
        /// 労災レセプト様式
        ///     0:旧様式
        ///     1:新様式(2019/05~）
        /// </summary>
        public int RousaiReceiptType { get => (int)GetSettingValue(94002, 0); }
        /// <summary>
        /// 東京都国保総括表用紙タイプ
        ///     0:専用紙
        ///     1:オーバーレイ
        /// </summary>
        public int P13KokhoSokatuType { get => (int)GetSettingValue(94003, 0); }
        /// <summary>
        /// 東京都負担医療費請求書用紙タイプ
        ///     0:専用紙
        ///     1:オーバーレイ
        /// </summary>
        public int P13WelfareGreenSeikyuType { get => (int)GetSettingValue(94003, 2); }
        /// <summary>
        /// 東京都難病医療費請求書用紙タイプ
        ///     0:専用紙
        ///     1:オーバーレイ
        /// </summary>
        public int P13WelfareBlueSeikyuType { get => (int)GetSettingValue(94003, 3); }
        /// <summary>
        /// 小児喘息レセプトタイプ
        ///     0:大阪市タイプ
        ///     1:東大阪市タイプ
        /// </summary>
        public int SyouniZensokuType { get => (int)GetSettingValue(94005, 0); }
        /// <summary>
        /// レセプト電算院外処方診療区分タイプ
        ///     0: 2x
        ///     1: Zx
        /// </summary>
        public int ReceiptOutDrgSinId { get => (int)GetSettingValue(94006, 0); }
        /// <summary>
        /// レセプトコメントのみのRpに点数×回数を記載するか
        ///     0: 記載しない
        ///     1: 記載する
        /// </summary>
        public int ReceiptCommentTenCount { get => (int)GetSettingValue(94007, 0); }
        /// <summary>
        /// 病名条件
        /// </summary>
        public int ByomeiCondition => (int)GetSettingValue(2002, 4);

        /// <summary>
        /// 特処１病名条件
        /// </summary>
        public int ByomeiShuByomei1 => (int)GetSettingValue(2002, 0);

        /// <summary>
        /// 特処１薬剤条件
        /// </summary>
        public int ByomeiShoho1 => (int)GetSettingValue(2002, 1);

        /// <summary>
        /// 特処２病名条件
        /// </summary>
        public int ByomeiShuByomei2 => (int)GetSettingValue(2002, 2);

        /// <summary>
        /// 特処２薬剤条件
        /// </summary>
        public int ByomeiShoho2 => (int)GetSettingValue(2002, 3);

        /// <summary>
        /// 特定疾患療養管理料
        /// </summary>
        public int SanteiTokuteiSikkan => (int)GetSettingValue(4001, 0);

        /// <summary>
        /// 皮膚科特定疾患指導管理料
        /// </summary>
        public int HifukaSanteiTokuteiSikkan => (int)GetSettingValue(4001, 2);

        /// <summary>
        /// てんかん指導料
        /// </summary>
        public int SanteiTenkan => (int)GetSettingValue(4001, 6);

        /// <summary>
        /// 難病外来指導管理料
        /// </summary>
        public int NanbyoGairai => (int)GetSettingValue(4001, 7);

        /// <summary>
        /// 特定疾患処方管理加算
        /// </summary>
        public int TouyakuTokuSyoSyoho => (int)GetSettingValue(4001, 1);

        /// <summary>
        /// 地域包括診療加算・自動算定
        /// 0: 自動算定なし
        /// 1: 自動算定あり
        /// </summary>
        public int TikiHokatuJidoSantei => (int)GetSettingValue(4001, 4);

        /// <summary>
        /// 地域包括診療加算
        /// 0: 加算２
        /// 1: 加算１
        /// </summary>
        public int TikiHokatu => (int)GetSettingValue(4001, 8);

        /// <summary>
        /// 薬剤情報提供料
        /// </summary>
        public int YakkuzaiJoho => (int)GetSettingValue(4001, 5);

        /// <summary>
        /// オンライン医学管理料
        /// </summary>
        public int SiOnline => (int)GetSettingValue(4001, 3);

        /// <summary>
        /// 残薬確認
        /// </summary>
        public int ZanYaku => (int)GetSettingValue(2012);

        /// <summary>
        /// 受診票
        /// </summary>
        public int YushinJiHyo => (int)GetSettingValue(91001);

        /// <summary>
        /// 1号紙
        /// </summary>
        public int Goshi1 => (int)GetSettingValue(91002);

        /// <summary>
        /// 2号紙
        /// </summary>
        public int Goshi2 => (int)GetSettingValue(91004);

        /// <summary>
        /// 病名一覧
        /// </summary>
        public int ByomeiIchiran => (int)GetSettingValue(91003);

        /// <summary>
        /// ネームラベル
        /// </summary>
        public int NameLableConfig { get => (int)GetSettingValue(91006, 0); }

        /// <summary>
        ///診察券
        /// </summary>
        public int ShinsatsuKen => (int)GetSettingValue(91005);

        public bool ShowShinsatsuKen => (int)GetSettingValue(91005) > 0;

        public int RenkeiSetting => (int)GetSettingValue(100016);

        /// <summary>
        /// 院内処方箋 92002
        /// </summary>
        public int InnaishohosenConf => (int)GetSettingValue(92002);
        public int InnaishohosenCheckMachine => (int)GetSettingValue(92002, 2);
        public string InnaishohosenCheckMachineParam => GetSettingParam(92002, 2, "KrtRenkei,TKImport");

        /// <summary>
        /// お薬手帳シール 92006
        /// </summary>
        public int OkusuritechoShiru => (int)GetSettingValue(92006);

        /// <summary>
        /// 薬袋ラベル 92005
        /// </summary>
        public int MinaiRaberu => (int)GetSettingValue(92005);
        /// <summary>
        /// 薬袋ラベル（単位表示）
        ///     0: オーダー単位
        ///     1: 換算単位
        /// </summary>
        public int YakutaiTaniDsp => (int)GetSettingValue(92005, 1);
        /// <summary>
        /// 薬袋ラベル（用紙サイズ）
        ///     0:服用量に応じて変更しない
        ///     1:服用量に応じて変更する
        /// </summary>
        public int YakutaiPaperSize => (int)GetSettingValue(92005, 2);
        /// <summary>
        /// 薬袋ラベル（1回量換算)
        ///     0:使用しない
        ///     1:使用する
        /// </summary>
        public int YakutaiOnceAmount => (int)GetSettingValue(92005, 3);
        /// <summary>
        /// 薬袋ラベル（印刷単位)
        ///     0:用法毎に印刷
        ///     1:Rp毎に印刷
        /// </summary>
        public int YakutaiPrintUnit => (int)GetSettingValue(92005, 4);
        /// <summary>
        /// 薬袋ラベル（内服／用紙小）
        /// </summary>
        public int YakutaiNaifukuPaperSmallMinValue => (int)GetSettingValue(92005, 11);
        /// <summary>
        /// 薬袋ラベル（内服／用紙小・プリンタ）
        /// </summary>
        public string YakutaiNaifukuPaperSmallPrinter => GetSettingParam(92005, 11);
        /// <summary>
        /// 薬袋ラベル（内服／用紙中・最小服用量）
        /// </summary>
        public int YakutaiNaifukuPaperNormalMinValue => (int)GetSettingValue(92005, 12);
        /// <summary>
        /// 薬袋ラベル（内服／用紙中・プリンタ）
        /// </summary>
        public string YakutaiNaifukuPaperNormalPrinter => GetSettingParam(92005, 12);
        /// <summary>
        /// 薬袋ラベル（内服／用紙大・最小服用量）
        /// </summary>
        public int YakutaiNaifukuPaperBigMinValue => (int)GetSettingValue(92005, 13);
        /// <summary>
        /// 薬袋ラベル（内服／用紙大・プリンタ）
        /// </summary>
        public string YakutaiNaifukuPaperBigPrinter => GetSettingParam(92005, 13);
        /// <summary>
        /// （頓服／用紙小）
        /// </summary>
        public int YakutaiTonpukuPaperSmallMinValue => (int)GetSettingValue(92005, 21);
        /// <summary>
        /// （頓服／用紙小・プリンタ）
        /// </summary>
        public string YakutaiTonpukuPaperSmallPrinter => GetSettingParam(92005, 21);
        /// <summary>
        /// （頓服／用紙中・最小服用量）
        /// </summary>
        public int YakutaiTonpukuPaperNormalMinValue => (int)GetSettingValue(92005, 22);
        /// <summary>
        /// （頓服／用紙中・プリンタ）
        /// </summary>
        public string YakutaiTonpukuPaperNormalPrinter => GetSettingParam(92005, 22);
        /// <summary>
        /// （頓服／用紙大・最小服用量）
        /// </summary>
        public int YakutaiTonpukuPaperBigMinValue => (int)GetSettingValue(92005, 23);
        /// <summary>
        /// （頓服／用紙大・プリンタ）
        /// </summary>
        public string YakutaiTonpukuPaperBigPrinter => GetSettingParam(92005, 23);
        /// <summary>
        /// （外用／用紙小）
        /// </summary>
        public int YakutaiGaiyoPaperSmallMinValue => (int)GetSettingValue(92005, 31);
        /// <summary>
        /// （外用／用紙小・プリンタ）
        /// </summary>
        public string YakutaiGaiyoPaperSmallPrinter => GetSettingParam(92005, 31);
        /// <summary>
        /// （外用／用紙中・最小服用量）
        /// </summary>
        public int YakutaiGaiyoPaperNormalMinValue => (int)GetSettingValue(92005, 32);
        /// <summary>
        /// （外用／用紙中・プリンタ）
        /// </summary>
        public string YakutaiGaiyoPaperNormalPrinter => GetSettingParam(92005, 32);
        /// <summary>
        /// （外用／用紙大・最小服用量）
        /// </summary>
        public int YakutaiGaiyoPaperBigMinValue => (int)GetSettingValue(92005, 33);
        /// <summary>
        /// （外用／用紙大・プリンタ）
        /// </summary>
        public string YakutaiGaiyoPaperBigPrinter => GetSettingParam(92005, 33);
        /// <summary>
        /// 薬袋ラベル用紙サイズ  
        /// </summary>
        public int YakutaiLabelSize => (int)GetSettingValue(92005, 2);
        /// <summary>
        /// 服用時点別一包化指示項目
        /// </summary>
        public string YakutaiFukuyojiIppokaItemCd => GetSettingParam(92005, 40);
        /// <summary>
        /// 薬情 92004
        /// </summary>
        public int Kusurijo => (int)GetSettingValue(92004);
        public int KusurijoCheckMachine => (int)GetSettingValue(92004, 16);
        public string KusurijoCheckMachineParam => GetSettingParam(92004, 16, "KrtRenkei,TKImport");

        /// <summary>
        /// 院外処方箋 92003
        /// </summary>
        public int IngaiShohosen => (int)GetSettingValue(92003);

        public int IngaiShohosenCheckMachine => (int)GetSettingValue(92003, 8);
        public string IngaiShohosenCheckMachineParam => GetSettingParam(92003, 8, "KrtRenkei,TKImport");

        /// <summary>
        /// カルテ２号紙 92007
        /// </summary>
        public int Karutegoshi => (int)GetSettingValue(92007);

        /// <summary>
        /// カルテ２号紙 初見
        /// </summary>
        public int KarutegoshiSyoken => (int)GetSettingValue(92007, 1);

        /// <summary>
        /// 指示箋 92008
        /// </summary>
        public int Instructions => (int)GetSettingValue(92008);

        /// <summary>
        /// オーダーラベル 92001
        /// </summary>
        public int OrderLabelEda0 => (int)GetSettingValue(92001, 0);
        public int OrderLabelEda1 => (int)GetSettingValue(92001, 1);
        public int OrderLabelCheckMachine => (int)GetSettingValue(92001, 9);

        public string OrderLabelCheckMachineParam => GetSettingParam(92001, 9, "KrtRenkei,TKImport");

        /// <summary>
        /// 検査ラベル
        /// </summary>
        public int KensaLabel => (int)GetSettingValue(92009, 0);

        public int KensaLabelCheckSinDay => (int)GetSettingValue(92009, 2);

        public int KensaLabelCheckInHospital => (int)GetSettingValue(92009, 1);

        public int LimitClient => (int)GetSettingValue(100018, defaultValue: 1);

        /// <summary>
        /// レセプト確認画面の総点数表示
        /// </summary>
        public int ReceiptListTensuCountDisplay { get => (int)GetSettingValue(6001, 0); }
        /// <summary>
        /// レセプト診療科検索
        /// 0-請求点数が最も高い診療科を検索する, 1-来院があった診療科を検索する
        /// </summary>
        public int ReceiptKaIdTarget { get => (int)GetSettingValue(6002, 0); }
        /// <summary>
        /// レセプト担当医検索
        /// 0-請求点数が最も高い担当医を検索する, 1-来院があった担当医を検索する
        /// </summary>
        public int ReceiptTantoIdTarget { get => (int)GetSettingValue(6002, 1); }
        /// <summary>
        /// チームカルテ連携　オーダー更新時にイベントを発生するかどうか
        /// </summary>
        public int TeamKarteRenkeiOdrEvent => (int)GetSettingValue(100021, 0);
        /// <summary>
        /// チームカルテ連携　所見更新時にイベントを発生するかどうか
        /// </summary>
        public int TeamKarteRenkeiKarteEvent => (int)GetSettingValue(100021, 1);
        /// <summary>
        /// チームカルテ連携　取込後状態
        /// </summary>
        public int TeamKarteRenkeiStatus => (int)GetSettingValue(100021, 2);
        /// <summary>
        /// チームカルテ　医療機関コード
        /// </summary>
        public int TeamKarteCd => (int)GetSettingValue(100021, 3);
        /// <summary>
        /// チームカルテ　名称
        /// </summary>
        public string TeamKarteName => GetSettingParam(100021, 3);
        /// <summary>
        /// チームカルテ　アップロード対象患者グループ
        /// グループID,グループコード
        /// </summary>
        public string TeamKarteTargetGrpId => GetSettingParam(100021, 4);
        /// <summary>
        /// チームカルテ　ファイル保存先
        /// </summary>
        public string TeamKarteFilePath => GetSettingParam(100021, 5);
        /// <summary>
        /// チームカルテ　訪問施設グループID
        /// </summary>
        public int TeamKarteSisetuGrpId => (int)GetSettingValue(100021, 6);
        /// <summary>
        /// チームカルテ　TK完了区分（来院区分）
        /// </summary>
        public int TeamKarteFinishRaiinKbn => (int)GetSettingValue(100021, 7);
        /// <summary>
        /// チームカルテ TK完了区分（来院区分　区分コード）
        /// </summary>
        public string TeamKarteFinishRaiinKbnCd => GetSettingParam(100021, 7);
        /// <summary>
        /// チームカルテ　検歴連携タイプ
        /// 0-Planet改, 1-Planet Next
        /// </summary>
        public int TeamKarteKenrekiType => (int)GetSettingValue(100021, 8);
        /// <summary>
        /// チームカルテ　検歴連携　最終連携日(yyyyMMdd)
        /// </summary>
        public string TeamKarteKenrekiLastDate => GetSettingParam(100021, 9);
        /// <summary>
        /// チームカルテ　検歴連携　連携時刻(HHmm)
        /// </summary>
        public int TeamKarteKenrekiTime => (int)GetSettingValue(100021, 9);
        /// <summary>
        /// チームカルテ　FTPサーバー
        /// </summary>
        public string TeamKarteFTPServer => GetSettingParam(100021, 10);
        /// <summary>
        /// チームカルテ　FTPユーザー名
        /// </summary>
        public string TeamKarteFTPUser => GetSettingParam(100021, 11);
        /// <summary>
        /// チームカルテ　FTPパスワード
        /// </summary>
        public string TeamKarteFTPPassword => GetSettingParam(100021, 12);
        /// <summary>
        /// チームカルテ　メールサーバー
        /// </summary>
        public string TeamKarteMailServer => GetSettingParam(100021, 13);
        /// <summary>
        /// チームカルテ　メールサーバーポート
        /// </summary>
        public int TeamKarteMailPort => (int)GetSettingValue(100021, 13);
        /// <summary>
        /// チームカルテ　メールユーザー名
        /// </summary>
        public string TeamKarteMailUser => GetSettingParam(100021, 14);
        /// <summary>
        /// チームカルテ　メールパスワード
        /// </summary>
        public string TeamKarteMailPassword => GetSettingParam(100021, 15);
        /// <summary>
        /// チームカルテ　検歴センターコード
        /// </summary>
        public string TeamKarteKenrekiCenterCd => GetSettingParam(100021, 16);
        /// <summary>
        /// チームカルテ　検歴　院内検査項目付加文字列
        /// </summary>
        public string TeamKarteKenrekiInnaiFuka => GetSettingParam(100021, 17);
        /// <summary>
        /// チームカルテ　検歴ホスト
        /// </summary>
        public string TeamKarteKenrekiHost => GetSettingParam(100021, 18);
        /// <summary>
        /// チームカルテ　検歴データベース
        /// </summary>
        public string TeamKarteKenrekiDB => GetSettingParam(100021, 19);
        /// <summary>
        /// チームカルテ　検歴ユーザー名
        /// </summary>
        public string TeamKarteKenrekiUser => GetSettingParam(100021, 20);
        /// <summary>
        /// チームカルテ　検歴パスワード
        /// </summary>
        public string TeamKarteKenrekiPassword => GetSettingParam(100021, 21);
        /// <summary>
        /// チームカルテ　来院コメント追加オプション　0-しない　1-する
        /// </summary>
        public int TeamKarteRaiinCommentAutoAdd => (int)GetSettingValue(100021, 22, 1);
        /// <summary>
        /// チームカルテ　来院番号自動取得
        /// </summary>
        public int TeamKarteRaiinNoAutoGet => (int)GetSettingValue(100021, 23, 1);
        /// <summary>
        /// チームカルテ　施設別コード（敬任会用）
        /// </summary>
        public string TeamKarteSisetubetuCd => GetSettingParam(100021, 24);
        /// <summary>
        /// チームカルテ　カルテ削除連携　0-する、1-しない
        /// </summary>
        public int TeamKarteKarteDelete => (int)GetSettingValue(100021, 26);

        // オーダー連携ファイル作成条件
        public int OdrRenkeiType => (int)GetSettingValue(100009, 1);

        // 所見連携ファイル作成条件
        public int KarteRenkeiType => (int)GetSettingValue(100009, 2);

        // 来院情報連携関連

        /// <summary>
        /// 来院情報連携　保険チェック　0-しない　1-する
        /// </summary>
        public int RaiinRenkeiHokenCheck => (int)GetSettingValue(100020, 1);
        /// <summary>
        /// 来院情報連携　保険チェック　<未登録時の来院区分設定値>,<期限切れのみ時の来院区分設定値>,<未確認時の来院区分設定値>
        /// </summary>
        public string RaiinRenkeiHokenCheckParam => GetSettingParam(100020, 1);
        /// <summary>
        /// 来院情報連携　新患登録時、自費レセ保険自動追加
        /// </summary>
        public int RaiinRenkeiAddJihiReceHoken => (int)GetSettingValue(100020, 2);
        /// <summary>
        /// 来院情報連携　受付種別変換設定　<適用開始時刻(HHmm),<適用終了時刻(HHmm),<受付区分コード>,<適用開始時刻(HHmm),<適用終了時刻(HHmm),<受付区分コード>･･･
        /// </summary>
        public string RaiinRenkeiUketukeSbtConv => GetSettingParam(100020, 3);
        /// <summary>
        /// プチWeb連携　医療機関コード
        /// </summary>
        public int PutiWebRenkeiCode => (int)GetSettingValue(100026, 0);
        /// <summary>
        /// プチWeb連携　キャンセル時新患削除
        /// 0-する、1-しない
        /// </summary>
        public int PutiWebRenkeiCancelPtDelete => (int)GetSettingValue(100026, 1);
        /// <summary>
        /// プチWeb利用可能期限連携　リハビリ設定　<リハ区分>,<コメント先頭文字列>,<リハ区分>,<コメント先頭文字列>,・・・
        /// </summary>
        public string PutiWebLimitRiha => GetSettingParam(100027, 0);
        /// <summary>
        /// プチWeb利用可能期限連携　汎用設定　<予約区分>=<診療行為コード>,<保険情報>,<診療行為コード>,<保険情報>,・・・[半角スペース]<予約区分>=<診療行為コード>,<保険情報>,<診療行為コード>,<保険情報>,・・・
        /// </summary>
        public string PutiWebLimitGeneral => GetSettingParam(100027, 1);
        /// <summary>
        /// プチWeb利用可能期限連携　最終送信日時（yyyyMMddHHmmss)
        /// </summary>
        public string PutiWebRenkeiLastUpdate => GetSettingParam(100027, 2, "", true);

        /// <summary>
        /// JunNavi連携ライセンス
        /// 0-なし　1-あり
        /// </summary>
        public int JunNaviRenkeiLicense => (int)GetSettingValue(100012, 0);
        /// <summary>
        /// JunNavi連携
        /// 0-しない　1-する　2-する（ファイル連携）
        /// </summary>
        public int JunNaviRenkeiType => (int)GetSettingValue(100012, 1);
        /// <summary>
        /// JunNavi連携　患者番号採番間隔
        /// </summary>
        public int JunNaviPtNumInterval => (int)GetSettingValue(100012, 2);
        /// <summary>
        /// JunNavi連携　患者番号独自採番　最大番号
        /// </summary>
        public string JunNaviPtNumMax => GetSettingParam(100012, 2);
        /// <summary>
        /// JunNavi連携　診療科コード変換設定
        /// JunNavi.診療科目コード=KA_MST.KA_ID,JunNavi.診療科目コード=KA_MST.KA_ID・・・
        /// </summary>
        public string JunNaviKaIdConvertConf => GetSettingParam(100012, 3);
        /// <summary>
        /// JunNavi連携　受付区分変換設定
        /// 0=受付区分,時間(HHMM)=受付区分,時間(HHMM)=受付区分・・・
        /// </summary>
        public string JunNaviUketukeKbnConvertConf => GetSettingParam(100012, 4);
        /// <summary>
        /// JunNavi連携　保留時来院区分
        /// </summary>
        public int JunNaviHoldRaiinKbn => (int)GetSettingValue(100012, 5);
        /// <summary>
        /// JunNavi連携　保留時来院区分設定値
        /// </summary>
        public string JunNaviHoldRaiinKbnCd => GetSettingParam(100012, 5);
        /// <summary>
        /// JunNavi連携　表示順桁数
        ///   0. JunNaviからの送信データの表示順を3桁として扱う											
        ///   1. JunNaviからの送信データの表示順を3桁として扱う 連番付与
        ///   2. JunNaviからの送信データの表示順を4桁として扱う
        /// </summary>
        public int JunNaviHyojiNoCount => (int)GetSettingValue(100012, 6);
        /// <summary>
        /// JunNavi連携　受付時来院区分
        /// </summary>
        public int JunNaviUketukeRaiinKbn => (int)GetSettingValue(100012, 7);
        /// <summary>
        /// JunNavi連携　受付時来院区分設定値
        /// </summary>
        public string JunNaviUketukeRaiinKbnCd => GetSettingParam(100012, 7);
        /// <summary>
        /// JunNavi連携　保険確認時来院区分
        /// </summary>
        public int JunNaviHokenCheckRaiinKbn => (int)GetSettingValue(100012, 8);
        /// <summary>
        /// JunNavi連携　保険確認時来院区分設定値
        /// 1カラム目-保険なし、2カラム目-有効保険なし、3カラム目-確認保険なし
        /// </summary>
        public string JunNaviHokenCheckRaiinKbnCd => GetSettingParam(100012, 8);
        /// <summary>
        /// JunNavi連携　来院コメントの前につける文字列
        /// </summary>
        public string JunNaviPresetRaiinCmt => GetSettingParam(100012, 9);
        /// <summary>
        /// JunNavi連携　JunNavi医療機関コード
        /// </summary>
        public int JunNaviHpCode => (int)GetSettingValue(100012, 10);
        /// <summary>
        /// JunNavi連携　JunNaviサーバーIPアドレス
        /// </summary>
        public string JunNaviIpAddress => GetSettingParam(100012, 10);
        /// <summary>
        /// JunNavi連携　JunNavi URL
        /// </summary>
        public string JunNaviURL => GetSettingParam(100012, 11);
        /// <summary>
        /// JunNavi連携　新患受付時来院区分
        /// </summary>
        public int JunNaviNewPtRaiinKbn => (int)GetSettingValue(100012, 12);
        /// <summary>
        /// JunNavi連携　新患受付時来院区分設定値
        /// </summary>
        public string JunNaviNewPtRaiinKbnCd => GetSettingParam(100012, 12);
        /// <summary>
        /// JunNavi連携　患者情報送信連携
        /// 0-なし 1-あり
        /// </summary>
        public int JunNaviSendPtInf => (int)GetSettingValue(100012, 13);
        /// <summary>
        /// JunNavi連携　診察中情報送信連携（カルテ）
        /// 0-なし 1-あり
        /// </summary>
        public int JunNaviSendSinsatuInfKarte => (int)GetSettingValue(100012, 14);
        /// <summary>
        /// JunNavi連携　診察中情報送信連携（保存）
        /// 0-なし 1-あり 2-あり(呼出中番号更新なし)
        /// </summary>
        public int JunNaviSendSinsatuInfHozon => (int)GetSettingValue(100012, 15);
        /// <summary>
        /// JunNavi連携　クリア連携
        /// 0-なし 1-あり
        /// </summary>
        public int JunNaviClearRenkei => (int)GetSettingValue(100012, 16);
        /// <summary>
        /// JunNavi連携　新患情報受診連携
        /// 0-なし 1-あり(電子カルテ採番） 2-あり(独自採番）
        /// </summary>
        public int JunNaviNewPtRenkei => (int)GetSettingValue(100012, 17);
        /// <summary>
        /// JunNavi連携　受付連携
        /// 0-なし 1-あり（予約取消あり） 2-あり（予約取消なし）
        /// </summary>
        public int JunNaviUketukeRenkei => (int)GetSettingValue(100012, 18);
        /// <summary>
        /// JunNavi連携　保留連携
        /// 0-なし 1-あり（受付順番変更あり） 2-あり（受付順番変更なし）
        /// </summary>
        public int JunNaviHoryuRenkei => (int)GetSettingValue(100012, 19);
        /// <summary>
        /// JunNavi連携　取消連携
        /// 0-なし 1-あり
        /// </summary>
        public int JunNaviTorikesiRenkei => (int)GetSettingValue(100012, 20);
        /// <summary>
        /// JunNavi連携　受付番号券発行枚数
        /// 0-なし >=1-あり、指定の数だけ発行する
        /// </summary>
        public int JunNaviPrintUketukeNoCard => (int)GetSettingValue(100012, 21);
        /// <summary>
        /// JunNavi連携　患者属性情報ファイル出力
        /// 0-なし 1-あり(新患除く) 2-あり(新患含む)
        /// </summary>
        public int JunNaviPutPtInfFile => (int)GetSettingValue(100012, 22);
        /// <summary>
        /// JunNavi連携　オプション印刷
        /// 0-なし 1-あり
        /// </summary>
        public int JunNaviOptionPrint => (int)GetSettingValue(100012, 23);
        /// <summary>
        /// JunNavi連携　受付時来院区分チェック
        /// 0-なし 1-あり
        /// </summary>
        public int JunNaviUketukeRaiinKbnCheck => (int)GetSettingValue(100012, 24);
        /// <summary>
        /// JunNavi連携　受付時受診票発行
        /// 0-なし 1-あり（予約オーダーありのみ） 2-あり（常時）
        /// </summary>
        public int JunNaviPrintJyusinHyo => (int)GetSettingValue(100012, 25);
        /// <summary>
        /// JunNavi連携　1-状態が済みでない来院と同一来院に設定する
        /// </summary>
        public int JunNaviDouituraiin => (int)GetSettingValue(100012, 26);
        /// <summary>
        /// JunNavi連携　受付順番採番方式　0-JunNavi表示順番を使用　1-電子カルテで採番
        /// </summary>
        public int JunNaviUketukeSaiban => (int)GetSettingValue(100012, 27);
        /// <summary>
        /// JunNavi連携　診察科目名　JunNavi.診療科目コード=《名称》,(以下繰り返し)
        /// </summary>
        public string JunNaviSinsatuName => GetSettingParam(100012, 28);
        /// <summary>
        /// JunNavi連携　受付時刻　0-JunNaviの受付時間を使用する　1-チェックイン時間を使用する
        /// </summary>
        public int JunNaviUketukeTime => (int)GetSettingValue(100012, 29);
        /// <summary>
        /// JunNavi連携　JunNavi URLタイプ　0-通常　1-ラクーン
        /// </summary>
        public int JunNaviURLType => (int)GetSettingValue(100012, 30);
        /// <summary>
        /// JunNavi連携　新患時保留取消受信オプション
        /// JunNaviで新患時即連携を設定しているユーザーのためのオプション
        /// 0-なし 1-あり
        /// </summary>
        public int JunNaviNewPtSoku => (int)GetSettingValue(100012, 31);
        /// <summary>
        /// JunNavi連携　新患時保留取消受信オプション利用開始日(yyyyMMdd)
        /// </summary>
        public string JunNaviNewPtSokuStart => GetSettingParam(100012, 31);
        /// <summary>
        /// プチWeb2 診察済み連携				
        /// 保存時連携
        /// </summary>
        public int HozonJiRenkei => (int)GetSettingValue(100025, 0);
        /// <summary>
        /// クリアボタン連携
        /// </summary>
        public int ClearBtnRenkei => (int)GetSettingValue(100025, 1);

        public int CreateRenkei => (int)GetSettingValue(100025, 2);
        /// <summary>
        /// 文書コメントの処方にコメントを含めるかどうか
        /// </summary>
        public int IsShowCommentInDoc => (int)GetSettingValue(8001);

        /// <summary>
        /// 文書編集後の動作
        /// </summary>
        public int OperationAfterEditDoc => (int)GetSettingValue(8002);

        public int ModeFocusEditDocument => (int)GetSettingValue(8002, 1);

        /// <summary>
        /// 文書コメントリストEnterキー押下時動作
        ///    0: 選択項目を改行付きで文書に挿入
        ///    1: 選択項目を改行なしで文書に挿入
        /// </summary>
        public int DocKeyEnterForInsertText => (int)GetSettingValue(8004);

        public int ConfirmSaveEditedDocument => (int)GetSettingValue(8005);

        public int PrintWordDocumentMode => (int)GetSettingValue(8006, 0);

        public int PrintExcelDocumentMode => (int)GetSettingValue(8006, 1);

        /// <summary>
        /// オーダー時検査依頼連携ライセンス
        /// </summary>
        public int OdrKensaIraiLicense => (int)GetSettingValue(100019, 0);
        /// <summary>
        /// オーダー時検査依頼連携対象期間
        /// </summary>
        public string OdrKensaIraiTargetTerm => GetSettingParam(100019, 1);

        /// <summary>
        /// オーダー時検査依頼連携センターコード
        /// </summary>
        public string OdrKensaIraiCenterCd => GetSettingParam(100019, 2);

        /// <summary>
        /// 保存ボタン押下時に連携
        /// </summary> 
        public int KaikeiSaveKensaIrai => (int)GetSettingValue(100019, 3);

        /// <summary>
        /// 計算保存ボタン押下時に連携
        /// </summary>
        public int KeisanSaveKensaIrai => (int)GetSettingValue(100019, 4);

        /// <summary>
        /// 一時保存ボタン押下時に連携
        /// </summary>
        public int TempSaveKensaIrai => (int)GetSettingValue(100019, 5);

        /// <summary>
        /// 帳票ボタン押下時に連携
        /// </summary>
        public int PrintBtnKensaIrai => (int)GetSettingValue(100019, 6);
        /// <summary>
        /// オーダー時検査依頼連携　ファイルレイアウト
        /// 0-標準
        /// 1-加古川
        /// </summary>
        public int OdrKensaIraiFileType => (int)GetSettingValue(100019, 7);
        /// <summary>
        /// オーダー時検査依頼連携　科コード
        /// 0 - 未使用, 1 - 指定グループの指定の値の場合、値の名称をセット(PT_GRP_ITEM.GRP_CODE_NAME)
        /// </summary>
        public int OdrKensaIraiKaCode => (int)GetSettingValue(100019, 8);
        /// <summary>
        /// オーダー時検査依頼連携　科コード パラメータ
        /// <グループID>=<グループコード>,<グループID>=<グループコード>・・・
        /// </summary>
        public string OdrkensaIraiKaCodeParam => GetSettingParam(100019, 8);
        /// <summary>
        /// Planet バイタル送信モード（テンプレート）
        /// 0: 検歴プログラム連携設定に従う
        /// 1: Planet改
        /// 2: PlanetNexxt
        /// </summary>
        public int PlanetVitalTemplateType => (int)GetSettingValue(100036, 0);
        /// <summary>
        /// Planet バイタル送信モード（身体情報）
        /// 0: 検歴プログラム連携設定に従う
        /// 1: Planet改
        /// 2: PlanetNexxt
        /// </summary>
        public int PlanetVitalPhysicalType => (int)GetSettingValue(100024, 0);
        /// <summary>
        /// Planet 種類
        /// 0: 連携なし
        /// 1: Planet 改
        /// 2: Planet Next
        /// </summary>
        public int PlanetType => (int)GetSettingValue(8010, 0);
        /// <summary>
        /// Planete 送信先 ホスト
        /// </summary>
        public string PlanetHostName => GetSettingParam(100022, 0);
        /// <summary>
        /// Planet 送信先 データベース
        /// </summary>
        public string PlanetDatabase => GetSettingParam(100022, 1);
        /// <summary>
        /// Planet 送信先 ユーザー名
        /// </summary>
        public string PlanetUserName => GetSettingParam(100022, 2);
        /// <summary>
        /// Planet 送信先 パスワード
        /// </summary>
        public string PlanetPassword => GetSettingParam(100022, 3);
        /// <summary>
        /// Planet センターコード
        /// </summary>
        public string PlanetCenterCode => GetSettingParam(100022, 4);
        /// <summary>
        /// Planet バイタル用送信先 ホスト（テンプレート）
        /// </summary>
        public string PlanetVitalTemplateHostName => GetSettingParam(100036, 1);
        /// <summary>
        /// Planet バイタル用送信先 データベース（テンプレート）
        /// </summary>
        public string PlanetVitalTemplateDatabase => GetSettingParam(100036, 2);
        /// <summary>
        /// Planet バイタル用送信先 ユーザー名（テンプレート）
        /// </summary>
        public string PlanetVitalTemplateUserName => GetSettingParam(100036, 3);
        /// <summary>
        /// Planet バイタル用送信先 パスワード（テンプレート）
        /// </summary>
        public string PlanetVitalTemplatePassword => GetSettingParam(100036, 4);
        /// <summary>
        /// Planet バイタル用 センターコード（テンプレート）
        /// </summary>
        public string PlanetVitalTemplateCenterCode => GetSettingParam(100036, 5);
        /// <summary>
        /// Planet バイタル用送信先 ホスト（身体情報）
        /// </summary>
        public string PlanetVitalPhysicalHostName => GetSettingParam(100024, 1);
        /// <summary>
        /// Planet バイタル用送信先 データベース（身体情報）
        /// </summary>
        public string PlanetVitalPhysicalDatabase => GetSettingParam(100024, 2);
        /// <summary>
        /// Planet バイタル用送信先 ユーザー名（身体情報）
        /// </summary>
        public string PlanetVitalPhysicalUserName => GetSettingParam(100024, 3);
        /// <summary>
        /// Planet バイタル用送信先 パスワード（身体情報）
        /// </summary>
        public string PlanetVitalPhysicalPassword => GetSettingParam(100024, 4);
        /// <summary>
        /// Planet バイタル用 センターコード（身体情報）
        /// </summary>
        public string PlanetVitalPhysicalCenterCode => GetSettingParam(100024, 5);
        /// <summary>
        /// 労災ライセンス
        /// 0: なし
        /// 1: あり
        /// </summary>
        public int RousaiLicense => (int)GetSettingValue(100001, 0);

        /// <summary>
        /// 自賠ライセンス
        /// 0: なし
        /// 1: あり
        /// </summary>
        public int JibaiLicense => (int)GetSettingValue(100002, 0);

        /// <summary>
        /// 労災レセプト電算ライセンス
        /// 1 - 労災レセプト電算を使用する
        /// </summary>
        public int RousaiRecedenLicense => (int)GetSettingValue(100003, 0);
        /// <summary>
        /// 労災レセプト電算開始年月
        /// </summary>
        public string RousaiRecedenStartYm => GetSettingParam(100003, 0);
        /// <summary>
        /// アフターケアレセプト電算ライセンス
        /// 1 - 労災レセプト電算を使用する
        /// </summary>
        public int AfterCareRecedenLicense => (int)GetSettingValue(100003, 1);
        /// <summary>
        /// アフターケアレセプト電算開始年月
        /// </summary>
        public string AfterCareRecedenStartYm => GetSettingParam(100003, 1);
        /// <summary>
        /// 初診時の病名転帰日
        /// </summary>
        public double FirstVisitTenkiDate => GetSettingValue(1012);

        /// <summary>
        /// 経過日数
        /// </summary>
        public string NumDaysFromLastVisit => GetSettingParam(1012);

        /// <summary>
        /// 受付時年齢チェック
        /// </summary>
        public int CheckAgeReception => (int)GetSettingValue(1005);

        /// <summary>
        /// 受付時年齢チェック・パラメーター
        /// </summary>
        public string CheckAgeParam => GetSettingParam(1005);
        /// <summary>
        /// 保険種の取り扱い（算定回数、背反等）
        /// 0 - 健保、労災、自賠を同一に考える
        /// 1 - すべて同一に考える
        /// 2 - すべて別に考える
        /// </summary>
        public int HokensyuHandling => (int)GetSettingValue(3013);
        /// <summary>
        /// 診療名さ取得時に同一Rpをまとめるかどうか
        /// 0-まとめる
        /// 1-まとめない
        /// </summary>
        public int SameRpMerge => (int)GetSettingValue(3014);
        /// <summary>
        /// オーダー取得来院数
        /// </summary>
        public int OrderVisitShows => (int)GetSettingValue(8003);

        /// <summary>
        /// プチ予約連携ライセンス
        /// </summary>
        public bool IsShowReservationsSetting => (int)GetSettingValue(100014) == 1;

        /// <summary>
        /// プチ予約連携ライセンス
        /// </summary>
        public string PetitReservationParam => GetSettingParam(100014);

        public double CheckZaiganIsoPatient => GetSettingValue(2028);
        /// <summary>
        /// 領収証
        /// </summary>
        public int PrintReceipt => (int)GetSettingValue(93001, 0);

        /// <summary>
        /// 請求額0円領収証
        /// </summary>
        public int PrintReceipt0Yen => (int)GetSettingValue(93001, 3);

        /// <summary>
        /// 入金額0円領収証
        /// </summary>
        public int PrintReceiptPay0Yen => (int)GetSettingValue(93001, 4);

        /// <summary>
        /// 明細書
        /// </summary>
        public int PrintDetail => (int)GetSettingValue(93002, 0);

        /// <summary>
        /// 請求額0円明細書
        /// </summary>
        public int PrintDetail0Yen => (int)GetSettingValue(93002, 3);

        /// <summary>
        /// 入金額0円明細書
        /// </summary>
        public int PrintDetailPay0Yen => (int)GetSettingValue(93002, 6);

        /// <summary>
        /// 院外処方箋
        /// </summary>
        public int PrintOutDrg => (int)GetSettingValue(92003, 0);

        /// <summary>
        /// 薬剤情報提供書
        /// </summary>
        public int PrintDrgInf => (int)GetSettingValue(92004, 0);

        /// <summary>
        /// 薬袋ラベル
        /// </summary>
        public int PrintDrgLabel => (int)GetSettingValue(92005, 0);

        public int PrintDrgLabelCheckMachine => (int)GetSettingValue(92005, 50);
        public string PrintDrgLabelCheckMachineParam => GetSettingParam(92005, 50, "KrtRenkei,TKImport");

        /// <summary>
        /// お薬手帳シール
        /// </summary>
        public int PrintDrgNote => (int)GetSettingValue(92006, 0);

        public int PrintDrgNoteCheckMachine => (int)GetSettingValue(92006, 1);
        public string PrintDrgNoteCheckMachineParam => GetSettingParam(92006, 1, "KrtRenkei,TKImport");

        /// <summary>
        /// コニカIP
        /// </summary>
        public string KonikaIPSetting => GetSettingParam(100013, 1);

        /// <summary>
        /// コニカUser
        /// </summary>
        public string KonikaUserSetting => GetSettingParam(100013, 2);

        /// <summary>
        /// コニカPassword
        /// </summary>
        public string KonikaPasswordSetting => GetSettingParam(100013, 3);

        /// <summary>
        /// コニカモダリティ
        /// </summary>
        public string KonikaModality => GetSettingParam(100013, 4);

        /// 家族登録確認
        /// </summary>
        public int ConfirmFamilyRegistration => (int)GetSettingValue(1002, 0);

        /// <summary>
        /// 妊娠週のカルテ入力
        /// </summary>
        public int AutoInsertPregnancy => (int)GetSettingValue(2003, 0);

        public int KensaKekkaValue => (int)GetSettingValue(8010, 0);

        public string KensaKekkaParam => GetSettingParam(8010, 0);

        public int ShohoRekiValue => (int)GetSettingValue(2009, 0);

        public int KaojashinValue => (int)GetSettingValue(2006, 0);

        public int PaxisLicense => (int)GetSettingValue(100004, 0, 0, true);

        public int BackupPDFValue => (int)GetSettingValue(8020, 0);

        public int BackupPDFDirMode => (int)GetSettingValue(8020, 1);

        public int StandbyServerValue => (int)GetSettingValue(8030, 0);

        public int PtSearchModel => (int)GetSettingValue(5005, 0);

        public string StandbyServerParam => GetSettingParam(8030, 0);

        /// <summary>
        /// カルテ作成時帳票印刷
        /// </summary>
        public bool IsEnablePrintInReception => (int)GetSettingValue(1011, 0) == 1;

        /// <summary>
        /// 来院一覧のメニューボタン
        /// </summary>
        public bool VisibleRowDetailsVisitingList => GetSettingValue(5001, 0, 1) == 1;

        public bool IsCheckDrugInformation => (int)GetSettingValue(100029, 0) == 1 && (int)GetSettingValue(100029, 99) == 1;

        public int DrugInformationChecked => (int)GetSettingValue(100037, 1);
        public int NumberOfMonthsBefore => (int)GetSettingValue(100037, 2);
        public int MedicalInformationChecked => (int)GetSettingValue(100037, 3);
        public int SpecificMedicalChecked => (int)GetSettingValue(100037, 4);

        public int RenkeiYoyakuValue => (int)GetSettingValue(2004, 0);

        public string RenkeiYoyakuParam => GetSettingParam(2004, 0);

        public int RenkeiTemplateValue => (int)GetSettingValue(2005, 0, fromLastestDb: false);

        public string RenkeiTemplateParam => GetSettingParam(2005, 0);

        public int IsEnableScanInBooking => (int)GetSettingValue(99001, 0);

        /// <summary>
        /// 限度額適用認定証提供
        /// </summary>
        public bool VisibleLimitConsFlg => (int)GetSettingValue(100029, 0) == 1;

        /// <summary>
        /// FtpTransferManager
        /// </summary>
        public string FtpFileLocation => @GetSettingParam(100021, 5);
        public string FtpServerName => GetSettingParam(100021, 10, defaultParam: "karte.sakura.ne.jp");
        public string FtpUserName => GetSettingParam(100021, 11, defaultParam: "ftptkr@karte.sakura.ne.jp");
        public string FtpPasswords => GetSettingParam(100021, 12, defaultParam: "315f6ffffb");
        public string FtpDestination => GetSettingParam(100021, 3);
        public int RousaiKufuValidate => (int)GetSettingValue(1006, 0);

        /// <summary>
        /// レセチェック一覧、診療科検索の設定
        /// </summary>
        public int ReceiptListKaIdSearch => (int)GetSettingValue(6002, 0, 0);

        /// <summary>
        /// レセチェック一覧、診療科検索の設定
        /// </summary>
        public int ReceiptListTantoIdSearch => (int)GetSettingValue(6002, 1, 0);

        /// <summary>
        /// オンライン資格確認
        /// VAL in (0, 1)以外はライセンスなし
        /// </summary>
        public int OnlineQualificationLicense => (int)GetSettingValue(100029, 0, 0);

        /// <summary>
        /// 基本情報チェック-氏名
        /// </summary>
        public int NameBasicInfoCheck => (int)GetSettingValue(100029, 1, 1);

        /// <summary>
        /// 基本情報チェック-氏名カナ
        /// </summary>
        public int KanaNameBasicInfoCheck => (int)GetSettingValue(100029, 2, 1);

        /// <summary>
        /// 基本情報チェック-性別
        /// </summary>
        public int GenderBasicInfoCheck => (int)GetSettingValue(100029, 3, 1);

        /// <summary>
        /// 基本情報チェック-生年月日
        /// </summary>
        public int BirthDayBasicInfoCheck => (int)GetSettingValue(100029, 4, 1);

        /// <summary>
        /// 基本情報チェック-住所
        /// </summary>
        public int AddressBasicInfoCheck => (int)GetSettingValue(100029, 5, 1);

        /// <summary>
        /// 基本情報チェック-郵便番号
        /// </summary>
        public int PostcodeBasicInfoCheck => (int)GetSettingValue(100029, 6, 1);

        /// <summary>
        /// 基本情報チェック-世帯主
        /// </summary>
        public int SeitaiNushiBasicInfoCheck => (int)GetSettingValue(100029, 7, 1);

        /// <summary>
        /// アフターケア電算 0-なし、1-あり
        /// </summary>
        public int AftercareDensanLicense { get => (int)GetSettingValue(100003, 1); }

        /// <summary>
        /// アフターケア電算 開始年月(yyyyMM)
        /// </summary>
        public string AftercareDensanTerm { get => GetSettingParam(100003, 1); }

        /// <summary>
        /// 至急
        /// </summary>
        public bool IsShowUrgentDetail { get => GetSettingValue(2029, 0) == 1; }
        /// <summary>
        /// 請求情報連携（窓口）　入金額0連携
        /// 0-しない、1-する
        /// </summary>
        public int SeikyuRenkeiMadoguchi { get => (int)GetSettingValue(100031, 0); }
        /// <summary>
        /// 請求情報連携（収納）　入金額0連携
        /// 0-しない、1-する
        /// </summary>
        public int SeikyuRenkeiSyuno { get => (int)GetSettingValue(100031, 1); }
        /// <summary>
        /// 請求情報連携　来院番号が異なる場合
        /// 0-同一ファイルに出力、1-別ファイルに出力
        /// </summary>
        public int SeikyuRenkeiDiffRaiinNo { get => (int)GetSettingValue(100031, 2); }
        /// <summary>
        /// アットリンク　クライアントID
        /// </summary>
        public string AtLinkClientId { get => GetSettingParam(100032, 0); }

        public bool IsEnableRaiinView { get => (int)GetSettingValue(100030, 0) == 1; }

        // 注意事項に記載する内容
        /// <summary>
        /// 年齢条件かつ性別条件のあるもの
        /// </summary>
        public bool IsPrecautionQueryAgeSex { get => (int)GetSettingValue(92004, 3) == 1; }
        /// <summary>
        /// （年齢条件のみがあるもの）
        /// </summary>
        public bool IsPrecautionQueryAgeNoSex { get => (int)GetSettingValue(92004, 4) == 1; }
        /// <summary>
        /// （性別条件のみがあるもの）
        /// </summary>
        public bool IsPrecautionQuerySexNoAge { get => (int)GetSettingValue(92004, 5) == 1; }
        /// <summary>
        /// （年齢及び性別の条件がないもの）
        /// </summary>
        public bool IsPrecautionQueryNoAgeNoSex { get => (int)GetSettingValue(92004, 6) == 1; }
        /// <summary>
        /// （服用方法、使用方法）
        /// </summary>
        public bool IsPrecautionQueryPropertyCd1 { get => (int)GetSettingValue(92004, 7) == 1; }
        /// <summary>
        /// （飲食物および嗜好品の指示、指導、注意）
        /// </summary>
        public bool IsPrecautionQueryPropertyCd2 { get => (int)GetSettingValue(92004, 8) == 1; }
        /// <summary>
        /// （生活上の指示、指導、注意）
        /// </summary>
        public bool IsPrecautionQueryPropertyCd3 { get => (int)GetSettingValue(92004, 9) == 1; }
        /// <summary>
        /// （相互作用に関する注意）
        /// </summary>
        public bool IsPrecautionQueryPropertyCd4 { get => (int)GetSettingValue(92004, 10) == 1; }
        /// <summary>
        /// （既往症･体質、治療中の疾患）
        /// </summary>
        public bool IsPrecautionQueryPropertyCd5 { get => (int)GetSettingValue(92004, 11) == 1; }
        /// <summary>
        /// （妊娠および授乳）
        /// </summary>
        public bool IsPrecautionQueryPropertyCd6 { get => (int)GetSettingValue(92004, 12) == 1; }
        /// <summary>
        /// （保管方法）
        /// </summary>
        public bool IsPrecautionQueryPropertyCd7 { get => (int)GetSettingValue(92004, 13) == 1; }
        /// <summary>
        /// （臨床検査）
        /// </summary>
        public bool IsPrecautionQueryPropertyCd8 { get => (int)GetSettingValue(92004, 14) == 1; }
        /// <summary>
        /// （薬の作用に関わる注意）
        /// </summary>
        public bool IsPrecautionQueryPropertyCd9 { get => (int)GetSettingValue(92004, 15) == 1; }

        /// <summary>
        /// 日計表の検索条件「入金時間」
        /// </summary>
        public bool IsShowPaymentTimeDailyStatistic { get => (int)GetSettingValue(95001, 0, 0) == 1; }

        public bool IsAutoEnableVisitingDataGrid { get => (int)GetSettingValue(5004, 0) == 1; }

        public bool IsCaptureScreenEnable { get => (int)GetSettingValue(2031, 0) == 1; }

        public bool IsCaptureScreenWithByomeiEnabled { get => (int)GetSettingValue(2032, 0) == 1; }

        /// <summary>
        /// ユーザー名
        /// </summary>
        public string CloudUserName { get => GetSettingParam(8031, 0) + "Emr"; }

        public string CloudPassword { get => "cbk#Emr" + GetSettingParam(8031, 0); }

        public int OdrItemNameDisplay { get => (int)GetSettingValue(100017, 4); }

        public int OdrJikarDate { get => (int)GetSettingValue(100017, 1); }

        public int OdrJikarCondition { get => (int)GetSettingValue(100017, 3); }

        public int HokenJikarCondition { get => (int)GetSettingValue(100017, 13); }

        public int ByomeiJikarCondition { get => (int)GetSettingValue(100017, 12); }

        public int WebIdLicense => (int)GetSettingValue(100017, 0, 0);

        /// <summary>
        /// 医療機関コード
        /// </summary>
        public string MedicalInstitutionCode => GetSettingParam(100017, 0, "");

        /// <summary>
        /// QRコード
        /// </summary>
        public string WebIdQrCode => GetSettingParam(100017, 15, "");

        /// <summary>
        /// PC用URL
        /// </summary>
        public string WebIdUrlForPc => GetSettingParam(100017, 16, "");

        /// <summary>
        /// Ip cloud WebId
        /// </summary>
        public string IpCloudWebId => GetSettingParam(100017, 18, "");

        /// <summary>
        /// UserId for WebId
        /// </summary>
        public string UserIdForWebId => GetSettingParam(8031, 0, "");

        public int CloudId => (int)GetSettingValue(8031, 0);

        public int WebIdPrintAutoChecked => (int)GetSettingValue(100017, 14, 0);

        public int ShowOdr => (int)GetSettingValue(100017, 2);

        public int ShowKarteS => (int)GetSettingValue(100017, 5);

        public int ShowKarteO => (int)GetSettingValue(100017, 6);

        public int ShowKarteA => (int)GetSettingValue(100017, 7);

        public int ShowKarteP => (int)GetSettingValue(100017, 8);

        public int ShowKarteF => (int)GetSettingValue(100017, 9);

        public int ShowKensa => (int)GetSettingValue(100017, 10);

        public int ShowByomei => (int)GetSettingValue(100017, 11);

        public bool IsCheckPatientFullName => (int)GetSettingValue(1017, 0, 0) == 0;

        /// <summary>
        /// 登美ヶ丘モール連携
        /// 1:連携する
        /// </summary>
        public int MallRenkei => (int)GetSettingValue(100034, 0);
        /// <summary>
        /// 登美ヶ丘モール連携　総合受付ポート
        /// </summary>
        public int MallRenkeiSogoPort => (int)GetSettingValue(100034, 1);
        /// <summary>
        /// 登美ヶ丘モール連携　総合受付ホスト
        /// </summary>
        public string MallRenkeiSogoHost => GetSettingParam(100034, 1);
        /// <summary>
        /// 登美ヶ丘モール連携　受付モード
        /// 0:常に新規来院 1:予約優先
        /// </summary>
        public int MallRenkeiAlwaysNewRaiin => (int)GetSettingValue(100034, 4);

        /// <summary>
        /// チームカルテ連携　取込後状態
        /// 0:一時保存, 1:計算, 2:精算待ち
        /// </summary>
        public int KarteRenkeiStatus => (int)GetSettingValue(100035, 0);

        /// <summary>
        /// 精算画面の入金額の既定値に前回未収分を含むかどうかをオプション化してください。（今回分だけを精算したい）
        /// 0:含む
        /// 1:含まない
        /// </summary>
        public bool AccountingNotPaymentDebitBalance => (int)GetSettingValue(3020, 0, 0) == 1;

        public bool VisibleBuiOrderCheck => (int)GetSettingValue(6003) == 1;

        public string AccountingTelegramHost => GetSettingParam(100034, 2);

        public int AccountingTelegramPort => (int)GetSettingValue(100034, 2);

        public int DefaultClinicCode => (int)GetSettingValue(100034, 3);

        public int AutoOdrItemFromHistory => (int)GetSettingValue(100020, 4);

        public string AutoOdrItemFromHistoryParam => GetSettingParam(100020, 4);

        public bool IsShowPregnancyInf => GetSettingValue(2003, 1) == 1;

        public SystemConf CreateNewSystemConf(int grpCd, int grpEdaNo = 0, int value = 0, string param = "")
        {
            SystemConf systemConf = new SystemConf();
            systemConf.HpId = TempIdentity.HpId;
            systemConf.GrpCd = grpCd;
            systemConf.GrpEdaNo = grpEdaNo;
            systemConf.Val = value;
            systemConf.Param = param;

            return systemConf;
        }
    }
}
