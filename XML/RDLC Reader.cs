using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Linq;

namespace JunX.NETStandard.XML
{
    /// <summary>
    /// Reads RDLC files.
    /// </summary>
    public static class RDLCReader
    {
        private static string Sanitize(string name)
        {
            return string.Concat(name
                .Where(x => char.IsLetterOrDigit(x) || x == '_'))
                .Replace(" ", "_");
        }

        /// <summary>
        /// Generates a C# <c>enum</c> file from the list of <c>ReportParameter</c> names defined in an RDLC report.
        /// </summary>
        /// <param name="RDLCPath">The full path to the RDLC file to parse.</param>
        /// <param name="SavePath">The destination path where the generated enum file will be saved.</param>
        /// <remarks>
        /// This method loads the RDLC file as XML, extracts all unique <c>ReportParameter</c> names,
        /// sanitizes them into valid C# identifiers, and writes them as members of a public enum.
        /// The enum name is derived from the exported C# file name (excluding extension).
        /// <para>
        /// Example: If <paramref name="RDLCPath"/> is <c>InvoiceReport.rdlc</c>, the output will be:
        /// <code>
        /// public enum InvoiceReport
        /// {
        ///     StartDate,
        ///     EndDate,
        ///     IncludeLogo
        /// }
        /// </code>
        /// </para>
        /// Each enum member is comma-separated except the last, ensuring valid syntax.
        /// </remarks>
        /// <exception cref="FileNotFoundException">
        /// Thrown when the RDLC file does not exist at the specified <paramref name="RDLCPath"/>.
        /// </exception>
        public static void RDLC_ToEnumFile(string RDLCPath, string SavePath)
        {
            if (!File.Exists(RDLCPath))
                throw new FileNotFoundException("RDLC file not found.", RDLCPath);

            XDocument doc = XDocument.Load(RDLCPath);

            List<string> parameters = doc.Descendants()
                .Where(x => x.Name.LocalName == "ReportParameter")
                .Select(x => x.Attribute("Name")?.Value)
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Distinct()
                .ToList();

            string enumName = Path.GetFileNameWithoutExtension(SavePath);

            using (StreamWriter writer = new StreamWriter(SavePath, false))
            {
                string sanitized = "";
                string comma;

                writer.WriteLine($"public enum {enumName}");
                writer.WriteLine("{");
                
                for(int i = 0; i < parameters.Count; i++)
                {
                    sanitized = Sanitize(parameters[i]);
                    comma = (i < parameters.Count - 1) ? "," : "";

                    writer.WriteLine($"     {sanitized}{comma}");
                }

                writer.WriteLine("}");
            }
        }
    }
}
