using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/**
 * These are Interfaces to Ensure objects that create or destroy objects follow the same logic and require a method to do so
 **/
public interface ISpawner
{
    void ActivateObj(string name);
}

public interface IDespawner
{
    void DeactivateObj(string name, GameObject obj);
}
