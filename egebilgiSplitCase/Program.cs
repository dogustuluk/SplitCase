using Microsoft.Extensions.DependencyInjection;

namespace egebilgiSplitCase
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = ConfigureServices();

            var app = serviceProvider.GetService<App>();
            app.Run();
        }

        private static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddScoped<App>();
            services.AddScoped<IConditionParser, ConditionParser>();
            services.AddScoped<IConditionEvaluator, ConditionEvaluator>();
            services.AddScoped<IResultFilterService, ResultFilterService>();
            services.AddScoped<IConditionStrategy, DefaultConditionStrategy>();
            services.AddScoped<DefaultConditionStrategy>();
            services.AddScoped<RangeConditionStrategy>();
            services.AddScoped<ConditionStrategyFactory>();

            services.AddScoped<ApplicationDbContext>();

            return services.BuildServiceProvider();
        }

    }

    // Diğer sınıflar ve arayüzler

    public class Result
    {
        public int Id { get; set; }
        public int ResultInfoId { get; set; }
        public string PieceName { get; set; }
        public string Results { get; set; }
        public string Condition { get; set; }
        public string ConditionType { get; set; }
    }

    public class ResultInfo
    {
        public int Id { get; set; }
        public int UnitId { get; set; }
    }

    public class ResultsViewModel
    {
        public ResultInfo ResultInfos { get; set; }
        public List<Result> Results { get; set; }
    }

    // App sınıfı
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

    public class ApplicationDbContext
    {
        public List<Result> Results { get; set; } = new List<Result>();
        public List<ResultInfo> ResultInfos { get; set; } = new List<ResultInfo>();
    }
}
