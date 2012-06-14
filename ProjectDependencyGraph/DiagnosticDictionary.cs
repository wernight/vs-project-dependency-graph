using System.Collections.Generic;

namespace Clifton.Collections.Generic
{
    /// <summary>
    /// A dictionary with an indexer that produces an informative KeyNotFoundException message.
    /// </summary>
    public class DiagnosticDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        private string _name = "unknown";

        /// <summary>
        /// Parameterless constructor.
        /// </summary>
        public DiagnosticDictionary()
        {
        }

        /// <summary>
        /// Constructor that takes a name.
        /// </summary>
        public DiagnosticDictionary(string name)
        {
            _name = name;
        }

        /// <summary>
        /// Gets/sets an object that you can associate with the dictionary.
        /// </summary>
        public object Tag { get; set; }

        /// <summary>
        /// The dictionary name.  The default is "unknown".  Used to enhance the KeyNotFoundException.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Indexer that produces a more useful KeyNotFoundException.
        /// </summary>
        public new TValue this[TKey key]
        {
            get
            {
                try
                {
                    return base[key];
                }
                catch (KeyNotFoundException)
                {
                    throw new KeyNotFoundException("The key '" + key + "' was not found in the dictionary '" + _name +
                                                   "'");
                }
            }

            set { base[key] = value; }
        }
    }
}