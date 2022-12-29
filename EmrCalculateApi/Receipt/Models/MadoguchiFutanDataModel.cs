using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Receipt.Models
{
    public class MadoguchiFutanDataModel
    {
        /// <summary>
        /// レコード識別情報
        /// </summary>
        public string RecId
        {
            get { return "MF"; }
        }
        /// <summary>
        /// 窓口負担額の区分
        /// 00. 高額療養費の現物給付なし
        /// 01. 高額療養費現物給付あり（多数回該当を除く）
        /// 02. 高額療養費現物給付あり（多数回該当）
        /// </summary>
        public int MadoguchiFutanKbn { get; set; } = 0;
        public string Yobi1 { get; set; } = string.Empty;
        public string Yobi2 { get; set; } = string.Empty;
        public string Yobi3 { get; set; } = string.Empty;
        public string Yobi4 { get; set; } = string.Empty;
        public string Yobi5 { get; set; } = string.Empty;
        public string Yobi6 { get; set; } = string.Empty;
        public string Yobi7 { get; set; } = string.Empty;
        public string Yobi8 { get; set; } = string.Empty;
        public string Yobi9 { get; set; } = string.Empty;
        public string Yobi10 { get; set; } = string.Empty;
        public string Yobi11 { get; set; } = string.Empty;
        public string Yobi12 { get; set; } = string.Empty;
        public string Yobi13 { get; set; } = string.Empty;
        public string Yobi14 { get; set; } = string.Empty;
        public string Yobi15 { get; set; } = string.Empty;
        public string Yobi16 { get; set; } = string.Empty;
        public string Yobi17 { get; set; } = string.Empty;
        public string Yobi18 { get; set; } = string.Empty;
        public string Yobi19 { get; set; } = string.Empty;
        public string Yobi20 { get; set; } = string.Empty;
        public string Yobi21 { get; set; } = string.Empty;
        public string Yobi22 { get; set; } = string.Empty;
        public string Yobi23 { get; set; } = string.Empty;
        public string Yobi24 { get; set; } = string.Empty;
        public string Yobi25 { get; set; } = string.Empty;
        public string Yobi26 { get; set; } = string.Empty;
        public string Yobi27 { get; set; } = string.Empty;
        public string Yobi28 { get; set; } = string.Empty;
        public string Yobi29 { get; set; } = string.Empty;
        public string Yobi30 { get; set; } = string.Empty;
        public string Yobi31 { get; set; } = string.Empty;
        public string Yobi(int index)
        {
            string ret = string.Empty;

            switch (index)
            {
                case 1: ret = Yobi1; break;
                case 2: ret = Yobi2; break;
                case 3: ret = Yobi3; break;
                case 4: ret = Yobi4; break;
                case 5: ret = Yobi5; break;
                case 6: ret = Yobi6; break;
                case 7: ret = Yobi7; break;
                case 8: ret = Yobi8; break;
                case 9: ret = Yobi9; break;
                case 10: ret = Yobi10; break;
                case 11: ret = Yobi11; break;
                case 12: ret = Yobi12; break;
                case 13: ret = Yobi13; break;
                case 14: ret = Yobi14; break;
                case 15: ret = Yobi15; break;
                case 16: ret = Yobi16; break;
                case 17: ret = Yobi17; break;
                case 18: ret = Yobi18; break;
                case 19: ret = Yobi19; break;
                case 20: ret = Yobi20; break;
                case 21: ret = Yobi21; break;
                case 22: ret = Yobi22; break;
                case 23: ret = Yobi23; break;
                case 24: ret = Yobi24; break;
                case 25: ret = Yobi25; break;
                case 26: ret = Yobi26; break;
                case 27: ret = Yobi27; break;
                case 28: ret = Yobi28; break;
                case 29: ret = Yobi29; break;
                case 30: ret = Yobi30; break;
                case 31: ret = Yobi31; break;
            }

            return ret;
        }
        public string MFRecord
        {
            get
            {
                string ret = "";

                // レコード識別
                ret += RecId;
                // 窓口負担額の区分
                ret += "," + MadoguchiFutanKbn.ToString().PadLeft(2, '0');
                // 予備
                for (int i = 1; i <= 31; i++)
                {
                    ret += "," +Yobi(i);
                }

                return ret;
            }
        }
    }
}
