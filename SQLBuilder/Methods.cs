using System;
using System.Collections.Generic;
using System.Text;

namespace JunX.NETStandard.SQLBuilder
{
    internal static class Methods
    {
        internal static string SQLSafeValue(string Value, DataTypes DataType)
        {
            if (DataType == DataTypes.NonNumeric)
                return "'" + Value + "'";
            else
                return Value;
        }
    }
}
