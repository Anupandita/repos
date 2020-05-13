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
    public abstract class OrmtFixture
    {
        private readonly Mock<IWmsToEmsMessageProcessorService> _messageTypeService;
        private readonly OrderMaintenanceController _ormtController;
        private Task<IHttpActionResult> _testResult;
        private BaseResult mockResponse;

        protected OrmtFixture()
        {
            _messageTypeService = new Mock<IWmsToEmsMessageProcessorService>(MockBehavior.Default);
            _ormtController = new OrderMaintenanceController(_messageTypeService.Object);
        }

        protected void ValidInput()
        {
            mockResponse = new BaseResult
            {
                ResultType = ResultTypes.Created
            };
        }

        protected void InvalidInput()
        {
            mockResponse = new BaseResult
            {
                ResultType = ResultTypes.NotFound
            };
        }

        private void MockGetOrmtMessageByCartonNumber()
        {
            _messageTypeService
                .Setup(el => el.GetOrmtMessageByCartonNumberAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(mockResponse));
        }

        private void MockGetOrmtMessageByWaveNumber()
        {
            _messageTypeService.Setup(el => el.GetOrmtMessageByWaveNumberAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(mockResponse));
        }

        protected void CreateOrmtMessagesByCartonNumber()
        {
            MockGetOrmtMessageByCartonNumber();
            _testResult = _ormtController.CreateOrmtMessageByCartonNumberAsync(It.IsAny<string>(), It.IsAny<string>());
        }

        protected void OrmtMessageShouldBeProcessed()
        {
            var result = _testResult.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.Created);
        }

        protected void OrmtMessageShouldNotBeProcessed()
        {
            var result = _testResult.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.NotFound);
        }
    }
}