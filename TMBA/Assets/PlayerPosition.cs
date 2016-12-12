
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;


//public class NetworkController : NetworkManager
//{
//	GameObject chosenCharacter; // character1, character2, etc.
//
//	// Instantiate whichever character the player chose and was assigned to chosenCharacter
//	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
//		//base.OnServerAddPlayer (conn, playerControllerId);
//
//		Vector3 playerSpawnPos = (GameObject)GameObject("Position1").Transform.Position;
//		var player = (GameObject)GameObject.Instantiate(chosenCharacter, playerSpawnPos, Quaternion.identity);
//		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
//
//	}
//
//}

//public class MsgTypes
//{
//	public const short PlayerPrefab = MsgType.Highest + 1;
//
//	public class PlayerPrefabMsg : MessageBase
//	{
//		public short controllerID;    
//		public short prefabIndex;
//	}
//}
//
//public class NetworkController : NetworkManager
//{
//	public short playerPrefabIndex;
//
//	public override void OnStartServer()
//	{
//		NetworkServer.RegisterHandler(MsgTypes.PlayerPrefab, OnResponsePrefab);
//		base.OnStartServer();
//	}
//
//	public override void OnClientConnect(NetworkConnection conn)
//	{
//		client.RegisterHandler(MsgTypes.PlayerPrefab, OnRequestPrefab);
//		base.OnClientConnect(conn);
//	}
//
//	private void OnRequestPrefab(NetworkMessage netMsg)
//	{
//		MsgTypes.PlayerPrefabMsg msg = new MsgTypes.PlayerPrefabMsg();
//		msg.controllerID = netMsg.ReadMessage<MsgTypes.PlayerPrefabMsg>().controllerID;
//		msg.prefabIndex = playerPrefabIndex;
//		client.Send(MsgTypes.PlayerPrefab, msg);
//	}
//
//	private void OnResponsePrefab(NetworkMessage netMsg)
//	{
//		MsgTypes.PlayerPrefabMsg msg = netMsg.ReadMessage<MsgTypes.PlayerPrefabMsg>();  
//		playerPrefab = spawnPrefabs[msg.prefabIndex];
//		base.OnServerAddPlayer(netMsg.conn, msg.controllerID);
//		Debug.Log(playerPrefab.name + " spawned!");
//	}
//
//	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
//	{
//		MsgTypes.PlayerPrefabMsg msg = new MsgTypes.PlayerPrefabMsg();
//		msg.controllerID = playerControllerId;
//		NetworkServer.SendToClient(conn.connectionId, MsgTypes.PlayerPrefab, msg);
//	}
//}