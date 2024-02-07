
using System.Linq;
using gay.lvna.common.udon.extensions;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace gay.lvna.lvnperms.core
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class PlayerContainer : ManagedOwnership
    {
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

        public bool HasRole(RoleContainer role)
        {
            return roles.Contains(role);
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

        public void AddRole(RoleContainer role)
        {
            if (!manager.CanTakeOwnership(Networking.LocalPlayer))
            {
                return;
            }

            Networking.SetOwner(Networking.LocalPlayer, role.gameObject);
            role.AddPlayer(player);
        }

        public void RemoveRole(RoleContainer role)
        {
            if (!manager.CanTakeOwnership(Networking.LocalPlayer))
            {
                return;
            }

            Networking.SetOwner(Networking.LocalPlayer, role.gameObject);
            role.RemovePlayer(player);
        }

        public void _lvn_init(VRCPlayerApi player)
        {
            name = player.displayName;
            _player = player;
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