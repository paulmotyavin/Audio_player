using MaterialDesignThemes.Wpf;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.XPath;

namespace WpfApp3
{

    public partial class MainWindow : Window
    {
        string[] files;
        public int counter = 0;
        int i = 0;

        public MainWindow()
        {
            InitializeComponent();
            Play.IsEnabled = false;
            Pause.IsEnabled = false;
            PrevAu.IsEnabled = false;
            NextAu.IsEnabled = false;
            reapeter.IsEnabled = false;
            Mixer.IsEnabled = false;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog { IsFolderPicker = true };
            
            var result = dialog.ShowDialog();
            if (result == CommonFileDialogResult.Ok)
            {

                files = Directory.GetFiles(dialog.FileName, "*.mp3");

                AudioList.ItemsSource = files;
                /*List<string> NameFiles = new List<string>();
                foreach (var file in files)
                {
                    string name = System.IO.Path.GetFileName(file);

                }*/
                media.Source = new Uri(AudioList.Items[i].ToString());
                if (media.Source != null)
                {
                    PlayAudio();
                }
            }
        }
        private void PlayAudio()
        {
            Play.IsEnabled = true;
            Pause.IsEnabled = true;
            PrevAu.IsEnabled = true;
            NextAu.IsEnabled = true;
            reapeter.IsEnabled = true;
            Mixer.IsEnabled = true;
            media.Play();
            Play.Visibility = Visibility.Hidden;
            Pause.Visibility = Visibility.Visible;
        }

        private void StartPauseButton(object sender, RoutedEventArgs e)
        {
            if (counter % 2 == 0)
            {
                media.Pause();
                Pause.Visibility = Visibility.Hidden;
                Play.Visibility = Visibility.Visible;
                counter++;
            }
            else
            {
                media.Play();
                Pause.Visibility = Visibility.Visible;
                Play.Visibility = Visibility.Hidden;
                counter++;
            }
        }

        private void PreviousAudio(object sender, RoutedEventArgs e)
        {
            if (i > 0)
            {
                Pause.Visibility = Visibility.Visible;
                Play.Visibility = Visibility.Hidden;
                counter = 0;
                i--;
                media.Source = new Uri(AudioList.Items[i].ToString());
                media.Play();
            }
    }

        private void NextAudio(object sender, RoutedEventArgs e)
        {
            if (i + 1 < files.Count())
            {
                Pause.Visibility = Visibility.Visible;
                Play.Visibility = Visibility.Hidden;
                counter = 0;
                i++;
                media.Source = new Uri(AudioList.Items[i].ToString());
                media.Play();
            }
                
        }
                
        private void ReapeatAudio(object sender, RoutedEventArgs e)
        {
            /*if (reapeter.IsChecked == true)
            {
                while (true)
                {
                    media.Source = new Uri(AudioList.Items[i].ToString());
                    media.Play();
                    if (reapeter.IsChecked == false)
                        break;
                }
            }*/
        }

        private void PositionAudioSlider(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            media.Position = new TimeSpan(Convert.ToInt64(slider.Value));
        }
        private void MaximumSlider(object sender, RoutedEventArgs e)
        {
            slider.Maximum = media.NaturalDuration.TimeSpan.Ticks;
        }

    }
}
