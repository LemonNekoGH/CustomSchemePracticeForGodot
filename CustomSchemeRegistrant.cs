using System;
using Godot;
using Microsoft.Win32;

namespace CustomSchemePracticeForGodot;

public partial class CustomSchemeRegistrant : Node
{
    public override void _Ready()
    {
        GD.Print("[CustomSchemeRegistrant] Registering custom scheme...");
        GD.Print($"[CustomSchemeRegistrant] OS: {OS.GetName()}");

        if (OperatingSystem.IsWindows())
        {
            var currentExecutablePath = OS.GetExecutablePath().Replace('/', '\\');
            GD.Print($"[CustomSchemeRegistrant] Current executable path: {currentExecutablePath}");

            var protocolKey = Registry.CurrentUser.CreateSubKey(@"Software\Classes\godot-scheme-test");
            protocolKey!.SetValue("URL Protocol", "", RegistryValueKind.String);

            var commandKey =
                Registry.CurrentUser.CreateSubKey(@"Software\Classes\godot-scheme-test\shell\open\command");
            commandKey!.SetValue("", $"{currentExecutablePath} %1", RegistryValueKind.String);

            GD.Print($"[CustomSchemeRegistrant] Done.");
        }
        else
        {
            GD.Print($"[CustomSchemeRegistrant] OS not supported, skipped.");
        }
    }
}
