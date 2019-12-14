using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Caliburn.Micro;
using Contracts;
using WinParty.Models;

namespace WinParty.ViewModels
{
    [Export]
    public class LoginViewModel : Screen
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IDataService _dataService;
        private string _userName;
        private string _password;
        private bool _canLogin;

        [ImportingConstructor]
        public LoginViewModel(IEventAggregator eventAggregator, IDataService dataService)
        {
            _eventAggregator = eventAggregator;
            _dataService = dataService;
            _canLogin = true;
        }

        public string UserName
        {
            get => _userName;
            set
            {
                if(_userName == value)
                    return;
                _userName = value;
                NotifyOfPropertyChange(()=>UserName);
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if(_password == value)
                    return;
                _password = value;
                NotifyOfPropertyChange(()=> Password);
            }
        }

        public bool CanLogin
        {
            get => _canLogin && !(string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password));
            set
            {
                if(_canLogin == value)
                    return; 
                _canLogin = value;
                NotifyOfPropertyChange(()=>CanLogin);
            }
        }

        public async Task Login(string userName, string password)
        {
            CanLogin = false;
            bool success = await _dataService.Authenticate(new LoginDetails { Username = userName, Password = password });
            CanLogin = true;
            if (success)
                _eventAggregator.PublishOnCurrentThread(new LoginMessage());
        }
    }
}