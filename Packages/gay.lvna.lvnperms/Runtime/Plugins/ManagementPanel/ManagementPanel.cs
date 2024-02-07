
using gay.lvna.common.udon;
using gay.lvna.lvnperms.core;
using TMPro;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

namespace gay.lvna.lvnperms.plugins.ManagementPanel
{
    public class ManagementPanel : Plugin
    {
        [HideInInspector]
        public PlayerContainer playerContainer;

        public RoleContainer[] roles;


        private GameObject playerListContent;
        private GameObject playerListTemplate;
        private GameObject actionsHeader;
        private GameObject roleListContent;
        private GameObject roleListTemplate;
        void Start()
        {

            playerListContent = transform.Find("PlayerList").Find("Viewport").Find("Content").gameObject;

            playerListTemplate = transform.Find("PlayerList").Find("Template").gameObject;
            playerListTemplate.SetActive(false);

            actionsHeader = transform.Find("Actions").Find("Header").gameObject;

            roleListContent = transform.Find("Actions").Find("Roles").Find("Viewport").Find("Content").gameObject;

            roleListTemplate = transform.Find("Actions").Find("Roles").Find("Template").gameObject;
            roleListTemplate.SetActive(false);


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

            Transform[] children = roleListContent.GetComponentsInChildren<Transform>();
            for (int i = 1; i < children.Length; i++)
            {
                Destroy(children[i].gameObject);
            }

            float step = roleListTemplate.GetComponent<RectTransform>().rect.height + 15;
            float top = -15;

            bool first = true;

            foreach (RoleContainer role in roles)
            {
                if (playerContainer != null && !role.CanModifyRole(playerContainer.player))
                {
                    // Hide roles that the player can't modify
                    continue;
                }
                GameObject roleListEntry = Instantiate(roleListTemplate);
                roleListEntry.name = role.name;

                ManagementPanelRoleShim boxRoleListEntry = roleListEntry.GetComponent<ManagementPanelRoleShim>();
                boxRoleListEntry.role = role;
                boxRoleListEntry.panel = this;

                roleListEntry.transform.SetParent(roleListContent.transform, false);
                roleListEntry.SetActive(true);
                TextMeshProUGUI tmproRole = roleListEntry.GetComponentInChildren<TextMeshProUGUI>();
                tmproRole.text = role.name;


                if (playerContainer != null)
                {
                    if (playerContainer.HasRole(role))
                    {
                        roleListEntry.GetComponent<Image>().color = role.color;
                    }
                    else
                    {
                        tmproRole.color = role.color;
                    }
                }

                RectTransform trans = roleListEntry.GetComponent<RectTransform>();
                if (first)
                {
                    trans.anchoredPosition = new Vector2(15, top);
                }
                else
                {
                    // Every other role is on the right side of the list
                    trans.anchoredPosition = new Vector2(-30, top);
                    trans.anchorMin = new Vector2(1, 1);
                    trans.anchorMax = new Vector2(1, 1);
                    trans.pivot = new Vector2(1, 1);

                    top -= step;
                }
                first = !first;
            }

            roleListContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, -top);
        }


        void RenderPlayerList()
        {
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