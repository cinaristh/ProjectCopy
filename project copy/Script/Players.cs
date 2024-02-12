
        using Mirror;
        using System.Collections.Generic;
        using System;
        using UnityEngine; 
        using UnityEngine.UI;
        using UnityEngine.SceneManagement;
        using GameLobby;
        using TMPro;




        namespace UI{

            
        public class Player: NetworkBehaviour
        {
            [SyncVar] public int nextPlayerId=1;
            public TextMesh NameDisplayText;
            public static Player localPlayer;
            [SyncVar(hook="DisplayPlayerName")] public string PlayerDisplayName;
            [SyncVar] public int PlayerNo = 0;
            public bool IsActive { get; private set; } = false;
        
            public static List<Player> playersList = new List<Player>();
            [SyncVar]public int TotalPlayersCount;
            public GameManager gameManager;

             public string Role;

             public SpriteRenderer civilianBoyRenderer;
            public SpriteRenderer civilianBoyEndRenderer;
            public SpriteRenderer policeBoyRenderer;
            public SpriteRenderer policeBoyEndRenderer;
            public SpriteRenderer killerBoyRenderer;
            public SpriteRenderer killerBoyEndRenderer;
            public NetworkIdentity networkId;
            [SyncVar]
            public uint id;
           [SerializeField]public GameObject chatUI=null;
           [SerializeField]private Text chatText=null;
           [SerializeField]private InputField inputField=null;    
           private static event Action<string> OnMessage;

                

                
        
        public void Start()
        {

            localPlayer = this;
                CmdSendName(MainMenu.instance.DisplayName);
                IsActive = true;
                networkId = GetComponent<NetworkIdentity>();
                id= networkId.netId;
                playersList.Add(localPlayer);
                

        }





        public void AddPlayer()
        {
                    TotalPlayersCount = playersList.Count;

        }

    


            [Command]
            public void CmdSendName(string name)
            {
                PlayerDisplayName=name; 
            }
            public void DisplayPlayerName(string name, string playerName)
            {
                name =PlayerDisplayName  ;
            

            
                NameDisplayText.text = playerName;


            }

            
        
            public override void OnStartAuthority()
            {
            chatUI.SetActive(true);
            

            OnMessage += HandleNewMessage;
            
            }


        

                [ClientCallback]
                private void OnDestroy()
                {
                    playersList.Remove(this);
                    if (!hasAuthority){return;}
                    OnMessage -= HandleNewMessage;
                }

                private void HandleNewMessage(string message)
                {   
                    chatText.text += message;

                }

                [Client]
                public void Send(string message)
                {
                    if (!Input.GetKeyDown(KeyCode.Return)){return;}
                    if (string.IsNullOrWhiteSpace(message)){return;}
                    CmdSendMessage(message);
                    inputField.text=string.Empty;
                

                }

                [Command]
                private void CmdSendMessage(string message)
                {   
                    string formattedMessage = $"{PlayerDisplayName}: {message}";
                    RpcHandleMessage($"{PlayerDisplayName}:{message}");
                }

                [ClientRpc]
                private void RpcHandleMessage(string message)
                {
                    OnMessage?.Invoke($"\n{message}");

                }

        

    

                public void TargetHandleVisibilityBasedOnRole(uint id)
            
                
                {


                        GetRoleForPlayer(id); 
                        
                        switch (Role)
                        {
                        
                            case "Police":
                                ShowPoliceSprites();
                                break;
                            case "Killer":
                                ShowKillerSprites();
                                break;
                            
                        }
                }

            public void GetRoleForPlayer(uint id)
            {
                if (gameManager.playerRolesDictionary.TryGetValue(id, out var role))
                {
                    
                Role=role;
                }
            }

            private void ShowPoliceSprites()
                {   
                    civilianBoyRenderer.enabled = false;
                    civilianBoyEndRenderer.enabled = false;
                    policeBoyRenderer.enabled = true;
                    policeBoyEndRenderer.enabled = true;
                    killerBoyRenderer.enabled = false;
                    killerBoyEndRenderer.enabled = false;
                }

                private void ShowKillerSprites()
                {   
                    civilianBoyRenderer.enabled = false;
                    civilianBoyEndRenderer.enabled = false;
                    policeBoyRenderer.enabled = false;
                    policeBoyEndRenderer.enabled = false;
                    killerBoyRenderer.enabled = true;
                    killerBoyEndRenderer.enabled = true;
                }
        

            }

        }
