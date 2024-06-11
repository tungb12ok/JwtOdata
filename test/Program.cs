// See https://aka.ms/new-console-template for more information
using BussinessLogic;
using DataAccess.Models;

Console.WriteLine("Hello, World!");
FUNewsManagementDBContext context = new FUNewsManagementDBContext();
NewsArticleDAO dAO = new NewsArticleDAO(context);
DateTime startDate = DateTime.Parse("02-02-2002");
DateTime endDate = DateTime.Parse("02-02-2022");
var a = dAO.GetArticlesByDateRangeAsync(startDate, endDate);
Console.WriteLine(a);