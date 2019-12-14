using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Caliburn.Micro;
using WinParty.Models;

namespace WinParty.ViewModels
{
    [Export]
    public class ShellViewModel : Conductor<object>, IHandle<LoginMessage>, IHandle<LogoutMessage>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly LoginViewModel _loginViewModel;
        private readonly ServersViewModel _serversViewModel;

        [ImportingConstructor]
        public ShellViewModel(IEventAggregator eventAggregator, 
            LoginViewModel loginViewModel, 
            ServersViewModel serversViewModel)
        {
            _eventAggregator = eventAggregator;
            _loginViewModel = loginViewModel;
            _serversViewModel = serversViewModel;
            eventAggregator.Subscribe(this);
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            ActivateItem(_loginViewModel);
        }

        public void Handle(LoginMessage message)
        {
            ActivateItem(_serversViewModel);
        }

        public void Handle(LogoutMessage message)
        {
            ActivateItem(_loginViewModel);
        }
    }
}