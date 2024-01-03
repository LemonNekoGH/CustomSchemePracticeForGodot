using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;
using Godot;
using Environment = System.Environment;

namespace CustomSchemePracticeForGodot.Utils;

public static class CallbackChecker
{
    public static bool Check()
    {
        var args = OS.GetCmdlineArgs().Join(";");
        GD.Print($"[CallbackChecker] Arguments: {args}");
        if (args == "") return false; // start game normally
        
        var currentExecutablePath = OS.GetExecutablePath();
        var isDebug = new FileInfo(currentExecutablePath).Directory!.FullName != Environment.CurrentDirectory;
        if (isDebug && args == "") return false; // start game normally

        try
        {
            // send argument to main process
            using var pipeClient = new NamedPipeClientStream(".", CallbackReceiver.PipeName);
            GD.Print($@"[CallbackChecker] Trying to connect to .\{CallbackReceiver.PipeName} and send message...");
            pipeClient.Connect(1000);
            pipeClient.Write(Encoding.UTF8.GetBytes(args + "\n"));
            GD.Print("[CallbackChecker] Message sent.");
        }
        catch (TimeoutException)
        {
            GD.Print("[CallbackChecker] Connection timeout...");
        }

        return true;
    }
}
