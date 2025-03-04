using System.Diagnostics;

Console.WriteLine("你好！");

try
{
    // Anaconda环境中Python解释器的路径
    string pythonPath = @"C:\Windows\System32\cmd.exe";

    // 要运行的Python脚本文件路径
    string scriptPath = @"cmd /k ""D:\Anaconda3\Scripts\activate.bat"" && ";
    string use = "cd .. && "
                  + "cd .. && "
                  + "D: && "
                  + "cd D:\\GithubCode\\NerfApp\\nerf-pytorch-master && "
                  + "conda activate nerf-pytorch && "
                  + "python run_nerf.py --config configs/fern.txt";


    scriptPath = scriptPath + use;

    // 打印要执行的完整命令，方便调试
    Console.WriteLine($"即将执行的命令: {pythonPath} {scriptPath}");

    // 创建ProcessStartInfo对象
    ProcessStartInfo startInfo = new ProcessStartInfo
    {
        FileName = pythonPath,
        Arguments = scriptPath,
        UseShellExecute = false,
        CreateNoWindow = true,
        RedirectStandardOutput = true,
        RedirectStandardError = true
    };

    // 创建Process对象
    using (Process process = new Process())
    {
        process.StartInfo = startInfo;

        // 为进程的输出和错误输出添加事件处理程序
        process.OutputDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                Console.WriteLine($"脚本输出: {e.Data}");
            }
        };
        process.ErrorDataReceived += (sender, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                Console.WriteLine($"脚本错误: {e.Data}");
            }
        };

        // 启动进程
        if (process.Start())
        {
            Console.WriteLine("进程已成功启动。");

            // 开始异步读取输出和错误流
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            // 等待进程结束
            process.WaitForExit();

            Console.WriteLine($"进程已结束，退出代码: {process.ExitCode}");
        }
        else
        {
            Console.WriteLine("无法启动进程。");
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}