using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.Core.OnPrem.Result;
using Sfc.Core.OnPrem.Security.Contracts.Dtos;
using Sfc.Core.OnPrem.Security.Contracts.Interfaces;
using Sfc.Wms.App.Api.Controllers;

namespace Sfc.Wms.App.Api.Tests.Unit.Fixtures
{
    public abstract class UserMasterControllerFixture
    {
        private readonly Mock<IUserRbacService> _mockIUserRbacService;
        private readonly PreferencesDto _preferencesDto;
        private readonly IEnumerable<PreferencesDto> _preferencesDtos;
        private readonly UserMasterController _userMasterController;
        private Task<IHttpActionResult> _testResponse;

        public UserMasterControllerFixture()
        {
            _mockIUserRbacService = new Mock<IUserRbacService>(MockBehavior.Default);
            _preferencesDtos = new List<PreferencesDto>
            {
                new PreferencesDto {Id = 0},
                new PreferencesDto {Id = 1}
            };
            _preferencesDto = Generator.Default.Single<PreferencesDto>();
            _userMasterController = new UserMasterController(_mockIUserRbacService.Object);
        }

        #region Mock

        private void VerifyUpdateUserPreferences()
        {
            _mockIUserRbacService.Verify(el => el.UpsertUserPreferences(_preferencesDtos));
        }

        private void MockUpdateUserPreferences(ResultTypes resultType)
        {
            var response = new BaseResult<IEnumerable<PreferencesDto>>
            {
                ResultType = resultType
            };
            _mockIUserRbacService.Setup(el => el.UpsertUserPreferences(_preferencesDtos))
                .Returns(Task.FromResult(response));
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
            _testResponse = _userMasterController.UpdateUserPreferences(_preferencesDtos);
        }

        protected void UpdateUserPreferencesReturnedOkResponse()
        {
            VerifyUpdateUserPreferences();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as OkNegotiatedContentResult<BaseResult<IEnumerable<PreferencesDto>>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.Ok, result.Content.ResultType);
        }

        protected void UpdateUserPreferencesReturnedNotFoundResponse()
        {
            VerifyUpdateUserPreferences();
            Assert.IsNotNull(_testResponse);
            var result = _testResponse.Result as NegotiatedContentResult<BaseResult<IEnumerable<PreferencesDto>>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.NotFound, result.Content.ResultType);
        }

        #endregion UpdateUserPreferences
    }
}