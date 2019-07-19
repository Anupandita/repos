using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.Wms.Asrs.App.Services;
using Sfc.Wms.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Asrs.Shamrock.Contracts.EnumsAndConstants.Constants;
using Sfc.Wms.Asrs.Shamrock.Repository.Entities;
using Sfc.Wms.Asrs.Shamrock.Repository.Interfaces;
using Sfc.Wms.Result;

namespace Sfc.Wms.Asrs.Test.Unit.Fixtures
{
    public class ShamrockServiceFixture
    {
        private readonly Mock<IShamrockGateway<SwmFromMhe>> _shamrockGateway;
        private readonly ShamrockService<SwmFromMheDto, SwmFromMhe> _shamrockService;
        private BaseResult<IEnumerable<SwmFromMheDto>> _getAllActualResult;
        private BaseResult<SwmFromMheDto> _getDetailsActualResult;
        private bool _isValid;
        private BaseResult _manipulationOperationsActualResult;

        private SwmFromMheDto _request;

        public ShamrockServiceFixture()
        {
            _shamrockGateway = new Mock<IShamrockGateway<SwmFromMhe>>(MockBehavior.Default);
            var mapper = new Mock<IMapper>(MockBehavior.Default);
            _shamrockService = new ShamrockService<SwmFromMheDto, SwmFromMhe>(mapper.Object, _shamrockGateway.Object);
        }

        #region Get All

        protected void GetAllOperationIsInvoked()
        {
            var response = new BaseResult<IEnumerable<SwmFromMhe>>
            {
                Payload = Generator.Default.List<SwmFromMhe>(),
                ResultType = ResultTypes.Ok
            };

            _shamrockGateway.Setup(el => el.GetAsync()).Returns(Task.FromResult(response));
            _getAllActualResult = _shamrockService.GetAsync().Result;
        }

        protected void TheGetAllReturnedOkResponse()
        {
            Assert.IsNotNull(_getAllActualResult);
            Assert.AreEqual(_getAllActualResult.ResultType, ResultTypes.Ok);
            Assert.IsNotNull(_getAllActualResult.Payload);
        }

        #endregion

        #region Get Details 

        protected void QueryParametersForWhichRecordDoesNotExists()
        {
            _isValid = false;
        }

        protected void GetDetailsByKeyIsInvoked()
        {
            var response = new BaseResult<SwmFromMhe>();
            if (_isValid)
            {
                response.ResultType = ResultTypes.Ok;
                response.Payload = Generator.Default.Single<SwmFromMhe>();
            }
            else
            {
                response.ResultType = ResultTypes.NotFound;
                response.ValidationMessages.Add(new ValidationMessage("Get", CustomMessages.NotFound));
            }

            _shamrockGateway.Setup(el => el.GetAsync(It.IsAny<Expression<Func<SwmFromMhe, bool>>>()))
                .Returns(Task.FromResult(response));

            _getDetailsActualResult = _shamrockService.GetAsync(It.IsAny<Expression<Func<SwmFromMhe, bool>>>()).Result;
        }

        protected void TheGetDetailsByKeyReturnedNotFound()
        {
            Assert.IsNotNull(_getDetailsActualResult);
            Assert.AreEqual(_getDetailsActualResult.ResultType, ResultTypes.NotFound);
            Assert.IsNull(_getDetailsActualResult.Payload);
        }

        protected void QueryParametersForWhichRecordExists()
        {
            _isValid = true;
        }

        protected void TheGetDetailsByKeyReturnedOkStatus()
        {
            Assert.IsNotNull(_getDetailsActualResult);
            Assert.AreEqual(_getDetailsActualResult.ResultType, ResultTypes.Ok);
        }

        #endregion

        #region Insert

        protected void InputRequestDataWhichConfilctsWithExistingRecords()
        {
            _isValid = false;
            _request = Generator.Default.Single<SwmFromMheDto>();
        }

        protected void ValidRecordIsPassedToAddService()
        {
            _isValid = true;
            _request = Generator.Default.Single<SwmFromMheDto>();
        }

        protected void AddOperationIsInvoked()
        {
            var response = new BaseResult();
            if (_isValid)
                response.ResultType = ResultTypes.Created;
            else
                response.ResultType = ResultTypes.Conflict;

            _shamrockGateway.Setup(el => el.InsertAsync(It.IsAny<SwmFromMhe>(),
                It.IsAny<Expression<Func<SwmFromMhe, bool>>>())).Returns(Task.FromResult(response));
            _manipulationOperationsActualResult = _shamrockService.InsertAsync(_request,
                It.IsAny<Expression<Func<SwmFromMhe, bool>>>()).Result;
        }

        protected void TheAddOperationReturnedConflictStatus()
        {
            Assert.IsNotNull(_manipulationOperationsActualResult);
            Assert.AreEqual(_manipulationOperationsActualResult.ResultType, ResultTypes.Conflict);
        }

        protected void TheAddOperationReturnedCreatedStatusAsResponse()
        {
            Assert.IsNotNull(_manipulationOperationsActualResult);
            Assert.AreEqual(_manipulationOperationsActualResult.ResultType, ResultTypes.Created);
        }

        #endregion

        #region Update

        protected void UpdateForWhichNoRecordExists()
        {
            _isValid = false;
            _request = Generator.Default.Single<SwmFromMheDto>();
        }

        protected void ValidRecordIsPassedToUpdateService()
        {
            _isValid = true;
            _request = Generator.Default.Single<SwmFromMheDto>();
        }

        protected void UpdateOperationIsInvoked()
        {
            var response = new BaseResult();
            if (_isValid)
                response.ResultType = ResultTypes.Ok;
            else
                response.ResultType = ResultTypes.NotFound;

            _shamrockGateway.Setup(el => el.UpdateAsync(It.IsAny<SwmFromMhe>(),
                It.IsAny<Expression<Func<SwmFromMhe, bool>>>())).Returns(Task.FromResult(response));
            _manipulationOperationsActualResult = _shamrockService.UpdateAsync(_request,
                It.IsAny<Expression<Func<SwmFromMhe, bool>>>()).Result;
        }

        protected void TheUpdateOperationReturnedNotFoundStatus()
        {
            Assert.IsNotNull(_manipulationOperationsActualResult);
            Assert.AreEqual(_manipulationOperationsActualResult.ResultType, ResultTypes.NotFound);
        }

        protected void TheUpdateOperationReturnedOkStatusAsResponse()
        {
            Assert.IsNotNull(_manipulationOperationsActualResult);
            Assert.AreEqual(_manipulationOperationsActualResult.ResultType, ResultTypes.Ok);
        }

        #endregion

        #region Delete

        protected void DeleteForWhichNoRecordExists()
        {
            _isValid = false;
        }

        protected void ValidKeyIsPassedToDeleteService()
        {
            _isValid = true;
        }

        protected void DeleteOperationIsInvoked()
        {
            var response = new BaseResult();
            if (_isValid)
                response.ResultType = ResultTypes.Ok;
            else
                response.ResultType = ResultTypes.NotFound;

            _shamrockGateway.Setup(el => el.DeleteAsync(It.IsAny<Expression<Func<SwmFromMhe, bool>>>()))
                .Returns(Task.FromResult(response));
            _manipulationOperationsActualResult = _shamrockService
                .DeleteAsync(It.IsAny<Expression<Func<SwmFromMhe, bool>>>()).Result;
        }

        protected void TheDeleteServiceOperationReturnedNotFoundStatus()
        {
            Assert.IsNotNull(_manipulationOperationsActualResult);
            Assert.AreEqual(_manipulationOperationsActualResult.ResultType, ResultTypes.NotFound);
        }

        protected void TheDeleteServiceOperationReturnedOkStatusAsResponse()
        {
            Assert.IsNotNull(_manipulationOperationsActualResult);
            Assert.AreEqual(_manipulationOperationsActualResult.ResultType, ResultTypes.Ok);
        }

        #endregion
    }
}