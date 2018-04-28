using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour {

    // Use this for initialization



    [SerializeField] private GameObject[] weapons;
    [SerializeField] private float switchDelay;
    private int index;
    private bool isSwitching;
	void Start () {

        InitializeWeapons();

	}

    private void InitializeWeapons()
    {
        for (int i = 0; i < weapons.Length; i++)
        {

            weapons[i].SetActive(false);
        }
        weapons[0].SetActive(true);

    }
	// Update is called once per frame
	void Update () {

        if (Input.GetAxis("Mouse ScrollWheel") > 0 && !isSwitching ) 
        {
            index++;

            if(index >= weapons.Length)
            {
                index = 0;
            }
            StartCoroutine(SwitchAfterDelay(index));
           
        }



        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && !isSwitching)
        {
            index--;

            if (index < weapons.Length)
            {
                index = weapons.Length -1;
            }
            StartCoroutine(SwitchAfterDelay(index));

        }

    }

    private IEnumerator SwitchAfterDelay(int newIndex)
    {
        isSwitching = true;
        yield return new WaitForSeconds(switchDelay);

        isSwitching = false;
        SwitchWeapons(newIndex);
    }



    private void SwitchWeapons(int newindex){


        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].SetActive(false);
        }

        weapons[newindex].SetActive(true);


        }
}



