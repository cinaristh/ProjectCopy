	using System;
	using System.Collections.Generic;
	using UnityEngine;
	using UI;
	using UnityEngine.UI;
	using System.Linq;
	using Mirror;

	public class GameManager : NetworkBehaviour
	{

	public SyncDictionary<uint, string> playerRolesDictionary = new SyncDictionary<uint, string>();
	
		public Button startButton;
		
		public Button quitButton;

		public SpriteRenderer daySpriteRenderer;

		private bool assignRolesCompleted = false;
	
		
		[SyncVar]
		public Timer timer; 
	
		public Player player;

	
		public void AssignRoles()
		{
			int policeNum, killerNum, villagersNum;
			CalculateRoles(out policeNum, out killerNum, out villagersNum);
			List<Player> activePlayers = Player.playersList;

			// Create the roles array
			string[] rolesArray = new string[villagersNum + killerNum + policeNum]; 

			
			Populate(rolesArray, 0, villagersNum, "Villager");
			Populate(rolesArray, villagersNum, killerNum, "Killer");
			Populate(rolesArray, villagersNum + killerNum, policeNum, "Police");

			// Shuffle the roles array
			Shuffle(rolesArray);
			Debug.Log($"activePlayers count: {activePlayers.Count}");
			Debug.Log($"rolesArray length: {rolesArray.Length}");

			// Assign roles to players and store in the dictionary
			AssignRolesToPlayers(activePlayers, rolesArray);
		}

		public void CalculateRoles(out int policeNum, out int killerNum, out int villagersNum )
		{ 
			
			if (6 <= player.TotalPlayersCount&& player.TotalPlayersCount < 11)
			{
				policeNum = 1;
				killerNum = 1;
			}
			else if (11 <= player.TotalPlayersCount && player.TotalPlayersCount< 15)
			{
				policeNum = 2;
				killerNum = 2;
			}
			else if (15 <= player.TotalPlayersCount && player.TotalPlayersCount < 17)
			{
				policeNum = 3;
				killerNum = 3;
			}
			else if (17 <= player.TotalPlayersCount && player.TotalPlayersCount < 19)
			{
				policeNum = 4;
				killerNum = 4;
			}
			else
			{
				policeNum = 0;
				killerNum = 0;
			}

			villagersNum = player.TotalPlayersCount- policeNum - killerNum;
		}

	public void AssignRolesToPlayers(List<Player> activePlayers, string[] rolesArray)
	{
		int currentIndex = 0;



		foreach (var player in activePlayers)
		{


				string role = rolesArray[currentIndex++];
				NetworkIdentity playerNetworkIdentity = player.GetComponent<NetworkIdentity>();
				uint id = playerNetworkIdentity.netId;
				playerRolesDictionary[id] = role;		

			
		}
		assignRolesCompleted=true; 
	}



	public void Play()

	{  
	if(isServer){
		player.AddPlayer();
		
		if (player.TotalPlayersCount >= 6)
			{
				AssignRoles();
			}
			
	}
			if (assignRolesCompleted==true && player.TotalPlayersCount >= 6)
			{  
				OnStartButtonPressed();
			}
	
	}







	[ClientRpc]
		public void OnStartButtonPressed()

	{  
			
			startButton.interactable = false;
			startButton.gameObject.SetActive(false);
		
		
			quitButton.interactable = false;
			quitButton.gameObject.SetActive(false);
			
	
				
			daySpriteRenderer.enabled = false;
				
				
		StartTimer();
		player.TargetHandleVisibilityBasedOnRole(player.id);
	
	}
		

	

		private static void Populate<T>(T[] array, int start, int count, T value)
		{
			for (int i = start; i < start + count; i++)
			{
				array[i] = value;
			}
		}

		private static void Shuffle<T>(T[] array)
		{
			int n = array.Length;
			while (n > 1)
			{
				n--;
				int k = UnityEngine.Random.Range(0, n + 1);
				T temp = array[k];
				array[k] = array[n];
				array[n] = temp;
			}
		}

	
		private void StartTimer()
		{
		  timer.StartTimer();
			
		}
	}
