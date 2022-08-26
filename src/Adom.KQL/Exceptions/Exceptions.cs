
namespace Adom.KQL.Exceptions;

public class BadExpressionInputFormatException : Exception
{
    /// <summary>
    /// Token with an error on the input
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// Position of the bad token in the input
    /// </summary>
    public int Position { get; set; }

    public BadExpressionInputFormatException(string token, int position, string message) : base(message)
    {
        Token = token;
        Position = position;

        this.Data.Add("Token", token);
        this.Data.Add("Position", position);
    }
}

public class QuerySyntaxEpressionException : Exception
{
    /// <summary>
    /// Incorrect text that causes the exception
    /// </summary>
    public string? Text { get; set; }

    public QuerySyntaxEpressionException(string text, string message): base(message)
    {
        Text = text;
        this.Data.Add("Syntaxt", text);
    }
}

public class UnknownFieldException : Exception
{
    public string? FieldName { get; set; }

    public string? TypeName { get; set; }

    public UnknownFieldException(string fieldName, string typeName, string message): base(message)
    {
        FieldName = fieldName;
        TypeName = typeName;
        this.Data.Add("Field", fieldName);
        this.Data.Add("Type", typeName);
    }
}

public class UnknownOperatorException : Exception
{
    public string? Operator { get; set; }

    public UnknownOperatorException(string @operator, string message) : base(message)
    {
        Operator = @operator;
        this.Data.Add("Operator", @operator);
    }
}
