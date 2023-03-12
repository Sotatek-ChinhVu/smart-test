using Domain.Models.CalculateModel;
using Domain.Models.Insurance;
using Helper.Common;
using Helper.Constants;
using Interactor.CalculateService;
using UseCase.Receipt.GetListReceInf;

namespace Interactor.Receipt
{
    public class GetInsuranceInfInteractor : IGetInsuranceInfInputPort
    {
        private readonly ICalculateService _calculateService;

        public GetInsuranceInfInteractor(ICalculateService calculateService)
        {
            _calculateService = calculateService;
        }

        public GetInsuranceInfOutputData Handle(GetInsuranceInfInputData inputData)
        {
            var receInfs = _calculateService.GetListReceInf(inputData);

            if (!receInfs.ReceInfModels.Any()) return new GetInsuranceInfOutputData(new(), GetInsuranceInfStatus.Failed);
            return new GetInsuranceInfOutputData(EditReceInf(receInfs.ReceInfModels), GetInsuranceInfStatus.Successed);
        }

        private List<InsuranceInfDto> EditReceInf(List<ReceInfModel> receInfs)
        {
            var insuranceInfDtos = new List<InsuranceInfDto>();

            foreach (var receInf in receInfs)
            {
                var hokenTitle1 = string.Empty;
                var hokenTitle2 = string.Empty;
                var hokenKigoBangoTitle = string.Empty;
                var hokensyaNo = string.Empty;
                var kigoBango = string.Empty;
                var bango = string.Empty;
                var kohi1No1Value = string.Empty;
                var kohi1No1Value1 = string.Empty;
                var kohi1No1Value2 = string.Empty;
                var kohi1No2Value = string.Empty;
                var kohi1No2Value1 = string.Empty;
                var kohi1No2Value2 = string.Empty;
                var kohi2No1Value = string.Empty;
                var kohi2No1Value1 = string.Empty;
                var kohi2No1Value2 = string.Empty;
                var kohi2No2Value = string.Empty;
                var kohi2No2Value1 = string.Empty;
                var kohi2No2Value2 = string.Empty;
                CreateNewInsuranceInf(out string hokenTitle, out string hokenKigoTitle, out string hokenBangoTitle, out string kohi1No1Title,
                                        out string kohi1No2Title, out string kohi2No1Title, out string kohi2No2Title, out string nissuTitle,
                                        out string tensuTitle, out string futanTitle);

                if (receInf.HokenKbn <= 2)
                {
                    EditReceInfByHoken(receInf, out hokenTitle, out hokenKigoTitle,
                                        out hokenBangoTitle, out kohi1No1Title, out kohi1No2Title, out kohi2No1Title,
                                        out kohi2No2Title, out nissuTitle, out tensuTitle, out futanTitle,
                                        out hokensyaNo, out string edaNo, out kigoBango, out string kigo, out bango,
                                        out kohi1No1Value, out kohi1No2Value, out kohi1No1Value2, out kohi1No2Value2,
                                        out kohi2No1Value, out kohi2No2Value, out kohi2No1Value1, out kohi2No2Value1,
                                        out kohi2No1Value2, out kohi2No2Value2, out kohi1No1Value1, out kohi1No2Value1);
                }
                else if (receInf.HokenKbn >= 11 && receInf.HokenKbn <= 13)
                {
                    EditReceInfByRosai(receInf, out hokenTitle, out hokenKigoBangoTitle, out hokenKigoTitle,
                                        out hokenBangoTitle, out kohi1No1Title, out kohi1No2Title, out kohi2No1Title,
                                        out kohi2No2Title, out nissuTitle, out tensuTitle, out futanTitle,
                                        out hokenTitle1, out hokenTitle2, out hokensyaNo);
                }
                else if (receInf.HokenKbn == 14)
                {
                    EditReceInfByJibai(out hokenTitle, out hokenKigoBangoTitle, out hokenKigoTitle, out hokenBangoTitle,
                                        out kohi1No1Title, out kohi1No2Title, out kohi2No1Title, out kohi2No2Title, out nissuTitle,
                                        out tensuTitle, out futanTitle);
                }

                var nissu = receInf.Nissu;
                var tensu = CIUtil.FormatIntToString(receInf.Tensu);
                var ichibuFutan = CIUtil.FormatIntToString(receInf.PtFutan);

                if (string.IsNullOrWhiteSpace(tensu))
                    tensu = "0";
                if (string.IsNullOrWhiteSpace(ichibuFutan))
                    ichibuFutan = "0";

                insuranceInfDtos.Add(new InsuranceInfDto(hokenTitle, hokenTitle1, hokenTitle2, hokenKigoBangoTitle, hokenKigoTitle,
                                    hokenBangoTitle, kohi1No1Title, kohi1No2Title, kohi2No1Title, kohi2No2Title, hokensyaNo, kigoBango, bango,
                                    kohi1No1Value, kohi1No1Value1, kohi1No1Value2, kohi1No2Value, kohi1No2Value1, kohi1No2Value2, kohi2No1Value,
                                    kohi2No1Value1, kohi2No1Value2, kohi2No2Value, kohi2No2Value1, kohi2No2Value2, nissuTitle, tensuTitle, futanTitle,
                                    nissu, tensu, ichibuFutan));
            }
            return insuranceInfDtos;

        }

        private void EditReceInfByHoken(ReceInfModel receInf, out string hokenTitle, out string hokenKigoTitle,
            out string hokenBangoTitle, out string kohi1No1Title, out string kohi1No2Title, out string kohi2No1Title,
            out string kohi2No2Title, out string nissuTitle, out string tensuTitle, out string futanTitle,
            out string hokensyaNo, out string edaNo, out string kigoBango, out string kigo, out string bango,
            out string kohi1No1Value, out string kohi1No2Value, out string kohi1No1Value2, out string kohi1No2Value2,
            out string kohi2No1Value, out string kohi2No2Value, out string kohi2No1Value1, out string kohi2No2Value1,
            out string kohi2No1Value2, out string kohi2No2Value2, out string kohi1No1Value1, out string kohi1No2Value1)
        {
            hokenTitle = "保険";
            hokenKigoTitle = "記号";
            hokenBangoTitle = "番号";

            kohi1No1Title = "公負①";
            kohi1No2Title = "公受①";
            kohi2No1Title = "公負②";
            kohi2No2Title = "公受②";

            nissuTitle = "実日数";
            tensuTitle = "請求点数";
            futanTitle = "一部負担金";

            hokensyaNo = receInf.HokensyaNo;
            edaNo = (receInf.HokenKbn > 0 && (receInf.HokenKbn < 11 || receInf.HokenKbn > 14)) ? receInf.EdaNo : string.Empty;

            kigoBango = string.Empty;
            kigo = string.Empty;
            bango = string.Empty;
            kohi1No1Value = string.Empty;
            kohi1No2Value = string.Empty;
            kohi1No1Value1 = string.Empty;
            kohi1No1Value2 = string.Empty;
            kohi1No2Value2 = string.Empty;
            kohi2No1Value = string.Empty;
            kohi2No2Value = string.Empty;
            kohi2No1Value1 = string.Empty;
            kohi2No2Value1 = string.Empty;
            kohi2No1Value2 = string.Empty;
            kohi1No2Value1 = string.Empty;
            kohi2No2Value2 = string.Empty;

            if (string.IsNullOrEmpty(receInf.Kigo) || string.IsNullOrEmpty(receInf.Bango))
            {

                if (!string.IsNullOrEmpty(receInf.Kigo))
                {
                    kigoBango = receInf.Kigo;
                }
                else
                {
                    kigoBango = receInf.Bango;
                }
            }
            else
            {
                kigo = receInf.Kigo;
                bango = receInf.Bango;
            }

            var kohi1 = new ReceInfKohi();
            var kohi2 = new ReceInfKohi();
            var kohi3 = new ReceInfKohi();
            var kohi4 = new ReceInfKohi();

            if (receInf.Kohi1ReceKisai == true && receInf.Kohi1Id > 0)
            {
                kohi1.IsExitData = true;
                kohi1.FutansyaNo = receInf.Kohi1FutansyaNo;
                kohi1.JyukyusyaNo = receInf.Kohi1JyukyusyaNo;
            }

            if (receInf.Kohi2ReceKisai == true && receInf.Kohi2Id > 0)
            {
                ReceInfKohi kohi = kohi2;
                if (kohi1.IsExitData == false) kohi = kohi1;

                kohi.IsExitData = true;
                kohi.FutansyaNo = receInf.Kohi2FutansyaNo;
                kohi.JyukyusyaNo = receInf.Kohi2JyukyusyaNo;
            }

            if (receInf.Kohi3ReceKisai == true && receInf.Kohi3Id > 0)
            {
                ReceInfKohi kohi = kohi3;
                if (kohi1.IsExitData == false)
                {
                    kohi = kohi1;
                }
                else if (kohi2.IsExitData == false)
                {
                    kohi = kohi2;
                }

                kohi.IsExitData = true;
                kohi.FutansyaNo = receInf.Kohi3FutansyaNo;
                kohi.JyukyusyaNo = receInf.Kohi3JyukyusyaNo;
            }

            if (receInf.Kohi4ReceKisai == true && receInf.Kohi3Id > 0)
            {
                ReceInfKohi kohi = kohi4;
                if (kohi1.IsExitData == false)
                {
                    kohi = kohi1;
                }
                else if (kohi2.IsExitData == false)
                {
                    kohi = kohi2;
                }
                else if (kohi3.IsExitData == false)
                {
                    kohi = kohi3;
                }

                kohi.IsExitData = true;
                kohi.FutansyaNo = receInf.Kohi4FutansyaNo;
                kohi.JyukyusyaNo = receInf.Kohi4JyukyusyaNo;
            }

            if (kohi3.IsExitData == false)
            {
                if (kohi1.IsExitData == true)
                {
                    kohi1No1Value = kohi1.FutansyaNo;
                    kohi1No2Value = kohi1.JyukyusyaNo;
                }
                if (kohi2.IsExitData == true)
                {
                    kohi2No1Value = kohi2.FutansyaNo;
                    kohi2No2Value = kohi2.JyukyusyaNo;
                }
            }
            else if (kohi4.IsExitData == false)
            {
                kohi1No1Value1 = kohi1.FutansyaNo;
                kohi1No2Value1 = kohi1.JyukyusyaNo;

                kohi1No1Value2 = kohi2.FutansyaNo;
                kohi1No2Value2 = kohi2.JyukyusyaNo;


                kohi2No1Value = kohi3.FutansyaNo;
                kohi2No2Value = kohi3.JyukyusyaNo;
            }
            else
            {
                kohi1No1Value1 = kohi1.FutansyaNo;
                kohi1No2Value1 = kohi1.JyukyusyaNo;

                kohi1No1Value2 = kohi2.FutansyaNo;
                kohi1No2Value2 = kohi2.JyukyusyaNo;

                kohi2No1Value1 = kohi3.FutansyaNo;
                kohi2No2Value1 = kohi3.JyukyusyaNo;

                kohi2No1Value2 = kohi4.FutansyaNo;
                kohi2No2Value2 = kohi4.JyukyusyaNo;
            }
        }

        private void CreateNewInsuranceInf(out string hokenTitle, out string hokenKigoTitle, out string hokenBangoTitle, out string kohi1No1Title, out string kohi1No2Title, out string kohi2No1Title, out string kohi2No2Title, out string nissuTitle, out string tensuTitle, out string futanTitle)
        {
            hokenTitle = "保険";
            hokenKigoTitle = "記号";
            hokenBangoTitle = "番号";

            kohi1No1Title = "公負①";
            kohi1No2Title = "公受①";
            kohi2No1Title = "公負②";
            kohi2No2Title = "公受②";

            nissuTitle = "実日数";
            tensuTitle = "請求点数";
            futanTitle = "一部負担金";
        }

        private void EditReceInfByRosai(ReceInfModel receInf, out string hokenTitle, out string hokenKigoBangoTitle, out string hokenKigoTitle,
                                        out string hokenBangoTitle, out string kohi1No1Title, out string kohi1No2Title, out string kohi2No1Title,
                                        out string kohi2No2Title, out string nissuTitle, out string tensuTitle, out string futanTitle,
                                        out string hokenTitle1, out string hokenTitle2, out string hokensyaNo)
        {
            hokenTitle = "";
            hokenKigoBangoTitle = "ー";
            hokenKigoTitle = "";
            hokenBangoTitle = "";

            kohi1No1Title = "ー";
            kohi1No2Title = "ー";
            kohi2No1Title = "ー";
            kohi2No2Title = "ー";

            nissuTitle = "（イ）";
            tensuTitle = "（ロ）";
            futanTitle = "合計額";

            hokenTitle1 = string.Empty;
            hokenTitle2 = string.Empty;

            if (receInf.HokenKbn == 11)
            {
                hokenTitle1 = "労働保険";
                hokenTitle2 = "番       号";
            }
            else if (receInf.HokenKbn == 12)
            {
                hokenTitle1 = "年金証書";
                hokenTitle2 = "番       号";
            }
            else if (receInf.HokenKbn == 13)
            {
                hokenTitle1 = "健康管理";
                hokenTitle2 = "手帳番号";
            }

            hokensyaNo = receInf.HokensyaNo;
        }

        private void EditReceInfByJibai(out string hokenTitle, out string hokenKigoBangoTitle, out string hokenKigoTitle, out string hokenBangoTitle,
            out string kohi1No1Title, out string kohi1No2Title, out string kohi2No1Title, out string kohi2No2Title, out string nissuTitle,
            out string tensuTitle, out string futanTitle)
        {
            hokenTitle = "ー";
            hokenKigoBangoTitle = "ー";
            hokenKigoTitle = "";
            hokenBangoTitle = "";

            kohi1No1Title = "ー";
            kohi1No2Title = "ー";
            kohi2No1Title = "ー";
            kohi2No2Title = "ー";

            nissuTitle = "点数";
            tensuTitle = "金額";
            futanTitle = "文書料";
        }
    }
}
