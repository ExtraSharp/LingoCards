// See https://aka.ms/new-console-template for more information

using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;

Flashcard flashcard = CreateFlashcard();
Console.WriteLine(flashcard.TargetWord);
Console.WriteLine(flashcard.Translation);
Console.ReadLine();

static Flashcard CreateFlashcard()
{
    Flashcard flashcard = new Flashcard();

    Console.WriteLine("New word or phrase: ");
    flashcard.TargetWord = Console.ReadLine();
    Console.WriteLine("Translation: ");
    flashcard.Translation = Console.ReadLine();

    SaveFlashcard(flashcard);
    return flashcard;
}

static void SaveFlashcard(Flashcard flashcard)
{
    SqliteCrud sql = new SqliteCrud(GetConnectionString());
    
    sql.CreateFlashcard(flashcard);    
    Console.WriteLine("Successfully saved!");
}

static string GetConnectionString(string connectionStringName ="Default")
{
    string output = "";

    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json");

    var config = builder.Build();

    output = config.GetConnectionString(connectionStringName);

    return output;
}
