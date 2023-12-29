WebApplication
    .CreateBuilder(args)
    .RegisterAllServices()
    .Build()
    .RegisterAllApp()
    .Run();

public partial class Program { }
