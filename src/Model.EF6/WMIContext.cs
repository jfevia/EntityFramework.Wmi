using System.Data.Entity;

namespace Model.EF6
{
    public class WMIContext : DbContext
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="WMIContext" /> class.
        /// </summary>
        /// <param name="nameOrConnectionString">Either the database name or a connection string.</param>
        public WMIContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            // Disable migrations
            Database.SetInitializer<WMIContext>(null);
        }

        /// <summary>
        ///     Gets or sets the processors.
        /// </summary>
        /// <value>
        ///     The processors.
        /// </value>
        public DbSet<Processor> Processors { get; set; }

        /// <summary>
        ///     Gets or sets the base boards.
        /// </summary>
        /// <value>
        ///     The base boards.
        /// </value>
        public DbSet<BaseBoard> BaseBoards { get; set; }

        /// <summary>
        ///     Gets or sets the bioses.
        /// </summary>
        /// <value>
        ///     The bioses.
        /// </value>
        public DbSet<Bios> Bioses { get; set; }

        /// <summary>
        ///     Gets or sets the desktop monitors.
        /// </summary>
        /// <value>
        ///     The desktop monitors.
        /// </value>
        public DbSet<DesktopMonitor> DesktopMonitors { get; set; }

        /// <summary>
        ///     Gets or sets the disk drives.
        /// </summary>
        /// <value>
        ///     The disk drives.
        /// </value>
        public DbSet<DiskDrive> DiskDrives { get; set; }

        /// <summary>
        ///     This method is called when the model for a derived context has been initialized, but
        ///     before the model has been locked down and used to initialize the context.  The default
        ///     implementation of this method does nothing, but it can be overridden in a derived class
        ///     such that the model can be further configured before it is locked down.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        /// <remarks>
        ///     Typically, this method is called only once when the first instance of a derived context
        ///     is created.  The model for that context is then cached and is for all further instances of
        ///     the context in the app domain.  This caching can be disabled by setting the ModelCaching
        ///     property on the given ModelBuidler, but note that this can seriously degrade performance.
        ///     More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        ///     classes directly.
        /// </remarks>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(GetType().Assembly);
        }
    }
}