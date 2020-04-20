using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform healthFill;

    [SerializeField]
    private GameObject pauseMenu;

    private Player currentPlayer;

    [SerializeField]
    private GameObject hitmarker;

    void Start(){
      hitmarker.SetActive(false);
      PauseMenu.isOn = false;
    }

    public void SetPlayer(Player _player){
      currentPlayer = _player;
    }

    private void Update(){
      SetHealthAmount(currentPlayer.GetHealthAmount());
      if(Input.GetKeyDown(KeyCode.Escape)){
        TogglePauseMenu();
      }
      if(PlayerShoot.hasHit){
        SetHitmarker();
      }
    }

    private void SetHitmarker(){
      hitmarker.SetActive(true);
      StartCoroutine("HitmarkerTimeout");

      PlayerShoot.hasHit = false;
    }

    private IEnumerator HitmarkerTimeout(){
      yield return new WaitForSeconds(0.05f);
      hitmarker.SetActive(false);
    }

    public void TogglePauseMenu(){
      pauseMenu.SetActive(!pauseMenu.activeSelf);
      PauseMenu.isOn = pauseMenu.activeSelf;
    }

    void SetHealthAmount(float amount){
      healthFill.localScale = new Vector3(amount/100f, 1f, 1f);
    }

}
