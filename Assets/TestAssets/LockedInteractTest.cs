
using gay.lvna.lvnperms.plugins;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class LockedInteractTest : LockedInteractable
{
    public override void _Interact()
    {
        Debug.Log("Interacted");
    }
}
