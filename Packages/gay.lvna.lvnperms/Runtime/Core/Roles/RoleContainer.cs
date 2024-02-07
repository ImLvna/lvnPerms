
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

        public Color32 color = new Color32(0, 0, 0, 255);

        public string[] hardcodedMembers;

        [SerializeField, UdonSynced, HideInInspector, FieldChangeCallback(nameof(members))]
        private string[] _members;
        private string[] members
        {
            get
            {
                return _members;
            }
            set
            {
                _members = value;
                if (manager != null)
                {
                    manager.UpdatePermissions();
                }
            }
        }

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
            manager = transform.parent.parent.GetComponent<Manager>();
        }


        public bool HasPlayer(VRCPlayerApi player)
        {
            if (isDefault)
            {
                return true;
            }
            return members.Contains(player.displayName);
        }

        public void AddPlayer(VRCPlayerApi player)
        {
            if (!Networking.IsOwner(gameObject) && !manager.CanTakeOwnership(Networking.LocalPlayer))
            {
                return;
            }




            if (!CanModifyRole(player))
            {
                return;
            }


            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            members = members.Add(player.displayName);
            RequestSerialization();
        }

        public void RemovePlayer(VRCPlayerApi player)
        {
            if (!manager.CanTakeOwnership(Networking.LocalPlayer))
            {
                return;
            }

            // Dont take away default roles
            if (isDefault)
            {
                return;
            }

            if (!CanModifyRole(player))
            {
                return;
            }
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
            members = members.Remove(player.displayName);
            RequestSerialization();
        }

        public bool CanModifyRole(VRCPlayerApi player)
        {
            if (manager == null || manager.GetPlayerContainer() == null)
            {
                return false;
            }



            // Let admins do whatever they want
            if (manager.GetPlayerContainer().HasPermission(manager.GetPermissionById("*")))
            {
                return true;
            }


            if (manager.GetPlayerContainer(player) == null)
            {
                return false;
            }

            // Dont let people add themselves to roles
            if (player.displayName == Networking.LocalPlayer.displayName)
            {
                return false;
            }

            // Dont let people modify roles equal to or higher than their own
            if (manager.GetPlayerContainer().GetTopRole().rolePriority <= rolePriority)
            {
                return false;
            }

            // Dont let people modify others with higher or equal roles
            if (manager.GetPlayerContainer(player).GetTopRole().rolePriority > rolePriority)
            {
                return false;
            }

            return true;
        }
    }
}