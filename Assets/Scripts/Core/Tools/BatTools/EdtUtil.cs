namespace Game
{
    public class EdtUtil
    {
        public static System.Diagnostics.Process CreateShellExProcess(string cmd, string args, string workingDir = "")
        {
            var pStartInfo = new System.Diagnostics.ProcessStartInfo(cmd);
            pStartInfo.Arguments = args;
            pStartInfo.CreateNoWindow = false;
            pStartInfo.UseShellExecute = true;
            pStartInfo.RedirectStandardError = false;
            pStartInfo.RedirectStandardInput = false;
            pStartInfo.RedirectStandardOutput = false;
            if (!string.IsNullOrEmpty(workingDir))
                pStartInfo.WorkingDirectory = workingDir;
            return System.Diagnostics.Process.Start(pStartInfo);
        }
        public static void RunBat(string batfile, string args, string workingDir = "")
        {
            var p = CreateShellExProcess(batfile, args, workingDir);
            p.Close();
        }
    }
}
