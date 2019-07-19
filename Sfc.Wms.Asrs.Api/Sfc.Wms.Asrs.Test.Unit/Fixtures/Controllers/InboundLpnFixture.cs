using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.Wms.Asrs.Api.Controllers.Dematic;
using Sfc.Wms.Asrs.App.Interfaces;
using Sfc.Wms.Asrs.Dematic.Repository.Dtos;
using Sfc.Wms.DematicMessage.Contracts.Dto;
using Sfc.Wms.Result;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;

namespace Sfc.Wms.Asrs.Test.Unit.Fixtures
{
    public abstract  class InboundLpnFixture
    {
        private readonly InboundLpnController _inboundLpnController;
        private readonly Mock<IInboundLpnService> _dematicService;
        private Task<IHttpActionResult> _testResult;

        protected InboundLpnFixture()
        {
            _dematicService = new Mock<IInboundLpnService>(MockBehavior.Default);
            _inboundLpnController = new InboundLpnController(_dematicService.Object);
        }

        protected void ValidData()
        {
            var response = new BaseResult()
            {
                ResultType = ResultTypes.Ok
            };

            _dematicService.Setup(el => el.UpdateCaseDtlQuantityAsync(It.IsAny<IvmtDto>(), It.IsAny<string>()))
                .Returns(Task.FromResult(response));
        }

        protected void InvalidData()
        {
            var response = new BaseResult()
            {
                ResultType = ResultTypes.BadRequest
            };

            _dematicService.Setup(el => el.UpdateCaseDtlQuantityAsync(It.IsAny<IvmtDto>(), It.IsAny<string>()))
                .Returns(Task.FromResult(response));
        }

        protected void UpdateQuantityInvoked()
        {
            _testResult = _inboundLpnController.UpdateCaseDtlQuantityAsync(Generator.Default.Single<IvmtDto>());
        }

        protected void QuantityShouldBeUpdated()
        {
            var result = _testResult.Result as OkNegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.Ok);
        }

        protected void QuantityShouldNotBeUpdated()
        {
            var result = _testResult.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.BadRequest);
        }
    }
}