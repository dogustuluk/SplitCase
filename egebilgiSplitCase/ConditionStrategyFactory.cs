public class ConditionStrategyFactory
{
    private readonly IConditionStrategy _defaultStrategy;
    private readonly IConditionStrategy _rangeStrategy;

    public ConditionStrategyFactory(
        DefaultConditionStrategy defaultStrategy,
        RangeConditionStrategy rangeStrategy)
    {
        _defaultStrategy = defaultStrategy;
        _rangeStrategy = rangeStrategy;
    }

    public IConditionStrategy GetStrategy(ConditionType conditionType)
    {
        return conditionType switch
        {
            ConditionType.Range => _rangeStrategy,
            _ => _defaultStrategy,
        };
    }
}