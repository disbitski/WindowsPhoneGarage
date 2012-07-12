using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace PhoneGarage
{
    public partial class WebBrowser : PhoneApplicationPage
    {
        string url = "http://";

        public WebBrowser()
        {
            InitializeComponent();

            //Get Webpage Url

            if (PhoneApplicationService.Current.State.ContainsKey("url"))
            {
                url = (string)PhoneApplicationService.Current.State["url"];
                PhoneApplicationService.Current.State.Remove("url");
            }

            ApplicationTitle.Text = url;
            this.Loaded += new RoutedEventHandler(WebBrowser_Loaded);

        }

        void WebBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            webBrowser.Navigate(new Uri(url));

        }

     

        private void btnBack_Click(object sender, System.EventArgs e)
        {
        	NavigationService.GoBack();
        }

    }
}
