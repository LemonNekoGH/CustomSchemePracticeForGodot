using System;
using System.IO.Pipes;
using System.Text;
using Godot;

namespace CustomSchemePracticeForGodot.Utils;

public static class CallbackChecker
{
    public static bool Check()
    {
        var args = OS.GetCmdlineArgs().Join(";");
        if (args == "") return false; // start game normally

        GD.Print($"[CallbackChecker] {args}");
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
