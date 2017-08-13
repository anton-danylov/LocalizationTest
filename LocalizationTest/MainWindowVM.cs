using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace LocalizationTest
{
    public enum Language { English, Russian, Polish };

    public class MainWindowVM : INotifyPropertyChanged
    {
        Dictionary<Language, string> _languageMap = 
            new Dictionary<Language, string>() { { Language.English, "en-US" }, { Language.Russian, "ru-RU" }, { Language.Polish, "pl-PL" } };

        private Language _selectedLanguage;
        public Language SelectedLanguage
        {
            get
            {
                return _selectedLanguage;
            }
            set
            {
                _selectedLanguage = value;
                ResourceProvider.ChangeCulture(_languageMap[_selectedLanguage]);

                OnPropertyChanged();
            }
        } 

        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
