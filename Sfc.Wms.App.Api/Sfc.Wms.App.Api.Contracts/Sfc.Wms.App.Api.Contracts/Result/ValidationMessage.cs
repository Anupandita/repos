namespace Wms.App.Contracts.Result
{
    public class ValidationMessage
    {
        public string FieldName;
        public string Message;

        public ValidationMessage(string fieldName, string message)
        {
            FieldName = fieldName;
            Message = message;
        }
    }
}