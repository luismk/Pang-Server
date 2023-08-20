using System;
using System.IO;
using System.Linq;
namespace PangyaAPI.ZIP
{
    public class ZipFiles
    {
        private ZipFile Files { get; set; }
        public string FileName { get; set; }

        public void ReadLoadFile()
        {
            Files.Close();
            try
            {
                if (File.Exists(FileName) && ZipFile.IsZipFile(FileName))
                {
                    Files = ZipFile.Read(FileName);
                }
            }
            catch
            {
                throw new Exception("File not found");
            }
        }

        public void LoadFile(string fileName)
        {
            FileName = fileName;
            try
            {
                if (File.Exists(FileName) && ZipFile.IsZipFile(fileName))
                {
                    Files = ZipFile.Read(FileName);
                }
            }
            catch
            {
                throw new Exception("File not found");
            }
        }

        public byte[] Reader(string FileName, string FileName1 = "")
        {
            try
            {
                var file = Files.Entries.FirstOrDefault(c => c.FileName == FileName);
                if (file == null)
                {
                    file = Files.Entries.FirstOrDefault(c => c.FileName == FileName1);
                }
                var ms = file.OpenReader();
                using (MemoryStream mStream = new MemoryStream())
                {
                    ms.CopyTo(mStream);
                    return mStream.ToArray();
                }
            }
            catch
            {
                throw new Exception("File not found");
            }
        }

        public bool IsLoad()
        {
            if (Files == null)
            {
                System.Windows.Forms.MessageBox.Show(" Please Load IFF First !", "Pangya IFF");
            }

            return Files != null;
        }
        public byte[] CheckFile()
        {
            try
            {
                var file = File.ReadAllBytes(FileName);
                using (MemoryStream mStream = new MemoryStream(file))
                {
                    return mStream.ToArray();
                }
            }
            catch
            {
                throw new Exception("File not found");
            }
        }


        public void ExtractAll(string filepath)
        {
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
            Files.ExtractAll(filepath);
        }

        public void ExtractOne(string filepath)
        {
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
            Files.ExtractSelectedEntries(filepath);
        }

        public void UpdateFile(string filepath)
        {
            if (File.Exists(filepath))
            {
                
                Files.UpdateFile(filepath, "");
            }
        }

        public void IffSave()
        {
            Files.Save();
        }

        public void IffSaveBck()
        {
            try
            {
                var path = Directory.GetCurrentDirectory() + "\\pangya_jp.bak";
                var path_or = Directory.GetCurrentDirectory() + "\\pangya_gb.iff";
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                if (File.Exists(path_or))
                {
                    File.Copy(path_or, path);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
    }
}
