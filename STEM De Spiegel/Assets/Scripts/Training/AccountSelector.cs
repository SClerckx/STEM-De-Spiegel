using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountSelector : MonoBehaviour
{
    public AccountDropdown accountDropdown;
    public ResistanceDropdown resistanceDropdown;

    // Start is called before the first frame update
    void Start()
    {
        accountDropdown = GetComponentInChildren<AccountDropdown>();
        resistanceDropdown = GetComponentInChildren<ResistanceDropdown>();
    }
}
