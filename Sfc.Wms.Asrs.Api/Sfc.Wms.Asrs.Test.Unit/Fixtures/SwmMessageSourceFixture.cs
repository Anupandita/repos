using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.Wms.Asrs.Api.Controllers.Shamrock;
using Sfc.Wms.Asrs.App.Interfaces;
using Sfc.Wms.Asrs.Shamrock.Repository.Entities;
using Sfc.Wms.Result;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Sfc.Wms.Asrs.Test.Unit.Fixtures
{
    public abstract class SwmMessageSourceFixture
    {
        private readonly SwmMessageSourceController _swmMessageSourceController;
        private readonly Mock<ISwmMessageSourceService<SwmMessageSourceDto, SwmMessageSource>> _swmMessageSourceService;

        private SwmMessageSourceDto request;
        private Task<IHttpActionResult> testResult;

        protected SwmMessageSourceFixture()
        {
            _swmMessageSourceService = new Mock<ISwmMessageSourceService<SwmMessageSourceDto, SwmMessageSource>>(MockBehavior.Default);
            _swmMessageSourceController = new SwmMessageSourceController(_swmMessageSourceService.Object);
        }

        protected bool IsValidData { get; set; } = false;

        protected void QueryParametersForWhichRecordDoesNotExists()
        {
            request = Generator.Default.Single<SwmMessageSourceDto>();
        }

        protected void EmptyQueryParameters()
        {
            request = null;
        }

        protected void ValidInputDataInRequest()
        {
            request = Generator.Default.Single<SwmMessageSourceDto>();
        }

        protected void TheInvokedGetAllSwmMessageSourceShouldReturnAllRecords()
        {
            Assert.IsNotNull(testResult);
            var result = testResult.Result as OkNegotiatedContentResult<BaseResult<IEnumerable<SwmMessageSourceDto>>>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.Ok);
        }



        protected void TheInvokedGetSwmMessageSourceByKeyOperationShouldReturnedWithOkResponse()
        {
            Assert.IsNotNull(testResult);
            var result = testResult.Result as OkNegotiatedContentResult<BaseResult<SwmMessageSourceDto>>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.Ok);
        }

        protected void TheInvokedInsertSwmMessageSourceOperationShouldReturnedWithOkResponse()
        {
            Assert.IsNotNull(testResult);

            var result = testResult.Result as CreatedAtRouteNegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.Created);
        }

        protected void TheInvokedUpdateSwmMessageSourceOperationShouldReturnedWithOkResponse()
        {
            Assert.IsNotNull(testResult);
            var result = testResult.Result as OkNegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.Ok);
        }

        protected void TheInvokedDeleteSwmMessageSourceOperationShouldReturnedWithOkResponse()
        {
            Assert.IsNotNull(testResult);
            var result = testResult.Result as OkNegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.Ok);
        }

        protected void TheInvokedGetSwmMessageSourceOperationShouldNotReturnAnyRecords()
        {
            var result = testResult.Result as NegotiatedContentResult<BaseResult<SwmMessageSourceDto>>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.NotFound);
        }

        protected void TheInvokedUpdateSwmMessageSourceShouldReturnedWithNotFoundResponse()
        {
            var result = testResult.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.NotFound);
        }

        protected void TheInvokedDeleteSwmMessageSourceShouldReturnedWithNotFoundResponse()
        {
            var result = testResult.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.NotFound);
        }
        protected void TheInvokedInsertOperationShouldReturnedWithConflictResponse()
        {
            var result = testResult.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(result.Content.ResultType, ResultTypes.Conflict);
        }

        protected void GetAllSwmMessageSourceIsInvoked()
        {
            var result = new BaseResult<IEnumerable<SwmMessageSourceDto>>
            {
                Payload = Generator.Default.List<SwmMessageSourceDto>(10) as List<SwmMessageSourceDto>,
                ResultType = ResultTypes.Ok
            };
            _swmMessageSourceService.Setup(el => el.GetAsync()).Returns(Task.FromResult(result));
            testResult = _swmMessageSourceController.GetAsync();
        }

        protected void GetSwmMessageSourceOperationIsInvoked()
        {
            var response = new BaseResult<SwmMessageSourceDto>();
            if (IsValidData)
            {
                response.ResultType = ResultTypes.Ok;
                response.Payload = request;
            }
            else
            {
                response.ResultType = ResultTypes.NotFound;
            }

            _swmMessageSourceService.Setup(el => el.GetAsync(It.IsAny<Expression<Func<SwmMessageSource, bool>>>()))
                .Returns(Task.FromResult(response));
            testResult = _swmMessageSourceController.GetAsync(request.SourceId);
        }

        protected void UpdateSwmMessageSourceOperationIsInvoked()
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

            _swmMessageSourceService.Setup(el => el.UpdateAsync(request, It.IsAny<Expression<Func<SwmMessageSource, bool>>>()))
                .Returns(Task.FromResult(response));
            testResult = _swmMessageSourceController.UpdateAsync(request);
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

            _swmMessageSourceService.Setup(el => el.DeleteAsync(It.IsAny<Expression<Func<SwmMessageSource, bool>>>()))
                .Returns(Task.FromResult(response));
            testResult = _swmMessageSourceController.DeleteAsync(request.SourceId);
        }

        protected void InsertSwmMessageSourceOperationIsInvoked()
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

            _swmMessageSourceService.Setup(el => el.InsertAsync(request, It.IsAny<Expression<Func<SwmMessageSource, bool>>>()))
                .Returns(Task.FromResult(response));
            testResult = _swmMessageSourceController.InsertAsync(request);
        }
    }
}