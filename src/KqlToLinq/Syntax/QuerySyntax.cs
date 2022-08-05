using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KqlToLinq.Syntax;

internal class QuerySyntax
{
    public Token PropertyColumnName { get; set; }

    public Token Property { get; set; }
}
