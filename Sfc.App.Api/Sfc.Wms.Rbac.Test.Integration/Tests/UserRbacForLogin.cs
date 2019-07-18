using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.UserRbac.Test.Integrated.Fixtures;
using TestStack.BDDfy;

namespace Sfc.Wms.UserRbac.Test.Integrated.Tests
{
    [TestClass] 
    [Story(
        AsA = "Authorized user",
        IWant = "To login with valid credentials and test RBAC Scenarios in Login Service",
        SoThat = "I can validate the menus,permissions,preferences associated with the user with valid database values"
        )]
   
    public class UserRbacForLoginApi : UserRbacFixture
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
                .When(x => x.SignInIsCalledOkIsReturned())               
                .Then(x => x.ReadTheMenusFromLoginResult())            
                .And(x => x.ValidateUserAssociatedMenuCounts())
                .And(x => x.ValidateUserAssociatedMenuIdValues())
                .BDDfy();
        }

        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void PermissionsAssociatedWithUserRoleTest()
        {
            this.
                Given(x => x.AValidNewLoginRecord())               
                  .And(x => x.AValidRbacUrls())
                  .When(x => x.SignInIsCalledOkIsReturned())
                  .Then(x => x.ReadThePermissionsFromLoginResult())                  
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
                  .When(x => x.SignInIsCalledOkIsReturned())                 
                  .Then(x => x.ReadThePreferencesFromLoginResult())               
                  .And(x => x.ValidateUserAssociatedPreferenceCounts())
                  .And(x => x.ValidateUserAssociatedPreferenceIdValues())
                .BDDfy();
        }
    }
}
