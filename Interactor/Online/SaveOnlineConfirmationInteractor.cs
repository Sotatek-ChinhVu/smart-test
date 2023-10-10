using Domain.Models.Insurance;
using Domain.Models.Online;
using Helper.Common;
using Helper.Constants;
using Helper.Extension;
using System.Xml;
using System.Xml.Serialization;
using UseCase.Online.SaveOnlineConfirmation;
using UseCase.XmlDTD.OnlineConfirmHistoryData;
using UseCase.XmlDTD.QCBIDXmlMsgResponse;

namespace Interactor.Online
{
    public class SaveOnlineConfirmationInteractor : ISaveOnlineConfirmationInputPort
    {
        private readonly IOnlineRepository _onlineRepository;

        public SaveOnlineConfirmationInteractor(IOnlineRepository onlineRepository)
        {
            _onlineRepository = onlineRepository;
        }

        public SaveOnlineConfirmationOutputData Handle(SaveOnlineConfirmationInputData inputData)
        {
            try
            {
                if (inputData.QCBIDResponse == null ||
                    inputData.QCBIDResponse.MessageHeader == null ||
                    inputData.QCBIDResponse.MessageHeader.ReceptionNumber != inputData.QCBIDRequest.MessageBody.ReceptionNumber)
                {
                    return new SaveOnlineConfirmationOutputData(SaveOnlineConfirmationStatus.InvalidReceptionNumber);
                }

                string segmentOfResult = inputData.QCBIDResponse.MessageHeader.SegmentOfResult;

                ConvertToListOnlConfirmHistoryModel(inputData.UserId, inputData.QCBIDResponse);

                bool isUpdateRaiinInfSuccess = true;
                if (segmentOfResult == "1" || segmentOfResult == "2" || segmentOfResult == "9")
                {
                    var onlConfirmModel = new QualificationInfModel(inputData.RaiinNo.ToString(),
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
                                                                            onlConfirmModel.YoyakuDate,
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
                        isUpdateRaiinInfSuccess = _onlConfHanlder.UpdateRaiinInfByResResult(listResultModel);
                        if (isUpdateRaiinInfSuccess)
                        {
                            string statistical = String.Format("{0}件(正常{1}件 異常{2}件)", response.MessageHeader.NumberOfProcessingResult, response.MessageHeader.NumberOfNormalProcessing, response.MessageHeader.NumberOfError);
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                Log.WriteLogMsg(ModuleName, this, functionName, "Dispatcher.Invoke EmrDialogMessage");
                                new EmrDialogMessage(EmrMessageType.mFree00010,
                                                    "一括照会結果を受信しました。" + Environment.NewLine +
                                                    SegmentOfResultDisplay(response.MessageHeader.SegmentOfResult) + Environment.NewLine +
                                                    (string.IsNullOrEmpty(response.MessageHeader.ErrorMessage) ? string.Empty : (response.MessageHeader.ErrorMessage + Environment.NewLine)) +
                                                    statistical,
                                                    EmrMessageButtons.mbClose, 0).Send();
                            });
                        }
                    }

                    if (!isUpdateOnlineConfrimSuccess || !isUpdateRaiinInfSuccess)
                    {
                        new EmrDialogMessage(EmrMessageType.mEdit01030, "オンライン資格確認一括照会", new EmrMessageButtons[] { EmrMessageButtons.mbOK }, 1).Send();
                    }
                }
                //var result = _onlineRepository.SaveOnlineConfirmation(inputData.UserId, inputData.QualificationInf, inputData.ModelStatus);

                //return new SaveOnlineConfirmationOutputData(result ? SaveOnlineConfirmationStatus.Successed : SaveOnlineConfirmationStatus.Failed);
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

            _onlineRepository.InsertOnlineConfirmHistory(userId, listOnlineConfirmationHistoryModel);

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
    }
}
