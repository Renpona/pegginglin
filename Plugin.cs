using BepInEx;
using HarmonyLib;
using BepInEx.Logging;
using System;
using Buttplug.Client;
using Buttplug.Client.Connectors.WebsocketConnector;
using System.Timers;
using System.Threading.Tasks;
using UnityEngine;

namespace PeglinCore
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInProcess("Peglin.exe")]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony(PluginInfo.PLUGIN_GUID);
        internal static ManualLogSource Log;
        internal static ButtplugClient client;
        internal static Timer timer;
        internal static bool quitting = false;

        private void Awake()
        {
            harmony.PatchAll();
            Log = base.Logger;
            Log.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            client = new ButtplugClient("Pegginglin");
            
            Task.Run(async () => {
                while (true)
                {
                    Log.LogInfo("Starting Buttplug Client");
                    if (quitting)
                    {
                        return;
                    }
                    if (!client.Connected)
                    {
                        try
                        {
                            await client.ConnectAsync(new ButtplugWebsocketConnector(new Uri("ws://127.0.0.1:12345")));
                            Log.LogInfo("Buttplug Client Started");
                        }
                        catch (Exception ex)
                        {
                            Log.LogError(ex);
                        }
                    }

                    // Wait for 5 seconds then try connecting again.
                    await Task.Delay(5000);
                }
            });

            Application.quitting += OnApplicationQuit;
            
            timer = new Timer();
            timer.Elapsed += OnTimerElapsed;
        }

        static void OnApplicationQuit()
        {
            Log.LogInfo("Application quitting, spinning down buttplug client");
            quitting = true;
            if (client != null)
            {
                Task.Run(async () =>
                {
                    if (client.Connected)
                    {
                        await client.DisconnectAsync();
                    }
                    client = null;
                });
            }
        }

        static void OnTimerElapsed(object o, EventArgs e)
        {
            client.StopAllDevicesAsync();
            timer.Stop();
        }


        [HarmonyPatch(typeof(RegularPeg), "PopPeg")]
        [HarmonyPostfix]
        static private void PatchCollided()
        {
            if (!timer.Enabled)
            {
                if (client.Devices != null)
                {
                    foreach (var d in client.Devices)
                    {
                        if (d.VibrateAttributes.Count > 0)
                        {
                            d.VibrateAsync(1.0);
                        }
                    }
                }
                timer.Start();
            }
            timer.Stop();
            timer.Start();
        }
    }
}
