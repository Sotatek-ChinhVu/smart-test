using Domain.Models.PtSupple;
using Helper.Extendsions;

namespace UseCase.HeaderSumaryInfo.Get
{
    public class PtSuppleItem
    {
        public PtSuppleModel PtSupple { get; }

        public PtSuppleItem(PtSuppleModel ptSupple)
        {
            PtSupple = ptSupple;
        }

        /// <summary>
        /// 索引語コード
        /// 
        /// </summary>
        public string IndexCd
        {
            get { return PtSupple.IndexCd; }
        }

        /// <summary>
        /// 索引語
        /// 
        /// </summary>
        public string IndexWord
        {
            get { return PtSupple.IndexWord; }
        }

        /// <summary>
        /// コメント
        /// 
        /// </summary>
        public string Cmt
        {
            get { return PtSupple.Cmt; }
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
            get { return PtSupple.StartDate; }
        }

        /// <summary>
        /// 消失日
        /// yyyymmdd or yyyymm
        /// </summary>
        public int EndDate
        {
            get { return PtSupple.EndDate; }
        }
    }
}
