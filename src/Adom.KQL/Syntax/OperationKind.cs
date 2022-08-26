
namespace Adom.KQL.Syntax;

internal enum OperatorKind
{
    Equals,
    NotEquals,
    GreaterThan,
    GreaterThanOrEqual,
    LessThan,
    LessThanOrEqual,
    Contains,
    StartWith,
    EndWith
}

internal enum QueryOperandKind
{
    Or,
    And,
}
