using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.Wms.Asrs.Api.Controllers.Shamrock;
using Sfc.Wms.Asrs.App.Interfaces;
using Sfc.Wms.DematicMessage.Contracts.Dto;
using Sfc.Wms.Result;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using DataGenerator;

namespace Sfc.Wms.Asrs.Test.Unit.Fixtures
{
    public abstract class PickLocationDetailFixture
    {
        private readonly PickLocationDetailController _pickLocationDtlController;
        private readonly Mock<IPickLocationService> _pickLocationDtlService;
        private Task<IHttpActionResult> _testResult;

        protected PickLocationDetailFixture()
        {
            _pickLocationDtlService = new Mock<IPickLocationService>(MockBehavior.Default);
            _pickLocationDtlController = new PickLocationDetailController(_pickLocationDtlService.Object);
        }

        protected void ValidData()
        {
            var response = new BaseResult()
            {
                ResultType = ResultTypes.Ok
            };

            _pickLocationDtlService.Setup(el => el.UpdateQuantityAsync(It.IsAny<CostDto>(), It.IsAny<string>()))
                .Returns(Task.FromResult(response));
        }

        protected void InvalidData()
        {
            var response = new BaseResult()
            {
                ResultType = ResultTypes.BadRequest
            };

            _pickLocationDtlService.Setup(el => el.UpdateQuantityAsync(It.IsAny<CostDto>(), It.IsAny<string>()))
                .Returns(Task.FromResult(response));
        }

        protected void UpdatePickLocationDetailInvoked()
        {
            _testResult = _pickLocationDtlController.UpdateAsync(Generator.Default.Single<CostDto>());
        }

        protected void PickLocationDetailShouldBeUpdated()
        {
            var result = _testResult.Result as OkNegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.Ok);
        }

        protected void PickLocationDetailShouldNotBeUpdated()
        {
            var result = _testResult.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.BadRequest);
        }
    }
}