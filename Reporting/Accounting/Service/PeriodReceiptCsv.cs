using Domain.Models.MstItem;
using Helper.Common;
using Reporting.Accounting.DB;
using Reporting.Accounting.Model;
using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Accounting.Service
{
    public class PeriodReceiptCsv : IPeriodReceiptCsv
    {
        private CoAccountingListModel _coModelList;
        private ICoAccountingFinder _finder;
        private List<CoJihiSbtMstModel> _jihiSbtMstModels;
        private List<CoPtGrpNameMstModel> _ptGrpNameMstModels;
        private List<CoPtGrpItemModel> _ptGrpItemModels;
        private List<CoSystemGenerationConfModel> _sysGeneHanreis;
        private string printWarningMessage = "";

        public PeriodReceiptCsv(ICoAccountingFinder finder)
        {
            _finder = finder;
            _coModelList = new();
            _jihiSbtMstModels = new();
            _ptGrpNameMstModels = new();
            _ptGrpItemModels = new();
            _sysGeneHanreis = new();
        }

        public CommonExcelReportingModel GetPeriodReceiptCsv(int hpId, int startDate, int endDate, List<(long ptId, int hokenId)> ptConditions, List<(int grpId, string grpCd)> grpConditions, int sort, int miseisanKbn, int saiKbn, int misyuKbn, int seikyuKbn, int hokenKbn)
        {
            _coModelList = GetDataList(hpId, startDate, endDate, ptConditions, grpConditions, sort, miseisanKbn, saiKbn, misyuKbn, seikyuKbn, hokenKbn);
            
            List<string> output = new List<string>();

            // ヘッダー
            string head = $"対象期間：{startDate}-{endDate} ";

            List<long> ptNums = _finder.GetPtNums(hpId, ptConditions.Select(p => p.ptId).ToList());
            string sheetName = "レセチェック一覧表";

            if (_coModelList == null || _coModelList.KaikeiInfListModels == null || _coModelList.KaikeiInfListModels.Any() == false)
            {
                return new CommonExcelReportingModel(sheetName + ".xlsx", sheetName, output);
            }

            if (ptNums.Any())
            {
                head += "対象患者：";
                foreach (long ptId in ptNums)
                {
                    head += $"{ptId}/";
                }
                head = head.Substring(0, head.Length - 1);
            }

            if (grpConditions != null && grpConditions.Any())
            {
                foreach ((int grpId, string grpCd) in grpConditions)
                {
                    head += $" 分類{CIUtil.ToWide(grpId.ToString())}：{grpCd}";
                }
            }

            output.Add(head);

            output.Add(
                "患者番号," +
                "患者漢字氏名," +
                "患者カナ氏名," +
                "診療総点数," +
                "診療合計額," +
                "患者負担合計," +
                "自費合計額," +
                "保険外医療計," +
                "自費分患者負担額," +
                "外税," +
                "入金額," +
                "請求金額," +
                "未収金額," +
                "診療合計自費合計," +
                "性別," +
                "生年月日," +
                "郵便番号," +
                "住所," +
                "電話１," +
                "電話２," +
                "電話３," +
                "分類コード１," +
                "分類名称１," +
                "分類コード２," +
                "分類名称２," +
                "分類コード３," +
                "分類名称３," +
                "分類コード４," +
                "分類名称４," +
                "分類コード５," +
                "分類名称５," +
                "分類コード６," +
                "分類名称６," +
                "患者メモ１," +
                "患者メモ２," +
                "患者メモ３," +
                "患者メモ４," +
                "患者メモ５"
                );

            foreach (CoKaikeiInfListModel kaikeiInf in _coModelList.KaikeiInfListModels)
            {
                string line = "";
                // 患者番号
                line += $"{kaikeiInf.PtNum},";
                // 患者漢字氏名
                line += $"{kaikeiInf.PtName},";
                // 患者カナ氏名
                line += $"{kaikeiInf.PtKanaName},";
                // 診療総点数
                line += $"{kaikeiInf.Tensu},";
                // 診療合計額
                line += $"{kaikeiInf.TotalIryohi},";
                // 患者負担合計
                line += $"{kaikeiInf.PtFutan},";
                // 自費合計額
                line += $"{kaikeiInf.JihiFutan + kaikeiInf.JihiOuttax},";
                // 保険外医療計
                line += $"{kaikeiInf.JihiKoumoku},";
                // 自費分患者負担額
                line += $"{kaikeiInf.JihiSinryo},";
                // 消費税
                line += $"{kaikeiInf.JihiOuttax},";
                // 入金額
                line += $"{kaikeiInf.NyukinGaku},";
                // 請求金額
                line += $"{kaikeiInf.SeikyuGaku},";
                // 未収金額
                line += $"{kaikeiInf.Misyu},";
                // 診療合計自費合計
                line += $"{kaikeiInf.TotalIryohi + kaikeiInf.JihiFutan + kaikeiInf.JihiOuttax},";
                // 性別
                line += $"{kaikeiInf.PtSex},";
                // 生年月日
                line += $"{kaikeiInf.BirthDay},";
                // 郵便番号
                line += $"{kaikeiInf.PostCd},";
                // 住所
                line += $"{kaikeiInf.Address},";
                // 電話１
                line += $"{kaikeiInf.Tel1},";
                // 電話２
                line += $"{kaikeiInf.Tel2},";
                // 電話３
                line += $"{kaikeiInf.RenrakuTel},";

                for (int i = 1; i <= 6; i++)
                {
                    // 分類コード１
                    line += $"{kaikeiInf.PtGroupInfCode(i)},";
                    // 分類名称１
                    line += $"{kaikeiInf.PtGroupInfCodeName(i)},";
                }

                // 患者メモ
                if (string.IsNullOrEmpty(kaikeiInf.Memo) == false)
                {
                    string[] del = { "\r\n", "\r", "\n" };
                    List<string> memos = kaikeiInf.Memo.Split(del, StringSplitOptions.None).ToList();

                    for (int i = 0; i < 5; i++)
                    {
                        if (memos.Count() > i)
                        {
                            line += $"{memos[i]},";
                        }
                        else
                        {
                            line += ",";
                        }
                    }
                }

                output.Add(line);
            }

            return new CommonExcelReportingModel(sheetName + ".xlsx", sheetName, output);
        }

        private CoAccountingListModel GetDataList(int hpId, int startDate, int endDate,
            List<(long ptId, int hokenId)> ptConditions, List<(int grpId, string grpCd)> grpConditions,
            int sort, int miseisanKbn, int saiKbn, int misyuKbn, int seikyuKbn, int hokenKbn)
        {

            // 診療日
            int sinDate = endDate;

            // 医療機関情報
            CoHpInfModel hpInfModel = _finder.FindHpInf(hpId, sinDate);

            // 会計情報
            List<CoWarningMessage> warningMessages = new List<CoWarningMessage>();

            List<CoKaikeiInfListModel> kaikeiInfListModels = _finder.FindKaikeiInfList(hpId, startDate, endDate, ptConditions, grpConditions, sort, miseisanKbn, saiKbn, misyuKbn, seikyuKbn, hokenKbn, ref warningMessages);

            if (warningMessages != null && warningMessages.Any())
            {
                foreach (CoWarningMessage warningMessage in warningMessages)
                {
                    if (string.IsNullOrEmpty(printWarningMessage) == false)
                    {
                        printWarningMessage += "\r\n";
                    }

                    printWarningMessage += $"[ID:{warningMessage.PtNum}]{warningMessage.WarningMessage}";

                    foreach (string detail in warningMessage.Detail)
                    {
                        printWarningMessage += "\r\n" + $"{detail}";
                    }

                    printWarningMessage += "\r\n";
                }
            }

            CoAccountingListModel result =
                new CoAccountingListModel(
                    hpInfModel, kaikeiInfListModels);

            _jihiSbtMstModels = _finder.FindJihiSbtMst(hpId);
            _ptGrpNameMstModels = _finder.FindPtGrpNameMst(hpId);
            _ptGrpItemModels = _finder.FindPtGrpItemMst(hpId);

            //sysGeneHanreis = finder.FindSystemGenerationConf(3004);
            _sysGeneHanreis = _finder.FindSystemGenerationConf(hpId, 3001);

            return result;
        }
    }
}
