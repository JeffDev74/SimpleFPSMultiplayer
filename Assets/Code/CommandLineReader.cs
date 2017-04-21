using System;
using UnityEngine;
using UnityEngine.Networking;

public class CommandLineReader : MonoBehaviour
{
    private void Start()
    {
        var headless = false;
        var allArgs = Environment.GetCommandLineArgs();

        for (var i = 0; i < allArgs.Length; ++i)
        {
            var arg = allArgs[i];

            if (arg == "-batchmode" || arg == "-nographics")
            {
                headless = true;
            }
        }

        if (headless)
        {
            NetworkManager.singleton.StartServer();
        }
    }
}