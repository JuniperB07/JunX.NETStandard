using System;
using System.Collections.Generic;
using System.Text;

namespace JunX.NETStandard.XML
{
    partial class JunXML
    {
        /// <summary>
        /// Occurs when an XML document or configuration has been successfully loaded.
        /// </summary>
        /// <remarks>
        /// This event is typically used to trigger post-load processing, validation, or initialization routines after XML content becomes available.
        /// It does not carry additional event data and serves as a simple notification hook.
        /// </remarks>
        public event EventHandler XMLLoad;
    }
}
