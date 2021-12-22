// vis2k: GUILayout instead of spacey += ...; removed Update hotkeys to avoid
// confusion if someone accidentally presses one.
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
        public string hostAddrPort = "...";
        public int selectedMap = 0;
        private string[] mapNames = { " Arena #1 - 'Plains'", " Arena #2 - 'Fountain'", " Arena #3 - 'Complex'" };

        void Awake()
        {
            manager = GetComponent<NetworkManager>();
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
                hostAddrPort = manager.networkAddress;
                GUILayout.Label(hostAddrPort);
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
                    te.text = hostAddrPort;
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
                GUILayout.Label($"<b>Client</b>: connected to {manager.networkAddress} via {Transport.activeTransport}");
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
                GUILayout.BeginArea(new Rect(410, 10, 300, 9999));

                GUILayout.Label("-- MATCH SETTINGS --");
                selectedMap = GUILayout.SelectionGrid(selectedMap, mapNames, 1, "toggle");

                GUILayout.EndArea();
            }
        }
    }
}
