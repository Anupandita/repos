using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.Wms.Asrs.Api.Controllers.Dematic;
using Sfc.Wms.Interface.Asrs.Interfaces;
using Sfc.Wms.Result;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Sfc.App.Api.Tests.Unit.Fixtures
{
    public abstract class EmsToWmsMessageFixture
    {
        private readonly EmsToWmsMessageController _emsToWmsMessageController;
        private readonly Mock<IEmsToWmsMessageProcessorSevice> _emsToWmsMessageProcessorService;
        private Task<IHttpActionResult> testResult;

        protected EmsToWmsMessageFixture()
        {
            _emsToWmsMessageProcessorService = new Mock<IEmsToWmsMessageProcessorSevice>(MockBehavior.Default);
            _emsToWmsMessageController = new EmsToWmsMessageController(_emsToWmsMessageProcessorService.Object);
        }

        protected void ValidMessageKey()
        {
            var response = new BaseResult()
            {
                ResultType = ResultTypes.Ok
            };

            _emsToWmsMessageProcessorService.Setup(el => el.GetMessageAsync(It.IsAny<long>()))
                .Returns(Task.FromResult(response));
        }

        protected void InvalidMessageKey()
        {
            var response = new BaseResult()
            {
                ResultType = ResultTypes.BadRequest
            };

            _emsToWmsMessageProcessorService.Setup(el => el.GetMessageAsync(It.IsAny<long>()))
                .Returns(Task.FromResult(response));
        }

        protected void EmsToWmsMessageProcessorInvoked()
        {
            testResult = _emsToWmsMessageController.CreateAsync(It.IsAny<long>());
        }

        protected void EmsToWmsMessageShouldBeProcessed()
        {
            var result = testResult.Result as OkNegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.Ok);
        }

        protected void EmsToWmsMessageShouldNotBeProcessed()
        {
            var result = testResult.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.BadRequest);
        }
    }
}