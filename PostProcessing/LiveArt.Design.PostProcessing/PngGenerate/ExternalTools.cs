using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace LiveArt.Design.PostProcessing.PngGenerate
{
    // helper to execute .exe file (like inkscape.exe )
    public interface IExternalTools
    {
        
        bool CanExecute();
        ExternalToolResult Execute(string commadArguments);
        void SetWorkingFolder(string workingFolderFullPath);

    }

    public class ExternalToolResult
    {
        public bool Success { get; set; }
        public List<string> StdOut { get; set; }
        public List<string> StrError { get; set; } 
    }

    public class ExternalTools:IExternalTools
    {
        private string WorkingFolder;
        private string PathToTools;

        public ExternalTools(string pathToTools)
        {
            PathToTools = pathToTools;
        }

        public bool CanExecute()
        {
            return System.IO.File.Exists(PathToTools);
            
        }

        public ExternalToolResult Execute(string commadArguments)
        {
            var process = new Process();

            process.StartInfo.FileName = PathToTools;
            if (!string.IsNullOrEmpty(commadArguments))
            {
                process.StartInfo.Arguments = commadArguments;
            }

            if(!String.IsNullOrEmpty(this.WorkingFolder))process.StartInfo.WorkingDirectory = this.WorkingFolder;

            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.UseShellExecute = false;

            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardOutput = true;
            //var stdOutput = new StringBuilder();
            var stdOutput = new List<string>();
            var stdErrorOutput=new List<string>();
            process.OutputDataReceived += (sender, args) => { if(args.Data!=null)stdOutput.Add(args.Data); };
            process.ErrorDataReceived += (sender, args) => { if (args.Data != null) stdErrorOutput.Add(args.Data); };

            try
            {
                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();
            }
            catch (Exception e)
            {
                throw new Exception($"OS error while executing '{PathToTools} {commadArguments}' : {e.Message}", e);
            }

            return new ExternalToolResult()
            {
                Success = process.ExitCode == 0,
                StdOut = stdOutput,
                StrError = stdErrorOutput
            };
        }

        public void SetWorkingFolder(string workingFolderFullPath)
        {
            this.WorkingFolder = workingFolderFullPath;
        }
    }
}
