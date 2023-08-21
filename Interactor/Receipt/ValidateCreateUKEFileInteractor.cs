using Domain.Models.Receipt;
using Domain.Models.Receipt.ReceiptCreation;
using Helper.Common;
using UseCase.Receipt.CreateUKEFile;
using UseCase.Receipt.ValidateCreateUKEFile;

namespace Interactor.Receipt
{
    public class ValidateCreateUKEFileInteractor : IValidateCreateUKEFileInputPort
    {
        private readonly IReceiptRepository _receiptRepository;

        public ValidateCreateUKEFileInteractor(IReceiptRepository receiptRepository)
        {
            _receiptRepository = receiptRepository;
        }

        public ValidateCreateUKEFileOutputData Handle(ValidateCreateUKEFileInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0)
                    return new ValidateCreateUKEFileOutputData(ValidateCreateUKEFileStatus.InvalidHpId, string.Empty);

                if (inputData.SeikyuYm <= 0)
                    return new ValidateCreateUKEFileOutputData(ValidateCreateUKEFileStatus.InvaliSeikyuYm, string.Empty);

                if (inputData.ModeType == ModeTypeCreateUKE.Rosai)
                {
                    string errorInfs = ValidateData(inputData.HpId, inputData.SeikyuYm);
                    if (!string.IsNullOrEmpty(errorInfs))
                    {
                        return new ValidateCreateUKEFileOutputData(ValidateCreateUKEFileStatus.ErrorValidateRosai, errorInfs);
                    }
                }

                if (inputData.ModeType == ModeTypeCreateUKE.Aftercare)
                {
                    string errorInfs = ValidateAftercare(inputData.HpId, inputData.SeikyuYm);
                    if (!string.IsNullOrEmpty(errorInfs))
                    {
                        return new ValidateCreateUKEFileOutputData(ValidateCreateUKEFileStatus.ErrorValidateAftercare, errorInfs);
                    }
                }

                return new ValidateCreateUKEFileOutputData(ValidateCreateUKEFileStatus.Successful, string.Empty);
            }
            finally
            {
                _receiptRepository.ReleaseResource();
            }
        }

        private string ValidateData(int hpId, int seikyuYm)
        {
            string errorSyobyo = string.Empty;
            string errorSyobyoKeika = string.Empty;
            string errorRousaiSaigai = string.Empty;

            List<ReceInfValidateModel> receInfModels = _receiptRepository.GetReceValidateReceiptCreation(hpId, new List<long>(), seikyuYm);
            foreach (var receInfItem in receInfModels)
            {
                if (receInfItem.IsTester == 1) continue;
                // check using Rosai Receden 
                if ((receInfItem.HokenKbn == 11 || receInfItem.HokenKbn == 12) &&
                    receInfItem.IsPaperRece == 0)
                {
                    // check error Rousai Saigai
                    if (receInfItem.RousaiSaigaiKbn != 1 &&
                        receInfItem.RousaiSaigaiKbn != 2)
                    {
                        errorRousaiSaigai += string.Format("    {0} ID:{1} [保険:{2}]", CIUtil.SMonthToShowSMonth(seikyuYm), receInfItem.PtNum, receInfItem.HokenId) + Environment.NewLine;
                    }
                    // check error Syobyo
                    if (receInfItem.RousaiSyobyoDate <= 0)
                    {
                        errorSyobyo += string.Format("    {0} ID:{1} [保険:{2}]", CIUtil.SMonthToShowSMonth(seikyuYm), receInfItem.PtNum, receInfItem.HokenId) + Environment.NewLine;
                    }
                    // check error SyobyoKeika
                    if (!_receiptRepository.ExistSyobyoKeikaData(hpId, receInfItem.PtId, receInfItem.SinYm, receInfItem.HokenId))
                    {
                        errorSyobyoKeika += string.Format("    {0} ID:{1} [保険:{2}]", CIUtil.SMonthToShowSMonth(seikyuYm), receInfItem.PtNum, receInfItem.HokenId) + Environment.NewLine;
                    }
                }
            }
            if (!string.IsNullOrEmpty(errorRousaiSaigai))
            {
                errorRousaiSaigai = errorRousaiSaigai.Insert(0, "■災害区分が設定されていません。" + Environment.NewLine);
            }
            if (!string.IsNullOrEmpty(errorSyobyo))
            {
                errorSyobyo = Environment.NewLine + errorSyobyo.Insert(0, "■傷病開始年月日が設定されていません。" + Environment.NewLine);
            }
            if (!string.IsNullOrEmpty(errorSyobyoKeika))
            {
                errorSyobyoKeika = Environment.NewLine + errorSyobyoKeika.Insert(0, "■傷病の経過が設定されていません。" + Environment.NewLine);
            }
            return errorRousaiSaigai + errorSyobyo + errorSyobyoKeika;
        }

        private string ValidateAftercare(int hpId, int seikyuYm)
        {
            string errorSyobyoKeika = string.Empty;
            List<ReceInfValidateModel> receInfModels = _receiptRepository.GetReceValidateReceiptCreation(hpId, new List<long>(), seikyuYm).Where(item => item.HokenKbn == 13).ToList();
            foreach (var receInfItem in receInfModels)
            {
                if (receInfItem.IsTester == 1) continue;
                // check using Aftercare Receden 
                if (receInfItem.HokenKbn == 13 && receInfItem.IsPaperRece == 0)
                {
                    // Check error SyobyoKeika 
                    if (!_receiptRepository.ExistSyobyoKeikaData(hpId, receInfItem.PtId, receInfItem.SinYm, receInfItem.HokenId))
                    {
                        errorSyobyoKeika += string.Format("    {0} ID:{1} [保険:{2}]", CIUtil.SMonthToShowSMonth(seikyuYm), receInfItem.PtNum, receInfItem.HokenId) + Environment.NewLine;
                    }
                }
            }
            if (!string.IsNullOrEmpty(errorSyobyoKeika))
            {
                errorSyobyoKeika = errorSyobyoKeika.Insert(0, "■傷病の経過が設定されていません。" + Environment.NewLine);
            }
            return errorSyobyoKeika;
        }
    }
}
