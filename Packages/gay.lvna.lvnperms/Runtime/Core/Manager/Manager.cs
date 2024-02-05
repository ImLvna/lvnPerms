
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace gay.lvna.lvnperms.core
{
    public class Manager : UdonSharpBehaviour
    {
        void Start()
        {

        }

        public override void OnPlayerLeft(VRCPlayerApi player)
        {
            if (Networking.LocalPlayer == player)
            {
                // Clear the player's permissions
                // Clear the player's roles
            }
        }
    }

}