using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;
using Godot;

namespace CustomSchemePracticeForGodot.Utils;

public static class CallbackReceiver
{
    public const string PipeName = "SCHEME_CALLBACK_PRACTICE_FOR_GODOT_PIPE";

    public static async Task<string> ReceiveANewMessage()
    {
        await using var pipeServer = new NamedPipeServerStream(PipeName);
        GD.Print("[CallbackReceiver] Created new server.");
        await pipeServer.WaitForConnectionAsync();
        GD.Print("[CallbackReceiver] Connected to a client.");
        var stream = new StreamReader(pipeServer);
        var message = await stream.ReadLineAsync();
        GD.Print("[CallbackReceiver] Message received: " + message);
        return message;
    }
}
