using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace FiddlerWCAT.Helper
{
    public class BatchCommand
    {
        public static void Execute(string command, string workingFolder)
        {
            const string tempBatchFile = "batch_temp.bat";
            var path = String.Format(@"{0}\ubrs\", workingFolder);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path = path + tempBatchFile;


            var fs = new FileStream(path, FileMode.Create);
            fs.Write(Encoding.UTF8.GetBytes(command), 0, Encoding.UTF8.GetByteCount(command));
            fs.Close();

            var processInfo = new ProcessStartInfo
            {
                WorkingDirectory = workingFolder,
                FileName = "cmd.exe",
                Arguments = "/c \"" + path + "\""
            };

            Process.Start(processInfo);
        }
    }
}
