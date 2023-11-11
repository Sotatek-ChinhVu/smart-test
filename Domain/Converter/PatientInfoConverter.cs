using Domain.Models.HokenMst;
using Domain.Models.Insurance;
using Domain.Models.Online;
using Domain.Models.Online.QualificationConfirmation;
using Domain.Models.PatientInfor;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;

namespace Domain.Converter
{
    public class PatientInfoConverter
    {
        public static List<HokenConfirmationModel> GetHokenConfirmationModels(HokenInfModel HokenInf, ResultOfQualificationConfirmation resultOfQualification, int birthDay, int sinDate)
        {
            int age = CIUtil.SDateToAge(birthDay, CIUtil.DateTimeToInt(DateTime.Now));
            var list = new List<HokenConfirmationModel>
            {
                new HokenConfirmationModel(HokenConfOnlQuaConst.HOKENSYA_NO, HokenInf.HokensyaNo, resultOfQualification.InsurerNumber?.AsString().Trim() ?? string.Empty, sinDate: sinDate),
                new HokenConfirmationModel(HokenConfOnlQuaConst.KIGO, HokenInf.Kigo, resultOfQualification.InsuredCardSymbol, sinDate: sinDate),
                new HokenConfirmationModel(HokenConfOnlQuaConst.BANGO, HokenInf.Bango, resultOfQualification.InsuredIdentificationNumber, sinDate: sinDate),
                new HokenConfirmationModel(HokenConfOnlQuaConst.EDANO, HokenInf.EdaNo, resultOfQualification.InsuredBranchNumber, sinDate: sinDate),
                new HokenConfirmationModel(HokenConfOnlQuaConst.HONKE, HokenInf.HonkeKbn.AsString(), resultOfQualification.PersonalFamilyClassification, sinDate: sinDate),
                new HokenConfirmationModel(HokenConfOnlQuaConst.KOFU_DATE, HokenInf.KofuDate.AsString(), resultOfQualification.InsuredCertificateIssuanceDate, sinDate: sinDate),
                new HokenConfirmationModel(HokenConfOnlQuaConst.START_DATE, HokenInf.StartDate.AsString(), resultOfQualification.InsuredCardValidDate, sinDate: sinDate),
                new HokenConfirmationModel(HokenConfOnlQuaConst.END_DATE, HokenInf.EndDate.AsString(), resultOfQualification.InsuredCardExpirationDate, sinDate: sinDate),
                new HokenConfirmationModel(HokenConfOnlQuaConst.KOGAKU_KBN
                                        , HokenInf.KogakuKbn.AsString()
                                        , resultOfQualification.LimitApplicationCertificateRelatedInfo?.LimitApplicationCertificateClassificationFlag
                                        , age, sinDate: sinDate, onlineKogakuHelper: new OnlineKogakuHelper(resultOfQualification, age)),
                 new HokenConfirmationModel(HokenConfOnlQuaConst.CREDENTIAL
                                        , (HokenInf.HokenNo == 68 && HokenInf.HokenEdaNo == 0) ? "該当" : string.Empty
                                        , (resultOfQualification.InsuredCardClassification.AsInteger() == 5) ?  "該当" : string.Empty, sinDate: sinDate)
            };
            return list;
        }
    }
}
