using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.UserRbac.Test.Integrated.Fixtures;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TestStack.BDDfy;

namespace Sfc.Wms.UserRbac.Test.Integrated.Tests
{
    [TestClass]
    [Story(
        AsA = "Authorized user",
        IWant = "To login with valid credentials and access the authorized token, and test the RBAC services for the roles specified",
        SoThat = "I can validate the menus,permissions,preferences associated with the user with valid database values"
        )]
    public class UserRbacTest : UserRbacFixture
    {     
        [TestInitialize]
        [TestCategory("FUNCTIONAL")]
        public void AValidUserRbacTestData()
        {
            UserRbacTestData();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void MenusAssociatedWithUserRoleTest() 
        {
            this.Given(x => x.AValidNewLoginRecord())     
                .And(x => x.AValidRbacUrls())
                .And(x => x.SetCurrentUrlToMenu())               
                .When(x => x.SignInIsCalledOkIsReturned())
                .Then(x => x.ReadTokenValue())
                .When(x => x.UserRbacApiIsCalledOkIsReturned())          
                .Then(x => x.ReadTheMenusFromMenuResult())               
                .And(x => x.ValidateUserAssociatedMenuCounts())
                .And(x => x.ValidateUserAssociatedMenuIdValues())
                .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void PermissionsAssociatedWithUserRoleTest()  
        {
            this.Given(x => x.AValidNewLoginRecord())
                .And(x => x.AValidRbacUrls())
                .And(x => x.SetCurrentUrlToPermission())
                .When(x => x.SignInIsCalledOkIsReturned())
                .Then(x => x.ReadTokenValue())
                .When(x => x.UserRbacApiIsCalledOkIsReturned())
                .Then(x => x.ReadThePermissionsFromPermissionResult())
                .And(x => x.ValidateUserAssociatedPermissionCounts())
                .And(x => x.ValidateUserAssociatedPermissionIdValues())
                .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void PreferencesAssociatedWithUserRoleTest() 
        {
            this.Given(x => x.AValidNewLoginRecord())
                .And(x => x.AValidRbacUrls())
                .And(x => x.SetCurrentUrlToPreference())
                .When(x => x.SignInIsCalledOkIsReturned())
                .Then(x => x.ReadTokenValue())
                .When(x => x.UserRbacApiIsCalledOkIsReturned())
                .Then(x => x.ReadThePreferencesFromPreferenceResult())
                .And(x => x.ValidateUserAssociatedPreferenceCounts())
                .And(x => x.ValidateUserAssociatedPreferenceIdValues())
                .BDDfy();
        }

    }
}
