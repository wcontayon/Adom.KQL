// Copyright © 2022 Adom.KQL / wcontayon All rights reserved.

using Adom.KQL.Exceptions;
using Adom.KQL.QueryBuilder;
using Adom.KQL.Syntax;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Adom.KQL;

internal partial class Grammar
{
    /// <summary>
    /// Parser class used to parse the <see cref="Queue{Token}"/>
    /// creted by the <see cref="Lexer"/> and build the <see cref="Expression"/>
    /// </summary>
    internal class Parser
    {
        /// <summary>
        /// Parse the <see cref="PriorityQueue{Token, int}"/> created by the parser
        /// and build the <see cref="Expression"/> tree.
        /// The parse method used the <see href="https://en.wikipedia.org/wiki/Shunting_yard_algorithm">Shunting yard algorithm</see>
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns><see cref="Expression"/> tree</returns>
#pragma warning disable CA1822 // Marquer les membres comme étant static
        internal Expression Parse(PriorityQueue<Token, int> tokens, IQueryExpressionBuilder queryBuilder)
#pragma warning restore CA1822 // Marquer les membres comme étant static
        {
            ArgumentNullException.ThrowIfNull(nameof(tokens));

            // Operators stack
            Stack<Token> operators = new Stack<Token>();
            // QuerySyntax stack
            Stack<Expression> expressions = new Stack<Expression>();

            while (tokens.Count > 0)
            {
                var token = tokens.Peek();

                if (token.Kind.IsLeftToken())
                {
                    // We may be in a query syntax, so we start create the QuerySyntax
                    // QuerySyntax is compose of 3 tokens
                    QuerySyntax querySyntax = new QuerySyntax(
                        SyntaxKind.QuerySyntax, 
                        new Token[] 
                        {
                            tokens.Dequeue(), // Should be the left token (column name)
                            tokens.Dequeue(), // Should be the operator token
                            tokens.Dequeue(), // Should be the right token (constant value)
                        });

                    // we evaluate the query syntax directly
                    expressions.Push(queryBuilder.EvaluateQuerySyntax(querySyntax));
                    continue;
                }

                if (token.Kind == TokenKind.OpenParenthesis || token.Kind.IsBinaryOperand())
                {
                    // Push the open parenthesis to the operator stack
                    operators.Push(tokens.Dequeue());
                    continue;
                }

                if (token.Kind == TokenKind.CloseParenthesis)
                {
                    tokens.Dequeue();

                    // We evaluate the all the expression and the operator
                    // in the concerned stack, until we find the '(' operator
                    var stopEvaluate = false;
                    while (operators.Count > 0 && !stopEvaluate)
                    {
                        var binaryOperator = operators.Pop();
                        if (binaryOperator.Kind != TokenKind.OpenParenthesis)
                        {
                            var rightExpression = expressions.Pop();
                            var leftExpression = expressions.Pop();

                            expressions.Push(queryBuilder.EvaluateBinaryOperand(
                                binaryOperator.Kind switch
                                {
                                    TokenKind.AndOperand => QueryOperandKind.And,
                                    _ => QueryOperandKind.Or,
                                },
                                leftExpression,
                                rightExpression));
                        }
                        else
                        {
                            // Nothing is done. It means that the current expression
                            // is between the parenthesis ()
                            stopEvaluate = true;
                        }
                    }
                }
            }

            // Parse is done. We now evaluate the stacks (operator and expressions)
            while (operators.Count > 0)
            {
                // On each operators peek/pop(), we pop() two expressions to evaluate
                var binaryOperator = operators.Peek();

                if (expressions.Count >= 2 && (binaryOperator.Kind == TokenKind.AndOperand || binaryOperator.Kind == TokenKind.OrOperand))
                {
                    operators.Pop();
                    var rightExpression = expressions.Pop();
                    var leftExpression = expressions.Pop();
                    expressions.Push(queryBuilder.EvaluateBinaryOperand(
                        binaryOperator.Kind switch
                        {
                            TokenKind.AndOperand => QueryOperandKind.And,
                            _ => QueryOperandKind.Or,
                        },
                        leftExpression,
                        rightExpression));
                }

                if (binaryOperator.Kind == TokenKind.OpenParenthesis && operators.Count == 1)
                    break;
            }

            // Whe should have one expression in the stack
            Debug.Assert(expressions.Count == 1, ExceptionMessages.INCORRECT_INPUT);
            Debug.Assert(operators.Count == 0, ExceptionMessages.INCORRECT_INPUT);

            return expressions.Pop();
        }
    }
}
