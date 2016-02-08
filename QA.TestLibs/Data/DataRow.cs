namespace QA.TestLibs.Data
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Key(string) value(object) storage. Key - column or field name.
    /// </summary>
    public class DataRow
    {
        /// <summary>
        /// NULL object. Represent nonexistent value.
        /// </summary>
        public static NullVal NV = new NullVal();

        private Dictionary<string, object> _values;

        /// <summary>
        /// Default constructor
        /// </summary>
        public DataRow()
        {
            _values = new Dictionary<string, object>();
        }

        /// <summary>
        /// Creare DataRow initilized by specified dictionary values
        /// </summary>
        /// <param name="values">Dictionary to initialize DataRow</param>
        public DataRow(Dictionary<string, object> values)
        {
            _values = new Dictionary<string, object>();
            foreach (var val in values)
            {
                _values.Add(val.Key, val.Value);
            }
        }

        /// <summary>
        /// Add data to the DataRow. If key exists, value will be rewritten.
        /// </summary>
        /// <param name="column">Column of field name</param>
        /// <param name="val">Value</param>
        public void Add(string column, object val)
        {
            if (!_values.ContainsKey(column))
                _values.Add(column, val);
            else
                _values[column] = val;
        }

        /// <summary>
        /// Get or Set value for specified key (column or field name)
        /// </summary>
        /// <param name="column">Column or field name</param>
        /// <returns>Value for specified key</returns>
        public object this[string column]
        {
            get
            {
                if (!_values.ContainsKey(column))
                    return NV;
                return _values[column];
            }
            set
            {
                Add(column, value);
            }
        }
        /// <summary>
        /// Get or Set value for specified key (column or field name)
        /// </summary>
        /// <param name="column">Column or field name</param>
        /// <param name="asString">true if value should be returned as string</param>
        /// <returns>String representation of value</returns>
        public string this[string column, bool asString]
        {
            get
            {
                return this[column] == null ? "null" : this[column].ToString();
            }
        }

        /// <summary>
        /// Return list of column names
        /// </summary>
        public List<string> Columns
        {
            get
            {
                return _values.Keys.ToList();
            }
        }
        /// <summary>
        /// Return list of values
        /// </summary>
        public List<object> Values
        {
            get
            {
                return _values.Values.ToList();
            }
        }

        /// <summary>
        /// Retun list of key value pairs
        /// </summary>
        public List<KeyValuePair<string, object>> Pairs
        {
            get
            {
                return _values.ToList();
            }
        }

        /// <summary>
        /// Clone current DataRow
        /// </summary>
        /// <returns></returns>
        public DataRow Clone()
        {
            var dataRow = new DataRow(_values);
            return dataRow;
        }

        /// <summary>
        /// Represents empty value. ToString returns null.
        /// </summary>
        public class NullVal
        {
            public override string ToString()
            {
                return null;
            }
        }
        
    }
}
