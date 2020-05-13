using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.Core.OnPrem.Result;
using Sfc.Wms.App.Api.Controllers;
using Sfc.Wms.Configuration.MessageLogger.Contracts.UoW.Dtos;
using Sfc.Wms.Configuration.MessageLogger.Contracts.UoW.Interfaces;

namespace Sfc.Wms.App.Api.Tests.Unit.Fixtures
{
    public class MessageLogControllerFixture
    {
        private readonly MessageLogController _messageLogController;
        private readonly Mock<IMessageLogService> _messageLogService;
        private IEnumerable<MessageLogDto> logs;
        private Task<IHttpActionResult> testResponse;

        protected MessageLogControllerFixture()
        {
            _messageLogService = new Mock<IMessageLogService>(MockBehavior.Default);
            _messageLogController = new MessageLogController(_messageLogService.Object);
        }

        protected void InputParametersForBatchInsertion()
        {
            logs = Generator.Default.List<MessageLogDto>(2);

            _messageLogService.Setup(el =>
                    el.InsertRangeAsync(It.IsAny<IEnumerable<MessageLogDto>>()))
                .Returns(Task.FromResult(new BaseResult {ResultType = ResultTypes.Created}));
        }

        protected void EmptyOrNullInputForInsertion()
        {
            logs = null;
            _messageLogService.Setup(el =>
                    el.InsertRangeAsync(It.IsAny<IEnumerable<MessageLogDto>>()))
                .Returns(Task.FromResult(new BaseResult {ResultType = ResultTypes.BadRequest}));
        }

        protected void BatchInsertionOperationInvoked()
        {
            testResponse = _messageLogController.BatchInsertAsync(logs);
        }

        protected void TheReturnedBadRequestResponse()
        {
            Assert.IsNotNull(testResponse);
            var result = testResponse.Result as NegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.BadRequest, result.Content.ResultType);
        }

        protected void TheReturnedOkResponse()
        {
            _messageLogService.Verify(el => el.InsertRangeAsync(It.IsAny<IEnumerable<MessageLogDto>>()));
            Assert.IsNotNull(testResponse);
            var result = testResponse.Result as OkNegotiatedContentResult<BaseResult>;
            Assert.IsNotNull(result);
            Assert.AreEqual(ResultTypes.Created, result.Content.ResultType);
        }
    }
}