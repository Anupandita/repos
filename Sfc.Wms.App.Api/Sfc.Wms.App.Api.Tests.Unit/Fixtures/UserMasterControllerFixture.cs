using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Controllers;
using Sfc.Wms.Configuration.UserMaster.Contracts.Dtos;
using Sfc.Wms.Configuration.UserMaster.Contracts.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Sfc.Wms.App.Api.Tests.Unit.Fixtures
{
    public abstract class UserMasterControllerFixture
    {
        private readonly UserMasterController _userMasterController;
        private readonly Mock<ISwmUserSettingService> _mockSwmUserSettingService;
        private Task<IHttpActionResult> _testResponse;
        private readonly List<SwmUserSettingDto> _swmUserSettingDtoCollection;
        private readonly SwmUserSettingDto _swmUserSettingDto;

        public UserMasterControllerFixture()
        {
            _mockSwmUserSettingService = new Mock<ISwmUserSettingService>(MockBehavior.Default);
            _swmUserSettingDtoCollection = new List<SwmUserSettingDto>() { 
                new SwmUserSettingDto() { Id = 0 }, 
                new SwmUserSettingDto() { Id = 1 } 
            };
            _swmUserSettingDto = Generator.Default.Single<SwmUserSettingDto>();
            _userMasterController = new UserMasterController(_mockSwmUserSettingService.Object);
        }

        #region Mock

        private void VerifyCreateUserPreferences()
        {
            _mockSwmUserSettingService.Verify(el => el.InsertAsync(It.IsAny<SwmUserSettingDto>()));
        }

        private void MockCreateUserPreferences(ResultTypes resultType)
        {
            var response = new BaseResult
            {
                ResultType = resultType
            };
            _mockSwmUserSettingService.Setup(el => el.InsertAsync(It.IsAny<SwmUserSettingDto>())).Returns(Task.FromResult(response));
        }

        private void VerifyUpdateUserPreferences()
        {
            _mockSwmUserSettingService.Verify(el => el.UpdateSpecificFieldsAsync(It.IsAny<SwmUserSettingDto>(), It.IsAny<string[]>()));
        }

        private void MockUpdateUserPreferences(ResultTypes resultType)
        {
            var response = new BaseResult
            {
                ResultType = resultType
            };
            _mockSwmUserSettingService.Setup(el => el.UpdateSpecificFieldsAsync(It.IsAny<SwmUserSettingDto>(), It.IsAny<string[]>())).Returns(Task.FromResult(response));
            _mockSwmUserSettingService.Setup(el => el.InsertAsync(It.IsAny<SwmUserSettingDto>())).Returns(Task.FromResult(response));
        }

        #endregion Mock

        #region UpdateUserPreferences

        protected void UpdateUserPreferencesExist()
        {
            MockUpdateUserPreferences(ResultTypes.Ok);
        }

        protected void UpdateUserPreferencesNotExist()
        {
            MockUpdateUserPreferences(ResultTypes.NotFound);
        }

        protected void UpdateUserPreferenceInvoked()
        {
            _testResponse = _userMasterController.UpdateUserPreferences(_swmUserSettingDtoCollection);
        }

        protected void UpdateUserPreferencesReturnedOkResponse()
        {
            VerifyUpdateUserPreferences();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as OkNegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.Ok, result.Content.ResultType);
        }

        protected void UpdateUserPreferencesReturnedNotFoundResponse()
        {
            VerifyCreateUserPreferences();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.NotFound, result.Content.ResultType);
        }

        #endregion UpdateUserPreferences
    }
}
