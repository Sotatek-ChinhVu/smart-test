using Domain.Models.Insurance;
using Domain.Models.Online;
using Domain.Models.Reception;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using Infrastructure.Interfaces;
using Infrastructure.Logger;
using System.Xml;
using System.Xml.Serialization;
using UseCase.Online.SaveOnlineConfirmation;
using UseCase.XmlDTD.OnlineConfirmHistoryData;
using UseCase.XmlDTD.QCBIDXmlMsgResponse;

namespace Interactor.Online
{
    public class UpdateOnlineConfirmationInteractor : IUpdateOnlineConfirmationInputPort
    {
        private readonly IOnlineRepository _onlineRepository;
        private readonly ILoggingHandler _loggingHandler;
        private readonly ITenantProvider _tenantProvider;

        public UpdateOnlineConfirmationInteractor(ITenantProvider tenantProvider, IOnlineRepository onlineRepository)
        {
            _onlineRepository = onlineRepository;
            _tenantProvider = tenantProvider;
            _loggingHandler = new LoggingHandler(_tenantProvider.CreateNewTrackingAdminDbContextOption(), tenantProvider);
        }

        public UpdateOnlineConfirmationOutputData Handle(UpdateOnlineConfirmationInputData inputData)
        {
            try
            {
                var message = string.Empty;

                if (inputData.QCBIDResponse == null ||
                    inputData.QCBIDResponse.MessageHeader == null ||
                    inputData.QCBIDResponse.MessageHeader.ReceptionNumber != inputData.ReceptionNumber)
                {
                    return new UpdateOnlineConfirmationOutputData(new(), UpdateOnlineConfirmationStatus.InvalidReceptionNumber, message);
                }

                string segmentOfResult = inputData.QCBIDResponse.MessageHeader.SegmentOfResult;

                ConvertToListOnlConfirmHistoryModel(inputData.UserId, inputData.QCBIDResponse);

                var updateRaiinInf = (true, new List<ReceptionRowModel>());
                if (segmentOfResult == "1" || segmentOfResult == "2" || segmentOfResult == "9")
                {
                    var onlConfirmModel = new QualificationInfModel(inputData.ReceptionNumber,
                                                                    inputData.QCBIDResponse.MessageHeader.SegmentOfResult,
                                                                    inputData.QCBIDResponse.MessageHeader.ErrorMessage
                                                                    );
                    bool isUpdateOnlineConfrimSuccess = _onlineRepository.SaveOnlineConfirmation(inputData.UserId, onlConfirmModel, ModelStatus.Modified);

                    if (segmentOfResult != "9" && inputData.QCBIDResponse.MessageBody != null)
                    {
                        var listDataInf = inputData.QCBIDResponse.MessageBody.QCBIDXmlMsgResponseInfo?.ToList();
                        var listResultModel = new List<ConfirmResultModel>();
                        if (listDataInf?.Count > 0)
                        {
                            listResultModel = listDataInf.Select(u => new ConfirmResultModel(
                                                                            GetPtIdFromArbitraryIdentifier(u.QualificationConfirmSearchInfo.ArbitraryIdentifier),
                                                                            inputData.YokakuDate,
                                                                            u.ResultOfQualificationConfirmation?.Birthdate ?? string.Empty,
                                                                            u.ResultOfQualificationConfirmation?.Sex1 ?? string.Empty,
                                                                            u.ResultOfQualificationConfirmation?.Sex2 ?? string.Empty,
                                                                            u.ResultOfQualificationConfirmation?.Address ?? string.Empty,
                                                                            u.ResultOfQualificationConfirmation?.InsuredName ?? string.Empty,
                                                                            u.ResultOfQualificationConfirmation?.PostNumber ?? string.Empty,
                                                                            u.ResultOfQualificationConfirmation?.NameKana ?? string.Empty,
                                                                            u.ResultOfQualificationConfirmation?.Name ?? string.Empty,
                                                                            u.ResultOfQualificationConfirmation?.InsurerNumber ?? string.Empty,
                                                                            u.ResultOfQualificationConfirmation?.InsuredCardSymbol ?? string.Empty,
                                                                            u.ResultOfQualificationConfirmation?.InsuredIdentificationNumber ?? string.Empty,
                                                                            u.ResultOfQualificationConfirmation?.InsuredBranchNumber ?? string.Empty,
                                                                            u.ResultOfQualificationConfirmation?.PersonalFamilyClassification ?? string.Empty,
                                                                            u.ResultOfQualificationConfirmation?.InsuredCertificateIssuanceDate ?? string.Empty,
                                                                            u.ResultOfQualificationConfirmation?.InsuredCardValidDate ?? string.Empty,
                                                                            u.ResultOfQualificationConfirmation?.InsuredCardExpirationDate ?? string.Empty,
                                                                            u.ResultOfQualificationConfirmation?.LimitApplicationCertificateChanged ?? string.Empty,
                                                                            u.ProcessingResultMessage,
                                                                            string.Empty,
                                                                            u.ProcessingResultStatus.AsInteger(),
                                                                            u.ReferenceNumber.AsLong(),
                                                                            new ConfirmDateModel(
                                                                                        GetPtIdFromArbitraryIdentifier(u.QualificationConfirmSearchInfo.ArbitraryIdentifier),
                                                                                        1,
                                                                                        u.QualificationConfirmSearchInfo.ArbitraryIdentifier?.Substring(u.QualificationConfirmSearchInfo.ArbitraryIdentifier.LastIndexOf("_") + 1).AsInteger() ?? 0,
                                                                                        CIUtil.IntToDate(inputData.QCBIDResponse.MessageHeader.QualificationConfirmationDate.AsInteger()),
                                                                                        inputData.UserId,
                                                                                        "オンライン資格確認一括照会"
                                                                                )

                                )).ToList();
                        }
                        updateRaiinInf = _onlineRepository.UpdateRaiinInfByResResult(inputData.HpId, inputData.UserId, listResultModel);

                        if (updateRaiinInf.Item1)
                        {
                            string statistical = String.Format("{0}件(正常{1}件 異常{2}件)", inputData.QCBIDResponse.MessageHeader.NumberOfProcessingResult, inputData.QCBIDResponse.MessageHeader.NumberOfNormalProcessing, inputData.QCBIDResponse.MessageHeader.NumberOfError);
                            message = String.Format("一括照会結果を受信しました。" + Environment.NewLine +
                                                SegmentOfResultDisplay(inputData.QCBIDResponse.MessageHeader.SegmentOfResult) + Environment.NewLine +
                                                (string.IsNullOrEmpty(inputData.QCBIDResponse.MessageHeader.ErrorMessage) ? string.Empty : (inputData.QCBIDResponse.MessageHeader.ErrorMessage + Environment.NewLine)) +
                                                statistical);
                            return new UpdateOnlineConfirmationOutputData(updateRaiinInf.Item2, UpdateOnlineConfirmationStatus.Successed, message);
                        }
                    }

                    if (!isUpdateOnlineConfrimSuccess || !updateRaiinInf.Item1)
                    {
                        message = "オンライン資格確認一括照会";
                        return new UpdateOnlineConfirmationOutputData(new(), UpdateOnlineConfirmationStatus.Failed, message);
                    }
                }

                return new UpdateOnlineConfirmationOutputData(updateRaiinInf.Item2, UpdateOnlineConfirmationStatus.Successed, message);
            }
            catch (Exception e)
            {
                _loggingHandler.WriteLogExceptionAsync(e);
                throw;
            }
            finally
            {
                _onlineRepository.ReleaseResource();
            }
        }

        private void ConvertToListOnlConfirmHistoryModel(int userId, QCBIDXmlMsgResponse responseFile)
        {
            var listOnlineConfirmationHistoryModel = new List<OnlineConfirmationHistoryModel>();

            // Message Header Convert
            var messageHeaderRes = responseFile.MessageHeader;
            var msgResponseInfoHeader = new ConfirmResultMessageHeader()
            {
                ProcessExecutionTime = messageHeaderRes.ProcessExecutionTime,
                QualificationConfirmationDate = messageHeaderRes.QualificationConfirmationDate,
                MedicalInstitutionCode = messageHeaderRes.MedicalInstitutionCode,
                ArbitraryFileIdentifier = messageHeaderRes.ArbitraryFileIdentifier,
                SegmentOfResult = messageHeaderRes.SegmentOfResult,
                ErrorCode = messageHeaderRes.ErrorCode,
                ErrorMessage = messageHeaderRes.ErrorMessage,
                CharacterCodeIdentifier = messageHeaderRes.CharacterCodeIdentifier,
                NumberOfError = messageHeaderRes.NumberOfError,
                NumberOfNormalProcessing = messageHeaderRes.NumberOfNormalProcessing,
                NumberOfProcessingResult = messageHeaderRes.NumberOfProcessingResult,
                ReceptionNumber = messageHeaderRes.ReceptionNumber,
            };
            var confrimResultRes = new ConfirmResultResponse
            {
                MessageHeader = msgResponseInfoHeader
            };
            // Message Body Convert
            var messageBodyRes = responseFile.MessageBody;
            ConfirmResultMessageBody msgResponseInfoBody;
            if (messageBodyRes == null) return;
            foreach (var qCBIDMsgResponseInfo in messageBodyRes.QCBIDXmlMsgResponseInfo)
            {
                // その他
                msgResponseInfoBody = new ConfirmResultMessageBody()
                {
                    ProcessingResultStatus = qCBIDMsgResponseInfo.ProcessingResultStatus,
                    ProcessingResultCode = qCBIDMsgResponseInfo.ProcessingResultCode,
                    ProcessingResultMessage = qCBIDMsgResponseInfo.ProcessingResultMessage,
                    QualificationValidity = qCBIDMsgResponseInfo.QualificationValidity,
                };

                // 資格確認照会用情報
                msgResponseInfoBody.QualificationConfirmSearchInfo = qCBIDMsgResponseInfo.QualificationConfirmSearchInfo == null ? null : new ConfirmResultQualificationConfirmSearchInfo()
                {
                    InsurerNumber = qCBIDMsgResponseInfo.QualificationConfirmSearchInfo.InsurerNumber,
                    InsuredCardSymbol = qCBIDMsgResponseInfo.QualificationConfirmSearchInfo.InsuredCardSymbol,
                    InsuredIdentificationNumber = qCBIDMsgResponseInfo.QualificationConfirmSearchInfo.InsuredIdentificationNumber,
                    InsuredBranchNumber = qCBIDMsgResponseInfo.QualificationConfirmSearchInfo.InsuredBranchNumber,
                    Birthdate = qCBIDMsgResponseInfo.QualificationConfirmSearchInfo.Birthdate,
                    ArbitraryIdentifier = qCBIDMsgResponseInfo.QualificationConfirmSearchInfo.ArbitraryIdentifier,
                };

                // 資格確認結果リスト
                var resultOfQualificationConfirmationRes = qCBIDMsgResponseInfo.ResultOfQualificationConfirmation;
                if (resultOfQualificationConfirmationRes == null) continue;
                msgResponseInfoBody.ResultList = new ConfirmResultResultList()
                {
                    ResultOfQualificationConfirmation = new ConfirmResultResultOfQualificationConfirmation()
                    {
                        InsuredCardClassification = resultOfQualificationConfirmationRes.InsuredCardClassification,
                        InsurerNumber = resultOfQualificationConfirmationRes.InsurerNumber,
                        InsuredCardSymbol = resultOfQualificationConfirmationRes.InsuredCardSymbol,
                        InsuredIdentificationNumber = resultOfQualificationConfirmationRes.InsuredIdentificationNumber,
                        InsuredBranchNumber = resultOfQualificationConfirmationRes.InsuredBranchNumber,
                        PersonalFamilyClassification = resultOfQualificationConfirmationRes.PersonalFamilyClassification,
                        InsuredName = resultOfQualificationConfirmationRes.InsuredName,
                        Name = resultOfQualificationConfirmationRes.Name,
                        NameOfOther = resultOfQualificationConfirmationRes.NameOfOther,
                        NameKana = resultOfQualificationConfirmationRes.NameKana,
                        NameOfOtherKana = resultOfQualificationConfirmationRes.NameOfOtherKana,
                        Birthdate = resultOfQualificationConfirmationRes.Birthdate,
                        Sex1 = resultOfQualificationConfirmationRes.Sex1,
                        Sex2 = resultOfQualificationConfirmationRes.Sex2,
                        Address = resultOfQualificationConfirmationRes.Address,
                        PostNumber = resultOfQualificationConfirmationRes.PostNumber,
                        InsuredCertificateIssuanceDate = resultOfQualificationConfirmationRes.InsuredCertificateIssuanceDate,
                        InsuredCardValidDate = resultOfQualificationConfirmationRes.InsuredCardValidDate,
                        InsuredCardExpirationDate = resultOfQualificationConfirmationRes.InsuredCardExpirationDate,
                        InsuredPartialContributionRatio = resultOfQualificationConfirmationRes.InsuredPartialContributionRatio,
                        PreschoolClassification = resultOfQualificationConfirmationRes.PreschoolClassification,
                        ReasonOfLoss = resultOfQualificationConfirmationRes.ReasonOfLoss,
                        InsurerName = resultOfQualificationConfirmationRes.InsurerName,
                        ElderlyRecipientCertificateInfo = resultOfQualificationConfirmationRes.ElderlyRecipientCertificateInfo == null ? null : new ConfirmResultElderlyRecipientCertificateInfo()
                        {
                            ElderlyRecipientCertificateDate = resultOfQualificationConfirmationRes.ElderlyRecipientCertificateInfo?.ElderlyRecipientCertificateDate,
                            ElderlyRecipientContributionRatio = resultOfQualificationConfirmationRes.ElderlyRecipientCertificateInfo.ElderlyRecipientContributionRatio,
                            ElderlyRecipientValidStartDate = resultOfQualificationConfirmationRes.ElderlyRecipientCertificateInfo.ElderlyRecipientValidStartDate,
                            ElderlyRecipientValidEndDate = resultOfQualificationConfirmationRes.ElderlyRecipientCertificateInfo.ElderlyRecipientValidEndDate,
                        },
                        LimitApplicationCertificateChanged = resultOfQualificationConfirmationRes.LimitApplicationCertificateChanged,
                        ReferenceNumber = qCBIDMsgResponseInfo.ReferenceNumber,
                    }
                };

                confrimResultRes.MessageBody = msgResponseInfoBody;

                // リストに追加
                listOnlineConfirmationHistoryModel.Add
                (
                    new OnlineConfirmationHistoryModel(0,
                                                       GetPtIdFromArbitraryIdentifier(qCBIDMsgResponseInfo.QualificationConfirmSearchInfo?.ArbitraryIdentifier ?? string.Empty),
                                                       CIUtil.GetJapanDateTimeNow(),
                                                       2,
                                                       string.Empty,
                                                       WriteXmlData(confrimResultRes),
                                                       0,
                                                       0
                    )
                );
            }

            _onlineRepository.InsertListOnlConfirmHistory(userId, listOnlineConfirmationHistoryModel);

        }

        private long GetPtIdFromArbitraryIdentifier(string arbitraryIdentifier)
        {
            if (string.IsNullOrEmpty(arbitraryIdentifier) || !arbitraryIdentifier.Contains("_"))
            {
                return 0;
            }
            return arbitraryIdentifier.Substring(0, arbitraryIdentifier.IndexOf("_")).AsLong();
        }

        private string WriteXmlData<T>(T model)
        {
            var xmlserializer = new XmlSerializer(typeof(T));
            var stringWriter = new Utf8StringWriter();
            XmlSerializerNamespaces serializerNamespaces = new XmlSerializerNamespaces();
            serializerNamespaces.Add(string.Empty, string.Empty);
            using (var writer = XmlWriter.Create(stringWriter))
            {
                xmlserializer.Serialize(writer, model, serializerNamespaces);
                return stringWriter.ToString();
            }
        }

        public string SegmentOfResultDisplay(string segmentOfResult)
        {
            if (string.IsNullOrEmpty(segmentOfResult) || segmentOfResult == "0")
            {
                return "照会中";
            }
            switch (segmentOfResult)
            {
                case "1":
                    return "正常終了";
                case "2":
                    return "処理中";
                case "9":
                    return "異常終了";
            }
            return string.Empty;
        }
    }
}
