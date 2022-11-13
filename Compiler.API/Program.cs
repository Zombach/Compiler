using Compiler.API;

CreateHostBuilder(args).Build().Run();

static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args)
.ConfigureWebHostDefaults(webBuilder => webBuilder.UseKestrel().UseUrls("http://*:*")
.UseStartup<Startup>());