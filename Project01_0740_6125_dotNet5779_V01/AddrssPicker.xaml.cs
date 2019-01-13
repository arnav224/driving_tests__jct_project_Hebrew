using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project01_0740_6125_dotNet5779_V01
{
    /// <summary>
    /// Interaction logic for AddressPicker.xaml
    /// </summary>
    public partial class AddressPicker : UserControl
    {
        private string token;
        public string Address 
        {
            set
            {
                TexBoxAddress.TextChanged -= TexBoxAddress_TextChanged;
                TexBoxAddress.Text = (value != null) ? value.ToString() : "";
                //new Thread for Google Address Suggestions 
                new Thread(() =>
                {
                    try
                    {
                        GenerateNewToken();
                        //var text = BE.Tools.Maps_GetPlaceAutoComplete(value.ToString())[0];
                        var text = BE.Tools.GetAddressSuggestionsGoogle(value.ToString(), token).First();
                        Action action = () =>
                        {
                            TexBoxAddress.Text = text;
                            TexBoxAddress.TextChanged += TexBoxAddress_TextChanged;
                            TexBoxAddress.BorderBrush = Brushes.Black;
                        };
                        Dispatcher.BeginInvoke(action);
                    }
                    catch
                    {
                        Action act = () => {
                            TexBoxAddress.BorderBrush = Brushes.Red;
                            TexBoxAddress.TextChanged += TexBoxAddress_TextChanged;
                        };
                        Dispatcher.BeginInvoke(act);
                    }
                }).Start();

            }
            get => TexBoxAddress.Text;
        }

        /// <summary>
        /// Address Picker ctor
        /// </summary>
        public AddressPicker()
        {
            InitializeComponent();
            GenerateNewToken();
        }

        public event EventHandler TextChanged;

        /// <summary>
        /// TexBox Address Text Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TexBoxAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (TexBoxAddress.Text != "")
                {

                    var text = TexBoxAddress.Text;
                    //new Thread for Google Address Suggestions
                    new Thread(() =>
                    {
                        try
                        {
                            var list = BE.Tools.GetAddressSuggestionsGoogle(text, token);
                            if (list.Any(x => x == text))
                            {
                                Action act = () => { ListBoxSuggestions.Visibility = Visibility.Hidden; };
                                Dispatcher.BeginInvoke(act);
                                return;
                            }

                            Action action = () =>
                            {
                                ListBoxSuggestions.ItemsSource = list;
                                ListBoxSuggestions.Visibility = Visibility.Visible;
                                ListBoxSuggestions.UnselectAll();
                                TexBoxAddress.BorderBrush = Brushes.Black;
                            };
                            Dispatcher.BeginInvoke(action);
                        }
                        catch
                        {
                            Action action = () => { TexBoxAddress.BorderBrush = System.Windows.Media.Brushes.Red; };
                            Dispatcher.BeginInvoke(action);
                        }
                    }).Start();

                    TextChanged(this, e);
                }
            }
            catch { }
        }

        /// <summary>
        /// User Control Lost Focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            ListBoxSuggestions.Visibility = Visibility.Collapsed;
            if (ListBoxSuggestions.SelectedItem == null)
            {
                try
                {
                    TexBoxAddress.Text = BE.Tools.GetAddressSuggestionsGoogle(TexBoxAddress.Text, token).First();
                }
                catch (Exception)
                {
                    TexBoxAddress.Text = null;
                }
            TextChanged(this, e);
            }
        }

        /// <summary>
        /// List Box Suggestions Selection Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBoxSuggestions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBoxSuggestions.SelectedItem != null)
            {
                TexBoxAddress.Text = (string)ListBoxSuggestions.SelectedItem;
                ListBoxSuggestions.Visibility = Visibility.Collapsed;
                GenerateNewToken();
            }
        }

        /// <summary>
        /// Generate New Token to google service
        /// </summary>
        private void GenerateNewToken()
        {
            token = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("=", "").Replace("+", "")
                .Replace(@"\", "").Replace("/", "").Replace(".", "").Replace(":", "");
        }
    }
}
