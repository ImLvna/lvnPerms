
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace gay.lvna.lvnperms.core
{
    public abstract class Plugin : UdonSharpBehaviour
    {
        public Manager manager;

        public abstract void _lvn_Start();

        public virtual void _lvn_logsUpdate()
        {

        }

        public virtual void _lvn_permissionsUpdate()
        {

        }
    }
}