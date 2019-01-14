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
        List<Wein> WeinList = new List<Wein>();

        string barcode;
        string error;
        string name;
        string detailname;
        string vendor;
        string origin;
        string descr;

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
                if (item is NavigationViewItem && item.Tag.ToString() == "WeinkellerPage")
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
                    }
                }
            }
        }

        private void barcodeLookup()
        {
            barcode = "4003301029387";

            // Überprüfen ob Barcode bereits vorhanden in eigener Datenbank
            int barcodeID = CheckDataBase(barcode);

            if (barcodeID == -1)
            {
                string url = "http://opengtindb.org/?ean=4003301029387&cmd=query&queryid=400000000";
                WebClient c = new WebClient();
                byte[] response = c.DownloadData(url);
                string result = Encoding.ASCII.GetString(response);



                error = result.Substring(result.IndexOf("error=") + 6);
                error = error.Substring(0, error.IndexOf("---"));
                error = error.Replace("\n", String.Empty);
                name = result.Substring(result.IndexOf("name=") + 5);
                name = name.Substring(0, name.IndexOf("detailname"));
                name = name.Replace("\n", String.Empty);
                detailname = result.Substring(result.IndexOf("detailname=") + 11);
                detailname = detailname.Substring(0, detailname.IndexOf("detailname"));
                detailname = detailname.Replace("\n", String.Empty);
                vendor = result.Substring(result.IndexOf("vendor=") + 7);
                vendor = vendor.Substring(0, vendor.IndexOf("maincat"));
                vendor = vendor.Replace("\n", String.Empty);
                origin = result.Substring(result.IndexOf("origin=") + 7);
                origin = origin.Substring(0, origin.IndexOf("descr"));
                origin = origin.Replace("\n", String.Empty);
                descr = result.Substring(result.IndexOf("descr=") + 5);
                descr = descr.Substring(0, descr.IndexOf("name_en"));
                descr = descr.Replace("\n", String.Empty);

                var messageDialog = new MessageDialog("Unbekannter Fehler: " + error);

                if (error == "0")
                    messageDialog = new MessageDialog("Operation war erfolgreich");
                else if (error == "1")
                    messageDialog = new MessageDialog("Die EAN konnte nicht gefunden werden");
                else if (error == "2")
                    messageDialog = new MessageDialog("Die EAN war fehlerhaft (Checksummenfehler)");
                else if (error == "3")
                    messageDialog = new MessageDialog("Die EAN war fehlerhaft (ungültiges Format / fehlerhafte Ziffernanzahl)");
                else if (error == "4")
                    messageDialog = new MessageDialog("Es wurde eine für interne Anwendungen reservierte EAN eingegeben (In-Store, Coupon etc.)");
                else if (error == "5")
                    messageDialog = new MessageDialog("Zugriffslimit auf die Datenbank wurde überschritten");
                else if (error == "6")
                    messageDialog = new MessageDialog("Es wurde kein Produktname angegeben");
                else if (error == "7")
                    messageDialog = new MessageDialog("Der Produktname ist zu lang (max. 20 Zeichen)");
                else if (error == "8")
                    messageDialog = new MessageDialog("Die Nummer für die Hauptkategorie fehlt oder liegt außerhalb des erlaubten Bereiches");
                else if (error == "9")
                    messageDialog = new MessageDialog("Die Nummer für die zugehörige Unterkategorie fehlt oder liegt außerhalb des erlaubten Bereiches");
                else if (error == "10")
                    messageDialog = new MessageDialog("Unerlaubte Daten im Herstellerfeld");
                else if (error == "11")
                    messageDialog = new MessageDialog("Unerlaubte Daten im Beschreibungsfeld");
                else if (error == "12")
                    messageDialog = new MessageDialog("Daten wurden bereits übertragen");
                else if (error == "13")
                    messageDialog = new MessageDialog("Die UserID/queryid fehlt in der Abfrage oder ist für diese Funktion nicht freigeschaltet");
                else if (error == "14")
                    messageDialog = new MessageDialog("Es wurde mit dem Parameter \"cmd\" ein unbekanntes Kommando übergeben");



                if (error == "0")
                {
                    WeinList.Add(new Wein(barcode, name, detailname, vendor, origin, descr));
                }
                else
                {
                    ManuellesAnlegenCheck();

                }
            }
            else
            {
                WeinList[barcodeID].addBottle();
                // Auf WeinSeite wechseln
                MainFrame.Navigate(typeof(WeinkellerPage));
            }
        }

        private async void ManuellesAnlegenCheck()
        {

            var messageCheck = new MessageDialog("Barcode " + barcode + " ist in der Datenbank nicht vorhanden. \n Manuell anlegen?", "Unbekannter Barcode");
            messageCheck.Commands.Add(new UICommand("Ja", null, 0));
            messageCheck.Commands.Add(new UICommand("Nein", null, 1));

            messageCheck.DefaultCommandIndex = 1;

            var commandChosen = await messageCheck.ShowAsync();

            if(commandChosen.Label == "Ja" )
            {
                // Auf manuelles Anlegen Seite wechseln
                MainFrame.Navigate(typeof(HinzufuegenPage));
            }
            else if(commandChosen.Label == "Nein")
            {
                // Auf Hauptseite wechseln
                MainFrame.Navigate(typeof(WeinkellerPage));
            }
        }

        private int CheckDataBase(string e_barcode)
        {

            for (int i = 0; i <= WeinList.Count; i++)
            {
                if (WeinList[i].getBarcode() == e_barcode)
                    return i;
            }

            return(-1);

        }
    }
}
