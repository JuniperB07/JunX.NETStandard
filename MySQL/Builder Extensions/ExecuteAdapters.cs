using JunX.NETStandard.SQLBuilder;
using Org.BouncyCastle.Crypto.Engines;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;

namespace JunX.NETStandard.MySQL
{
    public static partial class BuilderExtensions
    {
        /// <summary>
        /// Executes the composed SQL <c>SELECT</c> statement represented by the <see cref="SelectCommand{T}"/> instance using the specified <see cref="DBConnect"/> context and returns the result via a data adapter.
        /// </summary>
        /// <typeparam name="T">The enum type representing the table schema used in the query.</typeparam>
        /// <param name="SelectCMD">The <see cref="SelectCommand{T}"/> instance containing the SQL query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query and managing the adapter logic.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the adapter encounters an error during processing.
        /// </exception>
        public static void ExecuteAdapter<T>(this SelectCommand<T> SelectCMD, DBConnect DBC)
            where T: Enum
        {
            DBC.CommandText = SelectCMD.ToString();
            DBC.ExecuteAdapter();
        }
        /// <summary>
        /// Executes the composed SQL <c>SELECT</c> statement represented by the <see cref="SelectCommand{T}"/> instance using the specified <see cref="DBConnect"/> context and a single parameter, returning the result via a data adapter.
        /// </summary>
        /// <typeparam name="T">The enum type representing the table schema used in the query.</typeparam>
        /// <param name="SelectCMD">The <see cref="SelectCommand{T}"/> instance containing the SQL query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query and managing the adapter logic.</param>
        /// <param name="Parameter">The parameter metadata containing the name and value to bind to the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the adapter encounters an error during processing.
        /// </exception>
        public static void ExecuteAdapter<T>(this SelectCommand<T> SelectCMD, DBConnect DBC, ParametersMetadata Parameter)
            where T: Enum
        {
            DBC.CommandText = SelectCMD.ToString();
            DBC.ExecuteAdapter(Parameter);
        }
        /// <summary>
        /// Executes the composed SQL <c>SELECT</c> statement represented by the <see cref="SelectCommand{T}"/> instance using the specified <see cref="DBConnect"/> context and a collection of parameters, returning the result via a data adapter.
        /// </summary>
        /// <typeparam name="T">The enum type representing the table schema used in the query.</typeparam>
        /// <param name="SelectCMD">The <see cref="SelectCommand{T}"/> instance containing the SQL query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query and managing the adapter logic.</param>
        /// <param name="Parameters">A collection of parameter metadata objects, each specifying a name and value to bind to the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the adapter encounters an error during processing.
        /// </exception>
        public static void ExecuteAdapter<T>(this SelectCommand<T> SelectCMD, DBConnect DBC, IEnumerable<ParametersMetadata> Parameters)
            where T: Enum
        {
            DBC.CommandText = SelectCMD.ToString();
            DBC.ExecuteAdapter(Parameters);
        }

        /// <summary>
        /// Executes the composed SQL <c>SELECT</c> statement represented by the <see cref="SelectCommand{T,J}"/> instance using the specified <see cref="DBConnect"/> context and returns the result via a data adapter.
        /// </summary>
        /// <typeparam name="T">The primary enum type representing the main table schema.</typeparam>
        /// <typeparam name="J">The secondary enum type representing a joined or related table schema.</typeparam>
        /// <param name="SelectCMD">The <see cref="SelectCommand{T,J}"/> instance containing the SQL query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query and managing the adapter logic.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the adapter encounters an error during processing.
        /// </exception>
        public static void ExecuteAdapter<T,J>(this SelectCommand<T,J> SelectCMD, DBConnect DBC)
            where T: Enum
            where J: Enum
        {
            DBC.CommandText = SelectCMD.ToString();
            DBC.ExecuteAdapter();
        }
        /// <summary>
        /// Executes the composed SQL <c>SELECT</c> statement represented by the <see cref="SelectCommand{T,J}"/> instance using the specified <see cref="DBConnect"/> context and a single parameter, returning the result via a data adapter.
        /// </summary>
        /// <typeparam name="T">The primary enum type representing the main table schema.</typeparam>
        /// <typeparam name="J">The secondary enum type representing a joined or related table schema.</typeparam>
        /// <param name="SelectCMD">The <see cref="SelectCommand{T,J}"/> instance containing the SQL query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query and managing the adapter logic.</param>
        /// <param name="Parameter">The parameter metadata containing the name and value to bind to the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the adapter encounters an error during processing.
        /// </exception>
        public static void ExecuteAdapter<T,J>(this SelectCommand<T,J> SelectCMD, DBConnect DBC, ParametersMetadata Parameter)
            where T: Enum
            where J: Enum
        {
            DBC.CommandText = SelectCMD.ToString();
            DBC.ExecuteAdapter(Parameter);
        }
        /// <summary>
        /// Executes the composed SQL <c>SELECT</c> statement represented by the <see cref="SelectCommand{T,J}"/> instance using the specified <see cref="DBConnect"/> context and a collection of parameters, returning the result via a data adapter.
        /// </summary>
        /// <typeparam name="T">The primary enum type representing the main table schema.</typeparam>
        /// <typeparam name="J">The secondary enum type representing a joined or related table schema.</typeparam>
        /// <param name="SelectCMD">The <see cref="SelectCommand{T,J}"/> instance containing the SQL query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query and managing the adapter logic.</param>
        /// <param name="Parameters">A collection of parameter metadata objects, each specifying a name and value to bind to the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the adapter encounters an error during processing.
        /// </exception>
        public static void ExecuteAdapter<T,J>(this SelectCommand<T,J> SelectCMD, DBConnect DBC, IEnumerable<ParametersMetadata> Parameters)
            where T: Enum
            where J: Enum
        {
            DBC.CommandText = SelectCMD.ToString();
            DBC.ExecuteAdapter(Parameters);
        }

        /// <summary>
        /// Executes the composed SQL <c>SELECT</c> statement represented by the <see cref="SelectCommand"/> instance using the specified <see cref="DBConnect"/> context and returns the result via a data adapter.
        /// </summary>
        /// <param name="SCMD">The <see cref="SelectCommand"/> instance containing the SQL query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query and managing the adapter logic.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the adapter encounters an error during processing.
        /// </exception>
        public static void ExecuteAdapter(this SelectCommand SCMD, DBConnect DBC)
        {
            DBC.CommandText = SCMD.ToString();
            DBC.ExecuteAdapter();
        }
        /// <summary>
        /// Executes the composed SQL <c>SELECT</c> statement represented by the <see cref="SelectCommand"/> instance using the specified <see cref="DBConnect"/> context and a single parameter, returning the result via a data adapter.
        /// </summary>
        /// <param name="SCMD">The <see cref="SelectCommand"/> instance containing the SQL query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query and managing the adapter logic.</param>
        /// <param name="Parameter">The parameter metadata containing the name and value to bind to the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the adapter encounters an error during processing.
        /// </exception>
        public static void ExecuteAdapter(this SelectCommand SCMD, DBConnect DBC, ParametersMetadata Parameter)
        {
            DBC.CommandText = SCMD.ToString();
            DBC.ExecuteAdapter(Parameter);
        }
        /// <summary>
        /// Executes the composed SQL <c>SELECT</c> statement represented by the <see cref="SelectCommand"/> instance using the specified <see cref="DBConnect"/> context and a collection of parameters, returning the result via a data adapter.
        /// </summary>
        /// <param name="SCMD">The <see cref="SelectCommand"/> instance containing the SQL query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query and managing the adapter logic.</param>
        /// <param name="Parameters">A collection of parameter metadata objects, each specifying a name and value to bind to the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the adapter encounters an error during processing.
        /// </exception>
        public static void ExecuteAdapter(this SelectCommand SCMD, DBConnect DBC, IEnumerable<ParametersMetadata> Parameters)
        {
            DBC.CommandText = SCMD.ToString();
            DBC.ExecuteAdapter(Parameters);
        }
    }
}
