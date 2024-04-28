using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class VFXManager : MonoBehaviour
{
    //Actions
    public static Action<VFXObject, Vector3, Quaternion> RequestVFX;
    //Settings
    [SerializeField] private VFXManager_Settings m_ManagerSettings;
    //Pooler List
    private List<VFXObject> m_VFXPool;
    
    private void Awake()
    {
        //Set Up List
        m_VFXPool = new List<VFXObject>();
        for(int i=0; i<m_ManagerSettings.MaxVFX; i++)
        {
            m_VFXPool.Add(Instantiate(m_ManagerSettings.DefaultParticleSystem, transform));
            m_VFXPool[m_VFXPool.Count-1].gameObject.SetActive(false);
        }

        //Connect Action
        RequestVFX += PlaceVFX;
    }

    private void PlaceVFX(VFXObject VFX, Vector3 position, Quaternion rotation)
    {
        if(VFX == null)
            return;
        
        //Find first free VFX
        for (int i = 0; i < m_VFXPool.Count; i++)
        {
            if(!m_VFXPool[i].gameObject.activeInHierarchy)
            {
                //TODO: Fix this thing
                //m_VFXPool[i] = VFX;
                m_VFXPool[i].transform.position = position;
                m_VFXPool[i].transform.rotation = rotation;

                m_VFXPool[i].gameObject.SetActive(true);
                return;
            }
        }

        ////No free VFX Found -> if size is less than max -> Add new one
        //if (m_ManagerSettings.MaxVFX > m_VFXPool.Count)
        //{
        //    //Add Object To Pooler
        //    m_VFXPool.Add(Instantiate(VFX, position, rotation));
        //    m_VFXPool[m_VFXPool.Count - 1].Play();
        //}
    }

    private void OnDisable()
    {
        //Disconnect Event
        RequestVFX -= RequestVFX;
    }
}
