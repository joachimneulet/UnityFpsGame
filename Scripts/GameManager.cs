//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager gm;
    public MatchSettings matchSettings;

    [SerializeField]
    private GameObject sceneCamera;

    private const string PLAYER_ID_PREFIX = "Player ";
    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    private void Awake(){
      if(gm != null){
        Debug.LogError("Multiple instances of GameManager in the scene");
      }else{
        gm = this;
      }
    }

    public void SetSceneCameraActive(bool isActive){
      if(sceneCamera == null){
        return;
      }
      sceneCamera.SetActive(isActive);
    }

    public static void RegisterPlayer(string _netID, Player _player){
      string _playerID = PLAYER_ID_PREFIX + _netID;
      players.Add(_playerID, _player);
      _player.transform.name = _playerID;
    }

    public static void UnregisterPlayer(string _playerID){
      players.Remove(_playerID);
    }

    public static Player GetPlayer(string _playerID){
      return players[_playerID];
      return null;
    }

    /*
    private void OnGUI(){
      GUILayout.BeginArea(new Rect(200, 200, 200, 500));
      GUILayout.BeginVertical();

      foreach(string _playerID in players.Keys){
        GUILayout.Label(_playerID + " - " + players[_playerID].transform.name);
      }
      GUILayout.EndVertical();
      GUILayout.EndArea();
    }
    */

}
