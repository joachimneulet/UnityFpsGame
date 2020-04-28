using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead = false;
    public bool isDead{
      get {return _isDead;}
      protected set { _isDead = value;}
    }

    [SerializeField]
    private float maxHealth = 100f;

    [SyncVar]
    private float currentHealth;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    [SerializeField]
    private GameObject[] disableGameObjectOnDeath;

    private bool firstSetup = true;
    private bool[] wasEnabled;

    [SerializeField]
    private GameObject deathEffect;

    public float GetHealthAmount(){
      return (float)currentHealth / maxHealth;
    }

    public void SetupPlayer(){
      if(isLocalPlayer){
        //Change cam on death
        GameManager.gm.SetSceneCameraActive(false);
        GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
      }

      CmdBroadcastNewPlayerSetup();
    }

    [Command]
    private void CmdBroadcastNewPlayerSetup(){
      RpcSetupPlayerOnAllClients();
    }

    [ClientRpc]
    private void RpcSetupPlayerOnAllClients(){
      if(firstSetup){
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < disableOnDeath.Length; i++) {
          wasEnabled[i] = disableOnDeath[i].enabled;
        }
        firstSetup = false;
      }

      SetDefault();
    }

    private void Update(){
      if(!isLocalPlayer){
        return;
      }
    }

    [ClientRpc]
    public void RpcTakeDamage(float _amount){
      if(isDead){
        return;
      }
      currentHealth -= _amount;
      Debug.Log(transform.name + " a " + currentHealth + " pv");
      if(currentHealth <= 0){
        KillPlayer();
      }
    }

    private void KillPlayer(){
      isDead = true;
      //Disable components
      for (int i = 0; i < disableOnDeath.Length; i++) {
        disableOnDeath[i].enabled = false;
      }
      for (int i = 0; i < disableGameObjectOnDeath.Length; i++) {
        disableGameObjectOnDeath[i].SetActive(false);
      }
      //Disable collider
      Collider _col = GetComponent<Collider>();
      if(_col != null){
        _col.enabled = false;
      }

      //Death particle effect
      GameObject _gfxIns = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
      Destroy(_gfxIns, 3f);

      //Change cam on death
      if(isLocalPlayer){
        GameManager.gm.SetSceneCameraActive(true);
        GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
      }

      Debug.Log(transform.name + " is dead");
      StartCoroutine(Respawn());
    }

    private IEnumerator Respawn(){
      //Makes reference to respawnTime from GameManager
      yield return new WaitForSeconds(GameManager.gm.matchSettings.respawnTime);

      Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
      transform.position = _spawnPoint.position;
      transform.rotation = _spawnPoint.rotation;
      yield return new WaitForSeconds(0.1f);
      SetupPlayer();

      Debug.Log(transform.name + " has respawned");
    }

    private void SetDefault(){
      isDead = false;
      currentHealth = maxHealth;

      //Enable player components
      for (int i = 0; i < disableOnDeath.Length; i++) {
        disableOnDeath[i].enabled = wasEnabled[i];
      }
      //Enable Players GameObjects
      for (int i = 0; i < disableGameObjectOnDeath.Length; i++) {
        disableGameObjectOnDeath[i].SetActive(true);
      }

      Collider _col = GetComponent<Collider>();
      if(_col != null){
        _col.enabled = true;
      }
    }
}
