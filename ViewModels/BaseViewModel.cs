using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PaniersFermiers.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public ICommand RetourCommand { get; }
        public BaseViewModel()
        {
            RetourCommand = new Command(Retour);
        }
        protected void OnPropertyChanged([CallerMemberName] string nom = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nom));
        }
        protected bool SetProperty<T>(ref T champ, T valeur, [CallerMemberName] string nom = "")
        { 
            if(EqualityComparer<T>.Default.Equals(champ, valeur))
            {
            return false;
            }
            champ=valeur;
            OnPropertyChanged(nom);
            return true;
        }
    private async void Retour()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}

