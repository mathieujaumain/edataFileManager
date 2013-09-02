﻿using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using EdataFileManager.NdfBin;
using EdataFileManager.NdfBin.Model;
using EdataFileManager.ViewModel.Base;

namespace EdataFileManager.ViewModel
{
    public class ManagerMainViewModel : ViewModelBase
    {
        protected NdfBinManager NdfManager { get; set; }

        public ObservableCollection<NdfFile> Files { get; set; }

        public ICommand ExportNdfCommand { get; set; }
        public ICommand ExportTextureCommand { get; set; }

        public string current_file {get; set; }

        public string export_path { get; set; }


        protected void ExportNdfExecute(object obj)
        {
            
            var file = obj as NdfFile;

            var content = NdfManager.GetNdfContent(file);

            var f = new FileInfo(file.Path);

            using (var fs = new FileStream(export_path + f.Name, FileMode.OpenOrCreate))
            {
                fs.Write(content.Body, 0, content.Body.Length);
                fs.Flush();
            }
        }

        protected void ExportTextureExecute(object obj)
        {
            var file = obj as NdfFile;

            var buffer = NdfManager.GetRawData(file);

            var f = new FileInfo(file.Path);

            using (var fs = new FileStream(export_path + f.Name, FileMode.OpenOrCreate))
            {
                fs.Write(buffer, 0, buffer.Length);
                fs.Flush();
            }
        }

        public ManagerMainViewModel()
        {
            ExportNdfCommand = new ActionCommand(ExportNdfExecute);
            ExportTextureCommand = new ActionCommand(ExportTextureExecute);


            //@"E:\Programme\Steam\SteamApps\common\Wargame Airland Battle\Data\wargame\PC\2100001470\NDF_Win.dat"
            export_path = @"D:\SCIENCE\";

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = @"D:\Steam\SteamApps\common\Wargame Airland Battle\Data\wargame\PC\2060001225\ZZ_3.dat";
            dlg.DefaultExt = ".dat";
            dlg.Filter = "Edat (.dat)|*.dat";

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                current_file = dlg.FileName;
            }
            else
            {
                current_file = @"D:\Steam\SteamApps\common\Wargame Airland Battle\Data\wargame\PC\2060001225\ZZ_3.dat";
            }


            NdfManager = new NdfBinManager(current_file);


            NdfManager.ParseEdataFile();
            Files = NdfManager.Files;



            //foreach (var file in Files)
            //{
            //    var f = new FileInfo(file.Path);

            //    var dirToCreate = Path.Combine("c:\\temp\\", f.DirectoryName);

            //    if (!Directory.Exists(dirToCreate))
            //        Directory.CreateDirectory(dirToCreate);

            //    var buffer = NdfManager.GetRawData(file);
            //    using (var fs = new FileStream(Path.Combine(dirToCreate, f.Name), FileMode.OpenOrCreate))
            //    {
            //        fs.Write(buffer, 0, buffer.Length);
            //        fs.Flush();
            //    }

            //}
        }


    }
}