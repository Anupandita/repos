using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures.UIFixtures;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Constant;
using TestStack.BDDfy;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests.UITests
{
    [TestClass]
    [Story(
    Title = "Sysocde Api Api Testing",
    AsA = "Authorized User test for SysCode Apis",
    IWant = "To Test for SysCode Api  ",
    SoThat = "I can fetch data from sys code table using the RecType and CodeType",
    StoryUri = ""
     )]
    public class SysCodeApiTest:SysCodeFixture
    {

        [TestInitialize]
        protected void AValidTestData()
        {
            LoginToFetchToken();
            GetSysCodeRecordsFromDbForRecTypeCodeType(UIConstants.RecType,UIConstants.CodeType);
        }

        [TestMethod()]
        [TestCategory("UI_FUNCTIONAL")]
        protected void SysCodeApiTesting()
        {
            this.Given(x=>x.CreateSysCodeApiUrl())
             .When(x => x.CallSysCodeApi(UIConstants.SysCodeUrl))
            .Then(x => x.VerifySysCodeApiOutputAgainstDbOutput())
            .BDDfy("Test Case ID : ");
        }
    }
}
