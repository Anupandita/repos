using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Controllers;
using Sfc.Wms.Foundation.InboundLpn.Contracts.Dtos;
using Sfc.Wms.Foundation.InboundLpn.Contracts.Interfaces;

namespace Sfc.Wms.App.Api.Tests.Unit.Fixtures
{
    public abstract class LpnControllerFixture
    {
        private readonly CaseCommentDto _addLpnCommentRequest;
        private readonly CaseLockCommentDto _caseLockCommentDto;
        private readonly LpnController _lpnController;
        private readonly List<LpnMultipleUnlockDto> _lpnMultipleUnlockDtos;
        private readonly LpnParameterDto _lpnSearchRequest;
        private readonly Mock<ICaseCommentService> _mockCaseCommentService;
        private readonly Mock<ICaseDetailService> _mockCaseDetailService;
        private readonly Mock<ICaseHeaderService> _mockCaseHeaderService;
        private readonly Mock<ICaseLockService> _mockCaseLockService;
        private readonly Mock<ILpnService> _mockLpnService;
        private readonly LpnDetailsUpdateDto _updateLpnCaseDetailsRequest;
        private readonly CaseCommentDto _updateLpnCommentRequest;
        private readonly LpnHeaderUpdateDto _updateLpnHeaderUpdateDto;
        private Task<IHttpActionResult> _testResponse;
        private IEnumerable<CaseCommentDto> caseCommentDtos;
        private int commentSequenceNumber;
        private LpnBatchUpdateDto lpnBatchUpdateDto;
        private readonly IEnumerable<string> lpnIds;
        private string warehouse, lpnNumber, faceLocationId;

        protected LpnControllerFixture()
        {
            _mockLpnService = new Mock<ILpnService>(MockBehavior.Default);
            _mockCaseCommentService = new Mock<ICaseCommentService>(MockBehavior.Default);
            _mockCaseDetailService = new Mock<ICaseDetailService>(MockBehavior.Default);
            _mockCaseLockService = new Mock<ICaseLockService>(MockBehavior.Default);

            _lpnSearchRequest = Generator.Default.Single<LpnParameterDto>();
            _updateLpnHeaderUpdateDto = Generator.Default.Single<LpnHeaderUpdateDto>();
            _updateLpnCaseDetailsRequest = Generator.Default.Single<LpnDetailsUpdateDto>();
            _addLpnCommentRequest = Generator.Default.Single<CaseCommentDto>();
            _updateLpnCommentRequest = Generator.Default.Single<CaseCommentDto>();
            _caseLockCommentDto = Generator.Default.Single<CaseLockCommentDto>();

            _lpnMultipleUnlockDtos = Generator.Default.Single<List<LpnMultipleUnlockDto>>();
            _mockCaseHeaderService = new Mock<ICaseHeaderService>(MockBehavior.Default);
            _lpnController = new LpnController(_mockLpnService.Object, _mockCaseCommentService.Object,
                _mockCaseDetailService.Object, _mockCaseLockService.Object, _mockCaseHeaderService.Object);
            lpnNumber = "00100283000512198200";
            warehouse = "008";
            faceLocationId = "A07E06706N";
            commentSequenceNumber = 204;
            var lpnNumbers = new List<string> {"00100283000828329862,00100283000694052673,00100283009301204498"};
            lpnIds = lpnNumbers;
            lpnBatchUpdateDto = Generator.Default.Single<LpnBatchUpdateDto>();
            caseCommentDtos = Generator.Default.List<CaseCommentDto>(2);
        }

        #region Mock

        private void VerifyLpnSearch()
        {
            _mockLpnService.Verify(el => el.LpnSearchAsync(It.IsAny<LpnParameterDto>()));
        }

        private void MockLpnSearch(ResultTypes resultType)
        {
            var response = new BaseResult<LpnSearchResultsDto>
            {
                ResultType = resultType,
                Payload = resultType == ResultTypes.Ok ? Generator.Default.Single<LpnSearchResultsDto>() : null
            };
            _mockLpnService.Setup(el => el.LpnSearchAsync(It.IsAny<LpnParameterDto>()))
                .Returns(Task.FromResult(response));
        }

        private void VerifyLpnHistory()
        {
            _mockLpnService.Verify(el => el.GetLpnHistoryAsync(It.IsAny<string>(), It.IsAny<string>()));
        }

        private void MockLpnHistory(ResultTypes resultType)
        {
            var response = new BaseResult<List<LpnHistoryDto>>
            {
                ResultType = resultType,
                Payload = resultType == ResultTypes.Ok ? Generator.Default.Single<List<LpnHistoryDto>>() : null
            };
            _mockLpnService.Setup(el => el.GetLpnHistoryAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(response));
        }

        private void VerifyAisleTransaction()
        {
            _mockLpnService.Verify(el => el.GetAisleTransactionAsync(It.IsAny<string>(), It.IsAny<string>()));
        }

        private void MockAisleTransaction(ResultTypes resultType)
        {
            var response = new BaseResult<AisleTransactionDto>
            {
                ResultType = resultType,
                Payload = resultType == ResultTypes.Ok ? Generator.Default.Single<AisleTransactionDto>() : null
            };
            _mockLpnService.Setup(el => el.GetAisleTransactionAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(response));
        }

        private void VerifyDeleteComment()
        {
            _mockCaseCommentService.Verify(el => el.DeleteAsync(It.IsAny<string>(), It.IsAny<int>()));
        }

        private void MockDeleteComment(ResultTypes resultType)
        {
            var response = new BaseResult
            {
                ResultType = resultType
            };
            _mockCaseCommentService.Setup(el => el.DeleteAsync(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(response));
        }

        private void VerifyLpnUpdate()
        {
            _mockLpnService.Verify(el => el.UpdateLpnDetailsAsync(It.IsAny<LpnHeaderUpdateDto>()));
        }

        private void VerifyCaseCommentUpdate()
        {
            _mockCaseCommentService.Verify(el => el.UpdateAsync(It.IsAny<CaseCommentDto>()));
        }

        private void MockLpnUpdate(ResultTypes resultType)
        {
            var response = new BaseResult
            {
                ResultType = resultType
            };
            _mockLpnService.Setup(el => el.UpdateLpnDetailsAsync(It.IsAny<LpnHeaderUpdateDto>()))
                .Returns(Task.FromResult(response));
        }

        private void MockCaseCommentUpdate(ResultTypes resultType)
        {
            var response = new BaseResult<CaseCommentDto>
            {
                ResultType = resultType,
                Payload = _updateLpnCommentRequest
            };
            _mockCaseCommentService.Setup(el => el.UpdateAsync(It.IsAny<CaseCommentDto>()))
                .Returns(Task.FromResult(response));
        }

        private void VerifyUpdateLpnCaseDetails()
        {
            _mockCaseDetailService.Verify(el =>
                el.UpdateCaseDetailAssortAndCutNumberAsync(It.IsAny<LpnDetailsUpdateDto>()));
        }

        private void MockUpdateLpnCaseDetails(ResultTypes resultType)
        {
            var response = new BaseResult
            {
                ResultType = resultType
            };
            _mockCaseDetailService
                .Setup(el => el.UpdateCaseDetailAssortAndCutNumberAsync(It.IsAny<LpnDetailsUpdateDto>()))
                .Returns(Task.FromResult(response));
        }

        private void VerifyGetLpnComments()
        {
            _mockCaseCommentService.Verify(el => el.GetLpnCommentsWithCodeDescriptionAsync(It.IsAny<string>()));
        }

        private void VerifyAddLpnComments()
        {
            _mockCaseCommentService.Verify(el => el.InsertAsync(It.IsAny<CaseCommentDto>()));
        }

        private void VerifyGetLpnDetails()
        {
            _mockLpnService.Verify(el => el.GetLpnDetailsAsync(It.IsAny<string>()));
        }

        private void VerifyGetLpnLockUnlock()
        {
            _mockCaseLockService.Verify(el =>
                el.GetCaseLockUnlockAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        }

        private void VerifyGetLpnUnlockDetails()
        {
            _mockCaseLockService.Verify(el =>
                el.GetCaseUnLockDetailsAsync(It.IsAny<IEnumerable<string>>()));
        }

        private void MockGetLpnComments(ResultTypes resultType)
        {
            var response = new BaseResult<List<CaseCommentDto>>
            {
                ResultType = resultType,
                Payload = resultType == ResultTypes.Ok ? Generator.Default.Single<List<CaseCommentDto>>() : null
            };
            _mockCaseCommentService.Setup(el => el.GetLpnCommentsWithCodeDescriptionAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(response));
        }

        private void MockAddLpnComments(ResultTypes resultType)
        {
            var response = new BaseResult<CaseCommentDto>
            {
                ResultType = resultType
            };
            _mockCaseCommentService.Setup(el => el.InsertAsync(It.IsAny<CaseCommentDto>()))
                .Returns(Task.FromResult(response));
        }

        private void MockGetLpnDetails(ResultTypes resultType)
        {
            var response = new BaseResult<LpnDetailsDto>
            {
                ResultType = resultType,
                Payload = resultType == ResultTypes.Ok ? Generator.Default.Single<LpnDetailsDto>() : null
            };
            _mockLpnService.Setup(el => el.GetLpnDetailsAsync(It.IsAny<string>())).Returns(Task.FromResult(response));
        }

        private void MockGetLpnLockUnlock(ResultTypes resultType)
        {
            var response = new BaseResult<List<CaseLockUnlockDto>>
            {
                ResultType = resultType,
                Payload = resultType == ResultTypes.Ok ? Generator.Default.Single<List<CaseLockUnlockDto>>() : null
            };
            _mockCaseLockService
                .Setup(el => el.GetCaseLockUnlockAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(response));
        }

        private void MockGetLpnUnlockDetails(ResultTypes resultType)
        {
            var response = new BaseResult<List<CaseLockDto>>
            {
                ResultType = resultType,
                Payload = resultType == ResultTypes.Ok ? Generator.Default.Single<List<CaseLockDto>>() : null
            };
            _mockCaseLockService
                .Setup(el => el.GetCaseUnLockDetailsAsync(It.IsAny<IEnumerable<string>>()))
                .Returns(Task.FromResult(response));
        }

        private void MockAddCaseLockComment(ResultTypes resultType)
        {
            var response = new BaseResult<LpnMultipleUnlockResultDto>
            {
                ResultType = resultType,
                Payload = resultType == ResultTypes.Ok ? Generator.Default.Single<LpnMultipleUnlockResultDto>() : null
            };
            _mockCaseCommentService
                .Setup(el => el.AddCaseLockCommentWithBatchCorbaAsync(It.IsAny<CaseLockCommentDto>()))
                .Returns(Task.FromResult(response));
        }

        private void VerifyAddCaseLockComment()
        {
            _mockCaseCommentService.Verify(el =>
                el.AddCaseLockCommentWithBatchCorbaAsync(It.IsAny<CaseLockCommentDto>()));
        }

        private void MockUnlockCommentWithBatchCorba(ResultTypes resultType)
        {
            var response = new BaseResult<LpnMultipleUnlockResultDto>
            {
                ResultType = resultType,
                Payload = resultType == ResultTypes.Ok ? Generator.Default.Single<LpnMultipleUnlockResultDto>() : null
            };
            _mockCaseCommentService
                .Setup(el => el.UnlockCommentWithBatchCorbaAsync(It.IsAny<List<LpnMultipleUnlockDto>>()))
                .Returns(Task.FromResult(response));
        }

        private void VerifyUnlockCommentWithBatchCorba()
        {
            _mockCaseCommentService.Verify(el =>
                el.UnlockCommentWithBatchCorbaAsync(It.IsAny<List<LpnMultipleUnlockDto>>()));
        }

        #endregion Mock

        #region Lpn Search

        protected void ValidInputParametersForWhichRecordsExits()
        {
            MockLpnSearch(ResultTypes.Ok);
        }

        protected void InValidInputParametersAsInput()
        {
            MockLpnSearch(ResultTypes.BadRequest);
        }

        protected void FindLpnOperationInvoked()
        {
            _testResponse = _lpnController.LpnSearchAsync(_lpnSearchRequest);
        }

        protected void TheLpnSearchOperationReturnedOkAsResponseStatus()
        {
            VerifyLpnSearch();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as OkNegotiatedContentResult<BaseResult<LpnSearchResultsDto>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.Ok, result.Content.ResultType);
        }

        protected void TheFindLpnOperationReturnedBadRequestAsResponseStatus()
        {
            VerifyLpnSearch();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as NegotiatedContentResult<BaseResult<LpnSearchResultsDto>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.BadRequest, result.Content.ResultType);
        }

        #endregion Lpn Search

        #region LpnHistory

        protected void InputForLpnHistoryForWhichDetailsDoesNotExists()
        {
            MockLpnHistory(ResultTypes.NotFound);
        }

        protected void InputForLpnHistoryForWhichDetailsExists()
        {
            MockLpnHistory(ResultTypes.Ok);
        }

        protected void InValidInputForLpnHistory()
        {
            lpnNumber = warehouse = null;
            MockLpnHistory(ResultTypes.BadRequest);
        }

        protected void GetLpnHistoryOperationInvoked()
        {
            _testResponse = _lpnController.GetLpnHistoryAsync(lpnNumber, warehouse);
        }

        protected void TheGetLpnHistoryOperationReturnedNotFoundResponse()
        {
            VerifyLpnHistory();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as NegotiatedContentResult<BaseResult<List<LpnHistoryDto>>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.NotFound, result.Content.ResultType);
        }

        protected void TheGetLpnHistoryOperationReturnedOkResponse()
        {
            VerifyLpnHistory();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as OkNegotiatedContentResult<BaseResult<List<LpnHistoryDto>>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.Ok, result.Content.ResultType);
        }

        protected void TheGetLpnHistoryOperationReturnedBadRequestResponse()
        {
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as NegotiatedContentResult<BaseResult<List<LpnHistoryDto>>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.BadRequest, result.Content.ResultType);
        }

        #endregion LpnHistory

        #region AisleTransaction

        protected void InputForAisleTransactionForWhichRecordDoesNotExists()
        {
            MockAisleTransaction(ResultTypes.NotFound);
        }

        protected void InputForAisleTransactionForWhichRecordExists()
        {
            MockAisleTransaction(ResultTypes.Ok);
        }

        protected void InValidInputForAisleTransaction()
        {
            lpnNumber = faceLocationId = null;
            MockAisleTransaction(ResultTypes.BadRequest);
        }

        protected void AisleTransactionOperationInvoked()
        {
            _testResponse = _lpnController.GetAisleTransactionAsync(lpnNumber, faceLocationId);
        }

        protected void TheGetAisleTransactionOperationReturnedNotFoundResponse()
        {
            VerifyAisleTransaction();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as NegotiatedContentResult<BaseResult<AisleTransactionDto>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.NotFound, result.Content.ResultType);
        }

        protected void TheGetAisleTransactionOperationReturnedOkResponse()
        {
            VerifyAisleTransaction();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as OkNegotiatedContentResult<BaseResult<AisleTransactionDto>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.Ok, result.Content.ResultType);
        }

        protected void TheGetAisleTransactionOperationReturnedBadRequestResponse()
        {
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as NegotiatedContentResult<BaseResult<AisleTransactionDto>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.BadRequest, result.Content.ResultType);
        }

        #endregion AisleTransaction

        #region DeleteLpnComments

        protected void InputForDeleteLpnCommentsForWhichRecordDoesNotExists()
        {
            MockDeleteComment(ResultTypes.NotFound);
        }

        protected void InputForDeleteLpnCommentsForWhichRecordExists()
        {
            MockDeleteComment(ResultTypes.Ok);
        }

        protected void InValidInputForDeleteLpnComments()
        {
            commentSequenceNumber = 0;
            lpnNumber = null;
            MockDeleteComment(ResultTypes.BadRequest);
        }

        protected void DeleteLpnCommentsOperationInvoked()
        {
            _testResponse = _lpnController.DeleteLpnCommentAsync(lpnNumber, commentSequenceNumber);
        }

        protected void TheDeleteLpnCommentsOperationReturnedNotFoundResponse()
        {
            VerifyDeleteComment();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.NotFound, result.Content.ResultType);
        }

        protected void TheDeleteLpnCommentsOperationReturnedBadRequestResponse()
        {
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.BadRequest, result.Content.ResultType);
        }

        protected void TheDeleteLpnCommentsOperationReturnedOkResponse()
        {
            VerifyDeleteComment();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as OkNegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.Ok, result.Content.ResultType);
        }

        #endregion DeleteLpnComments

        #region LpnHeaderUpdate

        protected void InputForUpdateLpnForWhichRecordDoesNotExists()
        {
            MockLpnUpdate(ResultTypes.NotFound);
        }

        protected void InputForUpdateLpnForWhichRecordExists()
        {
            MockLpnUpdate(ResultTypes.Ok);
        }

        protected void InputForUpdateCaseCommentForWhichRecordExists()
        {
            MockCaseCommentUpdate(ResultTypes.Ok);
        }

        protected void InvalidInputForUpdateLpn()
        {
            MockLpnUpdate(ResultTypes.BadRequest);
        }

        protected void InvalidInputForUpdateCaseComment()
        {
            MockCaseCommentUpdate(ResultTypes.BadRequest);
        }

        protected void UpdateLpnOperationInvoked()
        {
            _testResponse = _lpnController.UpdateLpnHeaderAsync(_updateLpnHeaderUpdateDto);
        }

        protected void UpdateCaseCommentOperationInvoked()
        {
            _testResponse = _lpnController.UpdateLpnCommentAsync(_updateLpnCommentRequest);
        }

        protected void TheUpdateOperationReturnedNotFoundResponse()
        {
            VerifyLpnUpdate();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.NotFound, result.Content.ResultType);
        }

        protected void TheUpdateOperationReturnedBadRequestResponse()
        {
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.BadRequest, result.Content.ResultType);
        }

        protected void TheUpdateOperationReturnedOkResponse()
        {
            VerifyLpnUpdate();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as OkNegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.Ok, result.Content.ResultType);
        }

        protected void TheUpdateCaseCommentOperationReturnedOkResponse()
        {
            VerifyCaseCommentUpdate();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as OkNegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.Ok, result.Content.ResultType);
        }

        #endregion LpnHeaderUpdate

        #region UpdateLpnCaseDetails

        protected void UpdateCaseDetailsRecordDoesNotExists()
        {
            MockUpdateLpnCaseDetails(ResultTypes.NotFound);
        }

        protected void UpdateCaseDetailsRecordExists()
        {
            MockUpdateLpnCaseDetails(ResultTypes.Ok);
        }

        protected void InvalidInputForUpdateCaseDetails()
        {
            MockUpdateLpnCaseDetails(ResultTypes.BadRequest);
        }

        protected void UpdateCaseDetailsOperationInvoked()
        {
            _testResponse = _lpnController.UpdateLpnCaseDetailsAsync(_updateLpnCaseDetailsRequest);
        }

        protected void TheUpdateCaseDetailsOperationReturnedNotFoundResponse()
        {
            VerifyUpdateLpnCaseDetails();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.NotFound, result.Content.ResultType);
        }

        protected void TheUpdateCaseDetailsOperationReturnedBadRequestResponse()
        {
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.BadRequest, result.Content.ResultType);
        }

        protected void TheUpdateCaseDetailsOperationReturnedOkResponse()
        {
            VerifyUpdateLpnCaseDetails();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as OkNegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.Ok, result.Content.ResultType);
        }

        #endregion UpdateLpnCaseDetails

        #region GetLpnComments

        protected void GetLpnCommentsRecordDoesNotExists()
        {
            MockGetLpnComments(ResultTypes.NotFound);
        }

        protected void GetLpnCommentsRecordExists()
        {
            MockGetLpnComments(ResultTypes.Ok);
        }

        protected void GetLpnCommentsOperationInvoked()
        {
            lpnNumber = "00100283000829265701";
            _testResponse = _lpnController.GetLpnCommentsAsync(lpnNumber);
        }

        protected void TheGetLpnCommentsOperationReturnedNotFoundResponse()
        {
            VerifyGetLpnComments();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as NegotiatedContentResult<BaseResult<List<CaseCommentDto>>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.NotFound, result.Content.ResultType);
        }

        protected void TheGetLpnCommentsOperationReturnedOkResponse()
        {
            VerifyGetLpnComments();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as OkNegotiatedContentResult<BaseResult<List<CaseCommentDto>>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.Ok, result.Content.ResultType);
        }

        #endregion GetLpnComments

        #region AddLpnComments

        protected void ValidInputForWhichNoRecordExits()
        {
            MockAddLpnComments(ResultTypes.Created);
        }

        protected void AddLpnCommentsRecordExists()
        {
            MockAddLpnComments(ResultTypes.Conflict);
        }

        protected void InvalidInputForLpnComments()
        {
            MockAddLpnComments(ResultTypes.BadRequest);
        }

        protected void AddLpnCommentsOperationInvoked()
        {
            _testResponse = _lpnController.AddLpnCommentAsync(_addLpnCommentRequest);
        }

        protected void TheAddLpnCommentsOperationReturnedConflictResponse()
        {
            VerifyAddLpnComments();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as NegotiatedContentResult<BaseResult<CaseCommentDto>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.Conflict, result.Content.ResultType);
        }

        protected void TheAddLpnCommentsOperationReturnedCreatedResponse()
        {
            VerifyAddLpnComments();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as CreatedAtRouteNegotiatedContentResult<BaseResult<CaseCommentDto>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.Created, result.Content.ResultType);
        }

        protected void TheAddLpnCommentsOperationReturnedBadRequestResponse()
        {
            VerifyAddLpnComments();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as NegotiatedContentResult<BaseResult<CaseCommentDto>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.BadRequest, result.Content.ResultType);
        }

        #endregion AddLpnComments

        #region GetLpnDetails

        protected void GetLpnDetailsRecordDoesNotExists()
        {
            MockGetLpnDetails(ResultTypes.NotFound);
        }

        protected void GetLpnDetailsRecordExists()
        {
            MockGetLpnDetails(ResultTypes.Ok);
        }

        protected void InvalidInputForGetLpnDetails()
        {
            lpnNumber = null;
            MockGetLpnDetails(ResultTypes.BadRequest);
        }

        protected void GetLpnDetailsOperationInvoked()
        {
            _testResponse = _lpnController.GetLpnDetailsAsync(lpnNumber);
        }

        protected void TheGetLpnDetailsOperationReturnedNotFoundResponse()
        {
            VerifyGetLpnDetails();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as NegotiatedContentResult<BaseResult<LpnDetailsDto>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.NotFound, result.Content.ResultType);
        }

        protected void TheGetLpnDetailsOperationReturnedBadRequestResponse()
        {
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as NegotiatedContentResult<BaseResult<LpnDetailsDto>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.BadRequest, result.Content.ResultType);
        }

        protected void TheGetLpnDetailsOperationReturnedOkResponse()
        {
            VerifyGetLpnDetails();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as OkNegotiatedContentResult<BaseResult<LpnDetailsDto>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.Ok, result.Content.ResultType);
        }

        #endregion GetLpnDetails

        #region GetLpnLockUnlock

        protected void GetLpnLockUnlockRecordDoesNotExists()
        {
            MockGetLpnLockUnlock(ResultTypes.NotFound);
        }

        protected void GetLpnLockUnlockRecordExists()
        {
            MockGetLpnLockUnlock(ResultTypes.Ok);
        }

        protected void InValidInputForGetLpnLockUnlock()
        {
            MockGetLpnLockUnlock(ResultTypes.BadRequest);
        }

        protected void GetLpnLockUnlockOperationInvoked()
        {
            lpnNumber = "00100283000825583618";
            _testResponse = _lpnController.GetLpnLockUnlockAsync(lpnNumber);
        }

        protected void TheGetLpnLockUnlockOperationReturnedNotFoundResponse()
        {
            VerifyGetLpnLockUnlock();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as NegotiatedContentResult<BaseResult<List<CaseLockUnlockDto>>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.NotFound, result.Content.ResultType);
        }

        protected void TheGetLpnLockUnlockOperationReturnedBadRequestResponse()
        {
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as NegotiatedContentResult<BaseResult<List<CaseLockUnlockDto>>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.BadRequest, result.Content.ResultType);
        }

        protected void TheGetLpnLockUnlockOperationReturnedOkResponse()
        {
            VerifyGetLpnLockUnlock();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as OkNegotiatedContentResult<BaseResult<List<CaseLockUnlockDto>>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.Ok, result.Content.ResultType);
        }

        #endregion GetLpnLockUnlock

        #region GetLpnUnlockDetails

        protected void GetCaseUnLockDetailsRecordDoesNotExists()
        {
            MockGetLpnUnlockDetails(ResultTypes.NotFound);
        }

        protected void GetCaseUnLockDetailsRecordExists()
        {
            MockGetLpnUnlockDetails(ResultTypes.Ok);
        }

        protected void InValidInputForGetCaseUnLockDetails()
        {
            MockGetLpnUnlockDetails(ResultTypes.BadRequest);
        }

        protected void GetCaseUnLockDetailsOperationInvoked()
        {
            _testResponse = _lpnController.GetCaseUnLockDetailsAsync(lpnIds);
        }

        protected void TheGetCaseUnLockDetailsReturnedNotFoundResponse()
        {
            VerifyGetLpnUnlockDetails();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as NegotiatedContentResult<BaseResult<List<CaseLockDto>>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.NotFound, result.Content.ResultType);
        }

        protected void ThGetCaseUnLockDetailsReturnedBadRequestResponse()
        {
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as NegotiatedContentResult<BaseResult<List<CaseLockDto>>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.BadRequest, result.Content.ResultType);
        }

        protected void TheGetCaseUnLockDetailsReturnedOkResponse()
        {
            VerifyGetLpnUnlockDetails();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as OkNegotiatedContentResult<BaseResult<List<CaseLockDto>>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.Ok, result.Content.ResultType);
        }

        #endregion GetLpnUnlockDetails

        #region AddCaseLockComment

        protected void ValidParametersToAddCaseLockComments()
        {
            MockAddCaseLockComment(ResultTypes.Created);
        }

        protected void InvalidParametersToAddCaseLockComments()
        {
            MockAddCaseLockComment(ResultTypes.NotFound);
        }

        protected void AddCaseLockCommentsInvoked()
        {
            _testResponse = _lpnController.CaseLockCommentWithBatchCorbaAsync(_caseLockCommentDto);
        }

        protected void AddCaseLockCommentsOperationReturnedNotFoundResponse()
        {
            VerifyAddCaseLockComment();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as NegotiatedContentResult<BaseResult<LpnMultipleUnlockResultDto>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.NotFound, result.Content.ResultType);
        }

        protected void AddCaseLockCommentsOperationReturnedOkResponse()
        {
            VerifyAddCaseLockComment();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as OkNegotiatedContentResult<BaseResult<LpnMultipleUnlockResultDto>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.Created, result.Content.ResultType);
        }

        #endregion AddCaseLockComment

        #region UnlockCommentWithBatchCorba

        protected void ValidParametersToUnlockComment()
        {
            MockUnlockCommentWithBatchCorba(ResultTypes.Ok);
        }

        protected void InvalidParametersToUnlockComment()
        {
            MockUnlockCommentWithBatchCorba(ResultTypes.NotFound);
        }

        protected void UnlockCommentWithBatchCorbaInvoked()
        {
            _testResponse = _lpnController.UnlockCommentWithBatchCorbaAsync(_lpnMultipleUnlockDtos);
        }

        protected void UnlockCommentWithBatchCorbaReturnedOkResponse()
        {
            VerifyUnlockCommentWithBatchCorba();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as OkNegotiatedContentResult<BaseResult<LpnMultipleUnlockResultDto>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.Ok, result.Content.ResultType);
        }

        protected void UnlockCommentWithBatchCorbaReturnedNotFoundResponse()
        {
            VerifyUnlockCommentWithBatchCorba();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as NegotiatedContentResult<BaseResult<LpnMultipleUnlockResultDto>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.NotFound, result.Content.ResultType);
        }

        #endregion UnlockCommentWithBatchCorba

        #region Lpn Batch update

        private void MockLpnBatchUpdate(ResultTypes resultTypes)
        {
            _mockCaseHeaderService.Setup(el => el.LpnBatchUpdateAsync(It.IsAny<LpnBatchUpdateDto>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new BaseResult {ResultType = resultTypes}));
        }

        private void VerifyLpnBatchUpdate()
        {
            _mockCaseHeaderService.Verify(el =>
                el.LpnBatchUpdateAsync(It.IsAny<LpnBatchUpdateDto>(), It.IsAny<bool>()));
        }

        protected void EmptyOrNullInputForLpnBatchUpdate()
        {
            lpnBatchUpdateDto = null;
            MockLpnBatchUpdate(ResultTypes.BadRequest);
        }

        protected void InputForMultipleLpnUpdate()
        {
            MockLpnBatchUpdate(ResultTypes.NotFound);
        }

        protected void ValidInputForMultipleLpnUpdate()
        {
            MockLpnBatchUpdate(ResultTypes.Ok);
        }

        protected void LpnBatchUpdateInvocation()
        {
            _testResponse = _lpnController.MultipleLpnUpdateAsync(lpnBatchUpdateDto);
        }

        protected void LpnBatchUpdateReturnedOkAsResponseStatus()
        {
            VerifyLpnBatchUpdate();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as OkNegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.Ok, result.Content.ResultType);
        }

        protected void LpnBatchUpdateReturnedNotFoundAsResponseStatus()
        {
            VerifyLpnBatchUpdate();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.NotFound, result.Content.ResultType);
        }

        protected void LpnBatchUpdateReturnedBadRequestAsResponseStatus()
        {
            VerifyLpnBatchUpdate();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.BadRequest, result.Content.ResultType);
        }

        #endregion

        #region Batch Case comments insertion

        private void MockBatchCommentsInsertion(ResultTypes resultTypes)
        {
            _mockCaseCommentService.Setup(el => el.BatchInsertAsync(It.IsAny<IEnumerable<CaseCommentDto>>()))
                .Returns(Task.FromResult(new BaseResult {ResultType = resultTypes}));
        }

        private void VerifyBatchCommentsInsertion()
        {
            _mockCaseCommentService.Verify(el => el.BatchInsertAsync(It.IsAny<IEnumerable<CaseCommentDto>>()));
        }

        protected void EmptyOrNullInputForBatchCommentsInsertion()
        {
            caseCommentDtos = null;
            MockBatchCommentsInsertion(ResultTypes.BadRequest);
        }

        protected void InputForBatchCommentsInsertion()
        {
            MockBatchCommentsInsertion(ResultTypes.NotFound);
        }

        protected void ValidInputForBatchCommentsInsertion()
        {
            MockBatchCommentsInsertion(ResultTypes.Created);
        }

        protected void BatchCommentsInsertionInvocation()
        {
            _testResponse = _lpnController.MultipleLpnCommentsAddAsync(caseCommentDtos);
        }

        protected void BatchCommentsInsertionReturnedCreatedAsResponseStatus()
        {
            VerifyBatchCommentsInsertion();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as OkNegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.Created, result.Content.ResultType);
        }

        protected void BatchCommentsInsertionReturnedNotFoundAsResponseStatus()
        {
            VerifyBatchCommentsInsertion();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.NotFound, result.Content.ResultType);
        }

        protected void BatchCommentsInsertionReturnedBadRequestAsResponseStatus()
        {
            VerifyBatchCommentsInsertion();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.BadRequest, result.Content.ResultType);
        }

        #endregion
    }
}