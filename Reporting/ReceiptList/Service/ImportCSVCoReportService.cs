using Domain.Models.Receipt.ReceiptListAdvancedSearch;
using Helper.Common;
using Microsoft.VisualBasic.FileIO;
using Reporting.CommonMasters.Enums;
using Reporting.Mappers.Common;
using Reporting.ReceiptList.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReceiptListModel = Reporting.ReceiptList.Model.ReceiptListModel;

namespace Reporting.ReceiptList.Service
{
    public class ImportCSVCoReportService : IImportCSVCoReportService
    {
        protected bool IsPrinterRunning = false;
        private int hpId;
        private int seikyuYm;
        private List<ReceiptListModel> receiptListModels;
        private CoFileType fileType;

        public CommonExcelReportingModel GetImportCSVCoReportServiceReportingData(int hpId, int seikyuYm, List<ReceiptListModel> receiptListModels, CoFileType fileType, bool outputTitle = false)
        {
            this.hpId = hpId;
            this.seikyuYm = seikyuYm;
            this.receiptListModels = receiptListModels;
            this.fileType = fileType;
            OutputCsv(outputTitle);

            return new CommonExcelReportingModel(sheetName + ".xlsx", sheetName, retDatas);
        }

        private void OutputCsv(bool outputTitle)
        {
            if (IsPrinterRunning)
            {
                return;
            }

            if (receiptListModels == null || receiptListModels.Any() == false)
            {
                return;
            }

            List<string> output = new List<string>();

            // ヘッダー

            if (outputTitle)
            {
                output.Add(
                    "請求," +
                    "診療年月," +
                    "変更," +
                    "紙," +
                    "印刷," +
                    "付箋," +
                    "確認," +
                    "コメント," +
                    "患者番号," +
                    "カナ," +
                    "氏名," +
                    "性," +
                    "年齢," +
                    "生年月日," +
                    "レセプト種別コード," +
                    "レセプト種別," +
                    "保険者番号," +
                    "点数," +
                    "実日数," +
                    "症状詳記," +
                    "レセコメント," +
                    "傷病の経過," +
                    "再請求コメント," +
                    "最終来院日," +
                    "診療科," +
                    "担当医," +
                    "旧姓," +
                    "公１負担者番号," +
                    "公２負担者番号," +
                    "公３負担者番号," +
                    "公４負担者番号," +
                    "テスト患者," +
                    "振込予定金額," +
                    "労災交付番号," +
                    "労災事業所名," +
                    "労災都道府県名," +
                    "労災所在地郡市区名," +
                    "自賠保険会社名," +
                "自賠保険担当者," +
                "自賠保険連絡先"
                    );
            }

            //int count = 0;
            foreach (ReceiptListModel receiptList in receiptListModels)
            {
                string line = "";
                // 請求
                line += $"{receiptList.SeikyuYm},";
                // 診療年月
                line += $"{receiptList.SinYm},";
                // 変更
                line += $"{receiptList.ReceInfDetailExistDisplay},";
                // 紙
                line += $"{receiptList.PaperReceDisplay},";
                // 印刷
                line += $"{receiptList.OutputDisplay},";
                // 付箋
                line += $"{receiptList.FusenKbnDisplay},";
                // 確認
                line += $"{receiptList.StatusKbnDisplay},";
                // コメント
                line += $"{receiptList.ReceCheckCmt},";
                // 患者番号
                line += $"{receiptList.PtNum},";
                // カナ
                line += $"{receiptList.KanaName},";
                // 氏名
                line += $"{receiptList.Name},";
                // 性
                line += $"{receiptList.SexDisplay},";
                // 年齢
                line += $"{receiptList.Age},";
                // 生年月日
                line += $"{receiptList.BirthDay},";
                // レセプト種別コード
                line += $"{receiptList.ReceSbt},";
                // レセプト種別
                line += $"{receiptList.ReceSbtDisplay},";
                // 保険者番号
                line += $"{receiptList.HokensyaNo},";
                // 点数
                line += $"{receiptList.Tensu},";
                // 実日数
                //line += $"{receiptList.HokenNissu},";
                line += $"{receiptList.Nissu},";
                // 症状詳記
                line += $"{receiptList.SyoukiInfExistDisplay},";
                // レセコメント
                line += $"{receiptList.ReceCmtExistDisplay},";
                // 傷病の経過
                line += $"{receiptList.SyobyoKeikaExistDisplay},";
                // 再請求コメント
                line += $"{receiptList.ReceSeikyuCmt},";
                // 最終来院日
                line += $"{receiptList.LastVisitDate},";
                // 診療科
                line += $"{receiptList.KaName},";
                // 担当医
                line += $"{receiptList.SName},";
                // 旧姓
                line += $"{receiptList.PtKyuseiExistDisplay},";
                // 公１負担者番号
                line += $"{receiptList.FutansyaNoKohi1},";
                // 公２負担者番号
                line += $"{receiptList.FutansyaNoKohi2},";
                // 公３負担者番号
                line += $"{receiptList.FutansyaNoKohi3},";
                // 公４負担者番号
                line += $"{receiptList.FutansyaNoKohi4},";
                // テスト患者
                line += $"{receiptList.IsPtTestDisplay},";
                // 振込予定金額
                line += $"{receiptList.ExpectedPayment},";
                // 労災交付番号
                line += $"{receiptList.RousaiKofuNo},";
                // 労災事業所名
                line += $"{receiptList.RousaiJigyosyoName},";
                // 労災都道府県名
                line += $"{receiptList.RousaiPrefName},";
                // 労災所在地郡市区名
                line += $"{receiptList.RousaiCityName},";
                // 自賠保険会社名
                line += $"{receiptList.JibaiHokenName},";
                // 自賠保険担当者
                line += $"{receiptList.JibaiHokenTanto},";
                // 自賠保険連絡先
                line += $"{receiptList.JibaiHokenTel}";
                output.Add(line);
            }

        }
    }
}
