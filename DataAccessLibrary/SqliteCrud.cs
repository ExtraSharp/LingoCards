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

        public void CreateFlashcard(Flashcard flashcard)
        {
            string sql = "insert into Flashcards (TargetWord, Translation) values (@TargetWord, @Translation)";

            db.SaveData(sql, new { flashcard.TargetWord, flashcard.Translation }, _connectionString);
        }
    }
}
