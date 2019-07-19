using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Dematic.Contracts.EnumsAndConstants.Enums;
using Sfc.Wms.Asrs.Nuget.Gateways;
using Sfc.Wms.Result;

namespace Sfc.Wms.Asrs.Test.Unit.Fixtures.Nuget
{
    public abstract class WmsToEmsGatewayFixture
    {
        private readonly WmsToEmsGateway _emsToWmsGateway;

        private readonly Mock<IRestClient> _restClient;
        private BaseResult<IEnumerable<WmsToEmsDto>> getAllTestResult;
        private BaseResult<WmsToEmsDto> getDetailsTestResult;
        private BaseResult manipulationTestResult;

        protected WmsToEmsGatewayFixture()
        {
            _restClient = new Mock<IRestClient>();
            _emsToWmsGateway = new WmsToEmsGateway(_restClient.Object);
        }

        private void GetRestResponse1<T>(T entity, HttpStatusCode statusCode, ResponseStatus responseStatus)
            where T : new()
        {
            var response = new Mock<IRestResponse<T>>();
            response.Setup(_ => _.StatusCode).Returns(statusCode);
            response.Setup(_ => _.ResponseStatus).Returns(responseStatus);
            response.Setup(_ => _.Content).Returns(JsonConvert.SerializeObject(entity));
            _restClient.Setup(x => x.ExecuteTaskAsync<T>(It.IsAny<IRestRequest>()))
                .Returns(Task.FromResult(response.Object));
        }

        #region GetAsync All

        protected void GetAllInvoked()
        {
            var result = new BaseResult<IEnumerable<WmsToEmsDto>>
            {
                Payload = Generator.Default.List<WmsToEmsDto>(),
                ResultType = ResultTypes.Ok
            };
            GetRestResponse1(result, HttpStatusCode.OK, ResponseStatus.Completed);

            getAllTestResult = _emsToWmsGateway.GetAsync().Result;
        }

        protected void TheGetAllOperationReturnedOkResponse()
        {
            Assert.IsNotNull(getAllTestResult);
            Assert.AreEqual(getAllTestResult.ResultType, ResultTypes.Ok);
        }

        #endregion

        #region GetAsync Details By Status

        protected void ValidStatusAsInput()
        {
            var result = new BaseResult<WmsToEmsDto>
            {
                Payload = Generator.Default.Single<WmsToEmsDto>(),
                ResultType = ResultTypes.Ok
            };
            GetRestResponse1(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void GetByStatusInvoked()
        {
            getDetailsTestResult = _emsToWmsGateway.GetAsync(It.IsAny<RecordStatus>()).Result;
        }

        protected void TheGetByStatusOperationReturnedOkResponse()
        {
            Assert.IsNotNull(getDetailsTestResult);
            Assert.AreEqual(getDetailsTestResult.ResultType, ResultTypes.Ok);
        }

        #endregion

        #region DeleteAsync

        protected void InputKeyForWhichRecordDoesNotExistToDelete()
        {
            var result = new BaseResult<bool> {Payload = true, ResultType = ResultTypes.NotFound};
            GetRestResponse1(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InputValidKeyForDelete()
        {
            var result = new BaseResult<bool> {Payload = false, ResultType = ResultTypes.Ok};
            GetRestResponse1(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void DeleteInvoked()
        {
            manipulationTestResult = _emsToWmsGateway.DeleteAsync(It.IsAny<string>(), It.IsAny<long>()).Result;
        }

        protected void TheDeleteOperationReturnedOkResponse()
        {
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(manipulationTestResult.ResultType, ResultTypes.Ok);
        }

        protected void TheDeleteOperationReturnedNotFoundResponse()
        {
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(manipulationTestResult.ResultType, ResultTypes.NotFound);
        }

        #endregion

        #region InsertAsync

        protected void InputDataWithWhichRecordAlreadyExist()
        {
            var result = new BaseResult { ResultType = ResultTypes.Conflict};
            GetRestResponse1(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InputValidDataForInsertion()
        {
            var result = new BaseResult { ResultType = ResultTypes.Created};
            GetRestResponse1(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InsertInvoked()
        {
            manipulationTestResult = _emsToWmsGateway.InsertAsync(It.IsAny<WmsToEmsDto>()).Result;
        }

        protected void TheInsertOperationReturnedCreatedResponse()
        {
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(manipulationTestResult.ResultType, ResultTypes.Created);
        }

        protected void TheInsertOperationReturnedConflictResponse()
        {
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(manipulationTestResult.ResultType, ResultTypes.Conflict);
        }

        #endregion

        #region UpdateAsync

        protected void InputDataForWhichRecordDoesNotExistToUpdate()
        {
            var result = new BaseResult { ResultType = ResultTypes.NotFound};
            GetRestResponse1(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InputValidDataForUpdate()
        {
            var result = new BaseResult { ResultType = ResultTypes.Ok};
            GetRestResponse1(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void UpdateInvoked()
        {
            manipulationTestResult = _emsToWmsGateway.UpdateAsync(It.IsAny<WmsToEmsDto>()).Result;
        }

        protected void TheUpdateOperationReturnedOkResponse()
        {
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(manipulationTestResult.ResultType, ResultTypes.Ok);
        }

        protected void TheUpdateOperationReturnedNotFoundResponse()
        {
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(manipulationTestResult.ResultType, ResultTypes.NotFound);
        }

        #endregion

        #region GetDetails

        protected void InputKeyForWhichRecordDoesNotExist()
        {
            var result = new BaseResult<WmsToEmsDto>
                {Payload = Generator.Default.Single<WmsToEmsDto>(), ResultType = ResultTypes.NotFound};
            GetRestResponse1(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void ValidKeyAsInput()
        {
            var result = new BaseResult<WmsToEmsDto>
                {Payload = Generator.Default.Single<WmsToEmsDto>(), ResultType = ResultTypes.Ok};
            GetRestResponse1(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void GetDetailsInvoked()
        {
            getDetailsTestResult = _emsToWmsGateway.GetAsync(It.IsAny<string>(), It.IsAny<long>()).Result;
        }

        protected void TheGetDetailsOperationReturnedOkResponse()
        {
            Assert.IsNotNull(getDetailsTestResult);
            Assert.AreEqual(getDetailsTestResult.ResultType, ResultTypes.Ok);
        }

        protected void TheGetDetailsOperationReturnedNotFoundResponse()
        {
            Assert.IsNotNull(getDetailsTestResult);
            Assert.AreEqual(getDetailsTestResult.ResultType, ResultTypes.NotFound);
        }

        #endregion
    }
}