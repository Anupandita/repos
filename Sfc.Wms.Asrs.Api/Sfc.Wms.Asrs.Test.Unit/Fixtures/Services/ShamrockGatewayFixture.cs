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
    public abstract class ShamrockGatewayFixture
    {
        private readonly ShamrockGateway<SwmFromMhe> _shamrockGateway;
        private readonly Mock<ShamrockUnitOfWork<SwmFromMhe>> _shamrockUnitOfWork;

        private bool _emptyOrInvalidRequest;
        private BaseResult<SwmFromMhe> _getDetailsTestResult;
        private BaseResult _manipulationTestResult;

        protected ShamrockGatewayFixture()
        {
            var mapper = new Mock<IMapper>(MockBehavior.Default);
            var shamrockContext = new Mock<ShamrockContext>(ConfigurationManager.ConnectionStrings["ShamrockDbContext"].ConnectionString);
            _shamrockUnitOfWork = new Mock<ShamrockUnitOfWork<SwmFromMhe>>(MockBehavior.Default, shamrockContext.Object);
            _shamrockGateway = new ShamrockGateway<SwmFromMhe>(mapper.Object, _shamrockUnitOfWork.Object);
        }


        #region GetAsync Details 

        protected void QueryParametersForWhichRecordDoesNotExists()
        {
            _emptyOrInvalidRequest = true;
        }

        protected void ValidInputRequestKey()
        {
            _emptyOrInvalidRequest = false;
        }

        protected void GetDetailsByKeyGatewayInvoked()
        {
            var response = new BaseResult<SwmFromMhe>
            {
                ResultType = _emptyOrInvalidRequest ? ResultTypes.NotFound : ResultTypes.Ok,
                Payload = _emptyOrInvalidRequest ? null : Generator.Default.Single<SwmFromMhe>()
            };
            _shamrockUnitOfWork.Setup(el => el.GetAsync(It.IsAny<Expression<Func<SwmFromMhe, bool>>>()))
                .Returns(Task.FromResult(response));
            _getDetailsTestResult = _shamrockGateway.GetAsync(It.IsAny<Expression<Func<SwmFromMhe, bool>>>()).Result;
        }

        protected void TheReturnedResponseStatusIsOk()
        {
            Assert.IsNotNull(_getDetailsTestResult);
            Assert.AreEqual(_getDetailsTestResult.ResultType, ResultTypes.Ok);
            Assert.IsInstanceOfType(_getDetailsTestResult.Payload, typeof(SwmFromMhe));
        }

        protected void TheReturnedResponseStatusIsNotfound()
        {
            Assert.IsNotNull(_getDetailsTestResult);
            Assert.AreEqual(_getDetailsTestResult.ResultType, ResultTypes.NotFound);
            Assert.IsNull(_getDetailsTestResult.Payload);
        }

        #endregion

        #region InsertAsync Details 

        protected void InputRequestDataForWhichRecordAlreadyExists()
        {
            _emptyOrInvalidRequest = true;
        }

        protected void ValidInputRequestDataForInsert()
        {
            _emptyOrInvalidRequest = false;
        }

        protected void InsertGatewayInvoked()
        {
            var response = new BaseResult
            {
                ResultType = _emptyOrInvalidRequest ? ResultTypes.Conflict : ResultTypes.Created
            };
            _shamrockUnitOfWork.Setup(el => el.InsertAsync(It.IsAny<SwmFromMhe>(),
                It.IsAny<Expression<Func<SwmFromMhe, bool>>>())).Returns(Task.FromResult(response));
            _manipulationTestResult = _shamrockGateway.InsertAsync(It.IsAny<SwmFromMhe>(),
                It.IsAny<Expression<Func<SwmFromMhe, bool>>>()).Result;
        }

        protected void TheReturnedResponseStatusIsCreated()
        {
            Assert.IsNotNull(_manipulationTestResult);
            Assert.AreEqual(_manipulationTestResult.ResultType, ResultTypes.Created);
        }

        protected void TheReturnedResponseStatusIsConflicted()
        {
            Assert.IsNotNull(_manipulationTestResult);
            Assert.AreEqual(_manipulationTestResult.ResultType, ResultTypes.Conflict);
        }

        #endregion

        #region UpdateAsync Details 

        protected void InputRequestDataForWhichRecordDoesNotExists()
        {
            _emptyOrInvalidRequest = true;
        }

        protected void ValidInputRequestDataForUpdate()
        {
            _emptyOrInvalidRequest = false;
        }

        protected void UpdateGatewayInvoked()
        {
            var getResponse = new BaseResult<SwmFromMhe>
            {
                ResultType = _emptyOrInvalidRequest ? ResultTypes.NotFound : ResultTypes.Ok,
                Payload = _emptyOrInvalidRequest ? null : Generator.Default.Single<SwmFromMhe>()
            };
            var response = new BaseResult
            {
                ResultType = _emptyOrInvalidRequest ? ResultTypes.NotFound : ResultTypes.Ok
            };
            _shamrockUnitOfWork.Setup(el => el.GetAsync(It.IsAny<Expression<Func<SwmFromMhe, bool>>>()))
                .Returns(Task.FromResult(getResponse));

            _shamrockUnitOfWork.Setup(el => el.UpdateAsync(It.IsAny<SwmFromMhe>(),
                It.IsAny<Expression<Func<SwmFromMhe, bool>>>())).Returns(Task.FromResult(response));

            _manipulationTestResult = _shamrockGateway.UpdateAsync(It.IsAny<SwmFromMhe>(),
                It.IsAny<Expression<Func<SwmFromMhe, bool>>>()).Result;
        }

        protected void TheUpdateOperationReturnedOkResponse()
        {
            Assert.IsNotNull(_manipulationTestResult);
            Assert.AreEqual(_manipulationTestResult.ResultType, ResultTypes.Ok);
        }

        protected void TheUpdateOperationReturnedNotFoundResponse()
        {
            Assert.IsNotNull(_manipulationTestResult);
            Assert.AreEqual(_manipulationTestResult.ResultType, ResultTypes.NotFound);
        }

        #endregion

        #region DeleteAsync

        protected void InputRequestKeyForWhichRecordDoesNotExists()
        {
            _emptyOrInvalidRequest = true;
        }

        protected void ValidInputRequestKeyForDelete()
        {
            _emptyOrInvalidRequest = false;
        }

        protected void DeleteByKeyGatewayInvoked()
        {
            var response = new BaseResult
            {
                ResultType = _emptyOrInvalidRequest ? ResultTypes.NotFound : ResultTypes.Ok
            };
            _shamrockUnitOfWork.Setup(el => el.DeleteAsync(It.IsAny<Expression<Func<SwmFromMhe, bool>>>()))
                .Returns(Task.FromResult(response));
            _manipulationTestResult = _shamrockGateway.DeleteAsync(It.IsAny<Expression<Func<SwmFromMhe, bool>>>()).Result;
        }

        protected void TheDeleteOperationReturnedOkResponse()
        {
            Assert.IsNotNull(_manipulationTestResult);
            Assert.AreEqual(_manipulationTestResult.ResultType, ResultTypes.Ok);
        }

        protected void TheDeleteOperationReturnedNotFoundResponse()
        {
            Assert.IsNotNull(_manipulationTestResult);
            Assert.AreEqual(_manipulationTestResult.ResultType, ResultTypes.NotFound);
        }

        #endregion
    }
}