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
    public abstract class SkmtFixture
    {
        private readonly Mock<IWmsToEmsMessageProcessorService> _messageTypeService;
        private readonly SkuMaintenanceController _skmtController;
        private readonly Mock<IWmsToEmsParallelProcessService> _wmsToEmsParallelProcessService;
        private Task<IHttpActionResult> _testResult;

        protected SkmtFixture()
        {
            _messageTypeService = new Mock<IWmsToEmsMessageProcessorService>(MockBehavior.Default);
            _wmsToEmsParallelProcessService = new Mock<IWmsToEmsParallelProcessService>(MockBehavior.Default);
            _skmtController =
                new SkuMaintenanceController(_messageTypeService.Object, _wmsToEmsParallelProcessService.Object);
        }

        protected void ValidSkmtMessage()
        {
            var response = new BaseResult
            {
                ResultType = ResultTypes.Created
            };

            _messageTypeService.Setup(el => el.GetSkmtMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(response));
        }

        protected void InvalidSkmtMessage()
        {
            var response = new BaseResult
            {
                ResultType = ResultTypes.BadRequest
            };

            _messageTypeService.Setup(el => el.GetSkmtMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(response));
        }

        protected void InsertMessageInvoked()
        {
            _testResult = _skmtController.CreateAsync(It.IsAny<string>(), It.IsAny<string>());
            _messageTypeService.VerifyAll();
        }

        protected void SkmtMessageShouldBeProcessed()
        {
            var result = _testResult.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.Created, result.Content.ResultType);
        }

        protected void SkmtMessageShouldNotBeProcessed()
        {
            var result = _testResult.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.BadRequest, result.Content.ResultType);
        }
    }
}