using System.Text.RegularExpressions;

public class RangeConditionStrategy : IConditionStrategy
{
    public bool Evaluate(Condition condition, int resultInfoId)
    {
        // koşul -> en küçük ve en büyük değerleri çıkarma
        var matches = Regex.Matches(condition.Value, @"\d+");

        if (matches.Count >= 2)
        {
            int minValue = int.Parse(matches[0].Value);
            int maxValue = int.Parse(matches[1].Value);

            // yapılacak işlem varsa burada yap

            Console.WriteLine($"en küçük değer: {minValue}, en büyük değer: {maxValue}");
            return true;
        }

        return false;
    }
}
