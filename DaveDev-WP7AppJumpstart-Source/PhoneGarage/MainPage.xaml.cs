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
using System.Windows.Navigation;
using PhoneGarage.BingAPI2;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Shell;
using Microsoft.Advertising.Mobile.UI;

namespace PhoneGarage
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded +=new RoutedEventHandler(MainPage_Loaded);
            this.BackKeyPress += new EventHandler<System.ComponentModel.CancelEventArgs>(MainPage_BackKeyPress);
        }

        void MainPage_BackKeyPress(object sender, System.ComponentModel.CancelEventArgs e)
        {

            var result = MessageBox.Show("Are you sure you want to exit?  You will lose all unsaved work.", "Exit Application", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                //Exit App
            }
            else
            {
                e.Cancel = true;
            }

        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }

            DisplayAds();
        }


        private void DisplayAds()
        {
            //Test Ads
            //AdControl ad = new AdControl("test_client", // ApplicationID
            //                               "Image480_80",    // AdUnitID
            //                               AdModel.Contextual, // AdModel
            //                               true);         // RotationEnabled
            AdControl.TestMode = false;
            AdControl ad = new AdControl();
            ad.AdUnitId = "46155";
            ad.ApplicationId = "d22bc6bf-5dfd-4b13-b935-416a08be13a3";
            ad.AdModel = AdModel.Contextual;

            //Real Ads
            //AdControl.TestMode = false;
            //AdControl ad = new AdControl();
            //ad.AdUnitId = "<Your AdUnitId>";
            //ad.ApplicationId = "<Your ApplicationId>";
            //ad.AdModel = AdModel.Contextual;

            //Ad Layout and Container
            ad.Width = 480;
            ad.Height = 80;
            ad.HorizontalAlignment = HorizontalAlignment.Left;
            ad.VerticalAlignment = VerticalAlignment.Bottom;
            gridSearch.Children.Add(ad);

        }
			



        private void SubmitSearch(object sender, RoutedEventArgs e)
        {
            //DaveDev AppId - replace 
             //string AppId = "<Your Bing AppID>";
            string AppId = "E63C19DE8387684E648C67D2E6792CD790FB85AB";
             string query = txtSearch.Text;


            BingPortTypeClient client = new BingPortTypeClient();

            SearchRequest request = new BingAPI2.SearchRequest();

            request.AppId = AppId;
            request.Sources = new SourceType[] { SourceType.Web, SourceType.Image };
            request.Query = query;
            request.Image = new ImageRequest();
            request.Image.Count = 50;
            request.Image.CountSpecified = true;

            client.SearchCompleted += new EventHandler<SearchCompletedEventArgs>(WebSearchCompleted);
            client.SearchAsync(request);

            progressBing.IsIndeterminate = true;
            progressBing.Visibility = Visibility.Visible;
        }


        void WebSearchCompleted(object sender, SearchCompletedEventArgs e)
        {
            progressBing.IsIndeterminate = false;
            progressBing.Visibility = Visibility.Collapsed;
            searchPivot.SelectedIndex = 1;

            SearchResponse response = e.Result;
            ImageResponse imageResponse = e.Result.Image;

            SearchResultList searchResult = new SearchResultList();

            if (response.Web.Results.Count() > 0)
            {
                foreach (WebResult result in response.Web.Results)
                {
                    searchResult.Add(new SearchResult
                    {
                        Title = result.Title,
                        URL = result.Url,
                        Description = result.Description
                    });
                }
            }


            webSearchResult.ItemsSource = searchResult;
            imagesSearchResult.Items.Clear();

            foreach (ImageResult bingImage in response.Image.Results)
            {
                Image image = new Image();
                image.Source = new BitmapImage(new Uri(bingImage.Thumbnail.Url));

                image.Width = bingImage.Thumbnail.Width;
                image.Height = bingImage.Thumbnail.Height;

                imagesSearchResult.Items.Add(image);
            }
        }
	

	

        private void txtURL_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

            TextBlock url = sender as TextBlock;

            PhoneApplicationService.Current.State.Add("url", url.Text);

            NavigationService.Navigate(new Uri("/WebBrowser.xaml", UriKind.Relative));
	

        }

        private void btnAbout_Click(object sender, System.EventArgs e)
        {
        	NavigationService.Navigate(new Uri("/About.xaml",UriKind.Relative));
        }
    }
}