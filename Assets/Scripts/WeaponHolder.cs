using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    public int selectedWep = 0;
    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        //int previousSelectedWeapon = selectedWep;
        //if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        //{
        //    if (selectedWep >= transform.childCount - 1)
        //        selectedWep = 0;
        //    else
        //    selectedWep++;
        //}
        //if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        //{
        //    if (selectedWep <= 0)
        //        selectedWep = transform.childCount -1;
        //    else
        //        selectedWep--;
        //}
        //if(previousSelectedWeapon != selectedWep)
        //{
            SelectWeapon();
        //}
    }
    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWep)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }
}
