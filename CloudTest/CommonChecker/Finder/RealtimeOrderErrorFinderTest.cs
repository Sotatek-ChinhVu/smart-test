using CloudUnitTest.SampleData;
using CommonChecker.DB;
using Entity.Tenant;
using Helper.Extension;

namespace CloudUnitTest.CommonChecker.Finder
{
    public class RealtimeOrderErrorFinderTest : BaseUT
    {
        [Test]
        public void TC_001_FindAgeComment_Test_AgeCommentInfo_Is_Not_Null()
        {

            //Setup Data Test
            int hpId = 1;
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var testCmt1 = "UT6666";
            var testCmt2 = "UT7777";
            var ageCommentInfo = new List<M14CmtCode>()
            {
                new M14CmtCode()
                {
                    HpId = 1,
                    AttentionCmt = null,
                    AttentionCmtCd = testCmt1,
                },
                new M14CmtCode()
                {
                    HpId = 1,
                    AttentionCmt = "COMMENT-Test",
                    AttentionCmtCd = testCmt2,
                },
            };

            tenantTracking.M14CmtCode.AddRange(ageCommentInfo);

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            try
            {
                tenantTracking.SaveChanges();

                // Act
                var result1 = realtimcheckerfinder.FindAgeComment(hpId, testCmt1);
                var result2 = realtimcheckerfinder.FindAgeComment(hpId, testCmt2);

                // Assert
                Assert.That(result1, Is.EqualTo(string.Empty));
                Assert.That(result2, Is.EqualTo("COMMENT-Test"));
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
            int hpId = 1;
            //Setup Data Test
            var testCmt1 = "UT6666";
            var testCmt2 = "UT7777";

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            // Act
            var result1 = realtimcheckerfinder.FindAgeComment(hpId, testCmt1);
            var result2 = realtimcheckerfinder.FindAgeComment(hpId, testCmt2);

            // Assert
            Assert.That(result1, Is.EqualTo(string.Empty));
            Assert.That(result2, Is.EqualTo(string.Empty));
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
                    HpId = 1,
                    AnalogueCd = testAnalogueCd1,
                    AnalogueName = null,
                },
                new M56AnalogueCd()
                {
                    HpId = 1,
                    AnalogueCd = testAnalogueCd2,
                    AnalogueName = "Analogue-Name-Test",
                },
            };

            tenantTracking.M56AnalogueCd.AddRange(m56AnalogueCds);

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            try
            {
                tenantTracking.SaveChanges();

                // Act
                var result1 = realtimcheckerfinder.FindAnalogueName(1, testAnalogueCd1);
                var result2 = realtimcheckerfinder.FindAnalogueName(1, testAnalogueCd2);

                // Assert
                Assert.That(result1, Is.EqualTo(string.Empty));
                Assert.That(result2, Is.EqualTo("Analogue-Name-Test"));
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
            int hpId = 1;
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var testAnalogueCd1 = "UT7777";
            var testAnalogueCd2 = "UT8888";
            var testAnalogueCd3 = "UT9999";
            var m56AnalogueCds = new List<M56AnalogueCd>()
            {
                new M56AnalogueCd()
                {
                    HpId = hpId,
                    AnalogueCd = testAnalogueCd1,
                    AnalogueName = null,
                },
                new M56AnalogueCd()
                {
                    HpId = hpId,
                    AnalogueCd = testAnalogueCd2,
                    AnalogueName = "Analogue-Name-Test1",
                },
                new M56AnalogueCd()
                {
                    HpId = hpId,
                    AnalogueCd = testAnalogueCd3,
                    AnalogueName = "Analogue-Name-Test2",
                },
            };

            tenantTracking.M56AnalogueCd.AddRange(m56AnalogueCds);

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            var analogueCodeLists = new List<string>() { testAnalogueCd1, testAnalogueCd2 };
            try
            {
                tenantTracking.SaveChanges();
                // Act
                var result = realtimcheckerfinder.FindAnalogueNameDic(hpId, analogueCodeLists);

                // Assert
                Assert.That(result.Count, Is.EqualTo(2));
                Assert.That(result.First().Value, Is.EqualTo(string.Empty));
                Assert.That(result.Last().Value, Is.EqualTo("Analogue-Name-Test1"));
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
                    HpId = 1,
                    ClassCd = classCd1,
                    ClassName = null,
                },
                new M56DrugClass()
                {
                    HpId = 1,
                    ClassCd = classCd2,
                    ClassName = "Analogue-Name-Test1",
                },
                new M56DrugClass()
                {
                    HpId = 1,
                    ClassCd = classCd3,
                    ClassName = "Analogue-Name-Test2",
                },
            };

            tenantTracking.M56DrugClass.AddRange(m56s);

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);
            try
            {
                tenantTracking.SaveChanges();
                // Act
                var result1 = realtimcheckerfinder.FindClassName(1, classCd1);
                var result2 = realtimcheckerfinder.FindClassName(1, classCd2);
                var result3 = realtimcheckerfinder.FindClassName(1, classCd4);

                // Assert
                Assert.That(result1, Is.EqualTo(string.Empty));
                Assert.That(result2, Is.EqualTo("Analogue-Name-Test1"));
                Assert.That(result3, Is.EqualTo(string.Empty));
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
                    HpId = 1,
                    SeibunCd = sebunCd1,
                    SeibunIndexCd = "000",
                    SeibunName = "SeibunName-Test1"
                },
                new M56ExIngCode()
                {
                    HpId = 1,
                    SeibunCd = sebunCd2,
                    SeibunIndexCd = "000",
                    SeibunName = null,
                },
                new M56ExIngCode()
                {
                    HpId = 1,
                    SeibunCd = sebunCd3,
                    SeibunIndexCd = "000",
                    SeibunName = ""
                },
            };

            tenantTracking.M56ExIngCode.AddRange(m56s);

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);
            try
            {
                tenantTracking.SaveChanges();

                // Act
                var result1 = realtimcheckerfinder.FindComponentName(1, sebunCd1);
                var result2 = realtimcheckerfinder.FindComponentName(1, sebunCd2);
                var result3 = realtimcheckerfinder.FindComponentName(1, sebunCd3);

                // Assert
                Assert.That(result1, Is.EqualTo("SeibunName-Test1"));
                Assert.That(result2, Is.EqualTo(string.Empty));
                Assert.That(result3, Is.EqualTo(string.Empty));
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
                    HpId = 1,
                    SeibunCd = sebunCd1,
                    SeibunIndexCd = "000",
                    SeibunName = "SeibunName-Test1"
                },
                new M56ExIngCode()
                {
                    HpId = 1,
                    SeibunCd = sebunCd2,
                    SeibunIndexCd = "000",
                    SeibunName = null,
                },
                new M56ExIngCode()
                {
                    HpId = 1,
                    SeibunCd = sebunCd3,
                    SeibunIndexCd = "000",
                    SeibunName = ""
                },
            };

            tenantTracking.M56ExIngCode.AddRange(m56s);

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            var sebunCds = new List<string> { sebunCd1, sebunCd2, sebunCd3 };
            try
            {
                tenantTracking.SaveChanges();
                // Act
                var result = realtimcheckerfinder.FindComponentNameDic(1, sebunCds);

                // Assert
                Assert.That(result.Count, Is.EqualTo(3));
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
            int hpId = 1;
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cmtCd1 = "UT7777";
            var cmtCd2 = "UT8888";
            var cmtCd3 = "UT9999";
            var m42s = new List<M42ContraCmt>()
            {
                new M42ContraCmt()
                {
                    HpId = 1,
                    CmtCd = cmtCd1,
                    Cmt = null,
                },
                new M42ContraCmt()
                {
                    HpId = 1,
                    CmtCd = cmtCd2,
                    Cmt = "000",
                },
                new M42ContraCmt()
                {
                    HpId = 1,
                    CmtCd = cmtCd3,
                    Cmt = "",
                },
            };

            tenantTracking.M42ContraCmt.AddRange(m42s);

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            try
            {
                tenantTracking.SaveChanges();
                // Act
                var result1 = realtimcheckerfinder.FindDiseaseComment(hpId, cmtCd1);
                var result2 = realtimcheckerfinder.FindDiseaseComment(hpId, cmtCd2);
                var result3 = realtimcheckerfinder.FindDiseaseComment(hpId, cmtCd3);

                // Assert
                Assert.That(result1, Is.EqualTo(string.Empty));
                Assert.That(result2, Is.EqualTo("000"));
                Assert.That(result3, Is.EqualTo(string.Empty));
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
            int hpId = 1;
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = "UT7777";
            var cd2 = "UT8888";
            var cd3 = "UT9999";
            var m42s = new List<M42ContraindiDisCon>()
            {
                new M42ContraindiDisCon()
                {
                    HpId = 1,
                    ByotaiCd = cd1,
                    Byomei = null,
                },
                new M42ContraindiDisCon()
                {
                    HpId = 1,
                    ByotaiCd = cd2,
                    Byomei = "000",
                },
                new M42ContraindiDisCon()
                {
                    HpId = 1,
                    ByotaiCd = cd3,
                    Byomei = "",
                },
            };

            tenantTracking.M42ContraindiDisCon.AddRange(m42s);

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            try
            {
                tenantTracking.SaveChanges();
                // Act
                var result1 = realtimcheckerfinder.FindDiseaseName(hpId, cd1);
                var result2 = realtimcheckerfinder.FindDiseaseName(hpId, cd2);
                var result3 = realtimcheckerfinder.FindDiseaseName(hpId, cd3);

                // Assert
                Assert.That(result1, Is.EqualTo(string.Empty));
                Assert.That(result2, Is.EqualTo("000"));
                Assert.That(result3, Is.EqualTo(string.Empty));
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
            int hpId = 1;
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = "UT7777";
            var cd2 = "UT8888";
            var cd3 = "UT9999";
            var m42s = new List<M42ContraindiDisCon>()
            {
                new M42ContraindiDisCon()
                {
                    HpId = 1,
                    ByotaiCd = cd1,
                    Byomei = null,
                },
                new M42ContraindiDisCon()
                {
                    HpId = 1,
                    ByotaiCd = cd2,
                    Byomei = "000",
                },
                new M42ContraindiDisCon()
                {
                    HpId = 1,
                    ByotaiCd = cd3,
                    Byomei = "",
                },
            };

            tenantTracking.M42ContraindiDisCon.AddRange(m42s);

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            var byotaiCdList = new List<string> { cd1, cd2, cd3 };
            try
            {
                tenantTracking.SaveChanges();
                // Act
                var result = realtimcheckerfinder.FindDiseaseNameDic(hpId, byotaiCdList);

                // Assert
                Assert.That(result.Count, Is.EqualTo(3));
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
                    HpId = 1,
                    DrvalrgyCd = cd1,
                    DrvalrgyName = null,
                },
                new M56DrvalrgyCode()
                {
                    HpId = 1,
                    DrvalrgyCd = cd2,
                    DrvalrgyName = "000",
                },
                new M56DrvalrgyCode()
                {
                    HpId = 1,
                    DrvalrgyCd = cd3,
                    DrvalrgyName = "",
                },
            };

            tenantTracking.M56DrvalrgyCode.AddRange(m56s);

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            var drvalrgyCodeList = new List<string> { cd1, cd2, cd3 };
            try
            {
                tenantTracking.SaveChanges();
                // Act
                var result = realtimcheckerfinder.FindDrvalrgyNameDic(1, drvalrgyCodeList);

                // Assert
                Assert.That(result.Count, Is.EqualTo(3));
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
                    HpId = 1,
                    DrvalrgyCd = cd1,
                    DrvalrgyName = null,
                },
                new M56DrvalrgyCode()
                {
                    HpId = 1,
                    DrvalrgyCd = cd2,
                    DrvalrgyName = "UNITTEST",
                },
                new M56DrvalrgyCode()
                {
                    HpId = 1,
                    DrvalrgyCd = cd3,
                    DrvalrgyName = "",
                },
            };

            tenantTracking.M56DrvalrgyCode.AddRange(m56s);

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            try
            {
                tenantTracking.SaveChanges();
                // Act
                var result1 = realtimcheckerfinder.FindDrvalrgyName(1, cd1);
                var result2 = realtimcheckerfinder.FindDrvalrgyName(1, cd2);
                var result3 = realtimcheckerfinder.FindDrvalrgyName(1, cd3);

                // Assert
                Assert.That(result1, Is.EqualTo(string.Empty));
                Assert.That(result2, Is.EqualTo("UNITTEST"));
                Assert.That(result3, Is.EqualTo(string.Empty));
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
            int hpId = 1;
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = "T7";
            var cd2 = "T8";
            var cd3 = "T9";
            var m12FoodAlrgyKbns = new List<M12FoodAlrgyKbn>()
            {
                new M12FoodAlrgyKbn()
                {
                    HpId = 1,
                    FoodKbn = cd1,
                    FoodName = "UNITTEST1",
                },
                new M12FoodAlrgyKbn()
                {
                    HpId = 1,
                    FoodKbn = cd2,
                    FoodName = "UNITTEST2",
                },
                new M12FoodAlrgyKbn()
                {
                    HpId = 1,
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
                var result1 = realtimcheckerfinder.FindFoodName(hpId, cd1);
                var result2 = realtimcheckerfinder.FindFoodName(hpId, cd2);
                var result3 = realtimcheckerfinder.FindFoodName(hpId, cd3);

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
            int hpId = 1;
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = "T7";
            var cd2 = "T8";
            var cd3 = "T9";
            var m12FoodAlrgyKbns = new List<M12FoodAlrgyKbn>()
            {
                new M12FoodAlrgyKbn()
                {
                    HpId = 1,
                    FoodKbn = cd1,
                    FoodName = "UNITTEST1",
                },
                new M12FoodAlrgyKbn()
                {
                    HpId = 1,
                    FoodKbn = cd2,
                    FoodName = "UNITTEST2",
                },
                new M12FoodAlrgyKbn()
                {
                    HpId = 1,
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
                var result = realtimcheckerfinder.FindFoodNameDic(hpId, codeLists);

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
                    IpnNameCd  = cd1,
                    StartDate = 20230101,
                    SeqNo = 999,
                    IpnName = null
                },
                new IpnNameMst()
                {
                    IpnNameCd  = cd2,
                    StartDate = 20230101,
                    SeqNo = 999,
                    IpnName = "UNIT-Test"
                },
                new IpnNameMst()
                {
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
            int hpId = 999;

            var tenMsts = CommonCheckerData.ReadTenMst(string.Empty, string.Empty);

            tenantTracking.TenMsts.AddRange(tenMsts);
            tenantTracking.SaveChanges();

            // Arrange
            var realtimcheckerfinder = new RealtimeOrderErrorFinder(TenantProvider);

            try
            {
                // Act
                var result = realtimcheckerfinder.FindItemName(hpId, yjCd, 20230505);

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
            int hpId = 1;
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
                var result = realtimcheckerfinder.FindItemNameDic(hpId, yjCds, 20230505);

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
            int hpId = 999;
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
                var result = realtimcheckerfinder.FindItemNameByItemCode(hpId, itemCd, 20230505);

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
            int hpId = 1;
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
                var result = realtimcheckerfinder.FindItemNameByItemCodeDic(hpId, yjCds, 20230505);

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
            int hpId = 1;
            var m01KijyoCmts = new List<M01KijyoCmt>()
            {
                new M01KijyoCmt()
                {
                    HpId = 1,
                    CmtCd = cd1,
                    Cmt = null,
                },
                new M01KijyoCmt()
                {
                    HpId = 1,
                    CmtCd = cd2,
                    Cmt = "UNITTEST2",
                },
                new M01KijyoCmt()
                {
                    HpId = 1,
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
                var result1 = realtimcheckerfinder.FindKijyoComment(hpId, cd1);
                var result2 = realtimcheckerfinder.FindKijyoComment(hpId, cd2);
                var result3 = realtimcheckerfinder.FindKijyoComment(hpId, cd3);

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
            int hpId = 1;
            var m01KijyoCmts = new List<M01KijyoCmt>()
            {
                new M01KijyoCmt()
                {
                    HpId = 1,
                    CmtCd = cd1,
                    Cmt = null,
                },
                new M01KijyoCmt()
                {
                    HpId = 1,
                    CmtCd = cd2,
                    Cmt = "UNITTEST2",
                },
                new M01KijyoCmt()
                {
                    HpId = 1,
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
                var result = realtimcheckerfinder.FindKijyoCommentDic(hpId, commentCodes);

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
            int hpId = 1;
            var m01KinkiCmts = new List<M01KinkiCmt>()
            {
                new M01KinkiCmt()
                {
                    HpId = 1,
                    CmtCd = cd1,
                    Cmt = null,
                },
                new M01KinkiCmt()
                {
                    HpId = 1,
                    CmtCd = cd2,
                    Cmt = "UNITTEST2",
                },
                new M01KinkiCmt()
                {
                    HpId = 1,
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
                var result1 = realtimcheckerfinder.FindKinkiComment(hpId, cd1);
                var result2 = realtimcheckerfinder.FindKinkiComment(hpId, cd2);
                var result3 = realtimcheckerfinder.FindKinkiComment(hpId, cd3);

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
            int hpId = 1;
            var m01KinkiCmts = new List<M01KinkiCmt>()
            {
                new M01KinkiCmt()
                {
                    HpId = 1,
                    CmtCd = cd1,
                    Cmt = null,
                },
                new M01KinkiCmt()
                {
                    HpId = 1,
                    CmtCd = cd2,
                    Cmt = "UNITTEST2",
                },
                new M01KinkiCmt()
                {
                    HpId = 1,
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
                var result = realtimcheckerfinder.FindKinkiCommentDic(hpId, commentCodes);

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
            int hpId = 1;
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = 999999;
            var cd2 = 999998;
            var cd3 = 999997;
            var m38OtcMains = new List<M38OtcMain>()
            {
                new M38OtcMain()
                {
                    HpId = 1,
                    SerialNum = cd1,
                    TradeName = null,
                },
                new M38OtcMain()
                {
                    HpId = 1,
                    SerialNum = cd2,
                    TradeName = "UNITTEST2",
                },
                new M38OtcMain()
                {
                    HpId = 1,
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
                var result1 = realtimcheckerfinder.FindOTCItemName(hpId, cd1);
                var result2 = realtimcheckerfinder.FindOTCItemName(hpId, cd2);
                var result3 = realtimcheckerfinder.FindOTCItemName(hpId, cd3);

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
            int hpId = 1;
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = 999999;
            var cd2 = 999998;
            var cd3 = 999997;
            var m38OtcMains = new List<M38OtcMain>()
            {
                new M38OtcMain()
                {
                    HpId = 1,
                    SerialNum = cd1,
                    TradeName = null,
                },
                new M38OtcMain()
                {
                    HpId = 1,
                    SerialNum = cd2,
                    TradeName = "UNITTEST2",
                },
                new M38OtcMain()
                {
                    HpId = 1,
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
                var result = realtimcheckerfinder.FindOTCItemNameDic(hpId, commentCodes);

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
            int hpId = 1;
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = "UT5555";
            var cd2 = "UT6666";
            var cd3 = "UT7777";
            var m41SuppleIngres = new List<M41SuppleIngre>()
            {
                new M41SuppleIngre()
                {
                    HpId = 1,
                    SeibunCd = cd1,
                    Seibun = null,
                },
                new M41SuppleIngre()
                {
                    HpId = 1,
                    SeibunCd = cd2,
                    Seibun = "UNITTEST2",
                },
                new M41SuppleIngre()
                {
                    HpId = 1,
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
                var result1 = realtimcheckerfinder.FindSuppleItemName(hpId, cd1);
                var result2 = realtimcheckerfinder.FindSuppleItemName(hpId, cd2);
                var result3 = realtimcheckerfinder.FindSuppleItemName(hpId, cd3);

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
            int hpId = 1;
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = "UT5555";
            var cd2 = "UT6666";
            var cd3 = "UT7777";
            var m41SuppleIngres = new List<M41SuppleIngre>()
            {
                new M41SuppleIngre()
                {
                    HpId = 1,
                    SeibunCd = cd1,
                    Seibun = null,
                },
                new M41SuppleIngre()
                {
                    HpId = 1,
                    SeibunCd = cd2,
                    Seibun = "UNITTEST2",
                },
                new M41SuppleIngre()
                {
                    HpId = 1,
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
                var result = realtimcheckerfinder.FindSuppleItemNameDic(hpId, commentCodes);

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
            int hpId = 1;
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = "UT5555";
            var cd2 = "UT6666";
            var cd3 = "UT7777";
            var m38IngCodes = new List<M38IngCode>()
            {
                new M38IngCode()
                {
                    HpId = 1,
                    SeibunCd = cd1,
                    Seibun = null,
                },
                new M38IngCode()
                {
                    HpId = 1,
                    SeibunCd = cd2,
                    Seibun = "UNITTEST2",
                },
                new M38IngCode()
                {
                    HpId = 1,
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
                var result1 = realtimcheckerfinder.GetOTCComponentInfo(hpId, cd1);
                var result2 = realtimcheckerfinder.GetOTCComponentInfo(hpId, cd2);
                var result3 = realtimcheckerfinder.GetOTCComponentInfo(hpId, cd3);

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
            int hpId = 1;
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = "UT5555";
            var cd2 = "UT6666";
            var cd3 = "UT7777";
            var m38IngCodes = new List<M38IngCode>()
            {
                new M38IngCode()
                {
                    HpId = 1,
                    SeibunCd = cd1,
                    Seibun = null,
                },
                new M38IngCode()
                {
                    HpId = 1,
                    SeibunCd = cd2,
                    Seibun = "UNITTEST2",
                },
                new M38IngCode()
                {
                    HpId = 1,
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
                var result = realtimcheckerfinder.GetOTCComponentInfoDic(hpId, commentCodes);

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
            int hpId = 1;
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = "UT5555";
            var cd2 = "UT6666";
            var cd3 = "UT7777";
            var m41SuppleIngres = new List<M41SuppleIngre>()
            {
                new M41SuppleIngre()
                {
                    HpId = 1,
                    SeibunCd = cd1,
                    Seibun = null,
                },
                new M41SuppleIngre()
                {
                    HpId = 1,
                    SeibunCd = cd2,
                    Seibun = "UNITTEST2",
                },
                new M41SuppleIngre()
                {
                    HpId = 1,
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
                var result1 = realtimcheckerfinder.GetSupplementComponentInfo(hpId, cd1);
                var result2 = realtimcheckerfinder.GetSupplementComponentInfo(hpId, cd2);
                var result3 = realtimcheckerfinder.GetSupplementComponentInfo(hpId, cd3);

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
            int hpId = 1;
            //Setup Data Test
            var tenantTracking = TenantProvider.GetTrackingTenantDataContext();
            var cd1 = "UT5555";
            var cd2 = "UT6666";
            var cd3 = "UT7777";
            var m41SuppleIngres = new List<M41SuppleIngre>()
            {
                new M41SuppleIngre()
                {
                    HpId = 1,
                    SeibunCd = cd1,
                    Seibun = null,
                },
                new M41SuppleIngre()
                {
                    HpId = 1,
                    SeibunCd = cd2,
                    Seibun = "UNITTEST2",
                },
                new M41SuppleIngre()
                {
                    HpId = 1,
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
                var result = realtimcheckerfinder.GetSupplementComponentInfoDic(hpId, commentCodes);

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
            int hpId = 1;
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
                    HpId = hpId,
                    YjCd = yjCd1,
                    DoeiCd = cd1,
                },
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = yjCd2,
                    DoeiCd = cd2,
                },
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = yjCd3,
                    DoeiCd = cd3,
                },
            };

            var dosageDosages = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = 1,
                    DoeiCd = cd1,
                    DoeiSeqNo = 9999,
                    UsageDosage = "Test1"
                },
                new DosageDosage()
                {
                    HpId = 1,
                    DoeiCd = cd2,
                    DoeiSeqNo = 9999,
                    UsageDosage = "Test2"
                },
                new DosageDosage()
                {
                    HpId = 1,
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
                var result1 = realtimcheckerfinder.GetUsageDosage(hpId, yjCd1);
                var result2 = realtimcheckerfinder.GetUsageDosage(hpId, yjCd2);
                var result3 = realtimcheckerfinder.GetUsageDosage(hpId, yjCd3);

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
            int hpId = 1;
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
                    HpId = hpId,
                    YjCd = yjCd1,
                    DoeiCd = cd1,
                },
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = yjCd2,
                    DoeiCd = cd2,
                },
                new DosageDrug()
                {
                    HpId = hpId,
                    YjCd = yjCd3,
                    DoeiCd = cd3,
                },
            };

            var dosageDosages = new List<DosageDosage>()
            {
                new DosageDosage()
                {
                    HpId = 1,
                    DoeiCd = cd1,
                    DoeiSeqNo = 9999,
                    UsageDosage = "Test1"
                },
                new DosageDosage()
                {
                    HpId = 1,
                    DoeiCd = cd2,
                    DoeiSeqNo = 9999,
                    UsageDosage = "Test2"
                },
                new DosageDosage()
                {
                    HpId = 1,
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

            var yjCds = new List<string>() { yjCd1, yjCd2, yjCd3 };
            try
            {
                // Act
                var result = realtimcheckerfinder.GetUsageDosageDic(hpId, yjCds);

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
                    HpId = 1,
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
                var result = realtimcheckerfinder.IsNoMasterData(1);

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
