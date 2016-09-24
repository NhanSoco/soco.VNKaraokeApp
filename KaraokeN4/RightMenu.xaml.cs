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
    public sealed partial class RightMenu : UserControl
    {
        string RECENT = "Recent";
        string VIEWALL = "ViewAll";
        string NEWVOLUME = "NewVolume";
        string NHACTRE = "NhacTre";
        string NHACNGOAI = "NhacNgoai";
        string NHACTRINH = "NhacTrinh";
        string NHACTRUTINH = "NhacTruTinh";
        string NHACCACHMANG = "NhacCachMang";
        string NHACTHIEUNHI = "NhacThieuNhi";
        string NHACTIENGANH = "NhacTiengAnh";
        
        public RightMenu()
        {
            this.InitializeComponent();
        }

        private void labelButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            string tappedButtonName = (sender as Grid).Tag.ToString();
            if (tappedButtonName == RECENT)
            {

            }
            else if (tappedButtonName == VIEWALL)
            {
                
            }
            else if (tappedButtonName == NEWVOLUME)
            {

            }
            else if (tappedButtonName == NHACTRE)
            {

            }
            else if (tappedButtonName == NHACNGOAI)
            {
                
            }
            else if (tappedButtonName == NHACTRINH)
            {

            }
            else if (tappedButtonName == NHACTRUTINH)
            {

            }
            else if (tappedButtonName == NHACCACHMANG)
            {

            }
            else if (tappedButtonName == NHACTHIEUNHI)
            {

            }
            else if (tappedButtonName == NHACTIENGANH)
            {

            }
        }
    }
}
