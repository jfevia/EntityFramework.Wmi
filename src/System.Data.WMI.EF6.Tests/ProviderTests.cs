using System.Configuration;
using System.Data.Common;
using System.Data.Entity.Core.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Data.WMI.EF6.Tests
{
    [TestClass]
    public class ProviderTests
    {
        protected const string ProviderName = "System.Data.WMI.EF6";
        protected static readonly string DirectConnectionString = ConfigurationManager.ConnectionStrings["WMIEntitiesDirect"].ConnectionString;
        protected static readonly string MetadataConnectionString = ConfigurationManager.ConnectionStrings["WMIEntities"].ConnectionString;

        [TestMethod]
        public void Verify_provider_can_be_created_with_factory()
        {
            Assert.IsNotNull(DbProviderFactories.GetFactory(ProviderName));
        }

        [TestMethod]
        public void Verify_factory_created_by_provider_factories_and_connection_are_same()
        {
            var providerFactory = DbProviderFactories.GetFactory(ProviderName);
            Assert.IsNotNull(providerFactory);
            Assert.AreEqual(typeof(WMIProviderFactory), providerFactory.GetType());

            var providerFactoryFromConnection = ((WMIConnection)providerFactory.CreateConnection()).ProviderFactory;
            Assert.AreEqual(providerFactory.GetType(), providerFactoryFromConnection.GetType());
        }

        [TestMethod]
        public void Verify_SampleCommand_implements_ICloneable()
        {
            var providerFactory = DbProviderFactories.GetFactory(ProviderName);
            Assert.IsNotNull(providerFactory);

            var command = providerFactory.CreateCommand();

            var cloneable = command as ICloneable;
            Assert.IsNotNull(cloneable);

            var clonedCommand = cloneable.Clone();
            Assert.IsNotNull(cloneable);
        }

        [TestMethod]
        public void Verify_provider_supports_DbProviderServices()
        {
            var providerFactory = DbProviderFactories.GetFactory(ProviderName);
            Assert.IsNotNull(providerFactory);

            var serviceprovider = providerFactory as IServiceProvider;
            Assert.IsNotNull(serviceprovider);

            Assert.IsNotNull(serviceprovider.GetService(typeof(DbProviderServices)));
        }

        [TestMethod]
        public void Verify_provider_services_returns_provider_manifest()
        {
            var factory = DbProviderFactories.GetFactory(ProviderName);
            var providerServices = (DbProviderServices)((IServiceProvider)factory).GetService(typeof(DbProviderServices));
            var providerManifest = providerServices.GetProviderManifest("2005");
            Assert.IsNotNull(providerManifest);
        }

        [TestMethod]
        public void Verify_provider_manifest_token_returned_by_provider_services_is_correct()
        {
            var factory = DbProviderFactories.GetFactory(ProviderName);

            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = DirectConnectionString;

                var providerServices = (DbProviderServices)((IServiceProvider)factory).GetService(typeof(DbProviderServices));
                var providerManifestToken = providerServices.GetProviderManifestToken(connection);
                Assert.IsTrue(providerManifestToken == "2005" || providerManifestToken == "2008");
            }
        }
    }
}
