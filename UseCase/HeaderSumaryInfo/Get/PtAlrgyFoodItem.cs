using Domain.Models.PtAlrgyElse;
using Domain.Models.PtAlrgyFood;
using Helper.Extendsions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCase.HeaderSumaryInfo.Get
{
    public class PtAlrgyFoodItem: ObservableObject
    {
        public PtAlrgyFoodModel PtAlrgyFood { get; }

        public PtAlrgyFoodItem(PtAlrgyFoodModel ptAlrgyFood)
        {
            PtAlrgyFood = ptAlrgyFood;
        }

        /// <summary>
        /// アレルギー区分
        /// 
        /// </summary>
        public string AlrgyKbn
        {
            get { return PtAlrgyFood.AlrgyKbn; }
        }

        /// <summary>
        /// コメント
        /// 
        /// </summary>
        public string Cmt
        {
            get { return PtAlrgyFood.Cmt; }
        }

        private string foodName = string.Empty;
        public string FoodName
        {
            get => foodName;
            set { Set(ref foodName, value); }
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
            get { return PtAlrgyFood.StartDate; }
        }

        /// <summary>
        /// 消失日
        /// yyyymmdd or yyyymm
        /// </summary>
        public int EndDate
        {
            get { return PtAlrgyFood.EndDate; }
        }
    }
}
