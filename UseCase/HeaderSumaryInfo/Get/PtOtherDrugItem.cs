using Domain.Models.PtOtherDrug;
using Helper.Extendsions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.HeaderSumaryInfo.Get
{
    public class PtOtherDrugItem
    {
        public PtOtherDrugModel PtOtherDrug { get; }

        public PtOtherDrugItem(PtOtherDrugModel ptOtherDrug)
        {
            PtOtherDrug = ptOtherDrug;
        }

        /// <summary>
        /// 患者ID
        ///  患者を識別するためのシステム固有の番号       
        /// </summary>
        public long PtId
        {
            get { return PtOtherDrug.PtId; }
        }

        /// <summary>
        /// 項目コード
        /// 
        /// </summary>
        public string ItemCd
        {
            get { return PtOtherDrug.ItemCd; }
        }

        /// <summary>
        /// 医薬品名称
        /// 
        /// </summary>
        public string DrugName
        {
            get { return PtOtherDrug.DrugName; }
        }

        /// <summary>
        /// コメント
        /// 
        /// </summary>
        public string Cmt
        {
            get { return PtOtherDrug.Cmt; }
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
            get { return PtOtherDrug.StartDate; }
        }

        /// <summary>
        /// 消失日
        /// yyyymmdd or yyyymm
        /// </summary>
        public int EndDate
        {
            get { return PtOtherDrug.EndDate; }
        }
    }
}
