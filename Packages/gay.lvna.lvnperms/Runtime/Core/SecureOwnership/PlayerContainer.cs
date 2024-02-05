
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace gay.lvna.lvnperms.core
{
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class PlayerContainer : UdonSharpBehaviour
    {
        private VRCPlayerApi player;


        public void _lvn_start()
        {
            player = Networking.LocalPlayer;
        }
    }
}