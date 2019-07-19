using AutoMapper;
using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.Wms.Asrs.Api.Services;
using Sfc.Wms.Asrs.Shamrock.Contracts.EnumsAndConstants.Constants;
using Sfc.Wms.Asrs.Shamrock.Repository.Entities;
using Sfc.Wms.Asrs.Shamrock.Repository.Interfaces;
using Sfc.Wms.Result;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Sfc.Wms.Asrs.Test.Unit.Fixtures
{
    public class SwmMessageSourceServiceFixture
    {
        private readonly Mock<ISwmMessageSourceGateway<SwmMessageSource>> _shamrockGateway;
        private readonly SwmMessageSourceService<SwmMessageSourceDto, SwmMessageSource> _shamrockService;
        private BaseResult<IEnumerable<SwmMessageSourceDto>> getAllActualResult;
        private BaseResult<SwmMessageSourceDto> getDetailsActualResult;
        private bool isValid;
        private BaseResult manipulationOperationsActualResult;

        private SwmMessageSourceDto request;

        public SwmMessageSourceServiceFixture()
        {
            _shamrockGateway = new Mock<ISwmMessageSourceGateway<SwmMessageSource>>(MockBehavior.Default);
            var mapper = new Mock<IMapper>(MockBehavior.Default);
            _shamrockService = new SwmMessageSourceService<SwmMessageSourceDto, SwmMessageSource>(mapper.Object, _shamrockGateway.Object);
        }

        #region Get All

        protected void GetAllSwmMessageSourceOperationIsInvoked()
        {
            var response = new BaseResult<IEnumerable<SwmMessageSource>>
            {
                Payload = Generator.Default.List<SwmMessageSource>(),
                ResultType = ResultTypes.Ok
            };

            _shamrockGateway.Setup(el => el.GetAsync()).Returns(Task.FromResult(response));
            getAllActualResult = _shamrockService.GetAsync().Result;
        }

        protected void TheGetAllSwmMessageSourceReturnedOkResponse()
        {
            Assert.IsNotNull(getAllActualResult);
            Assert.AreEqual(getAllActualResult.ResultType, ResultTypes.Ok);
            Assert.IsNotNull(getAllActualResult.Payload);
        }

        #endregion Get All

        #region Get Details

        protected void QueryParametersForWhichRecordDoesNotExists()
        {
            isValid = false;
        }

        protected void GetSwmMessageSourceDetailsByKeyIsInvoked()
        {
            var response = new BaseResult<SwmMessageSource>();
            if (isValid)
            {
                response.ResultType = ResultTypes.Ok;
                response.Payload = Generator.Default.Single<SwmMessageSource>();
            }
            else
            {
                response.ResultType = ResultTypes.NotFound;
               
            }

            _shamrockGateway.Setup(el => el.GetAsync(It.IsAny<Expression<Func<SwmMessageSource, bool>>>()))
                .Returns(Task.FromResult(response));

            getDetailsActualResult = _shamrockService.GetAsync(It.IsAny<Expression<Func<SwmMessageSource, bool>>>()).Result;
        }

        protected void TheGetSwmMessageSourceDetailsByKeyReturnedNotFound()
        {
            Assert.IsNotNull(getDetailsActualResult);
            Assert.AreEqual(getDetailsActualResult.ResultType, ResultTypes.NotFound);
            Assert.IsNull(getDetailsActualResult.Payload);
        }

        protected void QueryParametersForWhichRecordExists()
        {
            isValid = true;
        }

        protected void TheGetSwmMessageSourceDetailsByKeyReturnedOkStatus()
        {
            Assert.IsNotNull(getDetailsActualResult);
            Assert.AreEqual(getDetailsActualResult.ResultType, ResultTypes.Ok);
        }

        #endregion Get Details

        #region Insert

        protected void InputRequestDataWhichConfilctsWithExistingRecords()
        {
            isValid = false;
            request = Generator.Default.Single<SwmMessageSourceDto>();
        }

        protected void ValidRecordIsPassedToAddService()
        {
            isValid = true;
            request = Generator.Default.Single<SwmMessageSourceDto>();
        }

        protected void InsertSwmMessageSourceOperationIsInvoked()
        {
            var response = new BaseResult();
            if (isValid)
                response.ResultType = ResultTypes.Created;
            else
                response.ResultType = ResultTypes.Conflict;

            _shamrockGateway.Setup(el => el.InsertAsync(It.IsAny<SwmMessageSource>(),
                It.IsAny<Expression<Func<SwmMessageSource, bool>>>())).Returns(Task.FromResult(response));
            manipulationOperationsActualResult = _shamrockService.InsertAsync(request,
                It.IsAny<Expression<Func<SwmMessageSource, bool>>>()).Result;
        }

        protected void TheInsertSwmMessageSourceOperationReturnedConflictStatus()
        {
            Assert.IsNotNull(manipulationOperationsActualResult);
            Assert.AreEqual(manipulationOperationsActualResult.ResultType, ResultTypes.Conflict);
        }

        protected void TheInsertSwmMessageSourceOperationReturnedCreatedStatusAsResponse()
        {
            Assert.IsNotNull(manipulationOperationsActualResult);
            Assert.AreEqual(manipulationOperationsActualResult.ResultType, ResultTypes.Created);
        }

        #endregion Insert

        #region Update

        protected void UpdateForWhichNoRecordExists()
        {
            isValid = false;
            request = Generator.Default.Single<SwmMessageSourceDto>();
        }

        protected void ValidRecordIsPassedToUpdateService()
        {
            isValid = true;
            request = Generator.Default.Single<SwmMessageSourceDto>();
        }

        protected void UpdateSwmMessageSourceOperationIsInvoked()
        {
            var response = new BaseResult();
            if (isValid)
                response.ResultType = ResultTypes.Ok;
            else
                response.ResultType = ResultTypes.NotFound;

            _shamrockGateway.Setup(el => el.UpdateAsync(It.IsAny<SwmMessageSource>(),
                It.IsAny<Expression<Func<SwmMessageSource, bool>>>())).Returns(Task.FromResult(response));
            manipulationOperationsActualResult = _shamrockService.UpdateAsync(request,
                It.IsAny<Expression<Func<SwmMessageSource, bool>>>()).Result;
        }

        protected void TheUpdateSwmMessageSourceOperationReturnedNotFoundStatus()
        {
            Assert.IsNotNull(manipulationOperationsActualResult);
            Assert.AreEqual(manipulationOperationsActualResult.ResultType, ResultTypes.NotFound);
        }

        protected void TheUpdateSwmMessageSourceOperationReturnedOkStatusAsResponse()
        {
            Assert.IsNotNull(manipulationOperationsActualResult);
            Assert.AreEqual(manipulationOperationsActualResult.ResultType, ResultTypes.Ok);
        }

        #endregion Update

        #region Delete

        protected void DeleteForWhichNoRecordExists()
        {
            isValid = false;
        }

        protected void ValidKeyIsPassedToDeleteService()
        {
            isValid = true;
        }

        protected void DeleteOperationIsInvoked()
        {
            var response = new BaseResult();
            if (isValid)
                response.ResultType = ResultTypes.Ok;
            else
                response.ResultType = ResultTypes.NotFound;

            _shamrockGateway.Setup(el => el.DeleteAsync(It.IsAny<Expression<Func<SwmMessageSource, bool>>>()))
                .Returns(Task.FromResult(response));
            manipulationOperationsActualResult = _shamrockService
                .DeleteAsync(It.IsAny<Expression<Func<SwmMessageSource, bool>>>()).Result;
        }

        protected void TheDeleteSwmMessageSourceServiceOperationReturnedNotFoundStatus()
        {
            Assert.IsNotNull(manipulationOperationsActualResult);
            Assert.AreEqual(manipulationOperationsActualResult.ResultType, ResultTypes.NotFound);
        }

        protected void TheDeleteSwmMessageSourceServiceOperationReturnedOkStatusAsResponse()
        {
            Assert.IsNotNull(manipulationOperationsActualResult);
            Assert.AreEqual(manipulationOperationsActualResult.ResultType, ResultTypes.Ok);
        }

        #endregion Delete
    }
}