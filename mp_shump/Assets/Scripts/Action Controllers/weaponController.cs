﻿using UnityEngine;

public class weaponController : actionController, IStatBroadcast, IInformationBroadcast
{
    public PlayerInformationController targetInformationUi
    {
        get;
        set;
    }

    [Header("Weapons")]
    public GameObject[] primaryWeapons;
    public GameObject[] secondaryWeapons;

    public weapon currentPrimary
    {
        get;
        private set;
    }
    public weapon currentSecondary
    {
        get;
        private set;
    }

    public int weaponLevel;
    private int currentWeaponLevel;

    //Stat Broadcast
    public LocalUiController targetLocalUi
    {
        get;
        set;
    }

    void Awake()
    {
        currentWeaponLevel = weaponLevel;
        updateCurrentWeapons(0);
    }

    public void updateCurrentWeapons(int levelIncrease)
    {
        currentWeaponLevel += levelIncrease;

        if (primaryWeapons.Length > 0 && currentWeaponLevel < primaryWeapons.Length)
        {
            GameObject primary = Instantiate(primaryWeapons[currentWeaponLevel], transform.position, Quaternion.identity) as GameObject;
            if (primary)
            {
                primary.transform.SetParent(transform);
                primary.transform.rotation = primary.transform.parent.rotation;

                currentPrimary = primary.GetComponent<weapon>();
            }
        }
        if (secondaryWeapons.Length > 0 && currentWeaponLevel < secondaryWeapons.Length)
        {
            GameObject secondary = Instantiate(secondaryWeapons[currentWeaponLevel], transform.position, Quaternion.identity) as GameObject;
            if (secondary)
            {
                secondary.transform.SetParent(transform);
                secondary.transform.rotation = secondary.transform.parent.rotation;

                currentSecondary = secondary.GetComponent<weapon>();
            }
        }
    }

    void Update()
    {
        if (targetInformationUi != null)
        {
            targetInformationUi.SetPrimaryIcon(currentPrimary.GetComponent<Icon>().icon);
            targetInformationUi.SetSecondaryIcon(currentSecondary.GetComponent<Icon>().icon);
        }
        if (isActive)
        {
            if (currentPrimary != null)
            {
                if (currentPrimary.canFire)
                {
                    currentPrimary.fireWeapon(input.primary.WasPressed);
                    targetLocalUi.updatePrimaryMeter(currentPrimary.currentAmmo, currentPrimary.maxAmmo);
                }

            }
            if (currentSecondary != null)
            {
                if (currentSecondary.canFire)
                {
                    currentSecondary.fireWeapon(input.secondary.WasPressed);
                    targetLocalUi.updateSecondaryMeter(currentSecondary.currentAmmo, currentSecondary.maxAmmo);
                }
            }
        }
    }
}