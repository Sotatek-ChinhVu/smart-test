using DocumentFormat.OpenXml.Office2010.Excel;
using Domain.Models.AccountDue;
using Domain.Models.PatientInfor;
using Domain.Models.Reception;
using Entity.Tenant;
using Interactor.AccountDue;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Runtime.InteropServices;
using UseCase.AccountDue.GetAccountDueList;

namespace CloudUnitTest.Interactor.AccountDue
{
    public class GetAccountDueListInteractorTest : BaseUT
    {
        [Test]
        public void TC_001_GetAccountDueListInteractor_Handle_InvalidHpId()
        {
            //Arrange
            var mockIAccountDueRepository = new Mock<IAccountDueRepository>();
            var mockIReceptionRepository = new Mock<IReceptionRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();

            var getAccountDueListInteractor = new GetAccountDueListInteractor(mockIAccountDueRepository.Object, mockIReceptionRepository.Object, mockIPatientInforRepository.Object);
            
            int hpId = -1;
            long ptId = 280301;
            int sinDate = 20010328;
            bool isUnpaidChecked = false;
            var inputData = new GetAccountDueListInputData(hpId, ptId, sinDate, isUnpaidChecked);

            // Act
            var result = getAccountDueListInteractor.Handle(inputData);

            // Assert
            Assert.That(result.Status == GetAccountDueListStatus.InvalidHpId);
        }

        [Test]
        public void TC_002_GetAccountDueListInteractor_Handle_InvalidPtId()
        {
            //Arrange
            var mockIAccountDueRepository = new Mock<IAccountDueRepository>();
            var mockIReceptionRepository = new Mock<IReceptionRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();

            var getAccountDueListInteractor = new GetAccountDueListInteractor(mockIAccountDueRepository.Object, mockIReceptionRepository.Object, mockIPatientInforRepository.Object);

            int hpId = 1;
            long ptId = 280301;
            int sinDate = 20010328;
            bool isUnpaidChecked = false;
            var inputData = new GetAccountDueListInputData(hpId, ptId, sinDate, isUnpaidChecked);

            // Act
            var result = getAccountDueListInteractor.Handle(inputData);

            // Assert
            Assert.That(result.Status == GetAccountDueListStatus.InvalidPtId);
        }

        [Test]
        public void TC_003_GetAccountDueListInteractor_Handle_InvalidSindate()
        {
            //Arrange
            var mockIAccountDueRepository = new Mock<IAccountDueRepository>();
            var mockIReceptionRepository = new Mock<IReceptionRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();

            var getAccountDueListInteractor = new GetAccountDueListInteractor(mockIAccountDueRepository.Object, mockIReceptionRepository.Object, mockIPatientInforRepository.Object);

            var tennal = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 1;
            long ptId = 99999999;
            long seqNo = 99999999;
            int sinDate = 2001032;
            bool isUnpaidChecked = false;
            var inputData = new GetAccountDueListInputData(hpId, ptId, sinDate, isUnpaidChecked);
            var ptIds = new List<long>();
            ptIds.Add(ptId);
            mockIPatientInforRepository.Setup(finder => finder.CheckExistIdList(hpId, ptIds))
            .Returns((int hpId, List<long> ptIds) => true);

            PtInf ptInf = new PtInf()
            {
                HpId = hpId,
                PtId = ptId,
                SeqNo = seqNo,
            };

            tennal.Add(ptInf);

            try
            {
                // Act
                tennal.SaveChanges();
                var result = getAccountDueListInteractor.Handle(inputData);

                // Assert
                Assert.That(result.Status == GetAccountDueListStatus.InvalidSindate);
            }
            finally
            {
                tennal.PtInfs.Remove(ptInf);
                tennal.SaveChanges();
            }
        }

        [Test]
        public void TC_004_GetAccountDueListInteractor_Handle_NyukinKbn()
        {
            //Arrange
            var mockIAccountDueRepository = new Mock<IAccountDueRepository>();
            var mockIReceptionRepository = new Mock<IReceptionRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();

            var getAccountDueListInteractor = new GetAccountDueListInteractor(mockIAccountDueRepository.Object, mockIReceptionRepository.Object, mockIPatientInforRepository.Object);

            Random random = new Random();
            var tennal = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 99999999;
            long ptId = random.Next(999, 99999);
            int sinDate = 20010328;
            long raiinNo = random.Next(999, 99999);
            long seqNo = random.Next(999, 99999);
            int status = random.Next(999, 99999);
            long id = random.Next(999, 99999);
            bool isUnpaidChecked = true;
            var inputData = new GetAccountDueListInputData(hpId, ptId, sinDate, isUnpaidChecked);
            var ptIds = new List<long>();
            Dictionary<int, string> getUketsukeSbt = new();
            Dictionary<int, string> getPaymentMethod = new();
            List<AccountDueModel> listAccountDues = new List<AccountDueModel>()
            {
                new AccountDueModel(hpId, ptId, sinDate, 200103, raiinNo, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, "", 0, 0, "", 0, 0, "", 0, 0, "", 0)
            };

            List<ReceptionRowModel> hokenPatternList = new List<ReceptionRowModel>()
            {
                new ReceptionRowModel(raiinNo, ptId, sinDate, 0)
            };

            var uketukeList = tennal.UketukeSbtMsts.Where(item => item.HpId == hpId && item.IsDeleted == 0).OrderBy(p => p.SortNo).ToList();
            var paymentMethodList = tennal.PaymentMethodMsts.Where(item => item.HpId == hpId).OrderBy(item => item.SortNo).ToList();

            foreach (var uketuke in uketukeList)
            {
                getUketsukeSbt.Add(uketuke.KbnId, uketuke.KbnName ?? string.Empty);
            }

            foreach (var paymentMethod in paymentMethodList)
            {
                getPaymentMethod.Add(paymentMethod.PaymentMethodCd, paymentMethod.PayName ?? string.Empty);
            }

            ptIds.Add(ptId);

            mockIPatientInforRepository.Setup(finder => finder.CheckExistIdList(hpId, ptIds))
            .Returns((int hpId, List<long> ptIds) => true);
            mockIAccountDueRepository.Setup(finder => finder.GetUketsukeSbt(hpId))
            .Returns((int hpId) => getUketsukeSbt);
            mockIAccountDueRepository.Setup(finder => finder.GetPaymentMethod(hpId))
            .Returns((int hpId) => getPaymentMethod);
            mockIAccountDueRepository.Setup(finder => finder.GetAccountDueList(hpId, ptId, sinDate, isUnpaidChecked))
            .Returns((int hpId, long ptId, int sinDate, bool isUnpaidChecked) => listAccountDues);
            mockIReceptionRepository.Setup(finder => finder.GetList(hpId, sinDate, -1, ptId, true, false, 2, false))
            .Returns((int hpId, int sinDate, long raiinNo, long ptId, [Optional] bool isGetAccountDue, [Optional] bool isGetFamily, int isDeleted, bool searchSameVisit) => hokenPatternList);

            PtInf ptInf = new PtInf()
            {
                HpId = hpId,
                PtId = ptId,
                SeqNo = seqNo,
            };
            SyunoSeikyu syunoSeikyu = new SyunoSeikyu()
            {
                HpId = hpId,
                PtId = ptId,
                SinDate = sinDate,
                RaiinNo = raiinNo
            };

            SyunoNyukin syunoNyukin = new SyunoNyukin()
            {
                HpId = hpId,
                PtId = ptId,
                RaiinNo = raiinNo,
                SeqNo = seqNo
            };

            RaiinInf raiinInf = new RaiinInf()
            {
                HpId = hpId,
                RaiinNo = raiinNo,
                PtId = ptId,
                Status = status
            };

            KaMst kaMst = new KaMst()
            {
                HpId = hpId,
                Id = id
            };

            tennal.Add(syunoSeikyu);
            tennal.Add(syunoNyukin);
            tennal.Add(raiinInf);
            tennal.Add(kaMst);
            tennal.Add(ptInf);

            try
            {
                // Act
                tennal.SaveChanges();
                var result = getAccountDueListInteractor.Handle(inputData);

                // Assert
                Assert.That(result.AccountDueModel.AccountDueList.Any());
            }
            finally
            {
                tennal.SyunoSeikyus.Remove(syunoSeikyu);
                tennal.SyunoNyukin.Remove(syunoNyukin);
                tennal.RaiinInfs.Remove(raiinInf);
                tennal.KaMsts.Remove(kaMst);
                tennal.PtInfs.Remove(ptInf);
                tennal.SaveChanges();
            }
        }

        [Test]
        public void TC_005_GetAccountDueListInteractor_Handle_Equals_RaiinNo()
        {
            //Arrange
            var mockIAccountDueRepository = new Mock<IAccountDueRepository>();
            var mockIReceptionRepository = new Mock<IReceptionRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();

            var getAccountDueListInteractor = new GetAccountDueListInteractor(mockIAccountDueRepository.Object, mockIReceptionRepository.Object, mockIPatientInforRepository.Object);

            Random random = new Random();
            var tennal = TenantProvider.GetTrackingTenantDataContext();

            int hpId = 99999999;
            long ptId = random.Next(999, 99999);
            int sinDate = 20010328;
            long raiinNo = 0;
            long seqNo = random.Next(999, 99999);
            int status = random.Next(999, 99999);
            long id = random.Next(999, 99999);
            bool isUnpaidChecked = true;
            var inputData = new GetAccountDueListInputData(hpId, ptId, sinDate, isUnpaidChecked);
            var ptIds = new List<long>();
            Dictionary<int, string> getUketsukeSbt = new();
            Dictionary<int, string> getPaymentMethod = new();
            List<AccountDueModel> listAccountDues = new List<AccountDueModel>()
            {
                new AccountDueModel(hpId, ptId, sinDate, 200103, raiinNo, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, "", 0, 0, "", 0, 0, "", 0, 0, "", 0)
            };

            List<ReceptionRowModel> hokenPatternList = new List<ReceptionRowModel>()
            {
                new ReceptionRowModel(raiinNo, ptId, sinDate, 0)
            };

            var uketukeList = tennal.UketukeSbtMsts.Where(item => item.HpId == hpId && item.IsDeleted == 0).OrderBy(p => p.SortNo).ToList();
            var paymentMethodList = tennal.PaymentMethodMsts.Where(item => item.HpId == hpId).OrderBy(item => item.SortNo).ToList();

            foreach (var uketuke in uketukeList)
            {
                getUketsukeSbt.Add(uketuke.KbnId, uketuke.KbnName ?? string.Empty);
            }

            foreach (var paymentMethod in paymentMethodList)
            {
                getPaymentMethod.Add(paymentMethod.PaymentMethodCd, paymentMethod.PayName ?? string.Empty);
            }

            ptIds.Add(ptId);

            mockIPatientInforRepository.Setup(finder => finder.CheckExistIdList(hpId, ptIds))
            .Returns((int hpId, List<long> ptIds) => true);
            mockIAccountDueRepository.Setup(finder => finder.GetUketsukeSbt(hpId))
            .Returns((int hpId) => getUketsukeSbt);
            mockIAccountDueRepository.Setup(finder => finder.GetPaymentMethod(hpId))
            .Returns((int hpId) => getPaymentMethod);
            mockIAccountDueRepository.Setup(finder => finder.GetAccountDueList(hpId, ptId, sinDate, isUnpaidChecked))
            .Returns((int hpId, long ptId, int sinDate, bool isUnpaidChecked) => listAccountDues);
            mockIReceptionRepository.Setup(finder => finder.GetList(hpId, sinDate, -1, ptId, true, false, 2, false))
            .Returns((int hpId, int sinDate, long raiinNo, long ptId, [Optional] bool isGetAccountDue, [Optional] bool isGetFamily, int isDeleted, bool searchSameVisit) => hokenPatternList);

            PtInf ptInf = new PtInf()
            {
                HpId = hpId,
                PtId = ptId,
                SeqNo = seqNo,
            };
            SyunoSeikyu syunoSeikyu = new SyunoSeikyu()
            {
                HpId = hpId,
                PtId = ptId,
                SinDate = sinDate,
                RaiinNo = raiinNo
            };

            SyunoNyukin syunoNyukin = new SyunoNyukin()
            {
                HpId = hpId,
                PtId = ptId,
                RaiinNo = raiinNo,
                SeqNo = seqNo
            };

            RaiinInf raiinInf = new RaiinInf()
            {
                HpId = hpId,
                RaiinNo = raiinNo,
                PtId = ptId,
                Status = status
            };

            KaMst kaMst = new KaMst()
            {
                HpId = hpId,
                Id = id
            };

            tennal.Add(syunoSeikyu);
            tennal.Add(syunoNyukin);
            tennal.Add(raiinInf);
            tennal.Add(kaMst);
            tennal.Add(ptInf);

            try
            {
                // Act
                tennal.SaveChanges();
                var result = getAccountDueListInteractor.Handle(inputData);

                // Assert
                Assert.That(result.AccountDueModel.AccountDueList.Any());
            }
            finally
            {
                tennal.SyunoSeikyus.Remove(syunoSeikyu);
                tennal.SyunoNyukin.Remove(syunoNyukin);
                tennal.RaiinInfs.Remove(raiinInf);
                tennal.KaMsts.Remove(kaMst);
                tennal.PtInfs.Remove(ptInf);
                tennal.SaveChanges();
            }
        }

        [Test]
        public void TC_006_GetAccountDueListInteractor_Handle_NotEquals_RaiinNo()
        {
            //Arrange
            var mockIAccountDueRepository = new Mock<IAccountDueRepository>();
            var mockIReceptionRepository = new Mock<IReceptionRepository>();
            var mockIPatientInforRepository = new Mock<IPatientInforRepository>();

            var getAccountDueListInteractor = new GetAccountDueListInteractor(mockIAccountDueRepository.Object, mockIReceptionRepository.Object, mockIPatientInforRepository.Object);

            Random random = new Random();
            var tennal = TenantProvider.GetTrackingTenantDataContext();
            int hpId = 99999999;
            long ptId = random.Next(999, 99999);
            int sinDate = 20010328;
            long raiinNo = random.Next(999, 99999);
            long seqNo = random.Next(999, 99999);
            int status = random.Next(999, 99999);
            long id = random.Next(999, 99999);
            bool isUnpaidChecked = true;
            var inputData = new GetAccountDueListInputData(hpId, ptId, sinDate, isUnpaidChecked);
            var ptIds = new List<long>();
            Dictionary<int, string> getUketsukeSbt = new();
            Dictionary<int, string> getPaymentMethod = new();

            List<AccountDueModel> listAccountDues = new List<AccountDueModel>()
            {
                new AccountDueModel(hpId, ptId, sinDate, 200103, raiinNo, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, "", 0, 0, "", 0, 0, "", 0, 0, "", 0)
            };

            List<ReceptionRowModel> hokenPatternList = new List<ReceptionRowModel>()
            {
                new ReceptionRowModel(raiinNo, ptId, sinDate, 0)
            };

            var uketukeList = tennal.UketukeSbtMsts.Where(item => item.HpId == hpId && item.IsDeleted == 0).OrderBy(p => p.SortNo).ToList();
            var paymentMethodList = tennal.PaymentMethodMsts.Where(item => item.HpId == hpId).OrderBy(item => item.SortNo).ToList();

            foreach (var uketuke in uketukeList)
            {
                getUketsukeSbt.Add(uketuke.KbnId, uketuke.KbnName ?? string.Empty);
            }

            foreach (var paymentMethod in paymentMethodList)
            {
                getPaymentMethod.Add(paymentMethod.PaymentMethodCd, paymentMethod.PayName ?? string.Empty);
            }

            ptIds.Add(ptId);

            mockIPatientInforRepository.Setup(finder => finder.CheckExistIdList(hpId, ptIds))
            .Returns((int hpId, List<long> ptIds) => true);
            mockIAccountDueRepository.Setup(finder => finder.GetUketsukeSbt(hpId))
            .Returns((int hpId) => getUketsukeSbt);
            mockIAccountDueRepository.Setup(finder => finder.GetPaymentMethod(hpId))
            .Returns((int hpId) => getPaymentMethod);
            mockIAccountDueRepository.Setup(finder => finder.GetAccountDueList(hpId, ptId, sinDate, isUnpaidChecked))
            .Returns((int hpId, long ptId, int sinDate, bool isUnpaidChecked) => listAccountDues);
            mockIReceptionRepository.Setup(finder => finder.GetList(hpId, sinDate, -1, ptId, true, false, 2, false))
            .Returns((int hpId, int sinDate, long raiinNo, long ptId, [Optional] bool isGetAccountDue, [Optional] bool isGetFamily, int isDeleted, bool searchSameVisit) => hokenPatternList);

            PtInf ptInf = new PtInf()
            {
                HpId = hpId,
                PtId = ptId,
                SeqNo = seqNo,
            };
            SyunoSeikyu syunoSeikyu = new SyunoSeikyu()
            {
                HpId = hpId,
                PtId = ptId,
                SinDate = sinDate,
                RaiinNo = raiinNo
            };

            SyunoNyukin syunoNyukin = new SyunoNyukin()
            {
                HpId = hpId,
                PtId = ptId,
                RaiinNo = raiinNo,
                SeqNo = seqNo
            };

            RaiinInf raiinInf = new RaiinInf()
            {
                HpId = hpId,
                RaiinNo = raiinNo,
                PtId = ptId,
                Status = status
            };

            KaMst kaMst = new KaMst()
            {
                HpId = hpId,
                Id = id
            };

            tennal.Add(syunoSeikyu);
            tennal.Add(syunoNyukin);
            tennal.Add(raiinInf);
            tennal.Add(kaMst);
            tennal.Add(ptInf);

            try
            {
                // Act
                tennal.SaveChanges();
                var result = getAccountDueListInteractor.Handle(inputData);

                // Assert
                Assert.That(result.AccountDueModel.AccountDueList.Any());
            }
            finally
            {
                tennal.SyunoSeikyus.Remove(syunoSeikyu);
                tennal.SyunoNyukin.Remove(syunoNyukin);
                tennal.RaiinInfs.Remove(raiinInf);
                tennal.KaMsts.Remove(kaMst);
                tennal.PtInfs.Remove(ptInf);
                tennal.SaveChanges();
            }
        }
    }
}
