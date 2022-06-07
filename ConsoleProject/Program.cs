// See https://aka.ms/new-console-template for more information

using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.Metrics;
DeckModel deck = new DeckModel();
DisplayMainMenu(deck);
Console.ReadLine();

static Flashcard CreateFlashcard(DeckModel deck)
{
    Flashcard flashcard = new Flashcard();
    flashcard.DeckId = deck.Id;

    DisplayFlashcardHeader();    

    Console.WriteLine("New word or phrase: ");
    flashcard.TargetWord = Console.ReadLine();
    Console.WriteLine("Translation: ");
    flashcard.Translation = Console.ReadLine();

    try
    {
        SaveFlashcard(flashcard);
        // DisplayFlashcard(flashcard);
        Console.WriteLine("Successfully saved!");
        DisplayFlashcardMenu(deck);
    }
    catch (Exception)
    {
        Console.WriteLine("Flashcard couldn't be created");
    }
    
    return flashcard;
}

static void CreateDeck(DeckModel deck)
{
    Console.Write("Name of new deck: ");
    string name = Console.ReadLine();

    try
    {
        SaveDeck(name);
        Console.WriteLine("Successfully saved!");
        DisplayMainMenu(deck);
    }
    catch (Exception)
    {
        Console.WriteLine("Deck couldn't be created");
    }   
}

static void DisplayFlashcardHeader()
{
    Console.Clear();
    Console.WriteLine();
    Console.WriteLine("=============================================");
    Console.WriteLine("===========Create new flashcard==============");
    Console.WriteLine("=============================================");
    Console.WriteLine();
}

static void DisplayFlashcard(Flashcard flashcard)
{
    Console.Clear();    
}

static void SaveFlashcard(Flashcard flashcard)
{
    SqliteCrud sql = new SqliteCrud(GetConnectionString());    
    sql.CreateFlashcard(flashcard);     
}

static void SaveDeck(string name)
{
    SqliteCrud sql = new SqliteCrud(GetConnectionString());
    sql.CreateDeck(name);
}

static void DisplayFlashcardMenu(DeckModel deck)
{
    Console.WriteLine();
    Console.WriteLine("[1] Create another one");
    Console.WriteLine($"[2] Back to Main Menu");

    string choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            CreateFlashcard(deck);
            break;
        case "2":
            DisplayMainMenu(deck);
            break;
        default:
            Console.WriteLine("Invalid selection. Please try again");
            DisplayMainMenu(deck);
            break;
    }
}

static void DisplayMainMenu(DeckModel deck)
{
    Console.Clear();
    Console.WriteLine();
    Console.WriteLine("[1] Create new deck");
    Console.WriteLine($"[2] Select Deck (currently selected: " + deck.Name + ")");
    Console.WriteLine("[3] Create new flashcard");
    
    string choice = Console.ReadLine();

    switch (choice)
    {
        case "1":
            CreateDeck(deck);
            break;
        case "2":
            SelectDeck(deck);
            break;
        case "3":
            CreateFlashcard(deck);
            break;
        default:
            Console.WriteLine("Invalid selection. Please try again");
            DisplayMainMenu(deck);
            break;
    }    
}

static void SelectDeck(DeckModel deck)
{
    SqliteCrud sql = new SqliteCrud(GetConnectionString());
    List<DeckModel> allDecks = sql.ReadAllDecks();

    int i = 1;

    foreach (var d in allDecks)
    {
        Console.WriteLine($"["+i+"] " + d.Name);
        i += 1;
    }
    Console.WriteLine();
    Console.Write("Your selection: ");
    string choice = Console.ReadLine();

    deck = LookupDeck(allDecks[int.Parse(choice) - 1].Name);
    
    DisplayMainMenu(deck);
}

static DeckModel LookupDeck(string deck)
{
    SqliteCrud sql = new SqliteCrud(GetConnectionString());

    return sql.ReadDeckByName(deck);
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
