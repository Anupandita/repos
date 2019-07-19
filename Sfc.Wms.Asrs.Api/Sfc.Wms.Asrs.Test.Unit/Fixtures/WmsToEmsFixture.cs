using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.Wms.Asrs.Api.Controllers.Dematic;
using Sfc.Wms.Asrs.App.Interfaces;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Dematic.Contracts.EnumsAndConstants.Enums;
using Sfc.Wms.Asrs.Dematic.Repository.Dtos;
using Sfc.Wms.Result;

namespace Sfc.Wms.Asrs.Test.Unit.Fixtures
{
    public abstract class WmsToEmsFixture
    {
        private readonly WmsToEmsController _emsToWmsController;
        private readonly Mock<IDematicService<WmsToEmsDto, WmsToEms>> _emsToWmsService;

        private WmsToEmsDto _request;
        private Task<IHttpActionResult> _testResult;

        protected WmsToEmsFixture()
        {
            _emsToWmsService = new Mock<IDematicService<WmsToEmsDto, WmsToEms>>(MockBehavior.Default);
            _emsToWmsController = new WmsToEmsController(_emsToWmsService.Object);
        }

        protected bool IsValidData { get; set; } = false;

        protected void QueryParametersForWhichRecordDoesNotExists()
        {
            _request = Generator.Default.Single<WmsToEmsDto>();
        }

        protected void EmptyQueryParameters()
        {
            _request = null;
        }

        protected void ValidInputDataInRequest()
        {
            _request = Generator.Default.Single<WmsToEmsDto>();
        }

        protected void TheReturnedResponseStatusOfGetAllIsOk()
        {
            Assert.IsNotNull(_testResult);
            var result = _testResult.Result as OkNegotiatedContentResult<BaseResult<IEnumerable<WmsToEmsDto>>>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.Ok);
        }

        protected void TheGetByStatusOperationReturnedOkResponseStatus()
        {
            Assert.IsNotNull(_testResult);
            var result = _testResult.Result as OkNegotiatedContentResult<BaseResult<WmsToEmsDto>>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.Ok);
        }

        protected void TheGetByKeyOperationReturnedOkResponseStatus()
        {
            Assert.IsNotNull(_testResult);
            var result = _testResult.Result as OkNegotiatedContentResult<BaseResult<WmsToEmsDto>>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.Ok);
        }

        protected void TheInsertOperationReturnedOkResponseStatus()
        {
            Assert.IsNotNull(_testResult);
            var result = _testResult.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
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
            var result = _testResult.Result as NegotiatedContentResult<BaseResult<WmsToEmsDto>>;
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
            var result = new BaseResult<IEnumerable<WmsToEmsDto>>
            {
                Payload = Generator.Default.List<WmsToEmsDto>(10) as List<WmsToEmsDto>,
                ResultType = ResultTypes.Ok
            };
            _emsToWmsService.Setup(el => el.GetAsync()).Returns(Task.FromResult(result));
            _testResult = _emsToWmsController.GetAsync();
        }

        protected void GetOperationIsInvoked()
        {
            var response = new BaseResult<WmsToEmsDto>();
            if (IsValidData)
            {
                response.ResultType = ResultTypes.Ok;
                response.Payload = _request;
            }
            else
            {
                response.ResultType = ResultTypes.NotFound;
            }

            _emsToWmsService.Setup(el => el.GetAsync(It.IsAny<Expression<Func<WmsToEms, bool>>>()))
                .Returns(Task.FromResult(response));
            _testResult = _emsToWmsController.GetAsync(_request.Process, _request.MessageKey);
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

            _emsToWmsService.Setup(el => el.UpdateAsync(_request, It.IsAny<Expression<Func<WmsToEms, bool>>>()))
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

            _emsToWmsService.Setup(el => el.DeleteAsync(It.IsAny<Expression<Func<WmsToEms, bool>>>()))
                .Returns(Task.FromResult(response));
            _testResult = _emsToWmsController.DeleteAsync(_request.Process, _request.MessageKey);
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

            _emsToWmsService.Setup(el => el.InsertAsync(_request, It.IsAny<Expression<Func<WmsToEms, bool>>>()))
                .Returns(Task.FromResult(response));
            _testResult = _emsToWmsController.InsertAsync(_request);
        }

        protected void GetByStatusOperationIsInvoked()
        {
            var status = It.IsAny<RecordStatus>();
            var response = new BaseResult<WmsToEmsDto>();
            if (IsValidData)
            {
                response.ResultType = ResultTypes.Ok;
                response.Payload = Generator.Default.Single<WmsToEmsDto>();
            }
            else
            {
                response.ResultType = ResultTypes.NotFound;
            }

            _emsToWmsService.Setup(el => el.GetAsync(It.IsAny<Expression<Func<WmsToEms, bool>>>()))
                .Returns(Task.FromResult(response));
            _testResult = _emsToWmsController.GetAsync(status);
        }
    }
}