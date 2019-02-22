using System.Data.Entity.ModelConfiguration;

namespace Model.EF6.ModelConfiguration
{
    internal sealed class BiosEntityTypeConfiguration : EntityTypeConfiguration<Bios>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BiosEntityTypeConfiguration" /> class.
        /// </summary>
        public BiosEntityTypeConfiguration()
        {
            ToTable("Win32_Bios");
            HasKey(s => s.SerialNumber);
        }
    }
}