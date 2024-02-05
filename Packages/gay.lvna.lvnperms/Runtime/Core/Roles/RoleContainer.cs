
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace gay.lvna.lvnperms.core
{
    public class RoleContainer : UdonSharpBehaviour
    {
        private Manager manager;

        public string roleName;
        public string roleId;
        public int rolePriority;

        public string[] hardcodedMembers;

        private string[] members;

        public PermissionContainer[] grantPermissions;
        public PermissionContainer[] denyPermissions;

        public RoleContainer[] inherits;
    }
}