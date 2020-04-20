using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Controllers;
using Sfc.Wms.Interfaces.Asrs.Contracts.Dtos;
using Sfc.Wms.Interfaces.Asrs.Contracts.Interfaces;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Sfc.Wms.App.Api.Tests.Unit.Fixtures
{
    public abstract class SynrFixture
    {
        private readonly SynchronizationRequestController _synchronizationRequestController;
        private readonly Mock<IWmsToEmsMessageProcessorService> _messageTypeService;
        private Task<IHttpActionResult> _testResult;

        protected SynrFixture()
        {
            _messageTypeService = new Mock<IWmsToEmsMessageProcessorService>(MockBehavior.Default);
            _synchronizationRequestController = new SynchronizationRequestController(_messageTypeService.Object);
        }

        protected void ValidSynchronizationRequestMessage()
        {
            var response = new BaseResult()
            {
                ResultType = ResultTypes.Created
            };

            _messageTypeService.Setup(el => el.SynchronizationRequestAsync())
                .Returns(Task.FromResult(response));
        }

        protected void InvalidSynchronizationRequestMessage()
        {
            var response = new BaseResult()
            {
                ResultType = ResultTypes.BadRequest
            };

            _messageTypeService.Setup(el => el.SynchronizationRequestAsync())
                .Returns(Task.FromResult(response));
        }

        protected void InsertMessageInvoked()
        {
            _testResult = _synchronizationRequestController.CreateAsync();
            _messageTypeService.VerifyAll();
        }

        protected void SynchronizationRequestMessageShouldBeProcessed()
        {
            var result = _testResult.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.Created, result.Content.ResultType);
        }

        protected void SynchronizationRequestMessageShouldNotBeProcessed()
        {
            var result = _testResult.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.BadRequest, result.Content.ResultType);
        }
    }
}