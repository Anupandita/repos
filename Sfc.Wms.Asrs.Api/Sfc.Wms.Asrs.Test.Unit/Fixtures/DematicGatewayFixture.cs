using AutoMapper;
using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.Wms.Asrs.Dematic.Repository.Context;
using Sfc.Wms.Asrs.Dematic.Repository.Dtos;
using Sfc.Wms.Asrs.Dematic.Repository.Gateways;
using Sfc.Wms.Result;
using System;
using System.Configuration;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sfc.Wms.Asrs.Test.Unit.Fixtures
{
    public abstract class DematicGatewayFixture
    {
        private readonly DematicGateway<EmsToWms> _dematicGateway;
        private readonly Mock<DematicUnitOfWork<EmsToWms>> _dematicUnitOfWork;

        private bool _emptyOrInvalidRequest;
        private BaseResult<EmsToWms> _getDetailsTestResult;
        private BaseResult _manipulationTestResult;

        public DematicGatewayFixture()
        {
            var mapper = new Mock<IMapper>(MockBehavior.Default);
            var dematicContext = new Mock<DematicContext>(ConfigurationManager.ConnectionStrings["DematicDbContext"].ConnectionString);
            _dematicUnitOfWork = new Mock<DematicUnitOfWork<EmsToWms>>(MockBehavior.Default, dematicContext.Object);
            _dematicGateway = new DematicGateway<EmsToWms>(mapper.Object, _dematicUnitOfWork.Object);
        }


        #region GetAsync Details 

        protected void QueryParamertesForWhichRecordDoesNotExists()
        {
            _emptyOrInvalidRequest = true;
        }

        protected void ValidInputRequestKey()
        {
            _emptyOrInvalidRequest = false;
        }

        protected void GetDetailsByKeyGatewayInvoked()
        {
            var response = new BaseResult<EmsToWms>
            {
                ResultType = _emptyOrInvalidRequest ? ResultTypes.NotFound : ResultTypes.Ok,
                Payload = _emptyOrInvalidRequest ? null : Generator.Default.Single<EmsToWms>()
            };
            _dematicUnitOfWork.Setup(el => el.Get(It.IsAny<Expression<Func<EmsToWms, bool>>>()))
                .Returns(Task.FromResult(response));
            _getDetailsTestResult = _dematicGateway.GetAsync(It.IsAny<Expression<Func<EmsToWms, bool>>>()).Result;
        }

        protected void TheReturnedResponseStatusIsOk()
        {
            Assert.IsNotNull(_getDetailsTestResult);
            Assert.AreEqual(_getDetailsTestResult.ResultType, ResultTypes.Ok);
            Assert.IsInstanceOfType(_getDetailsTestResult.Payload, typeof(EmsToWms));
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
            _dematicUnitOfWork.Setup(el => el.Insert(It.IsAny<EmsToWms>(),
                It.IsAny<Expression<Func<EmsToWms, bool>>>())).Returns(Task.FromResult(response));
            _manipulationTestResult = _dematicGateway.InsertAsync(It.IsAny<EmsToWms>(),
                It.IsAny<Expression<Func<EmsToWms, bool>>>()).Result;
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
            var getResponse = new BaseResult<EmsToWms>
            {
                ResultType = _emptyOrInvalidRequest ? ResultTypes.NotFound : ResultTypes.Ok,
                Payload = _emptyOrInvalidRequest ? null : Generator.Default.Single<EmsToWms>()
            };
            var response = new BaseResult
            {
                ResultType = _emptyOrInvalidRequest ? ResultTypes.NotFound : ResultTypes.Ok
            };
            _dematicUnitOfWork.Setup(el => el.Get(It.IsAny<Expression<Func<EmsToWms, bool>>>()))
                .Returns(Task.FromResult(getResponse));

            _dematicUnitOfWork.Setup(el => el.Update(It.IsAny<EmsToWms>(),
                It.IsAny<Expression<Func<EmsToWms, bool>>>())).Returns(Task.FromResult(response));

            _manipulationTestResult = _dematicGateway.UpdateAsync(It.IsAny<EmsToWms>(),
                It.IsAny<Expression<Func<EmsToWms, bool>>>()).Result;
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
            _dematicUnitOfWork.Setup(el => el.Delete(It.IsAny<Expression<Func<EmsToWms, bool>>>()))
                .Returns(Task.FromResult(response));
            _manipulationTestResult = _dematicGateway.DeleteAsync(It.IsAny<Expression<Func<EmsToWms, bool>>>()).Result;
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