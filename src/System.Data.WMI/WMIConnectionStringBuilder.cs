using System.Collections;
using System.ComponentModel;
using System.Data.Common;
using System.Reflection;

namespace System.Data.WMI
{
    /// <summary>
    ///     WMI implementation of DbConnectionStringBuilder.
    /// </summary>
    [DefaultProperty("DataSource")]
    [DefaultMember("Item")]
    public sealed class WMIConnectionStringBuilder : DbConnectionStringBuilder
    {
        /// <summary>
        ///     Properties of this class
        /// </summary>
        private Hashtable _properties;

        /// <overloads>
        ///     Constructs a new instance of the class
        /// </overloads>
        /// <summary>
        ///     Default constructor
        /// </summary>
        public WMIConnectionStringBuilder()
        {
            Initialize(null);
        }

        /// <summary>
        ///     Constructs a new instance of the class using the specified connection string.
        /// </summary>
        /// <param name="connectionString">The connection string to parse</param>
        public WMIConnectionStringBuilder(string connectionString)
        {
            Initialize(connectionString);
        }

        /// <summary>
        ///     Gets or sets the computer.
        /// </summary>
        /// <value>
        ///     The computer.
        /// </value>
        [DisplayName("Computer")]
        [Browsable(true)]
        [DefaultValue("")]
        public string Computer
        {
            get
            {
                TryGetValue("computer", out var value);
                return value?.ToString();
            }
            set => this["computer"] = value;
        }

        /// <summary>
        ///     Gets or sets the namespace.
        /// </summary>
        /// <value>
        ///     The namespace.
        /// </value>
        [DisplayName("Namespace")]
        [Browsable(true)]
        [DefaultValue("")]
        public string Namespace
        {
            get
            {
                TryGetValue("namespace", out var value);
                return value?.ToString();
            }
            set => this["namespace"] = value;
        }

        /// <summary>
        ///     Gets or sets the username.
        /// </summary>
        /// <value>
        ///     The username.
        /// </value>
        [DisplayName("Username")]
        [Browsable(true)]
        [DefaultValue("")]
        public string Username
        {
            get
            {
                TryGetValue("username", out var value);
                return value?.ToString();
            }
            set => this["uername"] = value;
        }

        /// <summary>
        ///     Gets or sets the password.
        /// </summary>
        /// <value>
        ///     The password.
        /// </value>
        [Browsable(true)]
        [PasswordPropertyText(true)]
        [DefaultValue("")]
        public string Password
        {
            get
            {
                TryGetValue("password", out var value);
                return value?.ToString();
            }
            set => this["password"] = value;
        }

        /// <summary>
        ///     Private initializer, which assigns the connection string and resets the builder
        /// </summary>
        /// <param name="cnnString">The connection string to assign</param>
        private void Initialize(string cnnString)
        {
            _properties = new Hashtable(StringComparer.OrdinalIgnoreCase);
            try
            {
                GetProperties(_properties);
            }
            catch (NotImplementedException)
            {
                FallbackGetProperties(_properties);
            }

            if (!string.IsNullOrWhiteSpace(cnnString))
                ConnectionString = cnnString;
        }

        /// <summary>
        ///     Helper function for retrieving values from the connection string
        /// </summary>
        /// <param name="keyword">The keyword to retrieve settings for</param>
        /// <param name="value">The resulting parameter value</param>
        /// <returns>Returns true if the value was found and returned</returns>
        public override bool TryGetValue(string keyword, out object value)
        {
            var result = base.TryGetValue(keyword, out value);

            if (!_properties.ContainsKey(keyword))
                return result;

            if (!(_properties[keyword] is PropertyDescriptor propertyDescriptor))
                return result;

            // Attempt to coerce the value into something more solid
            if (result)
            {
                if (propertyDescriptor.PropertyType == typeof(bool))
                    value = Convert.ToBoolean(value);
                else if (propertyDescriptor.PropertyType != typeof(byte[]))
                    value = TypeDescriptor.GetConverter(propertyDescriptor.PropertyType).ConvertFrom(value);
            }
            else
            {
                if (!(propertyDescriptor.Attributes[typeof(DefaultValueAttribute)] is DefaultValueAttribute att))
                    return false;

                value = att.Value;
            }

            return true;
        }

        /// <summary>
        ///     Fallback method for MONO, which doesn't implement DbConnectionStringBuilder.GetProperties()
        /// </summary>
        /// <param name="propertyList">The hashtable to fill with property descriptors</param>
        private void FallbackGetProperties(Hashtable propertyList)
        {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(this, true))
                if (descriptor.Name != nameof(ConnectionString) && propertyList.ContainsKey(descriptor.DisplayName) == false)
                    propertyList.Add(descriptor.DisplayName, descriptor);
        }
    }
}