using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public enum TypeOfTrash
{
    Bottle1 = 1, Bottle2=2, Bottle3=3,Can1=4,Can2=5, Can3=6, Can4=6,TrashBag1=7,TrashBag2=8,TrashBag3=9,TrashBag4=10,
}

public class TrashType : MonoBehaviour
{
    [SerializeField] public TypeOfTrash type;
}
