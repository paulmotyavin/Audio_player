using MaterialDesignThemes.Wpf;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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
        public List<string> mixer = new List<string>();
        public List<string> paths = new List<string>();
        private Thread timerThread;
        private TimeSpan all_time;
        private TimeSpan duration;
        public ManualResetEvent mre = new ManualResetEvent(false);
        public MainWindow()
        {
            InitializeComponent();
            timerThread = new Thread(new ThreadStart(Thread_ts));
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


                List<string> NameFiles = new List<string>();
                paths = new List<string>();
                foreach (var file in files)
                {
                    string name = System.IO.Path.GetFileName(file);
                    paths.Add(file);
                    NameFiles.Add(name);
                }
                AudioList.ItemsSource = NameFiles;
                media.Source = new Uri(paths[i]);
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
            timerThread.Start();
        }

        private void StartPauseButton(object sender, RoutedEventArgs e)
        {
            if (counter % 2 == 0)
            {
                mre.Reset();
                media.Pause();
                Pause.Visibility = Visibility.Hidden;
                Play.Visibility = Visibility.Visible;
                counter++;
            }
            else
            {
                mre.Set();
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
                media.Source = new Uri(paths[i].ToString());
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
                media.Source = new Uri(paths[i].ToString());
                media.Play();
            }
                
        }

        private void Thread_ts()
        {
            while (true)
            {

                Dispatcher.Invoke(() =>
                {
                    duration = media.Position;
                    StartTb.Text = duration.ToString(@"mm\:ss");
                    EndTb.Text = (all_time - duration).ToString(@"mm\:ss");
                    slider.Value = duration.Ticks;
                });
            }
        }

        private void ReapeatAudio()
        {
            if (reapeter.IsChecked == true)
            {
                media.Position = TimeSpan.Zero;
                media.Play();
            }
        }

        private void PositionAudioSlider(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            media.Position = new TimeSpan(Convert.ToInt64(slider.Value));
        }
        private void MaximumSlider(object sender, RoutedEventArgs e)
        {
            all_time = media.NaturalDuration.TimeSpan;
            slider.Maximum = media.NaturalDuration.TimeSpan.Ticks;
            
        }
        void Mixer_Audio()
        {
            if (Mixer.IsChecked == true)
            {
                Random random = new Random();
                mixer = paths;
                int number = random.Next(mixer.Count);
                media.Source = new Uri(mixer[number].ToString());
                mixer.RemoveAt(number);
            }
        }
        private void media_MediaEnded(object sender, RoutedEventArgs e)
        {
            Mixer_Audio();
            ReapeatAudio();
            if (i + 1 < files.Count())
            {
                i++;
                media.Source = new Uri(paths[i].ToString());
                media.Play();
            }
        }

        private void AudioList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            i = AudioList.SelectedIndex;
            media.Source = new Uri(paths[i].ToString());
            media.Play();
        }
    }
}
