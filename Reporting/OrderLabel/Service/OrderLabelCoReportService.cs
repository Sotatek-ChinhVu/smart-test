using Helper.Common;
using Infrastructure.Interfaces;
using Reporting.CommonMasters.Config;
using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.OrderLabel.DB;
using Reporting.OrderLabel.Mapper;
using Reporting.OrderLabel.Model;
using Reporting.ReadRseReportFile.Service;
using System.Text;

namespace Reporting.OrderLabel.Service;

public class OrderLabelCoReportService : IOrderLabelCoReportService
{
    // 出力先コントロール
    private enum TargetControl
    {
        // lsData
        Data = 0,
        // lsDataWide
        Wide = 1,
        // lsComment
        Comment = 2
    }

    private readonly ITenantProvider _tenantProvider;
    private CoOrderLabelModel? _coModel = null;
    private List<CoUserMstModel> _userMsts = new();
    private List<CoOrderLabelPrintDataModel> _printOutData = new();
    private readonly ISystemConfig _systemConfig;

    public OrderLabelCoReportService(ITenantProvider tenantProvider, ISystemConfig systemConfig)
    {
        _tenantProvider = tenantProvider;
        _systemConfig = systemConfig;
    }

    public CommonReportingRequestModel GetOrderLabelReportingData(int mode, int hpId, long ptId, int sinDate, long raiinNo, List<(int from, int to)> odrKouiKbns, List<RsvkrtOdrInfModel> rsvKrtOdrInfModels)
    {
        using (var noTrackingDataContext = _tenantProvider.GetNoTrackingDataContext())
        {
            var finder = new CoOrderLabelFinder(_tenantProvider);
            try
            {
                if (mode == 0)
                {
                    _coModel = GetData(hpId, ptId, sinDate, raiinNo, odrKouiKbns, finder);
                }
                else
                {
                    _coModel = GetDataYoyakuOrder(hpId, ptId, odrKouiKbns, rsvKrtOdrInfModels, finder);
                }
                if (_coModel == null)
                {
                    return new OrderLabelMapper(_printOutData).GetData();
                }
                _userMsts = finder.FindUserMst(hpId);
                MakeOdrDtlList(sinDate);
                return new OrderLabelMapper(_printOutData).GetData();
            }
            finally
            {
                finder.ReleaseResource();
                _tenantProvider.DisposeDataContext();
            }
        }
    }

    private CoOrderLabelModel? GetData(int hpId, long ptId, int sinDate, long raiinNo, List<(int from, int to)> odrKouiKbns, CoOrderLabelFinder finder)
    {
        // 患者情報
        CoPtInfModel ptInf = finder.FindPtInf(hpId, ptId);

        // 来院情報
        CoRaiinInfModel raiinInf = finder.FindRaiinInfData(hpId, ptId, sinDate, raiinNo);

        // 予約情報
        List<CoYoyakuModel> yoyakuModels = finder.FindYoyaku(hpId, ptId, sinDate);

        // オーダー情報
        List<CoOdrInfModel> odrInfs = finder.FindOdrInf(hpId, ptId, sinDate, raiinNo, odrKouiKbns);
        odrInfs =
            odrInfs.OrderBy(p => p.RaiinNo)
            .ThenBy(p => p.SortKouiKbn)
            .ThenBy(p => p.InoutKbn)
            .ThenBy(p => p.TosekiKbn)
            .ThenBy(p => p.SikyuKbn)
            .ThenBy(p => p.SortNo)
            .ThenBy(p => p.RpNo)
            .ThenBy(p => p.RpEdaNo)
            .ToList();

        // オーダー情報詳細
        List<CoOdrInfDetailModel> odrInfDtls = finder.FindOdrInfDetail(hpId, ptId, sinDate, raiinNo, odrKouiKbns);

        List<CoCommonOdrInfModel> commonOdrInfs = CommonOdrInfListFactory(odrInfs);
        List<CoCommonOdrInfDetailModel> commonOdrDtls = CommonOdrInfDetailListFactory(odrInfDtls);

        CoOrderLabelModel? coOrderLabel = null;

        if (odrInfs != null && odrInfs.Any(p => p.OdrKouiKbn > 10) && odrInfDtls != null && odrInfDtls.Any() ||
            commonOdrInfs != null && commonOdrInfs.Any(p => p.OdrKouiKbn > 10) && commonOdrDtls != null && commonOdrDtls.Any() ||
            yoyakuModels != null && yoyakuModels.Any())
        {
            coOrderLabel = new CoOrderLabelModel(ptInf, raiinInf, commonOdrInfs ?? new(), commonOdrDtls ?? new(), yoyakuModels, false);
        }

        return coOrderLabel;
    }

    private CoOrderLabelModel? GetDataYoyakuOrder(int hpId, long ptId, List<(int from, int to)> odrKouiKbns, List<RsvkrtOdrInfModel> rsvKrtOdrInfModels, CoOrderLabelFinder finder)
    {
        // 患者情報
        CoPtInfModel ptInf = finder.FindPtInf(hpId, ptId);

        // オーダー情報
        List<CoCommonOdrInfModel> commonOdrInfs = new();
        List<CoCommonOdrInfDetailModel> commonOdrDtls = new();
        foreach (var odrInf in rsvKrtOdrInfModels.Where(odrInf => odrKouiKbns.Any(p => p.from <= odrInf.OdrKouiKbn && p.to >= odrInf.OdrKouiKbn)))
        {
            commonOdrInfs.Add(
                            new CoCommonOdrInfModel(
                                hpId: odrInf.HpId, sinDate: odrInf.RsvDate, ptId: odrInf.PtId, raiinNo: odrInf.RsvkrtNo,
                                rpNo: odrInf.RpNo, rpEdaNo: odrInf.RpEdaNo,
                                odrKouiKbn: odrInf.OdrKouiKbn, rpName: odrInf.RpName, inoutKbn: odrInf.InoutKbn, sikyuKbn: odrInf.SikyuKbn,
                                syohoSbt: odrInf.SyohoSbt, santeiKbn: odrInf.SanteiKbn, tosekiKbn: odrInf.TosekiKbn, daysCnt: odrInf.DaysCnt, sortNo: odrInf.SortNo, createId: odrInf.CreateId));
            foreach (RsvkrtOdrInfDetailModel odrDtl in odrInf.OdrInfDetailModels)
            {
                if (odrDtl.IsEmpty) continue;
                commonOdrDtls.Add(
                    new CoCommonOdrInfDetailModel(
                        hpId: odrDtl.HpId, ptId: odrDtl.PtId, sinDate: odrDtl.RsvDate, raiinNo: odrDtl.RsvkrtNo,
                        rpNo: odrDtl.RpNo, rpEdaNo: odrDtl.RpEdaNo, rowNo: odrDtl.RowNo,
                        odrKouiKbn: odrInf.OdrKouiKbn,
                        sinKouiKbn: odrDtl.SinKouiKbn,
                        itemCd: odrDtl.ItemCd, itemName: odrDtl.ItemName, suryo: odrDtl.Suryo, unitName: odrDtl.UnitName,
                        syohoKbn: odrDtl.SyohoKbn, syohoLimitKbn: odrDtl.SyohoLimitKbn, bunkatu: odrDtl.Bunkatu
                        )
                    );
            }
        }

        CoOrderLabelModel? coOrderLabel = null;

        if (commonOdrInfs.Any() && commonOdrDtls.Any())
        {
            coOrderLabel = new CoOrderLabelModel(ptInf, new(), commonOdrInfs, commonOdrDtls, new(), true);
        }

        return coOrderLabel;

    }

    /// <summary>
    /// オーダー情報リストを共通オーダー情報モデルのリストに変換
    /// </summary>
    /// <param name="odrInfs"></param>
    /// <returns></returns>
    List<CoCommonOdrInfModel> CommonOdrInfListFactory(List<CoOdrInfModel> odrInfs)
    {
        List<CoCommonOdrInfModel> results = new List<CoCommonOdrInfModel>();

        foreach (CoOdrInfModel odrInf in odrInfs)
        {
            results.Add(CommonOdrInfFactory(odrInf));
        }

        return results;
    }

    /// <summary>
    /// オーダー情報詳細リストを共通オーダー情報詳細モデルのリストに変換
    /// </summary>
    /// <param name="odrDtls"></param>
    /// <returns></returns>
    List<CoCommonOdrInfDetailModel> CommonOdrInfDetailListFactory(List<CoOdrInfDetailModel> odrDtls)
    {
        List<CoCommonOdrInfDetailModel> results = new List<CoCommonOdrInfDetailModel>();

        foreach (CoOdrInfDetailModel odrDtl in odrDtls)
        {
            results.Add(CommonOdrDtlFactory(odrDtl));
        }

        return results;
    }

    /// <summary>
    /// オーダー情報から共通オーダー情報モデルを生成
    /// </summary>
    /// <param name="odrInf"></param>
    /// <returns></returns>
    private CoCommonOdrInfModel CommonOdrInfFactory(CoOdrInfModel odrInf)
    {
        return new CoCommonOdrInfModel(
            hpId: odrInf.HpId, ptId: odrInf.PtId, sinDate: odrInf.SinDate,
            raiinNo: odrInf.RaiinNo, rpNo: odrInf.RpNo, rpEdaNo: odrInf.RpEdaNo,
            odrKouiKbn: odrInf.OdrKouiKbn, rpName: odrInf.RpName,
            inoutKbn: odrInf.InoutKbn, sikyuKbn: odrInf.SikyuKbn, syohoSbt: odrInf.SyohoSbt,
            santeiKbn: odrInf.SanteiKbn, tosekiKbn: odrInf.TosekiKbn, daysCnt: odrInf.DaysCnt, sortNo: odrInf.SortNo, createId: odrInf.CreateId);
    }

    /// <summary>
    /// オーダー詳細情報から共通オーダー詳細情報モデルを生成
    /// </summary>
    /// <param name="odrDtl"></param>
    /// <returns></returns>
    private CoCommonOdrInfDetailModel CommonOdrDtlFactory(CoOdrInfDetailModel odrDtl)
    {
        return new CoCommonOdrInfDetailModel(
            hpId: odrDtl.HpId, ptId: odrDtl.PtId, sinDate: odrDtl.SinDate,
            raiinNo: odrDtl.RaiinNo, rpNo: odrDtl.RpNo, rpEdaNo: odrDtl.RpEdaNo, rowNo: odrDtl.RowNo,
            odrKouiKbn: odrDtl.OdrKouiKbn, sinKouiKbn: odrDtl.SinKouiKbn, itemCd: odrDtl.ItemCd, itemName: odrDtl.ItemName,
            suryo: odrDtl.Suryo, unitName: odrDtl.UnitName,
            syohoKbn: odrDtl.SyohoKbn, syohoLimitKbn: odrDtl.SyohoLimitKbn,
            bunkatu: odrDtl.Bunkatu
            );
    }

    private void MakeOdrDtlList(int sinDate)
    {
        #region sub method
        // 初再診
        string getSyosai(double suryo)
        {
            string itemName = string.Empty;
            // 初再診
            switch (suryo)
            {
                case 0: // 初再診なし
                    itemName = string.Empty;
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
                    itemName = string.Empty;
                    break;
            }

            return itemName;
        }
        // 時間枠
        string getJikan(double suryo)
        {
            string itemName = string.Empty;

            // 時間枠

            switch (suryo)
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

            return itemName;
        }

        string makeHeader(int sinDate, int date, bool yoyaku)
        {
            string ret = CIUtil.SDateToShowSDate(sinDate);

            if (yoyaku)
            {
                if (date == 99999999)
                {
                    ret = "次回来院日 ";
                }
                else
                {
                    ret = "次回 " + ret;
                }
            }
            ret = ret + $" {_coModel.PtNum} {_coModel.PtName}";
            return ret;
        }

        string getCreateUserName(int userId)
        {
            string user = string.Empty;

            if (_userMsts.Any(p => p.UserId == userId))
            {
                user = _userMsts.FirstOrDefault(p => p.UserId == userId)?.Sname ?? string.Empty;
            }

            return user;
        }
        #endregion

        _printOutData = new List<CoOrderLabelPrintDataModel>();

        List<CoOrderLabelPrintDataModel> addPrintOutData = new List<CoOrderLabelPrintDataModel>();

        // ヘッダー
        string header = string.Empty;

        _printOutData.AddRange(addPrintOutData);

        int rpNo = 0;
        List<CoCommonOdrInfModel> filteredOdrInfs = _coModel!.OdrInfModels.FindAll(p => p.OdrKouiKbn < 10 || p.OdrKouiKbn > 10);

        int createId = 0;

        for (int i = 0; i < filteredOdrInfs.Count; i++)
        {
            CoCommonOdrInfModel odrInf = filteredOdrInfs[i];

            addPrintOutData = new List<CoOrderLabelPrintDataModel>();

            if (i == 0)
            {
                // 最初にヘッダー印字
                header = makeHeader(sinDate, odrInf.SinDate, _coModel.IsYoyaku);

                addPrintOutData.Add(AddItem(TargetControl.Comment, header));

                if (!_coModel.IsYoyaku)
                {
                    // 診療科名＋担当医
                    if (_systemConfig.OrderLabelKaPrint() == 1)
                    {
                        // 診療科印字あり
                        addPrintOutData.Add(AddItem(TargetControl.Comment, $"{_coModel.KaName} {_coModel.TantoName}"));
                    }
                    else
                    {
                        // 診療科印字なし
                        addPrintOutData.Add(AddItem(TargetControl.Comment, $"{_coModel.TantoName}"));
                    }

                    // 初再診
                    if (_systemConfig.OrderLabelSyosaiPrint() == 1 && _coModel.OdrInfDetailModels.Any(p => p.OdrKouiKbn >= 10 && p.OdrKouiKbn <= 12))
                    {
                        string syosai = string.Empty;
                        CoCommonOdrInfDetailModel odrDtlSin = _coModel.OdrInfDetailModels.FirstOrDefault(p => p.ItemCd == "@SHIN") ?? new();
                        if (odrDtlSin != null)
                        {
                            syosai = getSyosai(odrDtlSin.Suryo);
                        }

                        if (syosai != string.Empty)
                        {
                            CoCommonOdrInfDetailModel odrDtlJikan = _coModel.OdrInfDetailModels.FirstOrDefault(p => p.ItemCd == "@JIKAN") ?? new();
                            if (odrDtlJikan != null)
                            {
                                string jikan = getJikan(odrDtlJikan.Suryo);
                                if (jikan != string.Empty)
                                {
                                    syosai += $"({jikan})";
                                }
                            }
                        }

                        if (syosai != string.Empty)
                        {
                            addPrintOutData.Add(AddItem(TargetControl.Comment, syosai));
                        }
                    }
                }

                if (_systemConfig.OrderLabelCreateNamePrint() == 1 &&
                    createId != odrInf.CreateId)
                {
                    // 入力者
                    string user = getCreateUserName(odrInf.CreateId);
                    if (!string.IsNullOrEmpty(user))
                    {
                        // 1行あけて・・・
                        addPrintOutData.Add(new CoOrderLabelPrintDataModel());
                        addPrintOutData.Add(AddItem(TargetControl.Comment, $"{user}"));
                    }
                }
            }
            else if (_systemConfig.OrderLabelHeaderPrint() == 1 && i > 0)
            {
                // ヘッダーをRpごとに出力する設定の場合
                // 1行あけて・・・
                addPrintOutData.Add(new CoOrderLabelPrintDataModel());
                // 患者名等を追加
                addPrintOutData.Add(AddItem(TargetControl.Comment, header));

                // 入力者
                string user = getCreateUserName(odrInf.CreateId);
                if (!string.IsNullOrEmpty(user))
                {
                    addPrintOutData.Add(AddItem(TargetControl.Comment, $"{user}"));
                }
            }
            else if (_systemConfig.OrderLabelCreateNamePrint() == 1 &&
                createId != odrInf.CreateId)
            {
                string user = getCreateUserName(odrInf.CreateId);
                if (!string.IsNullOrEmpty(user))
                {
                    // 1行あけて・・・
                    addPrintOutData.Add(new CoOrderLabelPrintDataModel());
                    // 入力者
                    addPrintOutData.Add(AddItem(TargetControl.Comment, $"{user}"));
                }
            }
            createId = odrInf.CreateId;

            // 行為名を除くRp先頭行のみ*を付ける
            rpNo++;
            string preSet = $"{rpNo:D2})";
            string mark = GetMark(odrInf.SikyuKbn, odrInf.SanteiKbn);
            string inout = string.Empty;
            if ((odrInf.OdrKouiKbn >= 20 && odrInf.OdrKouiKbn < 29 || odrInf.OdrKouiKbn >= 60 && odrInf.OdrKouiKbn < 69) && odrInf.InoutKbn == 1)
            {
                inout = "院外";
            }

            if (odrInf.OdrKouiKbn >= 60 && odrInf.OdrKouiKbn < 69 && _systemConfig.OrderLabelKensaDsp() == 0)
            {
                // 検査1行複数表示
                StringBuilder itemName = new();
                foreach (CoCommonOdrInfDetailModel odrDtl in _coModel.OdrInfDetailModels.FindAll(p => p.RpNo == odrInf.RpNo && p.RpEdaNo == odrInf.RpEdaNo))
                {
                    if (itemName.Length > 0)
                    {
                        itemName.Append("、");
                        itemName.Append(mark + odrDtl.ItemName);
                    }

                    mark = string.Empty;
                }

                if (itemName.Length > 0)
                {
                    // wide
                    addPrintOutData.Add(AddItem(TargetControl.Wide, itemName.ToString(), preSet, inout));
                }
            }
            else
            {
                foreach (CoCommonOdrInfDetailModel odrDtl in _coModel.OdrInfDetailModels.FindAll(p => p.RpNo == odrInf.RpNo && p.RpEdaNo == odrInf.RpEdaNo))
                {
                    if (string.IsNullOrEmpty(odrDtl.ItemCd) || odrDtl.ItemCd.StartsWith("C") || odrDtl.ItemCd.StartsWith("8") && odrDtl.ItemCd.Length == 9)
                    {
                        // コメント
                        addPrintOutData.Add(AddItem(TargetControl.Data, mark + odrDtl.ItemName, preSet, inout));
                    }
                    else
                    {
                        string itemName = mark + odrDtl.ItemName;

                        if (odrDtl.ItemCd == "@BUNKATU")
                        {
                            itemName += TenUtils.GetBunkatu(odrInf.OdrKouiKbn, odrDtl.Bunkatu);
                        }

                        if (odrDtl.SyohoKbn == 3)
                        {
                            string ipnSyoho = "般》";
                            switch (odrDtl.SyohoLimitKbn)
                            {
                                case 1:
                                    ipnSyoho += "剤";
                                    break;
                                case 2:
                                    ipnSyoho += "含";
                                    break;
                                case 3:
                                    ipnSyoho += "両";
                                    break;
                            }
                            itemName = ipnSyoho + itemName;
                        }

                        addPrintOutData.Add(AddItem(TargetControl.Data, itemName, preSet, inout));

                    }

                    preSet = string.Empty;
                    mark = string.Empty;
                    inout = string.Empty;

                    if (!string.IsNullOrEmpty(odrDtl.UnitName))
                    {
                        addPrintOutData.Last().Suuryo = odrDtl.Suryo.ToString();
                        addPrintOutData.Last().Tani = odrDtl.UnitName;
                    }
                }

            }

            _printOutData.AddRange(addPrintOutData);

        }

        // 予約情報
        if (_systemConfig.OrderLabelYoyakuDateDsp() == 1 && _coModel.YoyakuModels != null && _coModel.YoyakuModels.Any())
        {
            if (_printOutData.Any())
            {
                // 空行
                _printOutData.Add(new CoOrderLabelPrintDataModel());
            }

            foreach (CoYoyakuModel yoyakuModel in _coModel?.YoyakuModels ?? new())
            {
                string yoyaku = $"次回予約：{yoyakuModel.SinDate / 10000}年{yoyakuModel.SinDate % 10000 / 100}月{yoyakuModel.SinDate % 100}日";
                string week = CIUtil.GetYobi(yoyakuModel.SinDate);
                if (week != string.Empty)
                {
                    week = "（" + week + "）";
                }
                yoyaku += week;
                if (yoyakuModel.Time.Length >= 4)
                {
                    yoyaku += CIUtil.Copy(yoyakuModel.Time, 1, 2) + ":" + CIUtil.Copy(yoyakuModel.Time, 3, 2);
                }
                if (yoyakuModel.Frame != string.Empty)
                {
                    yoyaku += "[" + yoyakuModel.Frame + "]";
                }
                _printOutData.Add(AddItem(TargetControl.Comment, yoyaku));
            }
        }
    }

    /// <summary>
    /// 算定外区分、至急区分、を取得する
    /// 算定外の場合は、"■"(オプション)
    /// 至急の場合は"[急]"
    /// </summary>
    /// <param name="sikyuKbn">至急区分</param>
    /// <param name="santeiKbn">算定区分</param>
    /// <returns></returns>
    private string GetMark(int sikyuKbn, int santeiKbn)
    {
        string sikyu = string.Empty;

        if (_systemConfig.OrderLabelSanteiGaiDsp() == 1 && santeiKbn == 1)
        {
            sikyu = "■";
        }

        if (sikyuKbn == 1)
        {
            sikyu += "[急]";
        }

        return sikyu;
    }

    /// <summary>
    /// リストに追加
    /// </summary>
    /// <param name="target">
    /// データを格納する先
    ///     0:CoOrderLabelPrintDataModel.Data
    ///     1:CoOrderLabelPrintDataModel.DataWide
    ///     2:CoOrderLabelPrintDataModel.Comment
    /// </param>
    /// <param name="str">リストにセットする文字列</param>
    /// <param name="maxLength">1行の幅</param>
    /// <param name="preset">strの前に着ける文字</param>
    /// <param name="inout">院内院外区分</param>
    /// <returns></returns>
    private CoOrderLabelPrintDataModel AddItem(TargetControl target, string str, string preset = "", string inout = "")
    {
        CoOrderLabelPrintDataModel result = new();
        switch (target)
        {
            case TargetControl.Data:
                result.Data = str;
                break;
            case TargetControl.Wide:
                result.DataWide = str;
                break;
            case TargetControl.Comment:
                result.Comment = str;
                break;
        }

        result.RpNo = preset;
        result.InOut = inout;

        return result;
    }
}
