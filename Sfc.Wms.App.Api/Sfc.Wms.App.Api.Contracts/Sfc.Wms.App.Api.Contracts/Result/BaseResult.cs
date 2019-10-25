using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using Sfc.Wms.App.Api.Contracts.Enums;

namespace Sfc.Wms.App.Api.Contracts.Result
{
    public class BaseResult<T>
    {
        public ResultTypes ResultType { get; set; }

        public T Payload { get; set; }
        public IList<Parameter> Authentication { set; get; }

        public List<ValidationMessage> ValidationMessages = new List<ValidationMessage>();

        public static implicit operator BaseResult<T>(BaseResult<object> v)
        {
            throw new NotImplementedException();
        }
    }
}