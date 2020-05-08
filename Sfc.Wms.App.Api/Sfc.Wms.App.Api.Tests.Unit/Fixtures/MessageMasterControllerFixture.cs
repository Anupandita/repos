using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Controllers;
using Sfc.Wms.Configuration.MessageMaster.Contracts.UoW.Dtos;
using Sfc.Wms.Configuration.MessageMaster.Contracts.UoW.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

namespace Sfc.Wms.App.Api.Tests.Unit.Fixtures
{
    public class MessageMasterControllerFixture
    {
        private readonly MessageMasterController _messageMasterController;
        private readonly Mock<IMessageMasterService> _mock;
        private Task<IHttpActionResult> testResponse;

        protected MessageMasterControllerFixture()
        {
            _mock = new Mock<IMessageMasterService>(MockBehavior.Default);
            _messageMasterController = new MessageMasterController(_mock.Object);
        }

        protected void InputParametersForMessageDetailsRetrieval()
        {
            _mock.Setup(el =>
                    el.GetMessageDetailsAsync(It.IsAny<Expression<Func<MessageMasterDto, bool>>>()))
                .Returns(Task.FromResult(new BaseResult<List<MessageDetailDto>>
                {
                    ResultType = ResultTypes.Ok,
                    Payload = Generator.Default.List<MessageDetailDto>(2).ToList()
                }));
        }

        protected void GetOperationInvoked()
        {
            testResponse = _messageMasterController.GetUiSpecificMessageDetails();
        }

        protected void TheReturnedOkResponse()
        {
            _mock.Verify(el => el.GetMessageDetailsAsync(It.IsAny<Expression<Func<MessageMasterDto, bool>>>()));
            Assert.IsNotNull(testResponse);
            var result = testResponse.Result as OkNegotiatedContentResult<BaseResult<List<MessageDetailDto>>>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.IsNotNull(result.Content.Payload);
            Assert.AreEqual(ResultTypes.Ok, result.Content.ResultType);
        }

    }
}