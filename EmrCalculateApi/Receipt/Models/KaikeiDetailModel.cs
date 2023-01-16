using Entity.Tenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmrCalculateApi.Receipt.Models
{
    public class KaikeiDetailModel
    {
        public KaikeiDetail KaikeiDetail { get; } = null;

        public KaikeiDetailModel(KaikeiDetail kaikeiDetail)
        {
            KaikeiDetail = kaikeiDetail;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId
        {
            get { return KaikeiDetail.HpId; }
            set
            {
                if (KaikeiDetail.HpId == value) return;
                KaikeiDetail.HpId = value;
                //RaisePropertyChanged(() => HpId);
            }
        }

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId
        {
            get { return KaikeiDetail.PtId; }
            set
            {
                if (KaikeiDetail.PtId == value) return;
                KaikeiDetail.PtId = value;
                //RaisePropertyChanged(() => PtId);
            }
        }

        /// <summary>
        /// 診療日
        /// 
        /// </summary>
        public int SinDate
        {
            get { return KaikeiDetail.SinDate; }
            set
            {
                if (KaikeiDetail.SinDate == value) return;
                KaikeiDetail.SinDate = value;
                //RaisePropertyChanged(() => SinDate);
            }
        }

        /// <summary>
        /// 来院番号
        /// 
        /// </summary>
        public long RaiinNo
        {
            get { return KaikeiDetail.RaiinNo; }
            set
            {
                if (KaikeiDetail.RaiinNo == value) return;
                KaikeiDetail.RaiinNo = value;
                //RaisePropertyChanged(() => RaiinNo);
            }
        }

        /// <summary>
        /// 親来院番号
        /// 
        /// </summary>
        public long OyaRaiinNo
        {
            get { return KaikeiDetail.OyaRaiinNo; }
            set
            {
                if (KaikeiDetail.OyaRaiinNo == value) return;
                KaikeiDetail.OyaRaiinNo = value;
                //RaisePropertyChanged(() => OyaRaiinNo);
            }
        }

        /// <summary>
        /// 保険組合せID
        /// 
        /// </summary>
        public int HokenPid
        {
            get { return KaikeiDetail.HokenPid; }
            set
            {
                if (KaikeiDetail.HokenPid == value) return;
                KaikeiDetail.HokenPid = value;
                //RaisePropertyChanged(() => HokenPid);
            }
        }

        /// <summary>
        /// 合算調整Pid
        /// 
        /// </summary>
        public int AdjustPid
        {
            get { return KaikeiDetail.AdjustPid; }
            set
            {
                if (KaikeiDetail.AdjustPid == value) return;
                KaikeiDetail.AdjustPid = value;
                //RaisePropertyChanged(() => AdjustPid);
            }
        }

        /// <summary>
        /// 合算調整KohiId
        /// 
        /// </summary>
        public int AdjustKid
        {
            get { return KaikeiDetail.AdjustKid; }
            set
            {
                if (KaikeiDetail.AdjustKid == value) return;
                KaikeiDetail.AdjustKid = value;
                //RaisePropertyChanged(() => AdjustKid);
            }
        }

        /// <summary>
        /// 保険区分
        /// 
        /// </summary>
        public int HokenKbn
        {
            get { return KaikeiDetail.HokenKbn; }
            set
            {
                if (KaikeiDetail.HokenKbn == value) return;
                KaikeiDetail.HokenKbn = value;
                //RaisePropertyChanged(() => HokenKbn);
            }
        }

        /// <summary>
        /// 保険種別コード
        /// 
        /// </summary>
        public int HokenSbtCd
        {
            get { return KaikeiDetail.HokenSbtCd; }
            set
            {
                if (KaikeiDetail.HokenSbtCd == value) return;
                KaikeiDetail.HokenSbtCd = value;
                //RaisePropertyChanged(() => HokenSbtCd);
            }
        }

        /// <summary>
        /// 主保険保険ID
        /// 
        /// </summary>
        public int HokenId
        {
            get { return KaikeiDetail.HokenId; }
            set
            {
                if (KaikeiDetail.HokenId == value) return;
                KaikeiDetail.HokenId = value;
                //RaisePropertyChanged(() => HokenId);
            }
        }

        /// <summary>
        /// 公費１保険ID
        /// 
        /// </summary>
        public int Kohi1Id
        {
            get { return KaikeiDetail.Kohi1Id; }
            set
            {
                if (KaikeiDetail.Kohi1Id == value) return;
                KaikeiDetail.Kohi1Id = value;
                //RaisePropertyChanged(() => Kohi1Id);
            }
        }

        /// <summary>
        /// 公費２保険ID
        /// 
        /// </summary>
        public int Kohi2Id
        {
            get { return KaikeiDetail.Kohi2Id; }
            set
            {
                if (KaikeiDetail.Kohi2Id == value) return;
                KaikeiDetail.Kohi2Id = value;
                //RaisePropertyChanged(() => Kohi2Id);
            }
        }

        /// <summary>
        /// 公費３保険ID
        /// 
        /// </summary>
        public int Kohi3Id
        {
            get { return KaikeiDetail.Kohi3Id; }
            set
            {
                if (KaikeiDetail.Kohi3Id == value) return;
                KaikeiDetail.Kohi3Id = value;
                //RaisePropertyChanged(() => Kohi3Id);
            }
        }

        /// <summary>
        /// 公費４保険ID
        /// 
        /// </summary>
        public int Kohi4Id
        {
            get { return KaikeiDetail.Kohi4Id; }
            set
            {
                if (KaikeiDetail.Kohi4Id == value) return;
                KaikeiDetail.Kohi4Id = value;
                //RaisePropertyChanged(() => Kohi4Id);
            }
        }

        /// <summary>
        /// 労災保険ID
        /// 
        /// </summary>
        public int RousaiId
        {
            get { return KaikeiDetail.RousaiId; }
            set
            {
                if (KaikeiDetail.RousaiId == value) return;
                KaikeiDetail.RousaiId = value;
                //RaisePropertyChanged(() => RousaiId);
            }
        }

        /// <summary>
        /// 法別番号
        /// PT_HOKEN_INF.HOUBETU
        /// </summary>
        public string Houbetu
        {
            get { return KaikeiDetail.Houbetu; }
            set
            {
                if (KaikeiDetail.Houbetu == value) return;
                KaikeiDetail.Houbetu = value;
                //RaisePropertyChanged(() => Houbetu);
            }
        }

        /// <summary>
        /// 公１法別
        /// 
        /// </summary>
        public string Kohi1Houbetu
        {
            get { return KaikeiDetail.Kohi1Houbetu; }
            set
            {
                if (KaikeiDetail.Kohi1Houbetu == value) return;
                KaikeiDetail.Kohi1Houbetu = value;
                //RaisePropertyChanged(() => Kohi1Houbetu);
            }
        }

        /// <summary>
        /// 公２法別
        /// 
        /// </summary>
        public string Kohi2Houbetu
        {
            get { return KaikeiDetail.Kohi2Houbetu; }
            set
            {
                if (KaikeiDetail.Kohi2Houbetu == value) return;
                KaikeiDetail.Kohi2Houbetu = value;
                //RaisePropertyChanged(() => Kohi2Houbetu);
            }
        }

        /// <summary>
        /// 公３法別
        /// 
        /// </summary>
        public string Kohi3Houbetu
        {
            get { return KaikeiDetail.Kohi3Houbetu; }
            set
            {
                if (KaikeiDetail.Kohi3Houbetu == value) return;
                KaikeiDetail.Kohi3Houbetu = value;
                //RaisePropertyChanged(() => Kohi3Houbetu);
            }
        }

        /// <summary>
        /// 公４法別
        /// 
        /// </summary>
        public string Kohi4Houbetu
        {
            get { return KaikeiDetail.Kohi4Houbetu; }
            set
            {
                if (KaikeiDetail.Kohi4Houbetu == value) return;
                KaikeiDetail.Kohi4Houbetu = value;
                //RaisePropertyChanged(() => Kohi4Houbetu);
            }
        }

        /// <summary>
        /// 公費１優先順位
        ///     公費優先順位(都道府県番号+優先順位+法別番号)
        /// </summary>
        public string Kohi1Priority
        {
            get { return KaikeiDetail.Kohi1Priority; }
            set
            {
                if (KaikeiDetail.Kohi1Priority == value) return;
                KaikeiDetail.Kohi1Priority = value;
                //RaisePropertyChanged(() => Kohi1Priority);
            }
        }

        /// <summary>
        /// 公費２優先順位
        ///     公費優先順位(都道府県番号+優先順位+法別番号)
        /// </summary>
        public string Kohi2Priority
        {
            get { return KaikeiDetail.Kohi2Priority; }
            set
            {
                if (KaikeiDetail.Kohi2Priority == value) return;
                KaikeiDetail.Kohi2Priority = value;
                //RaisePropertyChanged(() => Kohi2Priority);
            }
        }

        /// <summary>
        /// 公費３優先順位
        ///     公費優先順位(都道府県番号+優先順位+法別番号)
        /// </summary>
        public string Kohi3Priority
        {
            get { return KaikeiDetail.Kohi3Priority; }
            set
            {
                if (KaikeiDetail.Kohi3Priority == value) return;
                KaikeiDetail.Kohi3Priority = value;
                //RaisePropertyChanged(() => Kohi3Priority);
            }
        }

        /// <summary>
        /// 公費４優先順位
        ///     公費優先順位(都道府県番号+優先順位+法別番号)
        /// </summary>
        public string Kohi4Priority
        {
            get { return KaikeiDetail.Kohi4Priority; }
            set
            {
                if (KaikeiDetail.Kohi4Priority == value) return;
                KaikeiDetail.Kohi4Priority = value;
                //RaisePropertyChanged(() => Kohi4Priority);
            }
        }

        /// <summary>
        /// 本人家族区分
        /// PT_HOKEN_INF.HONKE_KBN
        /// </summary>
        public int HonkeKbn
        {
            get { return KaikeiDetail.HonkeKbn; }
            set
            {
                if (KaikeiDetail.HonkeKbn == value) return;
                KaikeiDetail.HonkeKbn = value;
                //RaisePropertyChanged(() => HonkeKbn);
            }
        }

        /// <summary>
        /// 高額療養費区分
        /// PT_HOKEN_INF.KOGAKU_KBN
        /// </summary>
        public int KogakuKbn
        {
            get { return KaikeiDetail.KogakuKbn; }
            set
            {
                if (KaikeiDetail.KogakuKbn == value) return;
                KaikeiDetail.KogakuKbn = value;
                //RaisePropertyChanged(() => KogakuKbn);
            }
        }

        /// <summary>
        /// 高額療養費適用区分
        /// PT_HOKEN_INF.KOGAKU_TEKIYO_KBN
        /// </summary>
        public int KogakuTekiyoKbn
        {
            get { return KaikeiDetail.KogakuTekiyoKbn; }
            set
            {
                if (KaikeiDetail.KogakuTekiyoKbn == value) return;
                KaikeiDetail.KogakuTekiyoKbn = value;
                //RaisePropertyChanged(() => KogakuTekiyoKbn);
            }
        }

        /// <summary>
        /// 限度額特例フラグ
        /// 
        /// </summary>
        public int IsTokurei
        {
            get { return KaikeiDetail.IsTokurei; }
            set
            {
                if (KaikeiDetail.IsTokurei == value) return;
                KaikeiDetail.IsTokurei = value;
                //RaisePropertyChanged(() => IsTokurei);
            }
        }

        /// <summary>
        /// 多数回該当フラグ
        /// 
        /// </summary>
        public int IsTasukai
        {
            get { return KaikeiDetail.IsTasukai; }
            set
            {
                if (KaikeiDetail.IsTasukai == value) return;
                KaikeiDetail.IsTasukai = value;
                //RaisePropertyChanged(() => IsTasukai);
            }
        }

        /// <summary>
        /// 高額療養費合算対象
        ///     1:合算対象外
        ///     2:21,000未満合算対象
        /// </summary>
        public int KogakuTotalKbn
        {
            get { return KaikeiDetail.KogakuTotalKbn; }
            set
            {
                if (KaikeiDetail.KogakuTotalKbn == value) return;
                KaikeiDetail.KogakuTotalKbn = value;
                //RaisePropertyChanged(() => KogakuTotalKbn);
            }
        }

        /// <summary>
        /// マル長適用フラグ
        /// 1:適用
        /// </summary>
        public int IsChoki
        {
            get { return KaikeiDetail.IsChoki; }
            set
            {
                if (KaikeiDetail.IsChoki == value) return;
                KaikeiDetail.IsChoki = value;
                //RaisePropertyChanged(() => IsChoki);
            }
        }

        /// <summary>
        /// 高額療養費限度額
        /// 
        /// </summary>
        public int KogakuLimit
        {
            get { return KaikeiDetail.KogakuLimit; }
            set
            {
                if (KaikeiDetail.KogakuLimit == value) return;
                KaikeiDetail.KogakuLimit = value;
                //RaisePropertyChanged(() => KogakuLimit);
            }
        }

        /// <summary>
        /// 高額療養費限度額(合算)
        /// 
        /// </summary>
        public int TotalKogakuLimit
        {
            get { return KaikeiDetail.TotalKogakuLimit; }
            set
            {
                if (KaikeiDetail.TotalKogakuLimit == value) return;
                KaikeiDetail.TotalKogakuLimit = value;
                //RaisePropertyChanged(() => TotalKogakuLimit);
            }
        }

        /// <summary>
        /// 国保減免区分
        /// PT_HOKEN_INF.GENMEN_KBN
        /// </summary>
        public int GenmenKbn
        {
            get { return KaikeiDetail.GenmenKbn; }
            set
            {
                if (KaikeiDetail.GenmenKbn == value) return;
                KaikeiDetail.GenmenKbn = value;
                //RaisePropertyChanged(() => GenmenKbn);
            }
        }

        /// <summary>
        /// 点数単価
        /// PT_HOKEN_INF.EN_TEN
        /// </summary>
        public int EnTen
        {
            get { return KaikeiDetail.EnTen; }
            set
            {
                if (KaikeiDetail.EnTen == value) return;
                KaikeiDetail.EnTen = value;
                //RaisePropertyChanged(() => EnTen);
            }
        }

        /// <summary>
        /// 保険負担率
        /// 
        /// </summary>
        public int HokenRate
        {
            get { return KaikeiDetail.HokenRate; }
            set
            {
                if (KaikeiDetail.HokenRate == value) return;
                KaikeiDetail.HokenRate = value;
                //RaisePropertyChanged(() => HokenRate);
            }
        }

        /// <summary>
        /// 患者負担率
        /// 
        /// </summary>
        public int PtRate
        {
            get { return KaikeiDetail.PtRate; }
            set
            {
                if (KaikeiDetail.PtRate == value) return;
                KaikeiDetail.PtRate = value;
                //RaisePropertyChanged(() => PtRate);
            }
        }

        /// <summary>
        /// 公１負担限度額
        /// 
        /// </summary>
        public int Kohi1Limit
        {
            get { return KaikeiDetail.Kohi1Limit; }
            set
            {
                if (KaikeiDetail.Kohi1Limit == value) return;
                KaikeiDetail.Kohi1Limit = value;
                //RaisePropertyChanged(() => Kohi1Limit);
            }
        }

        /// <summary>
        /// 公１他院負担額
        ///     当該来院までの積み上げ
        /// </summary>
        public int Kohi1OtherFutan
        {
            get { return KaikeiDetail.Kohi1OtherFutan; }
            set
            {
                if (KaikeiDetail.Kohi1OtherFutan == value) return;
                KaikeiDetail.Kohi1OtherFutan = value;
                //RaisePropertyChanged(() => Kohi1OtherFutan);
            }
        }

        /// <summary>
        /// 公２負担限度額
        /// 
        /// </summary>
        public int Kohi2Limit
        {
            get { return KaikeiDetail.Kohi2Limit; }
            set
            {
                if (KaikeiDetail.Kohi2Limit == value) return;
                KaikeiDetail.Kohi2Limit = value;
                //RaisePropertyChanged(() => Kohi2Limit);
            }
        }

        /// <summary>
        /// 公２他院負担額
        ///     当該来院までの積み上げ
        /// </summary>
        public int Kohi2OtherFutan
        {
            get { return KaikeiDetail.Kohi2OtherFutan; }
            set
            {
                if (KaikeiDetail.Kohi2OtherFutan == value) return;
                KaikeiDetail.Kohi2OtherFutan = value;
                //RaisePropertyChanged(() => Kohi2OtherFutan);
            }
        }

        /// <summary>
        /// 公３負担限度額
        /// 
        /// </summary>
        public int Kohi3Limit
        {
            get { return KaikeiDetail.Kohi3Limit; }
            set
            {
                if (KaikeiDetail.Kohi3Limit == value) return;
                KaikeiDetail.Kohi3Limit = value;
                //RaisePropertyChanged(() => Kohi3Limit);
            }
        }

        /// <summary>
        /// 公３他院負担額
        ///     当該来院までの積み上げ
        /// </summary>
        public int Kohi3OtherFutan
        {
            get { return KaikeiDetail.Kohi3OtherFutan; }
            set
            {
                if (KaikeiDetail.Kohi3OtherFutan == value) return;
                KaikeiDetail.Kohi3OtherFutan = value;
                //RaisePropertyChanged(() => Kohi3OtherFutan);
            }
        }

        /// <summary>
        /// 公４負担限度額
        /// 
        /// </summary>
        public int Kohi4Limit
        {
            get { return KaikeiDetail.Kohi4Limit; }
            set
            {
                if (KaikeiDetail.Kohi4Limit == value) return;
                KaikeiDetail.Kohi4Limit = value;
                //RaisePropertyChanged(() => Kohi4Limit);
            }
        }

        /// <summary>
        /// 公４他院負担額
        ///     当該来院までの積み上げ
        /// </summary>
        public int Kohi4OtherFutan
        {
            get { return KaikeiDetail.Kohi4OtherFutan; }
            set
            {
                if (KaikeiDetail.Kohi4OtherFutan == value) return;
                KaikeiDetail.Kohi4OtherFutan = value;
                //RaisePropertyChanged(() => Kohi4OtherFutan);
            }
        }

        /// <summary>
        /// 診療点数
        /// 
        /// </summary>
        public int Tensu
        {
            get { return KaikeiDetail.Tensu; }
            set
            {
                if (KaikeiDetail.Tensu == value) return;
                KaikeiDetail.Tensu = value;
                //RaisePropertyChanged(() => Tensu);
            }
        }

        /// <summary>
        /// 総医療費
        /// 
        /// </summary>
        public int TotalIryohi
        {
            get { return KaikeiDetail.TotalIryohi; }
            set
            {
                if (KaikeiDetail.TotalIryohi == value) return;
                KaikeiDetail.TotalIryohi = value;
                //RaisePropertyChanged(() => TotalIryohi);
            }
        }

        /// <summary>
        /// 保険負担額
        /// 
        /// </summary>
        public int HokenFutan
        {
            get { return KaikeiDetail.HokenFutan; }
            set
            {
                if (KaikeiDetail.HokenFutan == value) return;
                KaikeiDetail.HokenFutan = value;
                //RaisePropertyChanged(() => HokenFutan);
            }
        }

        /// <summary>
        /// 高額負担額
        /// 
        /// </summary>
        public int KogakuFutan
        {
            get { return KaikeiDetail.KogakuFutan; }
            set
            {
                if (KaikeiDetail.KogakuFutan == value) return;
                KaikeiDetail.KogakuFutan = value;
                //RaisePropertyChanged(() => KogakuFutan);
            }
        }

        /// <summary>
        /// 公１負担額
        /// 
        /// </summary>
        public int Kohi1Futan
        {
            get { return KaikeiDetail.Kohi1Futan; }
            set
            {
                if (KaikeiDetail.Kohi1Futan == value) return;
                KaikeiDetail.Kohi1Futan = value;
                //RaisePropertyChanged(() => Kohi1Futan);
            }
        }

        /// <summary>
        /// 公２負担額
        /// 
        /// </summary>
        public int Kohi2Futan
        {
            get { return KaikeiDetail.Kohi2Futan; }
            set
            {
                if (KaikeiDetail.Kohi2Futan == value) return;
                KaikeiDetail.Kohi2Futan = value;
                //RaisePropertyChanged(() => Kohi2Futan);
            }
        }

        /// <summary>
        /// 公３負担額
        /// 
        /// </summary>
        public int Kohi3Futan
        {
            get { return KaikeiDetail.Kohi3Futan; }
            set
            {
                if (KaikeiDetail.Kohi3Futan == value) return;
                KaikeiDetail.Kohi3Futan = value;
                //RaisePropertyChanged(() => Kohi3Futan);
            }
        }

        /// <summary>
        /// 公４負担額
        /// 
        /// </summary>
        public int Kohi4Futan
        {
            get { return KaikeiDetail.Kohi4Futan; }
            set
            {
                if (KaikeiDetail.Kohi4Futan == value) return;
                KaikeiDetail.Kohi4Futan = value;
                //RaisePropertyChanged(() => Kohi4Futan);
            }
        }

        /// <summary>
        /// 一部負担額
        /// 
        /// </summary>
        public int IchibuFutan
        {
            get { return KaikeiDetail.IchibuFutan; }
            set
            {
                if (KaikeiDetail.IchibuFutan == value) return;
                KaikeiDetail.IchibuFutan = value;
                //RaisePropertyChanged(() => IchibuFutan);
            }
        }

        /// <summary>
        /// 減免額
        /// 
        /// </summary>
        public int GenmenGaku
        {
            get { return KaikeiDetail.GenmenGaku; }
            set
            {
                if (KaikeiDetail.GenmenGaku == value) return;
                KaikeiDetail.GenmenGaku = value;
                //RaisePropertyChanged(() => GenmenGaku);
            }
        }

        /// <summary>
        /// 保険負担額10円単位
        /// 
        /// </summary>
        public int HokenFutan10en
        {
            get { return KaikeiDetail.HokenFutan10en; }
            set
            {
                if (KaikeiDetail.HokenFutan10en == value) return;
                KaikeiDetail.HokenFutan10en = value;
                //RaisePropertyChanged(() => HokenFutan10en);
            }
        }

        /// <summary>
        /// 高額負担額10円単位
        /// 
        /// </summary>
        public int KogakuFutan10en
        {
            get { return KaikeiDetail.KogakuFutan10en; }
            set
            {
                if (KaikeiDetail.KogakuFutan10en == value) return;
                KaikeiDetail.KogakuFutan10en = value;
                //RaisePropertyChanged(() => KogakuFutan10en);
            }
        }

        /// <summary>
        /// 公１負担額10円単位
        /// 
        /// </summary>
        public int Kohi1Futan10en
        {
            get { return KaikeiDetail.Kohi1Futan10en; }
            set
            {
                if (KaikeiDetail.Kohi1Futan10en == value) return;
                KaikeiDetail.Kohi1Futan10en = value;
                //RaisePropertyChanged(() => Kohi1Futan10en);
            }
        }

        /// <summary>
        /// 公２負担額10円単位
        /// 
        /// </summary>
        public int Kohi2Futan10en
        {
            get { return KaikeiDetail.Kohi2Futan10en; }
            set
            {
                if (KaikeiDetail.Kohi2Futan10en == value) return;
                KaikeiDetail.Kohi2Futan10en = value;
                //RaisePropertyChanged(() => Kohi2Futan10en);
            }
        }

        /// <summary>
        /// 公３負担額10円単位
        /// 
        /// </summary>
        public int Kohi3Futan10en
        {
            get { return KaikeiDetail.Kohi3Futan10en; }
            set
            {
                if (KaikeiDetail.Kohi3Futan10en == value) return;
                KaikeiDetail.Kohi3Futan10en = value;
                //RaisePropertyChanged(() => Kohi3Futan10en);
            }
        }

        /// <summary>
        /// 公４負担額10円単位
        /// 
        /// </summary>
        public int Kohi4Futan10en
        {
            get { return KaikeiDetail.Kohi4Futan10en; }
            set
            {
                if (KaikeiDetail.Kohi4Futan10en == value) return;
                KaikeiDetail.Kohi4Futan10en = value;
                //RaisePropertyChanged(() => Kohi4Futan10en);
            }
        }

        /// <summary>
        /// 一部負担額10円単位
        /// 
        /// </summary>
        public int IchibuFutan10en
        {
            get { return KaikeiDetail.IchibuFutan10en; }
            set
            {
                if (KaikeiDetail.IchibuFutan10en == value) return;
                KaikeiDetail.IchibuFutan10en = value;
                //RaisePropertyChanged(() => IchibuFutan10en);
            }
        }

        /// <summary>
        /// 減免額10円単位
        /// 
        /// </summary>
        public int GenmenGaku10en
        {
            get { return KaikeiDetail.GenmenGaku10en; }
            set
            {
                if (KaikeiDetail.GenmenGaku10en == value) return;
                KaikeiDetail.GenmenGaku10en = value;
                //RaisePropertyChanged(() => GenmenGaku10en);
            }
        }

        /// <summary>
        /// 同一来院調整額
        /// 同一来院のまるめ調整額
        /// </summary>
        public int AdjustRound
        {
            get { return KaikeiDetail.AdjustRound; }
            set
            {
                if (KaikeiDetail.AdjustRound == value) return;
                KaikeiDetail.AdjustRound = value;
                //RaisePropertyChanged(() => AdjustRound);
            }
        }

        /// <summary>
        /// 患者負担額
        /// 
        /// </summary>
        public int PtFutan
        {
            get { return KaikeiDetail.PtFutan; }
            set
            {
                if (KaikeiDetail.PtFutan == value) return;
                KaikeiDetail.PtFutan = value;
                //RaisePropertyChanged(() => PtFutan);
            }
        }

        /// <summary>
        /// 高額療養費超過区分
        /// 
        /// </summary>
        public int KogakuOverKbn
        {
            get { return KaikeiDetail.KogakuOverKbn; }
            set
            {
                if (KaikeiDetail.KogakuOverKbn == value) return;
                KaikeiDetail.KogakuOverKbn = value;
                //RaisePropertyChanged(() => KogakuOverKbn);
            }
        }

        /// <summary>
        /// レセプト種別
        /// 11x2: 本人
        ///                     11x4: 未就学者          
        ///                     11x6: 家族          
        ///                     11x8: 高齢一般・低所          
        ///                     11x0: 高齢７割          
        ///                     12x2: 公費          
        ///                     13x8: 後期一般・低所          
        ///                     13x0: 後期７割          
        ///                     14x2: 退職本人          
        ///                     14x4: 退職未就学者          
        ///                     14x6: 退職家族          
        /// </summary>
        public string ReceSbt
        {
            get { return KaikeiDetail.ReceSbt; }
            set
            {
                if (KaikeiDetail.ReceSbt == value) return;
                KaikeiDetail.ReceSbt = value;
                //RaisePropertyChanged(() => ReceSbt);
            }
        }

        /// <summary>
        /// 実日数
        /// 
        /// </summary>
        public int Jitunisu
        {
            get { return KaikeiDetail.Jitunisu; }
            set
            {
                if (KaikeiDetail.Jitunisu == value) return;
                KaikeiDetail.Jitunisu = value;
                //RaisePropertyChanged(() => Jitunisu);
            }
        }

        /// <summary>
        /// 労災イ点負担額
        /// 
        /// </summary>
        public int RousaiIFutan
        {
            get { return KaikeiDetail.RousaiIFutan; }
            set
            {
                if (KaikeiDetail.RousaiIFutan == value) return;
                KaikeiDetail.RousaiIFutan = value;
                //RaisePropertyChanged(() => RousaiIFutan);
            }
        }

        /// <summary>
        /// 労災ロ円負担額
        /// 
        /// </summary>
        public int RousaiRoFutan
        {
            get { return KaikeiDetail.RousaiRoFutan; }
            set
            {
                if (KaikeiDetail.RousaiRoFutan == value) return;
                KaikeiDetail.RousaiRoFutan = value;
                //RaisePropertyChanged(() => RousaiRoFutan);
            }
        }

        /// <summary>
        /// 自賠イ技術点数
        /// 
        /// </summary>
        public int JibaiITensu
        {
            get { return KaikeiDetail.JibaiITensu; }
            set
            {
                if (KaikeiDetail.JibaiITensu == value) return;
                KaikeiDetail.JibaiITensu = value;
                //RaisePropertyChanged(() => JibaiITensu);
            }
        }

        /// <summary>
        /// 自賠ロ薬剤点数
        /// 
        /// </summary>
        public int JibaiRoTensu
        {
            get { return KaikeiDetail.JibaiRoTensu; }
            set
            {
                if (KaikeiDetail.JibaiRoTensu == value) return;
                KaikeiDetail.JibaiRoTensu = value;
                //RaisePropertyChanged(() => JibaiRoTensu);
            }
        }

        /// <summary>
        /// 自賠ハ円診察負担額
        /// 
        /// </summary>
        public int JibaiHaFutan
        {
            get { return KaikeiDetail.JibaiHaFutan; }
            set
            {
                if (KaikeiDetail.JibaiHaFutan == value) return;
                KaikeiDetail.JibaiHaFutan = value;
                //RaisePropertyChanged(() => JibaiHaFutan);
            }
        }

        /// <summary>
        /// 自賠ニ円他負担額
        /// 
        /// </summary>
        public int JibaiNiFutan
        {
            get { return KaikeiDetail.JibaiNiFutan; }
            set
            {
                if (KaikeiDetail.JibaiNiFutan == value) return;
                KaikeiDetail.JibaiNiFutan = value;
                //RaisePropertyChanged(() => JibaiNiFutan);
            }
        }

        /// <summary>
        /// 自賠ホ診断書料
        /// 
        /// </summary>
        public int JibaiHoSindan
        {
            get { return KaikeiDetail.JibaiHoSindan; }
            set
            {
                if (KaikeiDetail.JibaiHoSindan == value) return;
                KaikeiDetail.JibaiHoSindan = value;
                //RaisePropertyChanged(() => JibaiHoSindan);
            }
        }

        /// <summary>
        /// 自賠ホ診断書料枚数
        /// 
        /// </summary>
        public int JibaiHoSindanCount
        {
            get { return KaikeiDetail.JibaiHoSindanCount; }
            set
            {
                if (KaikeiDetail.JibaiHoSindanCount == value) return;
                KaikeiDetail.JibaiHoSindanCount = value;
                //RaisePropertyChanged(() => JibaiHoSindanCount);
            }
        }

        /// <summary>
        /// 自賠ヘ明細書料
        /// 
        /// </summary>
        public int JibaiHeMeisai
        {
            get { return KaikeiDetail.JibaiHeMeisai; }
            set
            {
                if (KaikeiDetail.JibaiHeMeisai == value) return;
                KaikeiDetail.JibaiHeMeisai = value;
                //RaisePropertyChanged(() => JibaiHeMeisai);
            }
        }

        /// <summary>
        /// 自賠ヘ明細書料枚数
        /// 
        /// </summary>
        public int JibaiHeMeisaiCount
        {
            get { return KaikeiDetail.JibaiHeMeisaiCount; }
            set
            {
                if (KaikeiDetail.JibaiHeMeisaiCount == value) return;
                KaikeiDetail.JibaiHeMeisaiCount = value;
                //RaisePropertyChanged(() => JibaiHeMeisaiCount);
            }
        }

        /// <summary>
        /// 自賠Ａ負担額
        /// 
        /// </summary>
        public int JibaiAFutan
        {
            get { return KaikeiDetail.JibaiAFutan; }
            set
            {
                if (KaikeiDetail.JibaiAFutan == value) return;
                KaikeiDetail.JibaiAFutan = value;
                //RaisePropertyChanged(() => JibaiAFutan);
            }
        }

        /// <summary>
        /// 自賠Ｂ負担額
        /// 
        /// </summary>
        public int JibaiBFutan
        {
            get { return KaikeiDetail.JibaiBFutan; }
            set
            {
                if (KaikeiDetail.JibaiBFutan == value) return;
                KaikeiDetail.JibaiBFutan = value;
                //RaisePropertyChanged(() => JibaiBFutan);
            }
        }

        /// <summary>
        /// 自賠Ｃ負担額
        /// 
        /// </summary>
        public int JibaiCFutan
        {
            get { return KaikeiDetail.JibaiCFutan; }
            set
            {
                if (KaikeiDetail.JibaiCFutan == value) return;
                KaikeiDetail.JibaiCFutan = value;
                //RaisePropertyChanged(() => JibaiCFutan);
            }
        }

        /// <summary>
        /// 自賠Ｄ負担額
        /// 
        /// </summary>
        public int JibaiDFutan
        {
            get { return KaikeiDetail.JibaiDFutan; }
            set
            {
                if (KaikeiDetail.JibaiDFutan == value) return;
                KaikeiDetail.JibaiDFutan = value;
                //RaisePropertyChanged(() => JibaiDFutan);
            }
        }

        /// <summary>
        /// 自賠健保点数
        /// 
        /// </summary>
        public int JibaiKenpoTensu
        {
            get { return KaikeiDetail.JibaiKenpoTensu; }
            set
            {
                if (KaikeiDetail.JibaiKenpoTensu == value) return;
                KaikeiDetail.JibaiKenpoTensu = value;
                //RaisePropertyChanged(() => JibaiKenpoTensu);
            }
        }

        /// <summary>
        /// 自賠健保負担額
        /// 
        /// </summary>
        public int JibaiKenpoFutan
        {
            get { return KaikeiDetail.JibaiKenpoFutan; }
            set
            {
                if (KaikeiDetail.JibaiKenpoFutan == value) return;
                KaikeiDetail.JibaiKenpoFutan = value;
                //RaisePropertyChanged(() => JibaiKenpoFutan);
            }
        }

        /// <summary>
        /// 自費負担額
        /// 
        /// </summary>
        public int JihiFutan
        {
            get { return KaikeiDetail.JihiFutan; }
            set
            {
                if (KaikeiDetail.JihiFutan == value) return;
                KaikeiDetail.JihiFutan = value;
                //RaisePropertyChanged(() => JihiFutan);
            }
        }

        /// <summary>
        /// 自費内税
        /// 
        /// </summary>
        public int JihiTax
        {
            get { return KaikeiDetail.JihiTax; }
            set
            {
                if (KaikeiDetail.JihiTax == value) return;
                KaikeiDetail.JihiTax = value;
                //RaisePropertyChanged(() => JihiTax);
            }
        }

        /// <summary>
        /// 自費外税
        /// 
        /// </summary>
        public int JihiOuttax
        {
            get { return KaikeiDetail.JihiOuttax; }
            set
            {
                if (KaikeiDetail.JihiOuttax == value) return;
                KaikeiDetail.JihiOuttax = value;
                //RaisePropertyChanged(() => JihiOuttax);
            }
        }

        /// <summary>
        /// 自費負担額(非課税)
        /// 
        /// </summary>
        public int JihiFutanTaxfree
        {
            get { return KaikeiDetail.JihiFutanTaxfree; }
            set
            {
                if (KaikeiDetail.JihiFutanTaxfree == value) return;
                KaikeiDetail.JihiFutanTaxfree = value;
                //RaisePropertyChanged(() => JihiFutanTaxfree);
            }
        }

        /// <summary>
        /// 自費負担額(内税・通常税率)
        /// 
        /// </summary>
        public int JihiFutanTaxNr
        {
            get { return KaikeiDetail.JihiFutanTaxNr; }
            set
            {
                if (KaikeiDetail.JihiFutanTaxNr == value) return;
                KaikeiDetail.JihiFutanTaxNr = value;
                //RaisePropertyChanged(() => JihiFutanTaxNr);
            }
        }

        /// <summary>
        /// 自費負担額(内税・軽減税率)
        /// 
        /// </summary>
        public int JihiFutanTaxGen
        {
            get { return KaikeiDetail.JihiFutanTaxGen; }
            set
            {
                if (KaikeiDetail.JihiFutanTaxGen == value) return;
                KaikeiDetail.JihiFutanTaxGen = value;
                //RaisePropertyChanged(() => JihiFutanTaxGen);
            }
        }

        /// <summary>
        /// 自費負担額(外税・通常税率)
        /// 
        /// </summary>
        public int JihiFutanOuttaxNr
        {
            get { return KaikeiDetail.JihiFutanOuttaxNr; }
            set
            {
                if (KaikeiDetail.JihiFutanOuttaxNr == value) return;
                KaikeiDetail.JihiFutanOuttaxNr = value;
                //RaisePropertyChanged(() => JihiFutanOuttaxNr);
            }
        }

        /// <summary>
        /// 自費負担額(内税・軽減税率)
        /// 
        /// </summary>
        public int JihiFutanOuttaxGen
        {
            get { return KaikeiDetail.JihiFutanOuttaxGen; }
            set
            {
                if (KaikeiDetail.JihiFutanOuttaxGen == value) return;
                KaikeiDetail.JihiFutanOuttaxGen = value;
                //RaisePropertyChanged(() => JihiFutanOuttaxGen);
            }
        }

        /// <summary>
        /// 自費内税(通常税率)
        /// 
        /// </summary>
        public int JihiTaxNr
        {
            get { return KaikeiDetail.JihiTaxNr; }
            set
            {
                if (KaikeiDetail.JihiTaxNr == value) return;
                KaikeiDetail.JihiTaxNr = value;
                //RaisePropertyChanged(() => JihiTaxNr);
            }
        }

        /// <summary>
        /// 自費内税(軽減税率)
        /// 
        /// </summary>
        public int JihiTaxGen
        {
            get { return KaikeiDetail.JihiTaxGen; }
            set
            {
                if (KaikeiDetail.JihiTaxGen == value) return;
                KaikeiDetail.JihiTaxGen = value;
                //RaisePropertyChanged(() => JihiTaxGen);
            }
        }

        /// <summary>
        /// 自費外税(通常税率)
        /// 
        /// </summary>
        public int JihiOuttaxNr
        {
            get { return KaikeiDetail.JihiOuttaxNr; }
            set
            {
                if (KaikeiDetail.JihiOuttaxNr == value) return;
                KaikeiDetail.JihiOuttaxNr = value;
                //RaisePropertyChanged(() => JihiOuttaxNr);
            }
        }

        /// <summary>
        /// 自費外税(軽減税率)
        /// 
        /// </summary>
        public int JihiOuttaxGen
        {
            get { return KaikeiDetail.JihiOuttaxGen; }
            set
            {
                if (KaikeiDetail.JihiOuttaxGen == value) return;
                KaikeiDetail.JihiOuttaxGen = value;
                //RaisePropertyChanged(() => JihiOuttaxGen);
            }
        }

        /// <summary>
        /// 患者負担合計額
        /// 
        /// </summary>
        public int TotalPtFutan
        {
            get { return KaikeiDetail.TotalPtFutan; }
            set
            {
                if (KaikeiDetail.TotalPtFutan == value) return;
                KaikeiDetail.TotalPtFutan = value;
                //RaisePropertyChanged(() => TotalPtFutan);
            }
        }

        /// <summary>
        /// 計算順番
        ///     診察日 + 診察開始時間 + 来院番号 + 公費優先順位(都道府県番号+優先順位+法別番号) + 保険PID + 0
        /// </summary>
        public string SortKey
        {
            get { return KaikeiDetail.SortKey; }
            set
            {
                if (KaikeiDetail.SortKey == value) return;
                KaikeiDetail.SortKey = value;
                //RaisePropertyChanged(() => SortKey);
            }
        }

        /// <summary>
        /// 妊婦フラグ
        /// 1:対象
        /// </summary>
        public int IsNinpu
        {
            get { return KaikeiDetail.IsNinpu; }
            set
            {
                if (KaikeiDetail.IsNinpu == value) return;
                KaikeiDetail.IsNinpu = value;
                //RaisePropertyChanged(() => IsNinpu);
            }
        }

        /// <summary>
        /// 在医総フラグ
        /// 1:1:在医総管又は在医総
        /// </summary>
        public int IsZaiiso
        {
            get { return KaikeiDetail.IsZaiiso; }
            set
            {
                if (KaikeiDetail.IsZaiiso == value) return;
                KaikeiDetail.IsZaiiso = value;
                //RaisePropertyChanged(() => IsZaiiso);
            }
        }

        /// <summary>
        /// 作成日時
        /// 
        /// </summary>
        public DateTime CreateDate
        {
            get { return KaikeiDetail.CreateDate; }
            set
            {
                if (KaikeiDetail.CreateDate == value) return;
                KaikeiDetail.CreateDate = value;
                //RaisePropertyChanged(() => CreateDate);
            }
        }

        /// <summary>
        /// 作成者
        /// 
        /// </summary>
        public int CreateId
        {
            get { return KaikeiDetail.CreateId; }
            set
            {
                if (KaikeiDetail.CreateId == value) return;
                KaikeiDetail.CreateId = value;
                //RaisePropertyChanged(() => CreateId);
            }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return KaikeiDetail.CreateMachine; }
            set
            {
                if (KaikeiDetail.CreateMachine == value) return;
                KaikeiDetail.CreateMachine = value;
                //RaisePropertyChanged(() => CreateMachine);
            }
        }


    }

}
