using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Nuget.Gateways;
using Sfc.Wms.App.Api.Nuget.Interfaces;
using Sfc.Wms.Foundation.Receiving.Contracts.UoW.Dtos;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.Tests.Unit.Fixtures
{
    public class ReceivingGatewayFixture
    {
        private readonly Mock<IRestCsharpClient> _restClient;
        private ReceiptInquiryDto receiptInquiryDto;
        private AnswerTextDto answerTextDto;
        private BaseResult<SearchResultDto> searchResult;
        private BaseResult<ShipmentDetailsDto> shipmentDetails;
        private BaseResult<IEnumerable<AsnLotTrackingDto>> lotTrackingDetails;
        private BaseResult updateResult;
        private BaseResult<IEnumerable<QvDetailsDto>> qvDetails;
        private BaseResult<IEnumerable<AsnDrillDownDetailsDto>> asnDetails;
        private readonly IReceivingGateway _receivingGateway;

        protected ReceivingGatewayFixture()
        {
            _restClient = new Mock<IRestCsharpClient>();
            answerTextDto = Generator.Default.Single<AnswerTextDto>();
            receiptInquiryDto = Generator.Default.Single<ReceiptInquiryDto>();
            _receivingGateway = new ReceivingGateway(new ResponseBuilder(), _restClient.Object);
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

        protected void InputForReceivingSearch()
        {
            var result = new BaseResult<SearchResultDto>
            { ResultType = ResultTypes.Ok, Payload = Generator.Default.Single<SearchResultDto>() };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }
        protected void EmptyOrNullSearchParameters()
        {
            var result = new BaseResult<SearchResultDto> { ResultType = ResultTypes.BadRequest };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void ReceivingSearchOperationInvoked()
        {
            searchResult = _receivingGateway.SearchAsync(receiptInquiryDto, It.IsAny<string>()).Result;
        }

        protected void TheLpnSearchOperationReturnedOkAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<SearchResultDto>>();
            Assert.IsNotNull(searchResult);
            Assert.AreEqual(ResultTypes.Ok, searchResult.ResultType);
        }

        protected void TheReceivingSearchOperationReturnedBadRequestAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<SearchResultDto>>();
            Assert.IsNotNull(searchResult);
            Assert.AreEqual(ResultTypes.BadRequest, searchResult.ResultType);
        }

        #region Shipment Details

        protected void InputToFetchShipmentDetails()
        {
            var result = new BaseResult<ShipmentDetailsDto>
            { ResultType = ResultTypes.Ok, Payload = Generator.Default.Single<ShipmentDetailsDto>() };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InputToFetchShipmentDetailsForWhichNoDetailsExists()
        {
            var result = new BaseResult<ShipmentDetailsDto> { ResultType = ResultTypes.NotFound };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void EmptyOrNullToFetchShipmentDetails()
        {
            var result = new BaseResult<ShipmentDetailsDto> { ResultType = ResultTypes.BadRequest };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void FetchShipmentDetailsOperationInvoked()
        {
            shipmentDetails = _receivingGateway.GetShipmentDetailsAsync(receiptInquiryDto.ShipmentNumber, It.IsAny<string>()).Result;
        }

        protected void FetchShipmentDetailsReturnedOkAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<ShipmentDetailsDto>>();
            Assert.IsNotNull(shipmentDetails);
            Assert.AreEqual(ResultTypes.Ok, shipmentDetails.ResultType);
        }

        protected void FetchShipmentDetailsReturnedBadRequestAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<ShipmentDetailsDto>>();
            Assert.IsNotNull(shipmentDetails);
            Assert.AreEqual(ResultTypes.BadRequest, shipmentDetails.ResultType);
        }

        protected void FetchShipmentDetailsReturnedNotFoundAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<ShipmentDetailsDto>>();
            Assert.IsNotNull(shipmentDetails);
            Assert.AreEqual(ResultTypes.NotFound, shipmentDetails.ResultType);
        }

        #endregion

        #region Update Answer text

        protected void InputToUpdateAnswerText()
        {
            var result = new BaseResult { ResultType = ResultTypes.Ok };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InputToUpdateAnswerTextForWhichNoDetailsExists()
        {
            var result = new BaseResult { ResultType = ResultTypes.NotFound };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void EmptyOrNullToUpdateAnswerText()
        {
            var result = new BaseResult { ResultType = ResultTypes.BadRequest };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void UpdateAnswerTextOperationInvoked()
        {
            updateResult = _receivingGateway.UpdateAnswerTextAsync(answerTextDto, It.IsAny<string>()).Result;
        }

        protected void UpdateAnswerTextReturnedOkAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult>();
            Assert.IsNotNull(updateResult);
            Assert.AreEqual(ResultTypes.Ok, updateResult.ResultType);
        }

        protected void UpdateAnswerTextReturnedBadRequestAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult>();
            Assert.IsNotNull(updateResult);
            Assert.AreEqual(ResultTypes.BadRequest, updateResult.ResultType);
        }

        protected void UpdateAnswerTextReturnedNotFoundAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult>();
            Assert.IsNotNull(updateResult);
            Assert.AreEqual(ResultTypes.NotFound, updateResult.ResultType);
        }

        #endregion

        #region Get Asn Lot Trcking Details

        protected void InputToFetchAsnLotTrackingDetails()
        {
            var result = new BaseResult<IEnumerable<AsnLotTrackingDto>>
            {
                ResultType = ResultTypes.Ok,
                Payload = Generator.Default.List<AsnLotTrackingDto>(3)
            };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InputToFetchAsnLotTrackingDetailsForWhichNoDetailsExists()
        {
            var result = new BaseResult<IEnumerable<AsnLotTrackingDto>> { ResultType = ResultTypes.NotFound };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void EmptyOrNullToFetchAsnLotTrackingDetails()
        {
            var result = new BaseResult<IEnumerable<AsnLotTrackingDto>> { ResultType = ResultTypes.BadRequest };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void FetchAsnLotTrackingDetailsOperationInvoked()
        {
            lotTrackingDetails = _receivingGateway.GetAsnLotTrackingDetailsAsync(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<string>()).Result;
        }

        protected void FetchAsnLotTrackingDetailsReturnedOkAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<IEnumerable<AsnLotTrackingDto>>>();
            Assert.IsNotNull(lotTrackingDetails);
            Assert.AreEqual(ResultTypes.Ok, lotTrackingDetails.ResultType);
        }

        protected void FetchAsnLotTrackingDetailsReturnedBadRequestAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<IEnumerable<AsnLotTrackingDto>>>();
            Assert.IsNotNull(lotTrackingDetails);
            Assert.AreEqual(ResultTypes.BadRequest, lotTrackingDetails.ResultType);
        }

        protected void FetchAsnLotTrackingDetailsReturnedNotFoundAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<IEnumerable<AsnLotTrackingDto>>>();
            Assert.IsNotNull(lotTrackingDetails);
            Assert.AreEqual(ResultTypes.NotFound, lotTrackingDetails.ResultType);
        }

        #endregion


        #region Get Asn Details

        protected void InputToFetchAsnDetails()
        {
            var result = new BaseResult<IEnumerable<AsnDrillDownDetailsDto>>
            {
                ResultType = ResultTypes.Ok,
                Payload = Generator.Default.List<AsnDrillDownDetailsDto>(3)
            };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InputToFetchAsnDetailsForWhichNoDetailsExists()
        {
            var result = new BaseResult<IEnumerable<AsnDrillDownDetailsDto>> { ResultType = ResultTypes.NotFound };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void EmptyOrNullToFetchAsnDetails()
        {
            var result = new BaseResult<IEnumerable<AsnDrillDownDetailsDto>> { ResultType = ResultTypes.BadRequest };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void FetchAsnDetailsOperationInvoked()
        {
            asnDetails = _receivingGateway.GetAsnDetailsAsync(It.IsAny<string>(), It.IsAny<string>()).Result;
        }

        protected void FetchAsnDetailsReturnedOkAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<IEnumerable<AsnDrillDownDetailsDto>>>();
            Assert.IsNotNull(asnDetails);
            Assert.AreEqual(ResultTypes.Ok, asnDetails.ResultType);
        }

        protected void FetchAsnDetailsReturnedBadRequestAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<IEnumerable<AsnDrillDownDetailsDto>>>();
            Assert.IsNotNull(asnDetails);
            Assert.AreEqual(ResultTypes.BadRequest, asnDetails.ResultType);
        }

        protected void FetchAsnDetailsReturnedNotFoundAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<IEnumerable<AsnDrillDownDetailsDto>>>();
            Assert.IsNotNull(asnDetails);
            Assert.AreEqual(ResultTypes.NotFound, asnDetails.ResultType);
        }

        #endregion


        #region Get Qv Details

        protected void InputToFetchQvDetails()
        {
            var result = new BaseResult<IEnumerable<QvDetailsDto>>
            {
                ResultType = ResultTypes.Ok,
                Payload = Generator.Default.List<QvDetailsDto>(3)
            };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InputToFetchQvDetailsForWhichNoDetailsExists()
        {
            var result = new BaseResult<IEnumerable<QvDetailsDto>> { ResultType = ResultTypes.NotFound };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void EmptyOrNullToFetchQvDetails()
        {
            var result = new BaseResult<IEnumerable<QvDetailsDto>> { ResultType = ResultTypes.BadRequest };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void FetchQvDetailsOperationInvoked()
        {
            qvDetails = _receivingGateway.GetQvDetailsAsync(It.IsAny<string>(), It.IsAny<string>()).Result;
        }

        protected void FetchQvDetailsReturnedOkAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<IEnumerable<QvDetailsDto>>>();
            Assert.IsNotNull(qvDetails);
            Assert.AreEqual(ResultTypes.Ok, qvDetails.ResultType);
        }

        protected void FetchQvDetailsReturnedBadRequestAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<IEnumerable<QvDetailsDto>>>();
            Assert.IsNotNull(qvDetails);
            Assert.AreEqual(ResultTypes.BadRequest, qvDetails.ResultType);
        }

        protected void FetchQvDetailsReturnedNotFoundAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<IEnumerable<QvDetailsDto>>>();
            Assert.IsNotNull(qvDetails);
            Assert.AreEqual(ResultTypes.NotFound, qvDetails.ResultType);
        }

        #endregion
    }
}