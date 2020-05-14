using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Controllers;
using Sfc.Wms.Foundation.Receiving.Contracts.UoW.Dtos;
using Sfc.Wms.Foundation.Receiving.Contracts.UoW.Interfaces;

namespace Sfc.Wms.App.Api.Tests.Unit.Fixtures
{
    public class ReceivingControllerFixture
    {
        private readonly Mock<IQuestionAnswerService> _answerService;
        private readonly Mock<IAsnLotTrackingService> _lotService;
        private readonly ReceivingController _receivingController;
        private readonly Mock<IInboundReceivingService> _receivingService;
        private AnswerTextDto answerTextDto;
        private ReceiptInquiryDto receiptInquiryDto;
        private string shipmentNumber;
        private string skuId;
        private Task<IHttpActionResult> testResponse;
        private UpdateAsnDto updateAsnDto;

        protected ReceivingControllerFixture()
        {
            _receivingService = new Mock<IInboundReceivingService>(MockBehavior.Default);
            _answerService = new Mock<IQuestionAnswerService>(MockBehavior.Default);
            _lotService = new Mock<IAsnLotTrackingService>(MockBehavior.Default);
            _receivingController =
                new ReceivingController(_receivingService.Object, _lotService.Object, _answerService.Object);
        }

        private void MockReceiptSearch(ResultTypes resultType)
        {
            var result = new BaseResult<SearchResultDto>
            {
                ResultType = resultType,
                Payload = resultType == ResultTypes.Ok ? Generator.Default.Single<SearchResultDto>() : null
            };

            _receivingService.Setup(el => el.ReceivingSearchAsync(It.IsAny<ReceiptInquiryDto>()))
                .Returns(Task.FromResult(result));
        }

        private void VerifyReceiptSearch()
        {
            _receivingService.Verify(el => el.ReceivingSearchAsync(It.IsAny<ReceiptInquiryDto>()));
        }

        protected void InputForReceiptInquiry()
        {
            receiptInquiryDto = Generator.Default.Single<ReceiptInquiryDto>();
            MockReceiptSearch(ResultTypes.Ok);
        }

        protected void EmptyOrNullInputForReceiptInquiry()
        {
            receiptInquiryDto = null;
            MockReceiptSearch(ResultTypes.BadRequest);
        }

        protected void SearchOperationInvoked()
        {
            testResponse = _receivingController.SearchAsync(receiptInquiryDto);
        }

        protected void TheSearchOperationReturnedBadRequestResponse()
        {
            Assert.IsNotNull(testResponse);
            var result = testResponse.Result as NegotiatedContentResult<BaseResult<SearchResultDto>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.BadRequest, result.Content.ResultType);
        }

        protected void TheSearchOperationReturnedOkResponse()
        {
            VerifyReceiptSearch();
            Assert.IsNotNull(testResponse);
            var result = testResponse.Result as OkNegotiatedContentResult<BaseResult<SearchResultDto>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.Ok, result.Content.ResultType);
        }

        private void MockGetAsnDetails(ResultTypes resultType)
        {
            var result = new BaseResult<IEnumerable<AsnDrillDownDetailsDto>>
            {
                ResultType = resultType,
                Payload = resultType == ResultTypes.Ok ? Generator.Default.List<AsnDrillDownDetailsDto>(2) : null
            };

            _receivingService.Setup(el => el.GetAsnDrillDownDetailsAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(result));
        }

        private void VerifyGetAsnDetails()
        {
            _receivingService.Verify(el => el.GetAsnDrillDownDetailsAsync(It.IsAny<string>()));
        }

        protected void InputToFetchAsnDetails()
        {
            shipmentNumber = Generator.Default.Single<ReceiptInquiryDto>().ShipmentNumber;
            MockGetAsnDetails(ResultTypes.Ok);
        }

        protected void EmptyOrNullInputToFetchAsnDetails()
        {
            shipmentNumber = null;
            MockGetAsnDetails(ResultTypes.BadRequest);
        }

        protected void FetchAsnDetailsOperationInvoked()
        {
            testResponse = _receivingController.GetAsnDetailsAsync(shipmentNumber);
        }

        protected void TheFetchAsnDetailsOperationReturnedBadRequestResponse()
        {
            VerifyGetAsnDetails();
            Assert.IsNotNull(testResponse);
            var result =
                testResponse.Result as NegotiatedContentResult<BaseResult<IEnumerable<AsnDrillDownDetailsDto>>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.BadRequest, result.Content.ResultType);
        }

        protected void TheFetchAsnDetailsOperationReturnedOkResponse()
        {
            VerifyGetAsnDetails();
            Assert.IsNotNull(testResponse);
            var result =
                testResponse.Result as OkNegotiatedContentResult<BaseResult<IEnumerable<AsnDrillDownDetailsDto>>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.Ok, result.Content.ResultType);
        }

        private void MockGetQvDetails(ResultTypes resultType)
        {
            var result = new BaseResult<List<QvDetailsDto>>
            {
                ResultType = resultType,
                Payload = resultType == ResultTypes.Ok ? Generator.Default.List<QvDetailsDto>(2).ToList() : null
            };

            _receivingService.Setup(el => el.GetQvDetailsAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(result));
        }

        private void VerifyGetQvDetails()
        {
            _receivingService.Verify(el => el.GetQvDetailsAsync(It.IsAny<string>()));
        }

        protected void InputToGetQvDetails()
        {
            shipmentNumber = Generator.Default.Single<ReceiptInquiryDto>().ShipmentNumber;
            MockGetQvDetails(ResultTypes.Ok);
        }

        protected void EmptyOrNullInputToGetQvDetails()
        {
            shipmentNumber = null;
            MockGetQvDetails(ResultTypes.BadRequest);
        }

        protected void GetQvDetailsOperationInvoked()
        {
            testResponse = _receivingController.GetQualityVerificationsDetailsAsync(shipmentNumber);
        }

        protected void TheGetQvDetailsOperationReturnedBadRequestResponse()
        {
            VerifyGetQvDetails();
            Assert.IsNotNull(testResponse);
            var result = testResponse.Result as NegotiatedContentResult<BaseResult<List<QvDetailsDto>>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.BadRequest, result.Content.ResultType);
        }

        protected void TheGetQvDetailsOperationReturnedOkResponse()
        {
            VerifyGetQvDetails();
            Assert.IsNotNull(testResponse);
            var result = testResponse.Result as OkNegotiatedContentResult<BaseResult<List<QvDetailsDto>>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.Ok, result.Content.ResultType);
        }

        private void MockGetAsnLotTrackDetails(ResultTypes resultType)
        {
            var result = new BaseResult<IEnumerable<AsnLotTrackingDto>>
            {
                ResultType = resultType,
                Payload = resultType == ResultTypes.Ok ? Generator.Default.List<AsnLotTrackingDto>(2) : null
            };

            _lotService.Setup(el => el.GetByShipmentAndSkuIdAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(result));
        }

        private void VerifyGetAsnLotTrackDetails()
        {
            _lotService.Verify(el => el.GetByShipmentAndSkuIdAsync(It.IsAny<string>(), It.IsAny<string>()));
        }

        protected void InputToGetAsnLotTrackDetails()
        {
            shipmentNumber = Generator.Default.Single<ReceiptInquiryDto>().ShipmentNumber;
            skuId = "123980";
            MockGetAsnLotTrackDetails(ResultTypes.Ok);
        }

        protected void InputToGetAsnLotTrackDetailsForWhichNoRecordExists()
        {
            shipmentNumber = Generator.Default.Single<ReceiptInquiryDto>().ShipmentNumber;
            skuId = "123980";
            MockGetAsnLotTrackDetails(ResultTypes.NotFound);
        }

        protected void EmptyOrNullInputToGetAsnLotTrackDetails()
        {
            skuId = shipmentNumber = null;
            MockGetAsnLotTrackDetails(ResultTypes.BadRequest);
        }

        protected void GetAsnLotTrackDetailsOperationInvoked()
        {
            testResponse = _receivingController.GetAsnLotTrackByShipmentAndSkuIdAsync(shipmentNumber, skuId);
        }

        protected void TheGetAsnLotTrackDetailsOperationReturnedBadRequestResponse()
        {
            VerifyGetAsnLotTrackDetails();
            Assert.IsNotNull(testResponse);
            var result = testResponse.Result as NegotiatedContentResult<BaseResult<IEnumerable<AsnLotTrackingDto>>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.BadRequest, result.Content.ResultType);
        }

        protected void TheGetAsnLotTrackDetailsOperationReturnedNotFoundAsResponse()
        {
            VerifyGetAsnLotTrackDetails();
            Assert.IsNotNull(testResponse);
            var result = testResponse.Result as NegotiatedContentResult<BaseResult<IEnumerable<AsnLotTrackingDto>>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.NotFound, result.Content.ResultType);
        }

        protected void TheGetAsnLotTrackDetailsOperationReturnedOkResponse()
        {
            VerifyGetAsnLotTrackDetails();
            Assert.IsNotNull(testResponse);
            var result = testResponse.Result as OkNegotiatedContentResult<BaseResult<IEnumerable<AsnLotTrackingDto>>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.Ok, result.Content.ResultType);
        }

        private void MockUpdateAnswerText(ResultTypes resultType)
        {
            var result = new BaseResult
            {
                ResultType = resultType
            };

            _answerService.Setup(el => el.UpdateAnswerTextAsync(It.IsAny<AnswerTextDto>()))
                .Returns(Task.FromResult(result));
        }

        private void VerifyUpdateAnswerText()
        {
            _answerService.Verify(el => el.UpdateAnswerTextAsync(It.IsAny<AnswerTextDto>()));
        }

        protected void InputToUpdateAnswerText()
        {
            answerTextDto = Generator.Default.Single<AnswerTextDto>();
            MockUpdateAnswerText(ResultTypes.Ok);
        }

        protected void InputToUpdateAnswerTextForWhichNoRecordExists()
        {
            answerTextDto = Generator.Default.Single<AnswerTextDto>();
            MockUpdateAnswerText(ResultTypes.NotFound);
        }

        protected void EmptyOrNullInputToUpdateAnswerText()
        {
            answerTextDto = null;
            MockUpdateAnswerText(ResultTypes.BadRequest);
        }

        protected void UpdateAnswerTextOperationInvoked()
        {
            testResponse = _receivingController.UpdateQualityVerificationsAsync(answerTextDto);
        }

        protected void TheUpdateAnswerTextOperationReturnedBadRequestResponse()
        {
            VerifyUpdateAnswerText();
            Assert.IsNotNull(testResponse);
            var result = testResponse.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.BadRequest, result.Content.ResultType);
        }

        protected void TheUpdateAnswerTextOperationReturnedNotFoundAsResponse()
        {
            VerifyUpdateAnswerText();
            Assert.IsNotNull(testResponse);
            var result = testResponse.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.NotFound, result.Content.ResultType);
        }

        protected void TheUpdateAnswerTextOperationReturnedOkResponse()
        {
            VerifyUpdateAnswerText();
            Assert.IsNotNull(testResponse);
            var result = testResponse.Result as OkNegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.Ok, result.Content.ResultType);
        }

        private void MockUpdateAsn(ResultTypes resultType)
        {
            var result = new BaseResult {ResultType = resultType};

            _receivingService.Setup(el =>
                    el.UpdateAdvanceShipmentNoticesDetailsAsync(It.IsAny<UpdateAsnDto>(), It.IsAny<string>()))
                .Returns(Task.FromResult(result));
        }

        private void VerifyUpdateAsn()
        {
            _receivingService.Verify(el =>
                el.UpdateAdvanceShipmentNoticesDetailsAsync(It.IsAny<UpdateAsnDto>(), It.IsAny<string>()));
        }

        protected void InputToUpdateAsn()
        {
            updateAsnDto = Generator.Default.Single<UpdateAsnDto>();
            MockUpdateAsn(ResultTypes.Ok);
        }

        protected void InputToUpdateAsnForWhichNoRecordExists()
        {
            updateAsnDto = Generator.Default.Single<UpdateAsnDto>();
            MockUpdateAsn(ResultTypes.NotFound);
        }

        protected void InvalidInputToUpdateAsn()
        {
            updateAsnDto = Generator.Default.Single<UpdateAsnDto>();
            MockUpdateAsn(ResultTypes.ExpectationFailed);
        }

        protected void EmptyOrNullInputToUpdateAsn()
        {
            answerTextDto = null;
            MockUpdateAsn(ResultTypes.BadRequest);
        }

        protected void UpdateAsnOperationInvoked()
        {
            testResponse = _receivingController.UpdateAdvanceShipmentNoticesDetailsAsync(shipmentNumber, updateAsnDto);
        }

        protected void TheUpdateAsnOperationReturnedBadRequestResponse()
        {
            VerifyUpdateAsn();
            Assert.IsNotNull(testResponse);
            var result = testResponse.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.BadRequest, result.Content.ResultType);
        }

        protected void TheUpdateAsnOperationReturnedNotFoundAsResponse()
        {
            VerifyUpdateAsn();
            Assert.IsNotNull(testResponse);
            var result = testResponse.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.NotFound, result.Content.ResultType);
        }

        protected void TheUpdateAsnOperationReturnedExpectationFailedAsResponse()
        {
            VerifyUpdateAsn();
            Assert.IsNotNull(testResponse);
            var result = testResponse.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.ExpectationFailed, result.Content.ResultType);
        }

        protected void TheUpdateAsnOperationReturnedOkResponse()
        {
            VerifyUpdateAsn();
            Assert.IsNotNull(testResponse);
            var result = testResponse.Result as OkNegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.Ok, result.Content.ResultType);
        }
    }
}