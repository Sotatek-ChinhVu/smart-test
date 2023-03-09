namespace Domain.Models.CalculateModel
{

    public class ReceInfModelDto
    {
        public List<ReceInfModel> ReceInfModels { get; set; }
    }
    public class ReceInfModel
    {
        public int hpId { get; set; }
        public int seikyuYm { get; set; }
        public int ptId { get; set; }
        public int sinYm { get; set; }
        public int hokenId { get; set; }
        public int hokenId2 { get; set; }
        public int kohi1Id { get; set; }
        public int kohi2Id { get; set; }
        public int kohi3Id { get; set; }
        public int kohi4Id { get; set; }
        public int hokenKbn { get; set; }
        public int hokenSbtCd { get; set; }
        public string receSbt { get; set; } = string.Empty;
        public string hokensyaNo { get; set; } = string.Empty;
        public string houbetu { get; set; } = string.Empty;
        public string kohi1Houbetu { get; set; } = string.Empty;
        public string kohi2Houbetu { get; set; } = string.Empty;
        public string kohi3Houbetu { get; set; } = string.Empty;
        public string kohi4Houbetu { get; set; } = string.Empty;
        public int honkeKbn { get; set; }
        public int kogakuKbn { get; set; }
        public int kogakuTekiyoKbn { get; set; }
        public int isTokurei { get; set; }
        public int isTasukai { get; set; }
        public int kogakuKohi1Limit { get; set; }
        public int kogakuKohi2Limit { get; set; }
        public int kogakuKohi3Limit { get; set; }
        public int kogakuKohi4Limit { get; set; }
        public int totalKogakuLimit { get; set; }
        public int genmenKbn { get; set; }
        public int hokenRate { get; set; }
        public int ptRate { get; set; }
        public int enTen { get; set; }
        public int kohi1Limit { get; set; }
        public int kohi1OtherFutan { get; set; }
        public int kohi2Limit { get; set; }
        public int kohi2OtherFutan { get; set; }
        public int kohi3Limit { get; set; }
        public int kohi3OtherFutan { get; set; }
        public int kohi4Limit { get; set; }
        public int kohi4OtherFutan { get; set; }
        public int tensu { get; set; }
        public int totalIryohi { get; set; }
        public int hokenFutan { get; set; }
        public int kogakuFutan { get; set; }
        public int kohi1Futan { get; set; }
        public int kohi2Futan { get; set; }
        public int kohi3Futan { get; set; }
        public int kohi4Futan { get; set; }
        public int ichibuFutan { get; set; }
        public int genmenGaku { get; set; }
        public int hokenFutan10en { get; set; }
        public int kogakuFutan10en { get; set; }
        public int kohi1Futan10en { get; set; }
        public int kohi2Futan10en { get; set; }
        public int kohi3Futan10en { get; set; }
        public int kohi4Futan10en { get; set; }
        public int ichibuFutan10en { get; set; }
        public int genmenGaku10en { get; set; }
        public int ptFutan { get; set; }
        public int kogakuOverKbn { get; set; }
        public int hokenTensu { get; set; }
        public int hokenIchibuFutan { get; set; }
        public int hokenIchibuFutan10en { get; set; }
        public int kohi1Tensu { get; set; }
        public int kohi1IchibuSotogaku { get; set; }
        public int kohi1IchibuSotogaku10en { get; set; }
        public int kohi1IchibuFutan { get; set; }
        public int kohi2Tensu { get; set; }
        public int kohi2IchibuSotogaku { get; set; }
        public int kohi2IchibuSotogaku10en { get; set; }
        public int kohi2IchibuFutan { get; set; }
        public int kohi3Tensu { get; set; }
        public int kohi3IchibuSotogaku { get; set; }
        public int kohi3IchibuSotogaku10en { get; set; }
        public int kohi3IchibuFutan { get; set; }
        public int kohi4Tensu { get; set; }
        public int kohi4IchibuSotogaku { get; set; }
        public int kohi4IchibuSotogaku10en { get; set; }
        public int kohi4IchibuFutan { get; set; }
        public int totalIchibuFutan { get; set; }
        public int totalIchibuFutan10en { get; set; }
        public int hokenReceTensu { get; set; }
        public string hokenReceFutan { get; set; } = string.Empty;
        public int? kohi1ReceTensu { get; set; }
        public string kohi1ReceFutan { get; set; } = string.Empty;
        public string kohi1ReceKyufu { get; set; } = string.Empty;
        public string kohi2ReceTensu { get; set; } = string.Empty;
        public string kohi2ReceFutan { get; set; } = string.Empty;
        public string kohi2ReceKyufu { get; set; } = string.Empty;
        public string kohi3ReceTensu { get; set; } = string.Empty;
        public string kohi3ReceFutan { get; set; } = string.Empty;
        public string kohi3ReceKyufu { get; set; } = string.Empty;
        public string kohi4ReceTensu { get; set; } = string.Empty;
        public string kohi4ReceFutan { get; set; } = string.Empty;
        public string kohi4ReceKyufu { get; set; } = string.Empty;
        public int hokenNissu { get; set; }
        public int? kohi1Nissu { get; set; }
        public string kohi2Nissu { get; set; } = string.Empty;
        public string kohi3Nissu { get; set; } = string.Empty;
        public string kohi4Nissu { get; set; } = string.Empty;
        public int kohi1ReceKisai { get; set; }
        public int kohi2ReceKisai { get; set; }
        public int kohi3ReceKisai { get; set; }
        public int kohi4ReceKisai { get; set; }
        public string kohi1NameCd { get; set; } = string.Empty;
        public string kohi2NameCd { get; set; } = string.Empty;
        public string kohi3NameCd { get; set; } = string.Empty;
        public string kohi4NameCd { get; set; } = string.Empty;
        public int seikyuKbn { get; set; }
        public string tokki { get; set; } = string.Empty;
        public string tokki1 { get; set; } = string.Empty;
        public string tokki2 { get; set; } = string.Empty;
        public string tokki3 { get; set; } = string.Empty;
        public string tokki4 { get; set; } = string.Empty;
        public string tokki5 { get; set; } = string.Empty;
        public string ptStatus { get; set; } = string.Empty;
        public int rousaiIFutan { get; set; }
        public int rousaiRoFutan { get; set; }
        public int jibaiITensu { get; set; }
        public int jibaiRoTensu { get; set; }
        public int jibaiHaFutan { get; set; }
        public int jibaiNiFutan { get; set; }
        public int jibaiHoSindan { get; set; }
        public int jibaiHoSindanCount { get; set; }
        public int jibaiHeMeisai { get; set; }
        public int jibaiHeMeisaiCount { get; set; }
        public int jibaiAFutan { get; set; }
        public int jibaiBFutan { get; set; }
        public int jibaiCFutan { get; set; }
        public int jibaiDFutan { get; set; }
        public int jibaiKenpoTensu { get; set; }
        public int jibaiKenpoFutan { get; set; }
        public int sinkei { get; set; }
        public int tenki { get; set; }
        public int kaId { get; set; }
        public int tantoId { get; set; }
        public int isTester { get; set; }
        public int isZaiiso { get; set; }
        public int chokiKbn { get; set; }
        public DateTime createDate { get; set; }
        public int createId { get; set; }
        public string createMachine { get; set; } = string.Empty;
    }
}
