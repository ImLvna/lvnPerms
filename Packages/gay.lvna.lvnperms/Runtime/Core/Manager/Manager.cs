
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using gay.lvna.common.udon.extensions;
using VRC.Udon.Common.Interfaces;
using UnityEngine.PlayerLoop;

namespace gay.lvna.lvnperms.core
{
    public class Manager : UdonSharpBehaviour
    {

        private bool hasInitalized = false;

        private Plugin[] plugins = new Plugin[0];

        public RoleContainer[] roles
        {
            get
            {
                return transform.Find("#Roles").gameObject.GetComponentsInChildren<RoleContainer>();
            }
        }

        public PermissionContainer[] permissions
        {
            get
            {
                PermissionContainer[] permissions = transform.Find("#Permissions").gameObject.GetComponentsInChildren<PermissionContainer>();
                return permissions.ConCat(corePermissions);
            }
        }

        public PermissionContainer[] corePermissions
        {
            get
            {
                return transform.Find("#CorePermissions").gameObject.GetComponentsInChildren<PermissionContainer>();
            }
        }

        public PermissionContainer GetPermissionById(string id)
        {
            foreach (PermissionContainer permission in permissions)
            {
                if (permission.permissionId == id)
                {
                    return permission;
                }
            }
            return null;
        }

        public RoleContainer GetRoleById(string id)
        {
            foreach (RoleContainer role in roles)
            {
                if (role.roleId == id)
                {
                    return role;
                }
            }
            return null;
        }

        public PlayerContainer[] players
        {
            get
            {
                return transform.Find("#PlayerContainers").gameObject.GetComponentsInChildren<PlayerContainer>();
            }
        }

        public PlayerContainer GetPlayerContainer(VRCPlayerApi player)
        {
            foreach (PlayerContainer playerContainer in players)
            {
                if (playerContainer.player == player)
                {
                    return playerContainer;
                }
            }
            return null;
        }

        public PlayerContainer GetPlayerContainer()
        {
            return GetPlayerContainer(Networking.LocalPlayer);
        }

        public PlayerContainer GetPlayerContainer(string name)
        {
            foreach (PlayerContainer playerContainer in players)
            {
                if (playerContainer.player.displayName == name)
                {
                    return playerContainer;
                }
            }
            return null;
        }

        public override bool OnOwnershipRequest(VRCPlayerApi requestingPlayer, VRCPlayerApi requestedOwner)
        {
            return CanTakeOwnership(requestedOwner);
        }


        private void CallPluginMethod(Plugin plugin, string method)
        {
            plugin.SendCustomEvent(method);
        }
        private void CallAllPluginsMethod(string method)
        {
            foreach (Plugin plugin in plugins)
            {
                CallPluginMethod(plugin, method);
            }
        }

        public void NetUpdatePermissions()
        {
            Log("Updating Permissions");
            CallAllPluginsMethod("_lvn_permissionsUpdate");
        }

        public void UpdatePermissions()
        {
            if (!Networking.IsOwner(gameObject))
            {
                return;
            }
            SendCustomNetworkEvent(NetworkEventTarget.All, "NetUpdatePermissions");
        }

        public void RegisterPlugin(Plugin plugin)
        {
            plugins = plugins.Add(plugin);

            CallPluginMethod(plugin, "_lvn_Start");
        }

        public string[] logs = new string[0];

        public void Log(string message)
        {
            Debug.Log(message);
            logs = logs.Add(message);

            CallAllPluginsMethod("_lvn_logsUpdate");
        }


        void InitPermissions()
        {

            PermissionContainer[] permissions = transform.Find("#Permissions").GetComponentsInChildren<PermissionContainer>();
            foreach (PermissionContainer permission in permissions)
            {
                permission.name = permission.permissionId;
            }
        }

        void Start()
        {
            InitPermissions();


            hasInitalized = true;
            CallAllPluginsMethod("_lvn_Start");
        }

        public override void OnPlayerLeft(VRCPlayerApi player)
        {
            UpdatePermissions();
        }

        public override void OnPlayerJoined(VRCPlayerApi player)
        {
            PlayerContainer playerContainer = GetPlayerContainer(player);
            if (playerContainer == null)
            {
                Log("Instantiating player container for " + player.displayName);
                playerContainer = Instantiate(transform.Find("#InitalizationPrefabs").Find("#PlayerContainer").gameObject).GetComponent<PlayerContainer>();
                playerContainer.transform.SetParent(transform.Find("#PlayerContainers"));
                playerContainer._lvn_init(player);
            }
            Log("Player " + player.displayName + " has joined. Roles:");
            foreach (RoleContainer role in playerContainer.roles)
            {
                Log(role.roleName);
            }

            UpdatePermissions();
        }

        public bool CanTakeOwnership(VRCPlayerApi player)
        {
            if (Networking.GetOwner(gameObject) == Networking.LocalPlayer)
            {
                return true;
            }
            if (GetPlayerContainer(player).HasPermission(GetPermissionById("gay.lvna.lvnperms.TakeOwnership")))
            {
                return true;
            }
            return false;
        }

    }
}