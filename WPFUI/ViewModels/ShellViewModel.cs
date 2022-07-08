using Caliburn.Micro;
using ConsoleProject;
using DataAccessLibrary;
using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUI.ViewModels
{
    public class ShellViewModel : Screen
    {
        #region Private Fields
        private BindableCollection<DeckModel> _decks = new();
        private DeckModel _selectedDeck;
        #endregion
        public ShellViewModel()
		{
			SqliteCrud sql = new SqliteCrud(GlobalConfig.GetConnectionString());
			_decks = new BindableCollection<DeckModel>(sql.ReadAllDecks());
		}

		public bool CreateFlashcardButtonIsVisible { get; set; }
		public DeckModel SelectedDeck
		{
			get { return _selectedDeck; }
			set 
			{ 
				_selectedDeck = value;
                NotifyOfPropertyChange(() => SelectedDeck);
				CreateFlashcardButtonIsVisible = true;
            }
		}						

		public BindableCollection<DeckModel> Decks
		{
			get { return _decks; }
			set { _decks = value; }
		}


	}
}
