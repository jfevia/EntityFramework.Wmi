using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Model.EF6.Tests
{
    [TestClass]
    public sealed class DiskDriveTests
    {
        [TestMethod]
        public void ToList()
        {
            using (var context = new WMIContext("name=WMIEntities"))
            {
                var diskDrives = context.DiskDrives.ToList();
                Assert.AreEqual(1, diskDrives.Count);
            }
        }
    }
}