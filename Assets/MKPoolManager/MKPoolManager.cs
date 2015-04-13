using UnityEngine;
using System.Collections.Generic;

// MKPoolManager created by KhacPM
// @Magik 2015
// Version 1.0

// MKPoolManager System:
// Preload GameObject to reuse them later, avoiding to Instantiate them.
// Very useful for mobile platforms.
// How to use?
// Call MKPoolManager.PreloadObject to Instantiate number of gameobjects
// The system will make those objects deactive after Instantiated.
// Call GetUnusedObject to get available GameObject (one of them) then active that object.

//===Change logs===
//==Version 1.0==Initial commit

//==Version 2.0==Garbage collector
//==Collect and remove unused objects that appear for long time

// Known issues:
// 1: Every preloadable object must have a prefab. 
// 2: Unused object appear in scene for along time will consume memory.

public class MKPoolManager : MonoBehaviour
{
    public GameObject[] objectsToPreload = new GameObject[0];
    public int[] objectsToPreloadTimes = new int[0];
    public bool hideObjectsInHierarchy;
    private bool allObjectsLoaded;
    private Dictionary<int, List<GameObject>> instantiatedObjects = new Dictionary<int, List<GameObject>>();

    void Start()
    {
        allObjectsLoaded = false;
        
        for(int i = 0; i < objectsToPreload.Length; i++)
        {
            PreloadObject(objectsToPreload[i], objectsToPreloadTimes[i]);
        }
        
        allObjectsLoaded = true;
    }

    /// <summary>
    /// Gets the unused object (Inactive Object)
    /// </summary>
    /// <returns>The unused object.</returns>
    /// <param name="sourceObj">Source object.</param>
    /// <param name="activateObject">If set to <c>true</c> activate object.</param>
    public GameObject GetUnusedObject(GameObject sourceObj, bool activateObject = true)
    {
        int uniqueId = sourceObj.GetInstanceID();
        if(!instantiatedObjects.ContainsKey(uniqueId))
        {
#if UNITY_EDITOR
            Debug.LogError("[MKPoolManager.GetUnusedObject()] Object hasn't been preloaded: " + sourceObj.name + " (ID:" + uniqueId + ")");
#endif
            return null;
        }

        GameObject unusedObj = null;

        bool allObjectsAreBusy = true;

        int arrLength = instantiatedObjects[uniqueId].Count;
#if UNITY_EDITOR
        if(arrLength > int.MaxValue)
            Debug.LogWarning("Array's size is too big, it may cause low performance!!!");
#endif
        for(int i = 0; i < arrLength; i++)
        {
            if(instantiatedObjects[uniqueId][i].activeSelf == false)
            {
                unusedObj = instantiatedObjects[uniqueId][i];
                allObjectsAreBusy = false;
                break;
            }
        }

        //if all Objects are being used, create new one.
        if(allObjectsAreBusy)
        {
            GameObject[] addedObjects = addObjectToPool(sourceObj, 1);
            unusedObj = addedObjects[0];
        }

        unusedObj.SetActive(activateObject);
        
        return unusedObj;
    }

    private GameObject[] addObjectToPool(GameObject sourceObject, int number)
    {
        int uniqueId = sourceObject.GetInstanceID();
        
        //Add new entry if it doesn't exist
        if(!instantiatedObjects.ContainsKey(uniqueId))
        {
            instantiatedObjects.Add(uniqueId, new List<GameObject>());
        }

        //Add the new objects
        GameObject[] recentInstantiatedObjects = new GameObject[number];
        GameObject newObj;
        for(int i = 0; i < number; i++)
        {
            newObj = (GameObject)Instantiate(sourceObject);
            newObj.name = sourceObject.name;
            newObj.SetActive(false);

//            #if MKSHURIKEN
//            //Set flag to not destruct object
//            AutoDestructShuriken[] autoDestruct = newObj.GetComponentsInChildren<AutoDestructShuriken>(true);
//            for(int j = 0; j < autoDestruct.Length; j++)
//            {
//                autoDestruct[j].OnlyDeactivate = true;
//            }
//            //Set flag to not destruct light
//            LightIntensityFade[] lightIntensity = newObj.GetComponentsInChildren<LightIntensityFade>(true);
//            for(int k = 0; k< lightIntensity.Length; k++)
//            {
//                lightIntensity[k].autodestruct = false;
//            }
//            #endif

            #region POOL_OBJECT_RULE
            #endregion

            instantiatedObjects[uniqueId].Add(newObj);
            
            if(hideObjectsInHierarchy)
                newObj.hideFlags = HideFlags.HideInHierarchy;

            recentInstantiatedObjects[i] = newObj;
        }

        return recentInstantiatedObjects;
    }


    /// <summary>
    /// Preload or Add gameobject with a defined number.
    /// </summary>
    /// <param name="sourceObj">Source object.</param>
    /// <param name="poolSize">Pool size/Expand size.</param>
    public GameObject[] PreloadObject(GameObject sourceObj, int poolSize = 1)
    {
        return addObjectToPool(sourceObj, poolSize);
    }

    /// <summary>
    /// Unloads all the preloaded objects from a source Object.
    /// </summary>
    /// <param name='sourceObj'>Source object.</param>
    public void UnloadObjects(GameObject sourceObj)
    {
        removeObjectsFromPool(sourceObj);
    }

    /// <summary>
    /// Gets a value indicating whether all objects defined in the Editor are loaded or not.
    /// </summary>
    /// <value>
    /// <c>true</c> if all objects are loaded; otherwise, <c>false</c>.
    /// </value>
    public bool AllObjectsLoaded
    {
        get
        {
            return allObjectsLoaded;
        }
    }

    private void removeObjectsFromPool(GameObject sourceObject)
    {
        int uniqueId = sourceObject.GetInstanceID();
        
        if(!instantiatedObjects.ContainsKey(uniqueId))
        {
#if UNITY_EDITOR
            Debug.LogWarning("[MKPoolManager.removeObjectsFromPool()] There aren't any preloaded object for: " + sourceObject.name + " (ID:" + uniqueId + ")");
#endif
            return;
        }
        
        //Destroy all objects
        for(int i = instantiatedObjects[uniqueId].Count - 1; i >= 0; i--)
        {
            GameObject obj = instantiatedObjects[uniqueId][i];
            instantiatedObjects[uniqueId].RemoveAt(i);
            GameObject.Destroy(obj);
        }
        
        //Remove pool entry
        instantiatedObjects.Remove(uniqueId);
    }
}
