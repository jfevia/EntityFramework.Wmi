using System.Data.Entity.ModelConfiguration;

namespace Model.EF6.ModelConfiguration
{
    internal sealed class DesktopMonitorEntityTypeConfiguration : EntityTypeConfiguration<DesktopMonitor>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DesktopMonitorEntityTypeConfiguration" /> class.
        /// </summary>
        public DesktopMonitorEntityTypeConfiguration()
        {
            ToTable("Win32_DesktopMonitor");
            HasKey(s => s.DeviceID);
        }
    }
}