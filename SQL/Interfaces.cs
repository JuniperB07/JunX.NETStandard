using System;
using System.Collections.Generic;
using System.Text;

namespace JunX.NETStandard.SQL
{
    public interface IValidateable
    {
        bool IsValid();
    }
    
    public interface IValueAccessible<T>
    {
        T Value { get; }
    }
}
