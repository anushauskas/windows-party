using System;
using System.Security;

namespace WinParty.Services
{
    public class VaultService
    {
        private SecureString _password;

        public SecureString Password
        {
            get => _password;
            set
            {
                if(_password == value)
                    return;
                _password = value;
                OnPasswordChanged();
            }
        }

        public string PlainPassword => new System.Net.NetworkCredential(string.Empty, Password).Password;

        public event EventHandler PasswordChanged;

        private void OnPasswordChanged()
        {
            PasswordChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}