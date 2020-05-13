using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Controllers;
using Sfc.Wms.Interfaces.Asrs.Contracts.Interfaces;

namespace Sfc.Wms.App.Api.Tests.Unit.Fixtures
{
    public abstract class EmsToWmsMessageFixture
    {
        private readonly EmsToWmsMessageController _emsToWmsMessageController;
        private readonly Mock<IEmsToWmsMessageProcessorService> _emsToWmsMessageProcessorService;
        private Task<IHttpActionResult> testResult;

        protected EmsToWmsMessageFixture()
        {
            _emsToWmsMessageProcessorService = new Mock<IEmsToWmsMessageProcessorService>(MockBehavior.Default);
            _emsToWmsMessageController = new EmsToWmsMessageController(_emsToWmsMessageProcessorService.Object);
        }

        protected void ValidMessageKey()
        {
            var response = new BaseResult
            {
                ResultType = ResultTypes.Created
            };

            _emsToWmsMessageProcessorService.Setup(el => el.GetMessageAsync(It.IsAny<long>(), It.IsAny<string>()))
                .Returns(Task.FromResult(response));
        }

        protected void InvalidMessageKey()
        {
            var response = new BaseResult
            {
                ResultType = ResultTypes.BadRequest
            };

            _emsToWmsMessageProcessorService.Setup(el => el.GetMessageAsync(It.IsAny<long>(), It.IsAny<string>()))
                .Returns(Task.FromResult(response));
        }

        protected void EmsToWmsMessageProcessorInvoked()
        {
            testResult = _emsToWmsMessageController.CreateAsync(It.IsAny<long>(), It.IsAny<string>());
        }

        protected void EmsToWmsMessageShouldBeProcessed()
        {
            var result = testResult.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.Created);
        }

        protected void EmsToWmsMessageShouldNotBeProcessed()
        {
            var result = testResult.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.BadRequest);
        }
    }
}