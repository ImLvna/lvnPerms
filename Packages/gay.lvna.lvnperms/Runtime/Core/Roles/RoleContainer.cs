
using System.Linq;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using gay.lvna.common.udon.extensions;

namespace gay.lvna.lvnperms.core
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class RoleContainer : ManagedOwnership
    {

        public string roleName;
        public string roleId;
        public int rolePriority;

        public string[] hardcodedMembers;

        [SerializeField, UdonSynced]
        private string[] members;

        public bool isDefault;

        public PermissionContainer[] _permissions;

        public PermissionContainer[] permissions
        {
            get
            {
                PermissionContainer[] permissions = _permissions;

                foreach (RoleContainer inherit in inherits)
                {
                    permissions = permissions.ConCat(inherit.permissions);
                }

                return permissions;
            }
        }

        public RoleContainer[] inherits;

        public void Start()
        {
            members = hardcodedMembers;
        }


        public bool HasPlayer(VRCPlayerApi player)
        {
            return members.Contains(player.displayName);
        }

        public void AddPlayer(VRCPlayerApi player)
        {
            if (!manager.CanTakeOwnership(Networking.LocalPlayer))
            {
                return;
            }
            SetOwner(Networking.LocalPlayer);
            members = members.Add(player.displayName);
            RequestSerialization();
            RevertOwner();
        }

        public void RemovePlayer(VRCPlayerApi player)
        {
            if (!manager.CanTakeOwnership(Networking.LocalPlayer))
            {
                return;
            }
            SetOwner(Networking.LocalPlayer);
            members = members.Remove(player.displayName);
            RequestSerialization();
            RevertOwner();
        }
    }
}