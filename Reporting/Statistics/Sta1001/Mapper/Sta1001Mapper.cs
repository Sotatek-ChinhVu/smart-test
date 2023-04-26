using Helper.Common;
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
        private int maxRow = 43;
        private int CurrentPage = 1;
        private List<string> headerL1;
        private List<string> headerL2;
        private List<CoSyunoInfModel> syunoInfs;
        private List<CoJihiSbtMstModel> jihiSbtMsts;
        private List<CoJihiSbtFutan> jihiSbtFutans;
        private List<PutColumn> putCurColumns = new List<PutColumn>();
        private List<string> _objectRseList;
        private int _currentPage;
        private bool _hasNextPage;

        Dictionary<string, string> _extralData = new Dictionary<string, string>();
        Dictionary<string, string> SingleData = new Dictionary<string, string>();
        List<Dictionary<string, CellModel>> CellData = new List<Dictionary<string, CellModel>>();

        public override int GetReportType()
        {
            return (int)CoReportType.Sta1001;
        }

        public override Dictionary<string, string> GetFileNamePageMap()
        {
            var fileName = new Dictionary<string, string>();
            fileName.Add("1", "sta1001a.rse");
            return fileName;
        }

        public override Dictionary<string, string> GetSingleFieldData()
        {
            return SingleData;
        }

        public override List<Dictionary<string, CellModel>> GetTableFieldData()
        {
            return CellData;
        }

        public override Dictionary<string, bool> GetVisibleFieldData()
        {
            return new Dictionary<string, bool>();
        }

        public override Dictionary<string, bool> GetWrapFieldData()
        {
            return new Dictionary<string, bool>();
        }

        public override string GetRowCountFieldName()
        {
            return "lsTekiyo";
        }

        public override Dictionary<string, string> GetExtralData()
        {
            return _extralData;
        }

        public void UpdateDrawForm()
        {
            _hasNextPage = true;

            #region SubMethod

            #region Header

            var celldata = new Dictionary<string, CellModel>();
            //タイトル
            SingleData.Add("Title", printConf.ReportName);
            //医療機関名
            _extralData.Add("HeaderR_0_0_" + _currentPage, hpInf.HpName);
            //作成日時
            _extralData.Add("HeaderR_0_1_" + _currentPage, CIUtil.SDateToShowSWDate(
                CIUtil.ShowSDateToSDate(DateTime.Now.ToString("yyyy/MM/dd")), 0, 1
            ) + DateTime.Now.ToString(" HH:mm") + "作成");
            //ページ数
            int totalPage = (int)Math.Ceiling((double)printDatas.Count / maxRow);
            _extralData.Add("HeaderR_0_2_" + _currentPage, CurrentPage + " / " + totalPage);
            //入金日
            _extralData.Add("HeaderL_0_1_" + _currentPage, headerL1.Count >= CurrentPage ? headerL1[CurrentPage - 1] : "");
            //改ページ条件
            _extralData.Add("HeaderL_0_2_" + _currentPage, headerL2.Count >= CurrentPage ? headerL2[CurrentPage - 1] : "");

            //期間
            SingleData.Add("Range",
                string.Format(
                    "期間: {0} ～ {1}",
                    CIUtil.SDateToShowSWDate(printConf.StartNyukinDate, 0, 1),
                    CIUtil.SDateToShowSWDate(printConf.EndNyukinDate, 0, 1)
                )
            );


            #endregion

            #region Body

            int ptIndex = (CurrentPage - 1) * maxRow;
            int lineCount = 0;

            //存在しているフィールドに絞り込み
            var existsCols = putCurColumns.Where(p => _objectRseList.Contains(p.ColName)).Select(p => p.ColName).ToList();

            for (short rowNo = 0; rowNo < maxRow; rowNo++)
            {
                var printData = printDatas[ptIndex];
                string baseListName = "";

                //保険外金額（内訳）タイトル
                foreach (var jihiSbtMst in jihiSbtMsts)
                {
                    SingleData.Add(string.Format("tJihiFutanSbt{0}", jihiSbtMst.JihiSbt), jihiSbtMst.Name);
                }

                //明細データ出力
                foreach (var colName in existsCols)
                {
                    var data = new Dictionary<string, CellModel>();
                    var value = typeof(CoSta1001PrintData).GetProperty(colName).GetValue(printData);
                    data.Add("colName", new CellModel(value == null ? "" : value.ToString()));
                    CellData.Add(data);
                    if (baseListName == "" && _objectRseList.Contains(colName))
                    {
                        baseListName = colName;
                    }
                }
                //自費種別毎の金額
                for (int i = 0; i <= jihiSbtMsts.Count - 1; i++)
                {
                    if (printData.JihiSbtFutans == null) break;
                    var data = new Dictionary<string, CellModel>();
                    var jihiSbtMst = jihiSbtMsts[i];
                    data.Add(string.Format("JihiFutanSbt{0}", jihiSbtMst.JihiSbt), new CellModel(printData.JihiSbtFutans[i]));

                    CellData.Add(data);
                }

                //合計行キャプションと件数
                celldata.Add("TotalCaption", new CellModel(printData.TotalCaption));
                celldata.Add("TotalCount", new CellModel(printData.TotalCount));
                celldata.Add("TotalPtCount", new CellModel(printData.TotalPtCount));

                CellData.Add(celldata);
                celldata.Clear();

                //5行毎に区切り線を引く
                lineCount = printData.RowType != RowType.Brank ? lineCount + 1 : lineCount;

                //if (lineCount == 5)
                //{
                //    lineCount = 0;
                //    (long startX1, long startY1, long endX1, long endY1) = CoRep.GetBounds("headerLine");
                //    (long startX2, long startY2, long endX2, long endY2) = CoRep.GetListRowBounds(baseListName, rowNo);

                //    CoRep.DrawLine(startX1, endY2, endX1, endY2, 10, Hos.CnDraw.Constants.ConLineStyle.Dash);
                //}

                ptIndex++;
                if (ptIndex >= printDatas.Count)
                {
                    _hasNextPage = false;
                    break;
                }
            }

            #endregion

            #endregion
        }
    }
}
