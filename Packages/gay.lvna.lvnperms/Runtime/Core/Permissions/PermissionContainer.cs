
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace gay.lvna.lvnperms.core
{
    public class PermissionContainer : UdonSharpBehaviour
    {

        // private Manager manager;

        public string permissionName;
        public string permissionId;
        public string permissionDescription;

        public void _lvn_start()
        {
            gameObject.name = permissionName;
        }
    }
}