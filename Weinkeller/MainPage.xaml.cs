using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Weinkeller.Views;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x407 dokumentiert.

namespace Weinkeller
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {


        public MainPage()
        {
            this.InitializeComponent();
            
            MainFrame.Navigate(typeof(WeinkellerPage));
        }

        private void NavMain_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {

        }

        private void NavMain_Loaded(object sender, RoutedEventArgs e)
        {
            // set the initial SelectedItem
            foreach (NavigationViewItemBase item in NavMain.MenuItems)
            {
                if (item is NavigationViewItem && item.Tag.ToString() == "weinkeller")
                {
                    NavMain.SelectedItem = item;
                    break;
                }
            }
            MainFrame.Navigate(typeof(WeinkellerPage));
        }

        private void NavMain_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                MainFrame.Navigate(typeof(SettingsPage));
            }
            else
            {
                TextBlock ItemContent = args.InvokedItem as TextBlock;
                if (ItemContent != null)
                {
                    switch (ItemContent.Tag)
                    {
                        case "Nav_Weinkeller":
                            MainFrame.Navigate(typeof(WeinkellerPage));
                            break;

                        case "Nav_Durchsuchen":
                            MainFrame.Navigate(typeof(DurchsuchenPage));
                            break;

                        case "Nav_Hinzufuegen":
                            MainFrame.Navigate(typeof(HinzufuegenPage));
                            break;

                        case "Nav_Next":
                            MainFrame.Navigate(typeof(WeinkellerPage));
                            break;
                        case "Nav_Back":
                            MainFrame.Navigate(typeof(WeinkellerPage));
                            break;
                    }
                }
            }
        }


    }
}
