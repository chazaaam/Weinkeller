using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage.Pickers;
using Windows.Devices.PointOfService;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace Weinkeller.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class HinzufuegenPage : Page
    {
        List<Wein> WeinList = new List<Wein>();

        string barcode;
        string error;
        string name;
        string detailname;
        string vendor;
        string origin;
        string descr;
        int quantity;

        BarcodeScanner scanner = null;
        ClaimedBarcodeScanner claimedScanner = null;

        

        public HinzufuegenPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void barcodeLookup()
        {
           

            // Überprüfen ob Barcode bereits vorhanden in eigener Datenbank
            int barcodeID = CheckDataBase(barcode);

            if (barcodeID == -1)
            {
                string url = "http://opengtindb.org/?ean="+ barcode + "&cmd=query&queryid=400000000";
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
                    messageDialog = new MessageDialog("Operation war erfolgreich.\nDatenbank Inhalte wurden geladen.");
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
                    WeinList.Add(new Wein(barcode, name, detailname, vendor, origin, descr, quantity));
                }
                else
                {
                    ManuellesAnlegenCheck();
                }
            }
            else
            {
                WeinList[barcodeID].addBottle();
                barcode = WeinList[barcodeID].getBarcode();
                name = WeinList[barcodeID].getName();
                detailname = WeinList[barcodeID].getDetailname();
                vendor = WeinList[barcodeID].getVendor();
                origin = WeinList[barcodeID].getOrigin();
                descr = WeinList[barcodeID].getDescr();
                quantity = WeinList[barcodeID].getQuantity();

                CreateFile();
                // Auf WeinSeite wechseln
                //MainFrame.Navigate(typeof(WeinkellerPage));
            }
        }

        private async void ManuellesAnlegenCheck()
        {

            var messageCheck = new MessageDialog("Barcode " + barcode + " ist in der Datenbank nicht vorhanden.\n\nManuell anlegen?", "Unbekannter Barcode");
            messageCheck.Commands.Add(new UICommand("Ja", null, 0));
            messageCheck.Commands.Add(new UICommand("Nein", null, 1));

            messageCheck.DefaultCommandIndex = 1;

            var commandChosen = await messageCheck.ShowAsync();

            if (commandChosen.Label == "Ja")
            {
                // Auf manuelles Anlegen Seite wechseln
                text_barcode.Text = barcode;
            }
            else if (commandChosen.Label == "Nein")
            {
                // Auf Hauptseite wechseln
                this.Frame.Navigate(typeof(WeinkellerPage));
            }
        }

        private int CheckDataBase(string e_barcode)
        {

            for (int i = 0; i <= WeinList.Count; i++)
            {
                if (WeinList[i].getBarcode() == e_barcode)
                    return i;
            }

            return (-1);

        }

        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            CreateFile();

        }

        private async void CreateFile()
        {
            try
            {
                string data_string = barcode + ";" + name + ";" + detailname + ";" + vendor + ";" + origin + ";" + descr + ";" + quantity.ToString();
                Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile sampleFile = await storageFolder.CreateFileAsync(barcode + ".txt", Windows.Storage.CreationCollisionOption.ReplaceExisting);
                await Windows.Storage.FileIO.WriteTextAsync(sampleFile, data_string);

                var messageCheck = new MessageDialog("Neuer Wein hinzugefügt", "Speichern");
                messageCheck.Commands.Add(new UICommand("Weiteren Barcode einscannen", null, 0));
                messageCheck.Commands.Add(new UICommand("Zu Hauptseite zurückkehren", null, 1));

                messageCheck.DefaultCommandIndex = 1;

                var commandChosen = await messageCheck.ShowAsync();

                if (commandChosen.Label == "Weiteren Barcode einscannen")
                {
                    // Auf manuelles Anlegen Seite wechseln
                    this.Frame.Navigate(typeof(HinzufuegenPage));
                }
                else if (commandChosen.Label == "Zu Hauptseite zurückkehren")
                {
                    // Auf Hauptseite wechseln
                    this.Frame.Navigate(typeof(WeinkellerPage));
                }
                
            }
            catch(Exception ex)
            {
                var messageCheck = new MessageDialog("Speichern ist fehlgeschlagen\nFehler: " + ex.Message, "Fehler");
                messageCheck.Commands.Add(new UICommand("Zur Eingabe zurückkehren", null, 0));
                messageCheck.Commands.Add(new UICommand("Zur Hauptseite zurückkehren", null, 1));

                messageCheck.DefaultCommandIndex = 1;

                var commandChosen = await messageCheck.ShowAsync();

                if (commandChosen.Label == "Zur Eingabe zurückkehren")
                {
                    // 
                }
                else if (commandChosen.Label == "Zur Hauptseite zurückkehren")
                {
                    // Auf Hauptseite wechseln
                    this.Frame.Navigate(typeof(WeinkellerPage));
                }
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            test_wein();
        }

        private async void test_open()
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile =await storageFolder.GetFileAsync("sample.txt");

            string text = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);
        }

        private void test_wein()
        {
            barcode = "4003301029387";
            name = "test Wein";
            detailname = "test Wein zum testen der Speicherung";
            vendor = "Test Inc.";
            origin = "Testingen";
            descr = "Dieser Wein wurde zum Testen der Speicherung angelegt";
            quantity = 34;

            CreateFile();
        }

        private void Btn_scan_Click(object sender, RoutedEventArgs e)
        {
            Scanner_input();
        }

        private async void  Scanner_input()
        {
            string scanned_barcode = "";

            scanner = await DeviceHelpers.GetFirstBarcodeScannerAsync();

            if (scanner != null)
            {
                // after successful creation, claim the scanner for exclusive use and enable it so that data reveived events are received.
                claimedScanner = await scanner.ClaimScannerAsync();

                if (claimedScanner != null)
                {
                    // It is always a good idea to have a release device requested event handler. If this event is not handled, there are chances of another app can
                    // claim ownsership of the barcode scanner.
                    claimedScanner.ReleaseDeviceRequested += claimedScanner_ReleaseDeviceRequested;

                    // after successfully claiming, attach the datareceived event handler.
                    claimedScanner.DataReceived += claimedScanner_DataReceived;

                    // Ask the API to decode the data by default. By setting this, API will decode the raw data from the barcode scanner and
                    // send the ScanDataLabel and ScanDataType in the DataReceived event
                    claimedScanner.IsDecodeDataEnabled = true;

                    // enable the scanner.
                    // Note: If the scanner is not enabled (i.e. EnableAsync not called), attaching the event handler will not be any useful because the API will not fire the event
                    // if the claimedScanner has not beed Enabled
                    await claimedScanner.EnableAsync();

                    //rootPage.NotifyUser("Ready to scan. Device ID: " + claimedScanner.DeviceId, NotifyType.StatusMessage);
                    Show_Message("Bereit zu scannen. Device ID: " + claimedScanner.DeviceId, "Status");
                    //ScenarioEndScanButton.IsEnabled = true;
                }
                else
                {
                    Show_Message("Claim barcode scanner failed.", "Fehler");

                    //ScenarioStartScanButton.IsEnabled = true;

                    this.Frame.Navigate(typeof(WeinkellerPage));
                }
            }
            else
            {
                //rootPage.NotifyUser("Barcode scanner not found. Please connect a barcode scanner.", NotifyType.ErrorMessage);

                Show_Message("Barcodescanner nicht gefunden.\nBitte einen Barcodescanner anschließen.", "Fehler");
                btn_scanner_stop.IsEnabled = true;
                //ScenarioStartScanButton.IsEnabled = true;
            }
            
        }

        /// <summary>
        /// Event handler for the Release Device Requested event fired when barcode scanner receives Claim request from another application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"> Contains the ClamiedBarcodeScanner that is sending this request</param>
        async void claimedScanner_ReleaseDeviceRequested(object sender, ClaimedBarcodeScanner e)
        {
            // always retain the device
            e.RetainDevice();

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                //rootPage.NotifyUser("Event ReleaseDeviceRequested received. Retaining the barcode scanner.", NotifyType.StatusMessage);
                Show_Message("Event ReleaseDeviceRequested erhalten.\nBarcodescanner wird gehalten", "Status");

            });
        }


        /// <summary>
        /// Event handler for the DataReceived event fired when a barcode is scanned by the barcode scanner
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"> Contains the BarcodeScannerReport which contains the data obtained in the scan</param>
        async void claimedScanner_DataReceived(ClaimedBarcodeScanner sender, BarcodeScannerDataReceivedEventArgs args)
        {
            // need to update the UI data on the dispatcher thread.
            // update the UI with the data received from the scan.
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                // read the data from the buffer and convert to string.
                string ScenarioOutputScanDataLabel = DataHelpers.GetDataLabelString(args.Report.ScanDataLabel, args.Report.ScanDataType);
                string ScenarioOutputScanData = DataHelpers.GetDataString(args.Report.ScanData);
                string ScenarioOutputScanDataType = BarcodeSymbologies.GetName(args.Report.ScanDataType);
            });
        }

        /// <summary>
        /// Reset the Scenario state
        /// </summary>
        private void ResetTheScenarioState()
        {
            if (claimedScanner != null)
            {
                // Detach the event handlers
                claimedScanner.DataReceived -= claimedScanner_DataReceived;
                claimedScanner.ReleaseDeviceRequested -= claimedScanner_ReleaseDeviceRequested;
                // Release the Barcode Scanner and set to null
                claimedScanner.Dispose();
                claimedScanner = null;
            }

            if (scanner != null)
            {
                scanner.Dispose();
                scanner = null;
            }

            // Reset the UI if we are still the current page.
            if (Frame.Content == this)
            {
                Show_Message("Click the start scanning button to begin.", "Status");
                //this.ScenarioOutputScanData.Text = "No data";
                //this.ScenarioOutputScanDataLabel.Text = "No data";
                //this.ScenarioOutputScanDataType.Text = "No data";

                //// reset the button state
                //ScenarioEndScanButton.IsEnabled = false;
                btn_scanner_stop.IsEnabled = false;
                //ScenarioStartScanButton.IsEnabled = true;
            }

            ScanningGrid.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Event handler for End Scan Button Click.
        /// Releases the Barcode Scanner and resets the text in the UI
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScenarioEndScanButton_Click(object sender, RoutedEventArgs e)
        {
            // reset the scenario state
            this.ResetTheScenarioState();
        }


        private async void Show_Message(string Message, string Titel)
        {
            var messageCheck = new MessageDialog(Message, Titel);
            await messageCheck.ShowAsync();
        }
    }
}
