using System.Collections.ObjectModel;
using System.ComponentModel;

namespace XGCalculator
{
    public class ChatViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<string> _chatMessages;
        public ObservableCollection<string> ChatMessages
        {
            get => _chatMessages;
            set
            {
                _chatMessages = value;
                OnPropertyChanged(nameof(ChatMessages));
            }
        }

        public ChatViewModel()
        {
            ChatMessages = new ObservableCollection<string>();
        }

        /// <summary>
        /// Adiciona uma mensagem ao Chat Feed.
        /// </summary>
        /// <param name="mensagem">Mensagem a ser adicionada</param>
        public void AdicionarMensagem(string mensagem)
        {
            ChatMessages.Add(mensagem);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
