using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.App.Api.Controllers;
using Sfc.Wms.Interface.Asrs.Dtos;
using Sfc.Wms.Interface.Asrs.Interfaces;
using Sfc.Wms.Result;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Sfc.App.Api.Tests.Unit.Fixtures
{
    public abstract class IvmtFixture
    {
        private readonly InventoryMaintenanceController _ivmtController;
        private readonly Mock<IWmsToEmsMessageProcessorService> _messageTypeService;
        private Task<IHttpActionResult> _testResult;

        protected IvmtFixture()
        {
            _messageTypeService = new Mock<IWmsToEmsMessageProcessorService>(MockBehavior.Default);
            _ivmtController = new InventoryMaintenanceController(_messageTypeService.Object);
        }

        protected void ValidIvmtMessage()
        {
            var response = new BaseResult()
            {
                ResultType = ResultTypes.Created
            };

            _messageTypeService.Setup(el => el.GetIvmtMessageAsync(It.IsAny<IvmtTriggerInputDto>()))
                .Returns(Task.FromResult(response));
        }

        protected void InvalidIvmtMessage()
        {
            var response = new BaseResult()
            {
                ResultType = ResultTypes.BadRequest
            };

            _messageTypeService.Setup(el => el.GetIvmtMessageAsync(It.IsAny<IvmtTriggerInputDto>()))
                .Returns(Task.FromResult(response));
        }

        protected void InsertMessageInvoked()
        {
            _testResult = _ivmtController.CreateAsync(Generator.Default.Single<IvmtTriggerInputDto>());
        }

        protected void IvmtMessageShouldBeProcessed()
        {
            var result = _testResult.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.Created);
        }

        protected void IvmtMessageShouldNotBeProcessed()
        {
            var result = _testResult.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.BadRequest);
        }
    }
}