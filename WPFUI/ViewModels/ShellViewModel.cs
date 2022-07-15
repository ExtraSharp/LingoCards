using Caliburn.Micro;
using ConsoleProject;
using DataAccessLibrary;
using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUI.Views;

namespace WPFUI.ViewModels
{
    public class ShellViewModel : Screen
    {
        #region Private Fields
        private BindableCollection<DeckModel> _decks = new();
        private DeckModel _selectedDeck;
		private IWindowManager _windowManager = new WindowManager();
        #endregion
        public ShellViewModel()
		{
			SqliteCrud sql = new SqliteCrud(GlobalConfig.GetConnectionString());
			Decks = new BindableCollection<DeckModel>(sql.ReadAllDecks());

			foreach (var item in Decks)
			{
				int counts = sql.ReadFlashcardIdsByDeckId(item.Id).Count;

				item.TotalCards = counts;
			}
		}

		public bool CreateFlashcardButtonIsVisible { get; set; }
		public DeckModel SelectedDeck
		{
			get { return _selectedDeck; }
			set 
			{ 
				_selectedDeck = value;
                NotifyOfPropertyChange(() => SelectedDeck);
            }
		}						

		public BindableCollection<DeckModel> Decks
		{
			get { return _decks; }
			set { _decks = value; }
		}

        #region Methods
		public void CreateDeck()
		{
            _windowManager.ShowWindowAsync(new CreateDeckViewModel(), null, null);
        }
        #endregion


    }
}
