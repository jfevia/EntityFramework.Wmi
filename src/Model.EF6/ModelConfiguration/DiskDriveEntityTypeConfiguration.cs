using System.Data.Entity.ModelConfiguration;

namespace Model.EF6.ModelConfiguration
{
    internal sealed class DiskDriveEntityTypeConfiguration : EntityTypeConfiguration<DiskDrive>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="DiskDriveEntityTypeConfiguration" /> class.
        /// </summary>
        public DiskDriveEntityTypeConfiguration()
        {
            ToTable("Win32_DiskDrive");
            HasKey(s => s.SerialNumber);
        }
    }
}