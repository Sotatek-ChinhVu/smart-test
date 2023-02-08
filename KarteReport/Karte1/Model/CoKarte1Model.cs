using Helper.Common;
using Helper.Constants;
using System.Text.Json.Serialization;

namespace KarteReport.Karte1.Model
{
    public class CoKarte1Model
    {
        CoPtInfModel PtInfModel { get; } = null;
        public List<CoPtByomeiModel> ListByomeis { get; } = null;

        CoPtHokenInfModel PtHokenInfModel { get; } = null;

        public CoKarte1Model(CoPtInfModel ptInfModel, List<CoPtByomeiModel> listByomeis, CoPtHokenInfModel ptHokenInfModel)
        {
            PtInfModel = ptInfModel;
            ListByomeis = listByomeis;
            PtHokenInfModel = ptHokenInfModel;
        }

        /// <summary>
        /// 患者番号
        /// </summary>
        [JsonPropertyName("PtNum")]
        public long PtNum
        {
            get => PtInfModel.PtNum;
        }
        /// <summary>
        /// 患者ID
        /// </summary>
        public long PtId
        {
            get => PtInfModel.PtId;
        }
        /// <summary>
        /// 患者氏名
        /// </summary>
        [JsonPropertyName("PtName")]
        public string PtName
        {
            get => PtInfModel.Name;
        }
        /// <summary>
        /// 患者カナ氏名
        /// </summary>
        [JsonPropertyName("PtKanaName")]
        public string PtKanaName
        {
            get => PtInfModel.KanaName;
        }
        /// <summary>
        /// 性別
        /// </summary>
        [JsonPropertyName("SexRaw")]
        public int Sex
        {
            get => PtInfModel.Sex;
        }
        /// <summary>
        /// 患者性別（男、女）
        /// </summary>
        [JsonPropertyName("Sex")]
        public string PtSex
        {
            get => PtInfModel.PtSex;
        }

        /// <summary>
        /// 生年月日
        /// </summary>
        public int BirthDay
        {
            get => PtInfModel.Birthday;
        }
        /// <summary>
        /// 年齢
        /// </summary>
        [JsonPropertyName("Age")]
        public int Age
        {
            get => PtInfModel.Age;
        }
        /// <summary>
        /// 郵便番号
        /// </summary>
        public string PtPostCd
        {
            get => PtInfModel.HomePost ?? "";
        }

        [JsonPropertyName("PtPostCode")]
        public string PtPostCdDsp
        {
            get
            {
                return GetPostCd(PtInfModel.HomePost);
            }
        }

        private string GetPostCd(string postcd)
        {
            string ret = postcd ?? "";

            if (ret.Length > 5 && ret.Contains("-") == false)
            {
                ret = $"{ret.Substring(0, 3)}-{ret.Substring(3, ret.Length - 3)}";
            }
            return ret;
        }
        /// <summary>
        /// 患者住所
        /// </summary>
        [JsonPropertyName("PtAddress")]
        public string PtHomeAddress
        {
            get => PtInfModel.HomeAddress1 + PtInfModel.HomeAddress2;
        }
        /// <summary>
        /// 患者住所１
        /// </summary>
        [JsonPropertyName("PtAddress1")]
        public string PtHomeAddress1
        {
            get => PtInfModel.HomeAddress1;
        }
        /// <summary>
        /// 患者住所２
        /// </summary>
        [JsonPropertyName("PtAddress2")]
        public string PtHomeAddress2
        {
            get => PtInfModel.HomeAddress2;
        }
        /// <summary>
        /// 患者電話番号
        /// </summary>
        [JsonPropertyName("PtTel")]
        public string PtTel
        {
            get
            {
                string ret = "";

                if (PtInfModel.Tel1 != "")
                {
                    ret = PtInfModel.Tel1;
                }
                else if (PtInfModel.Tel2 != "")
                {
                    ret = PtInfModel.Tel2;
                }
                else if (PtInfModel.RenrakuTel != "")
                {
                    ret = PtInfModel.RenrakuTel;
                }

                return ret;
            }
        }
        /// <summary>
        /// 患者電話番号１
        /// </summary>
        [JsonPropertyName("PtTel1")]
        public string PtTel1
        {
            get => PtInfModel.Tel1;
        }
        /// <summary>
        /// 患者電話番号２
        /// </summary>
        [JsonPropertyName("PtTel2")]
        public string PtTel2
        {
            get => PtInfModel.Tel2;
        }
        /// <summary>
        /// 患者連絡先電話番号
        /// </summary>
        [JsonPropertyName("PtRenrakuTel")]
        public string PtRenrakuTel
        {
            get => PtInfModel.RenrakuTel;
        }
        /// <summary>
        /// 世帯主
        /// </summary>
        [JsonPropertyName("SetaiNusi")]
        public string SetaiNusi
        {
            get => PtInfModel.Setanusi;
        }
        /// <summary>
        /// 本人家族
        /// </summary>
        public int HonkeKbn
        {
            get => PtHokenInfModel?.HonkeKbn ?? 0;
        }
        /// <summary>
        /// 記号
        /// </summary>
        public string Kigo
        {
            get => PtHokenInfModel?.Kigo ?? "";
        }
        /// <summary>
        /// 番号
        /// </summary>
        public string Bango
        {
            get => PtHokenInfModel?.Bango ?? "";
        }
        /// <summary>
        /// 枝番
        /// </summary>
        public string EdaNo
        {
            get => PtHokenInfModel?.EdaNo ?? "";
        }
        /// <summary>
        /// 記号番号
        /// </summary>
        [JsonPropertyName("KigoBango")]
        public string KigoBango
        {
            get => PtHokenInfModel?.KigoBango ?? "";
        }
        /// <summary>
        /// 保険有効期限
        /// </summary>
        public int HokenEndDate
        {
            get => PtHokenInfModel?.HokenEndDate ?? 0;
        }
        /// <summary>
        /// 保険取得日
        /// </summary>
        public int HokenSyutokuDate
        {
            get => PtHokenInfModel?.SikakuDate ?? 0;
        }
        /// <summary>
        /// 保険者番号
        /// </summary>
        [JsonPropertyName("HokensyaNo")]
        public string HokensyaNo
        {
            get => PtHokenInfModel?.HokensyaNo ?? "";
        }
        /// <summary>
        /// 保険の種類
        /// </summary>
        public string HokenSbt
        {
            get => PtHokenInfModel?.HokenSbt ?? "";
        }
        /// <summary>
        /// 患者負担率（割合）
        /// </summary>
        //public int FutanRate
        //{
        //    get
        //    {
        //        return (PtHokenInfModel?.Rate ?? 0) / 10;
        //    }
        //}
        public int? FutanRate
        {
            get
            {
                //int? ret = null;
                int? ret = 100; // PT_HOKEN=null && PT_KOHI=null の場合、自費算定の処理と考える

                if (PtHokenInfModel != null)
                {
                    if (new int[] { 1, 2 }.Contains(PtHokenInfModel.HokenKbn))
                    {

                        ret = GetHokenRate(PtHokenInfModel.Rate, PtHokenInfModel.HokenSbtKbn, PtHokenInfModel.KogakuKbn, PtHokenInfModel.Houbetu);

                    }
                    else if (PtHokenInfModel.HokenKbn == 0)
                    {
                        // 自費
                        ret = PtHokenInfModel.Rate;
                    }
                    else
                    {
                        // 労災・自賠
                        ret = PtHokenInfModel.Rate;
                    }

                    if (PtHokenInfModel.PtKohis != null)
                    {
                        for (int i = 0; i < PtHokenInfModel.PtKohis.Count(); i++)
                        {
                            if (PtHokenInfModel.PtKohis[i].HokenMst.FutanKbn == 0)
                            {
                                ret = 0;
                            }
                            else if (PtHokenInfModel.PtKohis[i].FutanRate > 0 && (ret == null || ret > PtHokenInfModel.PtKohis[i].FutanRate))
                            {
                                ret = PtHokenInfModel.PtKohis[i].FutanRate;
                            }
                        }
                    }
                }
                return ret / 10;
            }
        }

        /// <summary>
        /// 主保険負担率計算
        /// </summary>
        /// <param name="futanRate">負担率</param>
        /// <param name="hokenSbtKbn">保険種別区分</param>
        /// <param name="kogakuKbn">高額療養費区分</param>
        /// <param name="honkeKbn">本人家族区分</param>
        /// <param name="houbetu">法別番号</param>
        /// <param name="receSbt">レセプト種別</param>
        /// <returns></returns>
        public int GetHokenRate(int futanRate, int hokenSbtKbn, int kogakuKbn, string houbetu)
        {
            int wrkRate = futanRate;

            switch (hokenSbtKbn)
            {
                case 0:
                    //主保険なし
                    break;
                case 1:
                    //主保険
                    if (PtInfModel.IsPreSchool())
                    {
                        //６歳未満未就学児
                        wrkRate = 20;
                    }
                    else if (PtInfModel.IsElder() && houbetu != "39")
                    {
                        wrkRate =
                            PtInfModel.IsElder20per() ? wrkRate = 20 :  //前期高齢
                            PtInfModel.IsElderExpat() ? wrkRate = 20 :  //75歳以上海外居住者
                            wrkRate = 10;
                    }

                    if (PtInfModel.IsElder() || houbetu == "39")
                    {
                        if ((kogakuKbn == 3 && PtInfModel.SinDate < KaiseiDate.d20180801) ||
                            (new int[] { 26, 27, 28 }.Contains(kogakuKbn) && PtInfModel.SinDate >= KaiseiDate.d20180801))
                        {
                            //後期７割 or 高齢７割
                            wrkRate = 30;
                        }
                        else if (houbetu == "39" && kogakuKbn == 41 &&
                            PtInfModel.SinDate >= KaiseiDate.d20221001)
                        {
                            //後期８割
                            wrkRate = 20;
                        }
                    }
                    break;
                default:
                    break;
            }

            return wrkRate;
        }
        /// <summary>
        /// 保険給付率（割合）
        /// </summary>
        public int KyufuRate
        {
            get
            {
                if (PtHokenInfModel == null) return 0;
                return (100 - PtHokenInfModel.Rate) / 10;
            }
        }
        /// <summary>
        /// 公費ID
        /// </summary>
        /// <param name="index">取得したい公費のインデックス</param>
        /// <returns></returns>
        public int KohiId(int index)
        {
            return PtHokenInfModel?.KohiId(index) ?? 0;
        }
        /// <summary>
        /// 公費負担者番号
        /// </summary>
        /// <param name="index">取得したい公費のインデックス</param>
        /// <returns></returns>
        public string KohiFutansyaNo(int index)
        {
            return PtHokenInfModel?.KohiFutansyaNo(index)?? string.Empty;

        }
        /// <summary>
        /// 公費受給者番号
        /// </summary>
        /// <param name="index">取得したい公費のインデックス</param>
        /// <returns></returns>
        public string KohiJyukyusyaNo(int index)
        {
            return PtHokenInfModel?.KohiJyukyusyaNo(index) ?? string.Empty;

        }
        /// <summary>
        /// 公費有効期限
        /// </summary>
        /// <param name="index">取得したい公費のインデックス</param>
        /// <returns></returns>
        public int KohiStartDate(int index)
        {
            return PtHokenInfModel?.KohiStartDate(index)?? 0;

        }
        /// <summary>
        /// 公費有効期限
        /// </summary>
        /// <param name="index">取得したい公費のインデックス</param>
        /// <returns></returns>
        public int KohiEndDate(int index)
        {
            return PtHokenInfModel?.KohiEndDate(index)??0;

        }
        /// <summary>
        /// 公費資格取得日
        /// </summary>
        /// <param name="index">取得したい公費のインデックス</param>
        /// <returns></returns>
        public int KohiSikakuDate(int index)
        {
            return PtHokenInfModel?.KohiSikakuDate(index)??0;

        }
        /// <summary>
        /// 公費交付日日
        /// </summary>
        /// <param name="index">取得したい公費のインデックス</param>
        /// <returns></returns>
        public int KohiKofuDate(int index)
        {
            return PtHokenInfModel?.KohiKofuDate(index)??0;

        }
        /// <summary>
        /// 勤務先
        /// </summary>
        [JsonPropertyName("Office")]
        public string OfficeName
        {
            get => PtInfModel?.OfficeName ?? "";
        }
        /// <summary>
        /// 勤務先所在地
        /// </summary>
        [JsonPropertyName("OfficeAddress")]
        public string OfficeAddress
        {
            get => PtInfModel?.OfficeAddress1 ?? "" + PtInfModel?.OfficeAddress2 ?? "";
        }
        /// <summary>
        /// 勤務先電話番号
        /// </summary>
        [JsonPropertyName("OfficeTel")]
        public string OfficeTel
        {
            get => PtInfModel?.OfficeTel ?? "";
        }
        /// <summary>
        /// 勤務先郵便番号
        /// </summary>
        public string OfficePostCd
        {
            get => PtInfModel?.OfficePost ?? "";
        }
        public string OfficePostCdDsp
        {
            get
            {
                return GetPostCd(OfficePostCd);
            }
        }
        /// <summary>
        /// 勤務先備考
        /// </summary>
        public string OfficeMemo
        {
            get => PtInfModel?.OfficeMemo ?? "";
        }
        /// <summary>
        /// 事業所名
        /// </summary>
        public string RousaiJigyosyo
        {
            get => PtHokenInfModel?.RousaiJigyosyoName ?? "";
        }
        /// <summary>
        /// 事業所所在地
        /// </summary>
        public string RousaiJigyosyoAddress
        {
            get => PtHokenInfModel?.RousaiPrefName ?? "" + PtHokenInfModel?.RousaiCityName ?? "";
        }
        /// <summary>
        /// 保険者名
        /// </summary>
        [JsonPropertyName("HokensyaName")]
        public string HokensyaName
        {
            get => PtHokenInfModel?.HokensyaName ?? "";
        }
        /// <summary>
        /// 保険者電話番号
        /// </summary>
        [JsonPropertyName("HokensyaTel")]
        public string HokensyaTel
        {
            get => PtHokenInfModel?.HokensyaTel ?? "";
        }
        /// <summary>
        /// 保険者所在地
        /// </summary>
        [JsonPropertyName("HokensyaAddress")]
        public string HokensyaAddress
        {
            get => PtHokenInfModel?.HokensyaAddress ?? "";
        }
        /// <summary>
        /// メール
        /// </summary>
        public string Mail
        {
            get => PtInfModel.Mail;
        }
        /// <summary>
        /// 続柄
        /// </summary>
        [JsonPropertyName("Zokugara")]
        public string Zokugara
        {
            get => PtInfModel.Zokugara;
        }
        /// <summary>
        /// 職業
        /// </summary>
        [JsonPropertyName("Job")]
        public string Job
        {
            get => PtInfModel.Job;
        }

        public string PtMemoText
        {
            get => PtInfModel.PtMemoText;
        }
        public List<string> PtMemoList
        {
            get => PtInfModel.PtMemoList;
        }

        public string PtCmtText
        {
            get => PtInfModel.PtCmtText;
        }
        public List<string> PtCmtList
        {
            get => PtInfModel.PtCmtList;
        }

        [JsonPropertyName("SysDateTimeS")]
        public string SysDateTimeS
        {
            get => DateTime.Now.ToString("yyyy/MM/dd HH:mm");
        }

        [JsonPropertyName("FutansyaNo_K1")]
        public string FutansyaNo_K1
        {
            get => KohiFutansyaNo(1);
        }

        [JsonPropertyName("JyukyusyaNo_K1")]
        public string JyukyusyaNo_K1
        {
            get => KohiJyukyusyaNo(1);
        }

        [JsonPropertyName("FutansyaNo_K2")]
        public string FutansyaNo_K2
        {
            get => KohiFutansyaNo(2);
        }

        [JsonPropertyName("JyukyusyaNo_K2")]
        public string JyukyusyaNo_K2
        {
            get => KohiJyukyusyaNo(2);
        }

        [JsonPropertyName("HokenKigenW")]
        public string HokenKigenW
        {
            get => CIUtil.SDateToShowWDate3(HokenEndDate).Ymd;
        }

        [JsonPropertyName("BirthDateW")]
        public string BirthDateW
        {
            get => CIUtil.SDateToShowWDate3(BirthDay).Ymd;
        }

        [JsonPropertyName("HokenSyutokuW")]
        public string HokenSyutokuW
        {
            get => CIUtil.SDateToShowWDate3(HokenSyutokuDate).Ymd;
        }
    }
}
