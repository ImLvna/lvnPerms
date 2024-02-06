
using System.Linq;
using gay.lvna.common.udon.extensions;
using gay.lvna.lvnperms.core;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace gay.lvna.lvnperms.plugins
{
    public class Whitelist : Plugin
    {


        public PermissionContainer[] permissions;
        public RoleContainer[] roles;

        public GameObject[] showObjects;
        public GameObject[] hideObjects;

        public void Start()
        {

            foreach (GameObject obj in showObjects)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in hideObjects)
            {
                obj.SetActive(true);
            }

            manager.RegisterPlugin(this);
        }

        public override void _lvn_Start()
        {

            UpdateObjects();
        }

        public override void _lvn_permissionsUpdate()
        {
            UpdateObjects();
        }

        void UpdateObjects()
        {
            manager.Log("UpdateObjects");
            PlayerContainer playerContainer = manager.GetPlayerContainer(Networking.LocalPlayer);
            if (playerContainer == null)
            {
                manager.Log("PlayerContainer is null");
                return;
            }
            bool hasPermission = false;

            foreach (RoleContainer role in roles)
            {
                if (role.HasPlayer(Networking.LocalPlayer))
                {
                    hasPermission = true;
                    break;
                }
            }
            foreach (PermissionContainer permission in permissions)
            {
                Debug.Log("bvaswdas");
                Debug.Log(permission.permissionId);
                Debug.Log(playerContainer.permissions);
                if (playerContainer.permissions.Contains(permission))
                {
                    hasPermission = true;
                    break;
                }
            }

            foreach (GameObject obj in showObjects)
            {
                obj.SetActive(hasPermission);
            }
            foreach (GameObject obj in hideObjects)
            {
                obj.SetActive(!hasPermission);
            }
        }
    }
}