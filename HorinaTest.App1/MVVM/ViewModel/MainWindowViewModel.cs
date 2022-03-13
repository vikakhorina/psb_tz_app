using HorinaTest.App1.Core;
using HorinaTest.App1.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace HorinaTest.App1.MVVM.ViewModel
{
    internal class MainWindowViewModel : ObservableObject
    {
        public RelyCommand GetProcessInfoCommand { get; set; }

        public RelyCommand ChooseFileCommand { get; set; }
        public RelyCommand StartAnalyzeCommand { get; set; }

        private string _nameProcesses;
        public string NameProcesses
        {
            get => _nameProcesses;
            set => OnSetNewValue(ref _nameProcesses, value);
        }

        private string _fileName;
        public string FileName
        {
            get => _fileName;
            set => OnSetNewValue(ref _fileName, value);
        }
        public ObservableCollection<UserInfo> Users { get; set; }

        private int _minUserAge;
        public int MinUserAge
        {
            get => _minUserAge;
            set => OnSetNewValue(ref _minUserAge, value);
        }

        private int _maxUserAge;
        public int MaxUserAge
        {
            get => _maxUserAge;
            set => OnSetNewValue(ref _maxUserAge, value);
        }

        private double _avgOfMinMaxUserAge;
        public double AvgOfMinMaxUserAge
        {
            get => _avgOfMinMaxUserAge;
            set => OnSetNewValue(ref _avgOfMinMaxUserAge, value);
        }


        public MainWindowViewModel()
        {
            NameProcesses = "devenv,YourPhone";
            Users = new ObservableCollection<UserInfo>();

            GetProcessInfoCommand = new RelyCommand(
                (param) =>
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(NameProcesses))
                            throw new Exception("Не введены название процессов");

                        string[] processNames = NameProcesses.Split(',');
                        var listProcessInfo = new List<ProcessInfo>();
                        foreach (string processName in processNames)
                        {
                            try
                            {
                                Process[] processes = Process.GetProcessesByName(processName.Trim());
                                foreach (var process in processes)
                                {
                                    int id = process.Id;
                                    string name = process.ProcessName;
                                    string priority = process.PriorityClass.ToString();
                                    listProcessInfo.Add(new ProcessInfo(id, name, priority));
                                }
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show($"{processName} - {e.Message}");
                            }
                        }

                        if (!listProcessInfo.Any())
                            throw new Exception("Не найден ни один процесс");

                        string userDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        string pathToFile = Path.Combine(userDocuments, "process.csv");
                        SaveToFile(listProcessInfo, pathToFile);

                        using Process myProcess = new Process();
                        myProcess.StartInfo.UseShellExecute = true;
                        myProcess.StartInfo.FileName = pathToFile;
                        myProcess.Start();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                });

            ChooseFileCommand = new RelyCommand(
                (param) =>
                {
                    Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                    dlg.FileName = "Список_имен";
                    dlg.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    dlg.DefaultExt = ".csv";
                    dlg.Filter = "Text documents (.csv)|*.csv";

                    bool? result = dlg.ShowDialog();
                    if (result == true)
                        FileName = dlg.FileName;
                });

            StartAnalyzeCommand = new RelyCommand(
                (param) =>
                {
                    Users = new ObservableCollection<UserInfo>();
                    FileInfo fi = new FileInfo(FileName);

                    var encoding = Encoding.GetEncoding(1251);

                    var lines = File.ReadLines(FileName, encoding);
                    foreach (var line in lines)
                    {
                        string[] arr = line.Split(';');
                        string name = arr[0];
                        string birthStr = arr[1];
                        int birth = Convert.ToInt32(birthStr);
                        Users.Add(new UserInfo(name, birth, 60, 65));
                    }
                    OnPropertyChanged("Users");

                    MinUserAge = Users.Min(x => x.Age);
                    MaxUserAge = Users.Max(x => x.Age);
                    AvgOfMinMaxUserAge = (new List<int>() { MinUserAge, MaxUserAge }).Average();
                    //double averageAge = Users.Average(x => x.Age);
                },
                (param) => !string.IsNullOrEmpty(FileName));

        }

        /// <summary>
        /// write data to csv
        /// </summary>
        /// <param name="processInfoList"></param>
        /// <param name="filepath"></param>
        private void SaveToFile(List<ProcessInfo> processInfoList, string filepath)
        {
            try
            {
                using StreamWriter file = new StreamWriter(filepath, false);
                file.WriteLine("Id;ProcessName;PriorityClass");
                foreach (ProcessInfo proces in processInfoList)
                    file.WriteLine($"{proces.Id};{proces.Name};{proces.Prioriry}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
