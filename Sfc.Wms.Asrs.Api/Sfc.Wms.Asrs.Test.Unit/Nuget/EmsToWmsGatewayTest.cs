using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Asrs.Test.Unit.Constants;
using Sfc.Wms.Asrs.Test.Unit.Fixtures.Nuget;
using TestStack.BDDfy;

namespace Sfc.Wms.Asrs.Test.Unit.Nuget
{
    [TestClass]
    [Story(
        AsA = "108268 Create a package to perform crud operation on the EMSTOWMS table",
        IWant = "To perfoem CRUD operations on EmsToWms table",
        SoThat = "CRUD operations are done on EmsToWms table"
    )]
    public class EmsToWmsGatewayTest : EmsToWmsGatewayFixture
    {
        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Get_By_Status_Invocation_Returned_Ok_Status_As_Response()
        {
            this.Given(el => el.ValidStatusAsInput())
                .When(el => el.GetByStatusInvoked())
                .Then(el => el.TheGetByStatusOperationReturnedOkResponse())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetAll_Invocation_Returned_Ok_Status_As_Response()
        {
            this.Given("no input filter")
                .When(el => el.GetAllInvoked())
                .Then(el => el.TheGetAllOperationReturnedOkResponse())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Delete_Invocation_Returned_Ok_Status_As_Response()
        {
            this.Given(el => el.InputValidKeyForDelete())
                .When(el => el.DeleteInvoked())
                .Then(el => el.TheDeleteOperationReturnedOkResponse())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Delete_Invocation_Returned_Not_Found_Status_As_Response()
        {
            this.Given(el => el.InputKeyForWhichRecordDoesNotExistToDelete())
                .When(el => el.DeleteInvoked())
                .Then(el => el.TheDeleteOperationReturnedNotFoundResponse())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Insert_Invocation_Returned_Created_Status_As_Response()
        {
            this.Given(el => el.InputValidDataForInsertion())
                .When(el => el.InsertInvoked())
                .Then(el => el.TheInsertOperationReturnedCreatedResponse())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Insert_Invocation_Returned_Confilct_Status_As_Response()
        {
            this.Given(el => el.InputDataWithrWhichRecordAlreadyExist())
                .When(el => el.InsertInvoked())
                .Then(el => el.TheInsertOperationReturnedConflictResponse())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Update_Invocation_Returned_Not_Found_Status_As_Response()
        {
            this.Given(el => el.InputDataForWhichRecordDoesNotExistToUpdate())
                .When(el => el.UpdateInvoked())
                .Then(el => el.TheUpdateOperationReturnedNotFoundResponse())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Update_Invocation_Returned_Ok_Status_As_Response()
        {
            this.Given(el => el.InputValidDataForUpdate())
                .When(el => el.UpdateInvoked())
                .Then(el => el.TheUpdateOperationReturnedOkResponse())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetDetails_Invocation_Returned_Ok_Status_As_Response()
        {
            this.Given(el => el.ValidKeyAsInput())
                .When(el => el.GetDetailsInvoked())
                .Then(el => el.TheGetDetailsOperationReturnedOkResponse())
                .BDDfy();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetDetails_Invocation_Returned_Not_Found_Status_As_Response()
        {
            this.Given(el => el.InputKeyForWhichRecordDoesNotExist())
                .When(el => el.GetDetailsInvoked())
                .Then(el => el.TheGetDetailsOperationReturnedNotFoundResponse())
                .BDDfy();
        }
    }
}