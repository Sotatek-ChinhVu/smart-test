using CloudUnitTest.SampleData;
using Domain.Models.ChartApproval;
using Domain.Models.MstItem;
using Domain.Models.OrdInfDetails;
using Domain.Models.OrdInfs;
using Domain.Models.SystemConf;
using Infrastructure.Repositories;
using Moq;

namespace CloudUnitTest.MedicalCommon.ChangeAfterAutoCheckOrder
{
    public class ChangeAfterAutoCheckOrderTest : BaseUT
    {
        [Test]
        public void ChangeAfterAutoCheckOrder_001()
        {
            int hpId = 1;
            int userId = 1;
            int sinDate = 20240203;
            long raiinNo = 0;
            long ptId = 123456789;
            List<OrdInfModel> odrInfs = new List<OrdInfModel>()
            {
                new OrdInfModel()
                {
                }
            };
            List<Tuple<int, string, int, int, TenItemModel, double>> targetItems = new List<Tuple<int, string, int, int, TenItemModel, double>>();
            targetItems.Add(new Tuple<int, string, int, int, TenItemModel, double>(1, "Item 1", 0, 0, new TenItemModel(), 1.5));
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockapprovalInf = new Mock<IApprovalInfRepository>();
            TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);
            try
            {
                var result = todayOdrRepository.ChangeAfterAutoCheckOrder(hpId, sinDate, userId, raiinNo, ptId, odrInfs, targetItems);
                Assert.True(!result.Any());
            }
            finally
            {
                #region Remove Data Fetch
                todayOdrRepository.ReleaseResource();
                #endregion
            }
        }

        [Test]
        public void ChangeAfterAutoCheckOrder_002()
        {
            int hpId = 1;
            int userId = 1;
            int sinDate = 20240203;
            long raiinNo = 0;
            long ptId = 123456789;
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel(hpId,"12345678", sinDate)
            };
            List<OrdInfModel> odrInfs = new List<OrdInfModel>()
            {
                new OrdInfModel(-1, hpId, ptId, sinDate, raiinNo, ordInfDetailModels)
            };
            List<Tuple<int, string, int, int, TenItemModel, double>> targetItems = new List<Tuple<int, string, int, int, TenItemModel, double>>();
            targetItems.Add(new Tuple<int, string, int, int, TenItemModel, double>(1, "Item 1", 0, 0, new TenItemModel(), 1.5));
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockapprovalInf = new Mock<IApprovalInfRepository>();
            TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);
            try
            {
                var result = todayOdrRepository.ChangeAfterAutoCheckOrder(hpId, sinDate, userId, raiinNo, ptId, odrInfs, targetItems);
                Assert.True(!result.Any());
            }            
            finally
            {
                #region Remove Data Fetch
                todayOdrRepository.ReleaseResource();
                #endregion
            }
        }

        [Test]
        public void ChangeAfterAutoCheckOrder_003()
        {
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 1;
            int userId = 1;
            int sinDate = 20240203;
            long raiinNo = 0;
            long ptId = 123456789;
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel(hpId,"ItemCdTest", sinDate)
            };
            List<OrdInfModel> odrInfs = new List<OrdInfModel>()
            {
                new OrdInfModel(-1, hpId, ptId, sinDate, raiinNo, ordInfDetailModels)
            };
            List<Tuple<int, string, int, int, TenItemModel, double>> targetItems = new List<Tuple<int, string, int, int, TenItemModel, double>>();
            targetItems.Add(new Tuple<int, string, int, int, TenItemModel, double>(1, "Item 1", 0, 0, new TenItemModel(), 1.5));


            // TenMst
            var tenMsts = ChangeAfterAutoCheckOrderData.ReadTenMst();
            tenant.TenMsts.AddRange(tenMsts);
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockapprovalInf = new Mock<IApprovalInfRepository>();
            TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);

            try
            {
                tenant.SaveChanges();

                var result = todayOdrRepository.ChangeAfterAutoCheckOrder(hpId, sinDate, userId, raiinNo, ptId, odrInfs, targetItems);
                Assert.True(!result.Any());
            }
            finally
            {
                #region Remove Data Fetch
                todayOdrRepository.ReleaseResource();
                tenant.TenMsts.RemoveRange(tenMsts);
                tenant.SaveChanges();
                #endregion
            }
        }

        [Test]
        public void ChangeAfterAutoCheckOrder_004()
        {
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 1;
            int userId = 1;
            int sinDate = 20240203;
            long raiinNo = 0;
            long ptId = 123456789;
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCdTest", 85)
            };
            List<OrdInfModel> odrInfs = new List<OrdInfModel>()
            {
                new OrdInfModel(-1, hpId, ptId, sinDate, raiinNo, ordInfDetailModels)
            };
            List<Tuple<int, string, int, int, TenItemModel, double>> targetItems = new List<Tuple<int, string, int, int, TenItemModel, double>>();
            targetItems.Add(new Tuple<int, string, int, int, TenItemModel, double>(1, "Item 1", 0, 0, new TenItemModel(), 1.5));


            // TenMst
            var tenMsts = ChangeAfterAutoCheckOrderData.ReadTenMst();
            tenant.TenMsts.AddRange(tenMsts);
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockapprovalInf = new Mock<IApprovalInfRepository>();
            TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);

            try
            {
                tenant.SaveChanges();

                var result = todayOdrRepository.ChangeAfterAutoCheckOrder(hpId, sinDate, userId, raiinNo, ptId, odrInfs, targetItems);
                Assert.True(result.Any());
            }
            finally
            {
                #region Remove Data Fetch
                todayOdrRepository.ReleaseResource();
                tenant.TenMsts.RemoveRange(tenMsts);
                tenant.SaveChanges();
                #endregion
            }
        }



        [Test]
        public void ChangeAfterAutoCheckOrder_005()
        {
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 1;
            int userId = 1;
            int sinDate = 20240203;
            long raiinNo = 0;
            long ptId = 123456789;
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCdTest", 70)
            };
            List<OrdInfModel> odrInfs = new List<OrdInfModel>()
            {
                new OrdInfModel(-1, hpId, ptId, sinDate, raiinNo, ordInfDetailModels)
            };
            List<Tuple<int, string, int, int, TenItemModel, double>> targetItems = new List<Tuple<int, string, int, int, TenItemModel, double>>();
            targetItems.Add(new Tuple<int, string, int, int, TenItemModel, double>(1, "Item 1", 0, 0, new TenItemModel(), 1.5));


            // TenMst
            var tenMsts = ChangeAfterAutoCheckOrderData.ReadTenMst();
            tenant.TenMsts.AddRange(tenMsts);
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockapprovalInf = new Mock<IApprovalInfRepository>();
            TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);

            try
            {
                tenant.SaveChanges();

                var result = todayOdrRepository.ChangeAfterAutoCheckOrder(hpId, sinDate, userId, raiinNo, ptId, odrInfs, targetItems);
                Assert.True(result.Any());
            }
            finally
            {
                #region Remove Data Fetch
                todayOdrRepository.ReleaseResource();
                tenant.TenMsts.RemoveRange(tenMsts);
                tenant.SaveChanges();
                #endregion
            }
        }

        [Test]
        public void ChangeAfterAutoCheckOrder_006()
        {
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 1;
            int userId = 1;
            int sinDate = 20240203;
            long raiinNo = 0;
            long ptId = 123456789;
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCdTest", 70)
            };
            List<OrdInfModel> odrInfs = new List<OrdInfModel>()
            {
                new OrdInfModel(-1, hpId, ptId, sinDate, raiinNo, ordInfDetailModels)
            };
            List<Tuple<int, string, int, int, TenItemModel, double>> targetItems = new List<Tuple<int, string, int, int, TenItemModel, double>>();
            targetItems.Add(new Tuple<int, string, int, int, TenItemModel, double>(0, "Item 1", 0, 0, new TenItemModel(), 1.5));


            // TenMst
            var tenMsts = ChangeAfterAutoCheckOrderData.ReadTenMst();
            tenant.TenMsts.AddRange(tenMsts);
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockapprovalInf = new Mock<IApprovalInfRepository>();
            TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);

            try
            {
                tenant.SaveChanges();

                var result = todayOdrRepository.ChangeAfterAutoCheckOrder(hpId, sinDate, userId, raiinNo, ptId, odrInfs, targetItems);
                Assert.True(result.Any());
            }
            finally
            {
                #region Remove Data Fetch
                todayOdrRepository.ReleaseResource();
                tenant.TenMsts.RemoveRange(tenMsts);
                tenant.SaveChanges();
                #endregion
            }
        }
        [Test]
        public void ChangeAfterAutoCheckOrder_007()
        {
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 1;
            int userId = 1;
            int sinDate = 20240203;
            long raiinNo = 0;
            long ptId = 123456789;
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCdTest", 85)
            };
            List<OrdInfModel> odrInfs = new List<OrdInfModel>()
            {
                new OrdInfModel(-1, hpId, ptId, sinDate, raiinNo, ordInfDetailModels)
            };
            List<Tuple<int, string, int, int, TenItemModel, double>> targetItems = new List<Tuple<int, string, int, int, TenItemModel, double>>();
            targetItems.Add(new Tuple<int, string, int, int, TenItemModel, double>(1, "Item 1", 1, 0, new TenItemModel(), 1.5));


            // TenMst
            var tenMsts = ChangeAfterAutoCheckOrderData.ReadTenMst();
            tenant.TenMsts.AddRange(tenMsts);
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockapprovalInf = new Mock<IApprovalInfRepository>();
            TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);

            try
            {
                tenant.SaveChanges();

                var result = todayOdrRepository.ChangeAfterAutoCheckOrder(hpId, sinDate, userId, raiinNo, ptId, odrInfs, targetItems);
                Assert.True(!result.Any());
            }
            finally
            {
                #region Remove Data Fetch
                todayOdrRepository.ReleaseResource();
                tenant.TenMsts.RemoveRange(tenMsts);
                tenant.SaveChanges();
                #endregion
            }
        }

        [Test]
        public void ChangeAfterAutoCheckOrder_008()
        {
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 1;
            int userId = 1;
            int sinDate = 20240203;
            long raiinNo = 0;
            long ptId = 123456789;
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCdTest", 70)
            };
            List<OrdInfModel> odrInfs = new List<OrdInfModel>()
            {
                new OrdInfModel(-1, hpId, ptId, sinDate, raiinNo, ordInfDetailModels)
            };
            List<Tuple<int, string, int, int, TenItemModel, double>> targetItems = new List<Tuple<int, string, int, int, TenItemModel, double>>();
            targetItems.Add(new Tuple<int, string, int, int, TenItemModel, double>(1, "Item 1", 0, 0, new TenItemModel("ItemCdTest"), 1.5));


            // TenMst
            var tenMsts = ChangeAfterAutoCheckOrderData.ReadTenMst();
            tenant.TenMsts.AddRange(tenMsts);
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockapprovalInf = new Mock<IApprovalInfRepository>();
            TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);

            try
            {
                tenant.SaveChanges();

                var result = todayOdrRepository.ChangeAfterAutoCheckOrder(hpId, sinDate, userId, raiinNo, ptId, odrInfs, targetItems);
                Assert.True(result.Any());
            }
            finally
            {
                #region Remove Data Fetch
                todayOdrRepository.ReleaseResource();
                tenant.TenMsts.RemoveRange(tenMsts);
                tenant.SaveChanges();
                #endregion
            }
        }

        [Test]
        public void ChangeAfterAutoCheckOrder_009()
        {
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 1;
            int userId = 1;
            int sinDate = 20240203;
            long raiinNo = 0;
            long ptId = 123456789;
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCd1", 70)
            };
            List<OrdInfModel> odrInfs = new List<OrdInfModel>()
            {
                new OrdInfModel(-1, hpId, ptId, sinDate, raiinNo, ordInfDetailModels)
            };
            List<Tuple<int, string, int, int, TenItemModel, double>> targetItems = new List<Tuple<int, string, int, int, TenItemModel, double>>();
            targetItems.Add(new Tuple<int, string, int, int, TenItemModel, double>(1, "Item 1", 0, 0, new TenItemModel("ItemCd1"), 1.5));


            // TenMst
            var tenMsts = ChangeAfterAutoCheckOrderData.ReadTenMst();
            tenant.TenMsts.AddRange(tenMsts);
            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockapprovalInf = new Mock<IApprovalInfRepository>();
            TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);

            try
            {
                tenant.SaveChanges();

                var result = todayOdrRepository.ChangeAfterAutoCheckOrder(hpId, sinDate, userId, raiinNo, ptId, odrInfs, targetItems);
                Assert.True(result.Any());
            }
            finally
            {
                #region Remove Data Fetch
                todayOdrRepository.ReleaseResource();
                tenant.TenMsts.RemoveRange(tenMsts);
                tenant.SaveChanges();
                #endregion
            }
        }

        [Test]
        public void ChangeAfterAutoCheckOrder_010()
        {
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 1;
            int userId = 1;
            int sinDate = 20240203;
            long raiinNo = 0;
            long ptId = 123456789;
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCd1", 70, "ipnTest")
            };
            List<OrdInfModel> odrInfs = new List<OrdInfModel>()
            {
                new OrdInfModel(-1, hpId, ptId, sinDate, raiinNo, ordInfDetailModels)
            };
            List<Tuple<int, string, int, int, TenItemModel, double>> targetItems = new List<Tuple<int, string, int, int, TenItemModel, double>>();
            targetItems.Add(new Tuple<int, string, int, int, TenItemModel, double>(1, "Item 1", 0, 0, new TenItemModel("ItemCd1"), 1.5));


            // TenMst
            var tenMsts = ChangeAfterAutoCheckOrderData.ReadTenMst();
            tenant.TenMsts.AddRange(tenMsts);

            //IpnName
            var ipnNameMsts = ChangeAfterAutoCheckOrderData.ReadIpnNameMst();
            tenant.IpnNameMsts.AddRange(ipnNameMsts);

            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockapprovalInf = new Mock<IApprovalInfRepository>();
            TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);

            try
            {
                tenant.SaveChanges();

                var result = todayOdrRepository.ChangeAfterAutoCheckOrder(hpId, sinDate, userId, raiinNo, ptId, odrInfs, targetItems);
                Assert.True(result.Any());
            }
            finally
            {
                #region Remove Data Fetch
                todayOdrRepository.ReleaseResource();
                tenant.TenMsts.RemoveRange(tenMsts);
                tenant.IpnNameMsts.RemoveRange(ipnNameMsts);
                tenant.SaveChanges();
                #endregion
            }
        }

        [Test]
        public void ChangeAfterAutoCheckOrder_011()
        {
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 1;
            int userId = 1;
            int sinDate = 20240203;
            long raiinNo = 0;
            long ptId = 123456789;
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCd1", 85, "ipnTest")
            };
            List<OrdInfModel> odrInfs = new List<OrdInfModel>()
            {
                new OrdInfModel(-1, hpId, ptId, sinDate, raiinNo, ordInfDetailModels)
            };
            List<Tuple<int, string, int, int, TenItemModel, double>> targetItems = new List<Tuple<int, string, int, int, TenItemModel, double>>();
            targetItems.Add(new Tuple<int, string, int, int, TenItemModel, double>(1, "Item 1", 0, 0, new TenItemModel("ItemCd1", "Test", ""), 1.5));


            // TenMst
            var tenMsts = ChangeAfterAutoCheckOrderData.ReadTenMst();
            tenant.TenMsts.AddRange(tenMsts);

            //IpnName
            var ipnNameMsts = ChangeAfterAutoCheckOrderData.ReadIpnNameMst();
            tenant.IpnNameMsts.AddRange(ipnNameMsts);

            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockapprovalInf = new Mock<IApprovalInfRepository>();
            TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);

            try
            {
                tenant.SaveChanges();

                var result = todayOdrRepository.ChangeAfterAutoCheckOrder(hpId, sinDate, userId, raiinNo, ptId, odrInfs, targetItems);
                Assert.True(result.Any());
            }
            finally
            {
                #region Remove Data Fetch
                todayOdrRepository.ReleaseResource();
                tenant.TenMsts.RemoveRange(tenMsts);
                tenant.IpnNameMsts.RemoveRange(ipnNameMsts);
                tenant.SaveChanges();
                #endregion
            }
        }

        [Test]
        public void ChangeAfterAutoCheckOrder_012()
        {
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 1;
            int userId = 1;
            int sinDate = 20240203;
            long raiinNo = 0;
            long ptId = 123456789;
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCd1", 85, "ipnTest")
            };
            List<OrdInfModel> odrInfs = new List<OrdInfModel>()
            {
                new OrdInfModel(-1, hpId, ptId, sinDate, raiinNo, ordInfDetailModels)
            };
            List<Tuple<int, string, int, int, TenItemModel, double>> targetItems = new List<Tuple<int, string, int, int, TenItemModel, double>>();
            targetItems.Add(new Tuple<int, string, int, int, TenItemModel, double>(1, "Item 1", 0, 0, new TenItemModel("ItemCd1", "", "Test"), 1.5));


            // TenMst
            var tenMsts = ChangeAfterAutoCheckOrderData.ReadTenMst();
            tenant.TenMsts.AddRange(tenMsts);

            //IpnName
            var ipnNameMsts = ChangeAfterAutoCheckOrderData.ReadIpnNameMst();
            tenant.IpnNameMsts.AddRange(ipnNameMsts);

            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockapprovalInf = new Mock<IApprovalInfRepository>();
            TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);

            try
            {
                tenant.SaveChanges();

                var result = todayOdrRepository.ChangeAfterAutoCheckOrder(hpId, sinDate, userId, raiinNo, ptId, odrInfs, targetItems);
                Assert.True(result.Any());
            }
            finally
            {
                #region Remove Data Fetch
                todayOdrRepository.ReleaseResource();
                tenant.TenMsts.RemoveRange(tenMsts);
                tenant.IpnNameMsts.RemoveRange(ipnNameMsts);
                tenant.SaveChanges();
                #endregion
            }
        }

        [Test]
        public void ChangeAfterAutoCheckOrder_013()
        {
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 1;
            int userId = 1;
            int sinDate = 20240203;
            long raiinNo = 0;
            long ptId = 123456789;
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCd1", 20, 1)
            };
            List<OrdInfModel> odrInfs = new List<OrdInfModel>()
            {
                new OrdInfModel(-1, hpId, ptId, sinDate, raiinNo, ordInfDetailModels)
            };
            List<Tuple<int, string, int, int, TenItemModel, double>> targetItems = new List<Tuple<int, string, int, int, TenItemModel, double>>();
            targetItems.Add(new Tuple<int, string, int, int, TenItemModel, double>(1, "Item 1", 0, 0, new TenItemModel("ItemCd1", "", "", 20), 1.5));


            // TenMst
            var tenMsts = ChangeAfterAutoCheckOrderData.ReadTenMst();
            tenant.TenMsts.AddRange(tenMsts);

            //IpnName
            var ipnNameMsts = ChangeAfterAutoCheckOrderData.ReadIpnNameMst();
            tenant.IpnNameMsts.AddRange(ipnNameMsts);

            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockapprovalInf = new Mock<IApprovalInfRepository>();
            TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);

            try
            {
                tenant.SaveChanges();

                var result = todayOdrRepository.ChangeAfterAutoCheckOrder(hpId, sinDate, userId, raiinNo, ptId, odrInfs, targetItems);
                Assert.True(!result.Any());
            }
            finally
            {
                #region Remove Data Fetch
                todayOdrRepository.ReleaseResource();
                tenant.TenMsts.RemoveRange(tenMsts);
                tenant.IpnNameMsts.RemoveRange(ipnNameMsts);
                tenant.SaveChanges();
                #endregion
            }
        }

        [Test]
        public void ChangeAfterAutoCheckOrder_014()
        {
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 1;
            int userId = 1;
            int sinDate = 20240203;
            long raiinNo = 0;
            long ptId = 123456789;
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCd1", 20, 1)
            };
            List<OrdInfModel> odrInfs = new List<OrdInfModel>()
            {
                new OrdInfModel(-1, hpId, ptId, sinDate, raiinNo, ordInfDetailModels)
            };
            List<Tuple<int, string, int, int, TenItemModel, double>> targetItems = new List<Tuple<int, string, int, int, TenItemModel, double>>();
            targetItems.Add(new Tuple<int, string, int, int, TenItemModel, double>(1, "Item 1", 0, 0, new TenItemModel("ItemCd1", "Test", "", 0), 1.5));


            // TenMst
            var tenMsts = ChangeAfterAutoCheckOrderData.ReadTenMst();
            tenant.TenMsts.AddRange(tenMsts);

            //IpnName
            var ipnNameMsts = ChangeAfterAutoCheckOrderData.ReadIpnNameMst();
            tenant.IpnNameMsts.AddRange(ipnNameMsts);

            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockapprovalInf = new Mock<IApprovalInfRepository>();
            TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);

            try
            {
                tenant.SaveChanges();

                var result = todayOdrRepository.ChangeAfterAutoCheckOrder(hpId, sinDate, userId, raiinNo, ptId, odrInfs, targetItems);
                Assert.True(result.Any());
            }
            finally
            {
                #region Remove Data Fetch
                todayOdrRepository.ReleaseResource();
                tenant.TenMsts.RemoveRange(tenMsts);
                tenant.IpnNameMsts.RemoveRange(ipnNameMsts);
                tenant.SaveChanges();
                #endregion
            }
        }

        [Test]
        public void ChangeAfterAutoCheckOrder_015()
        {
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 1;
            int userId = 1;
            int sinDate = 20240203;
            long raiinNo = 0;
            long ptId = 123456789;
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCd1", 20, 1)
            };
            List<OrdInfModel> odrInfs = new List<OrdInfModel>()
            {
                new OrdInfModel(-1, hpId, ptId, sinDate, raiinNo, ordInfDetailModels)
            };
            List<Tuple<int, string, int, int, TenItemModel, double>> targetItems = new List<Tuple<int, string, int, int, TenItemModel, double>>();
            targetItems.Add(new Tuple<int, string, int, int, TenItemModel, double>(1, "Item 1", 0, 0, new TenItemModel("ItemCd1", "", "Test", 0), 1.5));


            // TenMst
            var tenMsts = ChangeAfterAutoCheckOrderData.ReadTenMst();
            tenant.TenMsts.AddRange(tenMsts);

            //IpnName
            var ipnNameMsts = ChangeAfterAutoCheckOrderData.ReadIpnNameMst();
            tenant.IpnNameMsts.AddRange(ipnNameMsts);

            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockapprovalInf = new Mock<IApprovalInfRepository>();
            TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);

            try
            {
                tenant.SaveChanges();

                var result = todayOdrRepository.ChangeAfterAutoCheckOrder(hpId, sinDate, userId, raiinNo, ptId, odrInfs, targetItems);
                Assert.True(result.Any());
            }
            finally
            {
                #region Remove Data Fetch
                todayOdrRepository.ReleaseResource();
                tenant.TenMsts.RemoveRange(tenMsts);
                tenant.IpnNameMsts.RemoveRange(ipnNameMsts);
                tenant.SaveChanges();
                #endregion
            }
        }

        [Test]
        public void ChangeAfterAutoCheckOrder_016()
        {
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 1;
            int userId = 1;
            int sinDate = 20240203;
            long raiinNo = 0;
            long ptId = 123456789;
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCd1", 20, "",1, 1)
            };
            List<OrdInfModel> odrInfs = new List<OrdInfModel>()
            {
                new OrdInfModel(-1, hpId, ptId, sinDate, raiinNo, ordInfDetailModels)
            };
            List<Tuple<int, string, int, int, TenItemModel, double>> targetItems = new List<Tuple<int, string, int, int, TenItemModel, double>>();
            targetItems.Add(new Tuple<int, string, int, int, TenItemModel, double>(1, "Item 1", 0, 0, new TenItemModel("ItemCd1", "", "", 20), 1.5));


            // TenMst
            var tenMsts = ChangeAfterAutoCheckOrderData.ReadTenMst();
            tenant.TenMsts.AddRange(tenMsts);

            //IpnName
            var ipnNameMsts = ChangeAfterAutoCheckOrderData.ReadIpnNameMst();
            tenant.IpnNameMsts.AddRange(ipnNameMsts);

            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockapprovalInf = new Mock<IApprovalInfRepository>();
            TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);

            try
            {
                tenant.SaveChanges();

                var result = todayOdrRepository.ChangeAfterAutoCheckOrder(hpId, sinDate, userId, raiinNo, ptId, odrInfs, targetItems);
                Assert.True(!result.Any());
            }
            finally
            {
                #region Remove Data Fetch
                todayOdrRepository.ReleaseResource();
                tenant.TenMsts.RemoveRange(tenMsts);
                tenant.IpnNameMsts.RemoveRange(ipnNameMsts);
                tenant.SaveChanges();
                #endregion
            }
        }

        [Test]
        public void ChangeAfterAutoCheckOrder_017()
        {
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 1;
            int userId = 1;
            int sinDate = 20240203;
            long raiinNo = 0;
            long ptId = 123456789;
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCd1", 20, "",2, 1)
            };
            List<OrdInfModel> odrInfs = new List<OrdInfModel>()
            {
                new OrdInfModel(-1, hpId, ptId, sinDate, raiinNo, ordInfDetailModels)
            };
            List<Tuple<int, string, int, int, TenItemModel, double>> targetItems = new List<Tuple<int, string, int, int, TenItemModel, double>>();
            targetItems.Add(new Tuple<int, string, int, int, TenItemModel, double>(1, "Item 1", 0, 0, new TenItemModel("ItemCd1", "", "", 20), 1.5));


            // TenMst
            var tenMsts = ChangeAfterAutoCheckOrderData.ReadTenMst();
            tenant.TenMsts.AddRange(tenMsts);

            //IpnName
            var ipnNameMsts = ChangeAfterAutoCheckOrderData.ReadIpnNameMst();
            tenant.IpnNameMsts.AddRange(ipnNameMsts);

            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockapprovalInf = new Mock<IApprovalInfRepository>();
            TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);

            try
            {
                tenant.SaveChanges();

                var result = todayOdrRepository.ChangeAfterAutoCheckOrder(hpId, sinDate, userId, raiinNo, ptId, odrInfs, targetItems);
                Assert.True(!result.Any());
            }
            finally
            {
                #region Remove Data Fetch
                todayOdrRepository.ReleaseResource();
                tenant.TenMsts.RemoveRange(tenMsts);
                tenant.IpnNameMsts.RemoveRange(ipnNameMsts);
                tenant.SaveChanges();
                #endregion
            }
        }

        [Test]
        public void ChangeAfterAutoCheckOrder_018()
        {
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 1;
            int userId = 1;
            int sinDate = 20240203;
            long raiinNo = 0;
            long ptId = 123456789;
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCd1", 20, "",2, 1,3)
            };
            List<OrdInfModel> odrInfs = new List<OrdInfModel>()
            {
                new OrdInfModel(-1, hpId, ptId, sinDate, raiinNo, ordInfDetailModels)
            };
            List<Tuple<int, string, int, int, TenItemModel, double>> targetItems = new List<Tuple<int, string, int, int, TenItemModel, double>>();
            targetItems.Add(new Tuple<int, string, int, int, TenItemModel, double>(1, "Item 1", 0, 0, new TenItemModel("ItemCd1", "", "", 20), 1.5));


            // TenMst
            var tenMsts = ChangeAfterAutoCheckOrderData.ReadTenMst();
            tenant.TenMsts.AddRange(tenMsts);

            //IpnName
            var ipnNameMsts = ChangeAfterAutoCheckOrderData.ReadIpnNameMst();
            tenant.IpnNameMsts.AddRange(ipnNameMsts);

            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockapprovalInf = new Mock<IApprovalInfRepository>();
            TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);

            try
            {
                tenant.SaveChanges();

                var result = todayOdrRepository.ChangeAfterAutoCheckOrder(hpId, sinDate, userId, raiinNo, ptId, odrInfs, targetItems);
                Assert.True(!result.Any());
            }
            finally
            {
                #region Remove Data Fetch
                todayOdrRepository.ReleaseResource();
                tenant.TenMsts.RemoveRange(tenMsts);
                tenant.IpnNameMsts.RemoveRange(ipnNameMsts);
                tenant.SaveChanges();
                #endregion
            }
        }

        [Test]
        public void ChangeAfterAutoCheckOrder_019()
        {
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 1;
            int userId = 1;
            int sinDate = 20240203;
            long raiinNo = 0;
            long ptId = 123456789;
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCd1", 20, "",2, 1,3)
            };
            List<OrdInfModel> odrInfs = new List<OrdInfModel>()
            {
                new OrdInfModel(-1, hpId, ptId, sinDate, raiinNo, ordInfDetailModels)
            };
            List<Tuple<int, string, int, int, TenItemModel, double>> targetItems = new List<Tuple<int, string, int, int, TenItemModel, double>>();
            targetItems.Add(new Tuple<int, string, int, int, TenItemModel, double>(1, "Item 1", 0, 0, new TenItemModel("ItemCd1", "", "", 0), 1.5));


            // TenMst
            var tenMsts = ChangeAfterAutoCheckOrderData.ReadTenMst();
            tenant.TenMsts.AddRange(tenMsts);

            //IpnName
            var ipnNameMsts = ChangeAfterAutoCheckOrderData.ReadIpnNameMst();
            tenant.IpnNameMsts.AddRange(ipnNameMsts);

            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockapprovalInf = new Mock<IApprovalInfRepository>();
            TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);

            try
            {
                tenant.SaveChanges();

                var result = todayOdrRepository.ChangeAfterAutoCheckOrder(hpId, sinDate, userId, raiinNo, ptId, odrInfs, targetItems);
                Assert.True(result.Any());
            }
            finally
            {
                #region Remove Data Fetch
                todayOdrRepository.ReleaseResource();
                tenant.TenMsts.RemoveRange(tenMsts);
                tenant.IpnNameMsts.RemoveRange(ipnNameMsts);
                tenant.SaveChanges();
                #endregion
            }
        }

        [Test]
        public void ChangeAfterAutoCheckOrder_020()
        {
            var tenant = TenantProvider.GetNoTrackingDataContext();
            int hpId = 1;
            int userId = 1;
            int sinDate = 20240203;
            long raiinNo = 0;
            long ptId = 123456789;
            List<OrdInfDetailModel> ordInfDetailModels = new List<OrdInfDetailModel>()
            {
                new OrdInfDetailModel("ItemCd1", 20, "",1, 1,3)
            };
            List<OrdInfModel> odrInfs = new List<OrdInfModel>()
            {
                new OrdInfModel(-1, hpId, ptId, sinDate, raiinNo, ordInfDetailModels)
            };
            List<Tuple<int, string, int, int, TenItemModel, double>> targetItems = new List<Tuple<int, string, int, int, TenItemModel, double>>();
            targetItems.Add(new Tuple<int, string, int, int, TenItemModel, double>(1, "Item 1", 0, 0, new TenItemModel("ItemCd1", "", "", 0), 1.5));


            // TenMst
            var tenMsts = ChangeAfterAutoCheckOrderData.ReadTenMst();
            tenant.TenMsts.AddRange(tenMsts);

            //IpnName
            var ipnNameMsts = ChangeAfterAutoCheckOrderData.ReadIpnNameMst();
            tenant.IpnNameMsts.AddRange(ipnNameMsts);

            var mockSystemConf = new Mock<ISystemConfRepository>();
            var mockapprovalInf = new Mock<IApprovalInfRepository>();
            TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, mockSystemConf.Object, mockapprovalInf.Object);

            try
            {
                tenant.SaveChanges();

                var result = todayOdrRepository.ChangeAfterAutoCheckOrder(hpId, sinDate, userId, raiinNo, ptId, odrInfs, targetItems);
                Assert.True(result.Any());
            }
            finally
            {
                #region Remove Data Fetch
                todayOdrRepository.ReleaseResource();
                tenant.TenMsts.RemoveRange(tenMsts);
                tenant.IpnNameMsts.RemoveRange(ipnNameMsts);
                tenant.SaveChanges();
                #endregion
            }
        }

    }
}
