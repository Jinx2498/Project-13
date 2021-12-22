// vis2k: GUILayout instead of spacey += ...; removed Update hotkeys to avoid
// confusion if someone accidentally presses one.
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mirror
{
    /// <summary>Shows NetworkManager controls in a GUI at runtime.</summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Network/NetworkManagerHUD")]
    [RequireComponent(typeof(NetworkManager))]
    [HelpURL("https://mirror-networking.gitbook.io/docs/components/network-manager-hud")]
    public class NetworkManagerHUD : MonoBehaviour
    {
        NetworkManager manager;

        public int offsetX;
        public int offsetY;
        public string joinAddr = "";
        public string hostAddr = "127.0.0.1";
        public int selectedMap = 0;
        private string[] mapNames = { " Arena #1", " Arena #2", " Arena #3" };

        void Awake()
        {
            manager = GetComponent<NetworkManager>();

            // get local ip
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach(IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    hostAddr = ip.ToString();
                    break;
                }
            }
            // hostAddr = new WebClient().DownloadString("https://icanhazip.com").Trim();

            manager.networkAddress = hostAddr;
            manager.StartHost();
        }

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 9999));

            if (NetworkServer.active && NetworkClient.active)
            {
                // hosting
                GUILayout.BeginHorizontal();
                GUILayout.Label("You are hosting a lobby on:");
                GUILayout.Label(hostAddr);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Close lobby", GUILayout.MinWidth(120)))
                {
                    manager.StopHost();
                }
                if (GUILayout.Button("Copy address", GUILayout.MinWidth(120)))
                {
                    // copy address + port to clipboard
                    TextEditor te = new TextEditor();
                    te.text = hostAddr;
                    te.SelectAll();
                    te.Copy();
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                // join other lobby
                GUILayout.Label("\nOr join another player's lobby:");
                GUILayout.BeginHorizontal();
                joinAddr = GUILayout.TextField(joinAddr, 21, GUILayout.MinWidth(180));
                if (GUILayout.Button("Join"))
                {
                    // temporarily disable offline scene to allow switching from host to client
                    manager.networkAddress = joinAddr;
                    string offlineTemp = manager.offlineScene;
                    manager.offlineScene = null;
                    manager.StopHost();
                    manager.StartClient();
                    manager.offlineScene = offlineTemp;
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            // client only
            else if (NetworkClient.isConnected)
            {
                GUILayout.Label($"Connected to {manager.networkAddress}");
                if (GUILayout.Button("Leave lobby"))
                {
                    manager.StopClient();
                }
            }

            // client ready
            if (NetworkClient.isConnected && !NetworkClient.ready)
            {
                if (GUILayout.Button("Client Ready"))
                {
                    NetworkClient.Ready();
                    if (NetworkClient.localPlayer == null)
                    {
                        NetworkClient.AddPlayer();
                    }
                }
            }

            GUILayout.EndArea();

            if (NetworkServer.active && NetworkClient.active)
            {
                GUILayout.BeginArea(new Rect(410, 10, 200, 9999));

                GUILayout.Label("-- MATCH SETTINGS --");
                selectedMap = GUILayout.SelectionGrid(selectedMap, mapNames, 1, "toggle");

                GUILayout.EndArea();
            }
        }
    }
}
