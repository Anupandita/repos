namespace Sfc.Wms.Api.Asrs.Test.Integrated.TestData.Queries.UIQueries
{
    public class MessageMasterQueries
    {
        public const string MsgMasterSql = "select mm.module,mm.msg_id messageId,md.lang_id languageId,md.msg message,md.short_msg shortMessage from msg_master mm inner " +
                              "join msg_dtl md on md.module=mm.module and md.msg_id = mm.msg_id where mm.prt_indic='C'";
    }
}
