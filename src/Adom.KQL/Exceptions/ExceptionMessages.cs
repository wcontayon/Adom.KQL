
namespace Adom.KQL.Exceptions;

internal class ExceptionMessages
{
    public const string PARENTHESIS_NON_CLOSE = "Parenthesis(es) did not closed correctly";
    public const string CLOSED_PARENTHESIS_WITHOUT_THIS_OPENED = "Closed parenthesis(es) found without his opening";
    public const string INVALID_FIELDNAME = "Field name is invalid";
    public const string INVALID_DATEFORMAT = "Date value format is invalid";
    public const string UNKNOW_OPERATOR = "Unknown operator {0} or invalid character used in operator";
    public const string QUERY_SYNTAX_INCORRECT = "Query syntax is incorrect";
    public const string UNKNOWN_FIELDNAME_IN_QUERY = "Cannot find field/property {0} in type {1}";
    public const string INCORRECT_INPUT = "Input is incorrect, the parser cannot build the query";
}
