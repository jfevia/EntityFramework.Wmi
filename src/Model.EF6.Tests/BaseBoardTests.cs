using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Model.EF6.Tests
{
    [TestClass]
    public sealed class BaseBoardTests
    {
        [TestMethod]
        public void ToList()
        {
            using (var context = new WMIContext("name=WMIEntities"))
            {
                var baseBoards = context.BaseBoards.ToList();
                Assert.AreEqual(1, baseBoards.Count);
            }
        }
    }
}