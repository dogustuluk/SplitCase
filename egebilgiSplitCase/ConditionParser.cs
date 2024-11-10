using System.Text.RegularExpressions;

public enum ConditionType
{
    Default,
    Range
}


public class Condition
{
    public string PieceName { get; set; }
    public string Operator { get; set; }
    public string Value { get; set; }
    public ConditionType ConditionType { get; set; }
}


public interface IConditionParser
{
    List<Condition> ParseConditions(string conditionString);
}


public class ConditionParser : IConditionParser
{
    private static readonly Regex ConditionRegex = new Regex(
        @"([A-Za-z0-9\-]+|<=|>=|=>|=<|[<>=]=?|!=)",
        RegexOptions.Compiled);

    public List<Condition> ParseConditions(string conditionString)
    {
        var conditions = new List<Condition>();
        var condParts = conditionString.Split('/');
        foreach (var cond in condParts)
        {
            var input = cond.Replace("(", "").Replace(")", "");
            var matches = ConditionRegex.Matches(input);

            if (matches.Count >= 3)
            {
                conditions.Add(new Condition
                {
                    PieceName = matches[0].Value,
                    Operator = matches[1].Value,
                    Value = matches[2].Value
                });
            }
            else if (matches.Count >= 1)
            {
                // formatlı koşul
                conditions.Add(new Condition
                {
                    PieceName = null,
                    Operator = null,
                    Value = input
                });
            }
        }
        return conditions;
    }
}
