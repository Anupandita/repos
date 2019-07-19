using AutoMapper;
using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.Wms.Asrs.Shamrock.Repository.Context;
using Sfc.Wms.Asrs.Shamrock.Repository.Entities;
using Sfc.Wms.Asrs.Shamrock.Repository.Gateways;
using Sfc.Wms.Result;
using System;
using System.Configuration;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sfc.Wms.Asrs.Test.Unit.Fixtures
{
    public abstract class SwmMessageSourceGatewayFixture
    {
        private readonly SwmMessageSourceGateway<SwmMessageSource> _swmMessageSourceGateway;
        private readonly Mock<ShamrockUnitOfWork<SwmMessageSource>> _shamrockUnitOfWork;

        private bool emptyOrInvalidRequest;
        private BaseResult<SwmMessageSource> getDetailsTestResult;
        private BaseResult manipulationTestResult;

        protected SwmMessageSourceGatewayFixture()
        {
            var mapper = new Mock<IMapper>(MockBehavior.Default);
            var shamrockContext = new Mock<ShamrockContext>(ConfigurationManager.ConnectionStrings["ShamrockDbContext"].ConnectionString);
            _shamrockUnitOfWork = new Mock<ShamrockUnitOfWork<SwmMessageSource>>(MockBehavior.Default, shamrockContext.Object);
            _swmMessageSourceGateway = new SwmMessageSourceGateway<SwmMessageSource>(mapper.Object, _shamrockUnitOfWork.Object);
        }

        #region Get Details

        protected void QueryParametersForWhichRecordDoesNotExists()
        {
            emptyOrInvalidRequest = true;
        }

        protected void ValidInputRequestKey()
        {
            emptyOrInvalidRequest = false;
        }

        protected void GetSwmMessageSourceDetailsByKeyGatewayInvoked()
        {
            var response = new BaseResult<SwmMessageSource>
            {
                ResultType = emptyOrInvalidRequest ? ResultTypes.NotFound : ResultTypes.Ok,
                Payload = emptyOrInvalidRequest ? null : Generator.Default.Single<SwmMessageSource>()
            };
            _shamrockUnitOfWork.Setup(el => el.GetAsync(It.IsAny<Expression<Func<SwmMessageSource, bool>>>()))
                .Returns(Task.FromResult(response));
            getDetailsTestResult = _swmMessageSourceGateway.GetAsync(It.IsAny<Expression<Func<SwmMessageSource, bool>>>()).Result;
        }

        protected void TheInvokedGetAllSwmMessageSourceShouldReturnedWithAllRecords()
        {
            Assert.IsNotNull(getDetailsTestResult);
            Assert.AreEqual(getDetailsTestResult.ResultType, ResultTypes.Ok);
            Assert.IsInstanceOfType(getDetailsTestResult.Payload, typeof(SwmMessageSource));
        }

        protected void TheInvokedGetAllSwmMessageSourceShouldNotReturnAnyRecords()
        {
            Assert.IsNotNull(getDetailsTestResult);
            Assert.AreEqual(getDetailsTestResult.ResultType, ResultTypes.NotFound);
            Assert.IsNull(getDetailsTestResult.Payload);
        }

        #endregion Get Details

        #region Insert Details

        protected void InputRequestDataForWhichRecordAlreadyExists()
        {
            emptyOrInvalidRequest = true;
        }

        protected void ValidInputRequestDataForInsert()
        {
            emptyOrInvalidRequest = false;
        }

        protected void InsertSwmMessageSourceGatewayInvoked()
        {
            var response = new BaseResult
            {
                ResultType = emptyOrInvalidRequest ? ResultTypes.Conflict : ResultTypes.Created
            };
            var getNextValueResponse = new BaseResult<decimal>
            {
                ResultType = ResultTypes.Created,
                Payload = It.IsAny<int>(),
            };

            _shamrockUnitOfWork.Setup(el => el.GetNextValueAsync())
                .Returns(Task.FromResult(getNextValueResponse));
            _shamrockUnitOfWork.Setup(el => el.InsertAsync(It.IsAny<SwmMessageSource>(),
                It.IsAny<Expression<Func<SwmMessageSource, bool>>>())).Returns(Task.FromResult(response));
            manipulationTestResult = _swmMessageSourceGateway.InsertAsync(Generator.Default.Single<SwmMessageSource>(),
                It.IsAny<Expression<Func<SwmMessageSource, bool>>>()).Result;
        }

        protected void TheReturnedResponseStatusIsCreated()
        {
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(manipulationTestResult.ResultType, ResultTypes.Created);
        }

        protected void TheInvokedInsertSwmMessageSourceShouldReturnedWithConflictResponse()
        {
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(manipulationTestResult.ResultType, ResultTypes.Conflict);
        }

        #endregion Insert Details

        #region Update Details

        protected void InputRequestDataForWhichRecordDoesNotExists()
        {
            emptyOrInvalidRequest = true;
        }

        protected void ValidInputRequestDataForUpdate()
        {
            emptyOrInvalidRequest = false;
        }

        protected void UpdateSwmMessageSourceGatewayInvoked()
        {
            var getResponse = new BaseResult<SwmMessageSource>
            {
                ResultType = emptyOrInvalidRequest ? ResultTypes.NotFound : ResultTypes.Ok,
                Payload = emptyOrInvalidRequest ? null : Generator.Default.Single<SwmMessageSource>()
            };
            var response = new BaseResult
            {
                ResultType = emptyOrInvalidRequest ? ResultTypes.NotFound : ResultTypes.Ok
            };
            _shamrockUnitOfWork.Setup(el => el.GetAsync(It.IsAny<Expression<Func<SwmMessageSource, bool>>>()))
                .Returns(Task.FromResult(getResponse));

            _shamrockUnitOfWork.Setup(el => el.UpdateAsync(It.IsAny<SwmMessageSource>(),
                It.IsAny<Expression<Func<SwmMessageSource, bool>>>())).Returns(Task.FromResult(response));

            manipulationTestResult = _swmMessageSourceGateway.UpdateAsync(It.IsAny<SwmMessageSource>(),
                It.IsAny<Expression<Func<SwmMessageSource, bool>>>()).Result;
        }

        protected void TheUpdateSwmMessageSourceOperationReturnedOkResponse()
        {
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(manipulationTestResult.ResultType, ResultTypes.Ok);
        }

        protected void TheUpdateOperationReturnedNotFoundResponse()
        {
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(manipulationTestResult.ResultType, ResultTypes.NotFound);
        }

        #endregion Update Details

        #region Delete

        protected void InputRequestKeyForWhichRecordDoesNotExists()
        {
            emptyOrInvalidRequest = true;
        }

        protected void ValidInputRequestKeyForDelete()
        {
            emptyOrInvalidRequest = false;
        }

        protected void DeleteSwmMessageSourceByKeyGatewayInvoked()
        {
            var response = new BaseResult
            {
                ResultType = emptyOrInvalidRequest ? ResultTypes.NotFound : ResultTypes.Ok
            };
            _shamrockUnitOfWork.Setup(el => el.DeleteAsync(It.IsAny<Expression<Func<SwmMessageSource, bool>>>()))
                .Returns(Task.FromResult(response));
            manipulationTestResult = _swmMessageSourceGateway.DeleteAsync(It.IsAny<Expression<Func<SwmMessageSource, bool>>>()).Result;
        }

        protected void TheDeleteSwmMessageSourceOperationReturnedOkResponse()
        {
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(manipulationTestResult.ResultType, ResultTypes.Ok);
        }

        protected void TheDeleteOperationReturnedNotFoundResponse()
        {
            Assert.IsNotNull(manipulationTestResult);
            Assert.AreEqual(manipulationTestResult.ResultType, ResultTypes.NotFound);
        }

        #endregion Delete
    }
}