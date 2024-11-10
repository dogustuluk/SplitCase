public class DefaultConditionStrategy : IConditionStrategy
{
    private readonly ApplicationDbContext _context;
    private readonly IConditionEvaluator _conditionEvaluator;

    public DefaultConditionStrategy(ApplicationDbContext context, IConditionEvaluator conditionEvaluator)
    {
        _context = context;
        _conditionEvaluator = conditionEvaluator;
    }

    public bool Evaluate(Condition condition, int resultInfoId)
    {
        var relatedResult = _context.Results
            .FirstOrDefault(r => r.ResultInfoId == resultInfoId && r.PieceName == condition.PieceName);

        if (relatedResult == null || !double.TryParse(relatedResult.Results, out double actualValue))
        {
            return false;
        }

        return _conditionEvaluator.Evaluate(condition, actualValue);
    }
}
