using egebilgiSplitCase;

public class ResultFilterService : IResultFilterService
{
    private readonly IConditionParser _conditionParser;
    private readonly ConditionStrategyFactory _strategyFactory;
    private readonly ApplicationDbContext _context;

    public ResultFilterService(
        IConditionParser conditionParser,
        ConditionStrategyFactory strategyFactory,
        ApplicationDbContext context)
    {
        _conditionParser = conditionParser;
        _strategyFactory = strategyFactory;
        _context = context;
    }


    public List<Result> FilterResults(List<Result> results, int resultInfoId)
    {
        var filteredResults = new List<Result>(results);

        foreach (var result in results)
        {
            if (!string.IsNullOrEmpty(result.Condition) && result.ConditionType.ToString() == "Kosul")
            {
                var conditions = _conditionParser.ParseConditions(result.Condition);
                bool allConditionsMet = true;

                foreach (var condition in conditions)
                {
                    // koşul tipi
                    condition.ConditionType = DetermineConditionType(condition);

                    // koşula uygun stratejiyi belirle. burada daha generic yapılabilir veya farklı bir yöntem düşün
                    var strategy = _strategyFactory.GetStrategy(condition.ConditionType);

                    if (!strategy.Evaluate(condition, resultInfoId))
                    {
                        allConditionsMet = false;
                        break;
                    }
                }

                if (!allConditionsMet)
                {
                    filteredResults.Remove(result);
                }
            }
        }

        return filteredResults;
    }

    private ConditionType DetermineConditionType(Condition condition)
    {
        // özel koşul tipi belirlemek istersek burada yapılır
        if (condition.Value.Contains("-") && condition.Value.Contains("="))
        {
            return ConditionType.Range;
        }

        return ConditionType.Default;
    }
}
