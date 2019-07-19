
using DataGenerator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Sfc.Wms.Asrs.Dematic.Contracts.Dtos;
using Sfc.Wms.Asrs.Dematic.Repository.Context;
using Sfc.Wms.Asrs.Dematic.Repository.Dtos;
using Sfc.Wms.Asrs.Dematic.Repository.Gateways;
using Sfc.Wms.Asrs.Test.Unit.Fakes;
using Sfc.Wms.Result;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Sfc.Wms.Asrs.Shamrock.Repository.Gateways;
using Sfc.Wms.DematicMessage.Contracts.Dto;

namespace Sfc.Wms.Asrs.Test.Unit.Fixtures
{
    public abstract class TaskGatewayFixture
    {
        private readonly TaskGateway _taskGateway;
        private readonly Mock<DematicContext> _dematicContext;
        private dynamic _manipulationTestResult;
        private IQueryable<TaskDetail>  taskDtlData;
        private IQueryable<TaskHeader> taskHdrData;
        private int containerId;
        private int skuId;

        protected TaskGatewayFixture()
        {
            var mapper = new Mock<IMapper>(MockBehavior.Default);
            _dematicContext = new Mock<DematicContext>(It.IsAny<string>());
            _taskGateway = new TaskGateway(_dematicContext.Object, mapper.Object);
        }

        protected void SetupFakeDb()
        {
            var mockSetHdr = new Mock<DbSet<TaskHeader>>();
            mockSetHdr.As<IDbAsyncEnumerable<TaskHeader>>().Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<TaskHeader>(taskHdrData.GetEnumerator()));
            mockSetHdr.As<IQueryable<TaskHeader>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<TaskHeader>(taskHdrData.Provider));
            mockSetHdr.As<IQueryable<TaskHeader>>().Setup(m => m.Expression).Returns(taskHdrData.Expression);
            mockSetHdr.As<IQueryable<TaskHeader>>().Setup(m => m.ElementType).Returns(taskHdrData.ElementType);
            mockSetHdr.As<IQueryable<TaskHeader>>().Setup(m => m.GetEnumerator()).Returns(taskHdrData.GetEnumerator());

            var mockSetDtl = new Mock<DbSet<TaskDetail>>();
            mockSetDtl.As<IDbAsyncEnumerable<TaskDetail>>().Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<TaskDetail>(taskDtlData.GetEnumerator()));
            mockSetDtl.As<IQueryable<TaskDetail>>().Setup(m => m.Provider).Returns(new TestDbAsyncQueryProvider<TaskDetail>(taskDtlData.Provider));
            mockSetDtl.As<IQueryable<TaskDetail>>().Setup(m => m.Expression).Returns(taskDtlData.Expression);
            mockSetDtl.As<IQueryable<TaskDetail>>().Setup(m => m.ElementType).Returns(taskDtlData.ElementType);
            mockSetDtl.As<IQueryable<TaskDetail>>().Setup(m => m.GetEnumerator()).Returns(taskDtlData.GetEnumerator());

            _dematicContext.Setup(x => x.TaskDetails).Returns(mockSetDtl.Object);
            _dematicContext.Setup(x => x.TaskHeaders).Returns(mockSetHdr.Object);
        }

        protected void ValidInputData()
        {
            containerId = Generator.Default.Single<int>();
            skuId = Generator.Default.Single<int>();
            var taskId = Generator.Default.Single<int>();
            taskDtlData = new List<TaskDetail>() { new TaskDetail() { ContainerNumber = containerId.ToString(), SkuId = skuId.ToString() ,TaskId = taskId} }.AsQueryable();
            taskHdrData = new List<TaskHeader>() { new TaskHeader() { TaskId = taskId } }.AsQueryable();
           
            SetupFakeDb();
        }
        protected void InvalidInputData()
        {
            taskDtlData = Generator.Default.List<TaskDetail>().AsQueryable();
            taskHdrData = Generator.Default.List<TaskHeader>().AsQueryable();
            
            SetupFakeDb();
        }

        protected void GetTaskDetailInvoked()
        {
            _manipulationTestResult = _taskGateway.GetTaskDetailAsync(containerId.ToString(), skuId.ToString()).Result;
        }

       

        protected void TaskDetailShouldBeReturned()
        {
            var result = _manipulationTestResult as BaseResult<List<CompleteTaskEntity>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ResultType, ResultTypes.Ok);
            Assert.IsNotNull(result.Payload);
        }

       
        protected void TaskDetailShouldNotBeReturned()
        {
            var result = _manipulationTestResult as BaseResult<List<CompleteTaskEntity>>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ResultType, ResultTypes.NotFound);
            Assert.IsNull(result.Payload);
        }

       
    }
}