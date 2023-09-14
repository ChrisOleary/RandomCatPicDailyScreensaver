using HtmlAgilityPack;
using System;
using System.Windows;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Controls;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Threading;
using System.Windows.Media.Imaging;

namespace RandomCatPicDailyScreensaver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SaveAndUpdate_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://genrandom.com/cats/";
            //string url = "https://catoftheday.com/";

            try
            {

                // Configure Selenium WebDriver to use Chrome
                ChromeOptions options = new ChromeOptions();
                options.AddArgument("--headless"); // Run Chrome in headless mode (no GUI)
                options.AddArgument("--ignore-certificate-errors"); // ignore certificate errors
                options.AddLocalStatePreference("browser", new { enabled_labs_experiments = new string[] { "http-auth-committed-interstitials@2" } });
                options.AddArgument("start-maximized");
                options.AddArgument("--allow-running-insecure-content");
                options.AcceptInsecureCertificates = true;
                using (IWebDriver driver = new ChromeDriver(options))
                {
                    // Navigate to the webpage
                    driver.Navigate().GoToUrl(url);

                    // Add a delay to allow the image to load (adjust the time as needed)
                    Thread.Sleep(TimeSpan.FromSeconds(5));

                    // Get the page source after waiting
                    string htmlContent = driver.PageSource;

                    // Parse the HTML using HtmlAgilityPack to find the image URL
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(htmlContent);

                    // Replace this XPath with the actual XPath to the image on the webpage
                    string imageUrl = doc.DocumentNode.SelectSingleNode("//img")?.GetAttributeValue("src", "");

                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        // Download the image
                        byte[] imageBytes = new WebClient().DownloadData(imageUrl);

                        // set the image to preview box
                        BitmapImage bitmapImage = new BitmapImage();
                        using (MemoryStream stream = new MemoryStream(imageBytes))
                        {
                            bitmapImage.BeginInit();
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.StreamSource = stream;
                            bitmapImage.EndInit();
                        }
                        ImagePreview.Source = bitmapImage;

                        // Set the desktop background
                        //Wallpaper.SetWallpaper(imageBytes);
                    }
                    else
                    {
                        MessageBox.Show("Image not found on the webpage.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

       
    }
}
