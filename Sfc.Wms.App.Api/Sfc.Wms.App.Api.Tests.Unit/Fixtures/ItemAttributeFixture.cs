using System.Net;
using System.Threading.Tasks;
using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Nuget.Gateways;
using Sfc.Wms.Configuration.ItemMasters.Contracts.Dtos;

namespace Sfc.Wms.App.Api.Tests.Unit.Fixtures
{
    public class ItemAttributeFixture
    {
        private readonly ItemAttributeGateway _attributeGateway;
        private readonly Mock<IRestClient> _restClient;
        private BaseResult<ItemAttributeDetailsDto> attributeDetailsResult;
        private readonly string itemId;
        private readonly ItemAttributeSearchInputDto searchInputDto;
        private BaseResult<ItemAttributeSearchResultDto> searchResult;

        protected ItemAttributeFixture()
        {
            _restClient = new Mock<IRestClient>();
            _attributeGateway = new ItemAttributeGateway(new ResponseBuilder(), _restClient.Object);
            searchInputDto = Generator.Default.Single<ItemAttributeSearchInputDto>();
            itemId = searchInputDto.ItemId;
        }

        private void GetRestResponse<T>(T entity, HttpStatusCode statusCode, ResponseStatus responseStatus)
            where T : new()
        {
            var response = new Mock<IRestResponse<T>>();
            response.Setup(_ => _.StatusCode).Returns(statusCode);
            response.Setup(_ => _.ResponseStatus).Returns(responseStatus);
            response.Setup(_ => _.Content).Returns(JsonConvert.SerializeObject(entity));
            _restClient.Setup(x => x.ExecuteTaskAsync<T>(It.IsAny<IRestRequest>()))
                .Returns(Task.FromResult(response.Object));
        }

        private void VerifyRestClientInvocation<T>() where T : new()
        {
            _restClient.Verify(x => x.ExecuteTaskAsync<T>(It.IsAny<IRestRequest>()));
        }

        protected void EmptyOrInvalidInputForSearch()
        {
            var response = new BaseResult<ItemAttributeSearchResultDto> {ResultType = ResultTypes.BadRequest};
            GetRestResponse(response, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void ValidInputDataForSearch()
        {
            var response = new BaseResult<ItemAttributeSearchResultDto>
            {
                ResultType = ResultTypes.Ok,
                Payload = Generator.Default.Single<ItemAttributeSearchResultDto>()
            };
            GetRestResponse(response, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void AttributeSearchOperationInvoked()
        {
            searchResult = _attributeGateway.SearchAsync(searchInputDto, It.IsAny<string>()).Result;
        }

        protected void SearchReturnedOkAsResponse()
        {
            VerifyRestClientInvocation<BaseResult<ItemAttributeSearchResultDto>>();
            Assert.IsNotNull(searchResult);
            Assert.AreEqual(ResultTypes.Ok, searchResult.ResultType);
        }

        protected void SearchReturnedBadRequestAsResponse()
        {
            VerifyRestClientInvocation<BaseResult<ItemAttributeSearchResultDto>>();
            Assert.IsNotNull(searchResult);
            Assert.AreEqual(ResultTypes.BadRequest, searchResult.ResultType);
        }


        protected void EmptyOrInvalidInputForAttributeDrillDown()
        {
            var response = new BaseResult<ItemAttributeDetailsDto> {ResultType = ResultTypes.BadRequest};
            GetRestResponse(response, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void ValidInputDataForAttributeDrillDown()
        {
            var response = new BaseResult<ItemAttributeDetailsDto>
            {
                ResultType = ResultTypes.Ok,
                Payload = Generator.Default.Single<ItemAttributeDetailsDto>()
            };
            GetRestResponse(response, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void ValidInputDataForAttributeDrillDownForWhichNoRecordExists()
        {
            var response = new BaseResult<ItemAttributeDetailsDto> {ResultType = ResultTypes.NotFound};
            GetRestResponse(response, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void AttributeDrillDownOperationInvoked()
        {
            attributeDetailsResult = _attributeGateway.AttributeDrillDownAsync(itemId, It.IsAny<string>()).Result;
        }

        protected void AttributeDrillDownReturnedOkAsResponse()
        {
            VerifyRestClientInvocation<BaseResult<ItemAttributeDetailsDto>>();
            Assert.IsNotNull(attributeDetailsResult);
            Assert.AreEqual(ResultTypes.Ok, attributeDetailsResult.ResultType);
            Assert.IsNotNull(attributeDetailsResult.Payload);
        }

        protected void AttributeDrillDownReturnedBadRequestAsResponse()
        {
            VerifyRestClientInvocation<BaseResult<ItemAttributeDetailsDto>>();
            Assert.IsNotNull(attributeDetailsResult);
            Assert.AreEqual(ResultTypes.BadRequest, attributeDetailsResult.ResultType);
        }

        protected void AttributeDrillDownReturnedNotFoundAsResponse()
        {
            VerifyRestClientInvocation<BaseResult<ItemAttributeDetailsDto>>();
            Assert.IsNotNull(attributeDetailsResult);
            Assert.AreEqual(ResultTypes.NotFound, attributeDetailsResult.ResultType);
        }
    }
}