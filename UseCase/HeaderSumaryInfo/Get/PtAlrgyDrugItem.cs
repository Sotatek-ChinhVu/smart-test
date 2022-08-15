using Domain.Models.PtAlrgyDrug;
using Helper.Extendsions;

namespace UseCase.HeaderSumaryInfo.Get
{
    public class PtAlrgyDrugItem : ObservableObject
    {
        public PtAlrgyDrugModel PtAlrgyDrug { get; }

        public PtAlrgyDrugItem(PtAlrgyDrugModel ptAlrgyDrug)
        {
            PtAlrgyDrug = ptAlrgyDrug;
        }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        public string ItemCd
        {
            get { return PtAlrgyDrug.ItemCd; }
        }

        /// <summary>
        /// 医薬品名称
        /// 
        /// </summary>
        public string DrugName
        {
            get { return PtAlrgyDrug.DrugName; }
        }

        /// <summary>
        /// コメント
        /// 
        /// </summary>
        public string Cmt
        {
            get { return PtAlrgyDrug.Cmt; }
        }

        public int FullStartDate
        {
            get
            {
                if (StartDate.AsString().Count() == 8)
                {
                    //Format of StartDate is yyyymmdd
                    return StartDate;
                }
                else
                {
                    //Format of StartDate is yyyymm
                    //Need to convert to yyyymm01
                    return StartDate * 100 + 1;
                }
            }
        }

        public int FullEndDate
        {
            get
            {
                if (EndDate.AsString().Count() == 8)
                {
                    //Format of EndDate is yyyymmdd
                    return EndDate;
                }
                else
                {
                    //Format of EndDate is yyyymm
                    //Need to convert to yyyymm31
                    return EndDate * 100 + 31;
                }
            }
        }
        public int StartDate
        {
            get { return PtAlrgyDrug.StartDate; }
        }

        /// <summary>
        /// 消失日
        /// yyyymmdd or yyyymm
        /// </summary>
        public int EndDate
        {
            get { return PtAlrgyDrug.EndDate; }
        }
    }
}
