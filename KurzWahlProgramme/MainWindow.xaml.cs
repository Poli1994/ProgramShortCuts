using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.IO;
using System.Diagnostics;
using System.Text.Json;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KurzWahlProgramme
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Program> programList = new ObservableCollection<Program>();
        private const string jsonFileName = "shortCutProgs.json";

        public MainWindow()
        {
            InitializeComponent();

            // Loading File on Start
            if (!File.Exists(jsonFileName))
            {
                File.Create("shortCutProgs.json").Close();
            }
            string jsonString = File.ReadAllText(jsonFileName);
            if(!String.IsNullOrEmpty(jsonString))
            {
                programList = JsonSerializer.Deserialize<ObservableCollection<Program>>(jsonString);
            }

            programs.ItemsSource = programList;

            if (programList.Count > 0)
            {
                programs.Visibility = Visibility.Visible;
            }
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (newProgramName.Text == "" || newProgramPath.Text == "")
            {
                return;
            }
            else
            {
                programList.Add(new Program(newProgramName.Text, newProgramPath.Text));
                newProgramName.Text = "";
                newProgramPath.Text = "";
            }
            if (programList.Count > 0)
            {
                programs.Visibility = Visibility.Visible;
            }
        }

        private void deleteButton_Click(object sender, RoutedEventArgs e)
        {
            programList.Remove((Program)programs.SelectedItem);
            if (programList.Count == 0)
            {
                programs.Visibility = Visibility.Hidden;
            }
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo start = new ProcessStartInfo(Convert.ToString((programs.SelectedItem as Program).FilePath));
            Process.Start(start);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string jsonString = JsonSerializer.Serialize(programList);
            File.WriteAllText(jsonFileName, jsonString);
        }

        private void showPathButton_Click(object sender, RoutedEventArgs e)
        {
            newProgramName.Text = (programs.SelectedItem as Program).FileName;
            newProgramPath.Text = (programs.SelectedItem as Program).FilePath;
        }

        private void programs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (programs.SelectedItem != null)
            {
                startButton.IsEnabled = true;
                showPathButton.IsEnabled = true;
                deleteButton.IsEnabled = true;
            }
            else
            {
                startButton.IsEnabled = false;
                showPathButton.IsEnabled = false;
                deleteButton.IsEnabled = false;
            }
        }
    }

    [Serializable]
    public class Program
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }

        public Program (string fileName, string filePath)
        {
            FileName = fileName;
            FilePath = filePath;
        }
    }
}
