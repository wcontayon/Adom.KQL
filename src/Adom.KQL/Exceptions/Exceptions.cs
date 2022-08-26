// Copyright © 2022 Adom.KQL / wcontayon All rights reserved.

namespace Adom.KQL.Exceptions;

#pragma warning disable CA1032 // Implémenter des constructeurs d'exception standard
public class BadExpressionInputFormatException : Exception
#pragma warning restore CA1032 // Implémenter des constructeurs d'exception standard
{
    /// <summary>
    /// Token with an error on the input
    /// </summary>
    public string? Token { get; set; }

    /// <summary>
    /// Position of the bad token in the input
    /// </summary>
    public int Position { get; set; }

    public BadExpressionInputFormatException():base(){ }

    public BadExpressionInputFormatException(string token, int position, string message) : base(message)
    {
        Token = token;
        Position = position;

        this.Data.Add("Token", token);
        this.Data.Add("Position", position);
    }
}

#pragma warning disable CA1032 // Implémenter des constructeurs d'exception standard
public class QuerySyntaxEpressionException : Exception
#pragma warning restore CA1032 // Implémenter des constructeurs d'exception standard
{
    /// <summary>
    /// Incorrect text that causes the exception
    /// </summary>
    public string? Text { get; set; }

    public QuerySyntaxEpressionException(): base() { }

    public QuerySyntaxEpressionException(string text, string message): base(message)
    {
        Text = text;
        this.Data.Add("Syntaxt", text);
    }
}

#pragma warning disable CA1032 // Implémenter des constructeurs d'exception standard
public class UnknownFieldException : Exception
#pragma warning restore CA1032 // Implémenter des constructeurs d'exception standard
{
    public string? FieldName { get; set; }

    public string? TypeName { get; set; }

    public UnknownFieldException(): base() { }

    public UnknownFieldException(string fieldName, string typeName, string message): base(message)
    {
        FieldName = fieldName;
        TypeName = typeName;
        this.Data.Add("Field", fieldName);
        this.Data.Add("Type", typeName);
    }
}

#pragma warning disable CA1032 // Implémenter des constructeurs d'exception standard
public class UnknownOperatorException : Exception
#pragma warning restore CA1032 // Implémenter des constructeurs d'exception standard
{
    public string? Operator { get; set; }

    public UnknownOperatorException(): base() { }

    public UnknownOperatorException(string @operator, string message) : base(message)
    {
        Operator = @operator;
        this.Data.Add("Operator", @operator);
    }
}
