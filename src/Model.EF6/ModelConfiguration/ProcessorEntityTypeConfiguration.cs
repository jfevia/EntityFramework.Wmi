using System.Data.Entity.ModelConfiguration;

namespace Model.EF6.ModelConfiguration
{
    internal sealed class ProcessorEntityTypeConfiguration : EntityTypeConfiguration<Processor>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ProcessorEntityTypeConfiguration" /> class.
        /// </summary>
        public ProcessorEntityTypeConfiguration()
        {
            ToTable("Win32_Processor");
        }
    }
}