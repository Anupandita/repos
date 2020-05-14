using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Controllers;
using Sfc.Wms.Configuration.ItemMasters.Contracts.Dtos;
using Sfc.Wms.Configuration.ItemMasters.Contracts.Interface;

namespace Sfc.Wms.App.Api.Tests.Unit.Fixtures
{
    public class ItemAttributeControllerFixture
    {
        private readonly ItemAttributeController _attributeController;
        private readonly Mock<IItemAttributeService> _mockedAttributeService;
        private string itemId;
        private ItemAttributeSearchInputDto searchInputDto;
        private IHttpActionResult testResult;

        protected ItemAttributeControllerFixture()
        {
            _mockedAttributeService = new Mock<IItemAttributeService>(MockBehavior.Default);
            _attributeController = new ItemAttributeController(_mockedAttributeService.Object);
            searchInputDto = Generator.Default.Single<ItemAttributeSearchInputDto>();
            itemId = searchInputDto.ItemId;
        }

        protected void EmptyOrInvalidInputForAttributeSearch()
        {
            searchInputDto = null;
            MockAttributeSearch(ResultTypes.BadRequest);
        }

        protected void ValidInputDataForAttributeSearch()
        {
            MockAttributeSearch(ResultTypes.Ok);
        }

        protected void AttributeSearchOperationInvoked()
        {
            testResult = _attributeController.SearchAsync(searchInputDto).Result;
        }

        protected void AttributeSearchReturnedOkAsResponse()
        {
            VerifyAttributeSearch();
            Assert.IsNotNull(testResult);
            var result = testResult as OkNegotiatedContentResult<BaseResult<ItemAttributeSearchResultDto>>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(ResultTypes.Ok, result.Content.ResultType);
        }

        protected void AttributeSearchReturnedBadRequestAsResponse()
        {
            VerifyAttributeSearch();
            Assert.IsNotNull(testResult);
            var result = testResult as NegotiatedContentResult<BaseResult<ItemAttributeSearchResultDto>>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(ResultTypes.BadRequest, result.Content.ResultType);
        }


        protected void EmptyOrInvalidInputForAttributeDrillDown()
        {
            itemId = null;
            MockAttributeDrillDown(ResultTypes.BadRequest);
        }

        protected void ValidInputDataForAttributeDrillDown()
        {
            MockAttributeDrillDown(ResultTypes.Ok);
        }

        protected void ValidInputDataForAttributeDrillDownForWhichNoRecordExists()
        {
            MockAttributeDrillDown(ResultTypes.NotFound);
        }

        protected void AttributeDrillDownOperationInvoked()
        {
            testResult = _attributeController.AttributeDrillDownAsync(itemId).Result;
        }

        protected void AttributeDrillDownReturnedOkAsResponse()
        {
            VerifyAttributeDrillDown();
            Assert.IsNotNull(testResult);
            var result = testResult as OkNegotiatedContentResult<BaseResult<ItemAttributeDetailsDto>>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(ResultTypes.Ok, result.Content.ResultType);
        }

        protected void AttributeDrillDownReturnedBadRequestAsResponse()
        {
            VerifyAttributeDrillDown();
            Assert.IsNotNull(testResult);
            var result = testResult as NegotiatedContentResult<BaseResult<ItemAttributeDetailsDto>>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(ResultTypes.BadRequest, result.Content.ResultType);
        }

        protected void AttributeDrillDownReturnedNotFoundAsResponse()
        {
            VerifyAttributeDrillDown();
            Assert.IsNotNull(testResult);
            var result = testResult as NegotiatedContentResult<BaseResult<ItemAttributeDetailsDto>>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(ResultTypes.NotFound, result.Content.ResultType);
        }

        #region Mock

        private void MockAttributeSearch(ResultTypes resultType)
        {
            var response = new BaseResult<ItemAttributeSearchResultDto>
            {
                ResultType = resultType,
                Payload = resultType == ResultTypes.Ok ? Generator.Default.Single<ItemAttributeSearchResultDto>() : null
            };
            _mockedAttributeService.Setup(el =>
                    el.AttributeSearchAsync(It.IsAny<ItemAttributeSearchInputDto>()))
                .Returns(Task.FromResult(response));
        }

        private void MockAttributeDrillDown(ResultTypes resultType)
        {
            var response = new BaseResult<ItemAttributeDetailsDto>
            {
                ResultType = resultType,
                Payload = resultType == ResultTypes.Ok ? Generator.Default.Single<ItemAttributeDetailsDto>() : null
            };
            _mockedAttributeService.Setup(el =>
                    el.AttributeDrillDownAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(response));
        }

        #endregion

        #region Verify

        private void VerifyAttributeSearch()
        {
            _mockedAttributeService.Verify(el =>
                el.AttributeSearchAsync(It.IsAny<ItemAttributeSearchInputDto>()));
        }


        private void VerifyAttributeDrillDown()
        {
            _mockedAttributeService.Verify(el =>
                el.AttributeDrillDownAsync(It.IsAny<string>()));
        }

        #endregion
    }
}