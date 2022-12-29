using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Receipt.Constants
{
    public class SinMeiMode
    {
        /// <summary>
        /// レセプト電算
        /// </summary>
        public static int Receden = 0;
        /// <summary>
        /// 紙レセプト
        /// </summary>
        public static int PaperRece = 1;
        /// <summary>
        /// レセチェック
        /// </summary>
        public static int ReceCheck = 2;
        /// <summary>
        /// 窓口精算画面等
        /// </summary>
        public static int Kaikei = 3;
        /// <summary>
        /// アフターケア
        /// </summary>
        public static int AfterCare = 4;
        /// <summary>
        /// 領収証
        /// </summary>
        public static int Ryosyu = 5;
        /// <summary>
        /// 紙レセプト点数欄
        /// </summary>
        public static int ReceTensu = 11;    //点数欄用
        /// <summary>
        /// 労災レセプト点数欄
        /// </summary>
        public static int ReceTensuRousai = 12;    //点数欄労災用
        /// <summary>
        /// アフターケア点数欄
        /// </summary>
        public static int ReceTensuAfter = 13;
        /// <summary>
        /// 会計カード
        /// </summary>
        public static int AccountingCard = 21;
        /// <summary>
        /// レセ電アフターケア
        /// </summary>
        public static int RecedenAfter = 22;
    }
}
