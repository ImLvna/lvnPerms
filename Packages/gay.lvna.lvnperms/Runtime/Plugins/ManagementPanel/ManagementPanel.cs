
using gay.lvna.common.udon;
using gay.lvna.lvnperms.core;
using gay.lvna.ui.core.scroll;
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDK3.Data;
using VRC.SDKBase;
using VRC.Udon;

namespace gay.lvna.lvnperms.plugins.ManagementPanel
{
    public class ManagementPanel : Plugin
    {
        [HideInInspector]
        public PlayerContainer playerContainer;

        public RoleContainer[] roles;

        bool initialized = false;


        private VerticalScroller playerList;
        private GameObject actionsHeader;
        private VerticalScroller roleList;
        void Start()
        {

            playerList = transform.Find("PlayerList").GetComponent<VerticalScroller>();

            actionsHeader = transform.Find("Actions").Find("Header").gameObject;

            roleList = transform.Find("Actions").Find("Roles").GetComponent<VerticalScroller>();



            manager.RegisterPlugin(this);
        }

        public override void lvn_Start()
        {
            Render();
        }

        public override void lvn_PermissionsUpdate()
        {
            Render();
        }

        public void SetRole(RoleContainer role)
        {
            if (playerContainer == null)
            {
                manager.Log("ManagementPanel: PlayerContainer is null");
                return;
            }
            if (playerContainer.HasRole(role))
            {
                playerContainer.RemoveRole(role);
            }
            else
            {
                playerContainer.AddRole(role);
            }
        }

        public void Render()
        {
            if (playerContainer == null)
            {
                playerContainer = manager.GetPlayerContainer();
            }
            if (playerContainer == null)
            {
                manager.Log("ManagementPanel: PlayerContainer is null");
                return;
            }
            RenderPlayerList();
            RenderActions();
        }


        void RenderActions()
        {
            TextMeshProUGUI tmpro = actionsHeader.GetComponentInChildren<TextMeshProUGUI>();
            tmpro.text = playerContainer.player.displayName;
            tmpro.color = playerContainer.GetTopRole().color;



        }


        void RenderPlayerList()
        {
            VRCPlayerApi[] players = VRCPlayerApi.GetPlayers(new VRCPlayerApi[VRCPlayerApi.GetPlayerCount()]);


            foreach (ScrollEntry entry in playerList.entries)
            {
                VRCPlayerApi player = (VRCPlayerApi)entry.data.Reference;
                if (player == null || !Utilities.IsValid(player))
                {
                    playerList.RemoveEntry(entry);
                }
            }

            foreach (VRCPlayerApi player in players)
            {
                if (player == null)
                {
                    continue;
                }
                PlayerContainer playerContainer = manager.GetPlayerContainer(player);
                if (playerContainer == null)
                {
                    continue;
                }

                ScrollEntry entry = playerList.GetEntry(new DataToken(playerContainer));

                if (entry == null)
                {
                    entry = playerList.AddEntry(new DataToken(playerContainer));
                    entry.name = player.displayName;
                    entry.GetComponentInChildren<TextMeshProUGUI>().text = player.displayName;
                }
                entry.GetComponentInChildren<TextMeshProUGUI>().color = playerContainer.GetTopRole().color;
            }
        }
    }
}