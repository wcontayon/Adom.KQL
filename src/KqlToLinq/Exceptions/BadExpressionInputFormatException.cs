
namespace KqlToLinq.Exceptions;

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
