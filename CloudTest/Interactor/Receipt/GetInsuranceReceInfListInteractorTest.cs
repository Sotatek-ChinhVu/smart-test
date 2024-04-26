using Domain.Models.Insurance;
using Domain.Models.InsuranceInfor;
using Domain.Models.MstItem;
using Domain.Models.PatientInfor;
using Domain.Models.Receipt;
using Interactor.Receipt;
using Moq;
using UseCase.Receipt.GetInsuranceReceInfList;

namespace CloudUnitTest.Interactor.Receipt
{
    public class GetInsuranceReceInfListInteractorTest : BaseUT
    {
        /// <summary>
        /// receSbt[0] == '8' and hokenKbn == 0
        /// </summary>
        [Test]
        public void TC_001_GetInsuranceReceInfListInteractor_Handle_HokenKbn_0()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();

            var getInsuranceReceInfListInteractor = new GetInsuranceReceInfListInteractor(mockIReceiptRepository.Object, mockIInsuranceRepository.Object);

            int hpId = 99999999, seikyuYm = 200103, sinYm = 200103, hokenId = 0, hokenId2 = 0, hokenSbtCd = 0, hokenPid = 0, hokenKbn = 0;
            int kohi1Id = 1, kohi2Id = 0, kohi3Id = 0, kohi4Id = 0, startDate = 0, endDate = 0, sinDate = 0;
            long ptId = 20010328, seqNo = 999999;
            string receSbt = "8801", hokensyaNo = "", tokki1 = "", tokki2 = "", tokki3 = "", tokki4 = "", tokki5 = "", futansyaNoKohi1 = "", futansyaNoKohi2 = "", futansyaNoKohi3 = "", futansyaNoKohi4 = "", jyukyusyaNoKohi1 = "", jyukyusyaNoKohi2 = "";
            string jyukyusyaNoKohi3 = "", jyukyusyaNoKohi4 = "", hokenInfRousaiKofuNo = "", kigo = "", bango = "", edaNo = "";
            int hokenReceTensu = 0, hokenReceFutan = 0, kohi1ReceTensu = 0, kohi1ReceFutan = 0, kohi1ReceKyufu = 0, kohi2ReceTensu = 0, kohi2ReceFutan = 0, kohi2ReceKyufu = 0, kohi3ReceTensu = 0, kohi3ReceFutan = 0, kohi3ReceKyufu = 0, kohi4ReceTensu = 0;
            int kohi4ReceFutan = 0, kohi4ReceKyufu = 0, hokenNissu = 0, kohi1Nissu = 0, kohi2Nissu = 0, kohi3Nissu = 0, kohi4Nissu = 0, kohi1ReceKisai = 0, kohi2ReceKisai = 0, kohi3ReceKisai = 0, kohi4ReceKisai = 0, rousaiIFutan = 0, rousaiRoFutan = 0;
            int jibaiITensu = 0, jibaiRoTensu = 0, jibaiHaFutan = 0, jibaiNiFutan = 0, jibaiHoSindan = 0, jibaiHeMeisai = 0, jibaiAFutan = 0, jibaiBFutan = 0, jibaiCFutan = 0, jibaiDFutan = 0, jibaiKenpoFutan = 0, hokenPId = 0;

            List<InsuranceModel> listInsurance = new List<InsuranceModel>()
            {
                new InsuranceModel(hpId, ptId, seqNo, hokenSbtCd, hokenPid, hokenKbn, hokenId, kohi1Id, kohi2Id, kohi3Id, kohi4Id, startDate, endDate, sinDate)
            };

            List<HokenInfModel> listHokenInf = new List<HokenInfModel>()
            {
                new HokenInfModel(0, 0, 0)
            };

            List<KohiInfModel> listKohi = new List<KohiInfModel>()
            {
                new KohiInfModel(hokenId)
            };

            InsuranceDataModel insuranceDataModel = new InsuranceDataModel(listInsurance, listHokenInf, listKohi, 12, 13, 14);
            InsuranceReceInfModel insuranceReceInfModel = new InsuranceReceInfModel(seikyuYm, ptId, sinYm, hokenId, hokenId2, kohi1Id, kohi2Id, kohi3Id, kohi4Id, hokenKbn, receSbt, hokensyaNo, hokenReceTensu,
                                                                                    hokenReceFutan, kohi1ReceTensu, kohi1ReceFutan, kohi1ReceKyufu, kohi2ReceTensu, kohi2ReceFutan, kohi2ReceKyufu, kohi3ReceTensu,
                                                                                    kohi3ReceFutan, kohi3ReceKyufu, kohi4ReceTensu, kohi4ReceFutan, kohi4ReceKyufu, hokenNissu, kohi1Nissu, kohi2Nissu, kohi3Nissu,
                                                                                    kohi4Nissu, kohi1ReceKisai, kohi2ReceKisai, kohi3ReceKisai, kohi4ReceKisai, tokki1, tokki2, tokki3, tokki4, tokki5, rousaiIFutan,
                                                                                    rousaiRoFutan, jibaiITensu, jibaiRoTensu, jibaiHaFutan, jibaiNiFutan, jibaiHoSindan, jibaiHeMeisai, jibaiAFutan, jibaiBFutan, jibaiCFutan,
                                                                                    jibaiDFutan, jibaiKenpoFutan, futansyaNoKohi1, futansyaNoKohi2, futansyaNoKohi3, futansyaNoKohi4, jyukyusyaNoKohi1, jyukyusyaNoKohi2, jyukyusyaNoKohi3, 
                                                                                    jyukyusyaNoKohi4, hokenInfRousaiKofuNo, kigo, bango, edaNo, hokenPId); 
            GetInsuranceReceInfListInputData inputData = new GetInsuranceReceInfListInputData(hpId, seikyuYm, sinYm, ptId, hokenId);
            mockIInsuranceRepository.Setup(x => x.GetInsuranceListById(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns((int input1, long input2, int input3, bool input4, bool input5) => insuranceDataModel);
            mockIReceiptRepository.Setup(x => x.GetInsuranceReceInfList(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>())).Returns((int input1, int input2, int input3, int input4, long input5) => insuranceReceInfModel);

            // Act
            var result = getInsuranceReceInfListInteractor.Handle(inputData);

            // Assert
            Assert.True(result.InsuranceReceInf.InsuranceName == "自費" && result.Status == GetInsuranceReceInfListStatus.Successed);
        }

        /// <summary>
        /// receSbt[0] == '9' and hokenKbn == 0
        /// </summary>
        [Test]
        public void TC_002_GetInsuranceReceInfListInteractor_Handle_HokenKbn_0()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();

            var getInsuranceReceInfListInteractor = new GetInsuranceReceInfListInteractor(mockIReceiptRepository.Object, mockIInsuranceRepository.Object);

            int hpId = 99999999, seikyuYm = 200103, sinYm = 200103, hokenId = 0, hokenId2 = 0, hokenSbtCd = 0, hokenPid = 0, hokenKbn = 0;
            int kohi1Id = 1, kohi2Id = 0, kohi3Id = 0, kohi4Id = 0, startDate = 0, endDate = 0, sinDate = 0;
            long ptId = 20010328, seqNo = 999999;
            string receSbt = "9818", hokensyaNo = "", tokki1 = "", tokki2 = "", tokki3 = "", tokki4 = "", tokki5 = "", futansyaNoKohi1 = "", futansyaNoKohi2 = "", futansyaNoKohi3 = "", futansyaNoKohi4 = "", jyukyusyaNoKohi1 = "", jyukyusyaNoKohi2 = "";
            string jyukyusyaNoKohi3 = "", jyukyusyaNoKohi4 = "", hokenInfRousaiKofuNo = "", kigo = "", bango = "", edaNo = "";
            int hokenReceTensu = 0, hokenReceFutan = 0, kohi1ReceTensu = 0, kohi1ReceFutan = 0, kohi1ReceKyufu = 0, kohi2ReceTensu = 0, kohi2ReceFutan = 0, kohi2ReceKyufu = 0, kohi3ReceTensu = 0, kohi3ReceFutan = 0, kohi3ReceKyufu = 0, kohi4ReceTensu = 0;
            int kohi4ReceFutan = 0, kohi4ReceKyufu = 0, hokenNissu = 0, kohi1Nissu = 0, kohi2Nissu = 0, kohi3Nissu = 0, kohi4Nissu = 0, kohi1ReceKisai = 0, kohi2ReceKisai = 0, kohi3ReceKisai = 0, kohi4ReceKisai = 0, rousaiIFutan = 0, rousaiRoFutan = 0;
            int jibaiITensu = 0, jibaiRoTensu = 0, jibaiHaFutan = 0, jibaiNiFutan = 0, jibaiHoSindan = 0, jibaiHeMeisai = 0, jibaiAFutan = 0, jibaiBFutan = 0, jibaiCFutan = 0, jibaiDFutan = 0, jibaiKenpoFutan = 0, hokenPId = 0;

            List<InsuranceModel> listInsurance = new List<InsuranceModel>()
            {
                new InsuranceModel(hpId, ptId, seqNo, hokenSbtCd, hokenPid, hokenKbn, hokenId, kohi1Id, kohi2Id, kohi3Id, kohi4Id, startDate, endDate, sinDate)
            };

            List<HokenInfModel> listHokenInf = new List<HokenInfModel>()
            {
                new HokenInfModel(0, 0, 0)
            };

            List<KohiInfModel> listKohi = new List<KohiInfModel>()
            {
                new KohiInfModel(hokenId)
            };

            InsuranceDataModel insuranceDataModel = new InsuranceDataModel(listInsurance, listHokenInf, listKohi, 12, 13, 14);
            InsuranceReceInfModel insuranceReceInfModel = new InsuranceReceInfModel(seikyuYm, ptId, sinYm, hokenId, hokenId2, kohi1Id, kohi2Id, kohi3Id, kohi4Id, hokenKbn, receSbt, hokensyaNo, hokenReceTensu,
                                                                                    hokenReceFutan, kohi1ReceTensu, kohi1ReceFutan, kohi1ReceKyufu, kohi2ReceTensu, kohi2ReceFutan, kohi2ReceKyufu, kohi3ReceTensu,
                                                                                    kohi3ReceFutan, kohi3ReceKyufu, kohi4ReceTensu, kohi4ReceFutan, kohi4ReceKyufu, hokenNissu, kohi1Nissu, kohi2Nissu, kohi3Nissu,
                                                                                    kohi4Nissu, kohi1ReceKisai, kohi2ReceKisai, kohi3ReceKisai, kohi4ReceKisai, tokki1, tokki2, tokki3, tokki4, tokki5, rousaiIFutan,
                                                                                    rousaiRoFutan, jibaiITensu, jibaiRoTensu, jibaiHaFutan, jibaiNiFutan, jibaiHoSindan, jibaiHeMeisai, jibaiAFutan, jibaiBFutan, jibaiCFutan,
                                                                                    jibaiDFutan, jibaiKenpoFutan, futansyaNoKohi1, futansyaNoKohi2, futansyaNoKohi3, futansyaNoKohi4, jyukyusyaNoKohi1, jyukyusyaNoKohi2, jyukyusyaNoKohi3,
                                                                                    jyukyusyaNoKohi4, hokenInfRousaiKofuNo, kigo, bango, edaNo, hokenPId);
            GetInsuranceReceInfListInputData inputData = new GetInsuranceReceInfListInputData(hpId, seikyuYm, sinYm, ptId, hokenId);
            mockIInsuranceRepository.Setup(x => x.GetInsuranceListById(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns((int input1, long input2, int input3, bool input4, bool input5) => insuranceDataModel);
            mockIReceiptRepository.Setup(x => x.GetInsuranceReceInfList(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>())).Returns((int input1, int input2, int input3, int input4, long input5) => insuranceReceInfModel);

            // Act
            var result = getInsuranceReceInfListInteractor.Handle(inputData);

            // Assert
            Assert.That(result.InsuranceReceInf.InsuranceName == "自費レセ単独" && result.Status == GetInsuranceReceInfListStatus.Successed);
        }

        /// <summary>
        /// hokenKbn == 1
        /// </summary>
        [Test]
        public void TC_003_GetInsuranceReceInfListInteractor_Handle_HokenKbn_1()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();

            var getInsuranceReceInfListInteractor = new GetInsuranceReceInfListInteractor(mockIReceiptRepository.Object, mockIInsuranceRepository.Object);

            int hpId = 99999999, seikyuYm = 200103, sinYm = 200103, hokenId = 0, hokenId2 = 0, hokenSbtCd = 0, hokenPid = 0, hokenKbn = 1;
            int kohi1Id = 0, kohi2Id = 0, kohi3Id = 0, kohi4Id = 0, startDate = 0, endDate = 0, sinDate = 0;
            long ptId = 20010328, seqNo = 999999;
            string receSbt = "1112", hokensyaNo = "", tokki1 = "", tokki2 = "", tokki3 = "", tokki4 = "", tokki5 = "", futansyaNoKohi1 = "", futansyaNoKohi2 = "", futansyaNoKohi3 = "", futansyaNoKohi4 = "", jyukyusyaNoKohi1 = "", jyukyusyaNoKohi2 = "";
            string jyukyusyaNoKohi3 = "", jyukyusyaNoKohi4 = "", hokenInfRousaiKofuNo = "", kigo = "", bango = "", edaNo = "";
            int hokenReceTensu = 0, hokenReceFutan = 0, kohi1ReceTensu = 0, kohi1ReceFutan = 0, kohi1ReceKyufu = 0, kohi2ReceTensu = 0, kohi2ReceFutan = 0, kohi2ReceKyufu = 0, kohi3ReceTensu = 0, kohi3ReceFutan = 0, kohi3ReceKyufu = 0, kohi4ReceTensu = 0;
            int kohi4ReceFutan = 0, kohi4ReceKyufu = 0, hokenNissu = 0, kohi1Nissu = 0, kohi2Nissu = 0, kohi3Nissu = 0, kohi4Nissu = 0, kohi1ReceKisai = 0, kohi2ReceKisai = 0, kohi3ReceKisai = 0, kohi4ReceKisai = 0, rousaiIFutan = 0, rousaiRoFutan = 0;
            int jibaiITensu = 0, jibaiRoTensu = 0, jibaiHaFutan = 0, jibaiNiFutan = 0, jibaiHoSindan = 0, jibaiHeMeisai = 0, jibaiAFutan = 0, jibaiBFutan = 0, jibaiCFutan = 0, jibaiDFutan = 0, jibaiKenpoFutan = 0, hokenPId = 0;

            List<InsuranceModel> listInsurance = new List<InsuranceModel>()
            {
                new InsuranceModel(hpId, ptId, seqNo, hokenSbtCd, hokenPid, hokenKbn, hokenId, kohi1Id, kohi2Id, kohi3Id, kohi4Id, startDate, endDate, sinDate)
            };

            List<HokenInfModel> listHokenInf = new List<HokenInfModel>()
            {
                new HokenInfModel(0, 0, 0)
            };

            List<KohiInfModel> listKohi = new List<KohiInfModel>()
            {
                new KohiInfModel(hokenId)
            };

            InsuranceDataModel insuranceDataModel = new InsuranceDataModel(listInsurance, listHokenInf, listKohi, 12, 13, 14);
            InsuranceReceInfModel insuranceReceInfModel = new InsuranceReceInfModel(seikyuYm, ptId, sinYm, hokenId, hokenId2, kohi1Id, kohi2Id, kohi3Id, kohi4Id, hokenKbn, receSbt, hokensyaNo, hokenReceTensu,
                                                                                    hokenReceFutan, kohi1ReceTensu, kohi1ReceFutan, kohi1ReceKyufu, kohi2ReceTensu, kohi2ReceFutan, kohi2ReceKyufu, kohi3ReceTensu,
                                                                                    kohi3ReceFutan, kohi3ReceKyufu, kohi4ReceTensu, kohi4ReceFutan, kohi4ReceKyufu, hokenNissu, kohi1Nissu, kohi2Nissu, kohi3Nissu,
                                                                                    kohi4Nissu, kohi1ReceKisai, kohi2ReceKisai, kohi3ReceKisai, kohi4ReceKisai, tokki1, tokki2, tokki3, tokki4, tokki5, rousaiIFutan,
                                                                                    rousaiRoFutan, jibaiITensu, jibaiRoTensu, jibaiHaFutan, jibaiNiFutan, jibaiHoSindan, jibaiHeMeisai, jibaiAFutan, jibaiBFutan, jibaiCFutan,
                                                                                    jibaiDFutan, jibaiKenpoFutan, futansyaNoKohi1, futansyaNoKohi2, futansyaNoKohi3, futansyaNoKohi4, jyukyusyaNoKohi1, jyukyusyaNoKohi2, jyukyusyaNoKohi3,
                                                                                    jyukyusyaNoKohi4, hokenInfRousaiKofuNo, kigo, bango, edaNo, hokenPId);
            GetInsuranceReceInfListInputData inputData = new GetInsuranceReceInfListInputData(hpId, seikyuYm, sinYm, ptId, hokenId);
            mockIInsuranceRepository.Setup(x => x.GetInsuranceListById(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns((int input1, long input2, int input3, bool input4, bool input5) => insuranceDataModel);
            mockIReceiptRepository.Setup(x => x.GetInsuranceReceInfList(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>())).Returns((int input1, int input2, int input3, int input4, long input5) => insuranceReceInfModel);

            // Act
            var result = getInsuranceReceInfListInteractor.Handle(inputData);

            // Assert
            Assert.That(result.InsuranceReceInf.InsuranceName == "社保単独・本人" && result.Status == GetInsuranceReceInfListStatus.Successed);
        }

        /// <summary>
        /// hokenKbn == 2
        /// </summary>
        [Test]
        public void TC_004_GetInsuranceReceInfListInteractor_Handle_HokenKbn_2()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();

            var getInsuranceReceInfListInteractor = new GetInsuranceReceInfListInteractor(mockIReceiptRepository.Object, mockIInsuranceRepository.Object);

            int hpId = 99999999, seikyuYm = 200103, sinYm = 200103, hokenId = 0, hokenId2 = 0, hokenSbtCd = 0, hokenPid = 0, hokenKbn = 2;
            int kohi1Id = 0, kohi2Id = 0, kohi3Id = 0, kohi4Id = 0, startDate = 0, endDate = 0, sinDate = 0;
            long ptId = 20010328, seqNo = 999999;
            string receSbt = "1112", hokensyaNo = "", tokki1 = "", tokki2 = "", tokki3 = "", tokki4 = "", tokki5 = "", futansyaNoKohi1 = "", futansyaNoKohi2 = "", futansyaNoKohi3 = "", futansyaNoKohi4 = "", jyukyusyaNoKohi1 = "", jyukyusyaNoKohi2 = "";
            string jyukyusyaNoKohi3 = "", jyukyusyaNoKohi4 = "", hokenInfRousaiKofuNo = "", kigo = "", bango = "", edaNo = "";
            int hokenReceTensu = 0, hokenReceFutan = 0, kohi1ReceTensu = 0, kohi1ReceFutan = 0, kohi1ReceKyufu = 0, kohi2ReceTensu = 0, kohi2ReceFutan = 0, kohi2ReceKyufu = 0, kohi3ReceTensu = 0, kohi3ReceFutan = 0, kohi3ReceKyufu = 0, kohi4ReceTensu = 0;
            int kohi4ReceFutan = 0, kohi4ReceKyufu = 0, hokenNissu = 0, kohi1Nissu = 0, kohi2Nissu = 0, kohi3Nissu = 0, kohi4Nissu = 0, kohi1ReceKisai = 0, kohi2ReceKisai = 0, kohi3ReceKisai = 0, kohi4ReceKisai = 0, rousaiIFutan = 0, rousaiRoFutan = 0;
            int jibaiITensu = 0, jibaiRoTensu = 0, jibaiHaFutan = 0, jibaiNiFutan = 0, jibaiHoSindan = 0, jibaiHeMeisai = 0, jibaiAFutan = 0, jibaiBFutan = 0, jibaiCFutan = 0, jibaiDFutan = 0, jibaiKenpoFutan = 0, hokenPId = 0;

            List<InsuranceModel> listInsurance = new List<InsuranceModel>()
            {
                new InsuranceModel(hpId, ptId, seqNo, hokenSbtCd, hokenPid, hokenKbn, hokenId, kohi1Id, kohi2Id, kohi3Id, kohi4Id, startDate, endDate, sinDate)
            };

            List<HokenInfModel> listHokenInf = new List<HokenInfModel>()
            {
                new HokenInfModel(0, 0, 0)
            };

            List<KohiInfModel> listKohi = new List<KohiInfModel>()
            {
                new KohiInfModel(hokenId)
            };

            InsuranceDataModel insuranceDataModel = new InsuranceDataModel(listInsurance, listHokenInf, listKohi, 12, 13, 14);
            InsuranceReceInfModel insuranceReceInfModel = new InsuranceReceInfModel(seikyuYm, ptId, sinYm, hokenId, hokenId2, kohi1Id, kohi2Id, kohi3Id, kohi4Id, hokenKbn, receSbt, hokensyaNo, hokenReceTensu,
                                                                                    hokenReceFutan, kohi1ReceTensu, kohi1ReceFutan, kohi1ReceKyufu, kohi2ReceTensu, kohi2ReceFutan, kohi2ReceKyufu, kohi3ReceTensu,
                                                                                    kohi3ReceFutan, kohi3ReceKyufu, kohi4ReceTensu, kohi4ReceFutan, kohi4ReceKyufu, hokenNissu, kohi1Nissu, kohi2Nissu, kohi3Nissu,
                                                                                    kohi4Nissu, kohi1ReceKisai, kohi2ReceKisai, kohi3ReceKisai, kohi4ReceKisai, tokki1, tokki2, tokki3, tokki4, tokki5, rousaiIFutan,
                                                                                    rousaiRoFutan, jibaiITensu, jibaiRoTensu, jibaiHaFutan, jibaiNiFutan, jibaiHoSindan, jibaiHeMeisai, jibaiAFutan, jibaiBFutan, jibaiCFutan,
                                                                                    jibaiDFutan, jibaiKenpoFutan, futansyaNoKohi1, futansyaNoKohi2, futansyaNoKohi3, futansyaNoKohi4, jyukyusyaNoKohi1, jyukyusyaNoKohi2, jyukyusyaNoKohi3,
                                                                                    jyukyusyaNoKohi4, hokenInfRousaiKofuNo, kigo, bango, edaNo, hokenPId);
            GetInsuranceReceInfListInputData inputData = new GetInsuranceReceInfListInputData(hpId, seikyuYm, sinYm, ptId, hokenId);
            mockIInsuranceRepository.Setup(x => x.GetInsuranceListById(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns((int input1, long input2, int input3, bool input4, bool input5) => insuranceDataModel);
            mockIReceiptRepository.Setup(x => x.GetInsuranceReceInfList(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>())).Returns((int input1, int input2, int input3, int input4, long input5) => insuranceReceInfModel);
            
            // Act
            var result = getInsuranceReceInfListInteractor.Handle(inputData);

            // Assert
            Assert.That(result.InsuranceReceInf.InsuranceName == "国保単独・本人" && result.Status == GetInsuranceReceInfListStatus.Successed);
        }

        /// <summary>
        /// hokenKbn == 11
        /// </summary>
        [Test]
        public void TC_005_GetInsuranceReceInfListInteractor_Handle_HokenKbn_11()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();

            var getInsuranceReceInfListInteractor = new GetInsuranceReceInfListInteractor(mockIReceiptRepository.Object, mockIInsuranceRepository.Object);

            int hpId = 99999999, seikyuYm = 200103, sinYm = 200103, hokenId = 0, hokenId2 = 0, hokenSbtCd = 0, hokenPid = 0, hokenKbn = 11;
            int kohi1Id = 0, kohi2Id = 0, kohi3Id = 0, kohi4Id = 0, startDate = 0, endDate = 0, sinDate = 0;
            long ptId = 20010328, seqNo = 999999;
            string receSbt = "1112", hokensyaNo = "", tokki1 = "", tokki2 = "", tokki3 = "", tokki4 = "", tokki5 = "", futansyaNoKohi1 = "", futansyaNoKohi2 = "", futansyaNoKohi3 = "", futansyaNoKohi4 = "", jyukyusyaNoKohi1 = "", jyukyusyaNoKohi2 = "";
            string jyukyusyaNoKohi3 = "", jyukyusyaNoKohi4 = "", hokenInfRousaiKofuNo = "", kigo = "", bango = "", edaNo = "";
            int hokenReceTensu = 0, hokenReceFutan = 0, kohi1ReceTensu = 0, kohi1ReceFutan = 0, kohi1ReceKyufu = 0, kohi2ReceTensu = 0, kohi2ReceFutan = 0, kohi2ReceKyufu = 0, kohi3ReceTensu = 0, kohi3ReceFutan = 0, kohi3ReceKyufu = 0, kohi4ReceTensu = 0;
            int kohi4ReceFutan = 0, kohi4ReceKyufu = 0, hokenNissu = 0, kohi1Nissu = 0, kohi2Nissu = 0, kohi3Nissu = 0, kohi4Nissu = 0, kohi1ReceKisai = 0, kohi2ReceKisai = 0, kohi3ReceKisai = 0, kohi4ReceKisai = 0, rousaiIFutan = 0, rousaiRoFutan = 0;
            int jibaiITensu = 0, jibaiRoTensu = 0, jibaiHaFutan = 0, jibaiNiFutan = 0, jibaiHoSindan = 0, jibaiHeMeisai = 0, jibaiAFutan = 0, jibaiBFutan = 0, jibaiCFutan = 0, jibaiDFutan = 0, jibaiKenpoFutan = 0, hokenPId = 0;

            List<InsuranceModel> listInsurance = new List<InsuranceModel>()
            {
                new InsuranceModel(hpId, ptId, seqNo, hokenSbtCd, hokenPid, hokenKbn, hokenId, kohi1Id, kohi2Id, kohi3Id, kohi4Id, startDate, endDate, sinDate)
            };

            List<HokenInfModel> listHokenInf = new List<HokenInfModel>()
            {
                new HokenInfModel(0, 0, 0)
            };

            List<KohiInfModel> listKohi = new List<KohiInfModel>()
            {
                new KohiInfModel(hokenId)
            };

            InsuranceDataModel insuranceDataModel = new InsuranceDataModel(listInsurance, listHokenInf, listKohi, 12, 13, 14);
            InsuranceReceInfModel insuranceReceInfModel = new InsuranceReceInfModel(seikyuYm, ptId, sinYm, hokenId, hokenId2, kohi1Id, kohi2Id, kohi3Id, kohi4Id, hokenKbn, receSbt, hokensyaNo, hokenReceTensu,
                                                                                    hokenReceFutan, kohi1ReceTensu, kohi1ReceFutan, kohi1ReceKyufu, kohi2ReceTensu, kohi2ReceFutan, kohi2ReceKyufu, kohi3ReceTensu,
                                                                                    kohi3ReceFutan, kohi3ReceKyufu, kohi4ReceTensu, kohi4ReceFutan, kohi4ReceKyufu, hokenNissu, kohi1Nissu, kohi2Nissu, kohi3Nissu,
                                                                                    kohi4Nissu, kohi1ReceKisai, kohi2ReceKisai, kohi3ReceKisai, kohi4ReceKisai, tokki1, tokki2, tokki3, tokki4, tokki5, rousaiIFutan,
                                                                                    rousaiRoFutan, jibaiITensu, jibaiRoTensu, jibaiHaFutan, jibaiNiFutan, jibaiHoSindan, jibaiHeMeisai, jibaiAFutan, jibaiBFutan, jibaiCFutan,
                                                                                    jibaiDFutan, jibaiKenpoFutan, futansyaNoKohi1, futansyaNoKohi2, futansyaNoKohi3, futansyaNoKohi4, jyukyusyaNoKohi1, jyukyusyaNoKohi2, jyukyusyaNoKohi3,
                                                                                    jyukyusyaNoKohi4, hokenInfRousaiKofuNo, kigo, bango, edaNo, hokenPId);
            GetInsuranceReceInfListInputData inputData = new GetInsuranceReceInfListInputData(hpId, seikyuYm, sinYm, ptId, hokenId);
            mockIInsuranceRepository.Setup(x => x.GetInsuranceListById(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns((int input1, long input2, int input3, bool input4, bool input5) => insuranceDataModel);
            mockIReceiptRepository.Setup(x => x.GetInsuranceReceInfList(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>())).Returns((int input1, int input2, int input3, int input4, long input5) => insuranceReceInfModel);

            // Act
            var result = getInsuranceReceInfListInteractor.Handle(inputData);

            // Assert
            Assert.That(result.InsuranceReceInf.InsuranceName == "労災(短期給付)" && result.Status == GetInsuranceReceInfListStatus.Successed);
        }

        /// <summary>
        /// hokenKbn == 12
        /// </summary>
        [Test]
        public void TC_006_GetInsuranceReceInfListInteractor_Handle_HokenKbn_12()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();

            var getInsuranceReceInfListInteractor = new GetInsuranceReceInfListInteractor(mockIReceiptRepository.Object, mockIInsuranceRepository.Object);

            int hpId = 99999999, seikyuYm = 200103, sinYm = 200103, hokenId = 0, hokenId2 = 0, hokenSbtCd = 0, hokenPid = 0, hokenKbn = 12;
            int kohi1Id = 0, kohi2Id = 0, kohi3Id = 0, kohi4Id = 0, startDate = 0, endDate = 0, sinDate = 0;
            long ptId = 20010328, seqNo = 999999;
            string receSbt = "1112", hokensyaNo = "", tokki1 = "", tokki2 = "", tokki3 = "", tokki4 = "", tokki5 = "", futansyaNoKohi1 = "", futansyaNoKohi2 = "", futansyaNoKohi3 = "", futansyaNoKohi4 = "", jyukyusyaNoKohi1 = "", jyukyusyaNoKohi2 = "";
            string jyukyusyaNoKohi3 = "", jyukyusyaNoKohi4 = "", hokenInfRousaiKofuNo = "", kigo = "", bango = "", edaNo = "";
            int hokenReceTensu = 0, hokenReceFutan = 0, kohi1ReceTensu = 0, kohi1ReceFutan = 0, kohi1ReceKyufu = 0, kohi2ReceTensu = 0, kohi2ReceFutan = 0, kohi2ReceKyufu = 0, kohi3ReceTensu = 0, kohi3ReceFutan = 0, kohi3ReceKyufu = 0, kohi4ReceTensu = 0;
            int kohi4ReceFutan = 0, kohi4ReceKyufu = 0, hokenNissu = 0, kohi1Nissu = 0, kohi2Nissu = 0, kohi3Nissu = 0, kohi4Nissu = 0, kohi1ReceKisai = 0, kohi2ReceKisai = 0, kohi3ReceKisai = 0, kohi4ReceKisai = 0, rousaiIFutan = 0, rousaiRoFutan = 0;
            int jibaiITensu = 0, jibaiRoTensu = 0, jibaiHaFutan = 0, jibaiNiFutan = 0, jibaiHoSindan = 0, jibaiHeMeisai = 0, jibaiAFutan = 0, jibaiBFutan = 0, jibaiCFutan = 0, jibaiDFutan = 0, jibaiKenpoFutan = 0, hokenPId = 0;

            List<InsuranceModel> listInsurance = new List<InsuranceModel>()
            {
                new InsuranceModel(hpId, ptId, seqNo, hokenSbtCd, hokenPid, hokenKbn, hokenId, kohi1Id, kohi2Id, kohi3Id, kohi4Id, startDate, endDate, sinDate)
            };

            List<HokenInfModel> listHokenInf = new List<HokenInfModel>()
            {
                new HokenInfModel(0, 0, 0)
            };

            List<KohiInfModel> listKohi = new List<KohiInfModel>()
            {
                new KohiInfModel(hokenId)
            };

            InsuranceDataModel insuranceDataModel = new InsuranceDataModel(listInsurance, listHokenInf, listKohi, 12, 13, 14);
            InsuranceReceInfModel insuranceReceInfModel = new InsuranceReceInfModel(seikyuYm, ptId, sinYm, hokenId, hokenId2, kohi1Id, kohi2Id, kohi3Id, kohi4Id, hokenKbn, receSbt, hokensyaNo, hokenReceTensu,
                                                                                    hokenReceFutan, kohi1ReceTensu, kohi1ReceFutan, kohi1ReceKyufu, kohi2ReceTensu, kohi2ReceFutan, kohi2ReceKyufu, kohi3ReceTensu,
                                                                                    kohi3ReceFutan, kohi3ReceKyufu, kohi4ReceTensu, kohi4ReceFutan, kohi4ReceKyufu, hokenNissu, kohi1Nissu, kohi2Nissu, kohi3Nissu,
                                                                                    kohi4Nissu, kohi1ReceKisai, kohi2ReceKisai, kohi3ReceKisai, kohi4ReceKisai, tokki1, tokki2, tokki3, tokki4, tokki5, rousaiIFutan,
                                                                                    rousaiRoFutan, jibaiITensu, jibaiRoTensu, jibaiHaFutan, jibaiNiFutan, jibaiHoSindan, jibaiHeMeisai, jibaiAFutan, jibaiBFutan, jibaiCFutan,
                                                                                    jibaiDFutan, jibaiKenpoFutan, futansyaNoKohi1, futansyaNoKohi2, futansyaNoKohi3, futansyaNoKohi4, jyukyusyaNoKohi1, jyukyusyaNoKohi2, jyukyusyaNoKohi3,
                                                                                    jyukyusyaNoKohi4, hokenInfRousaiKofuNo, kigo, bango, edaNo, hokenPId);
            GetInsuranceReceInfListInputData inputData = new GetInsuranceReceInfListInputData(hpId, seikyuYm, sinYm, ptId, hokenId);
            mockIInsuranceRepository.Setup(x => x.GetInsuranceListById(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns((int input1, long input2, int input3, bool input4, bool input5) => insuranceDataModel);
            mockIReceiptRepository.Setup(x => x.GetInsuranceReceInfList(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>())).Returns((int input1, int input2, int input3, int input4, long input5) => insuranceReceInfModel);

            // Act
            var result = getInsuranceReceInfListInteractor.Handle(inputData);

            // Assert
            Assert.That(result.InsuranceReceInf.InsuranceName == "労災(傷病年金)" && result.Status == GetInsuranceReceInfListStatus.Successed);
        }

        /// <summary>
        /// hokenKbn == 13
        /// </summary>
        [Test]
        public void TC_007_GetInsuranceReceInfListInteractor_Handle_HokenKbn_13()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();

            var getInsuranceReceInfListInteractor = new GetInsuranceReceInfListInteractor(mockIReceiptRepository.Object, mockIInsuranceRepository.Object);

            int hpId = 99999999, seikyuYm = 200103, sinYm = 200103, hokenId = 0, hokenId2 = 0, hokenSbtCd = 0, hokenPid = 0, hokenKbn = 13;
            int kohi1Id = 0, kohi2Id = 0, kohi3Id = 0, kohi4Id = 0, startDate = 0, endDate = 0, sinDate = 0;
            long ptId = 20010328, seqNo = 999999;
            string receSbt = "1112", hokensyaNo = "", tokki1 = "", tokki2 = "", tokki3 = "", tokki4 = "", tokki5 = "", futansyaNoKohi1 = "", futansyaNoKohi2 = "", futansyaNoKohi3 = "", futansyaNoKohi4 = "", jyukyusyaNoKohi1 = "", jyukyusyaNoKohi2 = "";
            string jyukyusyaNoKohi3 = "", jyukyusyaNoKohi4 = "", hokenInfRousaiKofuNo = "", kigo = "", bango = "", edaNo = "";
            int hokenReceTensu = 0, hokenReceFutan = 0, kohi1ReceTensu = 0, kohi1ReceFutan = 0, kohi1ReceKyufu = 0, kohi2ReceTensu = 0, kohi2ReceFutan = 0, kohi2ReceKyufu = 0, kohi3ReceTensu = 0, kohi3ReceFutan = 0, kohi3ReceKyufu = 0, kohi4ReceTensu = 0;
            int kohi4ReceFutan = 0, kohi4ReceKyufu = 0, hokenNissu = 0, kohi1Nissu = 0, kohi2Nissu = 0, kohi3Nissu = 0, kohi4Nissu = 0, kohi1ReceKisai = 0, kohi2ReceKisai = 0, kohi3ReceKisai = 0, kohi4ReceKisai = 0, rousaiIFutan = 0, rousaiRoFutan = 0;
            int jibaiITensu = 0, jibaiRoTensu = 0, jibaiHaFutan = 0, jibaiNiFutan = 0, jibaiHoSindan = 0, jibaiHeMeisai = 0, jibaiAFutan = 0, jibaiBFutan = 0, jibaiCFutan = 0, jibaiDFutan = 0, jibaiKenpoFutan = 0, hokenPId = 0;

            List<InsuranceModel> listInsurance = new List<InsuranceModel>()
            {
                new InsuranceModel(hpId, ptId, seqNo, hokenSbtCd, hokenPid, hokenKbn, hokenId, kohi1Id, kohi2Id, kohi3Id, kohi4Id, startDate, endDate, sinDate)
            };

            List<HokenInfModel> listHokenInf = new List<HokenInfModel>()
            {
                new HokenInfModel(0, 0, 0)
            };

            List<KohiInfModel> listKohi = new List<KohiInfModel>()
            {
                new KohiInfModel(hokenId)
            };

            InsuranceDataModel insuranceDataModel = new InsuranceDataModel(listInsurance, listHokenInf, listKohi, 12, 13, 14);
            InsuranceReceInfModel insuranceReceInfModel = new InsuranceReceInfModel(seikyuYm, ptId, sinYm, hokenId, hokenId2, kohi1Id, kohi2Id, kohi3Id, kohi4Id, hokenKbn, receSbt, hokensyaNo, hokenReceTensu,
                                                                                    hokenReceFutan, kohi1ReceTensu, kohi1ReceFutan, kohi1ReceKyufu, kohi2ReceTensu, kohi2ReceFutan, kohi2ReceKyufu, kohi3ReceTensu,
                                                                                    kohi3ReceFutan, kohi3ReceKyufu, kohi4ReceTensu, kohi4ReceFutan, kohi4ReceKyufu, hokenNissu, kohi1Nissu, kohi2Nissu, kohi3Nissu,
                                                                                    kohi4Nissu, kohi1ReceKisai, kohi2ReceKisai, kohi3ReceKisai, kohi4ReceKisai, tokki1, tokki2, tokki3, tokki4, tokki5, rousaiIFutan,
                                                                                    rousaiRoFutan, jibaiITensu, jibaiRoTensu, jibaiHaFutan, jibaiNiFutan, jibaiHoSindan, jibaiHeMeisai, jibaiAFutan, jibaiBFutan, jibaiCFutan,
                                                                                    jibaiDFutan, jibaiKenpoFutan, futansyaNoKohi1, futansyaNoKohi2, futansyaNoKohi3, futansyaNoKohi4, jyukyusyaNoKohi1, jyukyusyaNoKohi2, jyukyusyaNoKohi3,
                                                                                    jyukyusyaNoKohi4, hokenInfRousaiKofuNo, kigo, bango, edaNo, hokenPId);
            GetInsuranceReceInfListInputData inputData = new GetInsuranceReceInfListInputData(hpId, seikyuYm, sinYm, ptId, hokenId);
            mockIInsuranceRepository.Setup(x => x.GetInsuranceListById(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns((int input1, long input2, int input3, bool input4, bool input5) => insuranceDataModel);
            mockIReceiptRepository.Setup(x => x.GetInsuranceReceInfList(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>())).Returns((int input1, int input2, int input3, int input4, long input5) => insuranceReceInfModel);

            // Act
            var result = getInsuranceReceInfListInteractor.Handle(inputData);

            // Assert
            Assert.That(result.InsuranceReceInf.InsuranceName == "アフターケア" && result.Status == GetInsuranceReceInfListStatus.Successed);
        }

        /// <summary>
        /// hokenKbn == 14
        /// </summary>
        [Test]
        public void TC_008_GetInsuranceReceInfListInteractor_Handle_HokenKbn_14()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();

            var getInsuranceReceInfListInteractor = new GetInsuranceReceInfListInteractor(mockIReceiptRepository.Object, mockIInsuranceRepository.Object);

            int hpId = 99999999, seikyuYm = 200103, sinYm = 200103, hokenId = 0, hokenId2 = 0, hokenSbtCd = 0, hokenPid = 0, hokenKbn = 14;
            int kohi1Id = 0, kohi2Id = 0, kohi3Id = 0, kohi4Id = 0, startDate = 0, endDate = 0, sinDate = 0;
            long ptId = 20010328, seqNo = 999999;
            string receSbt = "1112", hokensyaNo = "", tokki1 = "", tokki2 = "", tokki3 = "", tokki4 = "", tokki5 = "", futansyaNoKohi1 = "", futansyaNoKohi2 = "", futansyaNoKohi3 = "", futansyaNoKohi4 = "", jyukyusyaNoKohi1 = "", jyukyusyaNoKohi2 = "";
            string jyukyusyaNoKohi3 = "", jyukyusyaNoKohi4 = "", hokenInfRousaiKofuNo = "", kigo = "", bango = "", edaNo = "";
            int hokenReceTensu = 0, hokenReceFutan = 0, kohi1ReceTensu = 0, kohi1ReceFutan = 0, kohi1ReceKyufu = 0, kohi2ReceTensu = 0, kohi2ReceFutan = 0, kohi2ReceKyufu = 0, kohi3ReceTensu = 0, kohi3ReceFutan = 0, kohi3ReceKyufu = 0, kohi4ReceTensu = 0;
            int kohi4ReceFutan = 0, kohi4ReceKyufu = 0, hokenNissu = 0, kohi1Nissu = 0, kohi2Nissu = 0, kohi3Nissu = 0, kohi4Nissu = 0, kohi1ReceKisai = 0, kohi2ReceKisai = 0, kohi3ReceKisai = 0, kohi4ReceKisai = 0, rousaiIFutan = 0, rousaiRoFutan = 0;
            int jibaiITensu = 0, jibaiRoTensu = 0, jibaiHaFutan = 0, jibaiNiFutan = 0, jibaiHoSindan = 0, jibaiHeMeisai = 0, jibaiAFutan = 0, jibaiBFutan = 0, jibaiCFutan = 0, jibaiDFutan = 0, jibaiKenpoFutan = 0, hokenPId = 0;

            List<InsuranceModel> listInsurance = new List<InsuranceModel>()
            {
                new InsuranceModel(hpId, ptId, seqNo, hokenSbtCd, hokenPid, hokenKbn, hokenId, kohi1Id, kohi2Id, kohi3Id, kohi4Id, startDate, endDate, sinDate)
            };

            List<HokenInfModel> listHokenInf = new List<HokenInfModel>()
            {
                new HokenInfModel(0, 0, 0)
            };

            List<KohiInfModel> listKohi = new List<KohiInfModel>()
            {
                new KohiInfModel(hokenId)
            };

            InsuranceDataModel insuranceDataModel = new InsuranceDataModel(listInsurance, listHokenInf, listKohi, 12, 13, 14);
            InsuranceReceInfModel insuranceReceInfModel = new InsuranceReceInfModel(seikyuYm, ptId, sinYm, hokenId, hokenId2, kohi1Id, kohi2Id, kohi3Id, kohi4Id, hokenKbn, receSbt, hokensyaNo, hokenReceTensu,
                                                                                    hokenReceFutan, kohi1ReceTensu, kohi1ReceFutan, kohi1ReceKyufu, kohi2ReceTensu, kohi2ReceFutan, kohi2ReceKyufu, kohi3ReceTensu,
                                                                                    kohi3ReceFutan, kohi3ReceKyufu, kohi4ReceTensu, kohi4ReceFutan, kohi4ReceKyufu, hokenNissu, kohi1Nissu, kohi2Nissu, kohi3Nissu,
                                                                                    kohi4Nissu, kohi1ReceKisai, kohi2ReceKisai, kohi3ReceKisai, kohi4ReceKisai, tokki1, tokki2, tokki3, tokki4, tokki5, rousaiIFutan,
                                                                                    rousaiRoFutan, jibaiITensu, jibaiRoTensu, jibaiHaFutan, jibaiNiFutan, jibaiHoSindan, jibaiHeMeisai, jibaiAFutan, jibaiBFutan, jibaiCFutan,
                                                                                    jibaiDFutan, jibaiKenpoFutan, futansyaNoKohi1, futansyaNoKohi2, futansyaNoKohi3, futansyaNoKohi4, jyukyusyaNoKohi1, jyukyusyaNoKohi2, jyukyusyaNoKohi3,
                                                                                    jyukyusyaNoKohi4, hokenInfRousaiKofuNo, kigo, bango, edaNo, hokenPId);
            GetInsuranceReceInfListInputData inputData = new GetInsuranceReceInfListInputData(hpId, seikyuYm, sinYm, ptId, hokenId);
            mockIInsuranceRepository.Setup(x => x.GetInsuranceListById(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns((int input1, long input2, int input3, bool input4, bool input5) => insuranceDataModel);
            mockIReceiptRepository.Setup(x => x.GetInsuranceReceInfList(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>())).Returns((int input1, int input2, int input3, int input4, long input5) => insuranceReceInfModel);

            // Act
            var result = getInsuranceReceInfListInteractor.Handle(inputData);

            // Assert
            Assert.That(result.InsuranceReceInf.InsuranceName == "自賠責" && result.Status == GetInsuranceReceInfListStatus.Successed);
        }

        /// <summary>
        /// receSbt[2] != {0,1}
        /// </summary>
        [Test]
        public void TC_009_GetInsuranceReceInfListInteractor_Handle()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();

            var getInsuranceReceInfListInteractor = new GetInsuranceReceInfListInteractor(mockIReceiptRepository.Object, mockIInsuranceRepository.Object);

            int hpId = 99999999, seikyuYm = 200103, sinYm = 200103, hokenId = 0, hokenId2 = 0, hokenSbtCd = 0, hokenPid = 0, hokenKbn = 0;
            int kohi1Id = 1, kohi2Id = 0, kohi3Id = 0, kohi4Id = 0, startDate = 0, endDate = 0, sinDate = 0;
            long ptId = 20010328, seqNo = 999999;
            string receSbt = "9828", hokensyaNo = "", tokki1 = "", tokki2 = "", tokki3 = "", tokki4 = "", tokki5 = "", futansyaNoKohi1 = "", futansyaNoKohi2 = "", futansyaNoKohi3 = "", futansyaNoKohi4 = "", jyukyusyaNoKohi1 = "", jyukyusyaNoKohi2 = "";
            string jyukyusyaNoKohi3 = "", jyukyusyaNoKohi4 = "", hokenInfRousaiKofuNo = "", kigo = "", bango = "", edaNo = "";
            int hokenReceTensu = 0, hokenReceFutan = 0, kohi1ReceTensu = 0, kohi1ReceFutan = 0, kohi1ReceKyufu = 0, kohi2ReceTensu = 0, kohi2ReceFutan = 0, kohi2ReceKyufu = 0, kohi3ReceTensu = 0, kohi3ReceFutan = 0, kohi3ReceKyufu = 0, kohi4ReceTensu = 0;
            int kohi4ReceFutan = 0, kohi4ReceKyufu = 0, hokenNissu = 0, kohi1Nissu = 0, kohi2Nissu = 0, kohi3Nissu = 0, kohi4Nissu = 0, kohi1ReceKisai = 0, kohi2ReceKisai = 0, kohi3ReceKisai = 0, kohi4ReceKisai = 0, rousaiIFutan = 0, rousaiRoFutan = 0;
            int jibaiITensu = 0, jibaiRoTensu = 0, jibaiHaFutan = 0, jibaiNiFutan = 0, jibaiHoSindan = 0, jibaiHeMeisai = 0, jibaiAFutan = 0, jibaiBFutan = 0, jibaiCFutan = 0, jibaiDFutan = 0, jibaiKenpoFutan = 0, hokenPId = 0;

            List<InsuranceModel> listInsurance = new List<InsuranceModel>()
            {
                new InsuranceModel(hpId, ptId, seqNo, hokenSbtCd, hokenPid, hokenKbn, hokenId, kohi1Id, kohi2Id, kohi3Id, kohi4Id, startDate, endDate, sinDate)
            };

            List<HokenInfModel> listHokenInf = new List<HokenInfModel>()
            {
                new HokenInfModel(0, 0, 0)
            };

            List<KohiInfModel> listKohi = new List<KohiInfModel>()
            {
                new KohiInfModel(hokenId)
            };

            InsuranceDataModel insuranceDataModel = new InsuranceDataModel(listInsurance, listHokenInf, listKohi, 12, 13, 14);
            InsuranceReceInfModel insuranceReceInfModel = new InsuranceReceInfModel(seikyuYm, ptId, sinYm, hokenId, hokenId2, kohi1Id, kohi2Id, kohi3Id, kohi4Id, hokenKbn, receSbt, hokensyaNo, hokenReceTensu,
                                                                                    hokenReceFutan, kohi1ReceTensu, kohi1ReceFutan, kohi1ReceKyufu, kohi2ReceTensu, kohi2ReceFutan, kohi2ReceKyufu, kohi3ReceTensu,
                                                                                    kohi3ReceFutan, kohi3ReceKyufu, kohi4ReceTensu, kohi4ReceFutan, kohi4ReceKyufu, hokenNissu, kohi1Nissu, kohi2Nissu, kohi3Nissu,
                                                                                    kohi4Nissu, kohi1ReceKisai, kohi2ReceKisai, kohi3ReceKisai, kohi4ReceKisai, tokki1, tokki2, tokki3, tokki4, tokki5, rousaiIFutan,
                                                                                    rousaiRoFutan, jibaiITensu, jibaiRoTensu, jibaiHaFutan, jibaiNiFutan, jibaiHoSindan, jibaiHeMeisai, jibaiAFutan, jibaiBFutan, jibaiCFutan,
                                                                                    jibaiDFutan, jibaiKenpoFutan, futansyaNoKohi1, futansyaNoKohi2, futansyaNoKohi3, futansyaNoKohi4, jyukyusyaNoKohi1, jyukyusyaNoKohi2, jyukyusyaNoKohi3,
                                                                                    jyukyusyaNoKohi4, hokenInfRousaiKofuNo, kigo, bango, edaNo, hokenPId);
            GetInsuranceReceInfListInputData inputData = new GetInsuranceReceInfListInputData(hpId, seikyuYm, sinYm, ptId, hokenId);
            mockIInsuranceRepository.Setup(x => x.GetInsuranceListById(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns((int input1, long input2, int input3, bool input4, bool input5) => insuranceDataModel);
            mockIReceiptRepository.Setup(x => x.GetInsuranceReceInfList(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>())).Returns((int input1, int input2, int input3, int input4, long input5) => insuranceReceInfModel);

            // Act
            var result = getInsuranceReceInfListInteractor.Handle(inputData);

            // Assert
            Assert.That(result.InsuranceReceInf.InsuranceName == "自費レセ２併" && result.Status == GetInsuranceReceInfListStatus.Successed);
        }

        /// <summary>
        /// receSbt.Length != 4
        /// </summary>
        [Test]
        public void TC_010_GetInsuranceReceInfListInteractor_Handle()
        {
            //Arrange
            var mockIReceiptRepository = new Mock<IReceiptRepository>();
            var mockIInsuranceRepository = new Mock<IInsuranceRepository>();

            var getInsuranceReceInfListInteractor = new GetInsuranceReceInfListInteractor(mockIReceiptRepository.Object, mockIInsuranceRepository.Object);

            int hpId = 99999999, seikyuYm = 200103, sinYm = 200103, hokenId = 0, hokenId2 = 0, hokenSbtCd = 0, hokenPid = 0, hokenKbn = 0;
            int kohi1Id = 1, kohi2Id = 0, kohi3Id = 0, kohi4Id = 0, startDate = 0, endDate = 0, sinDate = 0;
            long ptId = 20010328, seqNo = 999999;
            string receSbt = "98286", hokensyaNo = "", tokki1 = "", tokki2 = "", tokki3 = "", tokki4 = "", tokki5 = "", futansyaNoKohi1 = "", futansyaNoKohi2 = "", futansyaNoKohi3 = "", futansyaNoKohi4 = "", jyukyusyaNoKohi1 = "", jyukyusyaNoKohi2 = "";
            string jyukyusyaNoKohi3 = "", jyukyusyaNoKohi4 = "", hokenInfRousaiKofuNo = "", kigo = "", bango = "", edaNo = "";
            int hokenReceTensu = 0, hokenReceFutan = 0, kohi1ReceTensu = 0, kohi1ReceFutan = 0, kohi1ReceKyufu = 0, kohi2ReceTensu = 0, kohi2ReceFutan = 0, kohi2ReceKyufu = 0, kohi3ReceTensu = 0, kohi3ReceFutan = 0, kohi3ReceKyufu = 0, kohi4ReceTensu = 0;
            int kohi4ReceFutan = 0, kohi4ReceKyufu = 0, hokenNissu = 0, kohi1Nissu = 0, kohi2Nissu = 0, kohi3Nissu = 0, kohi4Nissu = 0, kohi1ReceKisai = 0, kohi2ReceKisai = 0, kohi3ReceKisai = 0, kohi4ReceKisai = 0, rousaiIFutan = 0, rousaiRoFutan = 0;
            int jibaiITensu = 0, jibaiRoTensu = 0, jibaiHaFutan = 0, jibaiNiFutan = 0, jibaiHoSindan = 0, jibaiHeMeisai = 0, jibaiAFutan = 0, jibaiBFutan = 0, jibaiCFutan = 0, jibaiDFutan = 0, jibaiKenpoFutan = 0, hokenPId = 0;

            List<InsuranceModel> listInsurance = new List<InsuranceModel>()
            {
                new InsuranceModel(hpId, ptId, seqNo, hokenSbtCd, hokenPid, hokenKbn, hokenId, kohi1Id, kohi2Id, kohi3Id, kohi4Id, startDate, endDate, sinDate)
            };

            List<HokenInfModel> listHokenInf = new List<HokenInfModel>()
            {
                new HokenInfModel(0, 0, 0)
            };

            List<KohiInfModel> listKohi = new List<KohiInfModel>()
            {
                new KohiInfModel(hokenId)
            };

            InsuranceDataModel insuranceDataModel = new InsuranceDataModel(listInsurance, listHokenInf, listKohi, 12, 13, 14);
            InsuranceReceInfModel insuranceReceInfModel = new InsuranceReceInfModel(seikyuYm, ptId, sinYm, hokenId, hokenId2, kohi1Id, kohi2Id, kohi3Id, kohi4Id, hokenKbn, receSbt, hokensyaNo, hokenReceTensu,
                                                                                    hokenReceFutan, kohi1ReceTensu, kohi1ReceFutan, kohi1ReceKyufu, kohi2ReceTensu, kohi2ReceFutan, kohi2ReceKyufu, kohi3ReceTensu,
                                                                                    kohi3ReceFutan, kohi3ReceKyufu, kohi4ReceTensu, kohi4ReceFutan, kohi4ReceKyufu, hokenNissu, kohi1Nissu, kohi2Nissu, kohi3Nissu,
                                                                                    kohi4Nissu, kohi1ReceKisai, kohi2ReceKisai, kohi3ReceKisai, kohi4ReceKisai, tokki1, tokki2, tokki3, tokki4, tokki5, rousaiIFutan,
                                                                                    rousaiRoFutan, jibaiITensu, jibaiRoTensu, jibaiHaFutan, jibaiNiFutan, jibaiHoSindan, jibaiHeMeisai, jibaiAFutan, jibaiBFutan, jibaiCFutan,
                                                                                    jibaiDFutan, jibaiKenpoFutan, futansyaNoKohi1, futansyaNoKohi2, futansyaNoKohi3, futansyaNoKohi4, jyukyusyaNoKohi1, jyukyusyaNoKohi2, jyukyusyaNoKohi3,
                                                                                    jyukyusyaNoKohi4, hokenInfRousaiKofuNo, kigo, bango, edaNo, hokenPId);
            GetInsuranceReceInfListInputData inputData = new GetInsuranceReceInfListInputData(hpId, seikyuYm, sinYm, ptId, hokenId);
            mockIInsuranceRepository.Setup(x => x.GetInsuranceListById(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<int>(), It.IsAny<bool>(), It.IsAny<bool>())).Returns((int input1, long input2, int input3, bool input4, bool input5) => insuranceDataModel);
            mockIReceiptRepository.Setup(x => x.GetInsuranceReceInfList(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<long>())).Returns((int input1, int input2, int input3, int input4, long input5) => insuranceReceInfModel);

            // Act
            var result = getInsuranceReceInfListInteractor.Handle(inputData);

            // Assert
            Assert.That(result.InsuranceReceInf.InsuranceName == "ー" && result.Status == GetInsuranceReceInfListStatus.Successed);
        }
    }
}
