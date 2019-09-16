using System;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;

namespace Game {
    public class Instance {
        public readonly Process Process;
        readonly IClientProxy Client;


        public Instance(IClientProxy client, string guid) {
            Client = client;

            Process = new Process();
            Process.StartInfo.FileName = InstanceHandler.ProgramPath;
            Process.StartInfo.UseShellExecute = false;
            Process.StartInfo.RedirectStandardOutput = true;
            Process.StartInfo.RedirectStandardError = true;
            Process.StartInfo.RedirectStandardInput = true;
            Process.StartInfo.Arguments = $"-web -guid {guid}";
            Process.StartInfo.WorkingDirectory = "C:\\Users\\Henri\\source\\repos\\henri-j.-norden---battleships\\Main\\bin\\Release\\netcoreapp2.2\\win10-x64\\";

            Process.OutputDataReceived += new DataReceivedEventHandler(async (sender, e) => {
                if (e.Data != null) {
                    await Client.SendAsync("Update", InstanceHandler.FormatGameString(e.Data));
                } else {
                    await Client.SendAsync("Update", "Game ended.");
                }
            });
            Process.ErrorDataReceived += new DataReceivedEventHandler(async (sender, e) => {
                if (e.Data != null) {
                    await Client.SendAsync("Log", e.Data);
                }
            });

            Process.Start();

            Process.BeginOutputReadLine();
            Process.BeginErrorReadLine();
        }

        public void WriteChar(char ch) {
            Process.StandardInput.Write(ch);
        }
        
        public void Dispose() {
            if (!Process.HasExited) Process.Kill();
        }

        ~Instance() {
            Dispose();
        }
    }
}
