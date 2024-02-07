
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

            manager.RegisterPlugin(this);

            foreach (GameObject obj in showObjects)
            {
                obj.SetActive(false);
            }
            foreach (GameObject obj in hideObjects)
            {
                obj.SetActive(true);
            }


        }

        public override void lvn_Start()
        {

            UpdateObjects();
        }

        public override void lvn_PermissionsUpdate()
        {
            UpdateObjects();
        }

        void UpdateObjects()
        {
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
                if (playerContainer.HasPermission(permission))
                {
                    Debug.Log("Has permission: " + permission.permissionId);
                    hasPermission = true;
                    break;
                }
            }

            Debug.Log("Has permission: " + hasPermission);
            foreach (GameObject obj in showObjects)
            {
                Debug.Log("Show object: " + obj.name);
                obj.SetActive(hasPermission);
            }
            foreach (GameObject obj in hideObjects)
            {
                Debug.Log("Hide object: " + obj.name);
                obj.SetActive(!hasPermission);
            }
        }
    }
}