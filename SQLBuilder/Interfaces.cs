using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace JunX.NETStandard.SQLBuilder
{
    /// <summary>
    /// Defines a contract for conditionable objects that expose a starting condition of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The reference type representing the condition model or query context.
    /// </typeparam>
    /// <remarks>
    /// This interface is typically used in dynamic SQL builders, filtering pipelines, or metadata-driven query composition.
    /// The <c>StartWhere</c> property serves as the entry point for condition chaining or predicate construction.
    /// </remarks>
    public interface IConditionable<T> where T: class
    {
        /// <summary>
        /// Gets the initial condition object of type <typeparamref name="T"/> used to begin a filtering or query chain.
        /// </summary>
        /// <value>
        /// An instance of <typeparamref name="T"/> representing the starting condition or predicate context.
        /// </value>
        /// <remarks>
        /// This property serves as the entry point for condition composition in dynamic SQL builders, rule engines, or metadata-driven pipelines.
        /// It is typically used to anchor subsequent logical operations such as <c>And</c>, <c>Or</c>, or <c>WhereEquals</c>.
        /// </remarks>
        T StartWhere { get; }
    }

    /// <summary>
    /// Defines a contract for fluent insert operations targeting a specific entity type <typeparamref name="J"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The command type implementing this interface, used to enable fluent chaining.
    /// </typeparam>
    /// <typeparam name="J">
    /// The type representing a column identifier, typically an enum or metadata class.
    /// </typeparam>
    /// <remarks>
    /// This interface supports fluent composition of insert statements by specifying target columns and corresponding values.
    /// It is commonly used in dynamic SQL builders, metadata-driven pipelines, or domain-specific reporting frameworks.
    /// The <c>Column</c> methods define the insertion targets, while the <c>Values</c> methods specify the data payload and type metadata.
    /// </remarks>
    public interface IInsertable<T, J> 
        where T: class
    {
        /// <summary>
        /// Specifies a single column to be included in the insert operation.
        /// </summary>
        /// <param name="Column">
        /// The column identifier of type <typeparamref name="J"/>, typically an enum or metadata descriptor.
        /// </param>
        /// <returns>
        /// The current <typeparamref name="T"/> instance to enable fluent chaining.
        /// </returns>
        /// <remarks>
        /// This method is used to define the target column for the insert statement. It is commonly used in metadata-driven SQL builders or domain-specific reporting pipelines.
        /// </remarks>
        T Column(J Column);
        /// <summary>
        /// Specifies multiple columns to be included in the insert operation using an enumerable collection.
        /// </summary>
        /// <param name="Columns">
        /// A collection of column identifiers of type <typeparamref name="J"/>, typically enums or metadata descriptors.
        /// </param>
        /// <returns>
        /// The current <typeparamref name="T"/> instance to enable fluent chaining.
        /// </returns>
        /// <remarks>
        /// This method is used to define multiple target columns for the insert statement in a flexible, collection-based format.
        /// Ideal for scenarios where column metadata is dynamically generated or retrieved from configuration.
        /// </remarks>
        T Column(IEnumerable<J> Columns);
        /// <summary>
        /// Specifies multiple columns to be included in the insert operation using a parameter array.
        /// </summary>
        /// <param name="Columns">
        /// A variable-length array of column identifiers of type <typeparamref name="J"/>, typically enums or metadata descriptors.
        /// </param>
        /// <returns>
        /// The current <typeparamref name="T"/> instance to enable fluent chaining.
        /// </returns>
        /// <remarks>
        /// This method provides a convenient way to define multiple target columns inline, especially when the column set is known at compile time.
        /// Useful for fluent insert builders, metadata-driven pipelines, or DSL-style query composition.
        /// </remarks>
        T Column(params J[] Columns);

        /// <summary>
        /// Specifies a single value and its associated data type to be inserted into the current column context.
        /// </summary>
        /// <param name="Value">
        /// The string representation of the value to insert.
        /// </param>
        /// <param name="Type">
        /// The <see cref="DataTypes"/> enumeration value indicating the type of the data being inserted.
        /// </param>
        /// <returns>
        /// The current <typeparamref name="T"/> instance to enable fluent chaining.
        /// </returns>
        /// <remarks>
        /// This method is used to bind a single value to the most recently defined column or column set.
        /// It supports explicit type declaration for scenarios requiring precision, such as SQL generation, reporting pipelines, or metadata-driven inserts.
        /// </remarks>
        T Values(string Value, DataTypes Type);
        /// <summary>
        /// Specifies multiple values to be inserted using a collection of <see cref="ValuesMetadata"/> objects.
        /// </summary>
        /// <param name="Values">
        /// A collection of <see cref="ValuesMetadata"/> instances, each representing a value and its associated type metadata.
        /// </param>
        /// <returns>
        /// The current <typeparamref name="T"/> instance to enable fluent chaining.
        /// </returns>
        /// <remarks>
        /// This method is used to bind multiple values to the previously defined column set in a structured, metadata-driven format.
        /// Ideal for dynamic insert scenarios, bulk operations, or pipelines that rely on runtime value resolution and type safety.
        /// </remarks>
        T Values(IEnumerable<ValuesMetadata> Values);
        /// <summary>
        /// Specifies multiple values to be inserted using a parameter array of <see cref="ValuesMetadata"/> objects.
        /// </summary>
        /// <param name="Values">
        /// A variable-length array of <see cref="ValuesMetadata"/> instances, each representing a value and its associated type metadata.
        /// </param>
        /// <returns>
        /// The current <typeparamref name="T"/> instance to enable fluent chaining.
        /// </returns>
        /// <remarks>
        /// This method provides a convenient way to define multiple insert values inline, especially when the value set is known at compile time.
        /// Useful for fluent insert builders, metadata-driven pipelines, or DSL-style query composition where type safety and clarity are essential.
        /// </remarks>
        T Values(params ValuesMetadata[] Values);
    }

    /// <summary>
    /// Defines a contract for fluent SQL SELECT operations targeting a specific column type <typeparamref name="J"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The command type implementing this interface, used to enable fluent chaining.
    /// </typeparam>
    /// <typeparam name="J">
    /// The type representing a column identifier, typically an enum or metadata descriptor.
    /// </typeparam>
    /// <remarks>
    /// This interface supports fluent composition of SQL SELECT statements by specifying columns, aliases, and wildcard selection.
    /// It is commonly used in dynamic SQL builders, reporting frameworks, or metadata-driven query pipelines.
    /// The <c>Select</c> methods define the projection targets, while <c>As</c> enables aliasing of the result set.
    /// </remarks>
    public interface ISelectable<T, J> where T: class
    {
        T SelectAll { get; }

        T Select(J Column, bool IsFullyQualified);
        T Select(IEnumerable<J> Columns, bool IsFullyQualified);
        T Selects(params J[] Columns);

        T As(string Alias);
    }

    /// <summary>
    /// Defines a contract for fluent SQL SELECT operations across a primary and joined entity context.
    /// </summary>
    /// <typeparam name="TClass">
    /// The command type implementing this interface, used to enable fluent chaining.
    /// </typeparam>
    /// <typeparam name="Primary">
    /// The type representing column identifiers from the primary table, typically an enum or metadata descriptor.
    /// </typeparam>
    /// <typeparam name="Joined">
    /// The type representing column identifiers from the joined table, typically an enum or metadata descriptor.
    /// </typeparam>
    /// <remarks>
    /// This interface supports fluent composition of SQL SELECT statements involving both primary and joined tables.
    /// It enables granular projection of columns from either source, wildcard selection, and aliasing of result sets.
    /// Commonly used in dynamic SQL builders, reporting frameworks, or metadata-driven query pipelines.
    /// </remarks>
    public interface ISelectable<TClass, Primary, Joined> where TClass: class
    {
        TClass SelectAllPrimary { get; }
        TClass SelectAllJoined { get; }

        TClass Select(Primary Column);
        TClass Select(Joined Column);
        TClass Select(IEnumerable<Primary> Columns);
        TClass Select(IEnumerable<Joined> Columns);
        TClass Select(params Primary[] Columns);
        TClass Select(params Joined[] Columns);

        TClass As(string Alias);
    }

    /// <summary>
    /// Defines a contract for aliasing column selections in SQL queries using metadata carriers.
    /// </summary>
    /// <typeparam name="TClass">
    /// The command type implementing this interface, used to enable fluent chaining.
    /// </typeparam>
    /// <typeparam name="TAliasCarrier">
    /// The type representing alias metadata, typically a wrapper that binds a column identifier to its alias.
    /// </typeparam>
    /// <remarks>
    /// This interface supports fluent composition of SQL SELECT statements with aliasing logic.
    /// It is commonly used in dynamic SQL builders, reporting frameworks, or metadata-driven pipelines where column renaming is required.
    /// The <c>SelectAs</c> methods allow aliasing of one or more columns using structured metadata.
    /// </remarks>
    public interface IAliasable<TClass, TAliasCarrier> where TClass: class
    {
        TClass SelectAs(IEnumerable<TAliasCarrier> SelectAs);
        TClass SelectAs(params TAliasCarrier[] SelectAs);
    }

    /// <summary>
    /// Defines a contract for aliasing column selections in SQL queries using multiple metadata carrier types.
    /// </summary>
    /// <typeparam name="TClass">
    /// The command type implementing this interface, used to enable fluent chaining.
    /// </typeparam>
    /// <typeparam name="TAliasCarrier1">
    /// The first alias metadata type, typically representing primary column-to-alias mappings.
    /// </typeparam>
    /// <typeparam name="TAliasCarrier2">
    /// The second alias metadata type, typically representing joined column-to-alias mappings or alternate metadata formats.
    /// </typeparam>
    /// <remarks>
    /// This interface supports fluent composition of SQL SELECT statements with aliasing logic across multiple metadata carrier types.
    /// It is commonly used in dynamic SQL builders, reporting frameworks, or metadata-driven pipelines where aliasing is applied to both primary and joined entities.
    /// The <c>SelectAs</c> methods allow aliasing of one or more columns using structured metadata from either carrier type.
    /// </remarks>
    public interface IAliasable<TClass, TAliasCarrier1, TAliasCarrier2> where TClass: class
    {
        TClass SelectAs(IEnumerable<TAliasCarrier1> SelectAs);
        TClass SelectAs(params TAliasCarrier1[] SelectAs);
        TClass SelectAs(IEnumerable<TAliasCarrier2> SelectAs);
        TClass SelectAs(params TAliasCarrier2[] SelectAs);
    }

    /// <summary>
    /// Defines a contract for applying ORDER BY clauses to SQL queries using a column identifier and sort direction.
    /// </summary>
    /// <typeparam name="T">
    /// The command type implementing this interface, used to enable fluent chaining.
    /// </typeparam>
    /// <typeparam name="J">
    /// The type representing a column identifier, typically an enum or metadata descriptor.
    /// </typeparam>
    /// <remarks>
    /// This interface supports fluent composition of SQL ORDER BY clauses in dynamic query builders or metadata-driven pipelines.
    /// The <c>OrderBy</c> method allows specifying both the target column and the sort direction (ascending or descending).
    /// </remarks>
    public interface IQuerySortable<T, J> where T: class
    {
        T OrderBy(J OrderBy, OrderByModes OrderMode);
    }

    public interface IQuerySortable<TClass, TPrimary, TJoined> where TClass : class
    {
        TClass OrderBy(TPrimary Column, OrderByModes OrderMode);
        TClass OrderBy(TJoined Column, OrderByModes OrderMode);
    }

    /// <summary>
    /// Defines a contract for applying ORDER BY clauses to SQL queries across primary and joined entity contexts.
    /// </summary>
    /// <typeparam name="TClass">
    /// The command type implementing this interface, used to enable fluent chaining.
    /// </typeparam>
    /// <typeparam name="TPrimary">
    /// The type representing column identifiers from the primary table, typically an enum or metadata descriptor.
    /// </typeparam>
    /// <typeparam name="TJoined">
    /// The type representing column identifiers from the joined table, typically an enum or metadata descriptor.
    /// </typeparam>
    /// <remarks>
    /// This interface supports fluent composition of SQL ORDER BY clauses in dynamic query builders or metadata-driven pipelines.
    /// It enables sorting by columns from either the primary or joined entity, with explicit control over sort direction.
    /// </remarks>
    public interface IJoinable<TClass, TPrimary, TJoined>
    {
        TClass Join(JoinModes JoinMode, TPrimary OnLeft, TJoined OnRight);
    }

    /// <summary>
    /// Defines a contract for composing SQL JOIN clauses using join mode, target table, and join conditions.
    /// </summary>
    /// <typeparam name="TClass">
    /// The command type implementing this interface, used to enable fluent chaining.
    /// </typeparam>
    /// <remarks>
    /// This interface supports fluent composition of SQL JOIN operations in dynamic query builders or metadata-driven pipelines.
    /// The <c>Join</c> method allows specifying the join type (e.g., INNER, LEFT), the table to join, and the ON clause conditions.
    /// </remarks>
    public interface IJoinable<TClass>
    {
        TClass Join(JoinModes JoinMode, string JoinTable, string OnLeft, string OnRight);
    }

    /// <summary>
    /// Defines a contract for applying update operations to SQL statements using structured metadata carriers.
    /// </summary>
    /// <typeparam name="TClass">
    /// The command type implementing this interface, used to enable fluent chaining.
    /// </typeparam>
    /// <typeparam name="TCarrier">
    /// The type representing update metadata, typically a struct or class that encapsulates column, value, and type information.
    /// </typeparam>
    /// <remarks>
    /// This interface supports fluent composition of SQL <c>UPDATE</c> statements in dynamic query builders or metadata-driven pipelines.
    /// The <c>Set</c> methods allow specifying one or more update targets using strongly typed metadata.
    /// </remarks>
    public interface IUpdateable<TClass, TCarrier>
        where TClass: class
    {
        TClass Set(TCarrier UpdateData);
        TClass Set(IEnumerable<TCarrier> UpdateData);
        TClass Set(params TCarrier[] UpdateData);
    }

    /// <summary>
    /// Defines a contract for SQL query builders that support combining results 
    /// from multiple queries using <c>UNION</c> and <c>UNION ALL</c>.
    /// </summary>
    /// <typeparam name="TClass">
    /// The type returned by the union operations, typically the fluent query builder instance.
    /// </typeparam>
    /// <remarks>
    /// Implementations of this interface provide methods to merge query results 
    /// either with distinct rows (<c>UNION</c>) or with duplicates preserved (<c>UNION ALL</c>).
    /// </remarks>
    public interface IUnionable<TClass>
    {
        TClass Union<J>(SelectCommand<J> Query) where J : Enum;
        TClass UnionAll<J>(SelectCommand<J> Query) where J : Enum;

    }

    /// <summary>
    /// Defines a contract for grouping operations in a query builder, allowing 
    /// grouping by one or multiple columns.
    /// </summary>
    /// <typeparam name="TClass">
    /// The type of the class implementing this interface. Must be a reference type.
    /// </typeparam>
    /// <typeparam name="T">
    /// The type representing a column identifier, typically an enum mapping to table columns.
    /// </typeparam>
    /// <remarks>
    /// Implementations of this interface provide methods to append <c>GROUP BY</c> clauses 
    /// to a SQL command. The interface supports grouping by a single column or by multiple 
    /// columns supplied as an <see cref="IEnumerable{T}"/>.
    /// </remarks>
    public interface IGroupable<TClass, T> where TClass: class
    {
        TClass GroupBy(T Col);
        TClass GroupBy(IEnumerable<T> Cols);
    }

    /// <summary>
    /// Defines a contract for adding <c>HAVING</c> clauses to a SQL query builder, 
    /// enabling conditional filtering on grouped results.
    /// </summary>
    /// <typeparam name="TClass">
    /// The type of the class implementing this interface. Must be a reference type.
    /// </typeparam>
    /// <remarks>
    /// Implementations of this interface provide methods to append <c>HAVING</c> clauses 
    /// to a SQL command. The interface supports both starting a <c>HAVING</c> clause 
    /// and applying conditions with operators.
    /// </remarks>
    public interface ISQLHaving<TClass> where TClass: class
    {
        TClass Having { get; }
        TClass HavingCondition(SQLOperator Op, object R);
    }

    /// <summary>
    /// Defines a contract for adding <c>EXISTS</c> and <c>WHERE EXISTS</c> clauses 
    /// to a SQL query builder, enabling conditional checks against subqueries.
    /// </summary>
    /// <typeparam name="TClass">
    /// The type of the class implementing this interface. Must be a reference type.
    /// </typeparam>
    /// <typeparam name="T">
    /// The enum type representing the table columns used in the subquery.
    /// </typeparam>
    /// <remarks>
    /// Implementations of this interface provide methods to append <c>EXISTS</c> 
    /// and <c>WHERE EXISTS</c> clauses to a SQL command. These clauses allow 
    /// filtering or validating rows based on the existence of records returned 
    /// by a subquery.
    /// </remarks>
    public interface ISQLExists<TClass, T> 
        where TClass: class
        where T: Enum
    {
        TClass WhereExists(SelectCommand<T> TestQry);
        TClass Exists(SelectCommand<T> TestQry);
    }

    /// <summary>
    /// Defines a contract for adding <c>BETWEEN</c> clauses to a SQL query builder, 
    /// enabling range‑based filtering on column values.
    /// </summary>
    /// <typeparam name="TClass">
    /// The type of the class implementing this interface. Must be a reference type.
    /// </typeparam>
    /// <typeparam name="T">
    /// The enum type representing the table columns that can be used in the 
    /// <c>BETWEEN</c> condition.
    /// </typeparam>
    /// <remarks>
    /// Implementations of this interface provide methods to append <c>BETWEEN</c> 
    /// and <c>WHERE BETWEEN</c> clauses to a SQL command. These clauses allow 
    /// filtering rows where a column value falls within a specified range.
    /// </remarks>
    public interface ISQLBetween<TClass, T>
        where TClass: class
        where T: Enum
    {
        TClass WhereBetween(T Col, object L, object R);
        TClass Between(T Col, object L, object R);
    }

    /// <summary>
    /// Defines a contract for adding <c>ANY</c> and <c>ALL</c> clauses to a SQL query builder, 
    /// enabling comparisons of a column against the results of a subquery.
    /// </summary>
    /// <typeparam name="TClass">
    /// The type of the class implementing this interface. Must be a reference type.
    /// </typeparam>
    /// <typeparam name="T">
    /// The enum type representing the table columns that can be used in the 
    /// <c>ANY</c> or <c>ALL</c> conditions.
    /// </typeparam>
    /// <remarks>
    /// Implementations of this interface provide methods to append <c>WHERE ANY</c>, 
    /// <c>ANY</c>, <c>WHERE ALL</c>, and <c>ALL</c> clauses to a SQL command. 
    /// These clauses allow filtering rows by checking whether a column value 
    /// satisfies a comparison against one or all values returned by a subquery.
    /// </remarks>
    public interface ISQLAnyAll<TClass, T>
        where TClass: class
        where T: Enum
    {
        TClass WhereAny(T Col, SQLOperator Op, SelectCommand<T> Subq);
        TClass Any(T Col, SQLOperator Op, SelectCommand<T> Subq);
        TClass WhereAll(T Col, SQLOperator Op, SelectCommand<T> Subq);
        TClass All(T Col, SQLOperator Op, SelectCommand<T> Subq);
    }
}
