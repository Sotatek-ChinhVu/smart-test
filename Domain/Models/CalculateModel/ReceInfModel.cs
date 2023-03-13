using Helper.Constants;
using Helper.Extension;

namespace Domain.Models.CalculateModel
{

    public class ReceInfModelDto
    {
        public List<ReceInfModel> ReceInfModels { get; set; } = new();
    }
    public class ReceInf
    {
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
        public string ReceSbt { get; set; } = string.Empty;
        public string HokensyaNo { get; set; } = string.Empty;
        public string Houbetu { get; set; } = string.Empty;
        public string Kohi1Houbetu { get; set; } = string.Empty;
        public string Kohi2Houbetu { get; set; } = string.Empty;
        public string Kohi3Houbetu { get; set; } = string.Empty;
        public string Kohi4Houbetu { get; set; } = string.Empty;
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
        public int? HokenReceFutan { get; set; }
        public int? kohi1ReceTensu { get; set; }
        public int? Kohi1ReceFutan { get; set; }
        public int? Kohi1ReceKyufu { get; set; }
        public int? Kohi2ReceTensu { get; set; }
        public int? Kohi2ReceFutan { get; set; }
        public int? Kohi2ReceKyufu { get; set; }
        public int? Kohi3ReceTensu { get; set; }
        public int? Kohi3ReceFutan { get; set; }
        public int? Kohi3ReceKyufu { get; set; }
        public int? Kohi4ReceTensu { get; set; }
        public int? Kohi4ReceFutan { get; set; }
        public int? Kohi4ReceKyufu { get; set; }
        public int HokenNissu { get; set; }
        public int? Kohi1Nissu { get; set; }
        public int? Kohi2Nissu { get; set; }
        public int? Kohi3Nissu { get; set; }
        public int? Kohi4Nissu { get; set; }
        public int Kohi1ReceKisai { get; set; }
        public int Kohi2ReceKisai { get; set; }
        public int Kohi3ReceKisai { get; set; }
        public int Kohi4ReceKisai { get; set; }
        public string Kohi1NameCd { get; set; } = string.Empty;
        public string Kohi2NameCd { get; set; } = string.Empty;
        public string Kohi3NameCd { get; set; } = string.Empty;
        public string Kohi4NameCd { get; set; } = string.Empty;
        public int SeikyuKbn { get; set; }
        public string Tokki { get; set; } = string.Empty;
        public string Tokki1 { get; set; } = string.Empty;
        public string Tokki2 { get; set; } = string.Empty;
        public string Tokki3 { get; set; } = string.Empty;
        public string Tokki4 { get; set; } = string.Empty;
        public string Tokki5 { get; set; } = string.Empty;
        public string ptStatus { get; set; } = string.Empty;
        public int rousaiIFutan { get; set; }
        public int rousaiRoFutan { get; set; }
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
        public DateTime createDate { get; set; }
        public int CreateId { get; set; }
        public string createMachine { get; set; } = string.Empty;
    }

    public class ReceInfModel
    {
        public ReceInf ReceInf { get; set; } = new();
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
        public string receSbt { get; set; } = string.Empty;
        public string HokensyaNo { get; set; } = string.Empty;
        public string Houbetu { get; set; } = string.Empty;
        public string Kohi1Houbetu { get; set; } = string.Empty;
        public string Kohi2Houbetu { get; set; } = string.Empty;
        public string Kohi3Houbetu { get; set; } = string.Empty;
        public string Kohi4Houbetu { get; set; } = string.Empty;
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
        public int genmenKbn { get; set; }
        public int HokenRate { get; set; }
        public int PtRate { get; set; }
        public int enTen { get; set; }
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
        public int genmenGaku { get; set; }
        public int HokenFutan10en { get; set; }
        public int KogakuFutan10en { get; set; }
        public int Kohi1Futan10en { get; set; }
        public int Kohi2Futan10en { get; set; }
        public int Kohi3Futan10en { get; set; }
        public int Kohi4Futan10en { get; set; }
        public int IchibuFutan10en { get; set; }
        public int genmenGaku10en { get; set; }
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
        public int? HokenReceFutan { get; set; }
        public int? kohi1ReceTensu { get; set; }
        public int? Kohi1ReceFutan { get; set; }
        public int? Kohi1ReceKyufu { get; set; }
        public int? Kohi2ReceTensu { get; set; }
        public int? Kohi2ReceFutan { get; set; }
        public int? Kohi2ReceKyufu { get; set; }
        public int? Kohi3ReceTensu { get; set; }
        public int? Kohi3ReceFutan { get; set; }
        public int? Kohi3ReceKyufu { get; set; }
        public int? Kohi4ReceTensu { get; set; }
        public int? Kohi4ReceFutan { get; set; }
        public int? Kohi4ReceKyufu { get; set; }
        public int HokenNissu { get; set; }
        public int? Kohi1Nissu { get; set; }
        public int? Kohi2Nissu { get; set; }
        public int? Kohi3Nissu { get; set; }
        public int? Kohi4Nissu { get; set; }
        public bool Kohi1ReceKisai { get; set; }
        public bool Kohi2ReceKisai { get; set; }
        public bool Kohi3ReceKisai { get; set; }
        public bool Kohi4ReceKisai { get; set; }
        public string Kohi1NameCd { get; set; } = string.Empty;
        public string Kohi2NameCd { get; set; } = string.Empty;
        public string Kohi3NameCd { get; set; } = string.Empty;
        public string Kohi4NameCd { get; set; } = string.Empty;
        public int SeikyuKbn { get; set; }
        public string Tokki { get; set; } = string.Empty;
        public string Tokki1 { get; set; } = string.Empty;
        public string Tokki2 { get; set; } = string.Empty;
        public string Tokki3 { get; set; } = string.Empty;
        public string Tokki4 { get; set; } = string.Empty;
        public string Tokki5 { get; set; } = string.Empty;
        public string PtStatus { get; set; } = string.Empty;
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
        public DateTime createDate { get; set; }
        public int CreateId { get; set; }
        public string CreateMachine { get; set; } = string.Empty;
        public string Kohi1Priority { get; set; } = string.Empty;
        public string Kohi2Priority { get; set; } = string.Empty;
        public string Kohi3Priority { get; set; } = string.Empty;
        public string Kohi4Priority { get; set; } = string.Empty;
        public int Kohi1HokenSbtKbn { get; set; }
        public int Kohi2HokenSbtKbn { get; set; }
        public int Kohi3HokenSbtKbn { get; set; }
        public int Kohi4HokenSbtKbn { get; set; }
        public int IsNinpu { get; set; }
        public bool IsNoHoken { get; set; }
        public bool IsSiteiKohi { get; set; }
        public bool IsElder { get; set; }
        public bool IsKouki { get; set; }
        public bool IsKokuhoKumiai { get; set; }
        public int AgeKbn { get; set; }
        public int Kohi0Id { get; set; }
        public int Kohi0Limit { get; set; }
        public int Nissu { get; set; }
        public string Kigo { get; set; } = string.Empty;
        public string Bango { get; set; } = string.Empty;
        public string EdaNo { get; set; } = string.Empty;
        public string Kohi1FutansyaNo { get; set; } = string.Empty;
        public string Kohi2FutansyaNo { get; set; } = string.Empty;
        public string Kohi3FutansyaNo { get; set; } = string.Empty;
        public string Kohi4FutansyaNo { get; set; } = string.Empty;
        public string Kohi1JyukyusyaNo { get; set; } = string.Empty;
        public string Kohi2JyukyusyaNo { get; set; } = string.Empty;
        public string Kohi3JyukyusyaNo { get; set; } = string.Empty;
        public string Kohi4JyukyusyaNo { get; set; } = string.Empty;
        public string Kohi1TokusyuNo { get; set; } = string.Empty;
        public string Kohi2TokusyuNo { get; set; } = string.Empty;
        public string Kohi3TokusyuNo { get; set; } = string.Empty;
        public string Kohi4TokusyuNo { get; set; } = string.Empty;
        public int KogakuIchibuFutan { get; set; }
        public int SiteiKohiIchibuFutan { get; set; }
        public string InsuranceName
        {
            get
            {
                string hokenName = HokenId.AsString().PadLeft(2, '0') + " ";

                string prefix = string.Empty;
                string postfix = string.Empty;
                if (HokenSbtCd == 0)
                {
                    switch (HokenKbn)
                    {
                        case 0:
                            if (HokenId > 0)
                            {
                                if (Houbetu == HokenConstant.HOUBETU_JIHI_108)
                                {
                                    hokenName += "自費";
                                }
                                else if (Houbetu == HokenConstant.HOUBETU_JIHI_109)
                                {
                                    hokenName += "自費レセ";
                                }
                            }
                            else
                            {
                                hokenName += "自費";
                            }
                            break;
                        case 11:
                            hokenName += "労災（短期給付）";
                            break;
                        case 12:
                            hokenName += "労災（傷病年金）";
                            break;
                        case 13:
                            hokenName += "労災（アフターケア）";
                            break;
                        case 14:
                            hokenName += "自賠責";
                            break;
                    }
                }
                else
                {
                    if (HokenSbtCd < 0)
                    {
                        return hokenName;
                    }
                    string hokenSbtCd = HokenSbtCd.AsString().PadRight(3, '0');
                    int firstNum = hokenSbtCd[0].AsInteger();
                    int secondNum = hokenSbtCd[1].AsInteger();
                    int thirNum = hokenSbtCd[2].AsInteger();
                    switch (firstNum)
                    {
                        case 1:
                            hokenName += "社保";
                            break;
                        case 2:
                            hokenName += "国保";
                            break;
                        case 3:
                            hokenName += "後期";
                            break;
                        case 4:
                            hokenName += "退職";
                            break;
                        case 5:
                            hokenName += "公費";
                            break;
                    }

                    if (secondNum > 0)
                    {

                        if (thirNum == 1)
                        {
                            prefix += "単独";
                        }
                        else
                        {
                            prefix += thirNum + "併";
                        }

                        if (Kohi1Id > 0 && Kohi1HokenSbtKbn != 2)
                        {
                            if (!string.IsNullOrEmpty(postfix))
                            {
                                postfix += "+";
                            }
                            postfix += Kohi1Houbetu;
                        }

                        if (Kohi2Id > 0 && Kohi2HokenSbtKbn != 2)
                        {
                            if (!string.IsNullOrEmpty(postfix))
                            {
                                postfix += "+";
                            }
                            postfix += Kohi2Houbetu;
                        }

                        if (Kohi3Id > 0 && Kohi3HokenSbtKbn != 2)
                        {
                            if (!string.IsNullOrEmpty(postfix))
                            {
                                postfix += "+";
                            }
                            postfix += Kohi3Houbetu;
                        }

                        if (Kohi4Id > 0 && Kohi4HokenSbtKbn != 2)
                        {
                            if (!string.IsNullOrEmpty(postfix))
                            {
                                postfix += "+";
                            }
                            postfix += Kohi4Houbetu;
                        }
                    }
                }

                if (!string.IsNullOrEmpty(postfix))
                {
                    return hokenName + prefix + "(" + postfix + ")";
                }
                return hokenName + prefix;
            }
        }

    }
}
