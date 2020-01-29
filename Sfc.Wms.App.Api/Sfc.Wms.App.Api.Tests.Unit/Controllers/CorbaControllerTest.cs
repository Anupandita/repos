using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Constants;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;

namespace Sfc.Wms.App.Api.Tests.Unit.Controllers
{
    [TestClass]
    [TestCategory(TestCategories.Unit)]
    public class CorbaControllerTest : CorbaControllerFixture
    {
        [TestMethod]
        public void Single_Corba_Invocation_Returned_Ok_As_Response_Status()
        {
            ValidInputParametersForSingleCorba();
            SingleCorbaInvoked();
            SingleCorbaInvocationReturnedOkAsResponse();
        }

        [TestMethod]
        public void Single_Corba_Invocation_Returned_Exception_As_Response_Status()
        {
            InValidInputParametersForSingleCorba();
            SingleCorbaInvoked();
            SingleCorbaInvocationReturnedExceptionAsResponse();
        }


        [TestMethod]
        public void Batch_Corba_Invocation_Returned_Ok_As_Response_Status()
        {
            ValidInputParametersForBatchCorba();
            BatchCorbaInvoked();
            BatchCorbaInvocationReturnedOkAsResponse();
        }

        [TestMethod]
        public void Batch_Corba_Invocation_Returned_Exception_As_Response_Status()
        {
            InValidInputParametersForBatchCorba();
            BatchCorbaInvoked();
            BatchCorbaInvocationReturnedExceptionAsResponse();
        }
    }
}
