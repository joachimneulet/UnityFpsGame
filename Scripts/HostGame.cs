using UnityEngine;
using UnityEngine.Networking;

public class HostGame : MonoBehaviour
{
    [SerializeField]
    private uint roomSize = 6;

    private string roomName;

    private string roomPassword = "";

    private NetworkManager networkManager;

    private void Start(){
      networkManager = NetworkManager.singleton;
      if(networkManager.matchMaker == null){
        networkManager.StartMatchMaker();
      }
    }

    public void SetRoomName(string _name){
      roomName = _name;
    }

    public void CreateRoom(){
      if(roomName != "" && roomName != null){
        Debug.Log("Opening room " + roomName + " containing " + roomSize + " slots");
        //Take arguments : matchName, matchSize, matchAdvertise, matchPswd, pubClientAddr, privClientAddr, eloRequired, requestDomain, functCallback
        networkManager.matchMaker.CreateMatch(roomName, roomSize, true, roomPassword, "", "", 0, 0, networkManager.OnMatchCreate);
      }
    }
}
