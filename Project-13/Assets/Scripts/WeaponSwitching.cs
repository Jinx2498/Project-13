using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{
    public int selectedWeapon = 0;      //Weapon list
    public GrenadeThrower grenadeScript;
    public int numGrenades = 2;

    void Start()
    {
        SelectWeapon();
    }

    void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        //Mouse scroll up for weapon change
        if ((Input.GetAxis("Mouse ScrollWheel") > 0f) && selectedWeapon < transform.childCount - 2)      //Minus 2 to allow for the grenade slot to remain untouched
        {
            selectedWeapon++;
        }

        //Mouse scroll down for weapon change
        if ((Input.GetAxis("Mouse ScrollWheel") < 0f) && selectedWeapon > 0)
        {
            selectedWeapon--;
        }

        //Change weapons through number keys
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedWeapon = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 2)
        {
            selectedWeapon = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 3)
        {
            selectedWeapon = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) && transform.childCount >= 4)
        {
            selectedWeapon = 3;
        }
        if (Input.GetKey("q") && transform.childCount >= 5 && numGrenades > 0)
        {
            selectedWeapon = 4;
        }

        if (selectedWeapon == 4 && numGrenades == 0)
        {
            selectedWeapon = 0;
        }

        //change weapon
        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;

        //Visual weapon change
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}