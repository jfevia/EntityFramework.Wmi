using System.Data.Entity.ModelConfiguration;

namespace Model.EF6.ModelConfiguration
{
    internal sealed class BaseBoardEntityTypeConfiguration : EntityTypeConfiguration<BaseBoard>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseBoardEntityTypeConfiguration" /> class.
        /// </summary>
        public BaseBoardEntityTypeConfiguration()
        {
            ToTable("Win32_BaseBoard");
            HasKey(s => s.SerialNumber);
        }
    }
}