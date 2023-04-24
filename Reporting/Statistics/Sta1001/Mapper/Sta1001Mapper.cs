using Helper.Common;
using Helper.Constants;
using Reporting.Mappers.Common;
using Reporting.Statistics.Enums;
using Reporting.Statistics.Model;
using Reporting.Statistics.Sta1001.Models;

namespace Reporting.Statistics.Sta1001.Mapper
{
    public class Sta1001Mapper : CommonReportingRequest
    {
        private CoSta1001PrintConf printConf;
        private CoHpInfModel hpInf;
        private List<CoSta1001PrintData> printDatas;
        protected int CurrentPage;
        private List<string> headerL1;
        private List<string> headerL2;
        private int maxRow;

        #region Constant
        private List<PutColumn> csvTotalColumns = new List<PutColumn>
        {
            new PutColumn("RowType", "明細区分"),
            new PutColumn("TotalCaption", "合計行"),
            new PutColumn("TotalCount", "合計件数"),
        };

        private List<PutColumn> putColumns = new List<PutColumn>
        {
            new PutColumn("KaId", "診療科ID", false),
            new PutColumn("KaSname", "診療科略称", false),
            new PutColumn("TantoId", "担当医ID", false),
            new PutColumn("TantoSname", "担当医略称", false),
            new PutColumn("PtNum", "患者番号", false),
            new PutColumn("PtKanaName", "カナ氏名", false),
            new PutColumn("PtName", "氏名", false),
            new PutColumn("SexCd", "性別コード", false),
            new PutColumn("Sex", "性別", false),
            new PutColumn("BirthDayFmt", "生年月日", false, "BirthDay"),
            new PutColumn("Age", "年齢", false),
            new PutColumn("HomePost", "郵便番号", false),
            new PutColumn("HomeAddress", "住所", false),
            new PutColumn("Tel1", "電話番号１", false),
            new PutColumn("Tel2", "電話番号２", false),
            new PutColumn("RenrakuTel", "緊急連絡先電話番号", false),
            new PutColumn("SinDateFmt", "診察日", false, "SinDate"),
            new PutColumn("RaiinNo", "来院番号", false),
            new PutColumn("HokenSbt", "保険種別", false),
            new PutColumn("Syosaisin", "初再診", false),
            new PutColumn("SeikyuGaku", "請求額"),
            new PutColumn("OldSeikyuGaku", "請求額(旧)"),
            new PutColumn("NewSeikyuGaku", "請求額(新)"),
            new PutColumn("AdjustFutan", "調整額"),
            new PutColumn("OldAdjustFutan", "調整額(旧)"),
            new PutColumn("NewAdjustFutan", "調整額(新)"),
            new PutColumn("TotalSeikyuGaku", "合計請求額"),
            new PutColumn("NyukinGaku", "入金額"),
            new PutColumn("MisyuGaku", "未収額"),
            new PutColumn("NyukinCmt", "コメント", false),
            new PutColumn("LastVisitDateFmt", "最終来院日", false, "LastVisitDate"),
            new PutColumn("MisyuKbn", "未収区分", false)
        };
        #endregion

        public override int GetReportType()
        {
            return (int)CoReportType.Sta1001;
        }

        public override Dictionary<string, string> GetSingleFieldData()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            //タイトル
            data.Add("Title", printConf.ReportName);

            //期間
            data.Add("Range",
                string.Format(
                    "期間: {0} ～ {1}",
                    CIUtil.SDateToShowSWDate(printConf.StartNyukinDate, 0, 1),
                    CIUtil.SDateToShowSWDate(printConf.EndNyukinDate, 0, 1)
                )
            );

            int ptIndex = (CurrentPage - 1) * maxRow;
            int lineCount = 0;

            //存在しているフィールドに絞り込み
            var existsCols = putColumns.Where(p => CoRep.ListExists(p.ColName)).Select(p => p.ColName).ToList();

            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                var printData = printDatas[ptIndex];
                string baseListName = "";

                //明細データ出力
                foreach (var colName in existsCols)
                {
                    var value = typeof(CoSta1010PrintData).GetProperty(colName).GetValue(printData);
                    CoRep.ListText(colName, 0, rowNo, value == null ? "" : value.ToString());

                    if (baseListName == "" && CoRep.ListExists(colName))
                    {
                        baseListName = colName;
                    }
                }

                //合計行キャプションと件数
                CoRep.ListText("TotalCaption", 0, rowNo, printData.TotalCaption);
                CoRep.ListText("TotalCount", 0, rowNo, printData.TotalCount);

                //5行毎に区切り線を引く
                lineCount = printData.RowType != RowType.Brank ? lineCount + 1 : lineCount;

                if (lineCount == 5)
                {
                    lineCount = 0;
                    (long startX1, long startY1, long endX1, long endY1) = CoRep.GetBounds("headerLine");
                    (long startX2, long startY2, long endX2, long endY2) = CoRep.GetListRowBounds(baseListName, rowNo);

                    CoRep.DrawLine(startX1, endY2, endX1, endY2, 10, Hos.CnDraw.Constants.ConLineStyle.Dash);
                }

                ptIndex++;
                if (ptIndex >= printDatas.Count)
                {
                    _hasNextPage = false;
                    break;
                }
            }

            return data;
        }

        public override List<Dictionary<string, CellModel>> GetTableFieldData()
        {
            List<Dictionary<string, CellModel>> result = new List<Dictionary<string, CellModel>>();
            Dictionary<string, CellModel> data = new Dictionary<string, CellModel>();
            //医療機関名
            data.Add("HeaderR", new CellModel(hpInf.HpName));
            //作成日時
            data.Add("HeaderR", new CellModel(CIUtil.SDateToShowSWDate(
                CIUtil.ShowSDateToSDate(DateTime.Now.ToString("yyyy/MM/dd")), 0, 1
            ) + DateTime.Now.ToString(" HH:mm") + "作成"));
            //ページ数
            int totalPage = (int)Math.Ceiling((double)printDatas.Count / maxRow);
            data.Add("HeaderR", new CellModel(CurrentPage + " / " + totalPage));
            //入金日
            data.Add("HeaderL", new CellModel(headerL1.Count >= CurrentPage ? headerL1[CurrentPage - 1] : ""));
            //改ページ条件
            data.Add("HeaderL", new CellModel(headerL2.Count >= CurrentPage ? headerL2[CurrentPage - 1] : ""));
            result.Add(data);

            return result;
        }
    }
}
