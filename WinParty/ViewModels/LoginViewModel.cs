using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Caliburn.Micro;
using Contracts;
using Serilog;
using WinParty.Models;
using WinParty.Services;

namespace WinParty.ViewModels
{
    [Export]
    public class LoginViewModel : Screen
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IDataService _dataService;
        private readonly VaultService _vaultService;
        private string _userName;
        private bool _canLogin;

        [ImportingConstructor]
        public LoginViewModel(
            IEventAggregator eventAggregator, 
            IDataService dataService,
            VaultService vaultService)
        {
            _eventAggregator = eventAggregator;
            _dataService = dataService;
            _vaultService = vaultService;
            _vaultService.PasswordChanged += _vaultService_PasswordChanged;
            _canLogin = true;
            _userName = "tesonet";
        }

        private void _vaultService_PasswordChanged(object sender, System.EventArgs e)
        {
            NotifyOfPropertyChange(()=>CanLogin);
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

        public bool CanLogin
        {
            get => _canLogin && !(string.IsNullOrEmpty(UserName)) && !string.IsNullOrEmpty(_vaultService.PlainPassword);
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
            Log.Information("Logging In");

            CanLogin = false;
            bool success = await _dataService.Authenticate(new LoginDetails { Username = userName, Password = _vaultService.PlainPassword });
            CanLogin = true;
            if (success)
                _eventAggregator.PublishOnCurrentThread(new LoginMessage());
        }
    }
}