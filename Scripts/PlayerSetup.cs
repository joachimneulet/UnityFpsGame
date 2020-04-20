using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;
    [SerializeField]
    private string remoteLayerName = "RemotePlayer";
    [SerializeField]
    private string dontDrawLayerName = "DontDraw";
    [SerializeField]
    private GameObject playerGraphics;

    [SerializeField]
    private GameObject playerUIPrefab;
    [HideInInspector]
    public GameObject playerUIInstance;

    private void Start(){
      if(!isLocalPlayer){
        DisableComponents();
        AssignRemoteLayer();
      }else{

        //Disable local player graphics on cam
        SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));
        //CreationUI
        playerUIInstance = Instantiate(playerUIPrefab);
        playerUIInstance.name = playerUIPrefab.name;

        PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
        if(ui == null){
          Debug.LogError("No UI Component found");
        }else{
          ui.SetPlayer(GetComponent<Player>());
        }
        GetComponent<Player>().SetupPlayer();
      }
    }

    private void SetLayerRecursively(GameObject obj, int newLayer){
      obj.layer = newLayer;
      foreach(Transform child in obj.transform) {
        SetLayerRecursively(child.gameObject, newLayer);
      }
    }

    public override void OnStartClient(){
      base.OnStartClient();

      string _netID = GetComponent<NetworkIdentity>().netId.ToString();
      Player _player = GetComponent<Player>();
      GameManager.RegisterPlayer(_netID, _player);
    }

    private void AssignRemoteLayer(){
      gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    private void DisableComponents(){
      for(int i = 0; i < componentsToDisable.Length; i++){
        componentsToDisable[i].enabled = false;
      }
    }

    private void OnDisable(){
      Destroy(playerUIInstance);
      if(isLocalPlayer){
        GameManager.gm.SetSceneCameraActive(true);
      }
      GameManager.UnregisterPlayer(transform.name);
    }
}
