using Domain.Models.SystemConf;
using Helper.Common;
using Reporting.Calculate.Constants;
using Reporting.Calculate.Receipt.Constants;
using Reporting.Calculate.Receipt.Models;
using Reporting.Mappers.Common;
using Reporting.Receipt.Constants;
using Reporting.Receipt.DB;
using Reporting.Receipt.Models;
using static Helper.Common.CIUtil;

namespace Reporting.Receipt.Mapper
{
    public class ReceiptPreviewMapper : CommonReportingRequest
    {
        private readonly CoReceiptModel _coReceipt;
        private readonly ISystemConfRepository _systemConfRepository;
        private readonly ICoReceiptFinder _coReceiptFinder;
        List<CoReceiptByomeiModel> ByomeiModels;
        List<CoReceiptTekiyoModel> TekiyoModels;
        List<CoReceiptTekiyoModel> TekiyoEnModels;
        protected int CurrentPage = 1;
        private int HpId;
        private int Target;
        private int _tekiyoRowCount;
        private int _tekiyoEnRowCount;
        private int _tekiyoRowCount2;

        public ReceiptPreviewMapper(CoReceiptModel coReceipt, List<CoReceiptByomeiModel> byomeiModels, List<CoReceiptTekiyoModel> tekiyoModels, List<CoReceiptTekiyoModel> tekiyoEnModels, int currentPage, int hpId, int target, ISystemConfRepository systemConfRepository, ICoReceiptFinder coReceiptFinder, int tekiyoRowCount, int tekiyoEnRowCount, int tekiyoRowCount2)
        {
            _coReceipt = coReceipt;
            ByomeiModels = byomeiModels;
            TekiyoModels = tekiyoModels;
            TekiyoEnModels = tekiyoEnModels;
            CurrentPage = currentPage;
            HpId = hpId;
            Target = target;
            _systemConfRepository = systemConfRepository;
            _coReceiptFinder = coReceiptFinder;
            _tekiyoRowCount = tekiyoRowCount;
            _tekiyoEnRowCount = tekiyoEnRowCount;
            _tekiyoRowCount2 = tekiyoRowCount2;
            UpdateDrawForm();
        }

        Dictionary<string, string> SingleData = new Dictionary<string, string>();
        List<Dictionary<string, CellModel>> CellData = new List<Dictionary<string, CellModel>>();

        public override Dictionary<string, string> GetFileNamePageMap()
        {
            var fileName = new Dictionary<string, string>();
            fileName.Add("1", "fmReceipt.rse");
            return fileName;
        }

        public override int GetReportType()
        {
            return (int)CoReportType.Receipt;
        }

        public override Dictionary<string, string> GetSingleFieldData()
        {
            return SingleData;
        }

        public override string GetRowCountFieldName()
        {
            return "lsTekiyo";
        }

        public override Dictionary<string, bool> GetVisibleFieldData()
        {
            return new Dictionary<string, bool>();
        }

        public override Dictionary<string, bool> GetWrapFieldData()
        {
            return new Dictionary<string, bool>();
        }

        public override List<Dictionary<string, CellModel>> GetTableFieldData()
        {
            return CellData;
        }

        public void UpdateDrawForm()
        {
            #region SubMethod

            // ヘッダーの印刷処理

            if (TargetIsKenpo())
            {
                // 健保
                PrintReceiptHeaderForKenpo();
            }
            else if (new int[] { TargetConst.RousaiTanki, TargetConst.RousaiNenkin }.Contains(Target))
            {
                // 労災（短期・傷病年金）
                PrintReceiptHeaderForRousai();
            }
            else if (new int[] { TargetConst.RousaiAfter }.Contains(Target))
            {
                // アフターケア
                PrintReceiptHeaderForAfter();
            }
            else if (new int[] { TargetConst.Jibai }.Contains(Target))
            {
                if ((int)_systemConfRepository.GetSettingValue(3001, 0, HpId) == 0)
                {
                    // 自賠健保準拠
                    PrintReceiptHeaderForJibaiKenpo();
                }
                else
                {
                    // 自賠労災準拠
                    PrintReceiptHeaderForJibaiRousai();
                }
            }

            // 本体部分印刷処理

            if (_tekiyoRowCount <= 0) return;
            if ((TekiyoModels?.Count() ?? 0) <= 0 && (CurrentPage > 1 || (TekiyoEnModels?.Count() ?? 0) <= 0)) return;

            int tekiyoIndex = (CurrentPage - 1) * _tekiyoRowCount;

            //摘要欄印刷
            if (new int[] { TargetConst.RousaiTanki, TargetConst.RousaiNenkin, TargetConst.RousaiAfter, TargetConst.Jibai }.Contains(Target) && CurrentPage > 1)
            {
                // 労災続紙
                tekiyoIndex = _tekiyoRowCount + (CurrentPage - 2) * (_tekiyoRowCount2 * 2);

                for (int i = 1; i <= 2; i++)
                {
                    for (short j = 0; j < _tekiyoRowCount2; j++)
                    {
                        if (TekiyoModels != null && tekiyoIndex < TekiyoModels.Count())
                        {
                            var data = new Dictionary<string, CellModel>();

                            data.Add($"lsSinId{i}", new CellModel(TekiyoModels[tekiyoIndex].SinId));
                            data.Add($"lsTekiyoMark{i}", new CellModel(TekiyoModels[tekiyoIndex].Mark));
                            data.Add($"lsTekiyo{i}", new CellModel(TekiyoModels[tekiyoIndex].Tekiyo));

                            CellData.Add(data);

                            tekiyoIndex++;
                            if (tekiyoIndex >= TekiyoModels.Count())
                            {
                                break;
                            }
                        }
                    }
                }

            }
            else
            {
                if (TekiyoModels != null && TekiyoModels.Any())
                {
                    for (short i = 0; i < _tekiyoRowCount; i++)
                    {
                        if (tekiyoIndex < TekiyoModels.Count())
                        {
                            var data = new Dictionary<string, CellModel>();

                            data.Add("lsSinId", new CellModel(TekiyoModels[tekiyoIndex].SinId));
                            data.Add("lsTekiyoMark", new CellModel(TekiyoModels[tekiyoIndex].Mark));
                            data.Add("lsTekiyo", new CellModel(TekiyoModels[tekiyoIndex].Tekiyo));

                            CellData.Add(data);

                            tekiyoIndex++;
                            if (tekiyoIndex >= TekiyoModels.Count())
                            {
                                break;
                            }
                        }
                    }
                }
            }

            if ((new int[] { TargetConst.RousaiTanki, TargetConst.RousaiNenkin }.Contains(Target) || (Target == TargetConst.Jibai && (int)_systemConfRepository.GetSettingValue(3001, 0, HpId) == 1)) && CurrentPage == 1)
            {
                // 労災 円項目用　本紙のみ
                int tekiyoEnIndex = (CurrentPage - 1) * _tekiyoEnRowCount;

                if (TekiyoEnModels != null && tekiyoEnIndex < TekiyoEnModels.Count())
                {
                    for (short i = 0; i < _tekiyoEnRowCount; i++)
                    {
                        var data = new Dictionary<string, CellModel>();
                        data.Add("lsEnTekiyo", new CellModel(TekiyoEnModels[tekiyoEnIndex].Tekiyo));

                        CellData.Add(data);

                        tekiyoEnIndex++;
                        if (tekiyoEnIndex >= TekiyoEnModels.Count())
                        {
                            //_hasNextPage = false;
                            break;
                        }
                    }
                }
            }

            // 長野県福岡県レセプト2枚目の場合、2ページ目以降は印字しない
            if (new int[] {
                    TargetConst.NaganoRece2,
                    TargetConst.FukuokaRece2 }.Contains(Target))
            {
                tekiyoIndex = -1;
            }



            #endregion

        }

        private bool TargetIsKenpo()
        {
            return new int[]
                {
                    TargetConst.Syaho,
                    TargetConst.Kokuho,
                    TargetConst.Kenpo,
                    TargetConst.Jihi,
                    TargetConst.IwateRece2,
                    TargetConst.KanagawaRece2,
                    TargetConst.NaganoRece2,
                    TargetConst.OsakaSyouni,
                    TargetConst.FukuokaRece2,
                    TargetConst.SagaRece2
                }.Contains(Target);
        }

        private void PrintReceiptHeaderForKenpo()
        {
            // 公費マスタを取得しておく（各種特殊処理で使用）
            List<KohiDataModel> kohiDatas = _coReceiptFinder.FindKohiData(HpId, _coReceipt.PtId, _coReceipt.SinYm * 100 + 1);

            #region Sub Function
            // 患者番号+法別番号
            string _getPtNo()
            {
                string ret = _coReceipt.PtNum.ToString();

                string houbetu = "";

                for (int i = 1; i <= 4; i++)
                {
                    if (_coReceipt.ReceKisai(i) == 0 &&
                        _coReceipt.KohiHoubetuReceInf(i) != "" &&
                        CIUtil.StrToIntDef(_coReceipt.KohiHoubetuReceInf(i), 999) <= 99)
                    {
                        if (houbetu != "")
                        {
                            houbetu += ",";
                        }
                        houbetu += _coReceipt.KohiHoubetuReceInf(i);
                    }
                }

                if (houbetu != "")
                {
                    ret += "(" + houbetu + ")";
                }

                return ret;
            }

            // 社保国保
            string _getSyaKoku()
            {
                string ret = "1 社";

                if (_coReceipt.HokenKbn == 2)
                {
                    ret = "2 国";
                }
                else if (_coReceipt.HokenKbn == 0)
                {
                    ret = "0 自";
                }

                return ret;
            }

            // 診療年月
            string _getSinYM()
            {
                string ret = "";

                int wDate = CIUtil.SDateToWDate(_coReceipt.SinYm * 100 + 1);
                int gengo = wDate / 1000000;

                switch (gengo)
                {
                    case 1:
                        ret = "明治　";
                        break;
                    case 2:
                        ret = "大正　";
                        break;
                    case 3:
                        ret = "昭和　";
                        break;
                    case 4:
                        ret = "平成　";
                        break;
                    case 5:
                        ret = "令和　";
                        break;
                }

                ret += string.Format("{0, 2}年{1, 2}月", wDate % 1000000 / 10000, wDate % 10000 / 100);

                return ret;
            }

            // レセ種別１
            string _getReceSbt1()
            {
                string ret = "";

                if (_coReceipt.HokenKbn == 0)
                {
                    ret = "0 自費";
                }
                else
                {
                    switch (CIUtil.Copy(_coReceipt.ReceiptSbt, 2, 1))
                    {
                        case "1":
                            if (_coReceipt.HokenKbn == 1)
                            {
                                ret = "1 社";
                            }
                            else
                            {
                                ret = "1 国";
                            }
                            break;
                        case "2":
                            ret = "2 公費";
                            break;
                        case "3":
                            ret = "3 後期";
                            break;
                        case "4":
                            ret = "4 退職";
                            break;
                    }
                }
                return ret;
            }

            // レセ種別２
            string _getReceSbt2()
            {
                string ret = "";

                switch (CIUtil.Copy(_coReceipt.ReceiptSbt, 3, 1))
                {
                    case "1":
                        ret = "1 単独";
                        break;
                    case "2":
                        ret = "2 ２併";
                        break;
                    case "3":
                    case "4":
                    case "5":
                        ret = "3 ３併";
                        break;
                }
                return ret;
            }

            // レセ種別３
            string _getReceSbt3()
            {
                string ret = "";

                switch (CIUtil.Copy(_coReceipt.ReceiptSbt, 4, 1))
                {
                    case "2":
                        ret = "2 本外";
                        break;
                    case "4":
                        ret = "4 六外";
                        break;
                    case "6":
                        ret = "6 家外";
                        break;
                    case "8":
                        ret = "8 高外一";
                        break;
                    case "0":
                        ret = "0 高外７";
                        break;
                    case "x":
                        if (_coReceipt.HokensyaNo != null && _coReceipt.HokensyaNo.StartsWith("97"))
                        {
                            if (_coReceipt.HonkeKbn == 1)
                            {
                                ret = "2 本外";
                            }
                            else if (_coReceipt.HonkeKbn == 2)
                            {
                                ret = "6 家外";
                            }

                            if (!(_coReceipt.IsStudent))
                            {
                                ret = "4 六外";
                            }
                            else if (_coReceipt.IsElder)
                            {
                                ret = "8 高外一";
                            }
                        }
                        break;
                }
                return ret;
            }

            // 性別
            string _getSex()
            {
                string ret = "1 男";
                if (_coReceipt.Sex == 2)
                {
                    ret = "2 女";
                }
                return ret;
            }

            // 生年月日
            string _getBirthDay()
            {
                string ret = "";

                int wDate = CIUtil.SDateToWDate(_coReceipt.BirthDay);
                int gengo = wDate / 1000000;

                switch (gengo)
                {
                    case 1:
                        ret = "1明";
                        break;
                    case 2:
                        ret = "2大";
                        break;
                    case 3:
                        ret = "3昭";
                        break;
                    case 4:
                        ret = "4平";
                        break;
                    case 5:
                        ret = "5令";
                        break;
                }

                ret += string.Format("{0, 2}.{1, 2}.{2, 2}", wDate % 1000000 / 10000, wDate % 10000 / 100, wDate % 100);

                return ret;
            }
            // 職務上の事由
            string _getSyokumuJiyu()
            {
                string ret = "";

                switch (_coReceipt.SyokumuKbn)
                {
                    case 1:
                        ret = "1 職務上";
                        break;
                    case 2:
                        ret = "2 下船後３月以内";
                        break;
                    case 3:
                        ret = "3 通勤災害";
                        break;
                }

                return ret;
            }
            //公費給付額（かっこ書き）
            string _getKohiKyufu(int index)
            {
                string ret = CIUtil.ToStringIgnoreNull(_coReceipt.KohiKyufu(index));

                if (ret != "")
                {
                    ret = "(" + string.Format("{0:N0}", CIUtil.StrToIntDef(ret, 0)) + ")";
                }

                return ret;
            }
            //国保減免
            string _getGenmenKbn()
            {
                string ret = "";

                switch (_coReceipt.GenmenKbn)
                {
                    case 1:
                        if (_coReceipt.GenmenGaku > 0)
                        {
                            ret = "減額 " + _coReceipt.GenmenGaku.ToString() + "円";
                        }
                        else
                        {
                            ret = "減額 " + (_coReceipt.GenmenRate / 10).ToString() + "割";
                        }
                        break;
                    case 2:
                        ret = "免除";
                        break;
                    case 3:
                        ret = "支払猶予";
                        break;
                }

                return ret;
            }

            //チェック
            string _getCheck(string str)
            {
                int[] baisu = new int[] { 2, 3, 4, 5, 6, 7 };
                int index = 0;
                int total = 0;

                for (int i = str.Length - 1; i >= 0; i--)
                {
                    total += CIUtil.StrToIntDef(str[i].ToString(), 0) * baisu[index];
                    index++;
                    if (index >= baisu.Length)
                    {
                        index = 0;
                    }
                }
                total = total % 11;

                if (total <= 1)
                {
                    total = 0;
                }
                else
                {
                    total = 11 - total;
                }

                return total.ToString();
            }
            // 数字のみ抽出
            string _getNums(string str)
            {
                List<string> nums = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                string narrow = CIUtil.ToNarrow(str);
                string ret = "";
                for (int i = 1; i <= narrow.Length; i++)
                {
                    if (nums.Contains(CIUtil.Copy(narrow, i, 1)))
                    {
                        ret += CIUtil.Copy(narrow, i, 1);
                    }
                }
                return ret;
            }
            //OCR1
            string _getOCR1()
            {
                string ret = "";

                // 保険者番号
                string hokensyaNo = _coReceipt.HokensyaNo.PadLeft(8, '0');
                //医療機関コード
                string hpCd = _coReceipt.HpCd.PadLeft(7, '0');
                //請求点数
                string tensu = _coReceipt.HokenReceTensu?.ToString().PadLeft(7, '0') ?? string.Empty;
                //チェック1
                string chk1 = _getCheck(tensu);
                //生月日
                string birthDay = (_coReceipt.BirthDay % 10000).ToString().PadLeft(5, '0');
                //チェック2
                string chk2 = _getCheck(birthDay);
                //一部負担金
                string futan = _coReceipt.HokenReceFutan?.ToString().PadLeft(5, '0') ?? string.Empty;
                //チェック3
                string chk3 = _getCheck(futan);
                //チェック4
                string chk4 = _getCheck(hokensyaNo + hpCd + tensu + chk1 + birthDay + chk2 + futan + chk3);
                //実日数
                string nissu = _coReceipt.HokenNissu?.ToString().PadLeft(2, '0') ?? string.Empty;
                //診療年月
                string sinYm = (CIUtil.SDateToWDate(_coReceipt.SinYm * 100 + 1) % 1000000 / 100).ToString().PadLeft(4, '0');
                //チェック5
                string chk5 = _getCheck(nissu + sinYm);
                //市町村番号
                string sicyo = new string('0', 8);
                //受給者番号
                string jyukyu = new string('0', 7);
                //チェック6
                string chk6 = "0";
                //都道府県番号
                string prefNo = _coReceipt.PrefNo.ToString().PadLeft(2, '0');
                //点数表
                string hyo = "1";
                //保険種別１
                string receSbt1 = CIUtil.Copy(_coReceipt.ReceiptSbt, 2, 1);
                //保険種別２
                string receSbt2 = CIUtil.Copy(_coReceipt.ReceiptSbt, 3, 1);
                //本人家族
                string honka = CIUtil.Copy(_coReceipt.ReceiptSbt, 4, 1);
                //整理番号
                string seiriNo = "1";
                //チェック7
                string chk7 = _getCheck(prefNo + hyo + receSbt1 + receSbt2 + honka + seiriNo);

                ret = hokensyaNo + hpCd + tensu + chk1 + birthDay + chk2 + futan + chk3 + chk4 + nissu + sinYm + chk5 + sicyo + jyukyu + chk6 + prefNo + hyo + receSbt1 + receSbt2 + honka + seiriNo + chk7;

                return ret;
            }
            //OCR2
            string _getOCR2()
            {
                string ret = "";

                string retA = "";
                string retB = "";

                //性別
                string sex = _coReceipt.Sex.ToString().PadLeft(1, '0');
                //元号
                string gengo = $"{CIUtil.SDateToWDate(_coReceipt.BirthDay) / 1000000,0}";
                //生年
                string birthY = (CIUtil.SDateToWDate(_coReceipt.BirthDay) % 1000000 / 10000).ToString().PadLeft(2, '0');
                //チェック1
                string chk1 = _getCheck(sex + gengo + birthY);
                //記号
                string kigo = _getNums(_coReceipt.Kigo).PadLeft(10, '0');
                //番号
                string bango = _getNums(_coReceipt.Bango).PadLeft(10, '0');
                //チェック2
                string chk2 = _getCheck(kigo + bango);

                retA = sex + gengo + birthY + chk1 + kigo + bango + chk2;

                if (CIUtil.Copy(_coReceipt.ReceiptSbt, 2, 1) == "2" || CIUtil.Copy(_coReceipt.ReceiptSbt, 3, 1) != "1")
                {
                    //公１負担者番号
                    string k1FutanNo = _coReceipt.KohiFutansyaNo(1).PadLeft(8, '0');
                    //公１受給者番号
                    string k1JyukyuNo = _coReceipt.KohiJyukyusyaNo(1).PadLeft(7, '0');
                    //チェック3
                    string chk3 = _getCheck(k1FutanNo + k1JyukyuNo);
                    //公１実日数
                    string k1Nissu = _coReceipt.KohiNissu(1)?.ToString().PadLeft(2, '0') ?? string.Empty;
                    //公１請求点数
                    string k1Tensu = _coReceipt.KohiReceTensu(1)?.ToString().PadLeft(7, '0') ?? string.Empty;
                    //チェック4
                    string chk4 = _getCheck(k1Nissu + k1Tensu);
                    //公１薬剤一部負担金
                    string k1Yaku = new string('0', 5);
                    //チェック5
                    string chk5 = "0";
                    //公１患者負担額
                    string k1Futan = _coReceipt.KohiReceFutan(1)?.ToString().PadLeft(5, '0') ?? string.Empty;
                    //チェック6
                    string chk6 = _getCheck(k1Futan);
                    //チェック7
                    string chk7 = _getCheck(k1FutanNo + k1JyukyuNo + chk3 + k1Nissu + k1Tensu + chk4 + k1Yaku + chk5 + k1Futan + chk6);
                    //予備
                    string yobi = new string(' ', 2);

                    retB = k1FutanNo + k1JyukyuNo + chk3 + k1Nissu + k1Tensu + chk4 + k1Yaku + chk5 + k1Futan + chk6 + chk7 + yobi;
                }
                ret = retA + retB;

                return ret;
            }
            //OCR3
            string _getOCR3()
            {
                string ret = "";

                if ((CIUtil.Copy(_coReceipt.ReceiptSbt, 2, 1) == "2" && CIUtil.StrToIntDef(CIUtil.Copy(_coReceipt.ReceiptSbt, 3, 1), 0) >= 2) ||
                     (CIUtil.StrToIntDef(CIUtil.Copy(_coReceipt.ReceiptSbt, 3, 1), 0) >= 3))
                {
                    //公２負担者番号
                    string k2FutanNo = _coReceipt.KohiFutansyaNo(2).PadLeft(8, '0');
                    //公２受給者番号
                    string k2JyukyuNo = _coReceipt.KohiJyukyusyaNo(2).PadLeft(7, '0');
                    //チェック1
                    string chk1 = _getCheck(k2FutanNo + k2JyukyuNo);
                    //公２実日数
                    string k2Nissu = _coReceipt.KohiNissu(2)?.ToString().PadLeft(2, '0') ?? string.Empty;
                    //公２請求点数
                    string k2Tensu = _coReceipt.KohiReceTensu(2)?.ToString().PadLeft(7, '0') ?? string.Empty;
                    //チェック2
                    string chk2 = _getCheck(k2Nissu + k2Tensu);
                    //公２薬剤一部負担金
                    string k2Yaku = new string('0', 5);
                    //チェック3
                    string chk3 = "0";
                    //公２患者負担額
                    string k2Futan = $"{_coReceipt.KohiReceFutan(2),00000}";
                    //チェック4
                    string chk4 = _getCheck(k2Futan);
                    //チェック5
                    string chk5 = _getCheck(k2FutanNo + k2JyukyuNo + chk1 + k2Nissu + k2Tensu + chk2 + k2Yaku + chk3 + k2Futan + chk4);
                    //予備
                    string yobi = new string(' ', 28);

                    ret = k2FutanNo + k2JyukyuNo + chk1 + k2Nissu + k2Tensu + chk2 + k2Yaku + chk3 + k2Futan + chk4 + chk5 + yobi;
                }

                return ret;
            }

            #endregion

            #region sub print
            // 実日数
            void _printoutJituNissu()
            {
                //実日数（保険）
                SingleData.Add("dfJituNissuHo", CIUtil.ToStringIgnoreNull(_coReceipt.HokenNissu));
                //実日数（公１）
                if ((_coReceipt.HokenNissu == null && _coReceipt.KohiNissu(1) != null) ||
                    (_coReceipt.HokenNissu ?? 0) != (_coReceipt.KohiNissu(1) ?? 0))
                {
                    SingleData.Add("dfJituNissuK1", CIUtil.ToStringIgnoreNull(_coReceipt.KohiNissu(1)));
                }
                //実日数（公２）
                if ((_coReceipt.KohiNissu(1) == null && _coReceipt.KohiNissu(2) != null) ||
                    (_coReceipt.KohiNissu(1) ?? 0) != (_coReceipt.KohiNissu(2) ?? 0))
                {
                    SingleData.Add("dfJituNissuK2", CIUtil.ToStringIgnoreNull(_coReceipt.KohiNissu(2)));
                }
            }

            // 特記事項
            void _printoutTokki()
            {
                List<string> tokki = _coReceipt.TokkiJiko;

                SingleData.Add("dfTokki1", $"{tokki[0]} {tokki[1]}");
                SingleData.Add("dfTokki2", $"{tokki[2]} {tokki[3]}");
                SingleData.Add("dfTokki3", tokki[4]);
            }

            //点数欄印字処理
            void _printoutTenCol()
            {
                // 初診
                SingleData.Add("dfSyosinJikan", _coReceipt.SyosinJikangai);

                // 点数
                List<string> tensuSyukeiSakils =
                    new List<string>
                    {
                            "1200",
                            "1220",
                            "1230",
                            "1240",
                            "1250",
                            "2110",
                            "2310",
                            "2500"
                    };
                foreach (string syukeisaki in tensuSyukeiSakils)
                {
                    SingleData.Add("dfTen_" + syukeisaki, CIUtil.ToStringIgnoreZero(_coReceipt.TenColTensu(syukeisaki)));
                }

                // 回数
                List<List<string>> countSyukeiSakils =
                    new List<List<string>>
                    {
                            //new List<string>{ "1100", "1110", "1120", "1130", "1140", "1150" },
                            new List<string>{ "1100" },
                            new List<string>{ "1200" },
                            new List<string>{ "1220" },
                            new List<string>{ "1230" },
                            new List<string>{ "1240" },
                            new List<string>{ "1250" },
                            new List<string>{ "1400" },
                            new List<string>{ "1410" },
                            new List<string>{ "1420" },
                            new List<string>{ "1430" },
                            new List<string>{ "2100" },
                            new List<string>{ "2110" },
                            new List<string>{ "2200" },
                            new List<string>{ "2300" },
                            new List<string>{ "2310" },
                            new List<string>{ "2500" },
                            new List<string>{ "2600" },
                            new List<string>{ "3110" },
                            new List<string>{ "3210" },
                            new List<string>{ "3310" },
                            new List<string>{ "4000" },
                            new List<string>{ "5000" },
                            new List<string>{ "6000" },
                            new List<string>{ "7000" },
                            new List<string>{ "8000" }
                    };
                foreach (List<string> syukeisaki in countSyukeiSakils)
                {
                    bool onlySI =
                        (syukeisaki.Any(p =>
                            new List<string> { "3110", "3210", "3310", "8000" }.Contains(p)));

                    SingleData.Add("dfCount_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(_coReceipt.TenColCount(syukeisaki, onlySI)));
                }
                // 合計点数
                List<List<string>> totalSyukeiSakils =
                    new List<List<string>>
                    {
                            new List<string>{ "1100", "1110", "1120", "1130", "1140", "1150", "1189" },
                            new List<string>{ "1200" },
                            new List<string>{ "1220" },
                            new List<string>{ "1230" },
                            new List<string>{ "1240" },
                            new List<string>{ "1250" },
                            new List<string>{ "1300" },
                            new List<string>{ "1400" },
                            new List<string>{ "1410" },
                            new List<string>{ "1420" },
                            new List<string>{ "1430" },
                            new List<string>{ "1440" },
                            new List<string>{ "1450" },
                            new List<string>{ "2100" },
                            new List<string>{ "2110" },
                            new List<string>{ "2200" },
                            new List<string>{ "2300" },
                            new List<string>{ "2310" },
                            new List<string>{ "2500" },
                            new List<string>{ "2600" },
                            new List<string>{ "2700" },
                            new List<string>{ "3110" },
                            new List<string>{ "3210" },
                            new List<string>{ "3310" },
                            new List<string>{ "4000" },
                            new List<string>{ "4010" },
                            new List<string>{ "5000" },
                            new List<string>{ "5010" },
                            new List<string>{ "6000" },
                            new List<string>{ "6010" },
                            new List<string>{ "7000" },
                            new List<string>{ "7010" },
                            new List<string>{ "8000" },
                            new List<string>{ "8010" },
                            new List<string>{ "8020" }
                    };
                foreach (List<string> syukeisaki in totalSyukeiSakils)
                {
                    SingleData.Add("dfTotal_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(_coReceipt.TenColTotalTen(syukeisaki)));
                }
                // 公費分点分
                if (_coReceipt.GetReceiptSbt(2) != 2 && _coReceipt.GetReceiptSbt(3) > 0 && _coReceipt.Tensu != (_coReceipt.KohiReceTensu(1) ?? 0))
                {
                    foreach (List<string> syukeisaki in totalSyukeiSakils)
                    {
                        SingleData.Add("dfKTen_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(_coReceipt.TenColTotalTenKohi(syukeisaki, 1)));
                    }
                }

                // 初診
                List<(string syukeiSaki, string comment)> syosinSyukeisaki =
                    new List<(string, string)>
                    {
                            (ReceSyukeisaki.SyosinJikanGai, "時間外"),
                            (ReceSyukeisaki.SyosinKyujitu, "休日"),
                            (ReceSyukeisaki.SyosinSinya, "深夜")
                    };
                string Syosin = "";

                for (int j = 0; j < syosinSyukeisaki.Count; j++)
                {
                    if (_coReceipt.TenColTensu(syosinSyukeisaki[j].syukeiSaki) > 0)
                    {
                        if (Syosin.Contains(syosinSyukeisaki[j].comment) == false)
                        {
                            if (Syosin != "") Syosin += ",";
                            Syosin += syosinSyukeisaki[j].comment;
                        }
                    }
                }

                if (Syosin != "")
                {
                    SingleData.Add("dfSyosin", Syosin);
                }
            }

            void _setValFieldZero(string field, string value)
            {
                if (string.IsNullOrEmpty(value) == false)
                {
                    if (value == "0")
                    {
                        SingleData.Add(field + "_0", value);
                    }
                    else
                    {
                        SingleData.Add(field, value);
                    }
                }
            }

            //療養の給付欄
            void _printoutRyoyoKyufu()
            {
                //保険点数
                if (Target == TargetConst.OsakaSyouni && (int)_systemConfRepository.GetSettingValue(94005, 0, HpId) == 1)
                {
                    // 大阪小児喘息レセプト(東大阪タイプ)の場合
                    SingleData.Add($"dfTensuHo", CIUtil.ToStringIgnoreNull(_coReceipt.KohiTensuReceInf(1)));
                }
                else
                {
                    // 通常の場合
                    SingleData.Add("dfTensuHo", CIUtil.ToStringIgnoreNull(_coReceipt.HokenReceTensu));
                }

                //保険決定点
                if (Target == TargetConst.OsakaSyouni && (int)_systemConfRepository.GetSettingValue(94005, 0, HpId) == 0)
                {
                    // 大阪小児喘息レセプト(大阪タイプ)の場合
                    SingleData.Add($"dfTensuZensoku", CIUtil.ToStringIgnoreNull(_coReceipt.KohiTensuReceInf(1)));
                }

                //国保減免
                SingleData.Add("dfKokuhoGenmen", _getGenmenKbn());

                string kohiField4 = "";
                if (CIUtil.StrToIntDef(CIUtil.Copy(_coReceipt.ReceiptSbt, 3, 1), 0) >= 4 ||
                    (_coReceipt.GetReceiptSbt(2) == 2 && _coReceipt.GetReceiptSbt(3) >= 3))
                {
                    // 公費が3つ以上のときは、印字するフィールドを変える
                    kohiField4 = "4";
                }

                //保険一部負担額
                SingleData.Add($"dfIchibu{kohiField4}Ho", CIUtil.ToStringIgnoreNull(_coReceipt.HokenReceFutan));
                //かっこ書き１
                SingleData.Add($"dfKyufu{kohiField4}K1", _getKohiKyufu(1));
                //かっこ書き２
                SingleData.Add($"dfKyufu{kohiField4}K2", _getKohiKyufu(2));

                //公１点数
                if (_coReceipt.HokenReceTensu != (_coReceipt.KohiReceTensu(1) ?? 0))
                {
                    _setValFieldZero($"dfTensu{kohiField4}K1", CIUtil.ToStringIgnoreNull(_coReceipt.KohiReceTensu(1)));
                }

                //小児喘息割合
                if (Target == TargetConst.OsakaSyouni)
                {
                    // 大阪小児喘息レセプトの場合
                    SingleData.Add("dfZensokuWari", _coReceipt.HokenRate / 10 + "割");
                }

                //公１一部負担額
                if (Target == TargetConst.OsakaSyouni)
                {
                    // 大阪小児喘息レセプトの場合
                    _setValFieldZero($"dfIchibu{kohiField4}K1", CIUtil.ToStringIgnoreNull(_coReceipt.KohiReceFutanReceInf(1)));
                }
                else
                {
                    // 通常
                    _setValFieldZero($"dfIchibu{kohiField4}K1", CIUtil.ToStringIgnoreNull(_coReceipt.KohiReceFutan(1)));
                }

                //公２点数
                if (_coReceipt.Tensu != (_coReceipt.KohiReceTensu(2) ?? 0) ||
                    (_coReceipt.KohiReceTensu(1) ?? 0) != (_coReceipt.KohiReceTensu(2) ?? 0))
                {
                    // 総点数と異なる or 一つ上の公費点数と異なる
                    _setValFieldZero($"dfTensu{kohiField4}K2", CIUtil.ToStringIgnoreNull(_coReceipt.KohiReceTensu(2)));
                }
                //公２一部負担額
                _setValFieldZero($"dfIchibu{kohiField4}K2", CIUtil.ToStringIgnoreNull(_coReceipt.KohiReceFutan(2)));

                if (CIUtil.StrToIntDef(CIUtil.Copy(_coReceipt.ReceiptSbt, 3, 1), 0) >= 4 ||
                    (_coReceipt.GetReceiptSbt(2) == 2 && _coReceipt.GetReceiptSbt(3) >= 3))
                {
                    //公３点数
                    if (_coReceipt.Tensu != (_coReceipt.KohiReceTensu(3) ?? 0) ||
                        (_coReceipt.KohiReceTensu(2) ?? 0) != (_coReceipt.KohiReceTensu(3) ?? 0))
                    {
                        // 総点数と異なる or 一つ上の公費点数と異なる
                        _setValFieldZero("dfTensu4K3", CIUtil.ToStringIgnoreNull(_coReceipt.KohiReceTensu(3)));
                    }
                    //公３一部負担額
                    _setValFieldZero("dfIchibu4K3", CIUtil.ToStringIgnoreNull(_coReceipt.KohiReceFutan(3)));
                    //かっこ書き３
                    SingleData.Add($"dfKyufu{kohiField4}K3", _getKohiKyufu(3));
                    //公４点数
                    if (_coReceipt.Tensu != (_coReceipt.KohiReceTensu(4) ?? 0) ||
                        (_coReceipt.KohiReceTensu(3) ?? 0) != (_coReceipt.KohiReceTensu(4) ?? 0))
                    {
                        // 総点数と異なる or 一つ上の公費点数と異なる
                        _setValFieldZero("dfTensu4K4", CIUtil.ToStringIgnoreNull(_coReceipt.KohiReceTensu(4)));
                    }
                    //公４一部負担額
                    _setValFieldZero("dfIchibu4K4", CIUtil.ToStringIgnoreNull(_coReceipt.KohiReceFutan(4)));
                    //かっこ書き４
                    SingleData.Add($"dfKyufu{kohiField4}K4", _getKohiKyufu(4));
                }
            }
            //療養の給付欄（神奈川レセプト２枚目用）
            void _printoutRyoyoKyufuForKanagawa2()
            {
                // 神奈川レセプト２枚目

                int kanagawaKohiIndex = 0;
                string kanagawaKohiHoubetu = "";
                int kanagawaKohiFutanRate = 0;

                for (int i = 1; i <= 4; i++)
                {
                    if (new List<string> { "80", "81", "85", "88", "89" }.Contains(_coReceipt.KohiHoubetu(i)))
                    {
                        kanagawaKohiIndex = i;
                        kanagawaKohiHoubetu = _coReceipt.KohiHoubetu(i);
                        kanagawaKohiFutanRate = _coReceipt.KohiRate(i);

                        break;
                    }
                }

                //保険点数
                if (new List<string> { "88", "89" }.Contains(kanagawaKohiHoubetu) &&
                    _coReceipt.KohiReceTensu(kanagawaKohiIndex) != _coReceipt.HokenReceTensu)
                {
                    // 88, 89で点数が保険分と異なる場合
                    SingleData.Add("dfTensuHoTorikesi", CIUtil.ToStringIgnoreNull(_coReceipt.KohiReceTensu(kanagawaKohiIndex)));
                    SingleData.Add("dfTensuHoKohi", CIUtil.ToStringIgnoreNull(_coReceipt.KohiReceTensu(kanagawaKohiIndex)));
                }
                else
                {
                    SingleData.Add("dfTensuHo", CIUtil.ToStringIgnoreNull(_coReceipt.KohiReceTensu(kanagawaKohiIndex)));
                }

                //保険一部負担額
                if (_coReceipt.KohiKyufu(kanagawaKohiIndex) > 0)
                {
                    SingleData.Add("dfIchibuHo", CIUtil.ToStringIgnoreNull(_coReceipt.KohiKyufu(kanagawaKohiIndex)));
                }
                else
                {
                    SingleData.Add("dfIchibuHo", CIUtil.ToStringIgnoreNull(_coReceipt.HokenReceFutan));
                }

                //かっこ書き１
                SingleData.Add("dfKyufuK1", _getKohiKyufu(1));
                //かっこ書き２
                SingleData.Add("dfKyufuK2", _getKohiKyufu(2));
                //国保減免
                SingleData.Add("dfKokuhoGenmen", _getGenmenKbn());

                string kohiField4 = "";
                if (CIUtil.StrToIntDef(CIUtil.Copy(_coReceipt.ReceiptSbt, 3, 1), 0) >= 4 ||
                    (_coReceipt.GetReceiptSbt(2) == 2 && _coReceipt.GetReceiptSbt(3) >= 3))
                {
                    // 公費が3つ以上のときは、印字するフィールドを変える
                    kohiField4 = "4";
                }

                //公１点数
                if (_coReceipt.Tensu != (_coReceipt.KohiReceTensu(1) ?? 0))
                {
                    _setValFieldZero($"dfTensu{kohiField4}K1", CIUtil.ToStringIgnoreNull(_coReceipt.KohiReceTensu(1)));
                }
                //小児喘息割合

                //公１一部負担額
                if (kanagawaKohiIndex == 1 && kanagawaKohiFutanRate == 100)
                {
                    // 公費の負担率が100%の場合は印字しない
                }
                else
                {
                    _setValFieldZero($"dfIchibu{kohiField4}K1", CIUtil.ToStringIgnoreNull(_coReceipt.KohiReceFutan(1)));
                }

                //公２点数
                if (_coReceipt.Tensu != (_coReceipt.KohiReceTensu(2) ?? 0) ||
                    (_coReceipt.KohiReceTensu(1) ?? 0) != (_coReceipt.KohiReceTensu(2) ?? 0))
                {
                    // 総点数と異なる or 一つ上の公費点数と異なる
                    _setValFieldZero($"dfTensu{kohiField4}K2", CIUtil.ToStringIgnoreNull(_coReceipt.KohiReceTensu(2)));
                }
                //公２一部負担額
                if (kanagawaKohiIndex == 2 && kanagawaKohiFutanRate == 100)
                {
                    // 公費の負担率が100%の場合は印字しない
                }
                else
                {
                    _setValFieldZero($"dfIchibu{kohiField4}K2", CIUtil.ToStringIgnoreNull(_coReceipt.KohiReceFutan(2)));
                }

                if (CIUtil.StrToIntDef(CIUtil.Copy(_coReceipt.ReceiptSbt, 3, 1), 0) >= 4 ||
                    (_coReceipt.GetReceiptSbt(2) == 2 && _coReceipt.GetReceiptSbt(3) >= 3))
                {
                    //公３点数
                    if (_coReceipt.Tensu != (_coReceipt.KohiReceTensu(3) ?? 0) ||
                        (_coReceipt.KohiReceTensu(2) ?? 0) != (_coReceipt.KohiReceTensu(3) ?? 0))
                    {
                        // 総点数と異なる or 一つ上の公費点数と異なる
                        _setValFieldZero("dfTensu4K3", CIUtil.ToStringIgnoreNull(_coReceipt.KohiReceTensu(3)));
                    }
                    //公３一部負担額
                    if (kanagawaKohiIndex == 3 && kanagawaKohiFutanRate == 100)
                    {
                        // 公費の負担率が100%の場合は印字しない
                    }
                    else
                    {
                        _setValFieldZero("dfIchibu4K3", CIUtil.ToStringIgnoreNull(_coReceipt.KohiReceFutan(3)));
                    }
                    //公４点数
                    if (_coReceipt.Tensu != (_coReceipt.KohiReceTensu(4) ?? 0) ||
                        (_coReceipt.KohiReceTensu(3) ?? 0) != (_coReceipt.KohiReceTensu(4) ?? 0))
                    {
                        // 総点数と異なる or 一つ上の公費点数と異なる
                        _setValFieldZero("dfTensu4K4", CIUtil.ToStringIgnoreNull(_coReceipt.KohiReceTensu(4)));
                    }
                    //公４一部負担額
                    if (kanagawaKohiIndex == 4 && kanagawaKohiFutanRate == 100)
                    {
                        // 公費の負担率が100%の場合は印字しない
                    }
                    else
                    {
                        _setValFieldZero("dfIchibu4K4", CIUtil.ToStringIgnoreNull(_coReceipt.KohiReceFutan(4)));
                    }
                }
            }

            //OCR印字処理
            void _printoutOCR()
            {
                SingleData.Add("dfOCR1", _getOCR1());
                SingleData.Add("dfOCR2", _getOCR2());
                SingleData.Add("dfOCR3", _getOCR3());
            }

            // 都道府県別特殊処理
            void _tokusyu()
            {
                if (_coReceipt.PrefNo == PrefCode.Iwate)
                {
                    #region 岩手県

                    if (kohiDatas.Any())
                    {
                        string fukusi = "";
                        string tokusyuNo = "";

                        for (int i = 1; i <= 4; i++)
                        {
                            if (_coReceipt.KohiHokenIdAll(i) > 0)
                            {
                                List<KohiDataModel> kohiData = kohiDatas.FindAll(p => p.HokenId == _coReceipt.KohiHokenIdAll(i));

                                if (kohiData.Any())
                                {
                                    if (new int[] { 110, 111 }.Contains(kohiData.First().HokenNo))
                                    {
                                        fukusi = "乳";
                                    }
                                    else if (new int[] { 120, 121 }.Contains(kohiData.First().HokenNo))
                                    {
                                        fukusi = "妊";
                                    }
                                    else if (new int[] { 130, 131 }.Contains(kohiData.First().HokenNo))
                                    {
                                        fukusi = "重";
                                    }
                                    else if (new int[] { 140, 141 }.Contains(kohiData.First().HokenNo))
                                    {
                                        fukusi = "母";
                                    }
                                    else if (new int[] { 150, 151, 160, 161 }.Contains(kohiData.First().HokenNo))
                                    {
                                        fukusi = "寡";
                                    }
                                    else if (new int[] { 170, 171 }.Contains(kohiData.First().HokenNo))
                                    {
                                        fukusi = "父";
                                    }
                                    else if (new int[] { 181 }.Contains(kohiData.First().HokenNo))
                                    {
                                        fukusi = "中";
                                    }
                                    else if (new int[] { 191 }.Contains(kohiData.First().HokenNo))
                                    {
                                        fukusi = "小";
                                    }

                                    if (fukusi != "")
                                    {
                                        // 福祉公費の場合、特殊番号を控えておく（レセプト２枚目の場合に印字）
                                        tokusyuNo = kohiData.First().TokusyuNo;
                                        break;
                                    }
                                }
                            }
                        }

                        if (fukusi != "")
                        {
                            SingleData.Add("dfFukusiKigo1", fukusi);

                            if (Target == TargetConst.IwateRece2)
                            {
                                if (tokusyuNo != "")
                                {
                                    SingleData.Add("dfFutanNoFukusi", Copy(tokusyuNo, 1, 8).PadRight(8, ' '));
                                    if (tokusyuNo.Length > 8)
                                    {
                                        SingleData.Add("dfJyukyuNoFukusi", Copy(tokusyuNo, 9, 7).PadRight(7, ' '));
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                else if (_coReceipt.PrefNo == PrefCode.Tokyo)
                {
                    #region 東京都

                    // 生保交付番号
                    for (int i = 1; i <= 4; i++)
                    {
                        if (_coReceipt.KohiHoubetu(i) == "12" || _coReceipt.KohiHoubetu(i) == "25")
                        {
                            SingleData.Add("dfKofu", _coReceipt.KohiTokusyuNo(i));
                            break;
                        }
                    }
                    # endregion
                }
                else if (_coReceipt.PrefNo == PrefCode.Nagano)
                {
                    #region 長野県
                    if (Target == TargetConst.NaganoRece2)
                    {
                        // 長野県レセプト2枚目
                        for (int i = 1; i <= 4; i++)
                        {
                            if (_coReceipt.KohiHokenIdAll(i) > 0)
                            {
                                List<KohiDataModel> kohiData = kohiDatas.FindAll(p => p.HokenId == _coReceipt.KohiHokenIdAll(i));

                                if (kohiData.Any() && kohiData.First().TokusyuNo != "")
                                {
                                    // 福祉番号
                                    SingleData.Add("dfFukusiNo", kohiData.First().TokusyuNo);
                                    break;
                                }
                            }
                        }
                    }
                    #endregion
                }
                else if (_coReceipt.PrefNo == PrefCode.Shiga)
                {
                    #region 滋賀県
                    SingleData.Add("df25KogakuKbn", _coReceipt.KogakuKbnMessage);
                    #endregion
                }
                else if (_coReceipt.PrefNo == PrefCode.Osaka)
                {
                    #region 大阪府
                    if (Target == TargetConst.OsakaSyouni)
                    {
                        // 大阪小児喘息
                        for (int i = 1; i <= 4; i++)
                        {
                            if (_coReceipt.KohiHokenIdAll(i) > 0 && new int[] { 198, 298 }.Contains(_coReceipt.KohiHokenIdAll(i)))
                            {
                                // 小児喘息公費
                                List<KohiDataModel> kohiData = kohiDatas.FindAll(p => p.HokenId == _coReceipt.KohiHokenIdAll(i));

                                if (kohiData.Any() && kohiData.First().TokusyuNo != "")
                                {
                                    // 福祉番号
                                    if ((int)_systemConfRepository.GetSettingValue(94005, 0, HpId) == 0)
                                    {
                                        // 大阪市タイプ
                                        if (kohiData.First().TokusyuNo.Length == 8)
                                        {
                                            SingleData.Add("dfFutanNoFukusi", kohiData.First().TokusyuNo);
                                        }
                                    }
                                    else
                                    {
                                        // 東大阪市タイプ
                                        SingleData.Add("dfSyoniZensoku", kohiData.First().TokusyuNo);
                                    }

                                    break;
                                }
                            }
                        }

                        SingleData.Add("dfFukusiKigo2", "（ゼ）");
                    }
                    #endregion
                }
                else if (_coReceipt.PrefNo == PrefCode.Nara)
                {
                    // 奈良県
                    if (_coReceipt.HokenKbn == 1)
                    {
                        for (int i = 1; i <= 4; i++)
                        {
                            if (new List<string> { "41", "71", "81", "91" }.Contains(_coReceipt.KohiHoubetuAll(i)))
                            {
                                SingleData.Add("dfFukusiKigo1", "奈福");
                            }
                        }
                    }
                }
                else if (_coReceipt.PrefNo == PrefCode.Fukuoka)
                {
                    #region 福岡
                    if (_coReceipt.HokenKbn == 1)
                    {
                        for (int i = 1; i <= 4; i++)
                        {
                            {
                                List<KohiDataModel> kohiData = kohiDatas.FindAll(p => p.HokenId == _coReceipt.KohiHokenIdAll(i));

                                if (kohiData.Any())
                                {
                                    List<(int hokenNo, string kigo)> fukuokaFukusi =
                                        new List<(int hokenNo, string kigo)>
                                        {
                                            ( 180, "障" ),
                                            ( 181, "乳" ),
                                            ( 190, "親" )
                                        };

                                    foreach ((int hokenNo, string kigo) in fukuokaFukusi)
                                    {
                                        string fukusiKigo = "";

                                        if (kohiData.First().HokenNo == hokenNo)
                                        {
                                            if (new int[] { 2, 3, 5 }.Contains(kohiData.First().ReceSeikyuKbn))
                                            {
                                                // 社保単独の設定の場合
                                                fukusiKigo = kigo;
                                            }
                                            int kyufuGai = 0;
                                            if (_coReceipt.PtFutan > 0)
                                            {
                                                kyufuGai =
                                                    _coReceipt.TotalIryoHi - _coReceipt.HokenFutan10en - _coReceipt.KogakuFutan10en - (_coReceipt.KohiFutan10enReceInf(i) ?? 0);
                                            }
                                            _printoutFukuokaFukusi(fukusiKigo, kohiData.First().FutansyaNo, kohiData.First().JyukyusyaNo, kyufuGai);

                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
            // 福岡福祉記号印字処理
            void _printoutFukuokaFukusi(string fukusiKigo, string futansyaNo, string jyukyusyaNo, int kyufuGai)
            {
                if (fukusiKigo != "")
                {
                    for (int i = 1; i <= 2; i++)
                    {
                        SingleData.Add($"dfFukusiKigo{i}", fukusiKigo);
                        SingleData.Add($"dfFukusiKigoMaru{i}", "〇");
                    }
                }

                if (Target == TargetConst.FukuokaRece2)
                {
                    SingleData.Add("dfFukuokaFukusiFutanNoCaption1", "乳・障・親");
                    SingleData.Add("dfFukuokaFukusiFutanNoCaption2", "負担者番号");
                    SingleData.Add("dfFukuokaFukusiFutanNo", futansyaNo);
                    SingleData.Add("dfFukuokaFukusiJyukyuNoCaption1", "乳・障・親");
                    SingleData.Add("dfFukuokaFukusiJyukyuNoCaption2", "受給者番号");
                    SingleData.Add("dfFukuokaFukusiJyukyuNo", jyukyusyaNo);
                    SingleData.Add("dfFukuokaFukusiKyufuGaiCaption1", "乳・障・親");
                    SingleData.Add("dfFukuokaFukusiKyufuGaiCaption2", "給付外の額");
                    SingleData.Add("dfFukuokaFukusiKyufuGai", kyufuGai.ToString() + "円");
                }
            }

            #endregion


            // HEAD印字

            #region 本紙・続紙に関わらず必須
            // 患者番号 + 法別番号
            SingleData.Add("dfPtNo", _getPtNo());

            // バーコード 患者番号前ゼロ9桁
            SingleData.Add("bcKanID_z9", $"{_coReceipt.PtNum:D9}");

            // 帳票番号
            if (!(new int[] { TargetConst.KanagawaRece2 }.Contains(Target)))
            {
                SingleData.Add("dfPrintNo", getPrintNo());
            }

            // 社保国保
            SingleData.Add("dfSyaKoku", _getSyaKoku());

            // 診療年月
            SingleData.Add("dfSinYM", _getSinYM());

            // 医療機関コード
            SingleData.Add("dfHpNo", CIUtil.FormatHpCd(_coReceipt.HpCd, _coReceipt.PrefNo));

            // レセ種別１
            SingleData.Add("dfReceSbt1", _getReceSbt1());

            // レセ種別２
            SingleData.Add("dfReceSbt2", _getReceSbt2());

            // レセ種別３
            SingleData.Add("dfReceSbt3", _getReceSbt3());

            //漢字氏名
            SingleData.Add("dfPtKanjiName", _coReceipt.PtName);

            // 保険者番号
            SingleData.Add("dfHokensyaNo", string.Format("{0, 8}", _coReceipt.HokensyaNo));

            // 記号
            SingleData.Add("dfKigo", _coReceipt.Kigo);

            // 番号
            SingleData.Add("dfBango", _coReceipt.Bango);

            // 枝番
            SingleData.Add("dfEdaNo", _coReceipt.EdaNo);
            #endregion

            if (CurrentPage == 1 || CIUtil.Copy(_coReceipt.ReceiptSbt, 2, 1) == "2")
            {
                // 公費医療の場合は、続紙にも公１の負担者番号・受給者番号を印字

                //公１負担者番号
                SingleData.Add("dfFutanNoK1", string.Format("{0, 8}", _coReceipt.KohiFutansyaNo(1)));
                //公１受給者番号
                SingleData.Add("dfJyukyuNoK1", string.Format("{0, 7}", _coReceipt.KohiJyukyusyaNo(1)));
            }

            if (CurrentPage == 1)
            {
                #region 本紙のみ
                //公２負担者番号
                SingleData.Add("dfFutanNoK2", string.Format("{0, 8}", _coReceipt.KohiFutansyaNo(2)));
                //公２受給者番号
                SingleData.Add("dfJyukyuNoK2", string.Format("{0, 7}", _coReceipt.KohiJyukyusyaNo(2)));

                // 負担率
                if (_coReceipt.HokenKbn == 2 && _coReceipt.GetReceiptSbt(2) != 3)
                {
                    SingleData.Add("dfKyufuWari", ((100 - _coReceipt.HokenRate) / 10).ToString() + "割");
                }

                // 県番号
                SingleData.Add("dfPrefNo", _coReceipt.PrefNo.ToString());

                //カナ氏名
                SingleData.Add("dfPtKanaName", _coReceipt.PtKanaName);

                //性別
                SingleData.Add("dfSex", _getSex());

                //生年月日
                SingleData.Add("dfBirthDay", _getBirthDay());

                //職務上の事由
                SingleData.Add("dfSyokumuJiyu", _getSyokumuJiyu());

                //特記事項
                _printoutTokki();

                //医療機関住所
                SingleData.Add("txHpAddress", _coReceipt.HpAddress);

                //医療機関名
                SingleData.Add("dfHpName", _coReceipt.ReceHpName);

                //医療機関電話番号
                SingleData.Add("dfHpTel", _coReceipt.HpTel);

                //病名欄
                foreach (var item in ByomeiModels)
                {
                    var data = new Dictionary<string, CellModel>();
                    data.Add("lsByomei", new CellModel(item.Byomei));
                    data.Add("lsByomeiStart", new CellModel(item.StartDate));
                    data.Add("lsByomeiTenki", new CellModel(item.Tenki));
                    CellData.Add(data);
                }

                // 実日数
                _printoutJituNissu();

                //点数欄
                if (!(new int[]
                    {
                        TargetConst.KanagawaRece2,
                        TargetConst.FukuokaRece2,
                        TargetConst.SagaRece2,
                        TargetConst.MiyazakiRece2
                    }.Contains(Target)))
                {
                    _printoutTenCol();
                }

                // 療養の給付欄
                if (Target == TargetConst.KanagawaRece2)
                {
                    _printoutRyoyoKyufuForKanagawa2();
                }
                else
                {
                    _printoutRyoyoKyufu();
                }

                //OCR
                if (!(new int[]
                    {
                        TargetConst.KanagawaRece2,
                        TargetConst.OsakaSyouni,
                        TargetConst.FukuokaRece2,
                        TargetConst.MiyazakiRece2
                    }.Contains(Target)))
                {
                    _printoutOCR();
                }

                // 都道府県別処理
                _tokusyu();


                #endregion
            }
            else if (CurrentPage >= 2)
            {
                #region 本紙以外
                // Page
                SingleData.Add("dfPage", $"P.{CurrentPage}");
                #endregion
            }
        }

        private void PrintReceiptHeaderForRousai()
        {
            #region Sub Function
            // 年月を労災レセプト様式に応じた書式に変換する
            string _getYm(int Ym)
            {
                string ret = "";

                if ((int)_systemConfRepository.GetSettingValue(94002, 0, HpId) == 1)
                {
                    ret = CIUtil.SDateToWDateForRousai(Ym).ToString();
                }
                else
                {
                    // 旧様式は元号なし
                    ret = $"{(CIUtil.SDateToWDateForRousai(Ym) % 1000000):D6}";
                }
                return ret;
            }
            // 傷病開始日
            string _getSyobyoDate()
            {
                return _getYm(_coReceipt.RousaiSyobyoDate);
            }
            // 療養開始日
            string _getRyoyoStartDate()
            {
                return _getYm(_coReceipt.RyoyoStartDate);
            }
            // 療養開始日
            string _getRyoyoEndDate()
            {
                return _getYm(_coReceipt.RyoyoEndDate);
            }

            // 実日数
            string _getJituNissu()
            {
                string ret = CIUtil.ToStringIgnoreNull(_coReceipt.RousaiJituNissu);
                if (_coReceipt.RousaiJituNissu != null)
                {
                    if (_coReceipt.RousaiJituNissu == 0)
                    {
                        ret = "999";
                    }
                }
                return ret;
            }

            // 年齢
            string _getAge()
            {
                string ret = CIUtil.SDateToAge(_coReceipt.BirthDay, CIUtil.GetLastDateOfMonth(_coReceipt.SinYm * 100 + 1)).ToString();
                return ret;
            }
            #endregion

            // HEAD印字

            #region 本紙・続紙に関わらず必須
            // 医療機関コード
            SingleData.Add("dfHpNo", CIUtil.FormatHpCd(_coReceipt.RousaiHpCd, 0));
            //医療機関名
            SingleData.Add("dfHpName", _coReceipt.ReceHpName);
            // 帳票番号
            SingleData.Add("dfPrintNo", getPrintNo());
            // バーコード 患者番号前ゼロ9桁
            SingleData.Add("bcKanID_z9", $"{_coReceipt.PtNum:D9}");
            // 漢字氏名
            SingleData.Add("dfPtKanjiName", _coReceipt.PtName);
            // 年齢
            SingleData.Add("dfAge", _getAge());
            #endregion

            if (CurrentPage == 1)
            {
                #region 本紙のみ
                // 患者番号
                SingleData.Add("dfPtNo", _coReceipt.PtNum.ToString());
                // 請求回数
                SingleData.Add("dfReceCount", _coReceipt.RousaiReceCount.ToString());
                //新継再別
                SingleData.Add("dfSinkei", _coReceipt.RousaiSinkei.ToString());

                //転帰
                SingleData.Add("dfTenki", _coReceipt.RousaiTenki.ToString());

                //労災交付番号
                SingleData.Add("dfRousaiKofuNo", _coReceipt.RousaiKofu);

                //生年月日
                int rousaiBirthDay = CIUtil.SDateToWDateForRousai(_coReceipt.BirthDay);
                SingleData.Add("dfBirthDay", rousaiBirthDay.ToString());

                string gengo = "";

                if (rousaiBirthDay > 0)
                {
                    switch (rousaiBirthDay / 1000000)
                    {
                        case 1: gengo = "M"; break;
                        case 3: gengo = "T"; break;
                        case 5: gengo = "S"; break;
                        case 7: gengo = "H"; break;
                        case 9: gengo = "R"; break;
                    }

                    SingleData.Add("dfBirthGengo" + gengo, "〇");

                    SingleData.Add("dfBirthY", (rousaiBirthDay / 10000 % 100).ToString());
                    SingleData.Add("dfBirthM", (rousaiBirthDay / 100 % 100).ToString());
                    SingleData.Add("dfBirthD", (rousaiBirthDay % 100).ToString());
                }

                //傷病開始日
                SingleData.Add("dfRousaiSyobyoDate", _getSyobyoDate());

                //療養開始日
                SingleData.Add("dfRyoyoStartDate", _getRyoyoStartDate());

                //療養終了日
                SingleData.Add("dfRyoyoEndDate", _getRyoyoEndDate());

                //実日数
                SingleData.Add("dfNissu", _getJituNissu().PadLeft(3, ' '));

                //合計金額
                SingleData.Add("dfTotal", $"{_coReceipt.RousaiTotal.ToString(),7}");

                //事業所名
                SingleData.Add("txJigyosyoName", _coReceipt.JigyosyoName);
                //事業所所在地（都道府県）
                SingleData.Add("dfPrefName", _coReceipt.RousaiPrefName);

                //事業所所在地（市町村）
                SingleData.Add("dfCityName", _coReceipt.RousaiCityName);

                //傷病の経過
                SingleData.Add("txSyobyoKeika", _coReceipt.SyobyoKeika);

                //小計
                SingleData.Add("dfSyokei", _coReceipt.RousaiSyokei?.ToString() ?? string.Empty);
                //イ
                SingleData.Add("dfTenTotal", _coReceipt.RousaiSyokeiGaku_I?.ToString() ?? string.Empty);
                //ロ
                SingleData.Add("dfEnTotal", _coReceipt.RousaiSyokeiGaku_RO?.ToString() ?? string.Empty);
                //病名欄
                short i = 0;
                foreach (CoReceiptByomeiModel byomei in ByomeiModels)
                {
                    var data = new Dictionary<string, CellModel>();
                    data.Add("lsByomei", new CellModel(byomei.Byomei));
                    CellData.Add(data);
                    i++;
                }

                //点数欄
                // 点数
                List<List<string>> tensuSyukeiSakils =
                    new List<List<string>>
                    {
                        new List<string>{"1200" },
                        new List<string>{"1220", "1221" },
                        new List<string>{"1230" },
                        new List<string>{"1240" },
                        new List<string>{"1250" },
                        new List<string>{"2110" },
                        new List<string>{"2310" },
                        new List<string>{"2500" }
                    };
                foreach (List<string> syukeisaki in tensuSyukeiSakils)
                {
                    SingleData.Add("dfTen_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(_coReceipt.TenColTensu(syukeisaki)));
                }

                // 回数
                List<List<string>> countSyukeiSakils =
                    new List<List<string>>
                    {
                        new List<string>{ "1100", "1110", "1120", "1130", "1140", "1150" },
                        new List<string>{ "1200" },
                        new List<string>{ "1220", "1221" },
                        new List<string>{ "1230" },
                        new List<string>{ "1240" },
                        new List<string>{ "1250" },
                        new List<string>{ "1400" },
                        new List<string>{ "1410" },
                        new List<string>{ "1420" },
                        new List<string>{ "1430" },
                        new List<string>{ "2100" },
                        new List<string>{ "2110" },
                        new List<string>{ "2200" },
                        new List<string>{ "2300" },
                        new List<string>{ "2310" },
                        new List<string>{ "2500" },
                        new List<string>{ "2600" },
                        new List<string>{ "3110" },
                        new List<string>{ "3210" },
                        new List<string>{ "3310" },
                        new List<string>{ "4000" },
                        new List<string>{ "5000" },
                        new List<string>{ "6000" },
                        new List<string>{ "7000" },
                        new List<string>{ "8000" },
                        new List<string>{ "A110" },
                        new List<string>{ "A120" },
                        new List<string>{ "A130" }
                    };
                foreach (List<string> syukeisaki in countSyukeiSakils)
                {
                    bool onlySI =
                        (syukeisaki.Any(p =>
                            new List<string> { "3110", "3210", "3310", "8000" }.Contains(p)));

                    SingleData.Add("dfCount_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(_coReceipt.TenColCount(syukeisaki, onlySI)));
                }

                // 合計点数
                List<List<string>> totalSyukeiSakils =
                    new List<List<string>>
                    {
                        new List<string>{ "1100", "1110", "1120", "1130", "1140", "1150", "1189" },
                        new List<string>{ "1200" },
                        new List<string>{ "1220", "1221" },
                        new List<string>{ "1230" },
                        new List<string>{ "1240" },
                        new List<string>{ "1250" },
                        new List<string>{ "1300" },
                        new List<string>{ "1400" },
                        new List<string>{ "1410" },
                        new List<string>{ "1420" },
                        new List<string>{ "1430" },
                        new List<string>{ "1440" },
                        new List<string>{ "1450" },
                        new List<string>{ "2100" },
                        new List<string>{ "2110" },
                        new List<string>{ "2200" },
                        new List<string>{ "2300" },
                        new List<string>{ "2310" },
                        new List<string>{ "2500" },
                        new List<string>{ "2600" },
                        new List<string>{ "2700" },
                        new List<string>{ "3110" },
                        new List<string>{ "3210" },
                        new List<string>{ "3310" },
                        new List<string>{ "4000" },
                        new List<string>{ "4010" },
                        new List<string>{ "5000" },
                        new List<string>{ "5010" },
                        new List<string>{ "6000" },
                        new List<string>{ "6010" },
                        new List<string>{ "7000" },
                        new List<string>{ "7010" },
                        new List<string>{ "8000" },
                        new List<string>{ "8010" },
                        new List<string>{ "8020" },
                        new List<string>{ "A110" },
                        new List<string>{ "A120" },
                        new List<string>{ "A130" }
                    };
                foreach (List<string> syukeisaki in totalSyukeiSakils)
                {
                    SingleData.Add("dfTotal_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(_coReceipt.TenColTotalTen(syukeisaki)));
                }

                // その他金額
                List<(int count, double kingaku)> tencolKingakuSonotas = _coReceipt.TenColKingakuSonota("A180");

                int sonotaIndex = 1;
                foreach ((int count, double kingaku) tencolKingakuSonota in tencolKingakuSonotas)
                {
                    SingleData.Add($"dfCount_A180_{sonotaIndex}", CIUtil.ToStringIgnoreZero(tencolKingakuSonota.count) + " 回");
                    SingleData.Add($"dfTotal_A180_{sonotaIndex}", CIUtil.ToStringIgnoreZero(tencolKingakuSonota.kingaku));

                    sonotaIndex++;
                    if (sonotaIndex > 4) break;
                }

                tencolKingakuSonotas = _coReceipt.TenColKingakuSonota("A131");

                foreach ((int count, double kingaku) tencolKingakuSonota in tencolKingakuSonotas)
                {
                    SingleData.Add($"dfCount_A180_{sonotaIndex}", CIUtil.ToStringIgnoreZero(tencolKingakuSonota.count) + " 回");
                    SingleData.Add($"dfTotal_A180_{sonotaIndex}", CIUtil.ToStringIgnoreZero(tencolKingakuSonota.kingaku));

                    sonotaIndex++;
                    if (sonotaIndex > 4) break;
                }

                // 初診
                List<(string syukeiSaki, string field)> syosinSyukeisaki =
                    new List<(string, string)>
                    {
                        (ReceSyukeisaki.SyosinJikanGai, "dfSyosinJikanGai"),
                        (ReceSyukeisaki.SyosinKyujitu, "dfSyosinKyujitu"),
                        (ReceSyukeisaki.SyosinSinya, "dfSyosinSinya")
                    };

                for (int j = 0; j < syosinSyukeisaki.Count; j++)
                {
                    if (_coReceipt.TenColTensu(syosinSyukeisaki[j].syukeiSaki) > 0)
                    {
                        SingleData.Add(syosinSyukeisaki[j].field, "〇");
                    }
                }
                #endregion
            }
            else
            {
                #region 続紙のみ
                if (Target == TargetConst.RousaiTanki)
                {
                    //労災交付番号
                    SingleData.Add("dfRousaiRoudouNo", _coReceipt.RousaiKofu);
                }
                else
                {
                    //労災交付番号
                    SingleData.Add("dfRousaiNenkinNo", _coReceipt.RousaiKofu);
                }
                #endregion
            }
        }

        private void PrintReceiptHeaderForAfter()
        {
            #region Sub Function

            #endregion

            // HEAD印字

            #region 本紙・続紙に関わらず必須
            //医療機関名
            SingleData.Add("dfHpName", _coReceipt.ReceHpName);
            // 帳票番号
            SingleData.Add("dfPrintNo", getPrintNo());
            // バーコード 患者番号前ゼロ9桁
            SingleData.Add("bcKanID_z9", $"{_coReceipt.PtNum:D9}");
            //漢字氏名
            SingleData.Add("dfPtKanjiName", _coReceipt.PtName);
            //労災交付番号
            SingleData.Add("dfRousaiKofuNo", _coReceipt.RousaiKofu);

            #endregion

            if (CurrentPage == 1)
            {
                #region 本紙のみ
                // 患者番号
                SingleData.Add("dfPtNo", _coReceipt.PtNum.ToString());

                // 診療年月日
                SingleData.Add("dfSinDate", CIUtil.SDateToWDateForRousai(_coReceipt.SinDate).ToString());

                // 検査日
                if (_coReceipt.KensaDate > 0)
                {
                    SingleData.Add("dfKensaDate", CIUtil.SDateToWDateForRousai(_coReceipt.KensaDate).ToString());
                }
                // 傷病名コード
                SingleData.Add("dfSyobyoCd", _coReceipt.SyobyoCd);

                // 前回検査日
                if (_coReceipt.ZenkaiKensaDate > 0)
                {
                    WarekiYmd wareki = CIUtil.SDateToShowWDate3(_coReceipt.ZenkaiKensaDate);

                    SingleData.Add("dfZenkaiKensaYear", wareki.Gengo + wareki.Year);
                    SingleData.Add("dfZenkaiKensaMonth", wareki.Month.ToString());
                    SingleData.Add("dfZenkaiKensaDay", wareki.Day.ToString());
                }

                //傷病の経過
                SingleData.Add("txSyobyoKeika", _coReceipt.SyobyoKeika);
                //合計金額
                SingleData.Add("dfTotal", $"{_coReceipt.AfterTotal.ToString(),7}");
                //小計
                SingleData.Add("dfSyokei", _coReceipt.AfterSyokei.ToString());
                //イ
                SingleData.Add("dfTenTotal", _coReceipt.AfterSyokeiGaku_I.ToString());
                //ロ
                SingleData.Add("dfEnTotal", _coReceipt.AfterSyokeiGaku_RO.ToString());

                //点数欄
                // 点数
                List<string> tensuSyukeiSakils =
                    new List<string>
                    {
                        "2110",
                        "2310",
                        "2500"
                    };
                foreach (string syukeisaki in tensuSyukeiSakils)
                {
                    SingleData.Add("dfTen_" + syukeisaki, CIUtil.ToStringIgnoreZero(_coReceipt.TenColTensu(syukeisaki)));
                }

                // 回数
                List<List<string>> countSyukeiSakils =
                    new List<List<string>>
                    {
                        new List<string>{ "1100", "1110", "1120", "1130", "1140", "1150", "1189" },
                        new List<string>{ "1200" },
                        new List<string>{ "1300" },
                        new List<string>{ "2100" },
                        new List<string>{ "2110" },
                        new List<string>{ "2200" },
                        new List<string>{ "2300" },
                        new List<string>{ "2310" },
                        new List<string>{ "2500" },
                        new List<string>{ "2600" },
                        new List<string>{ "3110" },
                        new List<string>{ "3210" },
                        new List<string>{ "3310" },
                        new List<string>{ "4000" },
                        new List<string>{ "5000" },
                        new List<string>{ "6000" },
                        new List<string>{ "7000" },
                        new List<string>{ "8000" }
                    };
                foreach (List<string> syukeisaki in countSyukeiSakils)
                {
                    if (syukeisaki[0] == "1100")
                    {
                        bool onlySI =
                            (syukeisaki.Any(p =>
                                new List<string> { "3110", "3210", "3310", "8000" }.Contains(p)));

                        int count = _coReceipt.TenColCount(syukeisaki, onlySI);
                        if (count == 0 && _coReceipt.TenColTotalTen(syukeisaki) > 0)
                        {
                            // 初診の加算の場合、TENCOL_COUNT=0になってしまうことがある
                            // その場合、初診の回数を入れる
                            count = _coReceipt.TenColCount(new List<string> { "A110" }, false);
                        }
                        SingleData.Add("dfCount_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(count));
                    }
                    else
                    {
                        bool onlySI =
                            (syukeisaki.Any(p =>
                                new List<string> { "3110", "3210", "3310", "8000" }.Contains(p)));

                        SingleData.Add("dfCount_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(_coReceipt.TenColCount(syukeisaki, onlySI)));
                    }
                }

                // 合計点数
                List<List<string>> totalSyukeiSakils =
                    new List<List<string>>
                    {
                        new List<string>{ "1100", "1110", "1120", "1130", "1140", "1150", "1189" },
                        new List<string>{ "1200", "1220", "1221", "1230", "1240", "1250" },
                        new List<string>{ "1300" },
                        new List<string>{ "2100" },
                        new List<string>{ "2110" },
                        new List<string>{ "2200" },
                        new List<string>{ "2300" },
                        new List<string>{ "2310" },
                        new List<string>{ "2500" },
                        new List<string>{ "2600" },
                        new List<string>{ "2700" },
                        new List<string>{ "3110" },
                        new List<string>{ "3210" },
                        new List<string>{ "3310" },
                        new List<string>{ "4000" },
                        new List<string>{ "4010" },
                        new List<string>{ "5000" },
                        new List<string>{ "5010" },
                        new List<string>{ "6000" },
                        new List<string>{ "6010" },
                        new List<string>{ "7000" },
                        new List<string>{ "7010" },
                        new List<string>{ "8000" },
                        new List<string>{ "8010" },
                        new List<string>{ "8020" },
                        new List<string>{ "A110" },
                        new List<string>{ "A120" }
                    };
                foreach (List<string> syukeisaki in totalSyukeiSakils)
                {
                    SingleData.Add("dfTotal_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(_coReceipt.TenColTotalTen(syukeisaki)));
                }

                // 初診
                List<string> syosinSyukeisaki =
                    new List<string>
                    {
                        ReceSyukeisaki.SyosinJikanGai,
                        ReceSyukeisaki.SyosinKyujitu,
                        ReceSyukeisaki.SyosinSinya
                    };

                if (_coReceipt.TenColTensuSum(syosinSyukeisaki) > 0)
                {
                    SingleData.Add("dfSyosin1", "〇");
                }

                if (_coReceipt.TenColTensuSum(new List<string> { "1100", "1110", "1120", "1130", "1140", "1150", "1189" }) > 0)
                {
                    WarekiYmd wareki = CIUtil.SDateToShowWDate3(_coReceipt.SinDate);

                    SingleData.Add("dfSyosinYear", wareki.Gengo + wareki.Year);
                    SingleData.Add("dfSyosinMonth", wareki.Month.ToString());
                    SingleData.Add("dfSyosinDay", wareki.Day.ToString());
                }

                //再診
                List<string> saisinSyukeisaki =
                    new List<string>
                    {
                        ReceSyukeisaki.SaisinJikangai,
                        ReceSyukeisaki.SaisinKyujitu,
                        ReceSyukeisaki.SaisinSinya
                    };

                if (_coReceipt.TenColTensuSum(syosinSyukeisaki) > 0)
                {
                    SingleData.Add("dfSaisin1", "〇");
                }

                if (_coReceipt.TenColTensuSum(new List<string> { "1200", "1220", "1221", "1230", "1240", "1250" }) > 0)
                {
                    WarekiYmd wareki = CIUtil.SDateToShowWDate3(_coReceipt.SinDate);

                    SingleData.Add("dfSaisinYear", wareki.Gengo + wareki.Year);
                    SingleData.Add("dfSaisinMonth", wareki.Month.ToString());
                    SingleData.Add("dfSaisinDay", wareki.Day.ToString());
                }
                #endregion
            }
            else
            {
                #region 続紙のみ
                // 医療機関コード
                SingleData.Add("dfHpNo", CIUtil.FormatHpCd(_coReceipt.RousaiHpCd, 0));
                #endregion
            }

        }

        private void PrintReceiptHeaderForJibaiKenpo()
        {
            #region Sub Function
            // 性別
            string _getSex()
            {
                string ret = "男";
                if (_coReceipt.Sex == 2)
                {
                    ret = "女";
                }
                return ret;
            }
            // 年齢
            string _getAge()
            {
                string ret = CIUtil.SDateToAge(_coReceipt.BirthDay, CIUtil.GetLastDateOfMonth(_coReceipt.SinYm * 100 + 1)).ToString();
                return ret;
            }
            // 初診
            string _getSyosin()
            {
                List<(string syukeiSaki, string comment)> syosinSyukeisaki =
                    new List<(string, string)>
                    {
                                            (ReceSyukeisaki.SyosinJikanGai, "時間外"),
                                            (ReceSyukeisaki.SyosinKyujitu, "休日"),
                                            (ReceSyukeisaki.SyosinSinya, "深夜")
                    };
                string Syosin = "";

                for (int j = 0; j < syosinSyukeisaki.Count; j++)
                {
                    if (_coReceipt.TenColTensu(syosinSyukeisaki[j].syukeiSaki) > 0)
                    {
                        if (Syosin.Contains(syosinSyukeisaki[j].comment) == false)
                        {
                            if (Syosin != "") Syosin += ",";
                            Syosin += syosinSyukeisaki[j].comment;
                        }
                    }
                }

                return Syosin;
            }
            // 西暦(yyyymmdd）を和暦に変換して各フィールドに印字する
            void _printoutWarekiField(int date, string field, bool gengo, bool year, bool month, bool day)
            {
                CIUtil.WarekiYmd wareki = CIUtil.SDateToShowWDate3(date);
                if (wareki.Ymd != "")
                {
                    if (gengo) SingleData.Add($"{field}Gengo", wareki.Gengo);
                    if (year) SingleData.Add($"{field}Year", wareki.Year.ToString());
                    if (month) SingleData.Add($"{field}Month", wareki.Month.ToString());
                    if (day) SingleData.Add($"{field}Day", wareki.Day.ToString());
                }
            }
            #endregion

            // HEAD印字

            #region 本紙・続紙に関わらず必須
            // 帳票番号
            SingleData.Add("dfPrintNo", getPrintNo());
            // バーコード 患者番号前ゼロ9桁
            SingleData.Add("bcKanID_z9", $"{_coReceipt.PtNum:D9}");
            //漢字氏名
            SingleData.Add("dfPtKanjiName", _coReceipt.PtName);
            #endregion

            if (CurrentPage == 1)
            {
                #region 本紙のみ
                // 患者番号
                SingleData.Add("dfPtNo", _coReceipt.PtNum.ToString());

                // 診療年月
                _printoutWarekiField(_coReceipt.SinYm * 100 + 1, "dfSin", true, true, true, false);
                //生年
                _printoutWarekiField(_coReceipt.BirthDay, "dfBirth", true, true, false, false);
                //性別
                SingleData.Add("dfSex", _getSex());
                //年齢
                SingleData.Add("dfAge", _getAge());
                //受傷日
                _printoutWarekiField(_coReceipt.JibaiJyusyouDate, "dfJyusyou", true, true, true, true);
                //初診日
                _printoutWarekiField(_coReceipt.JibaiSyosinDate, "dfSyosin", true, true, true, true);
                //診療期間自
                _printoutWarekiField(_coReceipt.RyoyoStartDate, "dfSinryoStart", true, true, true, true);
                //診療期間至
                _printoutWarekiField(_coReceipt.RyoyoEndDate, "dfSinryoEnd", true, true, true, true);

                //実日数
                SingleData.Add("dfNissu", CIUtil.ToStringIgnoreNull(_coReceipt.RousaiJituNissu));
                //転帰
                switch (_coReceipt.RousaiTenki)
                {
                    case 1:
                        SingleData.Add("dfTenkiTiyu", "〇");
                        break;
                    case 3:
                        SingleData.Add("dfTenkiKeizoku", "〇");
                        break;
                    case 5:
                        SingleData.Add("dfTenkiTeni", "〇");
                        break;
                    case 7:
                        SingleData.Add("dfTenkiTyusi", "〇");
                        break;
                    case 9:
                        SingleData.Add("dfTenkiSibo", "〇");
                        break;
                }
                // 初診
                SingleData.Add("dfSyosin", _getSyosin());
                // 円点レート
                SingleData.Add("dfEnTen", _coReceipt.EnTen.ToString());

                //ニ（その他）
                SingleData.Add("dfTotal_NI", CIUtil.ToStringIgnoreZero(_coReceipt.JibaiNiFutan));
                //ホ（診断書料）
                SingleData.Add("dfTotal_HO", CIUtil.ToStringIgnoreZero(_coReceipt.JibaiHoSindan));
                //ヘ（明細書料）
                SingleData.Add("dfTotal_HE", CIUtil.ToStringIgnoreZero(_coReceipt.JibaiHeMeisai));

                // 診断書枚数
                SingleData.Add("dfSindanCount", CIUtil.ToStringIgnoreZero(_coReceipt.JibaiHoSindanCount));
                // 明細書枚数
                SingleData.Add("dfMeisaiCount", CIUtil.ToStringIgnoreZero(_coReceipt.JibaiHeMeisaiCount));

                //合計D
                SingleData.Add("dfTotal_D", _coReceipt.JibaiDFutan.ToString());

                //点数合計
                //SingleData.Add("dfTotal_Ten", _coReceipt.HokenReceTensu);
                SingleData.Add("dfTotal_Ten", _coReceipt.JibaiKenpoTensu.ToString());
                //点数x円点レート
                SingleData.Add("dfTotal_EN", _coReceipt.JibaiKenpoFutan.ToString());

                //総合計
                SingleData.Add("dfTotal_ABCD1", (_coReceipt.JibaiKenpoFutan + _coReceipt.JibaiDFutan).ToString());
                SingleData.Add("dfTotal_ABCD2", (_coReceipt.JibaiKenpoFutan + _coReceipt.JibaiDFutan).ToString());

                //通院月
                SingleData.Add("dfTuinMonth", (_coReceipt.SinYm % 100).ToString());
                //通院日
                foreach (int tuuinDay in _coReceipt.TuuinDays)
                {
                    SingleData.Add($"dfDay{tuuinDay % 100}", "〇");
                }
                SingleData.Add("dfDayTotal", _coReceipt.TuuinDays.Count().ToString());

                //保険会社名
                SingleData.Add("dfHokenName", _coReceipt.JibaiHokenName);

                //医療機関住所
                SingleData.Add("txHpAddress", _coReceipt.HpAddress);
                //医療機関名
                SingleData.Add("dfHpName", _coReceipt.ReceHpName);
                //医師名
                SingleData.Add("dfKaisetuName", _coReceipt.KaisetuName);
                //医療機関電話番号
                SingleData.Add("dfHpTel", _coReceipt.HpTel);

                //病名欄

                foreach (CoReceiptByomeiModel byomei in ByomeiModels)
                {
                    var data = new Dictionary<string, CellModel>();
                    data.Add("lsByomei", new CellModel(byomei.Byomei));
                    CellData.Add(data);
                }

                //点数欄
                // 点数
                List<string> tensuSyukeiSakils =
                    new List<string>
                    {
                        "2110",
                        "2310",
                        "2500"
                    };
                foreach (string syukeisaki in tensuSyukeiSakils)
                {
                    SingleData.Add("dfTen_" + syukeisaki, CIUtil.ToStringIgnoreZero(_coReceipt.TenColTensu(syukeisaki)));
                }

                // 回数
                List<List<string>> countSyukeiSakils =
                    new List<List<string>>
                    {
                            new List<string>{ "1200" },
                            new List<string>{ "1220" },
                            new List<string>{ "1230" },
                            new List<string>{ "1240" },
                            new List<string>{ "1250" },
                            new List<string>{ "2100" },
                            new List<string>{ "2110" },
                            new List<string>{ "2200" },
                            new List<string>{ "2300" },
                            new List<string>{ "2310" },
                            new List<string>{ "2500" },
                            new List<string>{ "2600" },
                            new List<string>{ "3110" },
                            new List<string>{ "3210" },
                            new List<string>{ "3310" },
                            new List<string>{ "4000" },
                            new List<string>{ "5000" },
                            new List<string>{ "6000" },
                            new List<string>{ "7000" },
                            new List<string>{ "8000" },
                            new List<string>{ "8010" }
                    };
                foreach (List<string> syukeisaki in countSyukeiSakils)
                {
                    bool onlySI =
                        (syukeisaki.Any(p =>
                            new List<string> { "3110", "3210", "3310", "8000" }.Contains(p)));

                    SingleData.Add("dfCount_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(_coReceipt.TenColCount(syukeisaki, onlySI)));
                }

                // 合計点数
                List<List<string>> totalSyukeiSakils =
                    new List<List<string>>
                    {
                        new List<string>{ "1100", "1110", "1120", "1130", "1140", "1150", "1189" },
                        new List<string>{ "1200" },
                        new List<string>{ "1220" },
                        new List<string>{ "1230" },
                        new List<string>{ "1240" },
                        new List<string>{ "1250" },
                        new List<string>{ "1300" },
                        new List<string>{ "1900" },
                        new List<string>{ "2100" },
                        new List<string>{ "2110" },
                        new List<string>{ "2200" },
                        new List<string>{ "2300" },
                        new List<string>{ "2310" },
                        new List<string>{ "2500" },
                        new List<string>{ "2600" },
                        new List<string>{ "2700" },
                        new List<string>{ "3110" },
                        new List<string>{ "3210" },
                        new List<string>{ "3310" },
                        new List<string>{ "4000" },
                        new List<string>{ "4010" },
                        new List<string>{ "5000" },
                        new List<string>{ "5010" },
                        new List<string>{ "6000" },
                        new List<string>{ "6010" },
                        new List<string>{ "7000" },
                        new List<string>{ "7010" },
                        new List<string>{ "8000" },
                        new List<string>{ "8010" },
                        new List<string>{ "8020" }
                    };

                foreach (List<string> syukeisaki in totalSyukeiSakils)
                {
                    double ten;
                    ten = _coReceipt.TenColTotalTen(syukeisaki);
                    SingleData.Add("dfTotal_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(ten));
                    SingleData.Add("dfTotalEN_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(ten * _coReceipt.EnTen));
                }

                //小計
                double syokei;
                List<List<string>> syokeiSyukeiSakils =
                    new List<List<string>>
                    {
                        new List<string> { "1100", "1110", "1120", "1130", "1140", "1150", "1189", "1200", "1220", "1230", "1240", "1250", "1300", "1900" },
                        new List<string> { "2100", "2110", "2200", "2300", "2310", "2500", "2600", "2700" },
                        new List<string> { "3110", "3210", "3310" },
                        new List<string> { "4000", "4010" },
                        new List<string> { "5000", "5010" },
                        new List<string> { "6000", "6010" },
                        new List<string> { "7000", "7010" },
                        new List<string> { "8000", "8010", "8020" }
                    };
                foreach (List<string> syukeisaki in syokeiSyukeiSakils)
                {
                    syokei = _coReceipt.TenColTotalTen(syukeisaki);
                    SingleData.Add("dfSyokei_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(syokei));
                    SingleData.Add("dfSyokeiEN_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(syokei * _coReceipt.EnTen));
                }

                // 初診
                List<(string syukeiSaki, string field)> syosinSyukeisaki =
                    new List<(string, string)>
                    {
                        (ReceSyukeisaki.SyosinJikanGai, "dfSyosinJikanGai"),
                        (ReceSyukeisaki.SyosinKyujitu, "dfSyosinKyujitu"),
                        (ReceSyukeisaki.SyosinSinya, "dfSyosinSinya")
                    };

                for (int j = 0; j < syosinSyukeisaki.Count; j++)
                {
                    if (_coReceipt.TenColTensu(syosinSyukeisaki[j].syukeiSaki) > 0)
                    {
                        SingleData.Add(syosinSyukeisaki[j].field, "〇");
                    }
                }
                #endregion
            }
            else
            {
                #region 続紙のみ
                if (Target == TargetConst.RousaiTanki)
                {
                    //労災交付番号
                    SingleData.Add("dfRousaiRoudouNo", _coReceipt.RousaiKofu);
                }
                else
                {
                    //労災交付番号
                    SingleData.Add("dfRousaiNenkinNo", _coReceipt.RousaiKofu);
                }
                #endregion
            }
        }

        /// <summary>
        /// 自賠責（労災準拠）用ヘッダー印字処理
        /// </summary>
        private void PrintReceiptHeaderForJibaiRousai()
        {
            #region Sub Function

            // 性別
            string _getSex()
            {
                string ret = "男";
                if (_coReceipt.Sex == 2)
                {
                    ret = "女";
                }
                return ret;
            }
            // 年齢
            string _getAge()
            {
                string ret = CIUtil.SDateToAge(_coReceipt.BirthDay, CIUtil.GetLastDateOfMonth(_coReceipt.SinYm * 100 + 1)).ToString();
                return ret;
            }
            // 初診
            string _getSyosin()
            {
                List<(string syukeiSaki, string comment)> syosinSyukeisaki =
                    new List<(string, string)>
                    {
                                            (ReceSyukeisaki.SyosinJikanGai, "時間外"),
                                            (ReceSyukeisaki.SyosinKyujitu, "休日"),
                                            (ReceSyukeisaki.SyosinSinya, "深夜")
                    };
                string Syosin = "";

                for (int j = 0; j < syosinSyukeisaki.Count; j++)
                {
                    if (_coReceipt.TenColTensu(syosinSyukeisaki[j].syukeiSaki) > 0)
                    {
                        if (Syosin.Contains(syosinSyukeisaki[j].comment) == false)
                        {
                            if (Syosin != "") Syosin += ",";
                            Syosin += syosinSyukeisaki[j].comment;
                        }
                    }
                }

                return Syosin;
            }
            // 西暦(yyyymmdd）を和暦に変換して各フィールドに印字する
            void _printoutWarekiField(int date, string field, bool gengo, bool year, bool month, bool day)
            {
                CIUtil.WarekiYmd wareki = CIUtil.SDateToShowWDate3(date);
                if (wareki.Ymd != "")
                {
                    if (gengo) SingleData.Add($"{field}Gengo", wareki.Gengo);
                    if (year) SingleData.Add($"{field}Year", wareki.Year.ToString());
                    if (month) SingleData.Add($"{field}Month", wareki.Month.ToString());
                    if (day) SingleData.Add($"{field}Day", wareki.Day.ToString());
                }
            }
            #endregion

            // HEAD印字

            #region 本紙・続紙に関わらず必須
            // 帳票番号
            SingleData.Add("dfPrintNo", getPrintNo());
            // バーコード 患者番号前ゼロ9桁
            SingleData.Add("bcKanID_z9", $"{_coReceipt.PtNum:D9}");
            //漢字氏名
            SingleData.Add("dfPtKanjiName", _coReceipt.PtName);
            #endregion

            if (CurrentPage == 1)
            {
                #region 本紙のみ
                // 患者番号
                SingleData.Add("dfPtNo", _coReceipt.PtNum.ToString());
                // 診療年月
                _printoutWarekiField(_coReceipt.SinYm * 100 + 1, "dfSin", true, true, true, false);
                //生年
                _printoutWarekiField(_coReceipt.BirthDay, "dfBirth", true, true, false, false);

                //性別
                SingleData.Add("dfSex", _getSex());
                //年齢
                SingleData.Add("dfAge", _getAge());
                //受傷日
                _printoutWarekiField(_coReceipt.JibaiJyusyouDate, "dfJyusyou", true, true, true, true);
                //初診日
                _printoutWarekiField(_coReceipt.JibaiSyosinDate, "dfSyosin", true, true, true, true);
                //診療期間自
                _printoutWarekiField(_coReceipt.RyoyoStartDate, "dfSinryoStart", true, true, true, true);
                //診療期間至
                _printoutWarekiField(_coReceipt.RyoyoEndDate, "dfSinryoEnd", true, true, true, true);
                //実日数
                SingleData.Add("dfNissu", CIUtil.ToStringIgnoreNull(_coReceipt.RousaiJituNissu));
                //転帰
                switch (_coReceipt.RousaiTenki)
                {
                    case 1:
                        SingleData.Add("dfTenkiTiyu", "〇");
                        break;
                    case 3:
                        SingleData.Add("dfTenkiKeizoku", "〇");
                        break;
                    case 5:
                        SingleData.Add("dfTenkiTeni", "〇");
                        break;
                    case 7:
                        SingleData.Add("dfTenkiTyusi", "〇");
                        break;
                    case 9:
                        SingleData.Add("dfTenkiSibo", "〇");
                        break;
                }
                // 初診
                SingleData.Add("dfSyosin", _getSyosin());
                //イ
                SingleData.Add("dfTotal_I", CIUtil.ToStringIgnoreZero(_coReceipt.JibaiITensu));
                //ロ
                SingleData.Add("dfTotal_RO", CIUtil.ToStringIgnoreZero(_coReceipt.JibaiRoTensu));
                //ハ
                SingleData.Add("dfTotal_HA", CIUtil.ToStringIgnoreZero(_coReceipt.JibaiHaFutan));
                //ニ（その他）
                SingleData.Add("dfTotal_NI", CIUtil.ToStringIgnoreZero(_coReceipt.JibaiNiFutan));
                //ホ（診断書料）
                SingleData.Add("dfTotal_HO", CIUtil.ToStringIgnoreZero(_coReceipt.JibaiHoSindan));
                //ヘ（明細書料）
                SingleData.Add("dfTotal_HE", CIUtil.ToStringIgnoreZero(_coReceipt.JibaiHeMeisai));
                // 診断書枚数
                SingleData.Add("dfSindanCount", CIUtil.ToStringIgnoreZero(_coReceipt.JibaiHoSindanCount));
                // 明細書枚数
                SingleData.Add("dfMeisaiCount", CIUtil.ToStringIgnoreZero(_coReceipt.JibaiHeMeisaiCount));

                //加算率
                SingleData.Add("dfRateA", _systemConfRepository.GetSettingValue(3001, 1, HpId).ToString());
                SingleData.Add("dfRateC", _systemConfRepository.GetSettingValue(3001, 1, HpId).ToString());
                //合計A
                SingleData.Add("dfTotal_A", _coReceipt.JibaiAFutan.ToString());
                //合計B
                SingleData.Add("dfTotal_B", _coReceipt.JibaiBFutan.ToString());
                //合計C
                SingleData.Add("dfTotal_C", _coReceipt.JibaiCFutan.ToString());
                //合計D
                SingleData.Add("dfTotal_D", _coReceipt.JibaiDFutan.ToString());
                //合計ABCD
                SingleData.Add("dfTotal_ABCD1", _coReceipt.JibaiABCDFutan.ToString());
                SingleData.Add("dfTotal_ABCD2", _coReceipt.JibaiABCDFutan.ToString());
                //通院月
                SingleData.Add("dfTuinMonth", (_coReceipt.SinYm % 100).ToString());
                //通院日
                foreach (int tuuinDay in _coReceipt.TuuinDays)
                {
                    SingleData.Add($"dfDay{tuuinDay % 100}", "〇");
                }
                SingleData.Add("dfDayTotal", _coReceipt.TuuinDays.Count().ToString());
                //保険会社名
                SingleData.Add("dfHokenName", _coReceipt.JibaiHokenName);

                //医療機関住所
                SingleData.Add("txHpAddress", _coReceipt.HpAddress);
                //医療機関名
                SingleData.Add("dfHpName", _coReceipt.ReceHpName);
                //医師名
                SingleData.Add("dfKaisetuName", _coReceipt.KaisetuName);
                //医療機関電話番号
                SingleData.Add("dfHpTel", _coReceipt.HpTel);

                //病名欄
                foreach (CoReceiptByomeiModel byomei in ByomeiModels)
                {
                    var data = new Dictionary<string, CellModel>();
                    data.Add("lsByomei", new CellModel(byomei.Byomei));

                    CellData.Add(data);
                }

                //点数欄
                // 点数
                List<string> tensuSyukeiSakils =
                    new List<string>
                    {
                        "2110",
                        "2310",
                        "2500"
                    };
                foreach (string syukeisaki in tensuSyukeiSakils)
                {
                    SingleData.Add("dfTen_" + syukeisaki, CIUtil.ToStringIgnoreZero(_coReceipt.TenColTensu(syukeisaki)));
                }

                // 回数
                List<List<string>> countSyukeiSakils =
                    new List<List<string>>
                    {
                        new List<string>{ "1220", "1221" },
                        new List<string>{ "1230" },
                        new List<string>{ "1240" },
                        new List<string>{ "1250" },
                        new List<string>{ "2100" },
                        new List<string>{ "2110" },
                        new List<string>{ "2200" },
                        new List<string>{ "2300" },
                        new List<string>{ "2310" },
                        new List<string>{ "2500" },
                        new List<string>{ "2600" },
                        new List<string>{ "2700" },
                        new List<string>{ "3110" },
                        new List<string>{ "3210" },
                        new List<string>{ "3310" },
                        new List<string>{ "4000" },
                        new List<string>{ "5000" },
                        new List<string>{ "6000" },
                        new List<string>{ "7000" },
                        new List<string>{ "8000" },
                        new List<string>{ "A110" },
                        new List<string>{ "A120" },
                        new List<string>{ "A130" }
                    };
                foreach (List<string> syukeisaki in countSyukeiSakils)
                {
                    bool onlySI =
                        (syukeisaki.Any(p =>
                            new List<string> { "3110", "3210", "3310", "8000" }.Contains(p)));

                    SingleData.Add("dfCount_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(_coReceipt.TenColCount(syukeisaki, onlySI)));
                }

                // 合計点数
                List<List<string>> totalSyukeiSakils =
                    new List<List<string>>
                    {
                        new List<string>{ "1100", "1110", "1120", "1130", "1140", "1150", "1189" },
                        new List<string>{ "1220", "1221" },
                        new List<string>{ "1230" },
                        new List<string>{ "1240" },
                        new List<string>{ "1250" },
                        new List<string>{ "1300" },
                        new List<string>{ "1900" },
                        new List<string>{ "2100" },
                        new List<string>{ "2110" },
                        new List<string>{ "2200" },
                        new List<string>{ "2300" },
                        new List<string>{ "2310" },
                        new List<string>{ "2500" },
                        new List<string>{ "2600" },
                        new List<string>{ "2700" },
                        new List<string>{ "3110" },
                        new List<string>{ "3210" },
                        new List<string>{ "3310" },
                        new List<string>{ "3900" },
                        new List<string>{ "4000" },
                        new List<string>{ "4010" },
                        new List<string>{ "5000" },
                        new List<string>{ "5010" },
                        new List<string>{ "6000" },
                        new List<string>{ "6010" },
                        new List<string>{ "7000" },
                        new List<string>{ "7010" },
                        new List<string>{ "8000" },
                        new List<string>{ "8010" },
                        new List<string>{ "8020" },
                        new List<string>{ "A110" },
                        new List<string>{ "A120" },
                        new List<string>{ "A130" },
                        new List<string>{ "A131" }
                    };
                foreach (List<string> syukeisaki in totalSyukeiSakils)
                {
                    SingleData.Add("dfTotal_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(_coReceipt.TenColTotalTen(syukeisaki)));
                }

                //小計
                List<List<string>> syokeiSyukeiSakils =
                    new List<List<string>>
                    {
                        new List<string>{ "1100", "1110", "1120", "1130", "1140", "1150", "1189", "1220", "1221", "1230", "1240", "1250", "1300", "1900" },
                        new List<string>{ "2110", "2310", "2500", "2600", "2700" },
                        new List<string>{ "2100", "2200", "2300" },
                        new List<string>{ "3110", "3210", "3310" },
                        new List<string>{ "3900" }
                    };
                foreach (List<string> syukeisaki in syokeiSyukeiSakils)
                {
                    SingleData.Add("dfSyokei_" + syukeisaki[0], CIUtil.ToStringIgnoreZero(_coReceipt.TenColTotalTen(syukeisaki)));
                }

                // 初診
                List<(string syukeiSaki, string field)> syosinSyukeisaki =
                    new List<(string, string)>
                    {
                        (ReceSyukeisaki.SyosinJikanGai, "dfSyosinJikanGai"),
                        (ReceSyukeisaki.SyosinKyujitu, "dfSyosinKyujitu"),
                        (ReceSyukeisaki.SyosinSinya, "dfSyosinSinya")
                    };

                for (int j = 0; j < syosinSyukeisaki.Count; j++)
                {
                    if (_coReceipt.TenColTensu(syosinSyukeisaki[j].syukeiSaki) > 0)
                    {
                        SingleData.Add(syosinSyukeisaki[j].field, "〇");
                    }
                }
                #endregion
            }
            else
            {
                #region 続紙のみ
                if (Target == TargetConst.RousaiTanki)
                {
                    //労災交付番号
                    SingleData.Add("dfRousaiRoudouNo", _coReceipt.RousaiKofu);
                }
                else
                {
                    //労災交付番号
                    SingleData.Add("dfRousaiNenkinNo", _coReceipt.RousaiKofu);
                }
                #endregion
            }
        }

        private string getPrintNo()
        {
            string ret = _coReceipt.ReceiptNo.ToString();
            if (_coReceipt.Output == 1)
            {
                // 印刷済み
                ret = "R" + ret;
            }

            if (_coReceipt.SeikyuKbn == SeikyuKbnConst.Henrei)
            {
                // 返戻
                ret += "(H)";
            }

            return ret;
        }
    }
}
