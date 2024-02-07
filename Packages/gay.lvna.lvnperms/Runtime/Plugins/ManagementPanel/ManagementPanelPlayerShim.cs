﻿
using gay.lvna.lvnperms.core;
using gay.lvna.lvnperms.plugins.ManagementPanel;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace gay.lvna.lvnperms.plugins.ManagementPanel
{
    public class ManagementPanelPlayerShim : UdonSharpBehaviour
    {
        [HideInInspector]
        public PlayerContainer playerContainer;
        [HideInInspector]
        public ManagementPanel panel;

        public void OnClick()
        {
            panel.playerContainer = playerContainer;
            panel.Render();
        }
    }
}