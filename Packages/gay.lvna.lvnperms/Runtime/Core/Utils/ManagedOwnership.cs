
using gay.lvna.lvnperms.core;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ManagedOwnership : UdonSharpBehaviour
{
    public Manager manager;

    public override bool OnOwnershipRequest(VRCPlayerApi requestingPlayer, VRCPlayerApi requestedOwner)
    {
        if (manager == null)
        {
            return false;
        }

        return manager.CanTakeOwnership(requestedOwner);
    }
}
