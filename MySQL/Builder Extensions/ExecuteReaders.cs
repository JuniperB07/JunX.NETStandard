using JunX.NETStandard.SQLBuilder;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace JunX.NETStandard.MySQL
{
    /// <summary>
    /// Provides MySQL Execution extentions to the SQLBuilder group of classes.
    /// These methods are to be used in conjunction with an instance of DBConnect Class assuming that the instance's internal MySqlConnection is initialized and is open.
    /// </summary>
    public static partial class BuilderExtensions
    {
        /// <summary>
        /// Executes the composed SQL <c>SELECT</c> statement represented by the <see cref="SelectCommand{T}"/> instance using the specified <see cref="DBConnect"/> context.
        /// </summary>
        /// <typeparam name="T">The enum type representing the table schema used in the query.</typeparam>
        /// <param name="SelectCMD">The <see cref="SelectCommand{T}"/> instance containing the SQL query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the reader encounters an error during processing.
        /// </exception>
        public static void ExecuteReader<T>(this SelectCommand<T> SelectCMD, DBConnect DBC)
            where T: Enum
        {
            DBC.CommandText = SelectCMD.ToString();
            DBC.ExecuteReader();
        }
        /// <summary>
        /// Executes the composed SQL <c>SELECT</c> statement represented by the <see cref="SelectCommand{T}"/> instance using the specified <see cref="DBConnect"/> context and a single parameter.
        /// </summary>
        /// <typeparam name="T">The enum type representing the table schema used in the query.</typeparam>
        /// <param name="SelectCMD">The <see cref="SelectCommand{T}"/> instance containing the SQL query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query.</param>
        /// <param name="Parameter">The parameter metadata containing the name and value to bind to the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the reader encounters an error during processing.
        /// </exception>
        public static void ExecuteReader<T>(this SelectCommand<T> SelectCMD, DBConnect DBC, ParametersMetadata Parameter)
            where T: Enum
        {
            DBC.CommandText = SelectCMD.ToString();
            DBC.ExecuteReader(Parameter);
        }
        /// <summary>
        /// Executes the composed SQL <c>SELECT</c> statement represented by the <see cref="SelectCommand{T}"/> instance using the specified <see cref="DBConnect"/> context and a collection of parameters.
        /// </summary>
        /// <typeparam name="T">The enum type representing the table schema used in the query.</typeparam>
        /// <param name="SelectCMD">The <see cref="SelectCommand{T}"/> instance containing the SQL query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query.</param>
        /// <param name="Parameters">A collection of parameter metadata objects, each specifying a name and value to bind to the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the reader encounters an error during processing.
        /// </exception>
        public static void ExecuteReader<T>(this SelectCommand<T> SelectCMD, DBConnect DBC, IEnumerable<ParametersMetadata> Parameters)
            where T: Enum
        {
            DBC.CommandText = SelectCMD.ToString();
            DBC.ExecuteReader(Parameters);
        }

        /// <summary>
        /// Executes the composed SQL <c>SELECT</c> statement represented by the <see cref="SelectCommand{T,J}"/> instance using the specified <see cref="DBConnect"/> context.
        /// </summary>
        /// <typeparam name="T">The primary enum type representing the main table schema.</typeparam>
        /// <typeparam name="J">The secondary enum type representing a joined or related table schema.</typeparam>
        /// <param name="SelectCMD">The <see cref="SelectCommand{T,J}"/> instance containing the SQL query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the reader encounters an error during processing.
        /// </exception>
        public static void ExecuteReader<T,J>(this SelectCommand<T,J> SelectCMD, DBConnect DBC)
            where T: Enum
            where J: Enum
        {
            DBC.CommandText = SelectCMD.ToString();
            DBC.ExecuteReader();
        }
        /// <summary>
        /// Executes the composed SQL <c>SELECT</c> statement represented by the <see cref="SelectCommand{T,J}"/> instance using the specified <see cref="DBConnect"/> context and a single parameter.
        /// </summary>
        /// <typeparam name="T">The primary enum type representing the main table schema.</typeparam>
        /// <typeparam name="J">The secondary enum type representing a joined or related table schema.</typeparam>
        /// <param name="SelectCMD">The <see cref="SelectCommand{T,J}"/> instance containing the SQL query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query.</param>
        /// <param name="Parameter">The parameter metadata containing the name and value to bind to the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the reader encounters an error during processing.
        /// </exception>
        public static void ExecuteReader<T,J>(this SelectCommand<T,J> SelectCMD, DBConnect DBC, ParametersMetadata Parameter)
            where T: Enum
            where J: Enum
        {
            DBC.CommandText = SelectCMD.ToString();
            DBC.ExecuteReader(Parameter);
        }
        /// <summary>
        /// Executes the composed SQL <c>SELECT</c> statement represented by the <see cref="SelectCommand{T,J}"/> instance using the specified <see cref="DBConnect"/> context and a collection of parameters.
        /// </summary>
        /// <typeparam name="T">The primary enum type representing the main table schema.</typeparam>
        /// <typeparam name="J">The secondary enum type representing a joined or related table schema.</typeparam>
        /// <param name="SelectCMD">The <see cref="SelectCommand{T,J}"/> instance containing the SQL query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query.</param>
        /// <param name="Parameters">A collection of parameter metadata objects, each specifying a name and value to bind to the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the reader encounters an error during processing.
        /// </exception>
        public static void ExecuteReader<T,J>(this SelectCommand<T,J> SelectCMD, DBConnect DBC, IEnumerable<ParametersMetadata> Parameters)
            where T: Enum
            where J: Enum
        {
            DBC.CommandText = SelectCMD.ToString();
            DBC.ExecuteReader(Parameters);
        }

        /// <summary>
        /// Executes the composed SQL <c>SELECT</c> statement represented by the <see cref="SelectCommand"/> instance using the specified <see cref="DBConnect"/> context.
        /// </summary>
        /// <param name="SelectCMD">The <see cref="SelectCommand"/> instance containing the SQL query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the reader encounters an error during processing.
        /// </exception>
        public static void ExecuteReader(this SelectCommand SelectCMD, DBConnect DBC)
        {
            DBC.CommandText = SelectCMD.ToString();
            DBC.ExecuteReader();
        }
        /// <summary>
        /// Executes the composed SQL <c>SELECT</c> statement represented by the <see cref="SelectCommand"/> instance using the specified <see cref="DBConnect"/> context and a single parameter.
        /// </summary>
        /// <param name="SelectCMD">The <see cref="SelectCommand"/> instance containing the SQL query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query.</param>
        /// <param name="Parameter">The parameter metadata containing the name and value to bind to the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the reader encounters an error during processing.
        /// </exception>
        public static void ExecuteReader(this SelectCommand SelectCMD, DBConnect DBC, ParametersMetadata Parameter)
        {
            DBC.CommandText = SelectCMD.ToString();
            DBC.ExecuteReader(Parameter);
        }
        /// <summary>
        /// Executes the composed SQL <c>SELECT</c> statement represented by the <see cref="SelectCommand"/> instance using the specified <see cref="DBConnect"/> context and a collection of parameters.
        /// </summary>
        /// <param name="SelectCMD">The <see cref="SelectCommand"/> instance containing the SQL query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query.</param>
        /// <param name="Parameters">A collection of parameter metadata objects, each specifying a name and value to bind to the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the reader encounters an error during processing.
        /// </exception>
        public static void ExecuteReader(this SelectCommand SelectCMD, DBConnect DBC, IEnumerable<ParametersMetadata> Parameters)
        {
            DBC.CommandText = SelectCMD.ToString();
            DBC.ExecuteReader(Parameters);
        }
    }
}