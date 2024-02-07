
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace gay.lvna.lvnperms.core
{
    public abstract class Plugin : UdonSharpBehaviour
    {
        public Manager manager;

        public abstract void lvn_Start();

        public virtual void lvn_LogsUpdate()
        {

        }

        public virtual void lvn_PermissionsUpdate()
        {

        }
    }
}