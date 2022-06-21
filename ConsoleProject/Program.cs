// See https://aka.ms/new-console-template for more information

using ConsoleProject;
using DataAccessLibrary;
using DataAccessLibrary.Models;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.Metrics;

DeckModel deck = new DeckModel()
{
    Name = "none"
};

DisplayMainMenu(deck);
Console.ReadLine();

static FlashcardModel CreateFlashcard(DeckModel deck)
{
    FlashcardModel flashcard = new FlashcardModel();
    flashcard.DeckId = deck.Id;

    DisplayFlashcardHeader();

    Console.WriteLine($"Selected deck: " + deck.Name);
    Console.WriteLine();

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

static void DisplayFlashcard(FlashcardModel flashcard)
{
    Console.Clear();    
}

static void SaveFlashcard(FlashcardModel flashcard)
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
    List<MenuItem> menuItems = new List<MenuItem>();

    menuItems.Add(new MenuItem { Name = "Create new deck", IsActive = true, Selected = () => CreateDeck(deck) });
    menuItems.Add(new MenuItem { Name = "Select deck", IsActive = true, Selected = () => SelectDeck(deck) });
    if (deck.Name != "none")
    {
        menuItems.Add(new MenuItem { Name = "Create new flashcard", IsActive = true, Selected = () => CreateFlashcard(deck) });
        menuItems.Add(new MenuItem { Name = "Training", IsActive = true, Selected = () => LoadTrainingDeck(deck) });
    }
    
    Console.Clear();
    Console.WriteLine();

    int counter = 1;

    foreach (MenuItem item in menuItems)
    {
        item.Display(counter);
        counter += 1;
    }
    
    string choice = Console.ReadLine();

    menuItems[int.Parse(choice) - 1].Selected();
}

static void LoadTrainingDeck(DeckModel deck)
{
    Console.Write("Number of cards to train: ");
    string choice = Console.ReadLine();

    List<FlashcardModel> cardsToTrain = LoadDeck(deck).Take(int.Parse(choice)).ToList();

    int correctAnswers = 0;

    foreach (var card in cardsToTrain)
    {
        bool isCorrect = Train(card);
        if (isCorrect == true)
        {
            correctAnswers += 1;
        }
    }
    Console.WriteLine($"Correct answers: { correctAnswers.ToString()} / { cardsToTrain.Count() }");
    Console.WriteLine();
    Console.WriteLine("Press any key to return to main menu");
    Console.ReadLine();
    DisplayMainMenu(deck);
}

static bool Train(FlashcardModel card)
{
    Console.Clear();
    Console.WriteLine(card.TargetWord);
    Console.WriteLine();
    Console.Write("Your answer: ");
    string answer = Console.ReadLine();

    if (answer == card.Translation)
    {
        Console.WriteLine("Correct!");
        Console.WriteLine();
        Console.ReadLine();
        return true;
    }
    else
    {
        Console.WriteLine($"Wrong. Correct answer: { card.Translation }");
        Console.ReadLine();
        return false;
    }
}

static List<FlashcardModel> LoadDeck(DeckModel deck)
{
    SqliteCrud sql = new SqliteCrud(GetConnectionString());

    List<int> flashcardIds = sql.ReadFlashcardIdsByDeckId(deck.Id);

    List<FlashcardModel> output = new List<FlashcardModel>();

    foreach (var f in flashcardIds)
    {
        FlashcardModel card = new();
        card = sql.ReadCardByCardId(f);
        output.Add(card);
    }

    return output;
}

static void SelectDeck(DeckModel deck)
{
    Console.Clear();
    Console.WriteLine();
    Console.WriteLine("Select a card deck");
    
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
