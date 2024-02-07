
using System.Linq;
using gay.lvna.common.udon.extensions;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace gay.lvna.lvnperms.core
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class PlayerContainer : UdonSharpBehaviour
    {
        public Manager manager;

        private bool hasInitalized = false;

        [UdonSynced]
        public string target = "null";
        [UdonSynced]
        public string targetRole = "null";
        [UdonSynced]
        public bool targetGiveTake = false;

        public override void OnDeserialization()
        {
            if (!Networking.IsOwner(manager.gameObject))
            {
                return;
            }

            if (target != "null" && targetRole != "null")
            {
                RoleContainer role = manager.GetRoleById(targetRole);
                if (role != null)
                {
                    if (targetGiveTake)
                    {
                        manager.Log("Give " + target + " " + role.roleName);
                        role.AddPlayer(player);
                    }
                    else
                    {
                        manager.Log("Take " + target + " " + role.roleName);
                        role.RemovePlayer(player);
                    }
                }

            }
        }

        private VRCPlayerApi _player;
        public VRCPlayerApi player
        {
            get
            {
                return _player;
            }
        }

        public RoleContainer[] roles
        {
            get
            {
                RoleContainer[] roles = new RoleContainer[0];
                foreach (RoleContainer role in manager.roles)
                {
                    if (role.HasPlayer(player))
                    {
                        ArrayExtensions.Add(roles, role);
                        roles = roles.Add(role);
                    }
                }

                return roles;
            }
        }

        public PermissionContainer[] permissions
        {
            get
            {
                PermissionContainer[] permissions = new PermissionContainer[0];
                foreach (RoleContainer role in roles)
                {
                    permissions = permissions.ConCat(role.permissions);
                }

                if (permissions.Contains(manager.GetPermissionById("*")))
                {
                    return manager.permissions;
                }

                return permissions;
            }
        }

        public bool HasPermission(PermissionContainer permission)
        {
            foreach (PermissionContainer p in permissions)
            {
                if (p == permission || p.permissionId == "*")
                {
                    return true;
                }
            }
            return false;
        }

        public override bool OnOwnershipRequest(VRCPlayerApi requestingPlayer, VRCPlayerApi requestedOwner)
        {
            if (!hasInitalized)
            {
                return true;
            }
            return player == requestedOwner;
        }

        public RoleContainer GetTopRole()
        {
            if (roles.Length == 0)
            {
                return null;
            }
            RoleContainer topRole = roles[0];
            foreach (RoleContainer role in roles)
            {
                if (topRole.rolePriority < role.rolePriority)
                {
                    topRole = role;
                }
            }
            return topRole;
        }

        public void SetRole(PlayerContainer playerContainer, RoleContainer role, bool give)
        {
            if (!manager.CanTakeOwnership(Networking.LocalPlayer))
            {
                return;
            }
            target = playerContainer.player.displayName;
            targetRole = role.roleId;
            targetGiveTake = give;
            RequestSerialization();
        }

        public void _lvn_init(VRCPlayerApi player)
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            name = player.displayName;
            _player = player;
            hasInitalized = true;
            Networking.SetOwner(player, gameObject);
        }

        public override void OnPlayerLeft(VRCPlayerApi player)
        {
            if (player == _player)
            {
                Destroy(gameObject);
            }
        }
    }
}