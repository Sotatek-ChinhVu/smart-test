using Domain.Models.PatientInfor;
using Domain.Models.SpecialNote.PatientInfo;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using UseCase.MedicalExamination.CheckedAfter327Screen;
using UseCase.MedicalExamination.SummaryInf;
using SpecialNotePatienInfDomain = Domain.Models.SpecialNote.PatientInfo;

namespace Interactor.MedicalExamination
{
    public class SummaryInfInteractor
    {
        public bool IsGetDataFromSpecialNote = false;
        private readonly IPatientInforRepository _patientInfRepository;
        private readonly SpecialNotePatienInfDomain.IPatientInfoRepository _specialNotePatientInfRepository;
        public SummaryInfInteractor(IPatientInforRepository patientInfRepository, SpecialNotePatienInfDomain.IPatientInfoRepository specialNotePatientInfRepository)
        {
            _patientInfRepository = patientInfRepository;
            _specialNotePatientInfRepository = specialNotePatientInfRepository;
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


        private void GetPhysicalInfo(int hpId, long ptId, int sinDate, SummaryInfItem ptHeaderInfoModel)
        {
            int grpItemCd = 1;
            string headerName = "■身体情報";
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
                    ptHeaderInfoModel.HeaderInfo += (string.IsNullOrEmpty(kensaName) ? string.Empty : kensaName + ":" + Space) + kensaDetailModel.ResultVal + kensaDetailModel.Unit + Space + (string.IsNullOrEmpty(sSate) ? string.Empty : "(" + sSate + ")") + Space + "/";
                }
            }
            string headerInfo = ptHeaderInfoModel.HeaderInfo?.TrimEnd('/') ?? string.Empty;
        }


        private void GetPhoneNumber(int hpId, long ptId, SummaryInfItem ptHeaderInfoModel)
        {
            int grpItemCd = 12;
            string headerName = "◆電話番号";
            var ptInfModel = _patientInfRepository.GetPtInf(hpId, ptId);
            string headerInf = "";
            if (ptInfModel != null && !string.IsNullOrEmpty(ptInfModel.Tel1 + ptInfModel.Tel2))
            {
                headerInf = ptInfModel.Tel1 + Environment.NewLine + ptInfModel.Tel2;
            }
            ptHeaderInfoModel = new SummaryInfItem(headerInf, headerName, string.Empty, 0, 0, 0, grpItemCd, string.Empty);
        }

        private void GetLifeHistory(int hpId, long ptId, SummaryInfItem ptHeaderInfoModel)
        {
            var grpItemCd = 11;
            var headerName = "■生活歴";
            string headerInf = "";
            var seikaturekiInfModel = _specialNotePatientInfRepository.GetSeikaturekiInfList(ptId, hpId).FirstOrDefault();
            if (seikaturekiInfModel != null)
            {
                headerInf = seikaturekiInfModel.Text;
            }
            ptHeaderInfoModel = new SummaryInfItem(headerInf, headerName, string.Empty, 0, 0, 0, grpItemCd, string.Empty);
        }


    }
}
