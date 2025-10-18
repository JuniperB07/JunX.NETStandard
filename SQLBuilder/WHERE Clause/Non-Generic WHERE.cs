using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Text;

namespace JunX.NETStandard.SQLBuilder
{
    /// <summary>
    /// Provides a fluent interface for composing SQL <c>WHERE</c> clauses using raw condition strings and logical operators.
    /// </summary>
    /// <typeparam name="TCommand">
    /// The parent SQL builder type that this clause is attached to, enabling fluent chaining.
    /// </typeparam>
    public class WhereClause<TCommand>
        where TCommand: class
    {
        private readonly TCommand _parent;
        private readonly StringBuilder _cmd;

        /// <summary>
        /// Initializes a new instance of the <see cref="WhereClause&lt;TCommand&gt;"/> class with the specified parent builder and command buffer.
        /// </summary>
        /// <param name="parent">The parent SQL builder instance to return to after clause composition.</param>
        /// <param name="cmd">The <see cref="StringBuilder"/> used to construct the SQL command.</param>
        public WhereClause(TCommand parent, StringBuilder cmd)
        {
            _parent = parent;
            _cmd = cmd;
        }
        /// <summary>
        /// Ends the <c>WHERE</c> clause composition and returns control to the parent SQL builder for continued fluent chaining.
        /// </summary>
        /// <returns>The parent <typeparamref name="TCommand"/> instance.</returns>
        public TCommand EndWhere => _parent;


        /// <summary>
        /// Appends a SQL <c>WHERE</c> clause with the specified raw condition string to the command buffer.
        /// </summary>
        /// <param name="Condition">The raw SQL condition to include in the <c>WHERE</c> clause.</param>
        /// <returns>The current <see cref="WhereClause&lt;TCommand&gt;"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand> Where(string Condition)
        {
            _cmd.Append(" WHERE ").Append(Condition);
            return this;
        }
        /// <summary>
        /// Begins a grouped SQL condition by appending an opening parenthesis followed by the specified raw condition string.
        /// </summary>
        /// <param name="Condition">The raw SQL condition to start within the group.</param>
        /// <returns>The current <see cref="WhereClause&lt;TCommand&gt;"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand> StartGroup(string Condition)
        {
            _cmd.Append(" (").Append(Condition);
            return this;
        }
        /// <summary>
        /// Ends a grouped SQL condition by appending a closing parenthesis to the command buffer.
        /// </summary>
        /// <returns>The current <see cref="WhereClause&lt;TCommand&gt;"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand> EndGroup
        {
            get
            {
                _cmd.Append(")");
                return this;
            }
        }
        /// <summary>
        /// Appends a SQL <c>AND</c> connector to the current <c>WHERE</c> clause, enabling composition of additional conditions.
        /// </summary>
        /// <returns>The current <see cref="WhereClause&lt;TCommand&gt;"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand> And()
        {
            _cmd.Append(" AND ");
            return this;
        }
        /// <summary>
        /// Appends a SQL <c>AND</c> condition to the current <c>WHERE</c> clause using the specified raw condition string.
        /// </summary>
        /// <param name="Condition">The raw SQL condition to append after the <c>AND</c> keyword.</param>
        /// <returns>The current <see cref="WhereClause&lt;TCommand&gt;"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand> And(string Condition)
        {
            _cmd.Append(" AND ").Append(Condition);
            return this;
        }
        /// <summary>
        /// Appends a SQL <c>OR</c> connector to the current <c>WHERE</c> clause, enabling composition of alternative conditions.
        /// </summary>
        /// <returns>The current <see cref="WhereClause&lt;TCommand&gt;"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand> Or()
        {
            _cmd.Append(" OR ");
            return this;
        }
        /// <summary>
        /// Appends a SQL <c>OR</c> condition to the current <c>WHERE</c> clause using the specified raw condition string.
        /// </summary>
        /// <param name="Condition">The raw SQL condition to append after the <c>OR</c> keyword.</param>
        /// <returns>The current <see cref="WhereClause&lt;TCommand&gt;"/> instance for fluent chaining.</returns>
        public WhereClause<TCommand> Or(string Condition)
        {
            _cmd.Append(" OR ").Append(Condition);
            return this;
        }
    }
}
