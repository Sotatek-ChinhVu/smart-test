﻿using Helper.Common;
using Helper.Constants;
using Helper.Extension;

namespace Domain.Models.CalculateModel
{

    public class ReceInfModelDto
    {
        public List<ReceInfModel> ReceInfModels { get; set; } = new();
    }
    public class ReceInfModel
    {
        public int IsChoki { get; set; }
        public object Kohi1ReceKyufu10en { get; set; }
        public object Kohi2ReceKyufu10en { get; set; }
        public object Kohi3ReceKyufu10en { get; set; }
        public object Kohi4ReceKyufu10en { get; set; }
        public object Kohi1Priority { get; set; }
        public object Kohi2Priority { get; set; }
        public object Kohi3Priority { get; set; }
        public object Kohi4Priority { get; set; }
        public int Kohi1HokenSbtKbn { get; set; }
        public int Kohi2HokenSbtKbn { get; set; }
        public int Kohi3HokenSbtKbn { get; set; }
        public int Kohi4HokenSbtKbn { get; set; }
        public int IsNinpu { get; set; }
        public int HpId { get; set; }
        public int SeikyuYm { get; set; }
        public int PtId { get; set; }
        public int SinYm { get; set; }
        public int HokenId { get; set; }
        public int HokenId2 { get; set; }
        public int Kohi1Id { get; set; }
        public int Kohi2Id { get; set; }
        public int Kohi3Id { get; set; }
        public int Kohi4Id { get; set; }
        public int HokenKbn { get; set; }
        public int HokenSbtCd { get; set; }
        public string ReceSbt { get; set; }
        public string HokensyaNo { get; set; }
        public string Houbetu { get; set; }
        public string Kohi1Houbetu { get; set; }
        public string Kohi2Houbetu { get; set; }
        public string Kohi3Houbetu { get; set; }
        public string Kohi4Houbetu { get; set; }
        public int HonkeKbn { get; set; }
        public int KogakuKbn { get; set; }
        public int KogakuTekiyoKbn { get; set; }
        public int IsTokurei { get; set; }
        public int IsTasukai { get; set; }
        public int KogakuKohi1Limit { get; set; }
        public int KogakuKohi2Limit { get; set; }
        public int KogakuKohi3Limit { get; set; }
        public int KogakuKohi4Limit { get; set; }
        public int TotalKogakuLimit { get; set; }
        public int GenmenKbn { get; set; }
        public int HokenRate { get; set; }
        public int PtRate { get; set; }
        public int EnTen { get; set; }
        public int Kohi1Limit { get; set; }
        public int Kohi1OtherFutan { get; set; }
        public int Kohi2Limit { get; set; }
        public int Kohi2OtherFutan { get; set; }
        public int Kohi3Limit { get; set; }
        public int Kohi3OtherFutan { get; set; }
        public int Kohi4Limit { get; set; }
        public int Kohi4OtherFutan { get; set; }
        public int Tensu { get; set; }
        public int TotalIryohi { get; set; }
        public int HokenFutan { get; set; }
        public int KogakuFutan { get; set; }
        public int Kohi1Futan { get; set; }
        public int Kohi2Futan { get; set; }
        public int Kohi3Futan { get; set; }
        public int Kohi4Futan { get; set; }
        public int IchibuFutan { get; set; }
        public int GenmenGaku { get; set; }
        public int HokenFutan10en { get; set; }
        public int KogakuFutan10en { get; set; }
        public int Kohi1Futan10en { get; set; }
        public int Kohi2Futan10en { get; set; }
        public int Kohi3Futan10en { get; set; }
        public int Kohi4Futan10en { get; set; }
        public int IchibuFutan10en { get; set; }
        public int GenmenGaku10en { get; set; }
        public int PtFutan { get; set; }
        public int KogakuOverKbn { get; set; }
        public int HokenTensu { get; set; }
        public int HokenIchibuFutan { get; set; }
        public int HokenIchibuFutan10en { get; set; }
        public int Kohi1Tensu { get; set; }
        public int Kohi1IchibuSotogaku { get; set; }
        public int Kohi1IchibuSotogaku10en { get; set; }
        public int Kohi1IchibuFutan { get; set; }
        public int Kohi2Tensu { get; set; }
        public int Kohi2IchibuSotogaku { get; set; }
        public int Kohi2IchibuSotogaku10en { get; set; }
        public int Kohi2IchibuFutan { get; set; }
        public int Kohi3Tensu { get; set; }
        public int Kohi3IchibuSotogaku { get; set; }
        public int Kohi3IchibuSotogaku10en { get; set; }
        public int Kohi3IchibuFutan { get; set; }
        public int Kohi4Tensu { get; set; }
        public int Kohi4IchibuSotogaku { get; set; }
        public int Kohi4IchibuSotogaku10en { get; set; }
        public int Kohi4IchibuFutan { get; set; }
        public int TotalIchibuFutan { get; set; }
        public int TotalIchibuFutan10en { get; set; }
        public int HokenReceTensu { get; set; }
        public object HokenReceFutan { get; set; }
        public object Kohi1ReceTensu { get; set; }
        public object Kohi1ReceFutan { get; set; }
        public object Kohi1ReceKyufu { get; set; }
        public object Kohi2ReceTensu { get; set; }
        public object Kohi2ReceFutan { get; set; }
        public object Kohi2ReceKyufu { get; set; }
        public object Kohi3ReceTensu { get; set; }
        public object Kohi3ReceFutan { get; set; }
        public object Kohi3ReceKyufu { get; set; }
        public object Kohi4ReceTensu { get; set; }
        public object Kohi4ReceFutan { get; set; }
        public object Kohi4ReceKyufu { get; set; }
        public int HokenNissu { get; set; }
        public object Kohi1Nissu { get; set; }
        public object Kohi2Nissu { get; set; }
        public object Kohi3Nissu { get; set; }
        public object Kohi4Nissu { get; set; }
        public bool Kohi1ReceKisai { get; set; }
        public bool Kohi2ReceKisai { get; set; }
        public bool Kohi3ReceKisai { get; set; }
        public bool Kohi4ReceKisai { get; set; }
        public object Kohi1NameCd { get; set; }
        public object Kohi2NameCd { get; set; }
        public object Kohi3NameCd { get; set; }
        public object Kohi4NameCd { get; set; }
        public int SeikyuKbn { get; set; }
        public object Tokki { get; set; }
        public object Tokki1 { get; set; }
        public object Tokki2 { get; set; }
        public object Tokki3 { get; set; }
        public object Tokki4 { get; set; }
        public object Tokki5 { get; set; }
        public object PtStatus { get; set; }
        public int RousaiIFutan { get; set; }
        public int RousaiRoFutan { get; set; }
        public int JibaiITensu { get; set; }
        public int JibaiRoTensu { get; set; }
        public int JibaiHaFutan { get; set; }
        public int JibaiNiFutan { get; set; }
        public int JibaiHoSindan { get; set; }
        public int JibaiHoSindanCount { get; set; }
        public int JibaiHeMeisai { get; set; }
        public int JibaiHeMeisaiCount { get; set; }
        public int JibaiAFutan { get; set; }
        public int JibaiBFutan { get; set; }
        public int JibaiCFutan { get; set; }
        public int JibaiDFutan { get; set; }
        public int JibaiKenpoTensu { get; set; }
        public int JibaiKenpoFutan { get; set; }
        public int Sinkei { get; set; }
        public int Tenki { get; set; }
        public int KaId { get; set; }
        public int TantoId { get; set; }
        public int IsTester { get; set; }
        public int IsZaiiso { get; set; }
        public int ChokiKbn { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreateId { get; set; }
        public object CreateMachine { get; set; }
        public bool IsNoHoken { get; set; }
        public bool IsSiteiKohi { get; set; }
        public bool IsElder { get; set; }
        public bool IsKouki { get; set; }
        public bool IsKokuhoKumiai { get; set; }
        public int AgeKbn { get; set; }
        public int Kohi0Id { get; set; }
        public int Kohi0Limit { get; set; }
        public int Nissu { get; set; }
        public string Kigo { get; set; }
        public string Bango { get; set; }
        public string EdaNo { get; set; }
        public string Kohi1FutansyaNo { get; set; }
        public string Kohi2FutansyaNo { get; set; }
        public string Kohi3FutansyaNo { get; set; }
        public string Kohi4FutansyaNo { get; set; }
        public string Kohi1JyukyusyaNo { get; set; }
        public string Kohi2JyukyusyaNo { get; set; }
        public string Kohi3JyukyusyaNo { get; set; }
        public string Kohi4JyukyusyaNo { get; set; }
        public string Kohi1TokusyuNo { get; set; }
        public string Kohi2TokusyuNo { get; set; }
        public string Kohi3TokusyuNo { get; set; }
        public string Kohi4TokusyuNo { get; set; }
        public int KogakuIchibuFutan { get; set; }
        public int SiteiKohiIchibuFutan { get; set; }

        public string InsuranceName
        {
            get
            {
                string result = "ー";
                switch (HokenKbn)
                {
                    case 0:
                        if (!string.IsNullOrWhiteSpace(ReceSbt) && ReceSbt.Length == 4)
                        {
                            if (ReceSbt[0] == '8')
                            {
                                result = "自費";
                            }
                            else if (ReceSbt[0] == '9')
                            {
                                result = "自費レセ";
                            }

                            if (HaveKohi)
                            {
                                int kohiCount = ReceSbt[2].AsInteger();
                                string prefix = GetKohiCountName(kohiCount);
                                return result + prefix;
                            }
                        }
                        break;
                    case 1:
                        if (ReceiptListConstant.ShaHoDict.ContainsKey(ReceSbt))
                        {
                            result = ReceiptListConstant.ShaHoDict[ReceSbt];
                        }
                        break;
                    case 2:
                        if (ReceiptListConstant.KokuHoDict.ContainsKey(ReceSbt))
                        {
                            result = ReceiptListConstant.KokuHoDict[ReceSbt];
                        }
                        break;
                    case 11:
                        result = "労災(短期給付)";
                        break;
                    case 12:
                        result = "労災(傷病年金)";
                        break;
                    case 13:
                        result = "アフターケア";
                        break;
                    case 14:
                        result = "自賠責";
                        break;
                }

                return result;
            }
        }

        private bool HaveKohi
        {
            get => Kohi1Id > 0 ||
                   Kohi2Id > 0 ||
                   Kohi3Id > 0 ||
                   Kohi4Id > 0;
        }

        private string GetKohiCountName(int kohicount)
        {
            if (kohicount <= 0)
            {
                return string.Empty;
            }
            if (kohicount == 1)
            {
                return "単独";
            }
            else
            {
                return HenkanJ.HankToZen(kohicount.AsString()) + "併";
            }
        }
    }

}
