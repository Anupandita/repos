using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using RestSharp;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.RestResponse;
using Sfc.Wms.App.Api.Nuget.Gateways;
using Sfc.Wms.App.Api.Nuget.Interfaces;
using Sfc.Wms.Foundation.Corba.Contracts.Dtos;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Sfc.Wms.App.Api.Tests.Unit.Fixtures
{
    public class CorbaGatewayFixture
    {
        private readonly CorbaGateway _corbaGateway;
        private readonly Mock<IRestCsharpClient> _restClient;
        private BaseResult singleCorbaResult;
        private BaseResult<CorbaResponseDto> batchCorbaResult;

        public CorbaGatewayFixture()
        {
            _restClient = new Mock<IRestCsharpClient>();
            _corbaGateway = new CorbaGateway(new ResponseBuilder(), _restClient.Object);

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

        protected void ValidInputForSingleCorba()
        {
            var result = new BaseResult { ResultType = ResultTypes.Ok };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InValidInputForSingleCorba()
        {
            var result = new BaseResult { ResultType = ResultTypes.ExpectationFailed };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void EmptyOrBadInputForSingleCorba()
        {
            var result = new BaseResult { ResultType = ResultTypes.BadRequest };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InvokeSingleCorba()
        {
            singleCorbaResult = _corbaGateway.ProcessSingleCorbaCall(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<CorbaDto>(), It.IsAny<string>()).Result;
        }

        protected void SingleCorbaInvocationReturnedOkAsResponse()
        {
            ValidateSingleCorbaCall(ResultTypes.Ok);
        }

        protected void SingleCorbaInvocationReturnedExceptionAsResponse()
        {
            ValidateSingleCorbaCall(ResultTypes.ExpectationFailed);
        }

        protected void SingleCorbaInvocationReturnedBadRequestAsResponse()
        {
            ValidateSingleCorbaCall(ResultTypes.BadRequest);
        }

        private void ValidateSingleCorbaCall(ResultTypes expectedResultTypes)
        {
            VerifyRestClientInvocation<BaseResult>();
            Assert.IsNotNull(singleCorbaResult);
            Assert.AreEqual(expectedResultTypes, singleCorbaResult.ResultType);
        }

        protected void ValidInputForBatchCorba()
        {
            var result = new BaseResult<CorbaResponseDto> { ResultType = ResultTypes.Ok, Payload = Generator.Default.Single<CorbaResponseDto>() };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InValidInputForBatchCorba()
        {
            var result = new BaseResult<CorbaResponseDto> { ResultType = ResultTypes.ExpectationFailed };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void EmptyOrBadInputForBatchCorba()
        {
            var result = new BaseResult<CorbaResponseDto> { ResultType = ResultTypes.BadRequest };
            GetRestResponse(result, HttpStatusCode.OK, ResponseStatus.Completed);
        }

        protected void InvokeBatchCorba()
        {
            batchCorbaResult = _corbaGateway.ProcessBatchCorbaCall(It.IsAny<string>(),
                It.IsAny<string>(), It.IsAny<List<CorbaDto>>(), It.IsAny<string>()).Result;
        }

        protected void BatchCorbaInvocationReturnedOkAsResponse()
        {
            ValidateBatchCorbaCall(ResultTypes.Ok);
        }

        protected void BatchCorbaInvocationReturnedExceptionAsResponse()
        {
            ValidateBatchCorbaCall(ResultTypes.ExpectationFailed);
        }

        protected void BatchCorbaInvocationReturnedBadRequestAsResponse()
        {
            ValidateBatchCorbaCall(ResultTypes.BadRequest);
        }

        private void ValidateBatchCorbaCall(ResultTypes expectedResultTypes)
        {
            VerifyRestClientInvocation<BaseResult<CorbaResponseDto>>();
            Assert.IsNotNull(batchCorbaResult);
            Assert.AreEqual(expectedResultTypes, batchCorbaResult.ResultType);
        }
    }
}
