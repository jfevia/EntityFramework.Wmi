using System.Globalization;
using System.IO;
using System.Text;

namespace System.Data.WMI.EF6.Gen
{
    /// <summary>
    ///     This extends StringWriter primarily to add the ability to add an indent
    ///     to each line that is written out.
    /// </summary>
    internal class SqlWriter : StringWriter
    {
        private bool _atBeginningOfLine = true;

        /// <summary>
        /// </summary>
        /// <param name="b"></param>
        public SqlWriter(StringBuilder b)
            : base(b, CultureInfo.InvariantCulture)
        {
        }

        /// <summary>
        ///     The number of tabs to be added at the beginning of each new line.
        /// </summary>
        internal int Indent { get; set; } = -1;

        /// <summary>
        ///     Reset atBeginningOfLine if we detect the newline string.
        ///     <see cref="SqlBuilder.AppendLine" />
        ///     Add as many tabs as the value of indent if we are at the
        ///     beginning of a line.
        /// </summary>
        /// <param name="value"></param>
        public override void Write(string value)
        {
            if (value == "\r\n")
            {
                base.WriteLine();
                _atBeginningOfLine = true;
            }
            else
            {
                if (_atBeginningOfLine)
                {
                    if (Indent > 0) base.Write(new string('\t', Indent));
                    _atBeginningOfLine = false;
                }

                base.Write(value);
            }
        }

        /// <summary>
        /// </summary>
        public override void WriteLine()
        {
            base.WriteLine();
            _atBeginningOfLine = true;
        }
    }
}