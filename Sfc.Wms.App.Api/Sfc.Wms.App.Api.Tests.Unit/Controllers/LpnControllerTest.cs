using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Constants;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;

namespace Sfc.Wms.App.Api.Tests.Unit.Controllers
{
    [TestClass]
    public class LpnControllerTest : LpnControllerFixture
    {
        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void LpnSearch_Invocation_With_Valid_Parameters_Returned_Ok_As_Returned_Response()
        {
            ValidInputParametersForWhichRecordsExits();
            FindLpnOperationInvoked();
            TheLpnSearchOperationReturnedOkAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void LpnSearch_Invocation_With_InValid_Or_Empty_Parameters_Returned_BadRequest_As_Returned_Response()
        {
            InValidInputParametersAsInput();
            FindLpnOperationInvoked();
            TheFindLpnOperationReturnedBadRequestAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetLpnHistory_Invocation_Returned_NotFound_As_Returned_Response()
        {
            InputForLpnHistoryForWhichDetailsDoesNotExists();
            GetLpnHistoryOperationInvoked();
            TheGetLpnHistoryOperationReturnedNotFoundResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetLpnHistory_Invocation_With_Valid_Parameters_Returned_Ok_As_Returned_Response()
        {
            InputForLpnHistoryForWhichDetailsExists();
            GetLpnHistoryOperationInvoked();
            TheGetLpnHistoryOperationReturnedOkResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetLpnHistory_Invocation_With_InValid_Parameters_Returned_BadRequest_As_Returned_Response()
        {
            InValidInputForLpnHistory();
            GetLpnHistoryOperationInvoked();
            TheGetLpnHistoryOperationReturnedBadRequestResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetAisleTransaction_Invocation_Returned_NotFound_As_Returned_Response()
        {
            InputForAisleTransactionForWhichRecordDoesNotExists();
            AisleTransactionOperationInvoked();
            TheGetAisleTransactionOperationReturnedNotFoundResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetAisleTransaction_Invocation_Returned_Ok_As_Returned_Response()
        {
            InputForAisleTransactionForWhichRecordExists();
            AisleTransactionOperationInvoked();
            TheGetAisleTransactionOperationReturnedOkResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetAisleTransaction_Invocation_Returned_BadRequest_As_Returned_Response()
        {
            InValidInputForAisleTransaction();
            AisleTransactionOperationInvoked();
            TheGetAisleTransactionOperationReturnedBadRequestResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void DeleteLpnComments_Invocation_Returned_NotFound_As_Returned_Response()
        {
            InputForDeleteLpnCommentsForWhichRecordDoesNotExists();
            DeleteLpnCommentsOperationInvoked();
            TheDeleteLpnCommentsOperationReturnedNotFoundResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void DeleteLpnComments_Invocation_Returned_Ok_As_Returned_Response()
        {
            InputForDeleteLpnCommentsForWhichRecordExists();
            DeleteLpnCommentsOperationInvoked();
            TheDeleteLpnCommentsOperationReturnedOkResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void DeleteLpnComments_Invocation_Returned_BadRequest_As_Returned_Response()
        {
            InValidInputForDeleteLpnComments();
            DeleteLpnCommentsOperationInvoked();
            TheDeleteLpnCommentsOperationReturnedBadRequestResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void LpnHeaderUpdate_Invocation_Returned_Ok_As_Returned_Response()
        {
            InputForUpdateLpnForWhichRecordDoesNotExists();
            UpdateLpnOperationInvoked();
            TheUpdateOperationReturnedNotFoundResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void LpnHeaderUpdate_Operation_Is_Invoked_With_Valid_Data_Returned_Ok_As_Response_Status()
        {
            InputForUpdateLpnForWhichRecordExists();
            UpdateLpnOperationInvoked();
            TheUpdateOperationReturnedOkResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void CaseCommentUpdate_Operation_Is_Invoked_With_Valid_Data_Returned_Ok_As_Response_Status()
        {
            InputForUpdateCaseCommentForWhichRecordExists();
            UpdateCaseCommentOperationInvoked();
            TheUpdateCaseCommentOperationReturnedOkResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void LpnHeaderUpdate_Operation_Is_Invoked_With_InValid_Data_Returned_BadRequest_As_Response_Status()
        {
            InvalidInputForUpdateLpn();
            UpdateLpnOperationInvoked();
            TheUpdateOperationReturnedBadRequestResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void CaseCommentUpdate_Operation_Is_Invoked_With_InValid_Data_Returned_BadRequest_As_Response_Status()
        {
            InvalidInputForUpdateCaseComment();
            UpdateCaseCommentOperationInvoked();
            TheUpdateOperationReturnedBadRequestResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void UpdateCaseDetails_Invocation_Returned_NotFound_As_Returned_Response()
        {
            UpdateCaseDetailsRecordDoesNotExists();
            UpdateCaseDetailsOperationInvoked();
            TheUpdateCaseDetailsOperationReturnedNotFoundResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void UpdateCaseDetails_Invocation_Returned_Ok_As_Returned_Response()
        {
            UpdateCaseDetailsRecordExists();
            UpdateCaseDetailsOperationInvoked();
            TheUpdateCaseDetailsOperationReturnedOkResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void UpdateCaseDetails_Invocation_Returned_BadRequest_As_Returned_Response()
        {
            InvalidInputForUpdateCaseDetails();
            UpdateCaseDetailsOperationInvoked();
            TheUpdateCaseDetailsOperationReturnedBadRequestResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetLpnComments_Operation_Is_Invoked_With_Invalid_Record_NotFount_Is_Returned()
        {
            GetLpnCommentsRecordDoesNotExists();
            GetLpnCommentsOperationInvoked();
            TheGetLpnCommentsOperationReturnedNotFoundResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetLpnComments_Operation_Is_Invoked_With_Valid_Data_Ok_Response_Is_Returned()
        {
            GetLpnCommentsRecordExists();
            GetLpnCommentsOperationInvoked();
            TheGetLpnCommentsOperationReturnedOkResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void AddLpnComments_Invocation_Returned_Created_As_Response_Status()
        {
            ValidInputForWhichNoRecordExits();
            AddLpnCommentsOperationInvoked();
            TheAddLpnCommentsOperationReturnedCreatedResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void AddLpnComments_Invocation_Returned_Conflict_As_Response_Status()
        {
            AddLpnCommentsRecordExists();
            AddLpnCommentsOperationInvoked();
            TheAddLpnCommentsOperationReturnedConflictResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void AddLpnComments_Invocation_Returned_BadRequest_As_Response_Status()
        {
            InvalidInputForLpnComments();
            AddLpnCommentsOperationInvoked();
            TheAddLpnCommentsOperationReturnedBadRequestResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void LpnLockUnlock_Invocation_Returned_NotFound_As_Response_Status()
        {
            GetLpnLockUnlockRecordDoesNotExists();
            GetLpnLockUnlockOperationInvoked();
            TheGetLpnLockUnlockOperationReturnedNotFoundResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void LpnLockUnlock_Invocation_Returned_BadRequest_As_Response_Status()
        {
            InValidInputForGetLpnLockUnlock();
            GetLpnLockUnlockOperationInvoked();
            TheGetLpnLockUnlockOperationReturnedBadRequestResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void LpnLockUnlock_Invocation_Returned_Ok_As_Response_Status()
        {
            GetLpnLockUnlockRecordExists();
            GetLpnLockUnlockOperationInvoked();
            TheGetLpnLockUnlockOperationReturnedOkResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetCaseUnLockDetails_Invocation_Returned_NotFound_As_Response_Status()
        {
            GetCaseUnLockDetailsRecordDoesNotExists();
            GetCaseUnLockDetailsOperationInvoked();
            TheGetCaseUnLockDetailsReturnedNotFoundResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetCaseUnLockDetails_Invocation_Returned_BadRequest_As_Response_Status()
        {
            InValidInputForGetCaseUnLockDetails();
            GetCaseUnLockDetailsOperationInvoked();
            ThGetCaseUnLockDetailsReturnedBadRequestResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetCaseUnLockDetails_Invocation_Returned_Ok_As_Response_Status()
        {
            GetCaseUnLockDetailsRecordExists();
            GetCaseUnLockDetailsOperationInvoked();
            TheGetCaseUnLockDetailsReturnedOkResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetLpnDetails_Invocation_Returned_NotFound_As_Response_Status()
        {
            GetLpnDetailsRecordDoesNotExists();
            GetLpnDetailsOperationInvoked();
            TheGetLpnDetailsOperationReturnedNotFoundResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetLpnDetails_Invocation_Returned_Ok_As_Response_Status()
        {
            GetLpnDetailsRecordExists();
            GetLpnDetailsOperationInvoked();
            TheGetLpnDetailsOperationReturnedOkResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetLpnDetails_Invocation_Returned_BadRequest_As_Response_Status()
        {
            InvalidInputForGetLpnDetails();
            GetLpnDetailsOperationInvoked();
            TheGetLpnDetailsOperationReturnedBadRequestResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Add_Lock_comments_Operation_Is_Invoked_With_Invalid_Record_NotFount_Is_Returned()
        {
            InvalidParametersToAddCaseLockComments();
            AddCaseLockCommentsInvoked();
            AddCaseLockCommentsOperationReturnedNotFoundResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Add_Lock_Comments_Operation_Is_Invoked_With_Valid_Data_Ok_Response_Is_Returned()
        {
            ValidParametersToAddCaseLockComments();
            AddCaseLockCommentsInvoked();
            AddCaseLockCommentsOperationReturnedOkResponse();
        }

        [TestMethod()]
        [TestCategory(TestCategories.Unit)]
        public void Unlock_Comments_Is_Invoked_With_Invalid_Record_NotFount_Is_Returned()
        {
            InvalidParametersToUnlockComment();
            UnlockCommentWithBatchCorbaInvoked();
            UnlockCommentWithBatchCorbaReturnedNotFoundResponse();
        }

        [TestMethod()]
        [TestCategory(TestCategories.Unit)]
        public void Unlock_Comments_Is_Invoked_With_Valid_Data_Ok_Response_Is_Returned()
        {
            ValidParametersToUnlockComment();
            UnlockCommentWithBatchCorbaInvoked();
            UnlockCommentWithBatchCorbaReturnedOkResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Lpn_Batch_Update_Is_Invoked_With_Valid_Data_Returned_Ok_As_Response()
        {
            ValidInputForMultipleLpnUpdate();
            LpnBatchUpdateInvocation();
            LpnBatchUpdateReturnedOkAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Lpn_Batch_Update_Is_Invoked_With_Valid_Data_Returned_NotFound_As_Response()
        {
            InputForMultipleLpnUpdate();
            LpnBatchUpdateInvocation();
            LpnBatchUpdateReturnedNotFoundAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Lpn_Batch_Update_Is_Invoked_With_Empty_Input_Data_Returned_BadRequest_As_Response()
        {
            EmptyOrNullInputForLpnBatchUpdate();
            LpnBatchUpdateInvocation();
            LpnBatchUpdateReturnedBadRequestAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Batch_Case_Comments_Insertion_Is_Invoked_With_Valid_Data_Returned_Created_As_Response()
        {
            ValidInputForBatchCommentsInsertion();
            BatchCommentsInsertionInvocation();
            BatchCommentsInsertionReturnedCreatedAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Batch_Case_Comments_Insertion_Is_Invoked_With_Valid_Data_Returned_NotFound_As_Response()
        {
            InputForBatchCommentsInsertion();
            BatchCommentsInsertionInvocation();
            BatchCommentsInsertionReturnedNotFoundAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Batch_Case_Comments_Insertion_Is_Invoked_With_Empty_Input_Data_Returned_BadRequest_As_Response()
        {
            EmptyOrNullInputForBatchCommentsInsertion();
            BatchCommentsInsertionInvocation();
            BatchCommentsInsertionReturnedBadRequestAsResponseStatus();
        }
    }
}