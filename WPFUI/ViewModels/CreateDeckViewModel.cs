using Caliburn.Micro;
using DataAccessLibrary;
using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPFUI.ViewModels
{
    public class CreateDeckViewModel : Screen, ISaveToDatabase
    {
		private string _deckName;
		public string DeckName
		{
			get
			{
				return _deckName;
			}
			set
			{
				_deckName = value;
                NotifyOfPropertyChange(() => DeckName);                
			}
		}

		public void Save()
		{
			if (IsValid() == true)
			{
                DeckModel deck = new DeckModel();

                deck.Name = DeckName;

                MessageBox.Show(deck.Name);
            }			
			else
			{
                MessageBox.Show("Please type at least 3 characters.");
            }			
		}

		public bool IsValid()
		{
			if (DeckName.Length > 2)
			{
				return true;
			}

			return false;
		}
	}
}
