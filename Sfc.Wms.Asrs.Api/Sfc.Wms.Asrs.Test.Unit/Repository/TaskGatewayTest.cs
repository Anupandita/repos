using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Asrs.Test.Unit.Fixtures;
using TestStack.BDDfy;

namespace Sfc.Wms.Asrs.Test.Unit.Repository
{
    [TestClass]
    [Story(
        AsA = "End user",
        IWant = "Perform get operation on Task tables",
        SoThat = "get operations are done on Task tables"
    )]
    public class TaskGatewayTest : TaskGatewayFixture
    {
        [TestMethod]
        [TestCategory("UNIT")]
        public void Get_TaskDetail_Invoked_With_Valid_Input_Data()
        {
            this.Given(el => el.ValidInputData())
                .When(el => el.GetTaskDetailInvoked())
                .Then(el => el.TaskDetailShouldBeReturned())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory("UNIT")]
        public void Get_Task_Detail_Invoked_With_Invalid_Input_Data()
        {
            this.Given(el => el.InvalidInputData())
                .When(el => el.GetTaskDetailInvoked())
                .Then(el => el.TaskDetailShouldNotBeReturned())
                .BDDfy();
        }

    



    }
}