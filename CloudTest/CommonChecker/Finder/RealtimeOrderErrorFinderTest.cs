using CloudUnitTest.SampleData;
using CommonChecker.DB;
using Entity.Tenant;
using Helper.Extension;
using Reporting.Calculate.Extensions;

namespace CloudUnitTest.CommonChecker.Finder
{
    public class RealtimeOrderErrorFinderTest : BaseUT
    {
        [Test]
        public void TC_001_FindAgeComment_Test_AgeCommentInfo_Is_Not_Null()
        {

            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var testCmt1 = "UT6666";
            var testCmt2 = "UT7777";
            var ageCommentInfo = new List<M14CmtCode>()
            {
                new M14CmtCode()
                {
                    AttentionCmt = null,
                    AttentionCmtCd = testCmt1,
                },
                new M14CmtCode()
                {
                    AttentionCmt = "COMMENT-Test",
                    AttentionCmtCd = testCmt2,
                },
            };

            tenantTracking.M14CmtCode.AddRange(ageCommentInfo);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            try
            {
                // Act
                var result1 = realtimcheckerfinder.FindAgeComment(testCmt1);
                var result2 = realtimcheckerfinder.FindAgeComment(testCmt2);

                // Assert
                Assert.AreEqual(string.Empty, result1);
                Assert.AreEqual("COMMENT-Test", result2);
            }
            finally
            {
                tenantTracking.M14CmtCode.RemoveRange(ageCommentInfo);
                tenantTracking.SaveChanges();
            }

        }

        [Test]
        public void TC_002_FindAgeComment_Test_AgeCommentInfo_Is_Null()
        {

            //Setup Data Test
            var testCmt1 = "UT6666";
            var testCmt2 = "UT7777";

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            // Act
            var result1 = realtimcheckerfinder.FindAgeComment(testCmt1);
            var result2 = realtimcheckerfinder.FindAgeComment(testCmt2);

            // Assert
            Assert.AreEqual(string.Empty, result1);
            Assert.AreEqual(string.Empty, result2);
        }

        [Test]
        public void TC_003_FindAnalogueName_Test_AnalogueInfo_Is_Not_Null()
        {

            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var testAnalogueCd1 = "UT7777";
            var testAnalogueCd2 = "UT8888";
            var m56AnalogueCds = new List<M56AnalogueCd>()
            {
                new M56AnalogueCd()
                {
                    AnalogueCd = testAnalogueCd1,
                    AnalogueName = null,
                },
                new M56AnalogueCd()
                {
                    AnalogueCd = testAnalogueCd2,
                    AnalogueName = "Analogue-Name-Test",
                },
            };

            tenantTracking.M56AnalogueCd.AddRange(m56AnalogueCds);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            try
            {
                // Act
                var result1 = realtimcheckerfinder.FindAnalogueName(testAnalogueCd1);
                var result2 = realtimcheckerfinder.FindAnalogueName(testAnalogueCd2);

                // Assert
                Assert.AreEqual(string.Empty, result1);
                Assert.AreEqual("Analogue-Name-Test", result2);
            }
            finally
            {
                tenantTracking.M56AnalogueCd.RemoveRange(m56AnalogueCds);
                tenantTracking.SaveChanges();
            }

        }

        [Test]
        public void TC_004_TEST_FindAnalogueNameDic()
        {

            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var testAnalogueCd1 = "UT7777";
            var testAnalogueCd2 = "UT8888";
            var testAnalogueCd3 = "UT9999";
            var m56AnalogueCds = new List<M56AnalogueCd>()
            {
                new M56AnalogueCd()
                {
                    AnalogueCd = testAnalogueCd1,
                    AnalogueName = null,
                },
                new M56AnalogueCd()
                {
                    AnalogueCd = testAnalogueCd2,
                    AnalogueName = "Analogue-Name-Test1",
                },
                new M56AnalogueCd()
                {
                    AnalogueCd = testAnalogueCd3,
                    AnalogueName = "Analogue-Name-Test2",
                },
            };

            tenantTracking.M56AnalogueCd.AddRange(m56AnalogueCds);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            var analogueCodeLists = new List<string>() { testAnalogueCd1, testAnalogueCd2 };
            try
            {
                // Act
                var result = realtimcheckerfinder.FindAnalogueNameDic(analogueCodeLists);

                // Assert
                Assert.AreEqual(2, result.Count);
                Assert.AreEqual(string.Empty, result.First().Value);
                Assert.AreEqual("Analogue-Name-Test1", result.Last().Value);
            }
            finally
            {
                tenantTracking.M56AnalogueCd.RemoveRange(m56AnalogueCds);
                tenantTracking.SaveChanges();
            }

        }

        [Test]
        public void TC_005_TEST_FindClassName()
        {
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var classCd1 = "UT7777";
            var classCd2 = "UT8888";
            var classCd3 = "UT9999";
            var classCd4 = "UT6699";
            var m56s = new List<M56DrugClass>()
            {
                new M56DrugClass()
                {
                    ClassCd = classCd1,
                    ClassName = null,
                },
                new M56DrugClass()
                {
                    ClassCd = classCd2,
                    ClassName = "Analogue-Name-Test1",
                },
                new M56DrugClass()
                {
                    ClassCd = classCd3,
                    ClassName = "Analogue-Name-Test2",
                },
            };

            tenantTracking.M56DrugClass.AddRange(m56s);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);
            try
            {
                // Act
                var result1 = realtimcheckerfinder.FindClassName(classCd1);
                var result2 = realtimcheckerfinder.FindClassName(classCd2);
                var result3 = realtimcheckerfinder.FindClassName(classCd4);

                // Assert
                Assert.AreEqual(string.Empty, result1);
                Assert.AreEqual("Analogue-Name-Test1", result2);
                Assert.AreEqual(string.Empty, result3);
            }
            finally
            {
                tenantTracking.M56DrugClass.RemoveRange(m56s);
                tenantTracking.SaveChanges();
            }

        }

        [Test]
        public void TC_006_TEST_FindComponentName()
        {
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var sebunCd1 = "UT7777";
            var sebunCd2 = "UT8888";
            var sebunCd3 = "UT9999";
            var m56s = new List<M56ExIngCode>()
            {
                new M56ExIngCode()
                {
                    SeibunCd = sebunCd1,
                    SeibunIndexCd = "000",
                    SeibunName = "SeibunName-Test1"
                },
                new M56ExIngCode()
                {
                    SeibunCd = sebunCd2,
                    SeibunIndexCd = "000",
                    SeibunName = null,
                },
                new M56ExIngCode()
                {
                    SeibunCd = sebunCd3,
                    SeibunIndexCd = "000",
                    SeibunName = ""
                },
            };

            tenantTracking.M56ExIngCode.AddRange(m56s);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);
            try
            {
                // Act
                var result1 = realtimcheckerfinder.FindComponentName(sebunCd1);
                var result2 = realtimcheckerfinder.FindComponentName(sebunCd2);
                var result3 = realtimcheckerfinder.FindComponentName(sebunCd3);

                // Assert
                Assert.AreEqual("SeibunName-Test1", result1);
                Assert.AreEqual(string.Empty, result2);
                Assert.AreEqual(string.Empty, result3);
            }
            finally
            {
                tenantTracking.M56ExIngCode.RemoveRange(m56s);
                tenantTracking.SaveChanges();
            }

        }

        [Test]
        public void TC_007_TEST_FindComponentNameDic()
        {
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var sebunCd1 = "UT7777";
            var sebunCd2 = "UT8888";
            var sebunCd3 = "UT9999";
            var m56s = new List<M56ExIngCode>()
            {
                new M56ExIngCode()
                {
                    SeibunCd = sebunCd1,
                    SeibunIndexCd = "000",
                    SeibunName = "SeibunName-Test1"
                },
                new M56ExIngCode()
                {
                    SeibunCd = sebunCd2,
                    SeibunIndexCd = "000",
                    SeibunName = null,
                },
                new M56ExIngCode()
                {
                    SeibunCd = sebunCd3,
                    SeibunIndexCd = "000",
                    SeibunName = ""
                },
            };

            tenantTracking.M56ExIngCode.AddRange(m56s);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            var sebunCds = new List<string> { sebunCd1, sebunCd2, sebunCd3 };
            try
            {
                // Act
                var result = realtimcheckerfinder.FindComponentNameDic(sebunCds);

                // Assert
                Assert.AreEqual(3, result.Count);
            }
            finally
            {
                tenantTracking.M56ExIngCode.RemoveRange(m56s);
                tenantTracking.SaveChanges();
            }

        }

        [Test]
        public void TC_008_TEST_FindDiseaseComment()
        {
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cmtCd1 = "UT7777";
            var cmtCd2 = "UT8888";
            var cmtCd3 = "UT9999";
            var m42s = new List<M42ContraCmt>()
            {
                new M42ContraCmt()
                {
                    CmtCd = cmtCd1,
                    Cmt = null,
                },
                new M42ContraCmt()
                {
                    CmtCd = cmtCd2,
                    Cmt = "000",
                },
                new M42ContraCmt()
                {
                    CmtCd = cmtCd3,
                    Cmt = "",
                },
            };

            tenantTracking.M42ContraCmt.AddRange(m42s);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            try
            {
                // Act
                var result1 = realtimcheckerfinder.FindDiseaseComment(cmtCd1);
                var result2 = realtimcheckerfinder.FindDiseaseComment(cmtCd2);
                var result3 = realtimcheckerfinder.FindDiseaseComment(cmtCd3);

                // Assert
                Assert.AreEqual(string.Empty, result1);
                Assert.AreEqual("000", result2);
                Assert.AreEqual(string.Empty, result3);
            }
            finally
            {
                tenantTracking.M42ContraCmt.RemoveRange(m42s);
                tenantTracking.SaveChanges();
            }

        }

        [Test]
        public void TC_009_TEST_FindDiseaseName()
        {
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = "UT7777";
            var cd2 = "UT8888";
            var cd3 = "UT9999";
            var m42s = new List<M42ContraindiDisCon>()
            {
                new M42ContraindiDisCon()
                {
                    ByotaiCd = cd1,
                    Byomei = null,
                },
                new M42ContraindiDisCon()
                {
                    ByotaiCd = cd2,
                    Byomei = "000",
                },
                new M42ContraindiDisCon()
                {
                    ByotaiCd = cd3,
                    Byomei = "",
                },
            };

            tenantTracking.M42ContraindiDisCon.AddRange(m42s);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            try
            {
                // Act
                var result1 = realtimcheckerfinder.FindDiseaseName(cd1);
                var result2 = realtimcheckerfinder.FindDiseaseName(cd2);
                var result3 = realtimcheckerfinder.FindDiseaseName(cd3);

                // Assert
                Assert.AreEqual(string.Empty, result1);
                Assert.AreEqual("000", result2);
                Assert.AreEqual(string.Empty, result3);
            }
            finally
            {
                tenantTracking.M42ContraindiDisCon.RemoveRange(m42s);
                tenantTracking.SaveChanges();
            }

        }

        [Test]
        public void TC_010_TEST_FindDiseaseNameDic()
        {
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = "UT7777";
            var cd2 = "UT8888";
            var cd3 = "UT9999";
            var m42s = new List<M42ContraindiDisCon>()
            {
                new M42ContraindiDisCon()
                {
                    ByotaiCd = cd1,
                    Byomei = null,
                },
                new M42ContraindiDisCon()
                {
                    ByotaiCd = cd2,
                    Byomei = "000",
                },
                new M42ContraindiDisCon()
                {
                    ByotaiCd = cd3,
                    Byomei = "",
                },
            };

            tenantTracking.M42ContraindiDisCon.AddRange(m42s);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            var byotaiCdList = new List<string> { cd1, cd2, cd3 };
            try
            {
                // Act
                var result = realtimcheckerfinder.FindDiseaseNameDic(byotaiCdList);

                // Assert
                Assert.AreEqual(3, result.Count);
            }
            finally
            {
                tenantTracking.M42ContraindiDisCon.RemoveRange(m42s);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_011_TEST_FindDrvalrgyNameDic()
        {
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = "UT7777";
            var cd2 = "UT8888";
            var cd3 = "UT9999";
            var m56s = new List<M56DrvalrgyCode>()
            {
                new M56DrvalrgyCode()
                {
                    DrvalrgyCd = cd1,
                    DrvalrgyName = null,
                },
                new M56DrvalrgyCode()
                {
                    DrvalrgyCd = cd2,
                    DrvalrgyName = "000",
                },
                new M56DrvalrgyCode()
                {
                    DrvalrgyCd = cd3,
                    DrvalrgyName = "",
                },
            };

            tenantTracking.M56DrvalrgyCode.AddRange(m56s);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            var drvalrgyCodeList = new List<string> { cd1, cd2, cd3 };
            try
            {
                // Act
                var result = realtimcheckerfinder.FindDrvalrgyNameDic(drvalrgyCodeList);

                // Assert
                Assert.AreEqual(3, result.Count);
            }
            finally
            {
                tenantTracking.M56DrvalrgyCode.RemoveRange(m56s);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_012_TEST_FindDrvalrgyName()
        {
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = "UT7777";
            var cd2 = "UT8888";
            var cd3 = "UT9999";
            var m56s = new List<M56DrvalrgyCode>()
            {
                new M56DrvalrgyCode()
                {
                    DrvalrgyCd = cd1,
                    DrvalrgyName = null,
                },
                new M56DrvalrgyCode()
                {
                    DrvalrgyCd = cd2,
                    DrvalrgyName = "UNITTEST",
                },
                new M56DrvalrgyCode()
                {
                    DrvalrgyCd = cd3,
                    DrvalrgyName = "",
                },
            };

            tenantTracking.M56DrvalrgyCode.AddRange(m56s);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            try
            {
                // Act
                var result1 = realtimcheckerfinder.FindDrvalrgyName(cd1);
                var result2 = realtimcheckerfinder.FindDrvalrgyName(cd2);
                var result3 = realtimcheckerfinder.FindDrvalrgyName(cd3);

                // Assert
                Assert.AreEqual(string.Empty, result1);
                Assert.AreEqual("UNITTEST", result2);
                Assert.AreEqual(string.Empty, result3);
            }
            finally
            {
                tenantTracking.M56DrvalrgyCode.RemoveRange(m56s);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_013_TEST_FindFoodName()
        {
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = "T7";
            var cd2 = "T8";
            var cd3 = "T9";
            var m12FoodAlrgyKbns = new List<M12FoodAlrgyKbn>()
            {
                new M12FoodAlrgyKbn()
                {
                    FoodKbn = cd1,
                    FoodName = "UNITTEST1",
                },
                new M12FoodAlrgyKbn()
                {
                    FoodKbn = cd2,
                    FoodName = "UNITTEST2",
                },
                new M12FoodAlrgyKbn()
                {
                    FoodKbn = cd3,
                    FoodName = "",
                },
            };

            tenantTracking.M12FoodAlrgyKbn.AddRange(m12FoodAlrgyKbns);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            try
            {
                // Act
                var result1 = realtimcheckerfinder.FindFoodName(cd1);
                var result2 = realtimcheckerfinder.FindFoodName(cd2);
                var result3 = realtimcheckerfinder.FindFoodName(cd3);

                // Assert
                Assert.AreEqual("UNITTEST1", result1);
                Assert.AreEqual("UNITTEST2", result2);
                Assert.AreEqual(string.Empty, result3);
            }
            finally
            {
                tenantTracking.M12FoodAlrgyKbn.RemoveRange(m12FoodAlrgyKbns);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_014_TEST_FindFoodNameDic()
        {
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = "T7";
            var cd2 = "T8";
            var cd3 = "T9";
            var m12FoodAlrgyKbns = new List<M12FoodAlrgyKbn>()
            {
                new M12FoodAlrgyKbn()
                {
                    FoodKbn = cd1,
                    FoodName = "UNITTEST1",
                },
                new M12FoodAlrgyKbn()
                {
                    FoodKbn = cd2,
                    FoodName = "UNITTEST2",
                },
                new M12FoodAlrgyKbn()
                {
                    FoodKbn = cd3,
                    FoodName = "",
                },
            };

            tenantTracking.M12FoodAlrgyKbn.AddRange(m12FoodAlrgyKbns);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            var codeLists = new List<string> { cd1, cd2, cd3 };
            try
            {
                // Act
                var result = realtimcheckerfinder.FindFoodNameDic(codeLists);

                // Assert
                Assert.AreEqual(3, result.Count);
            }
            finally
            {
                tenantTracking.M12FoodAlrgyKbn.RemoveRange(m12FoodAlrgyKbns);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_015_TEST_FindIppanNameByIppanCode()
        {
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = "UT7777";
            var cd2 = "UT8888";
            var cd3 = "UT9999";
            var ipnNameMsts = new List<IpnNameMst>()
            {
                new IpnNameMst()
                {
                    HpId = 9999,
                    IpnNameCd  = cd1,
                    StartDate = 20230101,
                    SeqNo = 999,
                    IpnName = null
                },
                new IpnNameMst()
                {
                    HpId = 9999,
                    IpnNameCd  = cd2,
                    StartDate = 20230101,
                    SeqNo = 999,
                    IpnName = "UNIT-Test"
                },
                new IpnNameMst()
                {
                   HpId = 9999,
                    IpnNameCd  = cd3,
                    StartDate = 20230101,
                    SeqNo = 999,
                    IpnName = ""
                },
            };

            tenantTracking.IpnNameMsts.AddRange(ipnNameMsts);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            try
            {
                // Act
                var result1 = realtimcheckerfinder.FindIppanNameByIppanCode(cd1);
                var result2 = realtimcheckerfinder.FindIppanNameByIppanCode(cd2);
                var result3 = realtimcheckerfinder.FindIppanNameByIppanCode(cd3);

                // Assert
                Assert.AreEqual(string.Empty, result1);
                Assert.AreEqual("UNIT-Test", result2);
                Assert.AreEqual(string.Empty, result3);
            }
            finally
            {
                tenantTracking.IpnNameMsts.RemoveRange(ipnNameMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_016_TEST_FindItemName()
        {
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var yjCd = "UT271026";

            var tenMsts = CommonCheckerData.ReadTenMst(string.Empty, string.Empty);

            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            try
            {
                // Act
                var result = realtimcheckerfinder.FindItemName(yjCd, 20230505);

                // Assert
                Assert.AreEqual("UNITTEST", result);
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_016_TEST_FindItemNameDic()
        {
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var tenMsts = CommonCheckerData.ReadTenMst(string.Empty, string.Empty);

            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            var yjCds = new List<string> { "UT271023", "UT271024", "UT271025" };
            try
            {
                // Act
                var result = realtimcheckerfinder.FindItemNameDic(yjCds, 20230505);

                // Assert
                Assert.AreEqual(3, result.Count);
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_017_TEST_FindItemNameByItemCode()
        {
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var tenMsts = CommonCheckerData.ReadTenMst(string.Empty, string.Empty);

            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            var itemCd = "UT2720";
            try
            {
                // Act
                var result = realtimcheckerfinder.FindItemNameByItemCode(itemCd, 20230505);

                // Assert
                Assert.AreEqual("UNITTEST", result);
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_018_TEST_FindItemNameByItemCodeDic()
        {
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var tenMsts = CommonCheckerData.ReadTenMst(string.Empty, string.Empty);

            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            var yjCds = new List<string> { "UT2717", "UT2718", "UT2720" };
            try
            {
                // Act
                var result = realtimcheckerfinder.FindItemNameByItemCodeDic(yjCds, 20230505);

                // Assert
                Assert.AreEqual(3, result.Count);
            }
            finally
            {
                tenantTracking.TenMsts.RemoveRange(tenMsts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_019_TEST_FindKijyoComment()
        {
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = "UT5555";
            var cd2 = "UT6666";
            var cd3 = "UT7777";
            var m01KijyoCmts = new List<M01KijyoCmt>()
            {
                new M01KijyoCmt()
                {
                    CmtCd = cd1,
                    Cmt = null,
                },
                new M01KijyoCmt()
                {
                    CmtCd = cd2,
                    Cmt = "UNITTEST2",
                },
                new M01KijyoCmt()
                {
                    CmtCd = cd3,
                    Cmt = "",
                },
            };

            tenantTracking.M01KijyoCmt.AddRange(m01KijyoCmts);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            try
            {
                // Act
                var result1 = realtimcheckerfinder.FindKijyoComment(cd1);
                var result2 = realtimcheckerfinder.FindKijyoComment(cd2);
                var result3 = realtimcheckerfinder.FindKijyoComment(cd3);

                // Assert
                Assert.AreEqual(string.Empty, result1);
                Assert.AreEqual("UNITTEST2", result2);
                Assert.AreEqual(string.Empty, result3);
            }
            finally
            {
                tenantTracking.M01KijyoCmt.RemoveRange(m01KijyoCmts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_020_TEST_FindKijyoCommentDic()
        {
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = "UT5555";
            var cd2 = "UT6666";
            var cd3 = "UT7777";
            var m01KijyoCmts = new List<M01KijyoCmt>()
            {
                new M01KijyoCmt()
                {
                    CmtCd = cd1,
                    Cmt = null,
                },
                new M01KijyoCmt()
                {
                    CmtCd = cd2,
                    Cmt = "UNITTEST2",
                },
                new M01KijyoCmt()
                {
                    CmtCd = cd3,
                    Cmt = "",
                },
            };

            tenantTracking.M01KijyoCmt.AddRange(m01KijyoCmts);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            var commentCodes = new List<string> { cd1, cd2, cd3 };
            try
            {
                // Act
                var result = realtimcheckerfinder.FindKijyoCommentDic(commentCodes);

                // Assert
                Assert.AreEqual(3, result.Count);
            }
            finally
            {
                tenantTracking.M01KijyoCmt.RemoveRange(m01KijyoCmts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_021_TEST_FindKinkiComment()
        {
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = "UT5555";
            var cd2 = "UT6666";
            var cd3 = "UT7777";
            var m01KinkiCmts = new List<M01KinkiCmt>()
            {
                new M01KinkiCmt()
                {
                    CmtCd = cd1,
                    Cmt = null,
                },
                new M01KinkiCmt()
                {
                    CmtCd = cd2,
                    Cmt = "UNITTEST2",
                },
                new M01KinkiCmt()
                {
                    CmtCd = cd3,
                    Cmt = "",
                },
            };

            tenantTracking.M01KinkiCmt.AddRange(m01KinkiCmts);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            try
            {
                // Act
                var result1 = realtimcheckerfinder.FindKinkiComment(cd1);
                var result2 = realtimcheckerfinder.FindKinkiComment(cd2);
                var result3 = realtimcheckerfinder.FindKinkiComment(cd3);

                // Assert
                Assert.AreEqual(string.Empty, result1);
                Assert.AreEqual("UNITTEST2", result2);
                Assert.AreEqual(string.Empty, result3);
            }
            finally
            {
                tenantTracking.M01KinkiCmt.RemoveRange(m01KinkiCmts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_022_TEST_FindKinkiCommentDic()
        {
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = "UT5555";
            var cd2 = "UT6666";
            var cd3 = "UT7777";
            var m01KinkiCmts = new List<M01KinkiCmt>()
            {
                new M01KinkiCmt()
                {
                    CmtCd = cd1,
                    Cmt = null,
                },
                new M01KinkiCmt()
                {
                    CmtCd = cd2,
                    Cmt = "UNITTEST2",
                },
                new M01KinkiCmt()
                {
                    CmtCd = cd3,
                    Cmt = "",
                },
            };

            tenantTracking.M01KinkiCmt.AddRange(m01KinkiCmts);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            var commentCodes = new List<string> { cd1, cd2, cd3 };
            try
            {
                // Act
                var result = realtimcheckerfinder.FindKinkiCommentDic(commentCodes);

                // Assert
                Assert.AreEqual(3, result.Count);
            }
            finally
            {
                tenantTracking.M01KinkiCmt.RemoveRange(m01KinkiCmts);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_023_TEST_FindOTCItemName()
        {
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = 999999;
            var cd2 = 999998;
            var cd3 = 999997;
            var m38OtcMains = new List<M38OtcMain>()
            {
                new M38OtcMain()
                {
                    SerialNum = cd1,
                    TradeName = null,
                },
                new M38OtcMain()
                {
                    SerialNum = cd2,
                    TradeName = "UNITTEST2",
                },
                new M38OtcMain()
                {
                    SerialNum = cd3,
                    TradeName = "",
                },
            };

            tenantTracking.M38OtcMain.AddRange(m38OtcMains);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            try
            {
                // Act
                var result1 = realtimcheckerfinder.FindOTCItemName(cd1);
                var result2 = realtimcheckerfinder.FindOTCItemName(cd2);
                var result3 = realtimcheckerfinder.FindOTCItemName(cd3);

                // Assert
                Assert.AreEqual(string.Empty, result1);
                Assert.AreEqual("UNITTEST2", result2);
                Assert.AreEqual(string.Empty, result3);
            }
            finally
            {
                tenantTracking.M38OtcMain.RemoveRange(m38OtcMains);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_024_TEST_FindOTCItemNameDic()
        {
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = 999999;
            var cd2 = 999998;
            var cd3 = 999997;
            var m38OtcMains = new List<M38OtcMain>()
            {
                new M38OtcMain()
                {
                    SerialNum = cd1,
                    TradeName = null,
                },
                new M38OtcMain()
                {
                    SerialNum = cd2,
                    TradeName = "UNITTEST2",
                },
                new M38OtcMain()
                {
                    SerialNum = cd3,
                    TradeName = "",
                },
            };

            tenantTracking.M38OtcMain.AddRange(m38OtcMains);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            var commentCodes = new List<string> { cd1.AsString(), cd2.AsString(), cd3.AsString() };
            try
            {
                // Act
                var result = realtimcheckerfinder.FindOTCItemNameDic(commentCodes);

                // Assert
                Assert.AreEqual(3, result.Count);
            }
            finally
            {
                tenantTracking.M38OtcMain.RemoveRange(m38OtcMains);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_025_TEST_FindSuppleItemName()
        {
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = "UT5555";
            var cd2 = "UT6666";
            var cd3 = "UT7777";
            var m41SuppleIngres = new List<M41SuppleIngre>()
            {
                new M41SuppleIngre()
                {
                    SeibunCd = cd1,
                    Seibun = null,
                },
                new M41SuppleIngre()
                {
                    SeibunCd = cd2,
                    Seibun = "UNITTEST2",
                },
                new M41SuppleIngre()
                {
                    SeibunCd = cd3,
                    Seibun = "",
                },
            };

            tenantTracking.M41SuppleIngres.AddRange(m41SuppleIngres);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            try
            {
                // Act
                var result1 = realtimcheckerfinder.FindSuppleItemName(cd1);
                var result2 = realtimcheckerfinder.FindSuppleItemName(cd2);
                var result3 = realtimcheckerfinder.FindSuppleItemName(cd3);

                // Assert
                Assert.AreEqual(string.Empty, result1);
                Assert.AreEqual("UNITTEST2", result2);
                Assert.AreEqual(string.Empty, result3);
            }
            finally
            {
                tenantTracking.M41SuppleIngres.RemoveRange(m41SuppleIngres);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_026_TEST_FindKinkiCommentDic()
        {
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = "UT5555";
            var cd2 = "UT6666";
            var cd3 = "UT7777";
            var m41SuppleIngres = new List<M41SuppleIngre>()
            {
                new M41SuppleIngre()
                {
                    SeibunCd = cd1,
                    Seibun = null,
                },
                new M41SuppleIngre()
                {
                    SeibunCd = cd2,
                    Seibun = "UNITTEST2",
                },
                new M41SuppleIngre()
                {
                    SeibunCd = cd3,
                    Seibun = "",
                },
            };

            tenantTracking.M41SuppleIngres.AddRange(m41SuppleIngres);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            var commentCodes = new List<string> { cd1, cd2, cd3 };
            try
            {
                // Act
                var result = realtimcheckerfinder.FindSuppleItemNameDic(commentCodes);

                // Assert
                Assert.AreEqual(3, result.Count);
            }
            finally
            {
                tenantTracking.M41SuppleIngres.RemoveRange(m41SuppleIngres);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_027_TEST_GetOTCComponentInfo()
        {
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = "UT5555";
            var cd2 = "UT6666";
            var cd3 = "UT7777";
            var m38IngCodes = new List<M38IngCode>()
            {
                new M38IngCode()
                {
                    SeibunCd = cd1,
                    Seibun = null,
                },
                new M38IngCode()
                {
                    SeibunCd = cd2,
                    Seibun = "UNITTEST2",
                },
                new M38IngCode()
                {
                    SeibunCd = cd3,
                    Seibun = "",
                },
            };

            tenantTracking.M38IngCode.AddRange(m38IngCodes);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            try
            {
                // Act
                var result1 = realtimcheckerfinder.GetOTCComponentInfo(cd1);
                var result2 = realtimcheckerfinder.GetOTCComponentInfo(cd2);
                var result3 = realtimcheckerfinder.GetOTCComponentInfo(cd3);

                // Assert
                Assert.AreEqual(string.Empty, result1);
                Assert.AreEqual("UNITTEST2", result2);
                Assert.AreEqual(string.Empty, result3);
            }
            finally
            {
                tenantTracking.M38IngCode.RemoveRange(m38IngCodes);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_028_TEST_GetOTCComponentInfoDic()
        {
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = "UT5555";
            var cd2 = "UT6666";
            var cd3 = "UT7777";
            var m38IngCodes = new List<M38IngCode>()
            {
                new M38IngCode()
                {
                    SeibunCd = cd1,
                    Seibun = null,
                },
                new M38IngCode()
                {
                    SeibunCd = cd2,
                    Seibun = "UNITTEST2",
                },
                new M38IngCode()
                {
                    SeibunCd = cd3,
                    Seibun = "",
                },
            };

            tenantTracking.M38IngCode.AddRange(m38IngCodes);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            var commentCodes = new List<string> { cd1, cd2, cd3 };
            try
            {
                // Act
                var result = realtimcheckerfinder.GetOTCComponentInfoDic(commentCodes);

                // Assert
                Assert.AreEqual(3, result.Count);
            }
            finally
            {
                tenantTracking.M38IngCode.RemoveRange(m38IngCodes);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_029_TEST_GetSupplementComponentInfo()
        {
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = "UT5555";
            var cd2 = "UT6666";
            var cd3 = "UT7777";
            var m41SuppleIngres = new List<M41SuppleIngre>()
            {
                new M41SuppleIngre()
                {
                    SeibunCd = cd1,
                    Seibun = null,
                },
                new M41SuppleIngre()
                {
                    SeibunCd = cd2,
                    Seibun = "UNITTEST2",
                },
                new M41SuppleIngre()
                {
                    SeibunCd = cd3,
                    Seibun = "",
                },
            };

            tenantTracking.M41SuppleIngres.AddRange(m41SuppleIngres);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            try
            {
                // Act
                var result1 = realtimcheckerfinder.GetSupplementComponentInfo(cd1);
                var result2 = realtimcheckerfinder.GetSupplementComponentInfo(cd2);
                var result3 = realtimcheckerfinder.GetSupplementComponentInfo(cd3);

                // Assert
                Assert.AreEqual(string.Empty, result1);
                Assert.AreEqual("UNITTEST2", result2);
                Assert.AreEqual(string.Empty, result3);
            }
            finally
            {
                tenantTracking.M41SuppleIngres.RemoveRange(m41SuppleIngres);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_030_TEST_GetSupplementComponentInfoDic()
        {
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = "UT5555";
            var cd2 = "UT6666";
            var cd3 = "UT7777";
            var m41SuppleIngres = new List<M41SuppleIngre>()
            {
                new M41SuppleIngre()
                {
                    SeibunCd = cd1,
                    Seibun = null,
                },
                new M41SuppleIngre()
                {
                    SeibunCd = cd2,
                    Seibun = "UNITTEST2",
                },
                new M41SuppleIngre()
                {
                    SeibunCd = cd3,
                    Seibun = "",
                },
            };

            tenantTracking.M41SuppleIngres.AddRange(m41SuppleIngres);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            var commentCodes = new List<string> { cd1, cd2, cd3 };
            try
            {
                // Act
                var result = realtimcheckerfinder.GetSupplementComponentInfoDic(commentCodes);

                // Assert
                Assert.AreEqual(3, result.Count);
            }
            finally
            {
                tenantTracking.M41SuppleIngres.RemoveRange(m41SuppleIngres);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_031_TEST_GetUsageDosage()
        {
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = "UT5555";
            var cd2 = "UT6666";
            var cd3 = "UT7777";

            var yjCd1 = "YJCDTEST1";
            var yjCd2 = "YJCDTEST2";
            var yjCd3 = "YJCDTEST3";
            var dosageDrugs = new List<DosageDrug>()
            {
                new DosageDrug()
                {
                    YjCd = yjCd1,
                    DoeiCd = cd1,
                },
                new DosageDrug()
                {
                    YjCd = yjCd2,
                    DoeiCd = cd2,
                },
                new DosageDrug()
                {
                    YjCd = yjCd3,
                    DoeiCd = cd3,
                },
            };

            var dosageDosages = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    DoeiCd = cd1,
                    DoeiSeqNo = 9999,
                    UsageDosage = "Test1"
                },
                new DosageDosage()
                {
                    DoeiCd = cd2,
                    DoeiSeqNo = 9999,
                    UsageDosage = "Test2"
                },
                new DosageDosage()
                {
                    DoeiCd = cd3,
                    DoeiSeqNo = 9999,
                    UsageDosage = "Test3"
                },
            };

            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.DosageDosages.AddRange(dosageDosages);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            try
            {
                // Act
                var result1 = realtimcheckerfinder.GetUsageDosage(yjCd1);
                var result2 = realtimcheckerfinder.GetUsageDosage(yjCd2);
                var result3 = realtimcheckerfinder.GetUsageDosage(yjCd3);

                // Assert
                Assert.AreEqual("Test1", result1);
                Assert.AreEqual("Test2", result2);
                Assert.AreEqual("Test3", result3);
            }
            finally
            {
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.DosageDosages.RemoveRange(dosageDosages);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_032_TEST_GetUsageDosageDic()
        {
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = "UT5555";
            var cd2 = "UT6666";
            var cd3 = "UT7777";

            var yjCd1 = "YJCDTEST1";
            var yjCd2 = "YJCDTEST2";
            var yjCd3 = "YJCDTEST3";
            var dosageDrugs = new List<DosageDrug>()
            {
                new DosageDrug()
                {
                    YjCd = yjCd1,
                    DoeiCd = cd1,
                },
                new DosageDrug()
                {
                    YjCd = yjCd2,
                    DoeiCd = cd2,
                },
                new DosageDrug()
                {
                    YjCd = yjCd3,
                    DoeiCd = cd3,
                },
            };

            var dosageDosages = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    DoeiCd = cd1,
                    DoeiSeqNo = 9999,
                    UsageDosage = "Test1"
                },
                new DosageDosage()
                {
                    DoeiCd = cd2,
                    DoeiSeqNo = 9999,
                    UsageDosage = "Test2"
                },
                new DosageDosage()
                {
                    DoeiCd = cd3,
                    DoeiSeqNo = 9999,
                    UsageDosage = "Test3"
                },
            };

            tenantTracking.DosageDrugs.AddRange(dosageDrugs);
            tenantTracking.DosageDosages.AddRange(dosageDosages);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            var yjCds = new List<string>() {yjCd1, yjCd2, yjCd3 };
            try
            {
                // Act
                var result = realtimcheckerfinder.GetUsageDosageDic(yjCds);

                // Assert
                Assert.AreEqual(3, result.Count);
            }
            finally
            {
                tenantTracking.DosageDrugs.RemoveRange(dosageDrugs);
                tenantTracking.DosageDosages.RemoveRange(dosageDosages);
                tenantTracking.SaveChanges();
            }
        }

        [Test]
        public void TC_033_TEST_IsNoMasterData()
        {
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();

            var yjCd = "YJCDTEST999";
            var m56ExEdIngredients = new List<M56ExEdIngredients>()
            {
                new M56ExEdIngredients()
                {
                    YjCd = yjCd,
                    SeqNo = "999",
                }
            };

            tenantTracking.M56ExEdIngredients.AddRange(m56ExEdIngredients);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            try
            {
                // Act
                var result = realtimcheckerfinder.IsNoMasterData();

                // Assert
                Assert.AreEqual(false, result);
            }
            finally
            {
                tenantTracking.M56ExEdIngredients.RemoveRange(m56ExEdIngredients);
                tenantTracking.SaveChanges();
                realtimcheckerfinder.DisposeDataContext();
            }
        }
    }
}
