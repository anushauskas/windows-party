using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WinParty.Services;

namespace WinParty.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    [Export]
    public partial class LoginView : UserControl
    {
        private readonly VaultService _vaultService;

        [ImportingConstructor]
        public LoginView(VaultService vaultService)
        {
            _vaultService = vaultService;
            InitializeComponent();
        }
        
        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
                _vaultService.Password = passwordBox.SecurePassword;
        }
    }
}
