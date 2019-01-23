using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace Weinkeller.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class WeinkellerPage : Page
    {

        List<Wein> WeinList = new List<Wein>();
        List<Wein> WeinListEmpty = new List<Wein>();
        int currentWein;

        public WeinkellerPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Load_data();      
        }

        private async void Load_data()
        {
            string temp_barcode;
            string temp_name;
            string temp_detailname;
            string temp_vendor;
            string temp_origin;
            string temp_descr;
            int temp_quantity;

            string temp_string;

            List<string> filenameList = new List<string>();
            StorageFolder dataFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            IReadOnlyList<StorageFile> fileList = await dataFolder.GetFilesAsync();
            
            foreach (StorageFile file in fileList)
            {
                filenameList.Add(file.Name);
            }

            for (int i = 0; i < filenameList.Count; i++)
            {


                Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile sampleFile = await storageFolder.GetFileAsync(filenameList[i]);

                string text = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);

                temp_barcode = text.Substring(0, text.IndexOf(";"));
                temp_string = text.Substring(text.IndexOf(";") + 1);
                temp_name = temp_string.Substring(0, temp_string.IndexOf(";"));
                temp_string = temp_string.Substring(temp_string.IndexOf(";") + 1);
                temp_detailname = temp_string.Substring(0, temp_string.IndexOf(";"));
                temp_string = temp_string.Substring(temp_string.IndexOf(";") + 1);
                temp_vendor = temp_string.Substring(0, temp_string.IndexOf(";"));
                temp_string = temp_string.Substring(temp_string.IndexOf(";") + 1);
                temp_origin = temp_string.Substring(0, temp_string.IndexOf(";"));
                temp_string = temp_string.Substring(temp_string.IndexOf(";") + 1);
                temp_descr = temp_string.Substring(0, temp_string.IndexOf(";"));
                temp_string = temp_string.Substring(temp_string.IndexOf(";") + 1);
                temp_quantity = Convert.ToInt32(temp_string);

                if(temp_quantity != 0)
                    WeinList.Add(new Wein(temp_barcode, temp_name, temp_detailname, temp_vendor, temp_origin, temp_descr, temp_quantity));
                else
                    WeinListEmpty.Add(new Wein(temp_barcode, temp_name, temp_detailname, temp_vendor, temp_origin, temp_descr, temp_quantity));
            }

            currentWein = 0;
            Load_Wine(currentWein);
        }

        private void Load_Wine(int wine_index)
        {
            Text_Name.Text = WeinList[wine_index].getName();
            if(Text_Name.Text == null || Text_Name.Text == "")
                Text_Name.Text = WeinList[wine_index].getDetailname();
            text_Vendor.Text = WeinList[wine_index].getVendor();
            text_Origin.Text = WeinList[wine_index].getOrigin();
            text_descr.Text = WeinList[wine_index].getDescr();
            text_Quantity.Text = WeinList[wine_index].getQuantity().ToString();
            text_barcode.Text = WeinList[wine_index].getBarcode();
            text_Quantity.Text = WeinList[wine_index].getQuantity().ToString();

            Load_image(WeinList[wine_index].getBarcode());
            Load_page(wine_index);
        }

        private void Load_page(int wine_index)
        {
            int current_page = wine_index + 1;
            int max_page = WeinList.Count();

            text_page.Text = current_page.ToString() + "/" + max_page;
        }

        private void Load_image(string image_name)
        {
            FileInfo fInfo = new FileInfo("WeinBilder\\" + image_name + ".jpg");
            if (fInfo.Exists)
            {
                var path = Path.Combine(Environment.CurrentDirectory, "WeinBilder", image_name + ".jpg");
                var uri = new Uri(path);

                var bitmap = new BitmapImage(uri);

                WineImage.Source = bitmap;
            }     
        }

        private void Btn_back_Click(object sender, RoutedEventArgs e)
        {
            if (currentWein != 0)
                currentWein--;
            else
                currentWein = WeinList.Count - 1;
            Load_Wine(currentWein);
        }

        private void Btn_next_Click(object sender, RoutedEventArgs e)
        {
            if (currentWein != WeinList.Count - 1)
                currentWein++;
            else
                currentWein = 0;
            Load_Wine(currentWein);
        }
    }
}
