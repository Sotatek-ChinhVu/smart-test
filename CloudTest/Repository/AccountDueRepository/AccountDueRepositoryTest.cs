using Domain.Models.AccountDue;
using Entity.Tenant;
using Infrastructure.Repositories;

namespace CloudUnitTest.Repository.AccountDueRepo
{
    public class AccountDueRepositoryTest : BaseUT
    {
        [Test]
        public void TC_001_AccountDueRepository_GetUketsukeSbt()
        {
            //Arrange
            AccountDueRepository accountDueRepository = new AccountDueRepository(TenantProvider);
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            Random random = new Random();
            int hpId = random.Next(999, 99999);
            int kbnId = random.Next(999, 99999);

            UketukeSbtMst uketukeSbtMst = new UketukeSbtMst()
            {
                HpId = hpId,
                KbnId = kbnId,
                IsDeleted = 0
            };

            tenantTracking.Add(uketukeSbtMst);

            try
            {
                // Act
                tenantTracking.SaveChanges();
                var result = accountDueRepository.GetUketsukeSbt(hpId);

                // Assert
                Assert.That(result.Count() > 0 && result.Any(x => x.Key == kbnId));
            }
            finally
            {
                accountDueRepository.ReleaseResource();
                tenantTracking.Remove(uketukeSbtMst);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_002_AccountDueRepository_GetAccountDueList()
        {
            //Arrange
            AccountDueRepository accountDueRepository = new AccountDueRepository(TenantProvider);
            var tenantTracking = TenantProvider.GetNoTrackingDataContext();
            Random random = new Random();

            int hpId = random.Next(999, 99999);
            long ptId = random.Next(999, 99999);
            int sinDate = random.Next(999, 99999);
            long raiinNo = random.Next(999, 99999);
            long seqNo = random.Next(999, 99999);
            int status = random.Next(999, 99999);
            long id = random.Next(999, 99999);
            bool isUnpaidChecked = true;

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

            tenantTracking.Add(syunoSeikyu);
            tenantTracking.Add(syunoNyukin);
            tenantTracking.Add(raiinInf);
            tenantTracking.Add(kaMst);

            try
            {
                // Act
                tenantTracking.SaveChanges();
                var result = accountDueRepository.GetAccountDueList(hpId, ptId, sinDate, isUnpaidChecked);

                // Assert
                Assert.That(result.Count() > 0);
            }
            finally
            {
                accountDueRepository.ReleaseResource();
                tenantTracking.SyunoSeikyus.Remove(syunoSeikyu);
                tenantTracking.SyunoNyukin.Remove(syunoNyukin);
                tenantTracking.RaiinInfs.Remove(raiinInf);
                tenantTracking.KaMsts.Remove(kaMst);
                tenantTracking.SaveChanges();
            }

        }

        [Test]
        public void TC_003_AccountDueRepository_SaveAccountDueList()
        {
            //Arrange
            var accountDueRepository = new AccountDueRepository(TenantProvider);
            Random random = new Random();

            int hpId = 999999, status = random.Next(999, 99999), userId = random.Next(999, 99999), sinDate = random.Next(999, 99999), seikyuSinDate = 20240405, month = 4, hokenPid = 0, nyukinKbn = 1;
            long ptId = random.Next(999, 99999), raiinNo = random.Next(999, 99999), oyaRaiinNo = 0, seqNo = 0;
            string kaikeiTime = "kaito", nyukinCmt = "", kaDisplay = "", hokenPatternName = "", seikyuDetail = "";
            int seikyuTensu = 0, seikyuGaku = 1, adjustFutan = 0, nyukinGaku = 0, paymentMethodCd = 0, nyukinDate = 0, uketukeSbt = 0;
            int newSeikyuGaku = 0, newAdjustFutan = 0, sortNo = 0, seikyuAdjustFutan = 1, unPaid = 0;
            bool isSeikyuRow = true;
            bool isDelete = false;

            var tennal = TenantProvider.GetTrackingTenantDataContext();

            List<AccountDueModel> listAccountDues = new List<AccountDueModel>()
            {
                new AccountDueModel(hpId, ptId, seikyuSinDate, month, raiinNo, hokenPid, oyaRaiinNo, nyukinKbn, seikyuTensu, seikyuGaku, adjustFutan,
                                    nyukinGaku, paymentMethodCd, nyukinDate, uketukeSbt, nyukinCmt, unPaid, newSeikyuGaku, newAdjustFutan, kaDisplay,
                                    hokenPatternName, isSeikyuRow, sortNo, seqNo, seikyuDetail, seikyuAdjustFutan, isDelete)
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

            tennal.Add(syunoSeikyu);
            tennal.Add(syunoNyukin);
            tennal.Add(raiinInf);
            List<SyunoNyukin> syunoNyukins = new List<SyunoNyukin>();

            try
            {
                // Act
                tennal.SaveChanges();
                var result = accountDueRepository.SaveAccountDueList(hpId, ptId, userId, sinDate, listAccountDues, kaikeiTime);
                syunoNyukins = tennal.SyunoNyukin.Where(x => x.HpId == hpId && x.RaiinNo == raiinNo).ToList();

                // Assert
                Assert.That(result.Any(x => x.HpId == hpId && x.RaiinNo == raiinNo));
            }
            finally
            {
                accountDueRepository.ReleaseResource();
                tennal.SyunoSeikyus.Remove(syunoSeikyu);
                tennal.SyunoNyukin.RemoveRange(syunoNyukins);
                tennal.RaiinInfs.Remove(raiinInf);
                tennal.SaveChanges();
            }
        }

        [Test]
        public void TC_004_AccountDueRepository_UpdateStatusRaiin_Update_Menjo()
        {
            //Arrange
            var accountDueRepository = new AccountDueRepository(TenantProvider);
            Random random = new Random();

            int hpId = 999999, status = 9, userId = random.Next(999, 99999), sinDate = random.Next(999, 99999), seikyuSinDate = 20240405, month = 4, hokenPid = 0, nyukinKbn = 2;
            long ptId = random.Next(999, 99999), raiinNo = random.Next(999, 99999), oyaRaiinNo = 0, seqNo = 0;
            string kaikeiTime = "kaito", nyukinCmt = "", kaDisplay = "", hokenPatternName = "", seikyuDetail = "";
            int seikyuTensu = 0, seikyuGaku = 1, adjustFutan = 0, nyukinGaku = 0, paymentMethodCd = 0, nyukinDate = 0, uketukeSbt = 0;
            int newSeikyuGaku = 0, newAdjustFutan = 0, sortNo = 0, seikyuAdjustFutan = 1, unPaid = 0;
            bool isSeikyuRow = true;
            bool isDelete = false;

            var tennal = TenantProvider.GetTrackingTenantDataContext();

            List<AccountDueModel> listAccountDues = new List<AccountDueModel>()
            {
                new AccountDueModel(hpId, ptId, seikyuSinDate, month, raiinNo, hokenPid, oyaRaiinNo, nyukinKbn, seikyuTensu, seikyuGaku, adjustFutan,
                                    nyukinGaku, paymentMethodCd, nyukinDate, uketukeSbt, nyukinCmt, unPaid, newSeikyuGaku, newAdjustFutan, kaDisplay,
                                    hokenPatternName, isSeikyuRow, sortNo, seqNo, seikyuDetail, seikyuAdjustFutan, isDelete)
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

            tennal.Add(syunoSeikyu);
            tennal.Add(syunoNyukin);
            tennal.Add(raiinInf);
            List<SyunoNyukin> syunoNyukins = new List<SyunoNyukin>();

            try
            {
                // Act
                tennal.SaveChanges();
                var result = accountDueRepository.SaveAccountDueList(hpId, ptId, userId, sinDate, listAccountDues, kaikeiTime);
                syunoNyukins = tennal.SyunoNyukin.Where(x => x.HpId == hpId && x.RaiinNo == raiinNo).ToList();

                // Assert
                Assert.That(result.Any(x => x.HpId == hpId && x.RaiinNo == raiinNo));
            }
            finally
            {
                accountDueRepository.ReleaseResource();
                tennal.SyunoSeikyus.Remove(syunoSeikyu);
                tennal.SyunoNyukin.RemoveRange(syunoNyukins);
                tennal.RaiinInfs.Remove(raiinInf);
                tennal.SaveChanges();
            }
        }

        [Test]
        public void TC_005_AccountDueRepository_UpdateStatusSyunoSeikyu_Return_null()
        {
            //Arrange
            var accountDueRepository = new AccountDueRepository(TenantProvider);
            Random random = new Random();

            int hpId = 999999, status = 9, userId = random.Next(999, 99999), sinDate = random.Next(999, 99999), seikyuSinDate = 20240405, month = 4, hokenPid = 0, nyukinKbn = 2;
            long ptId = random.Next(999, 99999), raiinNo = random.Next(999, 99999), oyaRaiinNo = 0, seqNo = 1;
            string kaikeiTime = "kaito", nyukinCmt = "", kaDisplay = "", hokenPatternName = "", seikyuDetail = "";
            int seikyuTensu = 0, seikyuGaku = 1, adjustFutan = 0, nyukinGaku = 0, paymentMethodCd = 0, nyukinDate = 0, uketukeSbt = 0;
            int newSeikyuGaku = 0, newAdjustFutan = 0, sortNo = 0, seikyuAdjustFutan = 1, unPaid = 0;
            bool isSeikyuRow = true;
            bool isDelete = true;

            var tennal = TenantProvider.GetTrackingTenantDataContext();

            List<AccountDueModel> listAccountDues = new List<AccountDueModel>()
            {
                new AccountDueModel(hpId, ptId, seikyuSinDate, month, raiinNo, hokenPid, oyaRaiinNo, nyukinKbn, seikyuTensu, seikyuGaku, adjustFutan,
                                    nyukinGaku, paymentMethodCd, nyukinDate, uketukeSbt, nyukinCmt, unPaid, newSeikyuGaku, newAdjustFutan, kaDisplay,
                                    hokenPatternName, isSeikyuRow, sortNo, seqNo, seikyuDetail, seikyuAdjustFutan, isDelete)
            };

            SyunoSeikyu syunoSeikyu = new SyunoSeikyu()
            {
                HpId = hpId,
                PtId = ptId,
                SinDate = sinDate,
                RaiinNo = raiinNo + 1
            };

            SyunoNyukin syunoNyukin = new SyunoNyukin()
            {
                HpId = hpId,
                PtId = ptId,
                RaiinNo = raiinNo,
                SeqNo = seqNo,
                SortNo = sortNo + 1,
            };

            RaiinInf raiinInf = new RaiinInf()
            {
                HpId = hpId,
                RaiinNo = raiinNo,
                PtId = ptId,
                Status = status
            };

            tennal.Add(syunoSeikyu);
            tennal.Add(syunoNyukin);
            tennal.Add(raiinInf);
            List<SyunoNyukin> syunoNyukins = new List<SyunoNyukin>();

            try
            {
                // Act
                tennal.SaveChanges();
                var result = accountDueRepository.SaveAccountDueList(hpId, ptId, userId, sinDate, listAccountDues, kaikeiTime);
                syunoNyukins = tennal.SyunoNyukin.Where(x => x.HpId == hpId && x.RaiinNo == raiinNo).ToList();

                // Assert
                Assert.That(result.Any(x => x.HpId == hpId && x.RaiinNo == raiinNo));
            }
            finally
            {
                accountDueRepository.ReleaseResource();
                tennal.SyunoSeikyus.Remove(syunoSeikyu);
                tennal.SyunoNyukin.RemoveRange(syunoNyukins);
                tennal.RaiinInfs.Remove(raiinInf);
                tennal.SaveChanges();
            }
        }

        [Test]
        public void TC_006_AccountDueRepository_UpdateSyunoNyukin_Return_null()
        {
            //Arrange
            var accountDueRepository = new AccountDueRepository(TenantProvider);
            Random random = new Random();

            int hpId = 999999, status = random.Next(999, 99999), userId = random.Next(999, 99999), sinDate = random.Next(999, 99999), seikyuSinDate = 20240405, month = 4, hokenPid = 0, nyukinKbn = 1;
            long ptId = random.Next(999, 99999), raiinNo = random.Next(999, 99999), oyaRaiinNo = 0, seqNo = 1;
            string kaikeiTime = "kaito", nyukinCmt = "", kaDisplay = "", hokenPatternName = "", seikyuDetail = "";
            int seikyuTensu = 0, seikyuGaku = 1, adjustFutan = 0, nyukinGaku = 0, paymentMethodCd = 0, nyukinDate = 0, uketukeSbt = 0;
            int newSeikyuGaku = 0, newAdjustFutan = 0, sortNo = 0, seikyuAdjustFutan = 1, unPaid = 0;
            bool isSeikyuRow = true;
            bool isDelete = true;

            var tennal = TenantProvider.GetTrackingTenantDataContext();

            List<AccountDueModel> listAccountDues = new List<AccountDueModel>()
            {
                new AccountDueModel(hpId, ptId, seikyuSinDate, month, raiinNo, hokenPid, oyaRaiinNo, nyukinKbn, seikyuTensu, seikyuGaku, adjustFutan,
                                    nyukinGaku, paymentMethodCd, nyukinDate, uketukeSbt, nyukinCmt, unPaid, newSeikyuGaku, newAdjustFutan, kaDisplay,
                                    hokenPatternName, isSeikyuRow, sortNo, seqNo, seikyuDetail, seikyuAdjustFutan, isDelete)
            };

            SyunoSeikyu syunoSeikyu = new SyunoSeikyu()
            {
                HpId = hpId,
                PtId = ptId,
                SinDate = sinDate,
                RaiinNo = raiinNo
            };

            RaiinInf raiinInf = new RaiinInf()
            {
                HpId = hpId,
                RaiinNo = raiinNo,
                PtId = ptId,
                Status = status
            };

            tennal.Add(syunoSeikyu);
            tennal.Add(raiinInf);
            List<SyunoNyukin> syunoNyukins = new List<SyunoNyukin>();

            try
            {
                // Act
                tennal.SaveChanges();
                var result = accountDueRepository.SaveAccountDueList(hpId, ptId, userId, sinDate, listAccountDues, kaikeiTime);
                syunoNyukins = tennal.SyunoNyukin.Where(x => x.HpId == hpId && x.RaiinNo == raiinNo).ToList();

                // Assert
                Assert.That(result.Any(x => x.HpId == hpId && x.RaiinNo == raiinNo));
            }
            finally
            {
                accountDueRepository.ReleaseResource();
                tennal.SyunoSeikyus.Remove(syunoSeikyu);
                tennal.SyunoNyukin.RemoveRange(syunoNyukins);
                tennal.RaiinInfs.Remove(raiinInf);
                tennal.SaveChanges();
            }
        }
    }
}