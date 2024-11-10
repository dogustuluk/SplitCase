using egebilgiSplitCase;

public class ApplicationDbContext
{
    public List<Result> Results { get; set; } = new List<Result>();
    public List<ResultInfo> ResultInfos { get; set; } = new List<ResultInfo>();
}


public class App
{
    private readonly IResultFilterService _resultFilterService;
    private readonly ApplicationDbContext _context;

    public App(IResultFilterService resultFilterService, ApplicationDbContext context)
    {
        _resultFilterService = resultFilterService;
        _context = context;
    }

    public void Run()
    {
        var results = new List<Result>
        {
            new Result
            {
                Id = 1,
                ResultInfoId = 1,
                PieceName = "Test1",
                Results = "15",
                Condition = "Test1>=10",
                ConditionType = "Kosul"
            },
            new Result
            {
                Id = 2,
                ResultInfoId = 1,
                PieceName = "Test2",
                Results = "25",
                Condition = "1-(20-30=a) (10-25)",
                ConditionType = "Kosul"
            },
            new Result
            {
                Id = 3,
                ResultInfoId = 1,
                PieceName = "Test3",
                Results = "5",
                Condition = "Test3<10",
                ConditionType = "Kosul"
            }
        };

        var resultInfo = new ResultInfo
        {
            Id = 1,
            UnitId = 1
        };

        _context.Results.AddRange(results);
        _context.ResultInfos.Add(resultInfo);

        var filteredResults = _resultFilterService.FilterResults(results, resultInfo.Id);

        Console.WriteLine("filtrelenmiş sonuçlar:");
        foreach (var result in filteredResults)
        {
            Console.WriteLine($"ID: {result.Id}, PieceName: {result.PieceName}, Results: {result.Results}");
        }

        Console.WriteLine("çıkmak için bir tuşa basın.");
        Console.ReadKey();
    }
}
