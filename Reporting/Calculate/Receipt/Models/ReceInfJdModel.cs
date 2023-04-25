﻿using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Calculate.Receipt.Models
{
    public class ReceInfJdModel
    {
        public ReceInfJd ReceInfJd { get; } = null;

        public ReceInfJdModel(ReceInfJd receInfJd)
        {
            ReceInfJd = receInfJd;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return ReceInfJd.HpId; }
            set
            {
                if (ReceInfJd.HpId == value) return;
                ReceInfJd.HpId = value;
                //RaisePropertyChanged(() => HpId);
            }
        }

        /// <summary>
        /// 請求年月
        /// 
        /// </summary>
        public int SeikyuYm
        {
            get { return ReceInfJd.SeikyuYm; }
            set
            {
                if (ReceInfJd.SeikyuYm == value) return;
                ReceInfJd.SeikyuYm = value;
                //RaisePropertyChanged(() => SeikyuYm);
            }
        }

        /// <summary>
        /// 患者ID
        /// 
        /// </summary>
        public long PtId
        {
            get { return ReceInfJd.PtId; }
            set
            {
                if (ReceInfJd.PtId == value) return;
                ReceInfJd.PtId = value;
                //RaisePropertyChanged(() => PtId);
            }
        }

        /// <summary>
        /// 診療年月
        /// 
        /// </summary>
        public int SinYm
        {
            get { return ReceInfJd.SinYm; }
            set
            {
                if (ReceInfJd.SinYm == value) return;
                ReceInfJd.SinYm = value;
                //RaisePropertyChanged(() => SinYm);
            }
        }

        /// <summary>
        /// 主保険保険ID
        /// 
        /// </summary>
        public int HokenId
        {
            get { return ReceInfJd.HokenId; }
            set
            {
                if (ReceInfJd.HokenId == value) return;
                ReceInfJd.HokenId = value;
                //RaisePropertyChanged(() => HokenId);
            }
        }

        /// <summary>
        /// 公費保険ID
        /// 
        /// </summary>
        public int KohiId
        {
            get { return ReceInfJd.KohiId; }
            set
            {
                if (ReceInfJd.KohiId == value) return;
                ReceInfJd.KohiId = value;
                //RaisePropertyChanged(() => KohiId);
            }
        }

        /// <summary>
        /// 負担者種別コード
        ///     1:保険 2:公1 3:公2 4:公3 5:公4
        /// </summary>
        public int FutanSbtCd
        {
            get { return ReceInfJd.FutanSbtCd; }
            set
            {
                if (ReceInfJd.FutanSbtCd == value) return;
                ReceInfJd.FutanSbtCd = value;
                //RaisePropertyChanged(() => FutanSbtCd);
            }
        }

        /// <summary>
        /// 受診等区分コード(1日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        public int Nissu1
        {
            get { return ReceInfJd.Nissu1; }
            set
            {
                if (ReceInfJd.Nissu1 == value) return;
                ReceInfJd.Nissu1 = value;
                //RaisePropertyChanged(() => Nissu1);
            }
        }

        /// <summary>
        /// 　受診等区分コード(2日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        public int Nissu2
        {
            get { return ReceInfJd.Nissu2; }
            set
            {
                if (ReceInfJd.Nissu2 == value) return;
                ReceInfJd.Nissu2 = value;
                //RaisePropertyChanged(() => Nissu2);
            }
        }

        /// <summary>
        /// 　受診等区分コード(3日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        public int Nissu3
        {
            get { return ReceInfJd.Nissu3; }
            set
            {
                if (ReceInfJd.Nissu3 == value) return;
                ReceInfJd.Nissu3 = value;
                //RaisePropertyChanged(() => Nissu3);
            }
        }

        /// <summary>
        /// 　受診等区分コード(4日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        public int Nissu4
        {
            get { return ReceInfJd.Nissu4; }
            set
            {
                if (ReceInfJd.Nissu4 == value) return;
                ReceInfJd.Nissu4 = value;
                //RaisePropertyChanged(() => Nissu4);
            }
        }

        /// <summary>
        /// 　受診等区分コード(5日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        public int Nissu5
        {
            get { return ReceInfJd.Nissu5; }
            set
            {
                if (ReceInfJd.Nissu5 == value) return;
                ReceInfJd.Nissu5 = value;
                //RaisePropertyChanged(() => Nissu5);
            }
        }

        /// <summary>
        /// 　受診等区分コード(6日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        public int Nissu6
        {
            get { return ReceInfJd.Nissu6; }
            set
            {
                if (ReceInfJd.Nissu6 == value) return;
                ReceInfJd.Nissu6 = value;
                //RaisePropertyChanged(() => Nissu6);
            }
        }

        /// <summary>
        /// 　受診等区分コード(7日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        public int Nissu7
        {
            get { return ReceInfJd.Nissu7; }
            set
            {
                if (ReceInfJd.Nissu7 == value) return;
                ReceInfJd.Nissu7 = value;
                //RaisePropertyChanged(() => Nissu7);
            }
        }

        /// <summary>
        /// 　受診等区分コード(8日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        public int Nissu8
        {
            get { return ReceInfJd.Nissu8; }
            set
            {
                if (ReceInfJd.Nissu8 == value) return;
                ReceInfJd.Nissu8 = value;
                //RaisePropertyChanged(() => Nissu8);
            }
        }

        /// <summary>
        /// 　受診等区分コード(9日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        public int Nissu9
        {
            get { return ReceInfJd.Nissu9; }
            set
            {
                if (ReceInfJd.Nissu9 == value) return;
                ReceInfJd.Nissu9 = value;
                //RaisePropertyChanged(() => Nissu9);
            }
        }

        /// <summary>
        /// 　受診等区分コード(10日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        public int Nissu10
        {
            get { return ReceInfJd.Nissu10; }
            set
            {
                if (ReceInfJd.Nissu10 == value) return;
                ReceInfJd.Nissu10 = value;
                //RaisePropertyChanged(() => Nissu10);
            }
        }

        /// <summary>
        /// 　受診等区分コード(11日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        public int Nissu11
        {
            get { return ReceInfJd.Nissu11; }
            set
            {
                if (ReceInfJd.Nissu11 == value) return;
                ReceInfJd.Nissu11 = value;
                //RaisePropertyChanged(() => Nissu11);
            }
        }

        /// <summary>
        /// 　受診等区分コード(12日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        public int Nissu12
        {
            get { return ReceInfJd.Nissu12; }
            set
            {
                if (ReceInfJd.Nissu12 == value) return;
                ReceInfJd.Nissu12 = value;
                //RaisePropertyChanged(() => Nissu12);
            }
        }

        /// <summary>
        /// 　受診等区分コード(13日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        public int Nissu13
        {
            get { return ReceInfJd.Nissu13; }
            set
            {
                if (ReceInfJd.Nissu13 == value) return;
                ReceInfJd.Nissu13 = value;
                //RaisePropertyChanged(() => Nissu13);
            }
        }

        /// <summary>
        /// 　受診等区分コード(14日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        public int Nissu14
        {
            get { return ReceInfJd.Nissu14; }
            set
            {
                if (ReceInfJd.Nissu14 == value) return;
                ReceInfJd.Nissu14 = value;
                //RaisePropertyChanged(() => Nissu14);
            }
        }

        /// <summary>
        /// 　受診等区分コード(15日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        public int Nissu15
        {
            get { return ReceInfJd.Nissu15; }
            set
            {
                if (ReceInfJd.Nissu15 == value) return;
                ReceInfJd.Nissu15 = value;
                //RaisePropertyChanged(() => Nissu15);
            }
        }

        /// <summary>
        /// 　受診等区分コード(16日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        public int Nissu16
        {
            get { return ReceInfJd.Nissu16; }
            set
            {
                if (ReceInfJd.Nissu16 == value) return;
                ReceInfJd.Nissu16 = value;
                //RaisePropertyChanged(() => Nissu16);
            }
        }

        /// <summary>
        /// 　受診等区分コード(17日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        public int Nissu17
        {
            get { return ReceInfJd.Nissu17; }
            set
            {
                if (ReceInfJd.Nissu17 == value) return;
                ReceInfJd.Nissu17 = value;
                //RaisePropertyChanged(() => Nissu17);
            }
        }

        /// <summary>
        /// 　受診等区分コード(18日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        public int Nissu18
        {
            get { return ReceInfJd.Nissu18; }
            set
            {
                if (ReceInfJd.Nissu18 == value) return;
                ReceInfJd.Nissu18 = value;
                //RaisePropertyChanged(() => Nissu18);
            }
        }

        /// <summary>
        /// 　受診等区分コード(19日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        public int Nissu19
        {
            get { return ReceInfJd.Nissu19; }
            set
            {
                if (ReceInfJd.Nissu19 == value) return;
                ReceInfJd.Nissu19 = value;
                //RaisePropertyChanged(() => Nissu19);
            }
        }

        /// <summary>
        /// 　受診等区分コード(20日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        public int Nissu20
        {
            get { return ReceInfJd.Nissu20; }
            set
            {
                if (ReceInfJd.Nissu20 == value) return;
                ReceInfJd.Nissu20 = value;
                //RaisePropertyChanged(() => Nissu20);
            }
        }

        /// <summary>
        /// 　受診等区分コード(21日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        public int Nissu21
        {
            get { return ReceInfJd.Nissu21; }
            set
            {
                if (ReceInfJd.Nissu21 == value) return;
                ReceInfJd.Nissu21 = value;
                //RaisePropertyChanged(() => Nissu21);
            }
        }

        /// <summary>
        /// 　受診等区分コード(22日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        public int Nissu22
        {
            get { return ReceInfJd.Nissu22; }
            set
            {
                if (ReceInfJd.Nissu22 == value) return;
                ReceInfJd.Nissu22 = value;
                //RaisePropertyChanged(() => Nissu22);
            }
        }

        /// <summary>
        /// 　受診等区分コード(23日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        public int Nissu23
        {
            get { return ReceInfJd.Nissu23; }
            set
            {
                if (ReceInfJd.Nissu23 == value) return;
                ReceInfJd.Nissu23 = value;
                //RaisePropertyChanged(() => Nissu23);
            }
        }

        /// <summary>
        /// 　受診等区分コード(24日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        public int Nissu24
        {
            get { return ReceInfJd.Nissu24; }
            set
            {
                if (ReceInfJd.Nissu24 == value) return;
                ReceInfJd.Nissu24 = value;
                //RaisePropertyChanged(() => Nissu24);
            }
        }

        /// <summary>
        /// 　受診等区分コード(25日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        public int Nissu25
        {
            get { return ReceInfJd.Nissu25; }
            set
            {
                if (ReceInfJd.Nissu25 == value) return;
                ReceInfJd.Nissu25 = value;
                //RaisePropertyChanged(() => Nissu25);
            }
        }

        /// <summary>
        /// 　受診等区分コード(26日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        public int Nissu26
        {
            get { return ReceInfJd.Nissu26; }
            set
            {
                if (ReceInfJd.Nissu26 == value) return;
                ReceInfJd.Nissu26 = value;
                //RaisePropertyChanged(() => Nissu26);
            }
        }

        /// <summary>
        /// 　受診等区分コード(27日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        public int Nissu27
        {
            get { return ReceInfJd.Nissu27; }
            set
            {
                if (ReceInfJd.Nissu27 == value) return;
                ReceInfJd.Nissu27 = value;
                //RaisePropertyChanged(() => Nissu27);
            }
        }

        /// <summary>
        /// 　受診等区分コード(28日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        public int Nissu28
        {
            get { return ReceInfJd.Nissu28; }
            set
            {
                if (ReceInfJd.Nissu28 == value) return;
                ReceInfJd.Nissu28 = value;
                //RaisePropertyChanged(() => Nissu28);
            }
        }

        /// <summary>
        /// 　受診等区分コード(29日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        public int Nissu29
        {
            get { return ReceInfJd.Nissu29; }
            set
            {
                if (ReceInfJd.Nissu29 == value) return;
                ReceInfJd.Nissu29 = value;
                //RaisePropertyChanged(() => Nissu29);
            }
        }

        /// <summary>
        /// 　受診等区分コード(30日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        public int Nissu30
        {
            get { return ReceInfJd.Nissu30; }
            set
            {
                if (ReceInfJd.Nissu30 == value) return;
                ReceInfJd.Nissu30 = value;
                //RaisePropertyChanged(() => Nissu30);
            }
        }

        /// <summary>
        /// 　受診等区分コード(31日の情報)
        ///     1:実日数に計上する受診 2:実日数に計上しない受診
        /// </summary>
        public int Nissu31
        {
            get { return ReceInfJd.Nissu31; }
            set
            {
                if (ReceInfJd.Nissu31 == value) return;
                ReceInfJd.Nissu31 = value;
                //RaisePropertyChanged(() => Nissu31);
            }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return ReceInfJd.CreateDate; }
            set
            {
                if (ReceInfJd.CreateDate == value) return;
                ReceInfJd.CreateDate = value;
                //RaisePropertyChanged(() => CreateDate);
            }
        }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        public int CreateId
        {
            get { return ReceInfJd.CreateId; }
            set
            {
                if (ReceInfJd.CreateId == value) return;
                ReceInfJd.CreateId = value;
                //RaisePropertyChanged(() => CreateId);
            }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return ReceInfJd.CreateMachine; }
            set
            {
                if (ReceInfJd.CreateMachine == value) return;
                ReceInfJd.CreateMachine = value;
                //RaisePropertyChanged(() => CreateMachine);
            }
        }


    }

}
