using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Model.EF6.Tests
{
    [TestClass]
    public sealed class BiosTests
    {
        [TestMethod]
        public void ToList()
        {
            using (var context = new WMIContext("name=WMIEntities"))
            {
                var bioses = context.Bioses.ToList();
                Assert.AreEqual(1, bioses.Count);
            }
        }
    }
}