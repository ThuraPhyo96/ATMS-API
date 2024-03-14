// See https://aka.ms/new-console-template for more information
using ATMS.ConsoleApp.AdoDotNetExamples;
using ATMS.ConsoleApp.DapperExamples;
using ATMS.ConsoleApp.EFCoreExamples;
using ATMS.ConsoleApp.HttpClientExamples;


//AdoDotNetExample adoDotNetExample = new();
//adoDotNetExample.Run();

//DapperExample dapperExample = new();
//dapperExample.Run();

//EFCoreExample eFCoreExample = new();
//eFCoreExample.Run();


Console.WriteLine("Waiting the API.....");
Console.ReadKey();


HttpClientExample httpClientExample = new();
await httpClientExample.Run();

Console.ReadKey();