using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform healthFill;

    [SerializeField]
    private Text ammoText;

    [SerializeField]
    private GameObject pauseMenu;

    [SerializeField]
    private GameObject hitmarker;

    [SerializeField]
    private Image _crosshairTop;
    [SerializeField]
    private Image _crosshairBottom;
    [SerializeField]
    private Image _crosshairLeft;
    [SerializeField]
    private Image _crosshairRight;

    private Player currentPlayer;
    private WeaponManager weaponManager;

    private int leftPos = -10;
    private int rightPos = 10;
    private int topPos = 10;
    private int bottomPos = -10;

    void Start(){
      hitmarker.SetActive(false);
      PauseMenu.isOn = false;
    }

    public void SetPlayer(Player _player){
      currentPlayer = _player;
      weaponManager = _player.GetComponent<WeaponManager>();
    }

    private void Update(){
      SetHealthAmount(currentPlayer.GetHealthAmount());
      SetAmmoAmount(weaponManager.GetCurrentWeapon().currentAmmo);
      if(Input.GetKeyDown(KeyCode.Escape)){
        TogglePauseMenu();
      }
      if(PlayerShoot.hasHit){
        SetHitmarker();
      }

      if(PlayerController.isMoving){
        leftPos--;
        rightPos++;
        topPos++;
        bottomPos--;
        if(leftPos <= -50){leftPos = -50;}
        if(rightPos >= 50){rightPos = 50;}
        if(topPos >= 50){topPos = 50;}
        if(bottomPos <= -50){bottomPos = -50;}
      }else{
        leftPos++;
        rightPos--;
        topPos--;
        bottomPos++;
        if(leftPos >= -10){leftPos = -10;}
        if(rightPos <= 10){rightPos = 10;}
        if(topPos <= 10){topPos = 10;}
        if(bottomPos >= -10){bottomPos = -10;}
      }

      if(PlayerController.isSprinting){
        leftPos = -70;
        rightPos = 70;
        topPos = 70;
        bottomPos = -70;
      }

      SetCrosshair();
    }

    private void SetCrosshair(){
      //clamp values

      _crosshairLeft.rectTransform.anchoredPosition = new Vector2(leftPos, 0);
      _crosshairRight.rectTransform.anchoredPosition = new Vector2(rightPos, 0);
      _crosshairTop.rectTransform.anchoredPosition = new Vector2(0, topPos);
      _crosshairBottom.rectTransform.anchoredPosition = new Vector2(0, bottomPos);
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
      healthFill.localScale = new Vector3(amount, 1f, 1f);
    }

    void SetAmmoAmount(int amount){
      ammoText.text = amount.ToString();
    }

}
