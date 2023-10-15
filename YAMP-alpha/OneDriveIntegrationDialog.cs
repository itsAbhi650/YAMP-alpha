using KoenZomers.OneDrive.Api;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YAMP_alpha
{
    public partial class OneDriveIntegrationDialog : Form
    {
        private readonly Configuration _configuration;
        private string AuthToken = string.Empty;
        private string RefreshToken = string.Empty;
        private string AccessToken = string.Empty;
        public OneDriveIntegrationDialog()
        {
            InitializeComponent();
            _configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (YAMPVars.OneDriveApi == null)
            {
                InitiateOneDriveApi();
            }
            // First sign the current user out to make sure he/she needs to authenticate again
            var signoutUri = YAMPVars.OneDriveApi.GetSignOutUri();
            AuthenticationBrowser.Navigate(signoutUri);
        }

        /// <summary>
        /// Creates a new instance of the OneDrive API
        /// </summary>
        private void InitiateOneDriveApi()
        {
            // Define the type of OneDrive API to instantiate based on the dropdown list selection
            YAMPVars.OneDriveApi = new OneDriveConsumerApi(_configuration.AppSettings.Settings["OneDriveConsumerApiClientID"].Value, _configuration.AppSettings.Settings["OneDriveConsumerApiClientSecret"].Value);
            if (!string.IsNullOrEmpty(_configuration.AppSettings.Settings["OneDriveConsumerApiRedirectUri"].Value))
            {
                YAMPVars.OneDriveApi.AuthenticationRedirectUrl = _configuration.AppSettings.Settings["OneDriveConsumerApiRedirectUri"].Value;
            }
            YAMPVars.OneDriveApi.ProxyConfiguration = null;
        }

        private async void AuthenticationBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            // Get the currently displayed URL and show it in the textbox
            CurrentUrlTextBox.Text = e.Url.ToString();

            // Check if the current URL contains the authorization token
            AuthorizationCodeTextBox.Text = YAMPVars.OneDriveApi.GetAuthorizationTokenFromUrl(e.Url.ToString());

            AuthToken = AuthorizationCodeTextBox.Text;
            if (!string.IsNullOrEmpty(AuthToken))
            {
                await YAMPVars.OneDriveApi.GetAccessToken();
                if (YAMPVars.OneDriveApi.AccessToken != null)
                {
                    AccessToken = YAMPVars.OneDriveApi.AccessToken.AccessToken;
                    RefreshToken = YAMPVars.OneDriveApi.AccessToken.RefreshToken;
                    _configuration.AppSettings.Settings["OneDriveApiRefreshToken"].Value = RefreshToken;
                    _configuration.Save(ConfigurationSaveMode.Modified);
                    return;
                }
            }
            if (CurrentUrlTextBox.Text.StartsWith(YAMPVars.OneDriveApi.SignoutUri))
            {
                AuthenticationBrowser.Navigate(YAMPVars.OneDriveApi.GetAuthenticationUri());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
