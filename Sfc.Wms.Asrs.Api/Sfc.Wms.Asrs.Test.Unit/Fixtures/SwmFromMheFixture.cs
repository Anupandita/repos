using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.Wms.Asrs.Api.Controllers.Shamrock;
using Sfc.Wms.Asrs.App.Interfaces;
using Sfc.Wms.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Asrs.Shamrock.Contracts.EnumsAndConstants.Enums;
using Sfc.Wms.Asrs.Shamrock.Repository.Entities;
using Sfc.Wms.Result;

namespace Sfc.Wms.Asrs.Test.Unit.Fixtures
{
    public abstract class SwmFromMheFixture
    {
        private readonly SwmFromMheController _emsToWmsController;
        private readonly Mock<IShamrockService<SwmFromMheDto, SwmFromMhe>> _emsToWmsService;

        private SwmFromMheDto _request;
        private Task<IHttpActionResult> _testResult;

        protected SwmFromMheFixture()
        {
            _emsToWmsService = new Mock<IShamrockService<SwmFromMheDto, SwmFromMhe>>(MockBehavior.Default);
            _emsToWmsController = new SwmFromMheController(_emsToWmsService.Object);
        }

        protected bool IsValidData { get; set; } = false;

        protected void QueryParametersForWhichRecordDoesNotExists()
        {
            _request = Generator.Default.Single<SwmFromMheDto>();
        }

        protected void EmptyQueryParameters()
        {
            _request = null;
        }

        protected void ValidInputDataInRequest()
        {
            _request = Generator.Default.Single<SwmFromMheDto>();
        }

        protected void TheReturnedResponseStatusOfGetAllIsOk()
        {
            Assert.IsNotNull(_testResult);
            var result = _testResult.Result as OkNegotiatedContentResult<BaseResult<IEnumerable<SwmFromMheDto>>>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.Ok);
        }

        protected void TheGetByStatusOperationReturnedOkResponseStatus()
        {
            Assert.IsNotNull(_testResult);
            var result = _testResult.Result as OkNegotiatedContentResult<BaseResult<SwmFromMheDto>>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.Ok);
        }

        protected void TheGetByKeyOperationReturnedOkResponseStatus()
        {
            Assert.IsNotNull(_testResult);
            var result = _testResult.Result as OkNegotiatedContentResult<BaseResult<SwmFromMheDto>>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.Ok);
        }

        protected void TheInsertOperationReturnedOkResponseStatus()
        {
            Assert.IsNotNull(_testResult);
            var request = new HttpRequestMessage();
            var result = _testResult.Result as CreatedAtRouteNegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.IsNotNull(request.Headers.Contains("Location"));
            Assert.AreEqual(result.Content.ResultType, ResultTypes.Created);
        }

        protected void TheUpdateOperationReturnedOkResponseStatus()
        {
            Assert.IsNotNull(_testResult);
            var result = _testResult.Result as OkNegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.Ok);
        }

        protected void TheDeleteOperationReturnedOkResponseStatus()
        {
            Assert.IsNotNull(_testResult);
            var result = _testResult.Result as OkNegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.Ok);
        }

        protected void TheGetOperationReturnedNotFoundStatusAsResponse()
        {
            var result = _testResult.Result as NegotiatedContentResult<BaseResult<SwmFromMheDto>>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.NotFound);
        }

        protected void TheInvokeReturnedNotFoundResponse()
        {
            var result = _testResult.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.NotFound);
        }

        protected void TheReturnedResponseStatusIsConflict()
        {
            var result = _testResult.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.Conflict);
        }

        protected void GetAllIsInvoked()
        {
            var result = new BaseResult<IEnumerable<SwmFromMheDto>>
            {
                Payload = Generator.Default.List<SwmFromMheDto>(10) as List<SwmFromMheDto>,
                ResultType = ResultTypes.Ok
            };
            _emsToWmsService.Setup(el => el.GetAsync()).Returns(Task.FromResult(result));
            _testResult = _emsToWmsController.GetAsync();
        }

        protected void GetOperationIsInvoked()
        {
            var response = new BaseResult<SwmFromMheDto>();
            if (IsValidData)
            {
                response.ResultType = ResultTypes.Ok;
                response.Payload = _request;
            }
            else
            {
                response.ResultType = ResultTypes.NotFound;
            }

            _emsToWmsService.Setup(el => el.GetAsync(It.IsAny<Expression<Func<SwmFromMhe, bool>>>()))
                .Returns(Task.FromResult(response));
            _testResult = _emsToWmsController.GetAsync(_request.SourceMessageProcess, _request.SourceMessageKey);
        }

        protected void UpdateOperationIsInvoked()
        {
            var response = new BaseResult();
            if (IsValidData)
            {
                response.ResultType = ResultTypes.Ok;
            }
            else
            {
                response.ResultType = ResultTypes.NotFound;
            }

            _emsToWmsService.Setup(el => el.UpdateAsync(_request, It.IsAny<Expression<Func<SwmFromMhe, bool>>>()))
                .Returns(Task.FromResult(response));
            _testResult = _emsToWmsController.UpdateAsync(_request);
        }

        protected void DeleteOperationIsInvoked()
        {
            var response = new BaseResult();
            if (IsValidData)
            {
                response.ResultType = ResultTypes.Ok;
            }
            else
            {
                response.ResultType = ResultTypes.NotFound;
            }

            _emsToWmsService.Setup(el => el.DeleteAsync(It.IsAny<Expression<Func<SwmFromMhe, bool>>>()))
                .Returns(Task.FromResult(response));
            _testResult = _emsToWmsController.DeleteAsync(_request.SourceMessageProcess, _request.SourceMessageKey);
        }

        protected void InsertOperationIsInvoked()
        {
            var response = new BaseResult();
            if (IsValidData)
            {
                response.ResultType = ResultTypes.Created;
            }
            else
            {
                response.ResultType = ResultTypes.Conflict;
            }

            _emsToWmsService.Setup(el => el.InsertAsync(_request, It.IsAny<Expression<Func<SwmFromMhe, bool>>>()))
                .Returns(Task.FromResult(response));
            _testResult = _emsToWmsController.InsertAsync(_request);
        }

        protected void GetByStatusOperationIsInvoked()
        {
            var status = It.IsAny<RecordStatus>();
            var response = new BaseResult<SwmFromMheDto>();
            if (IsValidData)
            {
                response.ResultType = ResultTypes.Ok;
                response.Payload = Generator.Default.Single<SwmFromMheDto>();
            }
            else
            {
                response.ResultType = ResultTypes.NotFound;
            }

            _emsToWmsService.Setup(el => el.GetAsync(It.IsAny<Expression<Func<SwmFromMhe, bool>>>()))
                .Returns(Task.FromResult(response));
            _testResult = _emsToWmsController.GetAsync(status);
        }
    }
}