using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Constants;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;

namespace Sfc.Wms.App.Api.Tests.Unit.Nuget
{
    [TestClass, TestCategory(TestCategories.Unit)]
    public class MessageLogGatewayTest : MessageLogGatewayFixture
    {
        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Batch_Insert_Operation_Returned_Ok_As_Response_Status()
        {
            InputParametersForBatchInsertion();
            BatchInsertionOperationInvoked();
            TheReturnedCreatedResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Batch_Insert_Operation_Returned_BadRequest_As_Response_Status()
        {
            EmptyOrNullInputForInsertion();
            BatchInsertionOperationInvoked();
            TheReturnedBadRequestResponse();
        }

    }
}