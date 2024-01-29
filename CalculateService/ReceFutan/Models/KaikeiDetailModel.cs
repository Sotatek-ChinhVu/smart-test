﻿using Entity.Tenant;

namespace CalculateService.ReceFutan.Models
{
    public class KaikeiDetailModel
    {
        public KaikeiDetail KaikeiDetail { get; }

        /// <summary>
        /// 請求区分
        /// </summary>
        public int SeikyuKbn { get; } = 0;

        /// <summary>
        /// 診療科ID
        /// </summary>
        public int KaId { get; } = 0;

        /// <summary>
        /// 担当医ID
        /// </summary>
        public int TantoId { get; } = 0;

        public KaikeiDetailModel(KaikeiDetail kaikeiDetail, int seikyuKbn, int kaId, int tantoId)
        {
            KaikeiDetail = kaikeiDetail;
            SeikyuKbn = seikyuKbn;
            KaId = kaId;
            TantoId = tantoId;
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
            }
        }

        /// <summary>
        /// 点数単価
        /// 
        /// </summary>
        public int EnTen
        {
            get { return KaikeiDetail.EnTen; }
            set
            {
                if (KaikeiDetail.EnTen == value) return;
                KaikeiDetail.EnTen = value;
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
            }
        }

        /// <summary>
        /// 一部負担額10円単位
        /// 
        /// </summary>
        public int IchibuFutan10en
        {
            get { return KaikeiDetail.IchibuFutan10en + KaikeiDetail.AdjustRound; }
            set
            {
                if (KaikeiDetail.IchibuFutan10en == value) return;
                KaikeiDetail.IchibuFutan10en = value;
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
            }
        }

        /// <summary>
        /// 患者負担額
        /// 
        /// </summary>
        public int PtFutan
        {
            get { return KaikeiDetail.PtFutan + KaikeiDetail.AdjustRound; }
            set
            {
                if (KaikeiDetail.PtFutan == value) return;
                KaikeiDetail.PtFutan = value;
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
            }
        }

        /// <summary>
        /// 在医総フラグ
        /// 1:在医総及び在医総管
        /// </summary>
        public int IsZaiiso
        {
            get { return KaikeiDetail.IsZaiiso; }
            set
            {
                if (KaikeiDetail.IsZaiiso == value) return;
                KaikeiDetail.IsZaiiso = value;
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
            }
        }

        /// <summary>
        /// 作成端末
        /// 
        /// </summary>
        public string CreateMachine
        {
            get { return KaikeiDetail.CreateMachine ?? string.Empty; }
            set
            {
                if (KaikeiDetail.CreateMachine == value) return;
                KaikeiDetail.CreateMachine = value;
            }
        }

        /// <summary>
        /// 主保険の有無
        /// </summary>
        public bool IsNoHoken
        {
            get
            {
                if (HokenKbn == 1 && ReceSbt?.Substring(1, 1) == "2")
                {
                    return true;
                }
                return false;
            }
        }

    }
}