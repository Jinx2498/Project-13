// vis2k: GUILayout instead of spacey += ...; removed Update hotkeys to avoid
// confusion if someone accidentally presses one.
using UnityEngine;

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
                GUILayout.Label(manager.networkAddress);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                if (GUILayout.Button("Stop Host"))
                {
                    manager.StopHost();
                }

                // join other lobby
                GUILayout.Label("\nOr join another player's lobby:");
                GUILayout.BeginHorizontal();
                manager.networkAddress = GUILayout.TextField(joinAddr);
                if (GUILayout.Button("Join"))
                {
                    manager.networkAddress = joinAddr;
                    manager.StartClient();
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            // client only
            else if (NetworkClient.isConnected)
            {
                GUILayout.Label($"<b>Client</b>: connected to {manager.networkAddress} via {Transport.activeTransport}");
                if (GUILayout.Button("Stop Client"))
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
        }
    }
}
