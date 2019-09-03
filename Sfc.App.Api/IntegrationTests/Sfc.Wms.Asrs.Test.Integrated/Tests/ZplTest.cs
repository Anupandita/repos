using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;
using TestStack.BDDfy;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests
{
    [TestClass]
    public class ZplTest :ZplFixture
    {
        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void VerifyForOrmtMessageWithActionCodeAddRelease()
        {
            this.Given(x => x.AValidZplRecord())
            .When(x=>x.ZplDataReplace())
            .BDDfy();
        }
    }
}
