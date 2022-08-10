using Domain.Models.PtOtcDrug;
using Helper.Extendsions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.HeaderSumaryInfo.Get
{
    public class PtOtcDrugItem
    {
        public PtOtcDrugModel PtOtcDrug { get; }

        public PtOtcDrugItem(PtOtcDrugModel ptOtcDrug)
        {
            PtOtcDrug = ptOtcDrug;
        }

        /// <summary>
        /// 患者ID
        ///  患者を識別するためのシステム固有の番号       
        /// </summary>
        public long PtId
        {
            get { return PtOtcDrug.PtId; }
        }

        /// <summary>
        /// シリアルナンバー
        /// 
        /// </summary>
        public int SerialNum
        {
            get { return PtOtcDrug.SerialNum; }
        }

        /// <summary>
        /// 商品名
        /// 
        /// </summary>
        public string TradeName
        {
            get { return PtOtcDrug.TradeName; }
        }

        /// <summary>
        /// コメント
        /// 
        /// </summary>
        public string Cmt
        {
            get { return PtOtcDrug.Cmt; }
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
            get { return PtOtcDrug.StartDate; }
        }

        /// <summary>
        /// 消失日
        /// yyyymmdd or yyyymm
        /// </summary>
        public int EndDate
        {
            get { return PtOtcDrug.EndDate; }
        }
    }
}
