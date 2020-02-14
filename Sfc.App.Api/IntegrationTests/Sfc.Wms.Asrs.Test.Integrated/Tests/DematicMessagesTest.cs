using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sfc.Wms.Api.Asrs.Test.Integrated.Fixtures;
using Sfc.Wms.Api.Asrs.Test.Integrated.TestData;
using Sfc.Wms.Interfaces.ParserAndTranslator.Contracts.Constants;
using TestStack.BDDfy;

namespace Sfc.Wms.Api.Asrs.Test.Integrated.Tests
{
    [TestClass]
    [Story(
             )]

    public class DematicMessagesTest:DematicTest
    {
        [TestMethod()]
        [TestCategory("FUNCTIONAL")]
        public void TestData()
        {
            DematicTestProcessFlow();
        }

    }
}
