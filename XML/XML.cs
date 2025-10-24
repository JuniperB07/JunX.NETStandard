using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace JunX.NETStandard.XML
{
    /// <summary>
    /// Provides utility methods for loading, reading, and modifying XML configuration files using <see cref="XDocument"/>.
    /// </summary>
    /// <remarks>
    /// This class supports both file-based and in-memory XML operations, typically targeting configuration structures with <c>&lt;add key="..." value="..." /&gt;</c> elements.
    /// Future extensions may include support for nested sections, attribute-based filtering, and schema validation.
    /// </remarks>
    public class JunXML
    {
        private string _configPath;
        private XDocument _doc;

        /// <summary>
        /// Initializes a new instance of the <see cref="JunXML"/> class using the specified configuration file path.
        /// </summary>
        /// <param name="ConfigPath">The full path to the XML configuration file to be loaded and modified.</param>
        /// <remarks>
        /// This constructor sets the internal path reference but does not automatically load the document.
        /// Call <see cref="Load"/> to parse the XML content from the specified path.
        /// </remarks>
        public JunXML(string ConfigPath)
        {
            _configPath = ConfigPath;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="JunXML"/> class using the specified <see cref="XDocument"/>.
        /// </summary>
        /// <param name="Document">An in-memory XML document to be used for reading and modification.</param>
        /// <remarks>
        /// This constructor allows direct manipulation of an existing <see cref="XDocument"/> without relying on file-based loading.
        /// Useful for scenarios involving dynamic XML generation, testing, or runtime configuration overrides.
        /// </remarks>
        public JunXML(XDocument Document)
        {
            _doc = Document;
        }

        /// <summary>
        /// Gets the file path of the XML configuration source associated with this instance.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the full path to the XML configuration file.
        /// </value>
        /// <remarks>
        /// This property is only populated when the instance is constructed using the <c>ConfigPath</c> constructor.
        /// It remains <c>null</c> if the instance was initialized with an <see cref="XDocument"/> instead.
        /// </remarks>
        public string ConfigPath => _configPath;
        /// <summary>
        /// Gets the underlying <see cref="XDocument"/> instance associated with this XML handler.
        /// </summary>
        /// <value>
        /// An <see cref="XDocument"/> representing the loaded or injected XML content.
        /// </value>
        /// <remarks>
        /// This property provides direct access to the in-memory XML structure for advanced querying, transformation, or inspection.
        /// It may be <c>null</c> if the document has not been loaded or initialized.
        /// </remarks>
        public XDocument Document => _doc;

        /// <summary>
        /// Loads the XML document from the configured file path and returns the current <see cref="XML"/> instance.
        /// </summary>
        /// <returns>
        /// The same <see cref="XML"/> instance, allowing fluent chaining of subsequent operations.
        /// </returns>
        /// <remarks>
        /// This method parses the XML content located at <see cref="ConfigPath"/> and assigns it to the <see cref="Document"/> property.
        /// It enables fluent usage patterns such as <c>new XML(path).Load().ReadAdd("key")</c>.
        /// </remarks>
        /// <exception cref="Exception">
        /// Thrown when the XML file cannot be loaded due to I/O errors, invalid format, or access restrictions.
        /// </exception>
        public JunXML Load()
        {
            try
            {
                _doc = XDocument.Load(_configPath);
                return this;
            }
            catch(Exception e)
            {
                throw new Exception("Unable to load XML:\n\n" + e.Message.ToString());
            }
        }
        /// <summary>
        /// Retrieves the value of an <c>&lt;add&gt;</c> element with the specified key from the loaded XML document.
        /// </summary>
        /// <param name="Key">The value of the <c>key</c> attribute to search for.</param>
        /// <returns>
        /// A <see cref="string"/> containing the corresponding <c>value</c> attribute of the matched <c>&lt;add&gt;</c> element.
        /// </returns>
        /// <remarks>
        /// This method searches for an element in the form:
        /// <code>
        /// &lt;add key="..." value="..." /&gt;
        /// </code>
        /// If the element or attribute is missing, an <see cref="Exception"/> is thrown with a descriptive error message.
        /// </remarks>
        /// <exception cref="Exception">
        /// Thrown when the target element or attribute cannot be found or accessed.
        /// </exception>
        public string ReadAdd(string Key)
        {
            try
            {
                return _doc
                .Descendants("add")
                .FirstOrDefault(x => x.Attribute("key").Value == Key)
                .Attribute("value").Value;
            }
            catch(Exception e)
            {
                throw new Exception("Unable to read attribute:\n\n" + e.Message.ToString());
            }
        }
        /// <summary>
        /// Updates the <c>value</c> attribute of an <c>&lt;add&gt;</c> element with the specified key in the loaded XML document.
        /// </summary>
        /// <param name="Key">The value of the <c>key</c> attribute to locate the target element.</param>
        /// <param name="Value">The new value to assign to the <c>value</c> attribute.</param>
        /// <remarks>
        /// This method searches for an element in the form:
        /// <code>
        /// &lt;add key="..." value="..." /&gt;
        /// </code>
        /// If found, it updates the <c>value</c> attribute and saves the document back to <see cref="ConfigPath"/>.
        /// </remarks>
        /// <exception cref="Exception">
        /// Thrown when the XML document cannot be saved due to I/O errors, access restrictions, or invalid path configuration.
        /// </exception>
        public void ChangeAddValue(string Key, string Value)
        {
            XElement target = _doc
                .Descendants("add")
                .FirstOrDefault(x => x.Attribute("key").Value == Key);

            target.SetAttributeValue("value", Value);
            try
            {
                _doc.Save(_configPath);
            }
            catch(Exception e)
            {
                throw new Exception("Unable to change value:\n\n" + e.Message.ToString());
            }
        }
    }
}
