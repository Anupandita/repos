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
    public abstract class ComtFixture
    {
        private readonly ComtController _comtController;
        private readonly Mock<IWmsToEmsMessageProcessorService> _messageTypeService;
        private Task<IHttpActionResult> _testResult;

        protected ComtFixture()
        {
            _messageTypeService = new Mock<IWmsToEmsMessageProcessorService>(MockBehavior.Default);
            _comtController = new ComtController(_messageTypeService.Object);
        }

        protected void ValidComtMessage()
        {
            var response = new BaseResult()
            {
                ResultType = ResultTypes.Ok
            };

            _messageTypeService.Setup(el => el.GetComtMessageAsync(It.IsAny<ComtTriggerInputDto>()))
                .Returns(Task.FromResult(response));
        }

        protected void InvalidComtMessage()
        {
            var response = new BaseResult()
            {
                ResultType = ResultTypes.BadRequest
            };

            _messageTypeService.Setup(el => el.GetComtMessageAsync(It.IsAny<ComtTriggerInputDto>()))
                .Returns(Task.FromResult(response));
        }

        protected void InsertMessageInvoked()
        {
            _testResult = _comtController.CreateAsync(Generator.Default.Single<ComtTriggerInputDto>());
        }

        protected void ComtMessageShouldBeInserted()
        {
            var result = _testResult.Result as OkNegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.Ok);
        }

        protected void ComtMessageShouldNotBeInserted()
        {
            var result = _testResult.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.BadRequest);
        }
    }
}