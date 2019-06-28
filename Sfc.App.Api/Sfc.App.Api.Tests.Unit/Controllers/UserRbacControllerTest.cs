﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.App.Api.Tests.Unit.Fixtures;
using TestStack.BDDfy;

namespace Sfc.App.Api.Tests.Unit.Controllers
{
    [TestClass]
    [Story(
        AsA = "To do user Role based authentication",
        IWant = "User to get authenticated",
        SoThat = "Authenticated user can access the restricted resources"
    )]
    public class UserRbacControllerTest : UserRbacControllerFixture
    {
        [TestMethod]
        [TestCategory("UNIT")]
        public void Should_Return_Unauthorized_As_Response_When_InValid_Login_Credentials_Are_Passed()
        {
            this.Given(el => InvalidDataInRequest())
                .When(el => InvokeSignInUser())
                .Then(el => TheReturnedResponseStatusIsUnauthorized())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory("UNIT")]
        public void Should_Return_Authorized_As_Response_When_Valid_Login_Credentials_Are_Passed()
        {
            this.Given(el => ValidInputDataInRequest())
                .When(el => InvokeSignInUser())
                .Then(el => TheReturnedResponseStatusIsAuthorized())
                .BDDfy();
        }
    }
}