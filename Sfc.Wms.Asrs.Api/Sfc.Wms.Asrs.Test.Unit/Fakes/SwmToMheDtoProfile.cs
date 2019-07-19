﻿using DataGenerator;
using DataGenerator.Sources;
using Sfc.Wms.Asrs.Shamrock.Contracts.Dtos;

namespace Sfc.Wms.Asrs.Test.Unit.Fakes
{
    public class SwmToMheDtoProfile : MappingProfile<SwmToMheDto>
    {
        public override void Configure()
        {
            Property(e => e.MessageTargetId).DataSource<IntegerSource>();
            Property(e => e.SourceMessageProcess).DataSource<NameSource>();
            Property(e => e.SourceMessageKey).DataSource<IntegerSource>();
            Property(e => e.SourceMessageStatus).DataSource<NameSource>();
            Property(e => e.SourceMessageTransactionCode).DataSource<NameSource>();
            Property(e => e.SourceMessageText).DataSource<NameSource>();
            Property(e => e.SourceMessageResponseCode).DataSource<IntegerSource>();
            Property(e => e.ZplData).DataSource<NameSource>();
            Property(e => e.ErrorMessage).DataSource<NameSource>();
            Property(e => e.ErrorCode).DataSource<NameSource>();
            Property(e => e.MessageStatus).DataSource<IntegerSource>();
            Property(e => e.MessageJson).DataSource<NameSource>();
            Property(e => e.ContainerId).DataSource<NameSource>();
            Property(e => e.ContainerType).DataSource<NameSource>();
            Property(e => e.LocationId).DataSource<NameSource>();
            Property(e => e.LotId).DataSource<NameSource>();
            Property(e => e.OrderId).DataSource<NameSource>();
            Property(e => e.OrderLineId).DataSource<IntegerSource>();
            Property(e => e.OrderType).DataSource<NameSource>();
            Property(e => e.PoNumber).DataSource<NameSource>();
            Property(e => e.SkuId).DataSource<NameSource>();
            Property(e => e.StagingLocation).DataSource<NameSource>();
            Property(e => e.WaveNumber).DataSource<NameSource>();
            Property(e => e.Quantity).DataSource<IntegerSource>();
            Property(e => e.CreatedDateTime).DataSource<DateTimeSource>();
            Property(e => e.UpdatedDateTime).DataSource<DateTimeSource>();
            Property(e => e.UpdatedBy).DataSource<NameSource>();
            Property(e => e.CreatedBy).DataSource<NameSource>();
        }
    }
}
