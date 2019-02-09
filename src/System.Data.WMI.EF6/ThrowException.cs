namespace System.Data.WMI.EF6
{
    internal class ThrowException
    {
        public static void IfNull<T>(T value, string parameterName) where T : class
        {
            if (value == null)
                throw new ArgumentNullException(parameterName);
        }

        public static void IfNull<T>(T? value, string parameterName) where T : struct
        {
            if (value == null)
                throw new ArgumentNullException(parameterName);
        }

        public static void IfNullNorEmpty(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(parameterName);
        }
    }
}