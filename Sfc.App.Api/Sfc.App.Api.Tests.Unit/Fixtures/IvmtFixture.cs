using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.Wms.Asrs.Api.Controllers.Dematic;
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
        private readonly IvmtController _ivmtController;
        private readonly Mock<IWmsToEmsMessageProcessorService> _messageTypeService;
        private Task<IHttpActionResult> _testResult;

        protected IvmtFixture()
        {
            _messageTypeService = new Mock<IWmsToEmsMessageProcessorService>(MockBehavior.Default);
            _ivmtController = new IvmtController(_messageTypeService.Object);
        }

        protected void ValidIvmtMessage()
        {
            var response = new BaseResult()
            {
                ResultType = ResultTypes.Ok
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
            var result = _testResult.Result as OkNegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.Ok);
        }

        protected void IvmtMessageShouldNotBeProcessed()
        {
            var result = _testResult.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.BadRequest);
        }
    }
}