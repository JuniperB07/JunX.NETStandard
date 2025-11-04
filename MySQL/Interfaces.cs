using System;
using System.Collections.Generic;
using System.Text;

namespace JunX.NETStandard.MySQL
{
    public interface IExecutableQuery
    {
        void ExecuteNonQuery();
        void ExecuteNonQuery(ParametersMetadata Parameter);
        void ExecuteNonQuery(IEnumerable<ParametersMetadata> Parameters);
    }

    public interface IExecutableReader
    {
        void ExecuteReader();
        void ExecuteReader(ParametersMetadata Parameter);
        void ExecuteReader(IEnumerable<ParametersMetadata> Parameters);
    }

    public interface IExecutableAdapter
    {
        void ExecuteAdapter();
        void ExecuteAdapter(ParametersMetadata Parameter);
        void ExecuteAdapter(IEnumerable<ParametersMetadata> Parameters);
    }

    public interface IExecutableDataSet
    {
        void ExecuteDataSet();
        void ExecuteDataSet(ParametersMetadata Parameter);
        void ExecuteDataSet(IEnumerable<ParametersMetadata> Parameters);
    }

    public interface IValuesReadable
    {
        List<string> Values { get; }
    }
}
