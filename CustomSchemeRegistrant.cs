using System;
using System.Diagnostics;
using System.IO;
using Godot;
using Microsoft.Win32;

namespace CustomSchemePracticeForGodot;

public partial class CustomSchemeRegistrant : Node
{
    public override void _Ready()
    {
        GD.Print("[CustomSchemeRegistrant] Registering custom scheme...");
        var currentExecutablePath = OS.GetExecutablePath();
        GD.Print($"[CustomSchemeRegistrant] Current executable path: {currentExecutablePath}");
        GD.Print($"[CustomSchemeRegistrant] OS: {OS.GetName()}");

        if (OperatingSystem.IsWindows())
        {
            var currentExecutablePathForWindows = OS.GetExecutablePath().Replace('/', '\\');
            GD.Print($"[CustomSchemeRegistrant] Current executable path: {currentExecutablePathForWindows}");

            var protocolKey = Registry.CurrentUser.CreateSubKey(@"Software\Classes\godot-scheme-test");
            protocolKey!.SetValue("URL Protocol", "", RegistryValueKind.String);

            var commandKey =
                Registry.CurrentUser.CreateSubKey(@"Software\Classes\godot-scheme-test\shell\open\command");
            commandKey!.SetValue("", $"{currentExecutablePathForWindows} %1", RegistryValueKind.String);

            GD.Print($"[CustomSchemeRegistrant] Done.");
        }
        else if (OperatingSystem.IsLinux())
        {
            var homePath = OS.GetEnvironment("HOME");
            File.WriteAllText($"{homePath}/.local/share/applications/godot-scheme-test.desktop", $"""
                                                                                       [Desktop Entry]
                                                                                       Type=Application
                                                                                       Name=CustomSchemePracticeForGodot
                                                                                       Exec={currentExecutablePath} %u
                                                                                       MimeType=x-scheme-handler/godot-scheme-test
                                                                                       Terminal=false
                                                                                       """);
            var setDefaultProcessInfo = new ProcessStartInfo("xdg-mime", ["default", "~/.local/share/applications/godot-scheme-test.desktop", "x-scheme-handler/godot-scheme-test"]);
            var setDefaultProcess = Process.Start(setDefaultProcessInfo);
            setDefaultProcess!.WaitForExit();
            if (setDefaultProcess.ExitCode != 0)
            {
                GD.Print($"[CustomSchemeRegistrant] Failed.");
                return;
            }
            GD.Print($"[CustomSchemeRegistrant] Done.");
        }
        else
        {
            GD.Print($"[CustomSchemeRegistrant] OS not supported, skipped.");
        }
    }
}
