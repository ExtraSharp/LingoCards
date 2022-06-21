using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Models;

namespace DataAccessLibrary
{
    public class SqliteCrud
    {
        private readonly string _connectionString;
        private SqliteDataAccess db = new SqliteDataAccess();

        public SqliteCrud(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CreateFlashcard(FlashcardModel flashcard)
        {
            string sql = "insert into Flashcards (TargetWord, Translation) values (@TargetWord, @Translation)";

            db.SaveData(sql, new { flashcard.TargetWord, flashcard.Translation }, _connectionString);

            // Load ID of new card
            sql = "select Id from Flashcards where TargetWord = @TargetWord and Translation = @Translation";
            int cardId = db.LoadData<IdLookupModel, dynamic>(sql, new { flashcard.TargetWord, flashcard.Translation },_connectionString).First().Id;

            sql = "insert into FlashcardDeck (FlashcardId, DeckId) values (@FlashcardId, @DeckId)";
            db.SaveData(sql, new { FlashcardId = cardId, DeckId = flashcard.DeckId }, _connectionString);   
        }

        public void CreateDeck(string name)
        {
            string sql = "insert into Decks (Name) values (@Name)";
            db.SaveData(sql, new { name }, _connectionString);
        }

        public List<DeckModel> ReadAllDecks()
        {
            string sql = "select Id, Name from Decks";

            return db.LoadData<DeckModel, dynamic>(sql, new { }, _connectionString);
        }

        public List<int> ReadFlashcardIdsByDeckId(int deckId)
        {
            string sql = "select FlashcardId from FlashcardDeck where DeckId = @DeckId";

            return db.LoadData<int, dynamic>(sql, new { DeckId = deckId }, _connectionString);
        }


        public FlashcardModel ReadCardByCardId(int cardId)
        {
            string sql = "select TargetWord, Translation from Flashcards where Id = @Id";

            return db.LoadData<FlashcardModel, dynamic>(sql, new { Id = cardId }, _connectionString).FirstOrDefault();
        }

        public DeckModel ReadDeckByName(string name)
        {
            string sql = "select Id, Name from Decks where Name = @Name";

            return db.LoadData<DeckModel, dynamic>(sql, new { Name = name}, _connectionString).FirstOrDefault();
        }
    }
}
