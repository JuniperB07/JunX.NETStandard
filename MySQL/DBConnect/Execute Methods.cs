using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;

namespace JunX.NETStandard.MySQL
{
    public partial class DBConnect : IExecutableReader, IExecutableAdapter, IExecutableDataSet, IExecutableQuery
    {
        /// <summary>
        /// Executes the current SQL <c>SELECT</c> command and stores the result set values in the internal collection.
        /// </summary>
        /// <exception cref="Exception">
        /// Thrown when the command execution fails due to connection issues, invalid SQL syntax, or runtime errors.
        /// </exception>
        /// <remarks>
        /// This method performs the following steps:
        /// <list type="bullet">
        /// <item><description>Validates the connection and command text using <see cref="ValidateForExecution"/>.</description></item>
        /// <item><description>Assigns the current <see cref="CommandText"/> to the internal <see cref="MySqlCommand"/> and clears existing parameters.</description></item>
        /// <item><description>Executes the command using <c>ExecuteReader()</c> and stores the result in <c>InternalVariables.Reader</c>.</description></item>
        /// <item><description>If rows are returned, iterates through each field of each row and adds the stringified values to <c>InternalVariables.Values</c>.</description></item>
        /// </list>
        /// Intended for use with SQL <c>SELECT</c> queries that return tabular data.
        /// </remarks>
        public void ExecuteReader()
        {
            ValidateForExecution();

            InternalVariables.Command.CommandText = CommandText;
            InternalVariables.Command.Parameters.Clear();
            try
            {
                InternalVariables.Reader = InternalVariables.Command.ExecuteReader();

                InternalVariables.Values.Clear();
                _columnValues.Clear();
                if (InternalVariables.Reader.HasRows)
                {
                    while (InternalVariables.Reader.Read())
                    {
                        for (int i = 0; i < InternalVariables.Reader.FieldCount; i++)
                        {
                            InternalVariables.Values.Add(InternalVariables.Reader[i].ToString());
                        }

                        foreach(string colName in _selectedColumns)
                        {
                            string val = InternalVariables.Reader[colName]?.ToString() ?? "";
                            _columnValues.Add(colName, val);
                        }
                    }
                }
                ReaderExecuted?.Invoke(this, new ReaderExecutedEventArgs(CommandText, InternalVariables.Reader.HasRows));
            }
            catch (Exception e)
            {
                throw new Exception("Unable to execute reader.\n\n" + e.Message.ToString());
            }
        }
        /// <summary>
        /// Executes the current SQL <c>SELECT</c> command with a single parameter and stores the result set values in the internal collection.
        /// </summary>
        /// <param name="Parameter">
        /// A <see cref="ParametersMetadata"/> instance containing the name and value of the SQL parameter to bind.
        /// </param>
        /// <exception cref="Exception">
        /// Thrown when the command execution fails due to connection issues, invalid SQL syntax, or parameter binding errors.
        /// </exception>
        /// <remarks>
        /// This method performs the following steps:
        /// <list type="bullet">
        /// <item><description>Validates the connection and command text using <see cref="ValidateForExecution"/>.</description></item>
        /// <item><description>Assigns the current <see cref="CommandText"/> to the internal <see cref="MySqlCommand"/> and clears existing parameters.</description></item>
        /// <item><description>Binds the provided parameter using <c>AddWithValue</c> and executes the command using <c>ExecuteReader()</c>.</description></item>
        /// <item><description>If rows are returned, iterates through each field of each row and adds the stringified values to <c>InternalVariables.Values</c>.</description></item>
        /// </list>
        /// Intended for use with parameterized <c>SELECT</c> queries that return tabular data.
        /// </remarks>
        public void ExecuteReader(ParametersMetadata Parameter)
        {
            ValidateForExecution();

            InternalVariables.Command.CommandText = CommandText;
            InternalVariables.Command.Parameters.Clear();
            InternalVariables.Command.Parameters.AddWithValue(Parameter.ParameterName, Parameter.Value);

            try
            {
                InternalVariables.Reader = InternalVariables.Command.ExecuteReader();

                InternalVariables.Values.Clear();
                if (InternalVariables.Reader.HasRows)
                {
                    while (InternalVariables.Reader.Read())
                    {
                        for (int i = 0; i < InternalVariables.Reader.FieldCount; i++)
                            InternalVariables.Values.Add(InternalVariables.Reader[i].ToString());
                    }
                }
                ReaderExecuted?.Invoke(this, new ReaderExecutedEventArgs(CommandText, InternalVariables.Reader.HasRows));
            }
            catch (Exception e)
            {
                throw new Exception("Unable to execute reader.\n\n" + e.Message.ToString());
            }
        }
        /// <summary>
        /// Executes the current SQL <c>SELECT</c> command with multiple parameters and stores the result set values in the internal collection.
        /// </summary>
        /// <param name="Parameters">
        /// A collection of <see cref="ParametersMetadata"/> instances representing the parameter names and values to bind to the SQL command.
        /// </param>
        /// <exception cref="Exception">
        /// Thrown when the command execution fails due to connection issues, invalid SQL syntax, or parameter binding errors.
        /// </exception>
        /// <remarks>
        /// This method performs the following steps:
        /// <list type="bullet">
        /// <item><description>Validates the connection and command text using <see cref="ValidateForExecution"/>.</description></item>
        /// <item><description>Assigns the current <see cref="CommandText"/> to the internal <see cref="MySqlCommand"/> and clears existing parameters.</description></item>
        /// <item><description>Binds each provided parameter using <c>AddWithValue</c> and executes the command using <c>ExecuteReader()</c>.</description></item>
        /// <item><description>If rows are returned, iterates through each field of each row and adds the stringified values to <c>InternalVariables.Values</c>.</description></item>
        /// </list>
        /// Intended for use with parameterized <c>SELECT</c> queries that return tabular data.
        /// </remarks>
        public void ExecuteReader(IEnumerable<ParametersMetadata> Parameters)
        {
            ValidateForExecution();

            InternalVariables.Command.CommandText = CommandText;
            InternalVariables.Command.Parameters.Clear();
            foreach (ParametersMetadata P in Parameters)
                InternalVariables.Command.Parameters.AddWithValue(P.ParameterName, P.Value);

            try
            {
                InternalVariables.Reader = InternalVariables.Command.ExecuteReader();

                InternalVariables.Values.Clear();
                if (InternalVariables.Reader.HasRows)
                {
                    while (InternalVariables.Reader.Read())
                    {
                        for (int i = 0; i < InternalVariables.Reader.FieldCount; i++)
                            InternalVariables.Values.Add(InternalVariables.Reader[i].ToString());
                    }
                }
                ReaderExecuted?.Invoke(this, new ReaderExecutedEventArgs(CommandText, InternalVariables.Reader.HasRows));
            }
            catch (Exception e)
            {
                throw new Exception("Unable to execute reader.\n\n" + e.Message.ToString());
            }
        }

        /// <summary>
        /// Initializes the internal <see cref="MySqlDataAdapter"/> with a <c>SELECT</c> command for data operations.
        /// </summary>
        /// <exception cref="Exception">
        /// Thrown if validation fails due to an unopened connection or missing/invalid command text.
        /// </exception>
        /// <remarks>
        /// This method validates the connection and command text using <see cref="ValidateForExecution"/>.
        /// It then creates a new <see cref="MySqlDataAdapter"/> and assigns a <see cref="MySqlCommand"/> configured with the current <see cref="CommandText"/> and <see cref="Connection"/>.
        /// Intended for use with disconnected data access scenarios, such as filling a <see cref="DataSet"/> or performing updates.
        /// </remarks>
        public void ExecuteAdapter()
        {
            ValidateForExecution();

            InternalVariables.Adapter = new MySqlDataAdapter();
            InternalVariables.Adapter.SelectCommand = new MySqlCommand(CommandText, Connection);
            AdapterExecuted?.Invoke(this, new AdapterExecutedEventArgs(CommandText));
        }
        /// <summary>
        /// Initializes the internal <see cref="MySqlDataAdapter"/> with a parameterized <c>SELECT</c> command for data operations.
        /// </summary>
        /// <param name="Parameter">
        /// A <see cref="ParametersMetadata"/> instance containing the name and value of the SQL parameter to bind.
        /// </param>
        /// <exception cref="Exception">
        /// Thrown if validation fails due to an unopened connection or missing/invalid command text.
        /// </exception>
        /// <remarks>
        /// This method validates the connection and command text using <see cref="ValidateForExecution"/>.
        /// It assigns the current <see cref="CommandText"/> to the internal <see cref="MySqlCommand"/>,
        /// clears any existing parameters, binds the provided parameter using <c>AddWithValue</c>,
        /// and assigns the command to the <c>SelectCommand</c> of the internal <see cref="MySqlDataAdapter"/>.
        /// Intended for use with disconnected data access scenarios involving parameterized queries.
        /// </remarks>
        public void ExecuteAdapter(ParametersMetadata Parameter)
        {
            ValidateForExecution();

            InternalVariables.Adapter = new MySqlDataAdapter();
            InternalVariables.Command.CommandText = CommandText;
            InternalVariables.Command.Parameters.Clear();
            InternalVariables.Command.Parameters.AddWithValue(Parameter.ParameterName, Parameter.Value);
            InternalVariables.Adapter.SelectCommand = InternalVariables.Command;
            AdapterExecuted?.Invoke(this, new AdapterExecutedEventArgs(CommandText));
        }
        /// <summary>
        /// Initializes the internal <see cref="MySqlDataAdapter"/> with a parameterized <c>SELECT</c> command for data operations.
        /// </summary>
        /// <param name="Parameters">
        /// A collection of <see cref="ParametersMetadata"/> instances representing the parameter names and values to bind to the SQL command.
        /// </param>
        /// <exception cref="Exception">
        /// Thrown if validation fails due to an unopened connection or missing/invalid command text.
        /// </exception>
        /// <remarks>
        /// This method validates the connection and command text using <see cref="ValidateForExecution"/>.
        /// It assigns the current <see cref="CommandText"/> to the internal <see cref="MySqlCommand"/>,
        /// clears any existing parameters, binds each provided parameter using <c>AddWithValue</c>,
        /// and assigns the command to the <c>SelectCommand</c> of the internal <see cref="MySqlDataAdapter"/>.
        /// Intended for use with disconnected data access scenarios involving parameterized queries and batch operations.
        /// </remarks>
        public void ExecuteAdapter(IEnumerable<ParametersMetadata> Parameters)
        {
            ValidateForExecution();

            InternalVariables.Adapter = new MySqlDataAdapter();
            InternalVariables.Command.CommandText = CommandText;
            InternalVariables.Command.Parameters.Clear();

            foreach (ParametersMetadata P in Parameters)
                InternalVariables.Command.Parameters.AddWithValue(P.ParameterName, P.Value);

            InternalVariables.Adapter.SelectCommand = InternalVariables.Command;
            AdapterExecuted?.Invoke(this, new AdapterExecutedEventArgs(CommandText));
        }

        /// <summary>
        /// Executes the current SQL <c>SELECT</c> command and fills the internal <see cref="DataSet"/> with the result set.
        /// </summary>
        /// <exception cref="Exception">
        /// Thrown if adapter initialization or data fill operation fails due to connection issues or invalid command configuration.
        /// </exception>
        /// <remarks>
        /// This method initializes a new <see cref="DataSet"/>, invokes <see cref="ExecuteAdapter()"/> to configure the internal <see cref="MySqlDataAdapter"/>,
        /// and fills the dataset with the result of the executed query.
        /// Intended for disconnected data access scenarios such as UI binding, reporting, or in-memory data manipulation.
        /// </remarks>
        public void ExecuteDataSet()
        {
            InternalVariables.Dataset = new DataSet();
            ExecuteAdapter();
            InternalVariables.Adapter.Fill(InternalVariables.Dataset);
            DataSetExecuted?.Invoke(this, new DataSetExecutedEventArgs(CommandText, InternalVariables.Dataset));
        }
        /// <summary>
        /// Executes a parameterized SQL <c>SELECT</c> command and fills the internal <see cref="DataSet"/> with the result set.
        /// </summary>
        /// <param name="Parameter">
        /// A <see cref="ParametersMetadata"/> instance containing the name and value of the SQL parameter to bind.
        /// </param>
        /// <exception cref="Exception">
        /// Thrown if adapter initialization or data fill operation fails due to connection issues or invalid command configuration.
        /// </exception>
        /// <remarks>
        /// This method initializes a new <see cref="DataSet"/>, invokes <see cref="ExecuteAdapter(ParametersMetadata)"/> to configure the internal <see cref="MySqlDataAdapter"/> with the provided parameter,
        /// and fills the dataset with the result of the executed query.
        /// Intended for disconnected data access scenarios involving parameterized queries, such as UI binding or in-memory data manipulation.
        /// </remarks>
        public void ExecuteDataSet(ParametersMetadata Parameter)
        {
            InternalVariables.Dataset = new DataSet();
            ExecuteAdapter(Parameter);
            InternalVariables.Adapter.Fill(InternalVariables.Dataset);
            DataSetExecuted?.Invoke(this, new DataSetExecutedEventArgs(CommandText, InternalVariables.Dataset));
        }
        /// <summary>
        /// Executes a parameterized SQL <c>SELECT</c> command and fills the internal <see cref="DataSet"/> with the result set.
        /// </summary>
        /// <param name="Parameters">
        /// A collection of <see cref="ParametersMetadata"/> instances representing the parameter names and values to bind to the SQL command.
        /// </param>
        /// <exception cref="Exception">
        /// Thrown if adapter initialization or data fill operation fails due to connection issues or invalid command configuration.
        /// </exception>
        /// <remarks>
        /// This method initializes a new <see cref="DataSet"/>, invokes <see cref="ExecuteAdapter(IEnumerable{ParametersMetadata})"/> to configure the internal <see cref="MySqlDataAdapter"/> with the provided parameters,
        /// and fills the dataset with the result of the executed query.
        /// Intended for disconnected data access scenarios involving parameterized queries, batch operations, or UI-bound data workflows.
        /// </remarks>
        public void ExecuteDataSet(IEnumerable<ParametersMetadata> Parameters)
        {
            InternalVariables.Dataset = new DataSet();
            ExecuteAdapter(Parameters);
            InternalVariables.Adapter.Fill(InternalVariables.Dataset);
            DataSetExecuted?.Invoke(this, new DataSetExecutedEventArgs(CommandText, InternalVariables.Dataset));
        }

        /// <summary>
        /// Executes the current SQL command that does not return a result set, such as <c>INSERT</c>, <c>UPDATE</c>, or <c>DELETE</c>.
        /// </summary>
        /// <exception cref="Exception">
        /// Thrown when the command execution fails due to connection issues, invalid SQL syntax, or runtime errors.
        /// </exception>
        /// <remarks>
        /// This method assigns the current <see cref="CommandText"/> to the internal <see cref="MySqlCommand"/>,
        /// clears any existing parameters, and executes the command using <c>ExecuteNonQuery()</c>.
        /// Intended for use with SQL statements that modify data but do not return rows.
        /// </remarks>
        public void ExecuteNonQuery()
        {
            InternalVariables.Command.CommandText = CommandText;
            InternalVariables.Command.Parameters.Clear();

            try
            {
                InternalVariables.Command.ExecuteNonQuery();
                NonQueryExecuted?.Invoke(this, new NonQueryExecutedEventArgs(CommandText));
            }
            catch (Exception e)
            {
                throw new Exception("Unable to perform execution.\n\n" + e.Message.ToString());
            }
        }
        /// <summary>
        /// Executes a parameterized SQL command that does not return a result set, such as <c>INSERT</c>, <c>UPDATE</c>, or <c>DELETE</c>.
        /// </summary>
        /// <param name="Parameter">
        /// A <see cref="ParametersMetadata"/> instance containing the name and value of the SQL parameter to bind.
        /// </param>
        /// <exception cref="Exception">
        /// Thrown when the command execution fails due to connection issues, invalid SQL syntax, or parameter binding errors.
        /// </exception>
        /// <remarks>
        /// This method assigns the current <see cref="CommandText"/> to the internal <see cref="MySqlCommand"/>,
        /// clears any existing parameters, binds the provided parameter using <c>AddWithValue</c>,
        /// and executes the command using <c>ExecuteNonQuery()</c>.
        /// Intended for use with single-parameter SQL statements that modify data without returning rows.
        /// </remarks>
        public void ExecuteNonQuery(ParametersMetadata Parameter)
        {
            InternalVariables.Command.CommandText = CommandText;
            InternalVariables.Command.Parameters.Clear();
            InternalVariables.Command.Parameters.AddWithValue(Parameter.ParameterName, Parameter.Value);

            try
            {
                InternalVariables.Command.ExecuteNonQuery();
                NonQueryExecuted?.Invoke(this, new NonQueryExecutedEventArgs(CommandText));
            }
            catch (Exception e)
            {
                throw new Exception("Unable to perform execution.\n\n" + e.Message.ToString());
            }
        }
        /// <summary>
        /// Executes a parameterized SQL command that does not return a result set, such as <c>INSERT</c>, <c>UPDATE</c>, or <c>DELETE</c>.
        /// </summary>
        /// <param name="Parameters">
        /// A collection of <see cref="ParametersMetadata"/> instances representing the parameter names and values to bind to the SQL command.
        /// </param>
        /// <exception cref="Exception">
        /// Thrown when the command execution fails due to connection issues, invalid SQL syntax, or parameter binding errors.
        /// </exception>
        /// <remarks>
        /// This method assigns the current <see cref="CommandText"/> to the internal <see cref="MySqlCommand"/>,
        /// clears any existing parameters, binds each provided parameter using <c>AddWithValue</c>,
        /// and executes the command using <c>ExecuteNonQuery()</c>.
        /// Intended for use with multi-parameter SQL statements that modify data without returning rows.
        /// </remarks>
        public void ExecuteNonQuery(IEnumerable<ParametersMetadata> Parameters)
        {
            InternalVariables.Command.CommandText = CommandText;
            InternalVariables.Command.Parameters.Clear();

            foreach (ParametersMetadata P in Parameters)
                InternalVariables.Command.Parameters.AddWithValue(P.ParameterName, P.Value);

            try
            {
                InternalVariables.Command.ExecuteNonQuery();
                NonQueryExecuted?.Invoke(this, new NonQueryExecutedEventArgs(CommandText));
            }
            catch (Exception e)
            {
                throw new Exception("Unable to perform execution.\n\n" + e.Message.ToString());
            }
        }


        public void SetSelectedColumns(IEnumerable<Enum> Columns)
        {
            _selectedColumns.Clear();
            foreach (var col in Columns)
                _selectedColumns.Add(col.ToString());
        }
    }
}
