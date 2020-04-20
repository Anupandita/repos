using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Constants;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;

namespace Sfc.Wms.App.Api.Tests.Unit.Nuget
{
    [TestClass]
    [TestCategory(TestCategories.Unit)]
    public class CorbaGatewayTest : CorbaGatewayFixture
    {
        [TestMethod]
        public void Single_Corba_Invocation_Returned_Ok_As_Response_Status()
        {
            ValidInputForSingleCorba();
            InvokeSingleCorba();
            SingleCorbaInvocationReturnedOkAsResponse();
        }

        [TestMethod]
        public void Single_Corba_Invocation_Returned_BadRequest_As_Response_Status()
        {
            EmptyOrBadInputForSingleCorba();
            InvokeSingleCorba();
            SingleCorbaInvocationReturnedBadRequestAsResponse();
        }

        [TestMethod]
        public void Single_Corba_Invocation_Returned_Exception_As_Response_Status()
        {
            InValidInputForSingleCorba();
            InvokeSingleCorba();
            SingleCorbaInvocationReturnedExceptionAsResponse();
        }

        [TestMethod]
        public void Batch_Corba_Invocation_Returned_Ok_As_Response_Status()
        {
            ValidInputForBatchCorba();
            InvokeBatchCorba();
            BatchCorbaInvocationReturnedOkAsResponse();
        }

        [TestMethod]
        public void Batch_Corba_Invocation_Returned_BadRequest_As_Response_Status()
        {
            EmptyOrBadInputForBatchCorba();
            InvokeBatchCorba();
            BatchCorbaInvocationReturnedBadRequestAsResponse();
        }

        [TestMethod]
        public void Batch_Corba_Invocation_Returned_Exception_As_Response_Status()
        {
            InValidInputForBatchCorba();
            InvokeBatchCorba();
            BatchCorbaInvocationReturnedExceptionAsResponse();
        }
    }
}
