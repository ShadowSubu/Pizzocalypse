using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spitprojectile : MonoBehaviour
{
   void Awake()
    {
        Destroy(gameObject,3f);
    }
}
