using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace KaraokeN4
{
    public sealed partial class ItemViewer : UserControl
    {
        public ItemViewer()
        {
            this.InitializeComponent();
        }

        public void ShowDetails(Song item)
        {
            _item = item;

            ///favoriteImage.
            txtSongID.Text = _item.ID;
            txtSongVol.Text = _item.Vol;
            txtSongTitle.Text = _item.Title;
            txtSongLyric.Text = _item.Lyrics;
        }
        
        public void ClearData()
        {
            _item = null;
            favoriteImage.ClearValue(Image.SourceProperty);
            txtSongID.ClearValue(TextBlock.TextProperty);
            txtSongVol.ClearValue(TextBlock.TextProperty);
            txtSongTitle.ClearValue(TextBlock.TextProperty);
            txtSongLyric.ClearValue(TextBlock.TextProperty);
        }

        private Song _item;
    }
}
