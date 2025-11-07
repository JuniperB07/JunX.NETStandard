using System;
using System.Collections.Generic;
using System.Text;

namespace JunX.NETStandard.XML
{
    /// <summary>
    /// Defines a contract for accessing and manipulating XML-based configuration or data sources.
    /// </summary>
    /// <typeparam name="T">
    /// The type returned by the <c>Load</c> method, typically representing a deserialized XML object or document.
    /// </typeparam>
    /// <remarks>
    /// This interface supports structured XML access patterns, including loading the entire document, reading specific attributes or elements,
    /// and modifying values by key. It is suitable for configuration management, metadata-driven systems, or dynamic XML pipelines.
    /// </remarks>
    public interface IXMLAccessible<T>
    {
        T Load();
        string ReadAdd(string Key);
        void ChangeAddValue(string Key, string Value);
    }
}
