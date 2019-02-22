using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Model.EF6.Tests
{
    [TestClass]
    public sealed class SaveChangesTests
    {
        [TestMethod]
        public void SaveChanges()
        {
            using (var context = new WMIContext("name=WMIEntities"))
            {
                context.SaveChanges();
            }
        }

        [TestMethod]
        public async Task SaveChangesAsync()
        {
            using (var context = new WMIContext("name=WMIEntities"))
            {
                await context.SaveChangesAsync();
            }
        }
    }
}
