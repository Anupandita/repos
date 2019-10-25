using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.TestData
{
  
    public class DefaultValues
    {
        public const string Status = "Ready";
        public const string ContainerType = "Case";
        public const string DataControl = "F";
        public const string Owner = "Warehouse";
        public const int CaseHdrStatCode = 96;
        public const byte TaskStatusCode = 90;
        public const string CurrentlocnId = "000192818";
        public const string AttributeBitMap = "";
        public const string QuantityToInduct = "1";
        public const string Process = "EMS";
        public const string ActionCodeCost = "Arrival";
        public const string InvalidCase = "00000283000804736790";
        public const string MessageLengthCost = "00268";
        public const string InBoundPallet = "Y";
    }

    public class ResultType
    {
        public const string Created = "Created";
        public const string NotFound = "Conflict";
    }
}
