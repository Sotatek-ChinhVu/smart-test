﻿using Domain.Models.HpInf;
using Domain.Models.Insurance;
using Domain.Models.MstItem;
using Domain.Models.NextOrder;
using Domain.Models.PatientInfor;
using Domain.Models.User;
using Helper.Constants;
using Infrastructure.Interfaces;
using Infrastructure.Options;
using Microsoft.Extensions.Options;
using UseCase.NextOrder.Upsert;
using static Helper.Constants.KarteConst;
using static Helper.Constants.NextOrderConst;
using static Helper.Constants.OrderInfConst;
using static Helper.Constants.RsvkrtByomeiConst;

namespace Interactor.NextOrder
{
    public class UpsertNextOrderListInteractor : IUpsertNextOrderListInputPort
    {
        private readonly INextOrderRepository _nextOrderRepository;
        private readonly IHpInfRepository _hpInfRepository;
        private readonly IPatientInforRepository _patientInfRepository;
        private readonly IUserRepository _userRepository;
        private readonly IInsuranceRepository _insuranceRepository;
        private readonly IMstItemRepository _mstItemRepository;
        private readonly IAmazonS3Service _amazonS3Service;
        private readonly AmazonS3Options _options;

        public UpsertNextOrderListInteractor(IOptions<AmazonS3Options> optionsAccessor, IAmazonS3Service amazonS3Service, INextOrderRepository nextOrderRepository, IHpInfRepository hpInfRepository, IPatientInforRepository patientInfRepository, IUserRepository userRepository, IInsuranceRepository insuranceRepository, IMstItemRepository mstItemRepository)
        {
            _amazonS3Service = amazonS3Service;
            _options = optionsAccessor.Value;
            _nextOrderRepository = nextOrderRepository;
            _hpInfRepository = hpInfRepository;
            _patientInfRepository = patientInfRepository;
            _userRepository = userRepository;
            _insuranceRepository = insuranceRepository;
            _mstItemRepository = mstItemRepository;
        }

        public UpsertNextOrderListOutputData Handle(UpsertNextOrderListInputData inputData)
        {
            try
            {
                if (inputData.HpId <= 0 || !_hpInfRepository.CheckHpId(inputData.HpId))
                {
                    return new UpsertNextOrderListOutputData(UpsertNextOrderListStatus.InvalidHpId, new(), new(), new(), new());
                }
                if (inputData.PtId <= 0 || !_patientInfRepository.CheckExistListId(new List<long> { inputData.PtId }))
                {
                    return new UpsertNextOrderListOutputData(UpsertNextOrderListStatus.InvalidPtId, new(), new(), new(), new());
                }
                if (inputData.UserId <= 0 || !_userRepository.CheckExistedUserId(inputData.UserId))
                {
                    return new UpsertNextOrderListOutputData(UpsertNextOrderListStatus.InvalidUserId, new(), new(), new(), new());
                }

                var itemCds = new List<string>();
                var ipnCds = new List<Tuple<string, string>>();
                List<long> rsvkrtNos = new();
                List<int> rsvkrtDates = new();
                var checkOderInfs = _nextOrderRepository.GetCheckOrderInfs(inputData.HpId, inputData.PtId);
                var hokenPids = new List<int>();
                foreach (var item in inputData.NextOrderItems)
                {
                    hokenPids.AddRange(item.rsvKrtOrderInfItems.Select(o => o.HokenPid));
                }

                var checkHokens = _insuranceRepository.GetCheckListHokenInf(inputData.HpId, inputData.PtId, hokenPids.Distinct().ToList() ?? new List<int>());

                foreach (var nextOrder in inputData.NextOrderItems)
                {
                    rsvkrtNos.Add(nextOrder.RsvkrtNo);
                    rsvkrtNos.Add(nextOrder.rsvkrtKarteInf.RsvkrtNo);
                    rsvkrtDates.Add(nextOrder.RsvDate);
                    rsvkrtDates.Add(nextOrder.rsvkrtKarteInf.RsvDate);
                    foreach (var orderInfModel in nextOrder.rsvKrtOrderInfItems)
                    {
                        rsvkrtNos.Add(orderInfModel.RsvkrtNo);
                        rsvkrtNos.AddRange(orderInfModel.RsvKrtOrderInfDetailItems.Select(od => od.RsvkrtNo));
                        rsvkrtDates.Add(orderInfModel.RsvDate);
                        rsvkrtDates.AddRange(orderInfModel.RsvKrtOrderInfDetailItems.Select(od => od.RsvDate));
                        itemCds.AddRange(_mstItemRepository.GetCheckItemCds(orderInfModel.RsvKrtOrderInfDetailItems.Select(od => od.ItemCd.Trim()).ToList()));
                        ipnCds.AddRange(_mstItemRepository.GetCheckIpnCds(orderInfModel.RsvKrtOrderInfDetailItems.Select(od => od.IpnCd.Trim()).ToList()));
                    }
                    foreach (var byomei in nextOrder.rsvKrtByomeiItems)
                    {
                        rsvkrtNos.Add(byomei.RsvkrtNo);
                    }
                }

                if (rsvkrtNos.Distinct().Count() > 1)
                {
                    return new UpsertNextOrderListOutputData(UpsertNextOrderListStatus.InvalidRsvkrtNo, new(), new(), new(), new());
                }

                if (rsvkrtDates.Distinct().Count() > 1)
                {
                    return new UpsertNextOrderListOutputData(UpsertNextOrderListStatus.InvalidRsvkrtDate, new(), new(), new(), new());
                }

                var nextOrderModels = inputData.NextOrderItems.Select(n => NextOrderCommon.ConvertNextOrderToModel(inputData.HpId, inputData.PtId, ipnCds, n)).ToList();
                List<(int, KarteValidationStatus)> validationKarteInfs = new();
                Dictionary<int, NextOrderStatus> validationNextOrders = new();
                List<(int, int, RsvkrtByomeiStatus)> validationRsvkrtByomeis = new();
                var validationOrdInfs = new List<(int, Dictionary<string, KeyValuePair<string, OrdInfValidationStatus>>)>();

                for (int i = 0; i < nextOrderModels.Count; i++)
                {
                    var nextOrderModel = nextOrderModels[i];

                    var validationNextOrder = nextOrderModel.Validation();
                    if (validationNextOrder != NextOrderStatus.Valid)
                    {
                        validationNextOrders.Add(i, validationNextOrder);
                    }

                    var validationKarteInf = nextOrderModel.RsvkrtKarteInf.Validation();
                    if (validationKarteInf != KarteValidationStatus.Valid)
                    {
                        validationKarteInfs.Add((i, validationKarteInf));
                    }

                    for (int i1 = 0; i1 < nextOrderModel.RsvkrtByomeis.Count; i1++)
                    {
                        var byomei = nextOrderModel.RsvkrtByomeis[i1];
                        var validationByomei = byomei.Validation();
                        if (validationKarteInf != KarteValidationStatus.Valid)
                        {
                            validationRsvkrtByomeis.Add((i, i1, validationByomei));
                        }
                    }

                    var validationOneOrdInf = NextOrderCommon.CheckValidateOrderInf(inputData.HpId, inputData.PtId, itemCds, ipnCds.Select(ipn => ipn.Item1).ToList(), nextOrderModel, checkOderInfs, checkHokens);

                    if (validationOneOrdInf.Any())
                        validationOrdInfs.Add((i, validationOneOrdInf));
                }
                if (validationKarteInfs.Count > 0 || validationNextOrders.Count > 0 || validationRsvkrtByomeis.Count > 0 || validationOrdInfs.Count > 0)
                {
                    return new UpsertNextOrderListOutputData(UpsertNextOrderListStatus.Failed, validationNextOrders, validationOrdInfs, validationKarteInfs, validationRsvkrtByomeis);
                }
                var rsvkrtNo = _nextOrderRepository.Upsert(inputData.UserId, inputData.HpId, inputData.PtId, nextOrderModels);
                if (rsvkrtNo > 0)
                {
                    SaveFileNextOrder(inputData.HpId, inputData.PtId, rsvkrtNo, inputData.ListFileItems, true);
                }
                else
                {
                    SaveFileNextOrder(inputData.HpId, inputData.PtId, rsvkrtNo, inputData.ListFileItems, false);
                }
                return new UpsertNextOrderListOutputData(UpsertNextOrderListStatus.Successed, new(), new(), new(), new());
            }
            catch
            {
                return new UpsertNextOrderListOutputData(UpsertNextOrderListStatus.Failed, new(), new(), new(), new());
            }
        }

        private void SaveFileNextOrder(int hpId, long ptId, long rsvkrtNo, List<string> listFileItems, bool saveSuccess)
        {
            var ptInf = _patientInfRepository.GetById(hpId, ptId, 0, 0);
            List<string> listFolders = new();
            string path = string.Empty;
            listFolders.Add(CommonConstants.Store);
            listFolders.Add(CommonConstants.Karte);
            listFolders.Add(CommonConstants.NextPic);
            path = _amazonS3Service.GetFolderUploadToPtNum(listFolders, ptInf != null ? ptInf.PtNum : 0);
            string host = _options.BaseAccessUrl + "/" + path;
            var listUpdates = listFileItems.Select(item => item.Replace(host, string.Empty)).ToList();
            if (saveSuccess)
            {
                _nextOrderRepository.SaveListFileNextOrder(hpId, ptId, rsvkrtNo, listUpdates, false);
            }
            else
            {
                _nextOrderRepository.ClearTempData(hpId, ptId, listUpdates.ToList());
                foreach (var item in listUpdates)
                {
                    _amazonS3Service.DeleteObjectAsync(path + item);
                }
            }
        }
    }
}
