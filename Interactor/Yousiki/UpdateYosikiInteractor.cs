using Domain.Constant;
using Domain.Models.Yousiki;
using Helper.Common;
using Helper.Extension;
using Infrastructure.Base;
using Infrastructure.Interfaces;
using UseCase.Yousiki.UpdateYosiki;

namespace Interactor.Yousiki
{
    public class UpdateYosikiInteractor : RepositoryBase, IUpdateYosikiInputPort
    {
        private readonly IYousikiRepository _yousikiRepository;

        public UpdateYosikiInteractor(IYousikiRepository yousikiRepository, ITenantProvider tenantProvider) : base(tenantProvider)
        {
            _yousikiRepository = yousikiRepository;
        }
        public UpdateYosikiOutputData Handle(UpdateYosikiInputData inputData)
        {
            try
            {
                if (!inputData.IsTemporarySave)
                {
                    var validate = ValidationData(inputData.HpId, inputData.Yousiki1InfDetails, inputData.Yousiki1Inf);
                    if (!validate.First().Key)
                    {
                        return new UpdateYosikiOutputData(UpdateYosikiStatus.Failed, validate.First().Value);
                    }
                }

                _yousikiRepository.UpdateYosiki(inputData.HpId, inputData.UserId, inputData.Yousiki1InfDetails, inputData.Yousiki1Inf, inputData.CategoryModels, inputData.IsTemporarySave);

                return new UpdateYosikiOutputData(UpdateYosikiStatus.Successed, "");
            }
            finally
            {
                _yousikiRepository.ReleaseResource();
            }
        }

        public Dictionary<bool, string> ValidationData(int hpId, List<Yousiki1InfDetailModel> yousiki1InfDetailModels, Yousiki1InfModel yousiki1InfModel, string valueDefault = "")
        {
            Dictionary<bool, string> result = new();
            var birthDayModel = GetYousiki1InfDetailModel(hpId, Yousiki1InfDetailConstant.CodeNo_Attributes, 0, 1, yousiki1InfDetailModels, yousiki1InfModel);

            if (birthDayModel.Value.AsInteger() <= 0)
            {
                string message = string.Format(ErrorMessage.MessageType_mInp00010, "共通 - 属性 - 生年月日");
                result.Add(false, message);

                return result;
            }

            if (GetYousiki1InfDetailModel(hpId, Yousiki1InfDetailConstant.CodeNo_Attributes, 0, 2, yousiki1InfDetailModels, yousiki1InfModel).Value.AsInteger() <= 0)
            {
                string message = string.Format(ErrorMessage.MessageType_mInp00010, "共通 - 属性 - 性別");
                result.Add(false, message);

                return result;
            }

            var homePostModel = GetYousiki1InfDetailModel(hpId, Yousiki1InfDetailConstant.CodeNo_Attributes, 0, 2, yousiki1InfDetailModels, yousiki1InfModel).Value;

            if (string.IsNullOrEmpty(homePostModel))
            {
                string message = string.Format(ErrorMessage.MessageType_mInp00010, "共通 - 属性 - 郵便番号");
                result.Add(false, message);

                return result;
            }

            var bodyHeight = GetYousiki1InfDetailModel(hpId, Yousiki1InfDetailConstant.CodeNo_HeightAndWeight, 0, 1, yousiki1InfDetailModels, yousiki1InfModel).Value ?? string.Empty;
            var bodyWeight = GetYousiki1InfDetailModel(hpId, Yousiki1InfDetailConstant.CodeNo_HeightAndWeight, 0, 2, yousiki1InfDetailModels, yousiki1InfModel).Value ?? string.Empty;

            if (!bodyHeight.Equals("000") && bodyHeight.AsInteger() <= 0)
            {
                string message = string.Format(ErrorMessage.MessageType_mInp00010, "共通 - 身長・体重 - 身長");
                result.Add(false, message);

                return result;
            }

            if (!bodyWeight.Equals("000") && bodyWeight.AsDouble() <= 0)
            {
                string message = string.Format(ErrorMessage.MessageType_mInp00010, "共通 - 身長・体重 - 体重");
                result.Add(false, message);

                return result;
            }

            var smokingTypeModel = GetYousiki1InfDetailModel(hpId, Yousiki1InfDetailConstant.CodeNo_SmokingHistory, 0, 1, yousiki1InfDetailModels, yousiki1InfModel, "0");
            var smokingNumberOfDayModel = GetYousiki1InfDetailModel(hpId, Yousiki1InfDetailConstant.CodeNo_SmokingHistory, 0, 2, yousiki1InfDetailModels, yousiki1InfModel);
            var smokingYearModel = GetYousiki1InfDetailModel(hpId, Yousiki1InfDetailConstant.CodeNo_SmokingHistory, 0, 3, yousiki1InfDetailModels, yousiki1InfModel);

            if (smokingTypeModel.Value.AsInteger() > 0)
            {
                if (string.IsNullOrEmpty(smokingNumberOfDayModel.Value) || !smokingNumberOfDayModel.Value.Equals("000") && smokingNumberOfDayModel.Value.AsInteger() <= 0)
                {
                    string message = string.Format(ErrorMessage.MessageType_mInp00010, "共通 - 喫煙歴 - 1日の喫煙本数");
                    result.Add(false, message);

                    return result;
                }

                if (string.IsNullOrEmpty(smokingNumberOfDayModel.Value) || !smokingYearModel.Value.Equals("000") && smokingYearModel.Value.AsInteger() <= 0)
                {
                    string message = string.Format(ErrorMessage.MessageType_mInp00010, "共通 - 喫煙歴 - 喫煙年数");
                    result.Add(false, message);

                    return result;
                }
            }

            int sinDate = CIUtil.GetLastDateOfMonth(yousiki1InfModel.SinYm * 100 + 1);
            int age = CIUtil.SDateToAge(birthDayModel.Value.AsInteger(), sinDate);
            var elderlyInfModel = GetYousiki1InfDetailModel(hpId, Yousiki1InfDetailConstant.CodeNo_NursingCareInf, 0, 1, yousiki1InfDetailModels, yousiki1InfModel);
            var careRequiedLevelModel = GetYousiki1InfDetailModel(hpId, Yousiki1InfDetailConstant.CodeNo_NursingCareInf, 0, 2, yousiki1InfDetailModels, yousiki1InfModel);

            if (age >= 65)
            {
                if (elderlyInfModel.Value.AsInteger() < 0)
                {
                    string message = string.Format(ErrorMessage.MessageType_mInp00010, "共通 - 介護情報 - 高齢者情報");
                    result.Add(false, message);

                    return result;
                }

                if (careRequiedLevelModel.Value.AsInteger() < 0)
                {
                    string message = string.Format(ErrorMessage.MessageType_mInp00010, "共通 - 介護情報 - 要介護度");
                    result.Add(false, message);

                    return result;
                }
            }

            return result;
        }

        private Yousiki1InfDetailModel GetYousiki1InfDetailModel(int hpId, string codeNo, int rowNo, int payLoad, List<Yousiki1InfDetailModel> yousiki1InfDetailModels, Yousiki1InfModel yousiki1InfModel, string valueDefault = "")
        {
            Yousiki1InfDetailModel detail;
            if (yousiki1InfDetailModels == null || yousiki1InfDetailModels.Count == 0)
            {
                detail = CreateYousiki1InfDetailModel(hpId, codeNo, rowNo, payLoad, yousiki1InfModel);
            }
            else
            {
                detail = yousiki1InfDetailModels.FirstOrDefault(x => x.CodeNo == Yousiki1InfDetailConstant.CodeNo_Attributes) ?? new();
                if (detail == null || detail == new Yousiki1InfDetailModel())
                {
                    detail = CreateYousiki1InfDetailModel(hpId, codeNo, rowNo, payLoad, yousiki1InfModel);
                }
            }
            yousiki1InfDetailModels.Add(detail);
            return detail;
        }

        private Yousiki1InfDetailModel CreateYousiki1InfDetailModel(int hpId, string codeNo, int rowNo, int payLoad, Yousiki1InfModel yousiki1InfDetailModel)
        {
            var yousiki1Inf = NoTrackingDataContext.Yousiki1InfDetails.FirstOrDefault(x => x.HpId == hpId && x.PtId == yousiki1InfDetailModel.PtId && x.SinYm == yousiki1InfDetailModel.SinYm && x.DataType == yousiki1InfDetailModel.DataType && x.SeqNo == yousiki1InfDetailModel.SeqNo && x.CodeNo == codeNo && x.RowNo == rowNo && x.Payload == payLoad);
            if (yousiki1Inf == null)
            {
                return new();
            }

            var detail = new Yousiki1InfDetailModel(
                        yousiki1Inf.PtId,
                        yousiki1Inf.SinYm,
                        yousiki1Inf.DataType,
                        yousiki1Inf.SeqNo,
                        yousiki1Inf.CodeNo ?? string.Empty,
                        yousiki1Inf.RowNo,
                        yousiki1Inf.Payload,
                        yousiki1Inf.Value ?? string.Empty);

            return detail;
        }
    }
}
