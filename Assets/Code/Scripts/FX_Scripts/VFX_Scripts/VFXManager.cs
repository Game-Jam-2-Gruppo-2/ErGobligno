using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    //Singeton
    [HideInInspector] public static VFXManager Instance;
    //Actions
    public static Action<VFXObject, Vector3, Quaternion> RequestVFX;
    //Settings
    [SerializeField] private VFXManager_Settings m_ManagerSettings;
    //Pooler List
    private List<VFXObject> m_VFXList;
    
    private void Awake()
    {
        //Set Up Singleton
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("MULTIPLE VFX MANAGERS FOUND");
            Destroy(this.gameObject);
            return;
        }

        //Set Up List
        m_VFXList = new List<VFXObject>();
        //Connect Events
        RequestVFX += PlaceVFX;
    }

    private void PlaceVFX(VFXObject VFX, Vector3 position, Quaternion rotation)
    {
        if(VFX == null)
            return;

        //Find first free VFX
        for (int i = 0; i < m_VFXList.Count; i++)
            if(m_VFXList[i].isActiveAndEnabled)
            {
                m_VFXList[i] = VFX;
                m_VFXList[i].transform.position = position;
                m_VFXList[i].transform.rotation = rotation;
                m_VFXList[i].gameObject.SetActive(true);
                return;
            }
        
        //No free VFX Found -> if size is less than max -> Add new one
        if (m_ManagerSettings.MaxVFX > m_VFXList.Count)
        {
            //Add Object To Pooler
            m_VFXList.Add(Instantiate(VFX, position, rotation));
        }
    }

    private void OnDisable()
    {
        //Disconnect Event
        RequestVFX -= RequestVFX;
    }
}
