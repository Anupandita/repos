using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.Wms.Asrs.App.Services;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Dematic.Contracts.EnumsAndConstants.Constants;
using Sfc.Wms.Asrs.Dematic.Repository.Dtos;
using Sfc.Wms.Asrs.Dematic.Repository.Interfaces;
using Sfc.Wms.Result;

namespace Sfc.Wms.Asrs.Test.Unit.Fixtures
{
    public class DematicServiceFixture
    {
        private readonly Mock<IDematicGateway<EmsToWms>> _dematicGateway;
        private readonly DematicService<EmsToWmsDto, EmsToWms> _dematicService;
        private BaseResult<IEnumerable<EmsToWmsDto>> _getAllActualResult;
        private BaseResult<EmsToWmsDto> _getDetailsActualResult;
        private bool _isValid;
        private BaseResult _manipulationOperationsActualResult;

        private EmsToWmsDto _request;

        public DematicServiceFixture()
        {
            _dematicGateway = new Mock<IDematicGateway<EmsToWms>>(MockBehavior.Default);
            var mapper = new Mock<IMapper>(MockBehavior.Default);
            _dematicService = new DematicService<EmsToWmsDto, EmsToWms>(mapper.Object, _dematicGateway.Object);
        }

        #region GetAsync All

        protected void GetAllOperationIsInvoked()
        {
            var response = new BaseResult<IEnumerable<EmsToWms>>
            {
                Payload = Generator.Default.List<EmsToWms>(),
                ResultType = ResultTypes.Ok
            };

            _dematicGateway.Setup(el => el.GetAllAsync()).Returns(Task.FromResult(response));
            _getAllActualResult = _dematicService.GetAsync().Result;
        }

        protected void TheGetAllReturnedOkResponse()
        {
            Assert.IsNotNull(_getAllActualResult);
            Assert.AreEqual(_getAllActualResult.ResultType, ResultTypes.Ok);
            Assert.IsNotNull(_getAllActualResult.Payload);
        }

        #endregion

        #region GetAsync Details 

        protected void QueryParametersForWhichRecordDoesNotExists()
        {
            _isValid = false;
        }

        protected void GetDetailsByKeyIsInvoked()
        {
            var response = new BaseResult<EmsToWms>();
            if (_isValid)
            {
                response.ResultType = ResultTypes.Ok;
                response.Payload = Generator.Default.Single<EmsToWms>();
            }
            else
            {
                response.ResultType = ResultTypes.NotFound;
                response.Payload = null;
                response.ValidationMessages = new List<ValidationMessage>
                {
                    new ValidationMessage {Message = CustomMessages.NotFound, FieldName = "GetAsync"}
                };
            }

            _dematicGateway.Setup(el => el.GetAsync(It.IsAny<Expression<Func<EmsToWms, bool>>>()))
                .Returns(Task.FromResult(response));

            _getDetailsActualResult = _dematicService.GetAsync(It.IsAny<Expression<Func<EmsToWms, bool>>>()).Result;
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

        #region InsertAsync

        protected void InputRequestDataWhichConfilctsWithExistingRecords()
        {
            _isValid = false;
            _request = Generator.Default.Single<EmsToWmsDto>();
        }

        protected void ValidRecordIsPassedToAddService()
        {
            _isValid = true;
            _request = Generator.Default.Single<EmsToWmsDto>();
        }

        protected void AddOperationIsInvoked()
        {
            var response = new BaseResult();
            if (_isValid)
                response.ResultType = ResultTypes.Created;
            else
                response.ResultType = ResultTypes.Conflict;

            _dematicGateway.Setup(el => el.InsertAsync(It.IsAny<EmsToWms>(),
                It.IsAny<Expression<Func<EmsToWms, bool>>>())).Returns(Task.FromResult(response));
            _manipulationOperationsActualResult = _dematicService.InsertAsync(_request,
                It.IsAny<Expression<Func<EmsToWms, bool>>>()).Result;
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

        #region UpdateAsync

        protected void UpdateForWhichNoRecordExists()
        {
            _isValid = false;
            _request = Generator.Default.Single<EmsToWmsDto>();
        }

        protected void ValidRecordIsPassedToUpdateService()
        {
            _isValid = true;
            _request = Generator.Default.Single<EmsToWmsDto>();
        }

        protected void UpdateOperationIsInvoked()
        {
            var response = new BaseResult();
            if (_isValid)
                response.ResultType = ResultTypes.Ok;
            else
                response.ResultType = ResultTypes.NotFound;

            _dematicGateway.Setup(el => el.UpdateAsync(It.IsAny<EmsToWms>(),
                It.IsAny<Expression<Func<EmsToWms, bool>>>())).Returns(Task.FromResult(response));
            _manipulationOperationsActualResult = _dematicService.UpdateAsync(_request,
                It.IsAny<Expression<Func<EmsToWms, bool>>>()).Result;
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

        #region DeleteAsync

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

            _dematicGateway.Setup(el => el.DeleteAsync(It.IsAny<Expression<Func<EmsToWms, bool>>>()))
                .Returns(Task.FromResult(response));
            _manipulationOperationsActualResult = _dematicService
                .DeleteAsync(It.IsAny<Expression<Func<EmsToWms, bool>>>()).Result;
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