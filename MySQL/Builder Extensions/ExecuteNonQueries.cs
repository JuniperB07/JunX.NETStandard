using JunX.NETStandard.SQLBuilder;
using System;
using System.Collections.Generic;
using System.Text;

namespace JunX.NETStandard.MySQL
{
    public static partial class BuilderExtensions
    {
        /// <summary>
        /// Executes the composed SQL <c>DELETE</c> statement represented by the <see cref="DeleteCommand{T}"/> instance using the specified <see cref="DBConnect"/> context.
        /// </summary>
        /// <typeparam name="T">The enum type representing the table schema used in the delete operation.</typeparam>
        /// <param name="DCMD">The <see cref="DeleteCommand{T}"/> instance containing the SQL delete query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the command encounters an error during processing.
        /// </exception>
        public static void ExecuteNonQuery<T>(this DeleteCommand<T> DCMD, DBConnect DBC)
            where T: Enum
        {
            DBC.CommandText = DCMD.ToString();
            DBC.ExecuteNonQuery();
        }
        /// <summary>
        /// Executes the composed SQL <c>DELETE</c> statement represented by the <see cref="DeleteCommand{T}"/> instance using the specified <see cref="DBConnect"/> context and a single parameter.
        /// </summary>
        /// <typeparam name="T">The enum type representing the table schema used in the delete operation.</typeparam>
        /// <param name="DCMD">The <see cref="DeleteCommand{T}"/> instance containing the SQL delete query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query.</param>
        /// <param name="Parameter">The parameter metadata containing the name and value to bind to the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the command encounters an error during processing.
        /// </exception>
        public static void ExecuteNonQuery<T>(this DeleteCommand<T> DCMD, DBConnect DBC, ParametersMetadata Parameter)
            where T: Enum
        {
            DBC.CommandText = DCMD.ToString();
            DBC.ExecuteNonQuery(Parameter);
        }
        /// <summary>
        /// Executes the composed SQL <c>DELETE</c> statement represented by the <see cref="DeleteCommand{T}"/> instance using the specified <see cref="DBConnect"/> context and a collection of parameters.
        /// </summary>
        /// <typeparam name="T">The enum type representing the table schema used in the delete operation.</typeparam>
        /// <param name="DCMD">The <see cref="DeleteCommand{T}"/> instance containing the SQL delete query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query.</param>
        /// <param name="Parameters">A collection of parameter metadata objects, each specifying a name and value to bind to the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the command encounters an error during processing.
        /// </exception>
        public static void ExecuteNonQuery<T>(this DeleteCommand<T> DCMD, DBConnect DBC, IEnumerable<ParametersMetadata> Parameters)
            where T: Enum
        {
            DBC.CommandText = DCMD.ToString();
            DBC.ExecuteNonQuery(Parameters);
        }
        /// <summary>
        /// Executes the composed SQL <c>DELETE</c> statement represented by the <see cref="DeleteCommand"/> instance using the specified <see cref="DBConnect"/> context.
        /// </summary>
        /// <param name="DCMD">The <see cref="DeleteCommand"/> instance containing the SQL delete query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the command encounters an error during processing.
        /// </exception>
        public static void ExecuteNonQuery(this DeleteCommand DCMD, DBConnect DBC)
        {
            DBC.CommandText = DCMD.ToString();
            DBC.ExecuteNonQuery();
        }
        /// <summary>
        /// Executes the composed SQL <c>DELETE</c> statement represented by the <see cref="DeleteCommand"/> instance using the specified <see cref="DBConnect"/> context and a single parameter.
        /// </summary>
        /// <param name="DCMD">The <see cref="DeleteCommand"/> instance containing the SQL delete query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query.</param>
        /// <param name="Parameter">The parameter metadata containing the name and value to bind to the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the command encounters an error during processing.
        /// </exception>
        public static void ExecuteNonQuery(this DeleteCommand DCMD, DBConnect DBC, ParametersMetadata Parameter)
        {
            DBC.CommandText = DCMD.ToString();
            DBC.ExecuteNonQuery(Parameter);
        }
        /// <summary>
        /// Executes the composed SQL <c>DELETE</c> statement represented by the <see cref="DeleteCommand"/> instance using the specified <see cref="DBConnect"/> context and a collection of parameters.
        /// </summary>
        /// <param name="DCMD">The <see cref="DeleteCommand"/> instance containing the SQL delete query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query.</param>
        /// <param name="Parameters">A collection of parameter metadata objects, each specifying a name and value to bind to the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the command encounters an error during processing.
        /// </exception>
        public static void ExecuteNonQuery(this DeleteCommand DCMD, DBConnect DBC, IEnumerable<ParametersMetadata> Parameters)
        {
            DBC.CommandText = DCMD.ToString();
            DBC.ExecuteNonQuery(Parameters);
        }

        /// <summary>
        /// Executes the composed SQL <c>INSERT INTO</c> statement represented by the <see cref="InsertIntoCommand{T}"/> instance using the specified <see cref="DBConnect"/> context.
        /// </summary>
        /// <typeparam name="T">The enum type representing the table schema used in the insert operation.</typeparam>
        /// <param name="ICMD">The <see cref="InsertIntoCommand{T}"/> instance containing the SQL insert query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the command encounters an error during processing.
        /// </exception>
        public static void ExecuteNonQuery<T>(this InsertIntoCommand<T> ICMD, DBConnect DBC)
            where T: Enum
        {
            DBC.CommandText = ICMD.ToString();
            DBC.ExecuteNonQuery();
        }
        /// <summary>
        /// Executes the composed SQL <c>INSERT INTO</c> statement represented by the <see cref="InsertIntoCommand{T}"/> instance using the specified <see cref="DBConnect"/> context and a single parameter.
        /// </summary>
        /// <typeparam name="T">The enum type representing the table schema used in the insert operation.</typeparam>
        /// <param name="ICMD">The <see cref="InsertIntoCommand{T}"/> instance containing the SQL insert query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query.</param>
        /// <param name="Parameter">The parameter metadata containing the name and value to bind to the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the command encounters an error during processing.
        /// </exception>
        public static void ExecuteNonQuery<T>(this InsertIntoCommand<T> ICMD, DBConnect DBC, ParametersMetadata Parameter)
            where T: Enum
        {
            DBC.CommandText = ICMD.ToString();
            DBC.ExecuteNonQuery(Parameter);
        }
        /// <summary>
        /// Executes the composed SQL <c>INSERT INTO</c> statement represented by the <see cref="InsertIntoCommand{T}"/> instance using the specified <see cref="DBConnect"/> context and a collection of parameters.
        /// </summary>
        /// <typeparam name="T">The enum type representing the table schema used in the insert operation.</typeparam>
        /// <param name="ICMD">The <see cref="InsertIntoCommand{T}"/> instance containing the SQL insert query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query.</param>
        /// <param name="Parameters">A collection of parameter metadata objects, each specifying a name and value to bind to the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the command encounters an error during processing.
        /// </exception>
        public static void ExecuteNonQuery<T>(this InsertIntoCommand<T> ICMD, DBConnect DBC, IEnumerable<ParametersMetadata> Parameters)
            where T: Enum
        {
            DBC.CommandText = ICMD.ToString();
            DBC.ExecuteNonQuery(Parameters);
        }
        /// <summary>
        /// Executes the composed SQL <c>INSERT INTO</c> statement represented by the <see cref="InsertIntoCommand"/> instance using the specified <see cref="DBConnect"/> context.
        /// </summary>
        /// <param name="ICMD">The <see cref="InsertIntoCommand"/> instance containing the SQL insert query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the command encounters an error during processing.
        /// </exception>
        public static void ExecuteNonQuery(this InsertIntoCommand ICMD, DBConnect DBC)
        {
            DBC.CommandText = ICMD.ToString();
            DBC.ExecuteNonQuery();
        }
        /// <summary>
        /// Executes the composed SQL <c>INSERT INTO</c> statement represented by the <see cref="InsertIntoCommand"/> instance using the specified <see cref="DBConnect"/> context and a single parameter.
        /// </summary>
        /// <param name="ICMD">The <see cref="InsertIntoCommand"/> instance containing the SQL insert query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query.</param>
        /// <param name="Parameter">The parameter metadata containing the name and value to bind to the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the command encounters an error during processing.
        /// </exception>
        public static void ExecuteNonQuery(this InsertIntoCommand ICMD, DBConnect DBC, ParametersMetadata Parameter)
        {
            DBC.CommandText = ICMD.ToString();
            DBC.ExecuteNonQuery(Parameter);
        }
        /// <summary>
        /// Executes the composed SQL <c>INSERT INTO</c> statement represented by the <see cref="InsertIntoCommand"/> instance using the specified <see cref="DBConnect"/> context and a collection of parameters.
        /// </summary>
        /// <param name="ICMD">The <see cref="InsertIntoCommand"/> instance containing the SQL insert query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query.</param>
        /// <param name="Parameters">A collection of parameter metadata objects, each specifying a name and value to bind to the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the command encounters an error during processing.
        /// </exception>
        public static void ExecuteNonQuery(this InsertIntoCommand ICMD, DBConnect DBC, IEnumerable<ParametersMetadata> Parameters)
        {
            DBC.CommandText = ICMD.ToString();
            DBC.ExecuteNonQuery(Parameters);
        }

        /// <summary>
        /// Executes the composed SQL <c>UPDATE</c> statement represented by the <see cref="UpdateCommand{T}"/> instance using the specified <see cref="DBConnect"/> context.
        /// </summary>
        /// <typeparam name="T">The enum type representing the table schema used in the update operation.</typeparam>
        /// <param name="UCMD">The <see cref="UpdateCommand{T}"/> instance containing the SQL update query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the command encounters an error during processing.
        /// </exception>
        public static void ExecuteNonQuery<T>(this UpdateCommand<T> UCMD, DBConnect DBC)
            where T: Enum
        {
            DBC.CommandText = UCMD.ToString();
            DBC.ExecuteNonQuery();
        }
        /// <summary>
        /// Executes the composed SQL <c>UPDATE</c> statement represented by the <see cref="UpdateCommand{T}"/> instance using the specified <see cref="DBConnect"/> context and a single parameter.
        /// </summary>
        /// <typeparam name="T">The enum type representing the table schema used in the update operation.</typeparam>
        /// <param name="UCMD">The <see cref="UpdateCommand{T}"/> instance containing the SQL update query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query.</param>
        /// <param name="Parameter">The parameter metadata containing the name and value to bind to the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the command encounters an error during processing.
        /// </exception>
        public static void ExecuteNonQuery<T>(this UpdateCommand<T> UCMD, DBConnect DBC, ParametersMetadata Parameter)
            where T: Enum
        {
            DBC.CommandText = UCMD.ToString();
            DBC.ExecuteNonQuery(Parameter);
        }
        /// <summary>
        /// Executes the composed SQL <c>UPDATE</c> statement represented by the <see cref="UpdateCommand{T}"/> instance using the specified <see cref="DBConnect"/> context and a collection of parameters.
        /// </summary>
        /// <typeparam name="T">The enum type representing the table schema used in the update operation.</typeparam>
        /// <param name="UCMD">The <see cref="UpdateCommand{T}"/> instance containing the SQL update query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query.</param>
        /// <param name="Parameters">A collection of parameter metadata objects, each specifying a name and value to bind to the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the command encounters an error during processing.
        /// </exception>
        public static void ExecuteNonQuery<T>(this UpdateCommand<T> UCMD, DBConnect DBC, IEnumerable<ParametersMetadata> Parameters)
            where T: Enum
        {
            DBC.CommandText = UCMD.ToString();
            DBC.ExecuteNonQuery(Parameters);
        }
        /// <summary>
        /// Executes the composed SQL <c>UPDATE</c> statement represented by the <see cref="UpdateCommand"/> instance using the specified <see cref="DBConnect"/> context.
        /// </summary>
        /// <param name="UCMD">The <see cref="UpdateCommand"/> instance containing the SQL update query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the command encounters an error during processing.
        /// </exception>
        public static void ExecuteNonQuery(this UpdateCommand UCMD, DBConnect DBC)
        {
            DBC.CommandText = UCMD.ToString();
            DBC.ExecuteNonQuery();
        }
        /// <summary>
        /// Executes the composed SQL <c>UPDATE</c> statement represented by the <see cref="UpdateCommand"/> instance using the specified <see cref="DBConnect"/> context and a single parameter.
        /// </summary>
        /// <param name="UCMD">The <see cref="UpdateCommand"/> instance containing the SQL update query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query.</param>
        /// <param name="Parameter">The parameter metadata containing the name and value to bind to the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the command encounters an error during processing.
        /// </exception>
        public static void ExecuteNonQuery(this UpdateCommand UCMD, DBConnect DBC, ParametersMetadata Parameter)
        {
            DBC.CommandText = UCMD.ToString();
            DBC.ExecuteNonQuery(Parameter);
        }
        /// <summary>
        /// Executes the composed SQL <c>UPDATE</c> statement represented by the <see cref="UpdateCommand"/> instance using the specified <see cref="DBConnect"/> context and a collection of parameters.
        /// </summary>
        /// <param name="UCMD">The <see cref="UpdateCommand"/> instance containing the SQL update query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query.</param>
        /// <param name="Parameters">A collection of parameter metadata objects, each specifying a name and value to bind to the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the command encounters an error during processing.
        /// </exception>
        public static void ExecuteNonQuery(this UpdateCommand UCMD, DBConnect DBC, IEnumerable<ParametersMetadata> Parameters)
        {
            DBC.CommandText = UCMD.ToString();
            DBC.ExecuteNonQuery(Parameters);
        }

        /// <summary>
        /// Executes the composed SQL <c>TRUNCATE</c> statement represented by the <see cref="TruncateCommand{T}"/> instance using the specified <see cref="DBConnect"/> context.
        /// </summary>
        /// <typeparam name="T">The enum type representing the table schema targeted by the truncate operation.</typeparam>
        /// <param name="TCMD">The <see cref="TruncateCommand{T}"/> instance containing the SQL truncate query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the command encounters an error during processing.
        /// </exception>
        public static void ExecuteNonQuery<T>(this TruncateCommand<T> TCMD, DBConnect DBC)
            where T: Enum
        {
            DBC.CommandText = TCMD.ToString();
            DBC.ExecuteNonQuery();
        }
        /// <summary>
        /// Executes the composed SQL <c>TRUNCATE</c> statement represented by the <see cref="TruncateCommand"/> instance using the specified <see cref="DBConnect"/> context.
        /// </summary>
        /// <param name="TCMD">The <see cref="TruncateCommand"/> instance containing the SQL truncate query to execute.</param>
        /// <param name="DBC">The <see cref="DBConnect"/> instance responsible for executing the query.</param>
        /// <exception cref="Exception">
        /// Thrown when the delegated execution fails or the command encounters an error during processing.
        /// </exception>
        public static void ExecuteNonQuery(this TruncateCommand TCMD, DBConnect DBC)
        {
            DBC.CommandText = TCMD.ToString();
            DBC.ExecuteNonQuery();
        }
    }
}
