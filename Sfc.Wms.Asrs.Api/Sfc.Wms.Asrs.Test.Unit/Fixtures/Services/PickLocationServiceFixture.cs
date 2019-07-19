using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Http.Results;
using AutoMapper;
using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.Wms.Asrs.App.Services;
using Sfc.Wms.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Asrs.Shamrock.Contracts.EnumsAndConstants.Constants;
using Sfc.Wms.Asrs.Shamrock.Repository.Entities;
using Sfc.Wms.Asrs.Shamrock.Repository.Interfaces;
using Sfc.Wms.DematicMessage.Contracts.Dto;
using Sfc.Wms.Result;

namespace Sfc.Wms.Asrs.Test.Unit.Fixtures
{
    public class PickLocationServiceFixture
    {
        private readonly Mock<IShamrockGateway<PickLocationDtl>> _pickLocationGateway;
        private readonly PickLocationService _pickLocationService;
        private dynamic _testResult;

        public PickLocationServiceFixture()
        {
            _pickLocationGateway = new Mock<IShamrockGateway<PickLocationDtl>>(MockBehavior.Default);
            var mapper = new Mock<IMapper>(MockBehavior.Default);
            _pickLocationService = new PickLocationService(mapper.Object, _pickLocationGateway.Object);
        }

        private void SetupPickLocationGateway(bool valid)
        {
            var updateResponse = new BaseResult()
            {
                ResultType = valid? ResultTypes.Ok:ResultTypes.BadRequest,
            };

            var getResponse = new BaseResult<PickLocationDtl>()
            {
                ResultType = valid ? ResultTypes.Ok : ResultTypes.BadRequest,
                Payload =valid? Generator.Default.Single<PickLocationDtl>() : null
            };
            _pickLocationGateway.Setup(el => el.GetAsync(It.IsAny<Expression<Func<PickLocationDtl, bool>>>()))
                .Returns(Task.FromResult(getResponse));
            _pickLocationGateway.Setup(el => el.SaveChangesAsync())
                .Returns(Task.FromResult(updateResponse));

        }
        #region UpdateAsync

        protected void ValidData()
        {
            SetupPickLocationGateway(true);


        }

        protected void InvalidData()
        {
            SetupPickLocationGateway(false);
        }

        protected void UpdatePickLocationDetailInvoked()
        {    
            _testResult = _pickLocationService.UpdateQuantityAsync(Generator.Default.Single<CostDto>());
        }

        protected void PickLocationDetailShouldBeUpdated()
        {
            var result = _testResult.Result as BaseResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ResultType, ResultTypes.Ok);
        }

        protected void PickLocationDetailShouldNotBeUpdated()
        {
            var result = _testResult.Result as BaseResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ResultType, ResultTypes.BadRequest);
        }
        #endregion


    }
}