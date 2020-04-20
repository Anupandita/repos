using Moq;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Controllers;
using Sfc.Wms.Foundation.Corba.Contracts.Dtos;
using Sfc.Wms.Foundation.Corba.Contracts.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sfc.Wms.App.Api.Tests.Unit.Fixtures
{
    public class CorbaControllerFixture
    {
        protected readonly Mock<ICorbaService> _mock;
        protected readonly CorbaController _corbaController;
        private IHttpActionResult testResult;

        protected CorbaControllerFixture()
        {
            _mock = new Mock<ICorbaService>(MockBehavior.Default);
            _corbaController = new CorbaController(_mock.Object);
        }

        private void MockSingleCorba(ResultTypes resultTypes = ResultTypes.Ok)
        {
            var response = new BaseResult
            {
                ResultType = resultTypes
            };
            _mock.Setup(el => el.SingleCorbaAsync(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<CorbaDto>())).Returns(Task.FromResult(response));
        }

        private void MockBatchCorba(ResultTypes resultTypes = ResultTypes.Ok)
        {
            var response = new BaseResult<CorbaResponseDto>
            {
                ResultType = resultTypes,
                Payload = resultTypes == ResultTypes.Ok ? new CorbaResponseDto() : null
            };
            _mock.Setup(el => el.BatchCorbaAsync(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<List<CorbaDto>>())).Returns(Task.FromResult(response));
        }

        private void VerifySingleCorba()
        {
            _mock.Verify(el => el.SingleCorbaAsync(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<CorbaDto>()));
        }

        private void VerifyBatchCorba()
        {
            _mock.Verify(el => el.BatchCorbaAsync(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<List<CorbaDto>>()));
        }

        protected void ValidInputParametersForSingleCorba()
        {
            MockSingleCorba();
        }

        protected void InValidInputParametersForSingleCorba()
        {
            MockSingleCorba(ResultTypes.ExpectationFailed);
        }

        protected void SingleCorbaInvoked()
        {
            testResult = _corbaController.SingleCorbaAsync(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<CorbaDto>()).Result;
        }

        protected void SingleCorbaInvocationReturnedOkAsResponse()
        {
            VerifySingleCorba();
            var result = testResult as OkNegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.Ok,result.Content.ResultType);
        }

        protected void SingleCorbaInvocationReturnedExceptionAsResponse()
        {
            VerifySingleCorba();
            var result = testResult as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.ExpectationFailed, result.Content.ResultType);
        }
        protected void ValidInputParametersForBatchCorba()
        {
            MockBatchCorba();
        }

        protected void InValidInputParametersForBatchCorba()
        {
            MockBatchCorba(ResultTypes.ExpectationFailed);
        }

        protected void BatchCorbaInvoked()
        {
            testResult = _corbaController.BatchCorbaAsync(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<List<CorbaDto>>()).Result;
        }

        protected void BatchCorbaInvocationReturnedOkAsResponse()
        {
            VerifyBatchCorba();
            var result = testResult as OkNegotiatedContentResult<BaseResult<CorbaResponseDto>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.Ok, result.Content.ResultType);
            Assert.IsNotNull(result.Content.Payload);

        }

        protected void BatchCorbaInvocationReturnedExceptionAsResponse()
        {
            VerifyBatchCorba();
            var result = testResult as NegotiatedContentResult<BaseResult<CorbaResponseDto>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.ExpectationFailed, result.Content.ResultType);
        }
    }
}
