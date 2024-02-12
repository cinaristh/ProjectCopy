    using UnityEngine;
    using System.Collections.Generic;
    using Mirror;
    using UnityEngine.UI;
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Collections;
    using UI;
    using TMPro;

    namespace GameLobby
    {
    public class MainMenu: NetworkBehaviour{
        
        public static MainMenu menu;
        public NetworkManager networkManager;
        [SyncVar] public string DisplayName;



        [Header("Name")]
        public GameObject ChangeNamePanel;
        public GameObject CloseButton;
        public Button SetNameButton;
        public InputField NameInput;
        public int firstTime =1;
        public bool inGame;
        


        [Header("Chat")]
        public Text ChatHistoryText;
        public InputField MessageInput;
        public Button SendButton;





        
        private void Start()
        
        {   
            menu = this;
            networkManager = FindObjectOfType<NetworkManager>();
            firstTime = PlayerPrefs.GetInt("firstTime",1);
            if(!PlayerPrefs.HasKey("Name"))
            {return;}


            

            string defaultName = PlayerPrefs.GetString("Name");

            NameInput.text = defaultName;
            DisplayName = defaultName;
            SetName(defaultName);


            DisplayName = defaultName;
            SetName(defaultName);
        

            }


    

    
    


        
        private void Update()
        {        if(!inGame)
            {
            
            if (firstTime ==1){
                ChangeNamePanel.SetActive(true);
                CloseButton.SetActive(false);
        
            }

            else{
                CloseButton.SetActive(true);
            }
            PlayerPrefs.SetInt("firstTime",firstTime);
            }
        }
        
        
        public void SetName(string name)
        {
            SetNameButton.interactable = !string.IsNullOrEmpty(name);
        }

        public void SaveName()
        {
            firstTime= 0;
            ChangeNamePanel.SetActive(false);
            DisplayName= NameInput.text;
            PlayerPrefs.SetString("Name", DisplayName);
            
            Invoke(nameof(Disconnect),1f);
        }


    
        void Disconnect()
        {
            if (networkManager.mode == NetworkManagerMode.Host)
            {
                networkManager.StopHost();
            }
            else if (networkManager.mode == NetworkManagerMode.ClientOnly)
            {
                networkManager.StopClient();
            }
        }
    
    }
    }
