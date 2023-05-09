using Helper.Common;
using Reporting.CommonMasters.Config;
using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.Sijisen.Model;
using System.Text;

namespace Reporting.Sijisen.Mapper
{
    public class SijisenMapper : CommonReportingRequest
    {
        private readonly List<CoSijisenPrintDataModel> _printOutData = new List<CoSijisenPrintDataModel>();
        private readonly CoSijisenModel _coSijisen;
        private readonly List<CoRaiinKbnMstModel> _raiinKbnMstList;
        private readonly DateTime _printoutDateTime;
        private readonly int _formType;
        private readonly ISystemConfig _systemConfig;
        private readonly string _jobName;

        private const int _dataCharCount = 68;
        private const int _suryoCharCount = 9;
        private const int _unitCharCount = 8;
        private const int _dataRowCount = 40;

        public SijisenMapper(int formType, CoSijisenModel coSijisen, List<CoRaiinKbnMstModel> raiinKbnMstList, ISystemConfig systemConfig, string jobName)
        {
            _coSijisen = coSijisen;
            _raiinKbnMstList = raiinKbnMstList;
            _printoutDateTime = DateTime.Now;
            _formType = formType;
            _systemConfig = systemConfig;
            _jobName = jobName;
        }

        #region override function

        public override Dictionary<string, bool> GetVisibleFieldData()
        {
            return new Dictionary<string, bool>();
        }

        public override Dictionary<string, bool> GetWrapFieldData()
        {
            return new Dictionary<string, bool>();
        }

        public override Dictionary<string, string> GetSingleFieldData()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            var coModel = _coSijisen;

            #region print method
            // 発行日
            void _printPrintDate()
            {
                data.Add("dfPrintDateS", _printoutDateTime.ToString("yyyy/MM/dd"));
                data.Add("dfPrintDateW", CIUtil.SDateToShowWDate3(CIUtil.DateTimeToInt(_printoutDateTime)).Ymd);
            }
            // 発行時間
            void _printPrintTime()
            {
                data.Add("dfPrintTime", _printoutDateTime.ToString("HH:mm"));
            }
            // 発行日時
            void _printPrintDateTime()
            {
                data.Add("dfPrintDateTimeS", _printoutDateTime.ToString("yyyy/MM/dd HH:mm"));
                data.Add("dfPrintDateTimeW", CIUtil.SDateToShowWDate3(CIUtil.DateTimeToInt(_printoutDateTime)).Ymd + _printoutDateTime.ToString(" HH:mm"));
                // 下位互換
                data.Add("TimeStmp", _printoutDateTime.ToString("yyyy/MM/dd HH:mm"));
            }
            //// ページ
            //void _printPage()
            //{
            //    CoRep.SetFieldData("dfPage", CurrentPage);
            //    // 下位互換
            //    CoRep.SetFieldData("Page", CurrentPage);
            //}
            // 患者番号
            void _printPtNum()
            {
                data.Add("dfPtNum", coModel.PtNum.ToString());
                // 下位互換
                data.Add("KanId", coModel.PtNum.ToString());
            }
            // 患者番号バーコード
            void _printBcPtNum()
            {
                data.Add("bcPtNum", coModel.PtNum.ToString());
                data.Add("bcPtNum9", $"{coModel.PtNum:D9}");
                // 下位互換
                data.Add("KanID_BC", coModel.PtNum.ToString());
                data.Add("KanID_BC9", $"{coModel.PtNum:D9}");
            }
            // 患者カナ氏名
            void _printPtKanaName()
            {
                data.Add("dfPtKanaName", coModel.PtKanaName);
                // 下位互換
                data.Add("KanKana", coModel.PtKanaName);
            }
            // 患者氏名
            void _printPtName()
            {
                data.Add("dfPtName", coModel.PtName);
                // 下位互換
                data.Add("KanNm", coModel.PtName);
            }
            // 性別
            void _printSex()
            {
                data.Add("dfSex", coModel.PtSex);
                // 下位互換
                data.Add("KanSex", coModel.PtSex);
            }

            // 生年月日
            void _printBirthDay()
            {
                data.Add("dfBirthDayS", CIUtil.SDateToShowSDate(coModel.BirthDay));
                data.Add("dfBirthDayW", CIUtil.SDateToShowWDate3(coModel.BirthDay).Ymd);
                // 下位互換
                data.Add("KanBirth", $"{CIUtil.SDateToShowWDate3(coModel.BirthDay).Ymd} ({coModel.Age}歳)");
            }

            // 年齢
            void _printAge()
            {
                data.Add("dfAge", coModel.Age.ToString());
            }
            void _printAgeDsp()
            {
                data.Add("dfAgeDsp", coModel.AgeDsp);
            }
            void _printAgeDspMonth()
            {
                data.Add("dfAgeDspMonth", coModel.AgeDspMonth);
            }
            void _printAgeDspDay()
            {
                data.Add("dfAgeDspDay", coModel.AgeDspDay);
            }
            // 郵便番号
            void _printPostCd()
            {
                data.Add("dfPostCd", coModel.PtPostCdDsp);
            }

            // 患者住所
            void _printAddress()
            {
                data.Add("dfAddress", coModel.PtAddress);
            }

            // 電話番号
            void _printTel()
            {
                data.Add("dfTel", coModel.PtTel);
            }

            // 患者コメント
            void _printPtCmt()
            {
                data.Add("dfPtCmt", coModel.PtCmt);
            }

            // 受付番号
            void _printUketukeNo()
            {
                data.Add("dfUketukeNo", coModel.UketukeNo.ToString());
            }

            // 受付種別
            void _printUketukeSbt()
            {
                data.Add("dfUketukeKbn", coModel.UketukeSbtName);
            }

            // 受付区分（同日他来院の分）
            void _printUketukeSbtOther()
            {
                data.Add("dfUketukeKbnEtc", coModel.UketukeSbtNameEtc);
            }

            // 来院区分タイトル
            void _printRaiinKbnTitle()
            {
                foreach (CoRaiinKbnMstModel raiinKbnMst in _raiinKbnMstList)
                {
                    data.Add($"dfRaiinKbnTitle{raiinKbnMst.GrpCd}", raiinKbnMst.GrpName);
                    data.Add($"dfRaiinKbnTitle{raiinKbnMst.GrpCd}_2", raiinKbnMst.GrpName);
                }
            }

            // 来院区分
            void _printRaiinKbn()
            {
                foreach (CoRaiinKbnInfModel raiinKbnInf in coModel.RaiinKbnInfModels)
                {
                    data.Add($"dfRaiinKbn{raiinKbnInf.GrpId}", raiinKbnInf.KbnName);
                    data.Add($"dfRaiinKbn{raiinKbnInf.GrpId}_2", raiinKbnInf.KbnName);
                }
            }

            // 受付時間
            void _printUketukeTime()
            {
                data.Add("dfUketukeTime", coModel.UketukeTime);
            }

            // 診療科
            void _printKaName()
            {
                data.Add("dfKa", coModel.KaName);
                // 下位互換
                data.Add("SkaNm", coModel.KaName);
            }

            // 担当医
            void _printTantoName()
            {
                data.Add("dfTanto", coModel.TantoName);
                // 下位互換
                data.Add("Tanto", coModel.TantoName);
            }

            // 来院コメント
            void _printRaiinCmt()
            {
                data.Add("dfRaiinCmt", coModel.RaiinCmt);
            }

            // 予約時間
            void _printYoyakuTime()
            {
                data.Add("dfYoyakuTime", coModel.YoyakuTime);
            }
            // JunNavi順番
            void _printJunNaviUketukeNo()
            {
                if (coModel.RaiinCmt.Length == 3 && CIUtil.StrToIntDef(coModel.RaiinCmt, 0) > 0)
                {
                    data.Add("dfJunBango", coModel.RaiinCmt);
                    // 下位互換
                    data.Add("JunBango", coModel.RaiinCmt);
                }
            }

            // 最終来院日
            void _printLastSinDate()
            {
                data.Add("dfLastSinDateS", CIUtil.SDateToShowSDate(coModel.LastSinDate));
                data.Add("dfLastSinDateW", CIUtil.SDateToShowWDate3(coModel.LastSinDate).Ymd);
                // 下位互換
                data.Add("LastSinDay", CIUtil.SDateToShowSDate(coModel.LastSinDate));
            }
            #endregion

            // 発行日
            _printPrintDate();

            // 発行時間
            _printPrintTime();

            // 発行日時
            _printPrintDateTime();

            // ページ
            //_printPage();

            // 患者番号
            _printPtNum();

            // 患者番号バーコード
            _printBcPtNum();

            // 患者カナ氏名
            _printPtKanaName();

            // 患者氏名
            _printPtName();

            // 性別
            _printSex();

            // 生年月日
            _printBirthDay();

            // 年齢
            _printAge();
            _printAgeDsp();
            _printAgeDspMonth();
            _printAgeDspDay();

            // 患者住所郵便番号
            _printPostCd();

            // 患者住所
            _printAddress();

            // 電話番号
            _printTel();

            // 患者コメント
            _printPtCmt();

            // 受付番号
            _printUketukeNo();

            // 受付種別
            _printUketukeSbt();

            // 受付区分（同日他来院の分）
            _printUketukeSbtOther();

            // 来院区分タイトル
            _printRaiinKbnTitle();

            // 来院区分
            _printRaiinKbn();

            // 受付時間
            _printUketukeTime();

            // 診療科
            _printKaName();

            // 担当医
            _printTantoName();

            // 来院コメント
            _printRaiinCmt();

            // 予約時間
            _printYoyakuTime();

            // JunNavi順番
            _printJunNaviUketukeNo();

            // 最終来院日
            _printLastSinDate();

            return data;
        }

        public override List<Dictionary<string, CellModel>> GetTableFieldData()
        {
            MakeOdrDtlList();

            List<Dictionary<string, CellModel>> result = new List<Dictionary<string, CellModel>>();

            foreach (var item in _printOutData)
            {
                Dictionary<string, CellModel> data = new Dictionary<string, CellModel>
                {
                    { "lsOdrKbn", new CellModel(item.Sikyu) },
                    { "lsOrder", new CellModel(item.Data, item.UnderLine) },
                    { "lsSuryo", new CellModel(item.Suuryo) },
                    { "lsTani", new CellModel(item.Tani) }
                };

                result.Add(data);
            }

            return result;
        }

        public override int GetReportType()
        {
            if (_formType == (int)CoSijisenFormType.Sijisen)
            {
                return (int)CoReportType.Sijisen;
            }
            else
            {
                return (int)CoReportType.JyusinHyo;
            }
        }

        public override string GetRowCountFieldName()
        {
            return "lsOrder";
        }

        public override string GetJobName()
        {
            return _jobName;
        }

        #endregion

        #region helper
        /// <summary>
        /// オーダーリスト生成
        /// </summary>
        private void MakeOdrDtlList()
        {
            const string conAst = "＊";
            const string conNoAst = "　";
            string preKouiName = "";
            HashSet<string> kensaContainers = new HashSet<string>();
            List<CoSijisenPrintDataModel> addPrintOutData;


            //foreach(CoOdrInfModel odrInf in coModel.OdrInfModels)
            for (int i = 0; i < _coSijisen.OdrInfModels.Count; i++)
            {
                CoCommonOdrInfModel odrInf = _coSijisen.OdrInfModels[i];

                addPrintOutData = new List<CoSijisenPrintDataModel>();

                // 行為名を除くRp先頭行のみ*を付ける
                string ast = conAst;

                string kouiName = _getKouiName(odrInf.OdrKouiKbn, odrInf.InoutKbn);
                if (preKouiName != kouiName)
                {
                    addPrintOutData.AddRange(_addList(kouiName, _dataCharCount));

                    addPrintOutData.Last().UnderLine = true;
                    preKouiName = kouiName;
                }

                if (!string.IsNullOrEmpty(odrInf.RpName) &&
                    (
                        (_formType == (int)CoSijisenFormType.Sijisen && _systemConfig.SijisenRpName() == 1) ||
                        (_formType == (int)CoSijisenFormType.JyusinHyo && _systemConfig.JyusinHyoRpName() == 1)
                    )
                  )
                {
                    addPrintOutData.AddRange(_addList($"【{odrInf.RpName}】", _dataCharCount, ast));
                    ast = conNoAst;

                    string sikyu = _getSikyu(odrInf.SikyuKbn, odrInf.TosekiKbn);

                    if (sikyu != "")
                    {
                        addPrintOutData.Last().Sikyu = sikyu;
                    }
                }

                foreach (CoCommonOdrInfDetailModel odrDtl in
                    _coSijisen.OdrInfDetailModels.FindAll(p => p.RaiinNo == odrInf.RaiinNo && p.RpNo == odrInf.RpNo && p.RpEdaNo == odrInf.RpEdaNo))
                {
                    if (string.IsNullOrEmpty(odrDtl.ItemCd) || odrDtl.ItemCd.StartsWith("C") || (odrDtl.ItemCd.StartsWith("8") && odrDtl.ItemCd.Length == 9))
                    {
                        // コメント
                        addPrintOutData.AddRange(_addList(odrDtl.ItemName, _dataCharCount, ast));
                    }
                    else
                    {
                        string itemName = odrDtl.ItemName;



                        if (odrDtl.ItemCd == "@BUNKATU")
                        {
                            itemName += TenUtils.GetBunkatu(odrInf.OdrKouiKbn, odrDtl.Bunkatu);
                        }
                        else if (odrDtl.ItemCd == "@SHIN")
                        {
                            // 初再診
                            switch (odrDtl.Suryo)
                            {
                                case 0:
                                    itemName = "初再診なし";
                                    break;
                                case 1:
                                    itemName = "初診";
                                    break;
                                case 3:
                                    itemName = "再診";
                                    break;
                                case 4:
                                    itemName = "電話再診";
                                    break;
                                case 5:
                                    itemName = "医科計算なし";
                                    break;
                                case 6:
                                    itemName = "初診２科目";
                                    break;
                                case 7:
                                    itemName = "再診２科目";
                                    break;
                                case 8:
                                    itemName = "電話再診２科目";
                                    break;
                                default:
                                    itemName = "";
                                    break;
                            }
                        }
                        else if (odrDtl.ItemCd == "@JIKAN")
                        {
                            // 時間枠
                            if (odrDtl.Suryo == 0) continue;

                            switch (odrDtl.Suryo)
                            {
                                case 1:
                                    itemName = "時間外";
                                    break;
                                case 2:
                                    itemName = "休日";
                                    break;
                                case 3:
                                    itemName = "深夜";
                                    break;
                                case 4:
                                    itemName = "夜・早";
                                    break;
                            }
                        }

                        addPrintOutData.AddRange(_addList(itemName, _dataCharCount - _suryoCharCount - _unitCharCount, ast));

                        if (odrInf.OdrKouiKbn >= 60 && odrInf.OdrKouiKbn <= 69 && odrDtl.ContainerName != "")
                        {
                            kensaContainers.Add(odrDtl.ContainerName);
                        }
                    }

                    if (ast == conAst)
                    {
                        // ast != "" ということは、このRp最初の項目
                        // このとき、至急チェックを行う
                        string sikyu = _getSikyu(odrInf.SikyuKbn, odrInf.TosekiKbn);

                        if (sikyu != "")
                        {
                            addPrintOutData.Last().Sikyu = sikyu;
                        }
                    }

                    ast = conNoAst;

                    if (!string.IsNullOrEmpty(odrDtl.UnitName))
                    {
                        addPrintOutData.Last().Suuryo = odrDtl.Suryo.ToString();
                        addPrintOutData.Last().Tani = odrDtl.UnitName;
                    }
                }

                _printOutData.AddRange(_appendBlankRows(addPrintOutData.Count));
                _printOutData.AddRange(addPrintOutData);

                if (_dataRowCount - (_printOutData.Count % _dataRowCount) > 0)
                {
                    // 1行空ける
                    _printOutData.Add(new CoSijisenPrintDataModel());
                }

            }

            // アレルギー
            List<CoSijisenPrintDataModel> addAlrgyData = new List<CoSijisenPrintDataModel>();
            if ((_formType == (int)CoSijisenFormType.Sijisen && _systemConfig.SijisenAlrgy() == 0) ||
                    (_formType == (int)CoSijisenFormType.JyusinHyo && _systemConfig.JyusinHyoAlrgy() == 0))
            {
                addAlrgyData = _getAlrgyList();
            }


            // 患者コメント
            List<CoSijisenPrintDataModel> addPtCmtData = new List<CoSijisenPrintDataModel>();
            if ((_formType == (int)CoSijisenFormType.Sijisen && _systemConfig.SijisenPtCmt() == 0) ||
                    (_formType == (int)CoSijisenFormType.JyusinHyo && _systemConfig.JyusinHyoPtCmt() == 0))
            {
                addPtCmtData = _getPtCmtList();
            }

            // 検査容器
            List<CoSijisenPrintDataModel> addYokiData = new List<CoSijisenPrintDataModel>();
            if (
                (
                    (_formType == (int)CoSijisenFormType.Sijisen && _systemConfig.SijisenKensaYokiZairyo() == 1) ||
                    (_formType == (int)CoSijisenFormType.JyusinHyo && _systemConfig.JyusinHyoKensaYokiZairyo() == 1)
                ) && kensaContainers.Any())
            {
                StringBuilder containers = new();

                foreach (string container in kensaContainers)
                {
                    if (containers.Length > 0)
                        containers.Append(",");
                    containers.Append(container);
                }

                if (containers.Length > 0)
                {

                    addYokiData.Add(new CoSijisenPrintDataModel());
                    addYokiData.Last().Data = "[容器情報]";

                    addYokiData.AddRange(_addList(containers.ToString(), _dataCharCount));
                }
            }

            // アレルギー、患者コメント、検査容器情報を追加する
            if (addAlrgyData.Any() || addPtCmtData.Any() || addYokiData.Any())
            {
                addPrintOutData = new List<CoSijisenPrintDataModel>();

                if (_dataRowCount - (_printOutData.Count % _dataRowCount) > 0)
                {
                    _printOutData.Add(new CoSijisenPrintDataModel());
                    _printOutData.Last().Data = new string('ー', _dataCharCount);
                }

                if (addAlrgyData.Any())
                {
                    _printOutData.AddRange(addAlrgyData);

                    if ((addPtCmtData.Any() || addYokiData.Any()) && _dataRowCount - (_printOutData.Count % _dataRowCount) > 0)
                    {
                        _printOutData.Add(new CoSijisenPrintDataModel());
                    }
                }

                if (addPtCmtData.Any())
                {
                    _printOutData.AddRange(addPtCmtData);

                    if (addYokiData.Any() && _dataRowCount - (_printOutData.Count % _dataRowCount) > 0)
                    {
                        _printOutData.Add(new CoSijisenPrintDataModel());
                    }
                }

                if (addYokiData.Any())
                {
                    _printOutData.AddRange(addYokiData);
                }
            }
        }

        List<CoSijisenPrintDataModel> _getAlrgyList()
        {
            List<string> alrgys = _coSijisen.Alrgy;
            List<CoSijisenPrintDataModel> addPrintOutData = new List<CoSijisenPrintDataModel>();

            if (alrgys.Any())
            {
                addPrintOutData.Add(new CoSijisenPrintDataModel());
                addPrintOutData.Last().Data = "[アレルギー情報]";

                foreach (string alrgy in alrgys)
                {
                    addPrintOutData.AddRange(_addList(alrgy, _dataCharCount));
                }
            }

            return addPrintOutData;
        }

        List<CoSijisenPrintDataModel> _getPtCmtList()
        {
            List<string> ptCmts = _coSijisen.PtCmtList;
            List<CoSijisenPrintDataModel> addPrintOutData = new List<CoSijisenPrintDataModel>();

            if (ptCmts.Any())
            {
                addPrintOutData.Add(new CoSijisenPrintDataModel());
                addPrintOutData.Last().Data = "[コメント]";

                foreach (string ptCmt in ptCmts)
                {
                    addPrintOutData.AddRange(_addList(ptCmt, _dataCharCount));
                }
            }

            return addPrintOutData;
        }

        List<CoSijisenPrintDataModel> _appendBlankRows(int addRowCount)
        {
            List<CoSijisenPrintDataModel> addPrintOutData = new List<CoSijisenPrintDataModel>();

            if ((addRowCount + _getPrintedLineCount()) > _dataRowCount)
            {
                // 追加する行数 + このページの印字済み行数 > 1ページの最大行数(最終Rpの場合は0, その他は-1する）
                // つまり、このRpのデータを追加すると、ページの行数を超えてしまう場合、
                // 区切りの文字と残り行を埋める空行を追加する
                // このRpのデータは次ページに印字する

                // 追加する行数を決定する
                int appendRowCount = _getRemainingLineCount();
                if (appendRowCount % _dataRowCount != 0)
                {
                    for (int j = 0; j < appendRowCount; j++)
                    {
                        // 空行で埋める
                        addPrintOutData.Add(new CoSijisenPrintDataModel());
                    }
                }
            }

            return addPrintOutData;
        }

        /// <summary>
        /// このページの印字済み行数
        /// 既に追加した行数 % 1ページの最大行数
        /// </summary>
        /// <returns></returns>
        int _getPrintedLineCount()
        {
            return _printOutData.Count % _dataRowCount;
        }

        /// <summary>
        /// このページに印字可能な残り行数
        /// 1ページの最大行数 - (既に追加した行数 % 1ページの最大行数)
        /// </summary>
        /// <returns></returns>
        int _getRemainingLineCount()
        {
            return _dataRowCount - _getPrintedLineCount();
        }

        /// <summary>
        /// オーダー行為区分から行為名称を取得する
        /// </summary>

        private string _getKouiName(int odrKouiKbn, int inoutKbn)
        {
            string ret = "";
            string[] inout = new string[] { "（院内）", "（院外）" };

            if (odrKouiKbn >= 10 && odrKouiKbn <= 12)
            {
                ret = "初再診";
            }
            else if (odrKouiKbn == 13)
            {
                ret = "医学管理";
            }
            else if (odrKouiKbn == 14)
            {
                ret = "在宅";
            }
            else if ((odrKouiKbn >= 20 && odrKouiKbn <= 29) || (odrKouiKbn >= 100 && odrKouiKbn <= 101))
            {
                ret = "投薬" + inout[inoutKbn];
            }
            else if (odrKouiKbn >= 30 && odrKouiKbn <= 39)
            {
                ret = "注射";
            }
            else if (odrKouiKbn >= 40 && odrKouiKbn <= 49)
            {
                ret = "処置";
            }
            else if (odrKouiKbn >= 50 && odrKouiKbn <= 59)
            {
                ret = "手術";
            }
            else if (odrKouiKbn >= 60 && odrKouiKbn <= 69)
            {
                ret = "検査" + inout[inoutKbn];
            }
            else if (odrKouiKbn >= 70 && odrKouiKbn <= 79)
            {
                ret = "画像";
            }
            else if (odrKouiKbn >= 80 && odrKouiKbn <= 89)
            {
                ret = "その他";
            }
            else if (odrKouiKbn == 96)
            {
                ret = "自費";
            }

            return ret;
        }
        /// <summary>
        /// 至急区分を取得する
        /// </summary>
        /// <param name="sikyuKbn"></param>
        /// <param name="tosekiKbn"></param>
        /// <returns></returns>
        private string _getSikyu(int sikyuKbn, int tosekiKbn)
        {
            string sikyu = "";
            if (tosekiKbn == 1)
            {
                sikyu = "透前";
            }
            else if (tosekiKbn == 2)
            {
                sikyu = "透後";
            }

            if (sikyuKbn == 1)
            {
                if (sikyu == "")
                {
                    sikyu = "至急";
                }
                else
                {
                    sikyu = sikyu.Substring(1, 1) + "急";
                }
            }

            if (sikyu != "")
            {
                sikyu = $"《{sikyu}》";
            }
            return sikyu;
        }

        /// <summary>
        /// リストに追加
        /// </summary>
        /// <param name="str"></param>
        /// <param name="maxLength"></param>
        /// <param name="preset"></param>
        /// <returns></returns>
        private List<CoSijisenPrintDataModel> _addList(string str, int maxLength, string preset = "")
        {
            List<CoSijisenPrintDataModel> addPrintOutData = new List<CoSijisenPrintDataModel>();

            if (maxLength > 0)
            {
                string line = preset + str;

                while (line != "")
                {
                    string tmp = line;
                    if (CIUtil.LenB(line) > maxLength)
                    {
                        // 文字列が最大幅より広い場合、カット
                        tmp = CIUtil.CiCopyStrWidth(line, 1, maxLength);
                    }

                    CoSijisenPrintDataModel printOutData = new CoSijisenPrintDataModel();
                    printOutData.Data = tmp;
                    addPrintOutData.Add(printOutData);

                    // 今回出力分の文字列を削除
                    line = CIUtil.CiCopyStrWidth(line, CIUtil.LenB(tmp) + 1, CIUtil.LenB(line) - CIUtil.LenB(tmp));

                    // インデント
                    if (line != "" && preset != "")
                    {
                        line = new string(' ', CIUtil.LenB(preset)) + line;
                    }
                }
            }

            return addPrintOutData;
        }
        #endregion
    }
}
