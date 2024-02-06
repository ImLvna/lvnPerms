
using System.Linq;
using gay.lvna.common.udon.extensions;
using gay.lvna.lvnperms.core;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace gay.lvna.lvnperms.plugins
{
    public abstract class LockedInteractable : Plugin
    {


        public PermissionContainer[] permissions;
        public RoleContainer[] roles;

        public void Start()
        {

            manager.RegisterPlugin(this);
        }

        public override void _lvn_Start()
        {
        }

        private bool CanInteract()
        {
            PlayerContainer playerContainer = manager.GetPlayerContainer(Networking.LocalPlayer);
            if (playerContainer == null)
            {
                return false;
            }
            bool hasPermission = false;

            foreach (RoleContainer role in roles)
            {
                if (role.HasPlayer(playerContainer.player))
                {
                    hasPermission = true;
                    break;
                }
            }

            if (!hasPermission)
            {
                return false;
            }

            foreach (PermissionContainer permission in permissions)
            {
                if (playerContainer.permissions.Contains(permission))
                {
                    hasPermission = true;
                    break;
                }
            }

            if (!hasPermission)
            {
                return false;
            }

            return true;
        }

        public override void Interact()
        {
            _Interact();
        }

        public abstract void _Interact();
    }
}