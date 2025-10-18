using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace JunX.NETStandard.Utility
{
    public static class Utility
    {
        /// <summary>
        /// Converts a 6-digit hexadecimal color string into a <see cref="Color"/> instance usable in WinForms.
        /// </summary>
        /// <param name="Hex">A hex color string with or without a leading <c>#</c>, such as <c>#FF5733</c> or <c>FF5733</c>.</param>
        /// <returns>A <see cref="Color"/> object representing the RGB equivalent of the hex value.</returns>
        /// <exception cref="ArgumentException">Thrown when the input is null, empty, or not a valid 6-digit hex color.</exception>
        /// <remarks>
        /// This method is intended for use in Windows Forms applications targeting .NET 8.0 with <c>System.Drawing</c> support.
        /// It does not support shorthand hex formats (e.g., <c>#FFF</c>) or alpha channels. For extended color parsing,
        /// consider adding support for 8-digit hex values or using a dedicated color utility library.
        /// </remarks>
        public static Color HexToRGB(string Hex)
        {
            if (string.IsNullOrWhiteSpace(Hex))
                throw new ArgumentException("Invalid Hex value.");

            Hex = Hex.Trim('#');

            if (Hex.Length != 6)
                throw new ArgumentException("Invalid Hex value.");

            int R = Convert.ToInt32(Hex.Substring(0, 2), 16);
            int G = Convert.ToInt32(Hex.Substring(2, 2), 16);
            int B = Convert.ToInt32(Hex.Substring(4, 2), 16);

            return Color.FromArgb(R, G, B);
        }
    }
}
