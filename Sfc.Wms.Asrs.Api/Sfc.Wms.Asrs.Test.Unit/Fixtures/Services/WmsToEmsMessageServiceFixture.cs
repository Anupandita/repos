using AutoMapper;
using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.Wms.Amh.App.Interfaces;
using Sfc.Wms.Amh.Nuget.Interfaces;
using Sfc.Wms.Amh.Shamrock.Contracts.Dtos;
using Sfc.Wms.Asrs.App.Interfaces;
using Sfc.Wms.Asrs.App.Mappers;
using Sfc.Wms.Asrs.App.Services;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Dematic.Repository.Dtos;
using Sfc.Wms.Asrs.Dematic.Repository.Interfaces;
using Sfc.Wms.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Asrs.Shamrock.Repository.Entities;
using Sfc.Wms.Asrs.Shamrock.Repository.Interfaces;
using Sfc.Wms.DematicMessage.Contracts.Dto;
using Sfc.Wms.DematicMessage.Contracts.Interfaces;
using Sfc.Wms.Result;
using Sfc.Wms.TransitionalInventory.Contracts.Dtos;
using Sfc.Wms.TransitionalInventory.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Sfc.Wms.Amh.Shamrock.Repository.Entities;
using InboundMessageDto = Sfc.Wms.Amh.Shamrock.Contracts.Dtos.InboundMessageDto;

namespace Sfc.Wms.Asrs.Test.Unit.Fixtures
{
    public class WmsToEmsMessageServiceFixture
    {
        private readonly Mock<ICanBuildMessage> _buildMessage;
        private readonly Mock<IInboundMessageService> _inboundMessageGateway;
        private readonly Mock<IAmhService<CaseHeader, CaseHeaderDto>> _caseHeaderGateway;
        private readonly WmsToEmsMessageProcessorService _wmsToEmsMessageProcessorService;
        private dynamic _testResult;

        public WmsToEmsMessageServiceFixture()
        {
            _buildMessage = new Mock<ICanBuildMessage>(MockBehavior.Default);
            _inboundMessageGateway = new Mock<IInboundMessageService>(MockBehavior.Default);
            var dematicGateway = new Mock<IDematicGateway<WmsToEms>>(MockBehavior.Default);
            var itemMasterGateway = new Mock<IDematicGateway<ItemMaster>>(MockBehavior.Default);
            var shamrockGateway = new Mock<IShamrockGateway<SwmToMhe>>(MockBehavior.Default);
            var inboundLpnService = new Mock<IInboundLpnService>(MockBehavior.Default);
            var transitionalInventoryGateway = new Mock<ITransitionalInventoryService>(MockBehavior.Default);
            inboundLpnService.Setup(e => e.UpdateCaseDtlQuantityAsync(It.IsAny<IvmtDto>(), It.IsAny<string>()))
              .Returns(Task.FromResult(new BaseResult()
              {
                  ResultType = ResultTypes.Ok
              }));
            transitionalInventoryGateway.Setup(e => e.IvmtInsertAsync(It.IsAny<TransitionalInventoryDto>()))
               .Returns(Task.FromResult(new BaseResult()
               {
                   ResultType = ResultTypes.Created
               }));
            var mapper = new Mock<IMapper>(MockBehavior.Default);

            var mappingFixture = new Mock<IMappingFixture>(MockBehavior.Default);

            mappingFixture.Setup(el => el.Mapping<InboundMessageDto, IvmtDto>(It.IsAny<InboundMessageDto>()))
                .Returns(Generator.Default.Single<IvmtDto>());
            mapper.Setup(e => e.Map<SwmToMheDto, SwmToMhe>(It.IsAny<SwmToMheDto>())).Returns(Generator.Default.Single<SwmToMhe>());

            mapper.Setup(e => e.Map<WmsToEmsDto, WmsToEms>(It.IsAny<WmsToEmsDto>())).Returns(Generator.Default.Single<WmsToEms>());

            mappingFixture.Setup(el => el.Mapping<CaseHeaderDto, ComtDto>(It.IsAny<CaseHeaderDto>()))
                .Returns(Generator.Default.Single<ComtDto>());
            var ivmtTriggerInput = Generator.Default.Single<IvmtTriggerInputDto>();

            mappingFixture.Setup(el => el.Mapping<ComtTriggerInput, IvmtTriggerInputDto>(It.IsAny<ComtTriggerInput>()))
                .Returns(ivmtTriggerInput);

            dematicGateway.Setup(e => e.InsertAsync(It.IsAny<WmsToEms>(), It.IsAny<Expression<Func<WmsToEms, bool>>>()))
                .Returns(Task.FromResult(new BaseResult() { ResultType = ResultTypes.Created }));

            shamrockGateway.Setup(e => e.InsertAsync(It.IsAny<SwmToMhe>(), It.IsAny<Expression<Func<SwmToMhe, bool>>>()))
                .Returns(Task.FromResult(new BaseResult() { ResultType = ResultTypes.Created }));

            dematicGateway.Setup(e => e.UpdateAsync(It.IsAny<WmsToEms>(), It.IsAny<Expression<Func<WmsToEms, bool>>>()))
                .Returns(Task.FromResult(new BaseResult() { ResultType = ResultTypes.Ok }));

            shamrockGateway.Setup(e => e.GetNextValueAsync())
                .Returns(Task.FromResult(new BaseResult<decimal>() { Payload = Generator.Default.Single<int>() }));
            dematicGateway.Setup(e => e.GetNextValueAsync())
                .Returns(Task.FromResult(new BaseResult<decimal>() { Payload = Generator.Default.Single<int>() }));

            dematicGateway.Setup(e => e.UpdateAsync(It.IsAny<WmsToEms>(), It.IsAny<Expression<Func<WmsToEms, bool>>>()))
                .Returns(Task.FromResult(new BaseResult() { ResultType = ResultTypes.Ok }));

            itemMasterGateway.Setup(e => e.GetAllAsync(It.IsAny<Expression<Func<ItemMaster, bool>>>()))
                .Returns(Task.FromResult(new BaseResult<IEnumerable<ItemMaster>>()
                {
                    ResultType = ResultTypes.Ok,
                    Payload = Generator.Default.List<ItemMaster>()
                }));
            itemMasterGateway.Setup(e => e.GetAsync(It.IsAny<Expression<Func<ItemMaster, bool>>>()))
                .Returns(Task.FromResult(new BaseResult<ItemMaster>()
                {
                    ResultType = ResultTypes.Ok,
                    Payload = Generator.Default.Single<ItemMaster>()
                }));
            _caseHeaderGateway = new Mock<IAmhService<CaseHeader,CaseHeaderDto>>();
            dematicGateway.Setup(e => e.GetAsync(It.IsAny<Expression<Func<WmsToEms, bool>>>()))
                .Returns(Task.FromResult(new BaseResult<WmsToEms>() { Payload = Generator.Default.Single<WmsToEms>() }));
            dematicGateway.Setup(e => e.SaveChangesAsync())

                .Returns(Task.FromResult(new BaseResult() { ResultType = ResultTypes.Ok }));
            _wmsToEmsMessageProcessorService = new WmsToEmsMessageProcessorService(
                _inboundMessageGateway.Object,
                dematicGateway.Object,
                shamrockGateway.Object,
                _buildMessage.Object,
                 mappingFixture.Object,
                 mapper.Object,
                itemMasterGateway.Object,
                inboundLpnService.Object,
                transitionalInventoryGateway.Object,
                _caseHeaderGateway.Object

                );
        }

        private void MockAllTheInputServicesForIvmt(bool valid)
        {
            var getResponse = new BaseResult<InboundMessageDto>()
            {
                ResultType = valid ? ResultTypes.Ok : ResultTypes.BadRequest,
                Payload = valid ? Generator.Default.Single<InboundMessageDto>() : null
            };
            var getBuilderResponse = new BaseResult<SwmToMheDto>()
            {
                ResultType = valid ? ResultTypes.Ok : ResultTypes.ExpectationFailed,
                Payload = valid ? Generator.Default.Single<SwmToMheDto>() : null
            };
            var wmsToEms = Generator.Default.Single<WmsToEmsDto>();
            _buildMessage.Setup(e => e.BuildMessage(It.IsAny<DematicMessageBaseDto>(),
                    It.IsAny<string>(), out wmsToEms))
                .Returns(getBuilderResponse);

            _inboundMessageGateway.Setup(el => el.GetAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(getResponse));
        }

        private void MockAllTheInputServicesForComt(bool valid)
        {
            var getResponse = new BaseResult<CaseHeaderDto>()
            {
                ResultType = valid ? ResultTypes.Ok : ResultTypes.BadRequest,
                Payload = valid ? Generator.Default.Single<CaseHeaderDto>() : null
            };
            var getBuilderResponse = new BaseResult<SwmToMheDto>()
            {
                ResultType = valid ? ResultTypes.Ok : ResultTypes.ExpectationFailed,
                Payload = valid ? Generator.Default.Single<SwmToMheDto>() : null
            };
            var wmsToEms = Generator.Default.Single<WmsToEmsDto>();
            _caseHeaderGateway.Setup(el => el.GetAsync(It.IsAny<Expression<Func<CaseHeader, bool>>>()))
                .Returns(Task.FromResult(getResponse));
            _buildMessage.Setup(e => e.BuildMessage(It.IsAny<DematicMessageBaseDto>(),
               It.IsAny<string>(), out wmsToEms))
                .Returns(getBuilderResponse);
            MockAllTheInputServicesForIvmt(valid);
        }

        #region IvmtProcessor

        protected void ValidIvmtMessage()
        {
            MockAllTheInputServicesForIvmt(true);
        }

        protected void InvalidIvmtMessage()
        {
            MockAllTheInputServicesForIvmt(false);
        }

        protected void ProcessIvmtMessageIsInvoked()
        {
            _testResult = _wmsToEmsMessageProcessorService.GetIvmtMessageAsync(Generator.Default.Single<IvmtTriggerInputDto>());
        }

        protected void IvmtMessageShouldBeProcessed()
        {
            var result = _testResult.Result as BaseResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ResultType, ResultTypes.Created);
        }

        protected void IvmtMessageShouldNotBeProcessed()
        {
            var result = _testResult.Result as BaseResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ResultType, ResultTypes.ExpectationFailed);
        }

        #endregion IvmtProcessor

        #region ComtProcessor

        protected void ValidComtMessage()
        {
            MockAllTheInputServicesForComt(true);
        }

        protected void InvalidComtMessage()
        {
            MockAllTheInputServicesForComt(false);
        }

        protected void ProcessComtMessageIsInvoked()
        {
            _testResult = _wmsToEmsMessageProcessorService.GetComtMessageAsync(Generator.Default.Single<ComtTriggerInput>());
        }

        protected void ComtMessageShouldBeProcessed()
        {
            var result = _testResult.Result as BaseResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ResultType, ResultTypes.Created);
        }

        protected void ComtMessageShouldNotBeProcessed()
        {
            var result = _testResult.Result as BaseResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ResultType, ResultTypes.ExpectationFailed);
        }

        #endregion ComtProcessor
    }
}