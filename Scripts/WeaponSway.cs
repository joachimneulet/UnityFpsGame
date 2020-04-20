using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [SerializeField]
    private float amount;
    [SerializeField]
    private float maxAmount;
    [SerializeField]
    private float smoothAmount;

    private Vector3 initalPosition;
    // Start is called before the first frame update
    void Start()
    {
        initalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(!PlayerShoot.isAiming){
          float _movX = -Input.GetAxis("Mouse X") * amount;
          float _movY = -Input.GetAxis("Mouse Y") * amount;
          //Clamp values
          _movX = Mathf.Clamp(_movX, -maxAmount, maxAmount);
          _movY = Mathf.Clamp(_movY, -maxAmount, maxAmount);
          Vector3 finalPosition = new Vector3(_movX, _movY, 0);
          transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition + initalPosition, Time.deltaTime * smoothAmount);
        }
    }
}
