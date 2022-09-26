using Domain.Constant;
using Domain.Models.Insurance;
using Domain.Models.ReceptionInsurance;
using Helper.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UseCase.Insurance.ValidKohi;

namespace Interactor.Insurance
{
    public class ValidateKohiInteractor : IValidKohiInputPort
    {
        private readonly IReceptionInsuranceRepository _insuranceResponsitory;
        public ValidateKohiInteractor(IReceptionInsuranceRepository insuranceResponsitory)
        {
            _insuranceResponsitory = insuranceResponsitory;
        }

        public ValidKohiOutputData Handle(ValidKohiInputData inputData)
        {
            if (inputData.SinDate < 0)
            {
                return new ValidKohiOutputData(false, string.Empty, ValidKohiStatus.InvalidSindate);
            }


            return new ValidKohiOutputData(true, string.Empty, ValidKohiStatus.ValidSuccess);
        }

        private string IsValidKohi(KohiInfModel kohiModel, int numberKohi, int sinDate, bool isAddNew)
        {
            var message = "";
            var numberMessage = "";

            if (numberKohi == 1)
            {
                numberMessage = "１";
            }
            else if (numberKohi == 2)
            {
                numberMessage = "２";
            }
            else if (numberKohi == 3)
            {
                numberMessage = "３";
            }
            else
            {
                numberMessage = "４";
            }

            if (kohiModel == null)
            {
                return message;
            }


            if (kohiModel.HokenMstModel == null)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "保険番号" };
                message = String.Format(ErrorMessage.MessageType_mNG01010, paramsMessage);
                return message;
            }
            if (string.IsNullOrEmpty(kohiModel.FutansyaNo.Trim())
                && kohiModel.HokenMstModel.IsFutansyaNoCheck == 1)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "負担者番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                return message;
            }
            else if (string.IsNullOrEmpty(kohiModel.JyukyusyaNo.Trim())
                && kohiModel.HokenMstModel.IsJyukyusyaNoCheck == 1)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "受給者番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                return message;
            }
            else if (string.IsNullOrEmpty(kohiModel.TokusyuNo.Trim())
                && kohiModel.HokenMstModel.IsTokusyuNoCheck == 1)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "特殊番号" };
                message = String.Format(ErrorMessage.MessageType_mInp00010, paramsMessage);
                return message;
            }
            if (!string.IsNullOrEmpty(kohiModel.FutansyaNo) && Int32.Parse(kohiModel.FutansyaNo) == 0)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "負担者番号は 0〜9の数字で入力してください。" };
                message = String.Format(ErrorMessage.MessageType_mFree00030, paramsMessage);
                return message;
            }
            int KohiYukoFromDate = kohiModel.StartDate;
            int KohiYukoToDate = kohiModel.EndDate;
            if (KohiYukoFromDate != 0 && KohiYukoToDate != 0 && KohiYukoFromDate > KohiYukoToDate)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + "有効終了日", "公費" + numberMessage + "有効開始日以降" };
                message = String.Format(ErrorMessage.MessageType_mInp00041, paramsMessage);
                return message;
            }

            var checkMessageIsValidConfirmDateKohi = IsValidConfirmDateKohi(kohiModel, numberMessage, sinDate, isAddNew);
            if (!String.IsNullOrEmpty(checkMessageIsValidConfirmDateKohi))
            {
                return checkMessageIsValidConfirmDateKohi;
            }

            var checkMessageIsValidMasterDateKohi = IsValidMasterDateKohi(kohiModel, numberMessage, sinDate);
            if (!String.IsNullOrEmpty(checkMessageIsValidMasterDateKohi))
            {
                return checkMessageIsValidMasterDateKohi;
            }

            return message;
        }

        private string IsValidConfirmDateKohi(KohiInfModel kohiModel, string numberMessage, int sinDate, bool isAddNew)
        {
            var message = "";
            int kouhi1ConfirmDate = kohiModel.ConfirmDate;
            int confirmKohi1YM = Int32.Parse(CIUtil.Copy(kouhi1ConfirmDate.ToString(), 1, 6));
            if (kouhi1ConfirmDate == 0
                || sinDate != confirmKohi1YM)
            {
                // 公１・保険証確認日ﾁｪｯｸ(有効保険・新規保険の場合のみ)
                if (isAddNew)
                {
                    return message;
                }
                else
                {
                    var paramsMessage = new string[] { "公費" + numberMessage, "受給者証等" };
                    message = String.Format(ErrorMessage.MessageType_mChk00030, paramsMessage);
                    return message;
                }
            }
            return message;
        }

        private string IsValidMasterDateKohi(KohiInfModel kohiModel, string numberMessage, int sinDate)
        {
            var message = "";
            if (kohiModel.HokenMstModel == null) return message;
            if (kohiModel.HokenMstModel.StartDate > sinDate)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + " '" + kohiModel.HokenMstModel.DisplayTextMaster + "' の適用期間外です。" + "\n\r" + " ("
                            + CIUtil.SDateToShowSDate(kohiModel.HokenMstModel.StartDate) + "～)", "保険番号" };
                message = String.Format(ErrorMessage.MessageType_mChk00080, paramsMessage);
                return message;
            }
            if (kohiModel.HokenMstModel.EndDate < sinDate)
            {
                var paramsMessage = new string[] { "公費" + numberMessage + " '" + kohiModel.HokenMstModel.DisplayTextMaster + "' の適用期間外です。" + "\n\r" + " ("
                            + CIUtil.SDateToShowSDate(kohiModel.HokenMstModel.EndDate) + "～)", "保険番号" };
                message = String.Format(ErrorMessage.MessageType_mChk00080, paramsMessage);
                return message;
            }
            return message;
        }
    }
}
