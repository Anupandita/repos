using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.App.Api.Tests.Unit.Constants;
using Sfc.Wms.App.Api.Tests.Unit.Fixtures;

namespace Sfc.Wms.App.Api.Tests.Unit.Nuget
{
    [TestClass]
    public class LpnGatewayTest : LpnGatewayFixture
    {
        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void LpnSearch_Operation_Returned_Ok_As_Response_Status()
        {
            ValidLpnSearchParametersAsInput();
            LpnSearchOperationInvoked();
            TheLpnSearchOperationReturnedOkAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void LpnSearch_Operation_Returned_BadRequest_As_Response_Status()
        {
            InValidLpnSearchParameters();
            LpnSearchOperationInvoked();
            TheLpnSearchOperationReturnedBadRequestAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetLpnDetails_By_Lpn_Id_Operation_Returned_Ok_As_Response_Status()
        {
            ValidInputParametersToGetLpnDetails();
            GetLpnDetailsOperationInvoked();
            TheGetLpnDetailsOperationReturnedOkAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetLpnDetails_By_Lpn_Id_Operation_Returned_BadRequest_As_Response_Status()
        {
            InValidInputParametersToGetLpnDetails();
            GetLpnDetailsOperationInvoked();
            TheGetLpnDetailsReturnedBadRequestAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void InsertLpnComments_Operation_Returned_Created_As_Response_Status()
        {
            ValidInputParametersToInsertLpnComments();
            InsertLpnCommentsOperationInvoked();
            TheInsertLpnCommentsOperationReturnedCreatedAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void InsertLpnComments_Operation_Returned_BadRequest_As_Response_Status()
        {
            InValidInputParametersToInsertLpnComments();
            InsertLpnCommentsOperationInvoked();
            TheInsertLpnCommentsReturnedBadRequestAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void InsertLpnComments_Operation_Returned_Conflict_As_Response_Status()
        {
            InputParametersToInsertLpnCommentsForWhichRecordExists();
            InsertLpnCommentsOperationInvoked();
            TheInsertLpnCommentsReturnedConflictAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetLpnCommentsByLpnId_Operation_Returned_Ok_As_Response_Status()
        {
            ValidInputParametersToGetLpnComments();
            GetLpnCommentsByLpnIdOperationInvoked();
            TheGetLpnCommentsByLpnIdReturnedOkAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetLpnCommentsByLpnId_Operation_Returned_BadRequest_As_Response_Status()
        {
            InValidInputParametersToGetLpnComments();
            GetLpnCommentsByLpnIdOperationInvoked();
            TheGetLpnCommentsByLpnIdReturnedBadRequestAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetLpnCommentsByLpnId_Operation_Returned_NotFound_As_Response_Status()
        {
            InputParametersToGetLpnCommentsForNoCommentsExists();
            GetLpnCommentsByLpnIdOperationInvoked();
            TheGetLpnCommentsByLpnIdReturnedNotFoundAsResponseStatus();
        }

          [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetLpnHistoryByLpnId_Operation_Returned_Ok_As_Response_Status()
        {
            ValidInputParametersToGetLpnHistory();
            GetLpnHistoryByLpnIdAndWhseOperationInvoked();
            TheGetLpnHistoryByLpnIdAndWhseReturnedOkAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetLpnHistoryByLpnId_Operation_Returned_BadRequest_As_Response_Status()
        {
            InValidInputParametersToGetLpnHistory();
            GetLpnHistoryByLpnIdAndWhseOperationInvoked();
            TheGetLpnHistoryByLpnIdAndWhseReturnedBadRequestAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetLpnHistoryByLpnId_Operation_Returned_NotFound_As_Response_Status()
        {
            InputParametersToGetLpnHistoryForWhichNoCommentsExists();
            GetLpnHistoryByLpnIdAndWhseOperationInvoked();
            TheGetLpnHistoryByLpnIdAndWhseReturnedNotFoundAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetLpnLockUnlockByLpnId_Operation_Returned_Ok_As_Response_Status()
        {
            ValidInputParametersToGetLpnLockUnlockDetails();
            GetLpnLockUnlockByLpnIdOperationInvoked();
            TheGetLpnLockUnlockByLpnIdReturnedOkAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetLpnLockUnlockByLpnId_Operation_Returned_BadRequest_As_Response_Status()
        {
            InValidInputParametersToGetLpnLockUnlockDetails();
            GetLpnLockUnlockByLpnIdOperationInvoked();
            TheGetLpnLockUnlockByLpnIdReturnedBadRequestAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetLpnLockUnlockByLpnId_Operation_Returned_NotFound_As_Response_Status()
        {
            InputParametersToGetLpnLockUnlockByLpnIdForWhichNoDetailsExists();
            GetLpnLockUnlockByLpnIdOperationInvoked();
            TheGetLpnLockUnlockByLpnIdReturnedNotFoundAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetCaseUnLockDetails_Operation_Returned_Ok_As_Response_Status()
        {
            ValidInputParametersToGetCaseUnLockDetails();
            GetCaseUnLockDetailsOperationInvoked();
            TheGetCaseUnLockDetailsReturnedOkAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetCaseUnLockDetails_Operation_Returned_BadRequest_As_Response_Status()
        {
            InValidInputParametersToGetCaseUnLockDetails();
            GetCaseUnLockDetailsOperationInvoked();
            TheGetCaseUnLockDetailsReturnedBadRequestAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void GetCaseUnLockDetails_Operation_Returned_NotFound_As_Response_Status()
        {
            InputParametersToGetCaseUnLockDetailsForWhichNoDetailsExists();
            GetCaseUnLockDetailsOperationInvoked();
            TheGetCaseUnLockDetailsReturnedNotFoundAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void UpdateLpnHeader_Operation_Returned_Ok_As_Response_Status()
        {
            ValidInputParametersToUpdateLpnHeader();
            UpdateLpnHeaderOperationInvoked();
            TheUpdateLpnHeaderReturnedOkAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void UpdateLpnHeader_Operation_Returned_BadRequest_As_Response_Status()
        {
            InValidInputParametersToUpdateLpnHeader();
            UpdateLpnHeaderOperationInvoked();
            TheUpdateLpnHeaderReturnedBadRequestAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void UpdateLpnHeader_Operation_Returned_NotFound_As_Response_Status()
        {
            InputParametersToUpdateLpnHeaderForWhichNoDetailsExists();
            UpdateLpnHeaderOperationInvoked();
            TheUpdateLpnHeaderReturnedNotFoundAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void DeleteLpnComment_Operation_Returned_Ok_As_Response_Status()
        {
            ValidInputParametersToDeleteLpnComment();
            DeleteLpnCommentOperationInvoked();
            TheDeleteLpnCommentReturnedOkAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void DeleteLpnComment_Operation_Returned_BadRequest_As_Response_Status()
        {
            InValidInputParametersToDeleteLpnComment();
            DeleteLpnCommentOperationInvoked();
            TheDeleteLpnCommentReturnedBadRequestAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void DeleteLpnComment_Operation_Returned_NotFound_As_Response_Status()
        {
            InputParametersToDeleteLpnCommentForWhichNoDetailsExists();
            DeleteLpnCommentOperationInvoked();
            TheDeleteLpnCommentReturnedNotFoundAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void UpdateLpnDetail_Operation_Returned_Ok_As_Response_Status()
        {
            ValidInputParametersToUpdateLpnDetail();
            UpdateLpnDetailOperationInvoked();
            TheUpdateLpnDetailReturnedOkAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void UpdateLpnDetail_Operation_Returned_BadRequest_As_Response_Status()
        {
            InValidInputParametersToUpdateLpnDetail();
            UpdateLpnDetailOperationInvoked();
            TheUpdateLpnDetailReturnedBadRequestAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void UpdateLpnDetail_Operation_Returned_NotFound_As_Response_Status()
        {
            InputParametersToUpdateLpnDetailForWhichNoDetailsExists();
            UpdateLpnDetailOperationInvoked();
            TheUpdateLpnDetailReturnedNotFoundAsResponseStatus();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Add_Lock_Comments_Operation_Returned_Ok_As_Response_Status()
        {
            ValidParametersToAddCaseLockComments();
            AddCaseLockCommentsInvoked();
            AddCaseLockCommentsReturnedOkResponse();
        }

        [TestMethod]
        [TestCategory(TestCategories.Unit)]
        public void Add_Lock_Comments_Operation_Returned_NotFound_As_Response_Status()
        {
            InvalidParametersToAddCaseLockComments();
            AddCaseLockCommentsInvoked();
            AddCaseLockCommentsReturnedNotFoundResponse();
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

    }
}
