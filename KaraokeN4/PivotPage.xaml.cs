using KaraokeN4.Common;
using KaraokeN4.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Newtonsoft.Json;
using Windows.Storage;
using System.Threading.Tasks;

// The Pivot Application template is documented at http://go.microsoft.com/fwlink/?LinkID=391641

namespace KaraokeN4
{
    public class Song
    {
        public string Vol { get; set; }
        public string ID { get; set; }
        public string Title { get; set; }
        public string Lyrics { get; set; }
        public string Href { get; set; }
    }

    public class GroupInfoList<T> : List<object>
    {

        public object Key { get; set; }


        public new IEnumerator<object> GetEnumerator()
        {
            return (System.Collections.Generic.IEnumerator<object>)base.GetEnumerator();
        }
    }

    public sealed partial class PivotPage : Page
    {
        private const string FirstGroupName = "FirstGroup";
        private const string SecondGroupName = "SecondGroup";

        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        private Dictionary<string, Song> DictSongs { get; set; }
        private List<GroupInfoList<object>> SongGroup { get; set; }

        string BUTTON_LEFT_MENU = "leftMenu";
        string BUTTON_SEARCH_MENU = "searchMenu";
        string BUTTON_FAVORITE_MENU = "favMenu";
        string BUTTON_RIGHT_MENU = "rightMenu";
        string BUTTON_BACK = "backButton";
        string BUTTON_SEARCH_BOX = "searchButton";

        static bool isSearchBarVisible = false;
        static bool isMenuRightVisible = false;

        public PivotPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>.
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            Load();
        }

        private async Task Load()
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(@"ms-appx:///SomeFile.txt"));
            var data = await Windows.Storage.FileIO.ReadTextAsync(file);

            DictSongs = new Dictionary<string, Song>();
            DictSongs.Clear();
            DictSongs = JsonConvert.DeserializeObject<Dictionary<string, Song>>(data);

            // upper case for title
            foreach (var entry in DictSongs)
            {
                entry.Value.Title = entry.Value.Title.TrimStart(' ');
                entry.Value.Title = entry.Value.Title.ToUpper();
            }

            SongGroup = GetGroupsByLetter();
            cvs.Source = SongGroup;

            (semanticZoom.ZoomedOutView as ListViewBase).ItemsSource = cvs.View.CollectionGroups;
            //ItemsGridView.ItemsSource = DictSongs;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache. Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/>.</param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // TODO: Save the unique state of the page here.
        }

        /// <summary>
        /// Adds an item to the list when the app bar button is clicked.
        /// </summary>
        private void AddAppBarButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        /// <summary>
        /// Invoked when an item within a section is clicked.
        /// </summary>
        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Navigate to the appropriate destination page, configuring the new page
            // by passing required information as a navigation parameter
            var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            if (!Frame.Navigate(typeof(ItemPage), itemId))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        /// <summary>
        /// Loads the content for the second pivot item when it is scrolled into view.
        /// </summary>
        private async void SecondPivot_Loaded(object sender, RoutedEventArgs e)
        {
            var sampleDataGroup = await SampleDataSource.GetGroupAsync("Group-2");
            this.DefaultViewModel[SecondGroupName] = sampleDataGroup;
        }


        private List<GroupInfoList<object>> GetGroupsByLetter()
        {
            List<GroupInfoList<object>> groups = new List<GroupInfoList<object>>();

            if (DictSongs.Count == 0)
                return groups;
            

            var query = from item in DictSongs
                        orderby ((Song)item.Value).Title
                        group item by ((Song)item.Value).Title[0] into g
                        select new { GroupName = g.Key, Items = g };
            foreach (var g in query)
            {
                GroupInfoList<object> info = new GroupInfoList<object>();
                info.Key = g.GroupName;
                foreach (var item in g.Items)
                {
                    info.Add(item);
                }
                groups.Add(info);
            }

            return groups;
        }

        void ItemsGridView_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            ItemViewer iv = args.ItemContainer.ContentTemplateRoot as ItemViewer;

            if (args.InRecycleQueue == true)
            {
                iv.ClearData();
            }
            else if (args.Phase == 0)
            {
                KeyValuePair<string, Song> e = (KeyValuePair<string, Song>)args.Item;
                iv.ShowDetails(e.Value);

                // Register for async callback to visualize Title asynchronously
                args.RegisterUpdateCallback(ContainerContentChangingDelegate);
            }

            // For imporved performance, set Handled to true since app is visualizing the data item
            args.Handled = true;
        }

        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> ContainerContentChangingDelegate
        {
            get
            {
                if (_delegate == null)
                {
                    _delegate = new TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs>(ItemsGridView_ContainerContentChanging);
                }
                return _delegate;
            }
        }
        private TypedEventHandler<ListViewBase, ContainerContentChangingEventArgs> _delegate;

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void semanticZoom_ViewChangeStarted(object sender, SemanticZoomViewChangedEventArgs e)
        {
            
        }

        private void SwitchTopBar()
        {
            if (!isSearchBarVisible)
            {
                isSearchBarVisible = true;
                fadeIn.Begin();

                mainBar.Visibility = Visibility.Collapsed;
                searchBar.Visibility = Visibility.Visible;
            }
            else
            {
                fadeOut.Begin();
            }
        }

        private void fadeOut_Completed(object sender, object e)
        {
            isSearchBarVisible = false;
            mainBar.Visibility = Visibility.Visible;
            searchBar.Visibility = Visibility.Collapsed;
        }

        private void ShowMenuRight()
        {
            if (!isMenuRightVisible)
            {
                isMenuRightVisible = true;
                mRIn.Begin();
                menuRightPanel.Visibility = Visibility.Visible;
            }
            else
            {
                mROut.Begin();
            }
        }


        private void mROut_Completed(object sender, object e)
        {
            isMenuRightVisible = false;
            menuRightPanel.Visibility = Visibility.Collapsed;
        }

        private void onBackKeyPressed(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isSearchBarVisible)
            {
                SwitchTopBar();
                e.Cancel = true;
            }
        }

        private void myButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string tappedButtonName = (sender as Grid).Tag.ToString();
            if (tappedButtonName == BUTTON_LEFT_MENU)
            {

            }
            else if (tappedButtonName == BUTTON_SEARCH_MENU)
            {
                SwitchTopBar();
            }
            else if (tappedButtonName == BUTTON_FAVORITE_MENU)
            {

            }
            else if (tappedButtonName == BUTTON_RIGHT_MENU)
            {
                ShowMenuRight();
            }
            else if (tappedButtonName == BUTTON_BACK)
            {
                SwitchTopBar();
            }
            else if (tappedButtonName == BUTTON_SEARCH_BOX)
            {

            }
        }

        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

    }
}
