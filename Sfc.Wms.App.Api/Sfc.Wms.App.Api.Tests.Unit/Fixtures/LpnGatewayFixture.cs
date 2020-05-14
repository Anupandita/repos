using System.Collections.Generic;
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
using Sfc.Wms.Foundation.InboundLpn.Contracts.Dtos;

namespace Sfc.Wms.App.Api.Tests.Unit.Fixtures
{
    public abstract class LpnGatewayFixture
    {
        private readonly LpnParameterDto _findLpnRequest;
        private readonly LpnGateway _lpnGateway;
        private readonly List<LpnMultipleUnlockDto> _lpnMultipleUnlockDtos;
        private readonly Mock<IRestClient> _restClient;
        private BaseResult<CaseCommentDto> caseCommentBaseResult;
        private IEnumerable<CaseCommentDto> caseCommentDtos;
        private BaseResult<List<CaseLockUnlockDto>> caseLockUnlockBaseResult;
        private BaseResult<List<CaseLockDto>> caseUnlockDetailsBaseResult;
        private BaseResult<List<LpnHistoryDto>> lnpHistoryBaseResult;
        private LpnBatchUpdateDto lpnBatchUpdateDto;
        private BaseResult<List<CaseCommentDto>> lpnCommentsBaseResult;
        private BaseResult<LpnDetailsDto> lpnDetailsBaseResult;
        private BaseResult<LpnMultipleUnlockResultDto> lpnMultipleUnlockResult;
        private BaseResult manipulationTestResult;

        protected LpnGatewayFixture()
        {
            _restClient = new Mock<IRestClient>();
            _findLpnRequest = Generator.Default.Single<LpnParameterDto>();
            _lpnMultipleUnlockDtos = Generator.Default.Single<List<LpnMultipleUnlockDto>>();
            _lpnGateway = new LpnGateway(new ResponseBuilder(), _restClient.Object);
            lpnBatchUpdateDto = Generator.Default.Single<LpnBatchUpdateDto>();
            caseCommentDtos = Generator.Default.List<CaseCommentDto>(2);
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

        #region Lpn search

        protected void ValidLpnSearchParametersAsInput()
        {
            var result = new BaseResult<LpnSearchResultsDto>
                {ResultType = ResultTypes.Ok, Payload = Generator.Default.Single<LpnSearchResultsDto>()};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InValidLpnSearchParameters()
        {
            var result = new BaseResult<LpnSearchResultsDto> {ResultType = ResultTypes.BadRequest};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void LpnSearchOperationInvoked()
        {
            manipulationTestResult = _lpnGateway.LpnSearchAsync(_findLpnRequest, It.IsAny<string>())
                .Result;
        }

        protected void TheLpnSearchOperationReturnedOkAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<LpnSearchResultsDto>>();
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(ResultTypes.Ok, manipulationTestResult.ResultType);
        }

        protected void TheLpnSearchOperationReturnedBadRequestAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<LpnSearchResultsDto>>();
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(ResultTypes.BadRequest, manipulationTestResult.ResultType);
        }

        #endregion Lpn search

        #region Get Lpn Details By LpnId

        protected void ValidInputParametersToGetLpnDetails()
        {
            var result = new BaseResult<LpnDetailsDto> {ResultType = ResultTypes.Ok};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InValidInputParametersToGetLpnDetails()
        {
            var result = new BaseResult<LpnDetailsDto> {ResultType = ResultTypes.BadRequest};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void GetLpnDetailsOperationInvoked()
        {
            lpnDetailsBaseResult = _lpnGateway.GetLpnDetailsByLpnIdAsync(It.IsAny<string>(), It.IsAny<string>())
                .Result;
        }

        protected void TheGetLpnDetailsOperationReturnedOkAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<LpnDetailsDto>>();
            Assert.IsNotNull(lpnDetailsBaseResult);
            Assert.AreEqual(ResultTypes.Ok, lpnDetailsBaseResult.ResultType);
        }

        protected void TheGetLpnDetailsReturnedBadRequestAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<LpnDetailsDto>>();
            Assert.IsNotNull(lpnDetailsBaseResult);
            Assert.AreEqual(ResultTypes.BadRequest, lpnDetailsBaseResult.ResultType);
        }

        #endregion Get Lpn Details By LpnId

        #region Get Lpn Details By LpnId

        protected void ValidInputParametersToInsertLpnComments()
        {
            var result = new BaseResult<CaseCommentDto> {ResultType = ResultTypes.Created};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InValidInputParametersToInsertLpnComments()
        {
            var result = new BaseResult<CaseCommentDto> {ResultType = ResultTypes.BadRequest};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InputParametersToInsertLpnCommentsForWhichRecordExists()
        {
            var result = new BaseResult<CaseCommentDto> {ResultType = ResultTypes.Conflict};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InsertLpnCommentsOperationInvoked()
        {
            caseCommentBaseResult = _lpnGateway.InsertLpnCommentsAsync(It.IsAny<CaseCommentDto>(), It.IsAny<string>())
                .Result;
        }

        protected void TheInsertLpnCommentsOperationReturnedCreatedAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<CaseCommentDto>>();
            Assert.IsNotNull(caseCommentBaseResult);
            Assert.AreEqual(ResultTypes.Created, caseCommentBaseResult.ResultType);
        }

        protected void TheInsertLpnCommentsReturnedBadRequestAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<CaseCommentDto>>();
            Assert.IsNotNull(caseCommentBaseResult);
            Assert.AreEqual(ResultTypes.BadRequest, caseCommentBaseResult.ResultType);
        }

        protected void TheInsertLpnCommentsReturnedConflictAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<CaseCommentDto>>();
            Assert.IsNotNull(caseCommentBaseResult);
            Assert.AreEqual(ResultTypes.Conflict, caseCommentBaseResult.ResultType);
        }

        #endregion Get Lpn Details By LpnId

        #region Get Lpn Comments By LpnId

        protected void ValidInputParametersToGetLpnComments()
        {
            var result = new BaseResult<List<CaseCommentDto>> {ResultType = ResultTypes.Ok};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InValidInputParametersToGetLpnComments()
        {
            var result = new BaseResult<List<CaseCommentDto>> {ResultType = ResultTypes.BadRequest};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InputParametersToGetLpnCommentsForNoCommentsExists()
        {
            var result = new BaseResult<List<CaseCommentDto>> {ResultType = ResultTypes.NotFound};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void GetLpnCommentsByLpnIdOperationInvoked()
        {
            lpnCommentsBaseResult = _lpnGateway.GetLpnCommentsByLpnIdAsync(It.IsAny<string>(), It.IsAny<string>())
                .Result;
        }

        protected void TheGetLpnCommentsByLpnIdReturnedOkAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<List<CaseCommentDto>>>();
            Assert.IsNotNull(lpnCommentsBaseResult);
            Assert.AreEqual(ResultTypes.Ok, lpnCommentsBaseResult.ResultType);
        }

        protected void TheGetLpnCommentsByLpnIdReturnedBadRequestAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<List<CaseCommentDto>>>();
            Assert.IsNotNull(lpnCommentsBaseResult);
            Assert.AreEqual(ResultTypes.BadRequest, lpnCommentsBaseResult.ResultType);
        }

        protected void TheGetLpnCommentsByLpnIdReturnedNotFoundAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<List<CaseCommentDto>>>();
            Assert.IsNotNull(lpnCommentsBaseResult);
            Assert.AreEqual(ResultTypes.NotFound, lpnCommentsBaseResult.ResultType);
        }

        #endregion Get Lpn Comments By LpnId

        #region GetLpnHistory

        protected void ValidInputParametersToGetLpnHistory()
        {
            var result = new BaseResult<List<LpnHistoryDto>> {ResultType = ResultTypes.Ok};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InValidInputParametersToGetLpnHistory()
        {
            var result = new BaseResult<List<LpnHistoryDto>> {ResultType = ResultTypes.BadRequest};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InputParametersToGetLpnHistoryForWhichNoCommentsExists()
        {
            var result = new BaseResult<List<LpnHistoryDto>> {ResultType = ResultTypes.NotFound};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void GetLpnHistoryByLpnIdAndWhseOperationInvoked()
        {
            lnpHistoryBaseResult = _lpnGateway
                .GetLpnHistoryByLpnIdAndWhseAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())
                .Result;
        }

        protected void TheGetLpnHistoryByLpnIdAndWhseReturnedOkAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<List<LpnHistoryDto>>>();
            Assert.IsNotNull(lnpHistoryBaseResult);
            Assert.AreEqual(ResultTypes.Ok, lnpHistoryBaseResult.ResultType);
        }

        protected void TheGetLpnHistoryByLpnIdAndWhseReturnedBadRequestAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<List<LpnHistoryDto>>>();
            Assert.IsNotNull(lnpHistoryBaseResult);
            Assert.AreEqual(ResultTypes.BadRequest, lnpHistoryBaseResult.ResultType);
        }

        protected void TheGetLpnHistoryByLpnIdAndWhseReturnedNotFoundAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<List<LpnHistoryDto>>>();
            Assert.IsNotNull(lnpHistoryBaseResult);
            Assert.AreEqual(ResultTypes.NotFound, lnpHistoryBaseResult.ResultType);
        }

        #endregion GetLpnHistory

        #region Get Lpn LockUnlock By LpnId

        protected void ValidInputParametersToGetLpnLockUnlockDetails()
        {
            var result = new BaseResult<List<CaseLockUnlockDto>> {ResultType = ResultTypes.Ok};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InValidInputParametersToGetLpnLockUnlockDetails()
        {
            var result = new BaseResult<List<CaseLockUnlockDto>> {ResultType = ResultTypes.BadRequest};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InputParametersToGetLpnLockUnlockByLpnIdForWhichNoDetailsExists()
        {
            var result = new BaseResult<List<CaseLockUnlockDto>> {ResultType = ResultTypes.NotFound};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void GetLpnLockUnlockByLpnIdOperationInvoked()
        {
            caseLockUnlockBaseResult = _lpnGateway.GetLpnLockUnlockByLpnIdAsync(It.IsAny<string>(), It.IsAny<string>())
                .Result;
        }

        protected void TheGetLpnLockUnlockByLpnIdReturnedOkAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<List<CaseLockUnlockDto>>>();
            Assert.IsNotNull(caseLockUnlockBaseResult);
            Assert.AreEqual(ResultTypes.Ok, caseLockUnlockBaseResult.ResultType);
        }

        protected void TheGetLpnLockUnlockByLpnIdReturnedBadRequestAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<List<CaseLockUnlockDto>>>();
            Assert.IsNotNull(caseLockUnlockBaseResult);
            Assert.AreEqual(ResultTypes.BadRequest, caseLockUnlockBaseResult.ResultType);
        }

        protected void TheGetLpnLockUnlockByLpnIdReturnedNotFoundAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<List<CaseLockUnlockDto>>>();
            Assert.IsNotNull(caseLockUnlockBaseResult);
            Assert.AreEqual(ResultTypes.NotFound, caseLockUnlockBaseResult.ResultType);
        }

        #endregion Get Lpn LockUnlock By LpnId

        #region GetCaseUnLockDetails

        protected void ValidInputParametersToGetCaseUnLockDetails()
        {
            var result = new BaseResult<List<CaseLockDto>> {ResultType = ResultTypes.Ok};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InValidInputParametersToGetCaseUnLockDetails()
        {
            var result = new BaseResult<List<CaseLockDto>> {ResultType = ResultTypes.BadRequest};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InputParametersToGetCaseUnLockDetailsForWhichNoDetailsExists()
        {
            var result = new BaseResult<List<CaseLockDto>> {ResultType = ResultTypes.NotFound};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void GetCaseUnLockDetailsOperationInvoked()
        {
            caseUnlockDetailsBaseResult = _lpnGateway
                .GetCaseUnLockDetailsAsync(It.IsAny<IEnumerable<string>>(), It.IsAny<string>())
                .Result;
        }

        protected void TheGetCaseUnLockDetailsReturnedOkAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<List<CaseLockDto>>>();
            Assert.IsNotNull(caseUnlockDetailsBaseResult);
            Assert.AreEqual(ResultTypes.Ok, caseUnlockDetailsBaseResult.ResultType);
        }

        protected void TheGetCaseUnLockDetailsReturnedBadRequestAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<List<CaseLockDto>>>();
            Assert.IsNotNull(caseUnlockDetailsBaseResult);
            Assert.AreEqual(ResultTypes.BadRequest, caseUnlockDetailsBaseResult.ResultType);
        }

        protected void TheGetCaseUnLockDetailsReturnedNotFoundAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult<List<CaseLockDto>>>();
            Assert.IsNotNull(caseUnlockDetailsBaseResult);
            Assert.AreEqual(ResultTypes.NotFound, caseUnlockDetailsBaseResult.ResultType);
        }

        #endregion GetCaseUnLockDetails

        #region Update LpnHeader

        protected void ValidInputParametersToUpdateLpnHeader()
        {
            var result = new BaseResult {ResultType = ResultTypes.Ok};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InValidInputParametersToUpdateLpnHeader()
        {
            var result = new BaseResult {ResultType = ResultTypes.BadRequest};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InputParametersToUpdateLpnHeaderForWhichNoDetailsExists()
        {
            var result = new BaseResult {ResultType = ResultTypes.NotFound};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void UpdateLpnHeaderOperationInvoked()
        {
            manipulationTestResult = _lpnGateway
                .UpdateLpnHeaderAsync(It.IsAny<LpnHeaderUpdateDto>(), It.IsAny<string>())
                .Result;
        }

        protected void TheUpdateLpnHeaderReturnedOkAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult>();
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(ResultTypes.Ok, manipulationTestResult.ResultType);
        }

        protected void TheUpdateLpnHeaderReturnedBadRequestAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult>();
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(ResultTypes.BadRequest, manipulationTestResult.ResultType);
        }

        protected void TheUpdateLpnHeaderReturnedNotFoundAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult>();
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(ResultTypes.NotFound, manipulationTestResult.ResultType);
        }

        #endregion Update LpnHeader

        #region DeleteLpnComments

        protected void ValidInputParametersToDeleteLpnComment()
        {
            var result = new BaseResult {ResultType = ResultTypes.Ok};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InValidInputParametersToDeleteLpnComment()
        {
            var result = new BaseResult {ResultType = ResultTypes.BadRequest};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InputParametersToDeleteLpnCommentForWhichNoDetailsExists()
        {
            var result = new BaseResult {ResultType = ResultTypes.NotFound};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void DeleteLpnCommentOperationInvoked()
        {
            manipulationTestResult = _lpnGateway
                .DeleteLpnCommentsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<string>())
                .Result;
        }

        protected void TheDeleteLpnCommentReturnedOkAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult>();
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(ResultTypes.Ok, manipulationTestResult.ResultType);
        }

        protected void TheDeleteLpnCommentReturnedBadRequestAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult>();
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(ResultTypes.BadRequest, manipulationTestResult.ResultType);
        }

        protected void TheDeleteLpnCommentReturnedNotFoundAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult>();
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(ResultTypes.NotFound, manipulationTestResult.ResultType);
        }

        #endregion DeleteLpnComments

        #region Update Case Details

        protected void ValidInputParametersToUpdateLpnDetail()
        {
            var result = new BaseResult {ResultType = ResultTypes.Ok};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InValidInputParametersToUpdateLpnDetail()
        {
            var result = new BaseResult {ResultType = ResultTypes.BadRequest};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InputParametersToUpdateLpnDetailForWhichNoDetailsExists()
        {
            var result = new BaseResult {ResultType = ResultTypes.NotFound};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void UpdateLpnDetailOperationInvoked()
        {
            manipulationTestResult = _lpnGateway
                .UpdateLpnDetailsAsync(It.IsAny<LpnDetailsUpdateDto>(), It.IsAny<string>())
                .Result;
        }

        protected void TheUpdateLpnDetailReturnedOkAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult>();
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(ResultTypes.Ok, manipulationTestResult.ResultType);
        }

        protected void TheUpdateLpnDetailReturnedBadRequestAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult>();
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(ResultTypes.BadRequest, manipulationTestResult.ResultType);
        }

        protected void TheUpdateLpnDetailReturnedNotFoundAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult>();
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(ResultTypes.NotFound, manipulationTestResult.ResultType);
        }

        #endregion Update Case Details

        #region AddCaseLockComments

        protected void ValidParametersToAddCaseLockComments()
        {
            var result = new BaseResult<LpnMultipleUnlockResultDto> {ResultType = ResultTypes.Ok};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InvalidParametersToAddCaseLockComments()
        {
            var result = new BaseResult<LpnMultipleUnlockResultDto> {ResultType = ResultTypes.BadRequest};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void AddCaseLockCommentsInvoked()
        {
            manipulationTestResult = _lpnGateway
                .CaseLockCommentWithBatchCorbaAsync(It.IsAny<CaseLockCommentDto>(), It.IsAny<string>()).Result;
        }

        protected void AddCaseLockCommentsReturnedOkResponse()
        {
            VerifyRestClientInvocation<BaseResult<LpnMultipleUnlockResultDto>>();
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(ResultTypes.Ok, manipulationTestResult.ResultType);
        }

        protected void AddCaseLockCommentsReturnedNotFoundResponse()
        {
            VerifyRestClientInvocation<BaseResult<LpnMultipleUnlockResultDto>>();
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(ResultTypes.BadRequest, manipulationTestResult.ResultType);
        }

        #endregion AddCaseLockComments

        #region UnlockCommentWithBatchCorba

        protected void ValidParametersToUnlockComment()
        {
            var result = new BaseResult<LpnMultipleUnlockResultDto>
                {ResultType = ResultTypes.Ok, Payload = Generator.Default.Single<LpnMultipleUnlockResultDto>()};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InvalidParametersToUnlockComment()
        {
            var result = new BaseResult<LpnMultipleUnlockResultDto> {ResultType = ResultTypes.BadRequest};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void UnlockCommentWithBatchCorbaInvoked()
        {
            lpnMultipleUnlockResult =
                _lpnGateway.LpnMultipleUnlockAsync(_lpnMultipleUnlockDtos, It.IsAny<string>()).Result;
        }

        protected void UnlockCommentWithBatchCorbaReturnedOkResponse()
        {
            VerifyRestClientInvocation<BaseResult<LpnMultipleUnlockResultDto>>();
            Assert.IsNotNull(lpnMultipleUnlockResult);
            Assert.AreEqual(ResultTypes.Ok, lpnMultipleUnlockResult.ResultType);
        }

        protected void UnlockCommentWithBatchCorbaReturnedNotFoundResponse()
        {
            VerifyRestClientInvocation<BaseResult<LpnMultipleUnlockResultDto>>();
            Assert.IsNotNull(lpnMultipleUnlockResult);
            Assert.AreEqual(ResultTypes.BadRequest, lpnMultipleUnlockResult.ResultType);
        }

        #endregion UnlockCommentWithBatchCorba

        #region Bacth lpn update

        protected void ValidInputParametersToMultipleLpnUpdate()
        {
            var result = new BaseResult {ResultType = ResultTypes.Ok};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InValidInputParametersToMultipleLpnUpdate()
        {
            var result = new BaseResult {ResultType = ResultTypes.BadRequest};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InputParametersToMultipleLpnUpdateForWhichNoDetailsExists()
        {
            var result = new BaseResult {ResultType = ResultTypes.NotFound};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void MultipleLpnUpdateOperationInvoked()
        {
            manipulationTestResult = _lpnGateway
                .MultipleLpnsUpdateAsync(It.IsAny<LpnBatchUpdateDto>(), It.IsAny<string>())
                .Result;
        }

        protected void TheMultipleLpnUpdateReturnedOkAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult>();
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(ResultTypes.Ok, manipulationTestResult.ResultType);
        }

        protected void TheMultipleLpnUpdateReturnedBadRequestAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult>();
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(ResultTypes.BadRequest, manipulationTestResult.ResultType);
        }

        protected void TheMultipleLpnUpdateReturnedNotFoundAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult>();
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(ResultTypes.NotFound, manipulationTestResult.ResultType);
        }

        #endregion DeleteLpnComments


        #region Bacth Case Comments Insertion

        protected void ValidInputParametersToBatchCommentsInsertion()
        {
            var result = new BaseResult {ResultType = ResultTypes.Ok};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InValidInputParametersToBatchCommentsInsertion()
        {
            var result = new BaseResult {ResultType = ResultTypes.BadRequest};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InputParametersToBatchCommentsInsertionForWhichNoDetailsExists()
        {
            var result = new BaseResult {ResultType = ResultTypes.NotFound};
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void BatchCommentsInsertionOperationInvoked()
        {
            manipulationTestResult = _lpnGateway
                .MultipleLpnsCommentsAddAsync(It.IsAny<IEnumerable<CaseCommentDto>>(), It.IsAny<string>())
                .Result;
        }

        protected void TheBatchCommentsInsertionReturnedOkAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult>();
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(ResultTypes.Ok, manipulationTestResult.ResultType);
        }

        protected void TheBatchCommentsInsertionReturnedBadRequestAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult>();
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(ResultTypes.BadRequest, manipulationTestResult.ResultType);
        }

        protected void TheBatchCommentsInsertionReturnedNotFoundAsResponseStatus()
        {
            VerifyRestClientInvocation<BaseResult>();
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(ResultTypes.NotFound, manipulationTestResult.ResultType);
        }

        #endregion DeleteLpnComments
    }
}