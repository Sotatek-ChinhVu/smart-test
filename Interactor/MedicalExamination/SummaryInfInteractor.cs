using Domain.Models.PatientInfor;
using Domain.Models.Santei;
using Domain.Models.SpecialNote.ImportantNote;
using Domain.Models.SpecialNote.PatientInfo;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using System.Text;
using UseCase.MedicalExamination.CheckedAfter327Screen;
using UseCase.MedicalExamination.SummaryInf;
using SpecialNotePatienInfDomain = Domain.Models.SpecialNote.PatientInfo;

namespace Interactor.MedicalExamination
{
    public class SummaryInfInteractor
    {
        private const string Space = " ";
        private readonly IPatientInforRepository _patientInfRepository;
        private readonly SpecialNotePatienInfDomain.IPatientInfoRepository _specialNotePatientInfRepository;
        private readonly IImportantNoteRepository _importantNoteRepository;
        private readonly ISanteiInfRepository _santeiInfRepository;
        public SummaryInfInteractor(IPatientInforRepository patientInfRepository, SpecialNotePatienInfDomain.IPatientInfoRepository specialNotePatientInfRepository, IImportantNoteRepository importantNoteRepository, ISanteiInfRepository santeiInfRepository)
        {
            _patientInfRepository = patientInfRepository;
            _specialNotePatientInfRepository = specialNotePatientInfRepository;
            _importantNoteRepository = importantNoteRepository;
            _santeiInfRepository = santeiInfRepository;
        }

        public CheckedAfter327ScreenOutputData Handle(CheckedAfter327ScreenInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                {
                    return new CheckedAfter327ScreenOutputData(CheckedAfter327ScreenStatus.InvalidHpId, new(), new());
                }
                if (inputData.PtId <= 0)
                {
                    return new CheckedAfter327ScreenOutputData(CheckedAfter327ScreenStatus.InvalidPtId, new(), new());
                }
                if (inputData.SinDate < 0)
                {
                    return new CheckedAfter327ScreenOutputData(CheckedAfter327ScreenStatus.InvalidSinDate, new(), new());
                }

                var data = _medicalExaminationRepository.GetCheckedAfter327Screen(inputData.HpId, inputData.PtId, inputData.SinDate, inputData.CheckedOrderModels, inputData.IsTokysyoOrder, inputData.IsTokysyosenOrder);

                return new CheckedAfter327ScreenOutputData(CheckedAfter327ScreenStatus.Successed, data.Item1, data.Item2);
            }
            finally
            {
                _medicalExaminationRepository.ReleaseResource();
            }
        }


        private void GetPhysicalInfo(int hpId, long ptId, int sinDate, SummaryInfItem summaryInfItem)
        {
            int grpItemCd = 1;
            string headerName = "■身体情報";
            string headerInfo = summaryInfItem.HeaderInfo;
            List<KensaInfDetailModel> listKensaInfDetailModel = _specialNotePatientInfRepository.GetListKensaInfDetailModel(hpId, ptId, sinDate);
            long maxSortNo = listKensaInfDetailModel.Max(u => u.SortNo);
            KensaInfDetailModel heightModel = listKensaInfDetailModel.Where(u => u.KensaItemCd == IraiCodeConstant.HEIGHT_CODE).FirstOrDefault() ?? new();
            KensaInfDetailModel weightModel = listKensaInfDetailModel.Where(u => u.KensaItemCd == IraiCodeConstant.WEIGHT_CODE).FirstOrDefault() ?? new();
            KensaInfDetailModel bmiModel = listKensaInfDetailModel.Where(u => u.KensaItemCd == IraiCodeConstant.BMI_CODE).FirstOrDefault() ?? new();
            var newlistKensaInfDetailModel = new List<KensaInfDetailModel>();
            newlistKensaInfDetailModel.AddRange(listKensaInfDetailModel.Where(u => u != bmiModel));

            if (heightModel == null ||
                weightModel == null ||
                heightModel.ResultVal.AsDouble() <= 0 ||
                weightModel.ResultVal.AsDouble() <= 0)
            {
                if (bmiModel != null)
                {
                    listKensaInfDetailModel.Remove(bmiModel);
                }
            }
            else
            {
                string bmi = string.Format("{0:0.0}", weightModel.ResultVal.AsDouble() / (heightModel.ResultVal.AsDouble() * heightModel.ResultVal.AsDouble() / 10000));
                if (bmiModel != null)
                {
                    string resultVal = bmi;
                    int iraiDate = heightModel.IraiDate >= weightModel.IraiDate ? heightModel.IraiDate : weightModel.IraiDate;
                    var newBMIModel = new KensaInfDetailModel(iraiDate, resultVal);
                    newlistKensaInfDetailModel.Add(newBMIModel);
                }
                else
                {
                    if (bmi.AsDouble() > 0)
                    {
                        listKensaInfDetailModel.Add(
                        new KensaInfDetailModel(
                            ++maxSortNo,
                            "BMI",
                            IraiCodeConstant.BMI_CODE,
                            bmi
                        ));
                    }
                }
            }
            newlistKensaInfDetailModel = newlistKensaInfDetailModel.OrderBy(u => u.SortNo).ToList();
            foreach (var kensaDetailModel in newlistKensaInfDetailModel)
            {
                string sSate = CIUtil.SDateToShowSDate(kensaDetailModel.IraiDate);
                string kensaName = string.Empty;
                if (kensaDetailModel.KensaItemCd != IraiCodeConstant.HEIGHT_CODE && kensaDetailModel.KensaItemCd != IraiCodeConstant.WEIGHT_CODE)
                {
                    kensaName = kensaDetailModel.KensaName;
                }
                else
                {
                    if (string.IsNullOrEmpty(kensaDetailModel.ResultVal))
                    {
                        continue;
                    }
                }
                if (!string.IsNullOrEmpty(kensaDetailModel.ResultVal))
                {
                    headerInfo += (string.IsNullOrEmpty(kensaName) ? string.Empty : kensaName + ":" + Space) + kensaDetailModel.ResultVal + kensaDetailModel.Unit + Space + (string.IsNullOrEmpty(sSate) ? string.Empty : "(" + sSate + ")") + Space + "/";
                }
            }
            headerInfo = headerInfo.TrimEnd('/') ?? string.Empty;

            summaryInfItem = new SummaryInfItem(headerInfo, headerName, string.Empty, 0, 0, 0, grpItemCd, string.Empty);
        }

        private void GetDrugInfo(long ptId, int sinDate, SummaryInfItem summaryInfItem)
        {
            int grpItemCd = 2;
            string headerName = "◆アレルギー";
            StringBuilder headerInf = new StringBuilder();
            List<PtAlrgyElseModel> listPtAlrgyElseModel = new List<PtAlrgyElseModel>();
            List<PtAlrgyFoodModel> listPtAlrgyFoodModel = new List<PtAlrgyFoodModel>();
            List<PtAlrgyDrugModel> listPtAlrgyDrugModel = new List<PtAlrgyDrugModel>();

            listPtAlrgyElseModel = _importantNoteRepository.GetAlrgyElseList(ptId, sinDate);
            listPtAlrgyFoodModel = _importantNoteRepository.GetAlrgyFoodList(ptId, sinDate);
            listPtAlrgyDrugModel = _importantNoteRepository.GetAlrgyDrugList(ptId, sinDate);

            foreach (var ptAlrgyDrugModel in listPtAlrgyDrugModel)
            {
                if (!string.IsNullOrEmpty(ptAlrgyDrugModel.DrugName))
                {
                    headerInf.Append(ptAlrgyDrugModel.DrugName);
                    if (!string.IsNullOrEmpty(ptAlrgyDrugModel.Cmt))
                    {
                        headerInf.Append("／" + ptAlrgyDrugModel.Cmt);
                    }
                    headerInf.Append(Environment.NewLine);
                }
            }

            foreach (var ptAlrgyFoodModel in listPtAlrgyFoodModel)
            {
                if (!string.IsNullOrEmpty(ptAlrgyFoodModel.FoodName))
                {
                    headerInf.Append(ptAlrgyFoodModel.FoodName);
                    if (!string.IsNullOrEmpty(ptAlrgyFoodModel.Cmt))
                    {
                        headerInf.Append("／" + ptAlrgyFoodModel.Cmt);
                    }
                    headerInf.Append(Environment.NewLine);
                }
            }

            foreach (var ptAlrgyElseModel in listPtAlrgyElseModel)
            {
                if (!string.IsNullOrEmpty(ptAlrgyElseModel.AlrgyName))
                {
                    headerInf.Append(ptAlrgyElseModel.AlrgyName);
                    if (!string.IsNullOrEmpty(ptAlrgyElseModel.Cmt))
                    {
                        headerInf.Append("／" + ptAlrgyElseModel.Cmt);
                    }
                    headerInf.Append(Environment.NewLine);
                }
            }

            string strHeaderInf = headerInf.ToString().TrimEnd(Environment.NewLine.ToCharArray());

            summaryInfItem = new SummaryInfItem(strHeaderInf, headerName, string.Empty, 0, 0, 0, grpItemCd, string.Empty);
        }

        private void GetPathologicalStatus(long ptId, SummaryInfItem summaryInfItem)
        {
            int grpItemCd = 3;
            string headerName = "◆病歴";
            StringBuilder headerInfo = new StringBuilder();
            List<PtKioRekiModel> listPtKioRekiModel = new List<PtKioRekiModel>();
            List<PtInfectionModel> listPtInfectionModel = new List<PtInfectionModel>();

            listPtKioRekiModel = _importantNoteRepository.GetKioRekiList(ptId);
            listPtInfectionModel = _importantNoteRepository.GetInfectionList(ptId);

            foreach (var ptKioRekiModel in listPtKioRekiModel)
            {
                if (!string.IsNullOrEmpty(ptKioRekiModel.Byomei))
                {
                    headerInfo.Append(ptKioRekiModel.Byomei);
                    if (!string.IsNullOrEmpty(ptKioRekiModel.Cmt))
                    {
                        headerInfo.Append("／" + ptKioRekiModel.Cmt);
                    }
                    headerInfo.Append(Environment.NewLine);
                }
            }
            foreach (var ptInfectionModel in listPtInfectionModel)
            {
                if (!string.IsNullOrEmpty(ptInfectionModel.Byomei))
                {
                    headerInfo.Append(ptInfectionModel.Byomei);
                    if (!string.IsNullOrEmpty(ptInfectionModel.Cmt))
                    {
                        headerInfo.Append("／" + ptInfectionModel.Cmt);
                    }
                    headerInfo.Append(Environment.NewLine);
                }
            }
            string strHeaderInfo = headerInfo.ToString().TrimEnd(Environment.NewLine.ToCharArray());
            summaryInfItem = new SummaryInfItem(strHeaderInfo, headerName, string.Empty, 0, 0, 0, grpItemCd, string.Empty);
        }

        private void GetInteraction(long ptId, int sinDate, SummaryInfItem summaryInfItem)
        {
            int grpItemCd = 4;
            string headerName = "◆服薬情報";
            StringBuilder headerInf = new StringBuilder();
            List<PtOtherDrugModel> listPtOtherDrugModel = new List<PtOtherDrugModel>();
            List<PtOtcDrugModel> listPtOtcDrugModel = new List<PtOtcDrugModel>();
            List<PtSuppleModel> listPtSuppleModel = new List<PtSuppleModel>();

            listPtOtherDrugModel = _importantNoteRepository.GetOtherDrugList(ptId, sinDate);
            listPtOtcDrugModel = _importantNoteRepository.GetOtcDrugList(ptId, sinDate);
            listPtSuppleModel = _importantNoteRepository.GetSuppleList(ptId, sinDate);

            foreach (var ptOtherDrugModel in listPtOtherDrugModel)
            {
                if (!string.IsNullOrEmpty(ptOtherDrugModel.DrugName))
                {
                    headerInf.Append(ptOtherDrugModel.DrugName);
                    if (!string.IsNullOrEmpty(ptOtherDrugModel.Cmt))
                    {
                        headerInf.Append("／" + ptOtherDrugModel.Cmt);
                    }
                    headerInf.Append(Environment.NewLine);
                }
            }
            foreach (var ptOtcDrugModel in listPtOtcDrugModel)
            {
                if (!string.IsNullOrEmpty(ptOtcDrugModel.TradeName))
                {
                    headerInf.Append(ptOtcDrugModel.TradeName);
                    if (!string.IsNullOrEmpty(ptOtcDrugModel.Cmt))
                    {
                        headerInf.Append("／" + ptOtcDrugModel.Cmt);
                    }
                    headerInf.Append(Environment.NewLine);
                }
            }
            foreach (var suppleModel in listPtSuppleModel)
            {
                if (!string.IsNullOrEmpty(suppleModel.IndexWord))
                {
                    headerInf.Append(suppleModel.IndexWord);
                    if (!string.IsNullOrEmpty(suppleModel.Cmt))
                    {
                        headerInf.Append("／" + suppleModel.Cmt);
                    }
                    headerInf.Append(Environment.NewLine);
                }
            }
            string strHeaderInfo = headerInf.ToString().TrimEnd(Environment.NewLine.ToCharArray());
            summaryInfItem = new SummaryInfItem(strHeaderInfo, headerName, string.Empty, 0, 0, 0, grpItemCd, string.Empty);
        }

        private void GetCalculationInfo(int hpId, long ptId, int sinDate, SummaryInfItem summaryInfItem)
        {
            int grpItemCd = 5;
            string headerName = "◆算定情報";
            string headerInf = "";
            List<SanteiInfModel> listSanteiInfModels = _santeiInfRepository.GetCalculationInfo(hpId, ptId, sinDate);
            if (listSanteiInfModels.Count > 0)
            {
                listSanteiInfModels = listSanteiInfModels.Where(u => u.DayCount > u.AlertDays).ToList();
                foreach (var santeiInfomationModel in listSanteiInfModels)
                {
                    headerInf += santeiInfomationModel.ItemName?.Trim() + "(" + santeiInfomationModel.KisanType + " " + CIUtil.SDateToShowSDate(santeiInfomationModel.LastOdrDate) + "～　" + santeiInfomationModel.DayCountDisplay + ")" + Environment.NewLine;
                }
                headerInf = headerInf.TrimEnd(Environment.NewLine.ToCharArray());
                summaryInfItem = new SummaryInfItem(headerInf, headerName, string.Empty, 0, 0, 0, grpItemCd, string.Empty);
            }
        }

        private void GetPhoneNumber(int hpId, long ptId, SummaryInfItem summaryInfItem)
        {
            int grpItemCd = 12;
            string headerName = "◆電話番号";
            var ptInfModel = _patientInfRepository.GetPtInf(hpId, ptId);
            string headerInf = "";
            if (ptInfModel != null && !string.IsNullOrEmpty(ptInfModel.Tel1 + ptInfModel.Tel2))
            {
                headerInf = ptInfModel.Tel1 + Environment.NewLine + ptInfModel.Tel2;
            }
            summaryInfItem = new SummaryInfItem(headerInf, headerName, string.Empty, 0, 0, 0, grpItemCd, string.Empty);
        }

        private void GetLifeHistory(int hpId, long ptId, SummaryInfItem summaryInfItem)
        {
            var grpItemCd = 11;
            var headerName = "■生活歴";
            string headerInf = "";
            var seikaturekiInfModel = _specialNotePatientInfRepository.GetSeikaturekiInfList(ptId, hpId).FirstOrDefault();
            if (seikaturekiInfModel != null)
            {
                headerInf = seikaturekiInfModel.Text;
            }
            summaryInfItem = new SummaryInfItem(headerInf, headerName, string.Empty, 0, 0, 0, grpItemCd, string.Empty);
        }


    }
}
