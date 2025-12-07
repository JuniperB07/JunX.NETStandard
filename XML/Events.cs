using System;
using System.Collections.Generic;
using System.Text;

namespace JunX.NETStandard.XML
{
    partial class JunXML
    {
        /// <summary>
        /// Occurs when the value of an XML attribute or element identified by a key has been changed.
        /// </summary>
        /// <remarks>
        /// This event provides change-tracking context via the <see cref="XMLAddValueChangedEventArgs"/> payload,
        /// typically used to respond to configuration updates, trigger validation, or log XML modifications.
        /// </remarks>
        public event EventHandler<XMLAddValueChangedEventArgs> AddValueChanged;

        /// <summary>
        /// Occurs when an XML document or configuration has been successfully loaded.
        /// </summary>
        /// <remarks>
        /// This event is typically used to trigger post-load processing, validation, or initialization routines after XML content becomes available.
        /// It does not carry additional event data and serves as a simple notification hook.
        /// </remarks>
        public event EventHandler XMLLoad;

    }

    /// <summary>
    /// Represents event data for a change in an XML attribute or element value identified by a specific key.
    /// </summary>
    /// <remarks>
    /// This class is used to notify subscribers when a value within an XML structure has been modified.
    /// It provides access to the key, the previous value, and the updated value, enabling change tracking, logging, or validation logic.
    /// </remarks>
    public class XMLAddValueChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the key that identifies the XML attribute or element whose value was changed.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the name or path of the XML node being modified.
        /// </value>
        /// <remarks>
        /// This property provides context for the change, enabling event subscribers to locate and respond to the affected XML entry.
        /// </remarks>
        public string Key { get; }
        /// <summary>
        /// Gets the previous value of the XML attribute or element before the change occurred.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the original value associated with the specified key.
        /// </value>
        /// <remarks>
        /// This property is useful for tracking changes, auditing configuration updates, or validating XML modifications.
        /// </remarks>
        public object OldValue { get; }
        /// <summary>
        /// Gets the updated value of the XML attribute or element after the change occurred.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> representing the new value associated with the specified key.
        /// </value>
        /// <remarks>
        /// This property is useful for responding to configuration updates, validating changes, or logging XML modifications.
        /// </remarks>
        public object NewValue { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XMLAddValueChangedEventArgs"/> class with the specified key and value change details.
        /// </summary>
        /// <param name="SetKey">
        /// The key that identifies the XML attribute or element whose value was changed.
        /// </param>
        /// <param name="SetOldValue">
        /// The previous value associated with the specified key before the change occurred.
        /// </param>
        /// <param name="SetNewValue">
        /// The updated value associated with the specified key after the change occurred.
        /// </param>
        /// <remarks>
        /// This constructor captures the context of an XML value change, enabling event subscribers to respond with full awareness of what was modified.
        /// </remarks>
        public XMLAddValueChangedEventArgs(string SetKey, object SetOldValue, object SetNewValue)
        {
            Key = SetKey;   
            OldValue = SetOldValue;
            NewValue = SetNewValue;
        }
    }
}
