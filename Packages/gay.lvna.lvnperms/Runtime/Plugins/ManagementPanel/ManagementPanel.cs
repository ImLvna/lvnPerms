
using gay.lvna.common.udon;
using gay.lvna.lvnperms.core;
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace gay.lvna.lvnperms.plugins.ManagementPanel
{
    public class ManagementPanel : Plugin
    {
        [HideInInspector]
        public PlayerContainer playerContainer;


        private GameObject playerListContent;
        private GameObject playerListTemplate;
        private GameObject actionsHeader;
        void Start()
        {

            playerListContent = transform.Find("PlayerList").Find("Viewport").Find("Content").gameObject;

            playerListTemplate = transform.Find("PlayerList").Find("Template").gameObject;
            playerListTemplate.SetActive(false);

            actionsHeader = transform.Find("Actions").Find("Header").gameObject;


            manager.RegisterPlugin(this);
        }

        public override void lvn_Start()
        {
            Render();
        }

        public override void lvn_PermissionsUpdate()
        {
            Debug.Log("lvnperms");
            Render();
        }

        public void Render()
        {
            Debug.Log("render");
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
            manager.Log("RenderPlayerList");
            Transform[] children = playerListContent.GetComponentsInChildren<Transform>();
            for (int i = 1; i < children.Length; i++)
            {
                Destroy(children[i].gameObject);
            }



            VRCPlayerApi[] players = VRCPlayerApi.GetPlayers(new VRCPlayerApi[VRCPlayerApi.GetPlayerCount()]);

            float step = playerListTemplate.GetComponent<RectTransform>().rect.height;
            float top = 0;

            foreach (VRCPlayerApi player in players)
            {
                if (player == null)
                {
                    manager.Log("Player is null");
                    continue;
                }
                PlayerContainer playerContainer = manager.GetPlayerContainer(player);
                if (playerContainer == null)
                {
                    manager.Log("PlayerContainer is null");
                    continue;
                }
                GameObject playerListEntry = Instantiate(playerListTemplate);

                playerListEntry.name = player.displayName;

                ManagementPanelPlayerShim boxPlayerListEntry = playerListEntry.GetComponent<ManagementPanelPlayerShim>();
                boxPlayerListEntry.playerContainer = playerContainer;
                boxPlayerListEntry.panel = this;


                playerListEntry.transform.SetParent(playerListContent.transform, false);
                playerListEntry.SetActive(true);
                TextMeshProUGUI tmpro = playerListEntry.GetComponentInChildren<TextMeshProUGUI>();
                tmpro.text = player.displayName;
                tmpro.color = playerContainer.GetTopRole().color;
                playerListEntry.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, top);
                top -= step;
            }

            playerListContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, -top);
        }
    }
}