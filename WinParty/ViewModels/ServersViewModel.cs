using System.ComponentModel.Composition;
using Caliburn.Micro;
using Contracts;
using WinParty.Models;

namespace WinParty.ViewModels
{
    [Export]
    public class ServersViewModel : Screen
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IDataService _dataService;

        [ImportingConstructor]
        public ServersViewModel(IEventAggregator eventAggregator, IDataService dataService)
        {
            _eventAggregator = eventAggregator;
            _dataService = dataService;
            Servers = new BindableCollection<Server>();
        }
        public BindableCollection<Server> Servers { get; set; }

        protected override async void OnActivate()
        {
            base.OnActivate();
            var servers = await _dataService.GetServerList();
            Servers.AddRange(servers);
        }

        public void Logout()
        {
            _dataService.Logout();
            _eventAggregator.PublishOnCurrentThread(new LogoutMessage());
        }
    }
}