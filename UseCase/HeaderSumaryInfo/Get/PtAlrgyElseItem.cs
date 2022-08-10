using Domain.Models.PtAlrgyElse;
using Helper.Extendsions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.HeaderSumaryInfo.Get
{
    public class PtAlrgyElseItem
    {
        public PtAlrgyElseModel PtAlrgyElse { get; }

        public PtAlrgyElseItem(PtAlrgyElseModel ptAlrgyElse)
        {
            PtAlrgyElse = ptAlrgyElse;
        }

        /// <summary>
        /// アレルギー名称
        /// 
        /// </summary>
        public string AlrgyName
        {
            get { return PtAlrgyElse.AlrgyName; }
        }

        /// <summary>
        /// コメント
        /// 
        /// </summary>
        public string Cmt
        {
            get { return PtAlrgyElse.Cmt; }
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
            get { return PtAlrgyElse.StartDate; }
        }

        /// <summary>
        /// 消失日
        /// yyyymmdd or yyyymm
        /// </summary>
        public int EndDate
        {
            get { return PtAlrgyElse.EndDate; }
        }
    }
}
