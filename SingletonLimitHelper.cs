using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;
using Godot;

namespace CustomSchemePracticeForGodot;

public partial class SingletonLimitHelper : Node
{
    private const string PipeName = "PIPE_CUSTOM_SCHEME_PRACTICE_FOR_GODOT";
    private bool _needCloseStream;
    public delegate void MessageFromCustomSchemeHandler(string message);
    public static event MessageFromCustomSchemeHandler MessageFromCustomSchemeEvent;

    public override void _Ready()
    {
        // try to create a client and connect
        var pipeClient = new NamedPipeClientStream($@"\\.\pipe\{PipeName}");
        // if (OperatingSystem.IsWindows())
        // {
        //     var security = new PipeSecurity();
        //     security.AddAccessRule(new PipeAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null), PipeAccessRights.ReadWrite, AccessControlType.Allow));
        //     pipeClient.SetAccessControl(security);
        // }
        try
        {
            GD.Print($@"[SingletonLimitHelper] Trying to connect to \\.\pipe\{PipeName} and send message...");
            pipeClient.Connect(1000);
            pipeClient.Write(Encoding.UTF8.GetBytes(OS.GetCmdlineArgs().Join(";") + "\n"));
            // exit
            pipeClient.Dispose();
            GetTree().Quit();
        }
        catch (TimeoutException)
        {
            GD.Print("[SingletonLimitHelper] Connection timeout...");
        }
        
        // failed, no game running, create server
        var pipeServer = new NamedPipeServerStream(PipeName, PipeDirection.In);
        // if (OperatingSystem.IsWindows())
        // {
        //     var security = new PipeSecurity();
        //     security.AddAccessRule(new PipeAccessRule(new SecurityIdentifier(WellKnownSidType.BuiltinUsersSid, null), PipeAccessRights.ReadWrite, AccessControlType.Allow));
        //     pipeServer.SetAccessControl(security);
        // }
            
        GD.Print("[SingletonLimitHelper] Created new server.");
        Task.Run(() =>
        {
            pipeServer.WaitForConnection();
            GD.Print("[SingletonLimitHelper] Connection received.");
            var stream = new StreamReader(pipeServer);
            while (!_needCloseStream)
            {
                var message = stream.ReadLine();
                GD.Print("[SingletonLimitHelper] Message received: " + message);
                MessageFromCustomSchemeEvent?.Invoke(message);
                GD.Print("[SingletonLimitHelper] Server closed.");
            }
        });
    }

    public override void _ExitTree()
    {
        _needCloseStream = true;
    }
}