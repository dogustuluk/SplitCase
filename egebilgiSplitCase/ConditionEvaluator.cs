public class ConditionEvaluator : IConditionEvaluator
{
    public bool Evaluate(Condition condition, double actualValue)
    {
        if (condition.Value.Contains("-"))
        {
            var rangeParts = condition.Value.Split('-');
            if (double.TryParse(rangeParts[0], out double minValue) &&
                double.TryParse(rangeParts[1], out double maxValue))
            {
                return actualValue >= minValue && actualValue <= maxValue;
            }
        }
        else if (condition.Value.Contains(","))
        {
            var values = condition.Value.Split(',').Select(double.Parse);
            return values.Contains(actualValue);
        }
        else
        {
            return EvaluateOperator(condition.Operator, actualValue, double.Parse(condition.Value));
        }

        return false;
    }

    private bool EvaluateOperator(string operatorSymbol, double actualValue, double comparisonValue)
    {
        return operatorSymbol switch
        {
            "<" => actualValue < comparisonValue,
            ">" => actualValue > comparisonValue,
            "<=" or "=<" => actualValue <= comparisonValue,
            ">=" or "=>" => actualValue >= comparisonValue,
            "=" or "==" => actualValue == comparisonValue,
            "!=" => actualValue != comparisonValue,
            _ => throw new InvalidOperationException("geçersiz operatör"),
        };
    }
}
