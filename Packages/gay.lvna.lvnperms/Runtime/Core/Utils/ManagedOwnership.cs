
using gay.lvna.lvnperms.core;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ManagedOwnership : UdonSharpBehaviour
{
    public Manager manager;

    private VRCPlayerApi _oldOwner;

    public override bool OnOwnershipRequest(VRCPlayerApi requestingPlayer, VRCPlayerApi requestedOwner)
    {
        if (manager == null)
        {
            return false;
        }

        return manager.CanTakeOwnership(requestedOwner);
    }

    public void SetOwner(VRCPlayerApi player)
    {
        _oldOwner = Networking.GetOwner(gameObject);
        Networking.SetOwner(player, gameObject);
    }

    public void RevertOwner()
    {
        Networking.SetOwner(_oldOwner, gameObject);
    }
}
