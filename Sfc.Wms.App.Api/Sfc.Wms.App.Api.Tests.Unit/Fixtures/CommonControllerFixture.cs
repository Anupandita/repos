using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.Core.OnPrem.Pagination;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Contracts.Dto;
using Sfc.Wms.App.Api.Controllers;
using Sfc.Wms.Configuration.SystemCode.Contracts.Dtos;
using Sfc.Wms.Configuration.SystemCode.Contracts.Interfaces;

namespace Sfc.Wms.App.Api.Tests.Unit.Fixtures
{
    public class CommonControllerFixture
    {
        private readonly CommonController _commonController;
        private readonly Mock<ISystemCodeService> _systemCodeService;
        private SystemCodeInputDto systemCodeInputDto;
        private Task<IHttpActionResult> testResponse;

        protected CommonControllerFixture()
        {
            _systemCodeService = new Mock<ISystemCodeService>(MockBehavior.Default);
            _commonController = new CommonController(_systemCodeService.Object);
        }

        protected void InputParametersToGetSystemCodes()
        {
            systemCodeInputDto = Generator.Default.Single<SystemCodeInputDto>();
            var result = new BaseResult<IEnumerable<SysCodeDto>>
                {ResultType = ResultTypes.Ok, Payload = Generator.Default.List<SysCodeDto>(10)};

            _systemCodeService.Setup(el =>
                    el.GetSystemCodeAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<SortOption>()))
                .Returns(Task.FromResult(result));
        }

        protected void InValidInputParametersToGetSystemCodes()
        {
            systemCodeInputDto = Generator.Default.Single<SystemCodeInputDto>();
            var result = new BaseResult<IEnumerable<SysCodeDto>> {ResultType = ResultTypes.BadRequest};

            _systemCodeService.Setup(el =>
                    el.GetSystemCodeAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                        It.IsAny<SortOption>()))
                .Returns(Task.FromResult(result));
        }

        protected void GetSystemCodesOperationInvoked()
        {
            testResponse = _commonController.GetSystemCodesAsync(systemCodeInputDto);
        }

        protected void TheGetSystemCodesOperationReturnedBadRequestResponse()
        {
            Assert.IsNotNull(testResponse);
            var result = testResponse.Result as NegotiatedContentResult<BaseResult<IEnumerable<SysCodeDto>>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.BadRequest, result.Content.ResultType);
        }

        protected void TheGetSystemCodesOperationReturnedOkResponse()
        {
            _systemCodeService.Verify(el => el.GetSystemCodeAsync(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<SortOption>()));
            Assert.IsNotNull(testResponse);
            var result = testResponse.Result as OkNegotiatedContentResult<BaseResult<IEnumerable<SysCodeDto>>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.Ok, result.Content.ResultType);
        }
    }
}