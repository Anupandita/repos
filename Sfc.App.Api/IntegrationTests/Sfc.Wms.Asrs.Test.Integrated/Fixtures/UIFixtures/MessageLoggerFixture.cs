using Newtonsoft.Json;
using Sfc.Core.OnPrem.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sfc.Wms.Configuration.MessageLogger.Contracts.Dtos;
using System.Data;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures.UIFixtures
{
  public  class MessageLoggerFixture : BaseFixture
    {

        MessageLogDto messageLogDto;
        protected DataTable MessageLoggerDt = new DataTable();
        public void CreateInputDtoForMessageLoggerApi()
        {
            messageLogDto = new MessageLogDto()
            { Module = "",
              MessageId = "",
              LogDateTime = System.DateTime.Today
                      };

        }

        public void CallMessageLoggerApiWithUrl(string url)
        {
            var input = JsonConvert.SerializeObject(messageLogDto);
            var response = CallPostApi(url, input);
            VerifyOkResultAndStoreBearerToken(response);
        }

        public void VerifyLogInsteredInMessageLogTableInDb()
        { 

        }
    }
}
