using AutoMapper;
using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.Wms.Asrs.App.Interfaces;
using Sfc.Wms.Asrs.App.Mappers;
using Sfc.Wms.Asrs.App.Services;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Dematic.Repository.Dtos;
using Sfc.Wms.Asrs.Dematic.Repository.Interfaces;
using Sfc.Wms.Asrs.Shamrock.Contracts.Dtos;
using Sfc.Wms.Asrs.Shamrock.Repository.Entities;
using Sfc.Wms.Asrs.Shamrock.Repository.Interfaces;
using Sfc.Wms.DematicMessage.Contracts.Constants;
using Sfc.Wms.DematicMessage.Contracts.Dto;
using Sfc.Wms.DematicMessage.Contracts.Interfaces;
using Sfc.Wms.DematicMessage.Contracts.Validation;
using Sfc.Wms.DematicMessage.FakeBuilder.Fakes;
using Sfc.Wms.DematicMessage.Translator.Interfaces;
using Sfc.Wms.Result;
using Sfc.Wms.TransitionalInventory.Contracts.Dtos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sfc.Wms.Amh.App.Interfaces;
using Sfc.Wms.Amh.Nuget.Interfaces;
using Sfc.Wms.Amh.Shamrock.Contracts.Dtos;
using Sfc.Wms.TransitionalInventory.Contracts.Interfaces;

namespace Sfc.Wms.Asrs.Test.Unit.Fixtures
{
    public class EmsToWmsMessageServiceFixture
    {
        private readonly Mock<IMainParser> _messageParser;
        private readonly Mock<IDematicGateway<EmsToWms>> _dematicGateway;
        private readonly EmsToWmsMessageProcessorService _emsToWmsMessageProcessorService;
        private dynamic _testResult;

        public EmsToWmsMessageServiceFixture()
        {
            _messageParser = new Mock<IMainParser>(MockBehavior.Default);

            _dematicGateway = new Mock<IDematicGateway<EmsToWms>>(MockBehavior.Default);
            var itemMasterGateway = new Mock<IDematicGateway<ItemMaster>>(MockBehavior.Default);
            var shamrockGateway = new Mock<IShamrockGateway<SwmFromMhe>>(MockBehavior.Default);
            var inboundLpnService = new Mock<IInboundLpnService>(MockBehavior.Default);
            var transitionalInventoryGateway = new Mock<ITransitionalInventoryService>(MockBehavior.Default);
            inboundLpnService.Setup(e => e.UpdateCaseDtlQuantityAsync(It.IsAny<IvmtDto>(), It.IsAny<string>()))
              .Returns(Task.FromResult(new BaseResult()
              {
                  ResultType = ResultTypes.Ok
              }));
            transitionalInventoryGateway.Setup(e => e.CostUpdateAsync(It.IsAny<TransitionalInventoryDto>()))
               .Returns(Task.FromResult(new BaseResult()
               {
                   ResultType = ResultTypes.Created
               }));
            var mapper = new Mock<IMapper>(MockBehavior.Default);

            var mappingFixture = new Mock<IMappingFixture>(MockBehavior.Default);

            mappingFixture.Setup(el => el.Mapping<Amh.Shamrock.Contracts.Dtos.InboundMessageDto, IvmtDto>(It.IsAny<Amh.Shamrock.Contracts.Dtos.InboundMessageDto>()))
                .Returns(Generator.Default.Single<IvmtDto>());
            mapper.Setup(e => e.Map<EmsToWms, EmsToWmsDto>(It.IsAny<EmsToWms>())).Returns(Generator.Default.Single<EmsToWmsDto>());

            mapper.Setup(e => e.Map<SwmFromMheDto, SwmFromMhe>(It.IsAny<SwmFromMheDto>())).Returns(Generator.Default.Single<SwmFromMhe>());

            mappingFixture.Setup(el => el.Mapping<CaseHeaderDto, ComtDto>(It.IsAny<CaseHeaderDto>()))
                .Returns(Generator.Default.Single<ComtDto>());
            var ivmtTriggerInput = Generator.Default.Single<IvmtTriggerInputDto>();

            mappingFixture.Setup(el => el.Mapping<ComtTriggerInput, IvmtTriggerInputDto>(It.IsAny<ComtTriggerInput>()))
                .Returns(ivmtTriggerInput);

            _dematicGateway.Setup(e => e.SaveChangesAsync())
                .Returns(Task.FromResult(new BaseResult() { ResultType = ResultTypes.Ok }));

            shamrockGateway.Setup(e => e.InsertAsync(It.IsAny<SwmFromMhe>(), It.IsAny<Expression<Func<SwmFromMhe, bool>>>()))
                .Returns(Task.FromResult(new BaseResult() { ResultType = ResultTypes.Created }));

            shamrockGateway.Setup(e => e.GetNextValueAsync())
                .Returns(Task.FromResult(new BaseResult<decimal>() { Payload = Generator.Default.Single<int>() }));
            _dematicGateway.Setup(e => e.GetNextValueAsync())
                .Returns(Task.FromResult(new BaseResult<decimal>() { Payload = Generator.Default.Single<int>() }));

            shamrockGateway.Setup(e => e.GetAsync(It.IsAny<Expression<Func<SwmFromMhe, bool>>>()))
                .Returns(Task.FromResult(new BaseResult<SwmFromMhe>() { Payload = Generator.Default.Single<SwmFromMhe>() }));

            shamrockGateway.Setup(e => e.SaveChangesAsync())
                .Returns(Task.FromResult(new BaseResult()));

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
            var dematicTranslator = new Mock<IDematicTranslatorInterface>(MockBehavior.Default);
            dematicTranslator.Setup(e => e.TranslateToSwmFromMhe(It.IsAny<EmsToWmsDto>()))
                .Returns(Generator.Default.Single<BaseResult<SwmFromMheDto>>());
            var allocationInvnResponse = new BaseResult<AllocationInventoryDetail>() {ResultType = ResultTypes.Ok};
            var allocationGateway = new Mock<IShamrockGateway<AllocationInventoryDetail>>(MockBehavior.Default);
            allocationGateway.Setup(e => e.GetAsync(It.IsAny<Expression<Func<AllocationInventoryDetail, bool>>>()))
                .Returns(Task.FromResult(allocationInvnResponse));

            var pickLocationDtlGateway = new Mock<IShamrockGateway<PickLocationDtl>>(MockBehavior.Default);
            var pickLocationService = new Mock<PickLocationService>(mapper.Object, pickLocationDtlGateway.Object);
            
            var emsToWms = Generator.Default.Single<EmsToWmsDto>();

            var caseHdrGateway = new Mock<IDematicGateway<CaseHeaderDto>>(MockBehavior.Default);
            var inboundMessageGateway = new Mock<IInboundMessageService>(MockBehavior.Default);

            _emsToWmsMessageProcessorService = new EmsToWmsMessageProcessorService(
                _dematicGateway.Object,
                shamrockGateway.Object,
                _messageParser.Object,
                mapper.Object,
                dematicTranslator.Object,
                allocationGateway.Object,
                pickLocationService.Object,
                itemMasterGateway.Object,
                transitionalInventoryGateway.Object,
                inboundMessageGateway.Object
               );
        }

        private void MockAllTheInputServices(bool valid)
        {
            var getResponse = new BaseResult<EmsToWms>()
            {
                ResultType = valid ? ResultTypes.Ok : ResultTypes.BadRequest,
                Payload = valid ? Generator.Default.Single<EmsToWms>() : null
            };
            var swmFromMheDto=Generator.Default.Single<SwmFromMheDto>();
            var fakeTextBuilder = new TextBuilder();
            var fakeMessageBuilder = new FakeMessageBuilder(fakeTextBuilder);
            var costDto =
                fakeMessageBuilder.BuildMessageJson<IvstDto, IvstValidationRule>(MessageBuilderTestCase.ValidRegex);
            swmFromMheDto.MessageJson = JsonConvert.SerializeObject(costDto);
           
            var getParserResponse = new BaseResult<SwmFromMheDto>()
            {
                ResultType = valid ? ResultTypes.Ok : ResultTypes.BadRequest,
                Payload = valid ? swmFromMheDto : null
            };
            var emsToWms = Generator.Default.Single<EmsToWmsDto>();
            _messageParser.Setup(e => e.ParseMessage(It.IsAny<EmsToWmsDto>(), out emsToWms))
                .Returns(getParserResponse);
            var wmsToEms = Generator.Default.Single<WmsToEmsDto>();
            _dematicGateway.Setup(e => e.GetAsync(It.IsAny<Expression<Func<EmsToWms, bool>>>()))
                .Returns(Task.FromResult(getResponse));
        }

        protected void ValidMessageKey()
        {
            MockAllTheInputServices(true);
        }

        protected void InvalidMessageKey()
        {
            MockAllTheInputServices(false);
        }

        protected void EmsToWmsMessageProcessorInvoked()
        {
            _testResult = _emsToWmsMessageProcessorService.GetMessageAsync(Generator.Default.Single<long>());
        }

        protected void EmsToWmsMessageShouldBeProcessed()
        {
            var result = _testResult.Result as BaseResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ResultType, ResultTypes.Created);
        }

        protected void EmsToWmsMessageShouldNotBeProcessed()
        {
            var result = _testResult.Result as BaseResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ResultType, ResultTypes.BadRequest);
        }
    }
}