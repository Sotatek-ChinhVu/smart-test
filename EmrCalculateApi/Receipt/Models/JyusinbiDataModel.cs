using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.Tenant;
using Helper.Common;

namespace EmrCalculateApi.Receipt.Models
{
    public class JyusinbiDataModel
    {
        ReceInfJdModel ReceInfJd;

        public JyusinbiDataModel(ReceInfJdModel receInfJd)
        {
            ReceInfJd = receInfJd;
        }
        /// <summary>
        /// レコード識別情報
        /// </summary>
        public string RecId
        {
            get { return "JD"; }
        }
        /// <summary>
        /// 負担者種別
        /// 1. 医療保険、国民健康保険、退職者医療又は後期高齢者医療
        /// 2.第１公費負担医療
        /// 3.第２公費負担医療
        /// 4.第３公費負担医療
        /// 5.第４公費負担医療
        /// </summary>
        public int FutansyaSbt 
        {
            get{ return (ReceInfJd != null ? ReceInfJd.FutanSbtCd : 1); }
        }

        /// <summary>
        /// 日の情報
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public int Nissu(int index)
        {
            int ret = 0;

            switch(index)
            {
                case 1: ret = ReceInfJd.Nissu1; break;
                case 2: ret = ReceInfJd.Nissu2; break;
                case 3: ret = ReceInfJd.Nissu3; break;
                case 4: ret = ReceInfJd.Nissu4; break;
                case 5: ret = ReceInfJd.Nissu5; break;
                case 6: ret = ReceInfJd.Nissu6; break;
                case 7: ret = ReceInfJd.Nissu7; break;
                case 8: ret = ReceInfJd.Nissu8; break;
                case 9: ret = ReceInfJd.Nissu9; break;
                case 10: ret = ReceInfJd.Nissu10; break;
                case 11: ret = ReceInfJd.Nissu11; break;
                case 12: ret = ReceInfJd.Nissu12; break;
                case 13: ret = ReceInfJd.Nissu13; break;
                case 14: ret = ReceInfJd.Nissu14; break;
                case 15: ret = ReceInfJd.Nissu15; break;
                case 16: ret = ReceInfJd.Nissu16; break;
                case 17: ret = ReceInfJd.Nissu17; break;
                case 18: ret = ReceInfJd.Nissu18; break;
                case 19: ret = ReceInfJd.Nissu19; break;
                case 20: ret = ReceInfJd.Nissu20; break;
                case 21: ret = ReceInfJd.Nissu21; break;
                case 22: ret = ReceInfJd.Nissu22; break;
                case 23: ret = ReceInfJd.Nissu23; break;
                case 24: ret = ReceInfJd.Nissu24; break;
                case 25: ret = ReceInfJd.Nissu25; break;
                case 26: ret = ReceInfJd.Nissu26; break;
                case 27: ret = ReceInfJd.Nissu27; break;
                case 28: ret = ReceInfJd.Nissu28; break;
                case 29: ret = ReceInfJd.Nissu29; break;
                case 30: ret = ReceInfJd.Nissu30; break;
                case 31: ret = ReceInfJd.Nissu31; break;
            }

            return ret;
        }
        public string JDRecord
        {
            get
            {
                string ret = "";

                // レコード識別
                ret += RecId;
                // 負担者種別
                ret += "," + FutansyaSbt;
                // 日の情報
                for(int i = 1; i <= 31; i++)
                {
                    ret += "," + CIUtil.ToStringIgnoreZero(Nissu(i));
                }
                
                return ret;
            }
        }

    }
}
