using Domain.Models.OrdInfDetails;
using Entity.Tenant;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudUnitTest.MedicalCommon.ConvertConversionItemToDetailModel
{
    public class ConvertConversionItemToDetailModelTest : BaseUT
    {
        //[Test]
        //public void ConvertConversionItemToDetailModel_001()
        //{
        //    var tenant = TenantProvider.GetNoTrackingDataContext();
        //    var mockConfiguration = new Mock<IConfiguration>();
        //    mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisHost")]).Returns("10.2.15.78");
        //    mockConfiguration.SetupGet(x => x[It.Is<string>(s => s == "Redis:RedisPort")]).Returns("6379");
        //    var mockUserService = new Mock<IUserInfoService>();
        //    SystemConfRepository systemConfRepository = new SystemConfRepository(TenantProvider, mockConfiguration.Object);
        //    UserRepository userRepository = new UserRepository(TenantProvider, mockConfiguration.Object, mockUserService.Object);
        //    ApprovalinfRepository approvalinfRepository = new ApprovalinfRepository(TenantProvider, userRepository);
        //    TodayOdrRepository todayOdrRepository = new TodayOdrRepository(TenantProvider, systemConfRepository, approvalinfRepository);
        //    int hpId = 1;
        //    OrdInfDetailModel sourceDetail = new OrdInfDetailModel();
        //    TenMst tenMst = new TenMst();
        //    List<IpnNameMst> ipnNameMsts = new List<IpnNameMst>();
        //    int autoSetKohatu = 1;
        //    int autoSetSenpatu = 1;
        //    int autoSetSyohoKbnKohatuDrug = 1;
        //    int autoSetSyohoLimitKohatuDrug = 1;
        //    int autoSetSyohoKbnSenpatuDrug = 1;
        //    int autoSetSyohoLimitSenpatuDrug = 1;
        //    List<KensaMst> kensaMsts = new();
        //    List<IpnMinYakkaMst> ipnMinYakkaMsts = new();
        //    int sinDate = 20240308;
        //    long raiinNo = 0;
        //    long ptId = 0;
        //    int odrKouiKbn = 1;
        //    int kensaIraiCondition1;
        //    int kensaIra = 1;


        //}
    }
}
