using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Model.EF6.Tests
{
    [TestClass]
    public sealed class ProcessorsTests
    {
        [TestMethod]
        public void ToList()
        {
            using (var context = new WMIContext("name=WMIEntities"))
            {
                var processors = context.Processors.ToList();
                Assert.AreEqual(1, processors.Count);
            }
        }
    }
}