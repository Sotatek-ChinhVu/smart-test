using Entity.Tenant;

namespace EmrCalculateApi.ReceFutan.Models
{
    public class SumKaikeiPidModel
    {
        public SumKaikeiPidModel(
            int hpId, long ptId, int sinYm, int hokenPid, int adjustKid, int hokenKbn, int hokenSbtCd, int hokenId,
            int kohi1Id, int kohi2Id, int kohi3Id, int kohi4Id, string houbetu, string kohi1Houbetu,
            string kohi2Houbetu, string kohi3Houbetu, string kohi4Houbetu, string kohi1Priority,
            string kohi2Priority, string kohi3Priority, string kohi4Priority, int honkeKbn, int kogakuKbn,
            int kogakuTekiyoKbn, int isTokurei, int isTasukai, /*int isNotKogakuTotal,*/ int isChoki, int kogakuLimit, int totalKogakuLimit,
            int genmenKbn, int hokenRate, int ptRate, int enTen, int kohi1Limit, int kohi1OtherFutan, int kohi2Limit,
            int kohi2OtherFutan, int kohi3Limit, int kohi3OtherFutan, int kohi4Limit, int kohi4OtherFutan,
            int tensu, int totalIryohi, int hokenFutan, int kogakuFutan, int kohi1Futan,
            int kohi2Futan, int kohi3Futan, int kohi4Futan, int ichibuFutan, int genmenGaku, int hokenFutan10en,
            int kogakuFutan10en, int kohi1Futan10en, int kohi2Futan10en, int kohi3Futan10en, int kohi4Futan10en,
            int ichibuFutan10en, int genmenGaku10en, int ptFutan, int kogakuOverKbn, string receSbt,
            int rousaiIFutan, int rousaiRoFutan, int jibaiITensu, int jibaiRoTensu,
            int jibaiHaFutan, int jibaiNiFutan, int jibaiHoSindan, int jibaiHoSindanCount,
            int jibaiHeMeisai, int jibaiHeMeisaiCount, int jibaiAFutan,
            int jibaiBFutan, int jibaiCFutan, int jibaiDFutan, int jibaiKenpoTensu, int jibaiKenpoFutan,
            int isNinpu, int isZaiiso, int seikyuKbn
        )
        {
            HpId = hpId;
            PtId = ptId;
            SinYm = sinYm;
            HokenPid = hokenPid;
            AdjustKid = adjustKid;
            HokenKbn = hokenKbn;
            HokenSbtCd = hokenSbtCd;
            HokenId = hokenId;
            Kohi1Id = kohi1Id;
            Kohi2Id = kohi2Id;
            Kohi3Id = kohi3Id;
            Kohi4Id = kohi4Id;
            Houbetu = houbetu;
            Kohi1Houbetu = kohi1Houbetu;
            Kohi2Houbetu = kohi2Houbetu;
            Kohi3Houbetu = kohi3Houbetu;
            Kohi4Houbetu = kohi4Houbetu;
            Kohi1Priority = kohi1Priority;
            Kohi2Priority = kohi2Priority;
            Kohi3Priority = kohi3Priority;
            Kohi4Priority = kohi4Priority;
            HonkeKbn = honkeKbn;
            KogakuKbn = kogakuKbn;
            KogakuTekiyoKbn = kogakuTekiyoKbn;
            IsTokurei = isTokurei;
            IsTasukai = isTasukai;
            //IsNotKogakuTotal = isNotKogakuTotal;
            IsChoki = isChoki;
            KogakuLimit = kogakuLimit;
            TotalKogakuLimit = totalKogakuLimit;
            GenmenKbn = genmenKbn;
            HokenRate = hokenRate;
            PtRate = ptRate;
            EnTen = enTen;
            Kohi1Limit = kohi1Limit;
            Kohi1OtherFutan = kohi1OtherFutan;
            Kohi2Limit = kohi2Limit;
            Kohi2OtherFutan = kohi2OtherFutan;
            Kohi3Limit = kohi3Limit;
            Kohi3OtherFutan = kohi3OtherFutan;
            Kohi4Limit = kohi4Limit;
            Kohi4OtherFutan = kohi4OtherFutan;
            Tensu = tensu;
            TotalIryohi = totalIryohi;
            HokenFutan = hokenFutan;
            KogakuFutan = kogakuFutan;
            Kohi1Futan = kohi1Futan;
            Kohi2Futan = kohi2Futan;
            Kohi3Futan = kohi3Futan;
            Kohi4Futan = kohi4Futan;
            IchibuFutan = ichibuFutan;
            GenmenGaku = genmenGaku;
            HokenFutan10en = hokenFutan10en;
            KogakuFutan10en = kogakuFutan10en;
            Kohi1Futan10en = kohi1Futan10en;
            Kohi2Futan10en = kohi2Futan10en;
            Kohi3Futan10en = kohi3Futan10en;
            Kohi4Futan10en = kohi4Futan10en;
            IchibuFutan10en = ichibuFutan10en;
            GenmenGaku10en = genmenGaku10en;
            PtFutan = ptFutan;
            KogakuOverKbn = kogakuOverKbn;
            ReceSbt = receSbt;
            RousaiIFutan = rousaiIFutan;
            RousaiRoFutan = rousaiRoFutan;
            JibaiITensu = jibaiITensu;
            JibaiRoTensu = jibaiRoTensu;
            JibaiHaFutan = jibaiHaFutan;
            JibaiNiFutan = jibaiNiFutan;
            JibaiHoSindan = jibaiHoSindan;
            JibaiHoSindanCount = jibaiHoSindanCount;
            JibaiHeMeisai = jibaiHeMeisai;
            JibaiHeMeisaiCount = jibaiHeMeisaiCount;
            JibaiAFutan = jibaiAFutan;
            JibaiBFutan = jibaiBFutan;
            JibaiCFutan = jibaiCFutan;
            JibaiDFutan = jibaiDFutan;
            JibaiKenpoTensu = jibaiKenpoTensu;
            JibaiKenpoFutan = jibaiKenpoFutan;
            IsNinpu = isNinpu;
            IsZaiiso = isZaiiso;
            SeikyuKbn = seikyuKbn;
        }

        /// <summary>
        /// 医療機関識別ID
        /// 
        /// </summary>
        public int HpId;

        /// <summary>
        /// 患者ID
        /// 患者を識別するためのシステム固有の番号
        /// </summary>
        public long PtId;

        /// <summary>
        /// 診療月
        /// 
        /// </summary>
        public int SinYm;

        /// <summary>
        /// 保険組合せID
        /// 
        /// </summary>
        public int HokenPid;

        /// <summary>
        /// 合算調整KohiId
        /// 
        /// </summary>
        public int AdjustKid;

        /// <summary>
        /// 保険区分
        /// 
        /// </summary>
        public int HokenKbn;

        /// <summary>
        /// 保険種別コード
        /// 
        /// </summary>
        public int HokenSbtCd;

        /// <summary>
        /// 主保険保険ID
        /// 
        /// </summary>
        public int HokenId;

        /// <summary>
        /// 公費１保険ID
        /// 
        /// </summary>
        public int Kohi1Id;

        /// <summary>
        /// 公費２保険ID
        /// 
        /// </summary>
        public int Kohi2Id;

        /// <summary>
        /// 公費３保険ID
        /// 
        /// </summary>
        public int Kohi3Id;

        /// <summary>
        /// 公費４保険ID
        /// 
        /// </summary>
        public int Kohi4Id;

        /// <summary>
        /// 法別番号
        /// PT_HOKEN_INF.HOUBETU
        /// </summary>
        public string Houbetu;

        /// <summary>
        /// 公１法別
        /// 
        /// </summary>
        public string Kohi1Houbetu;

        /// <summary>
        /// 公２法別
        /// 
        /// </summary>
        public string Kohi2Houbetu;

        /// <summary>
        /// 公３法別
        /// 
        /// </summary>
        public string Kohi3Houbetu;

        /// <summary>
        /// 公４法別
        /// 
        /// </summary>
        public string Kohi4Houbetu;

        /// <summary>
        /// 公費１優先順位
        ///     公費優先順位(都道府県番号+優先順位+法別番号)
        /// </summary>
        public string Kohi1Priority;

        /// <summary>
        /// 公費２優先順位
        ///     公費優先順位(都道府県番号+優先順位+法別番号)
        /// </summary>
        public string Kohi2Priority;

        /// <summary>
        /// 公費３優先順位
        ///     公費優先順位(都道府県番号+優先順位+法別番号)
        /// </summary>
        public string Kohi3Priority;

        /// <summary>
        /// 公費４優先順位
        ///     公費優先順位(都道府県番号+優先順位+法別番号)
        /// </summary>
        public string Kohi4Priority;

        /// <summary>
        /// 本人家族区分
        /// PT_HOKEN_INF.HONKE_KBN
        /// </summary>
        public int HonkeKbn;

        /// <summary>
        /// 高額療養費区分
        /// PT_HOKEN_INF.KOGAKU_KBN
        /// </summary>
        public int KogakuKbn;

        /// <summary>
        /// 高額療養費適用区分
        /// PT_HOKEN_INF.KOGAKU_TEKIYO_KBN
        /// </summary>
        public int KogakuTekiyoKbn;

        /// <summary>
        /// 限度額特例フラグ
        /// 
        /// </summary>
        public int IsTokurei;

        /// <summary>
        /// 多数回該当フラグ
        /// 
        /// </summary>
        public int IsTasukai;

        /// <summary>
        /// 高額療養費合算対象外
        /// 1:対象外
        /// </summary>
        //public int IsNotKogakuTotal;

        /// <summary>
        /// マル長適用フラグ
        /// 1:適用
        /// </summary>
        public int IsChoki;

        /// <summary>
        /// 高額療養費限度額
        /// 
        /// </summary>
        public int KogakuLimit;

        /// <summary>
        /// 高額療養費限度額(合算)
        /// 
        /// </summary>
        public int TotalKogakuLimit;

        /// <summary>
        /// 国保減免区分
        /// PT_HOKEN_INF.GENMEN_KBN
        /// </summary>
        public int GenmenKbn;

        /// <summary>
        /// 保険負担率
        /// 
        /// </summary>
        public int HokenRate;

        /// <summary>
        /// 患者負担率
        /// 
        /// </summary>
        public int PtRate;

        /// <summary>
        /// 点数単価
        /// 
        /// </summary>
        public int EnTen;

        /// <summary>
        /// 公１負担限度額
        /// 
        /// </summary>
        public int Kohi1Limit;

        /// <summary>
        /// 公１他院負担額
        /// 
        /// </summary>
        public int Kohi1OtherFutan;

        /// <summary>
        /// 公２負担限度額
        /// 
        /// </summary>
        public int Kohi2Limit;

        /// <summary>
        /// 公２他院負担額
        /// 
        /// </summary>
        public int Kohi2OtherFutan;

        /// <summary>
        /// 公３負担限度額
        /// 
        /// </summary>
        public int Kohi3Limit;

        /// <summary>
        /// 公３他院負担額
        /// 
        /// </summary>
        public int Kohi3OtherFutan;

        /// <summary>
        /// 公４負担限度額
        /// 
        /// </summary>
        public int Kohi4Limit;

        /// <summary>
        /// 公４他院負担額
        /// 
        /// </summary>
        public int Kohi4OtherFutan;

        /// <summary>
        /// 診療点数
        /// 
        /// </summary>
        public int Tensu;

        /// <summary>
        /// 総医療費
        /// 
        /// </summary>
        public int TotalIryohi;

        /// <summary>
        /// 保険負担額
        /// 
        /// </summary>
        public int HokenFutan;

        /// <summary>
        /// 高額負担額
        /// 
        /// </summary>
        public int KogakuFutan;

        /// <summary>
        /// 公１負担額
        /// 
        /// </summary>
        public int Kohi1Futan;

        /// <summary>
        /// 公２負担額
        /// 
        /// </summary>
        public int Kohi2Futan;

        /// <summary>
        /// 公３負担額
        /// 
        /// </summary>
        public int Kohi3Futan;

        /// <summary>
        /// 公４負担額
        /// 
        /// </summary>
        public int Kohi4Futan;

        /// <summary>
        /// 一部負担額
        /// 
        /// </summary>
        public int IchibuFutan;

        /// <summary>
        /// 減免額
        /// 
        /// </summary>
        public int GenmenGaku;

        /// <summary>
        /// 保険負担額(10円単位)
        /// 
        /// </summary>
        public int HokenFutan10en;

        /// <summary>
        /// 高額負担額(10円単位)
        /// 
        /// </summary>
        public int KogakuFutan10en;

        /// <summary>
        /// 公１負担額(10円単位)
        /// 
        /// </summary>
        public int Kohi1Futan10en;

        /// <summary>
        /// 公２負担額(10円単位)
        /// 
        /// </summary>
        public int Kohi2Futan10en;

        /// <summary>
        /// 公３負担額(10円単位)
        /// 
        /// </summary>
        public int Kohi3Futan10en;

        /// <summary>
        /// 公４負担額(10円単位)
        /// 
        /// </summary>
        public int Kohi4Futan10en;

        /// <summary>
        /// 一部負担額(10円単位)
        /// 
        /// </summary>
        public int IchibuFutan10en;

        /// <summary>
        /// 減免額(10円単位)
        /// 
        /// </summary>
        public int GenmenGaku10en;

        /// <summary>
        /// 患者負担額
        /// 
        /// </summary>
        public int PtFutan;

        /// <summary>
        /// 高額療養費超過区分
        /// 
        /// </summary>
        public int KogakuOverKbn;

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
        public string ReceSbt;

        /// <summary>
        /// 労災イ点負担額
        /// 
        /// </summary>
        public int RousaiIFutan;

        /// <summary>
        /// 労災ロ円負担額
        /// 
        /// </summary>
        public int RousaiRoFutan;

        /// <summary>
        /// 自賠イ技術点数
        /// 
        /// </summary>
        public int JibaiITensu;

        /// <summary>
        /// 自賠ロ薬剤点数
        /// 
        /// </summary>
        public int JibaiRoTensu;

        /// <summary>
        /// 自賠ハ円診察負担額
        /// 
        /// </summary>
        public int JibaiHaFutan;

        /// <summary>
        /// 自賠ニ円他負担額
        /// 
        /// </summary>
        public int JibaiNiFutan;

        /// <summary>
        /// 自賠ホ診断書料
        /// 
        /// </summary>
        public int JibaiHoSindan;

        /// <summary>
        /// 自賠ホ診断書料枚数
        /// 
        /// </summary>
        public int JibaiHoSindanCount;

        /// <summary>
        /// 自賠ヘ明細書料
        /// 
        /// </summary>
        public int JibaiHeMeisai;

        /// <summary>
        /// 自賠ヘ明細書料枚数
        /// 
        /// </summary>
        public int JibaiHeMeisaiCount;

        /// <summary>
        /// 自賠Ａ負担額
        /// 
        /// </summary>
        public int JibaiAFutan;

        /// <summary>
        /// 自賠Ｂ負担額
        /// 
        /// </summary>
        public int JibaiBFutan;

        /// <summary>
        /// 自賠Ｃ負担額
        /// 
        /// </summary>
        public int JibaiCFutan;

        /// <summary>
        /// 自賠Ｄ負担額
        /// 
        /// </summary>
        public int JibaiDFutan;

        /// <summary>
        /// 自賠健保点数
        /// 
        /// </summary>
        public int JibaiKenpoTensu;

        /// <summary>
        /// 自賠健保負担額
        /// 
        /// </summary>
        public int JibaiKenpoFutan;


        /// <summary>
        /// 妊婦フラグ
        /// 1:対象
        /// </summary>
        public int IsNinpu;

        /// <summary>
        /// 在医総フラグ
        /// </summary>
        public int IsZaiiso;


        /// <summary>
        /// 請求区分
        /// 1:月遅れ 2:返戻 3:オンライン返戻
        /// </summary>
        public int SeikyuKbn;

        /// <summary>
        /// 公費IDの取得
        /// </summary>
        public int GetKohiId(int kohiNo)
        {
            switch (kohiNo)
            {
                case 1:
                    return Kohi1Id;
                case 2:
                    return Kohi2Id;
                case 3:
                    return Kohi3Id;
                case 4:
                    return Kohi4Id;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 公費法別番号から公費Noの取得
        /// </summary>
        /// <param name="houbetu"></param>
        /// <returns></returns>
        public int GetKohiNo(string houbetu)
        {
            if (Kohi1Houbetu == houbetu) return 1;
            if (Kohi2Houbetu == houbetu) return 2;
            if (Kohi3Houbetu == houbetu) return 3;
            if (Kohi4Houbetu == houbetu) return 4;

            return 0;
        }

        /// <summary>
        /// 公費法別番号の取得
        /// </summary>
        public string GetHoubetu(int kohiNo)
        {
            switch (kohiNo)
            {
                case 1:
                    return Kohi1Houbetu ?? "";
                case 2:
                    return Kohi2Houbetu ?? "";
                case 3:
                    return Kohi3Houbetu ?? "";
                case 4:
                    return Kohi4Houbetu ?? "";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 公費優先順位の取得
        /// </summary>
        public string GetKohiPriority(int kohiNo)
        {
            switch (kohiNo)
            {
                case 1:
                    return Kohi1Priority ?? "".PadLeft(7, '9');
                case 2:
                    return Kohi2Priority ?? "".PadLeft(7, '9');
                case 3:
                    return Kohi3Priority ?? "".PadLeft(7, '9');
                case 4:
                    return Kohi4Priority ?? "".PadLeft(7, '9');
                default:
                    return "".PadLeft(7, '9');
            }
        }

        /// <summary>
        /// 公費負担限度額の取得
        /// </summary>
        public int GetKohiLimit(int kohiNo)
        {
            switch (kohiNo)
            {
                case 1:
                    return Kohi1Limit;
                case 2:
                    return Kohi2Limit;
                case 3:
                    return Kohi3Limit;
                case 4:
                    return Kohi4Limit;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 他院負担額の取得
        /// </summary>
        public int GetOtherFutan(int kohiNo)
        {
            switch (kohiNo)
            {
                case 1:
                    return Kohi1OtherFutan;
                case 2:
                    return Kohi2OtherFutan;
                case 3:
                    return Kohi3OtherFutan;
                case 4:
                    return Kohi4OtherFutan;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 公費負担額の取得
        /// </summary>
        public int GetKohiFutan(int kohiNo)
        {
            switch (kohiNo)
            {
                case 1:
                    return Kohi1Futan;
                case 2:
                    return Kohi2Futan;
                case 3:
                    return Kohi3Futan;
                case 4:
                    return Kohi4Futan;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 公費負担額(10円単位)の取得
        /// </summary>
        public int GetKohiFutan10en(int kohiNo)
        {
            switch (kohiNo)
            {
                case 1:
                    return Kohi1Futan10en;
                case 2:
                    return Kohi2Futan10en;
                case 3:
                    return Kohi3Futan10en;
                case 4:
                    return Kohi4Futan10en;
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 主保険がない場合に生活保護のHokenIdを返す
        /// </summary>
        public int SeihoId
        {
            get
            {
                if (HokenKbn == 1 && ReceSbt?.Substring(1, 1) == "2")
                {
                    return
                        new string[] { "12", "25" }.Contains(Kohi1Houbetu) ? Kohi1Id :
                        new string[] { "12", "25" }.Contains(Kohi2Houbetu) ? Kohi2Id :
                        new string[] { "12", "25" }.Contains(Kohi3Houbetu) ? Kohi3Id :
                        new string[] { "12", "25" }.Contains(Kohi4Houbetu) ? Kohi4Id :
                        0;
                }
                return 0;
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

        /// <summary>
        /// 高額療養費の指定公費計算用（保険分一部負担額）
        /// </summary>
        public int KogakuIchibuFutan
        {
            get
            {
                return
                    AdjustKid == 0 ? IchibuFutan + Kohi1Futan + Kohi2Futan + Kohi3Futan + Kohi4Futan : 0;
            }
        }
    }

}
