using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {
    gun, bodyPart
}

public class Item : MonoBehaviour {

    public string itemName;
    public ItemType type;

    [Space (10)]
    public GameObject itemPrefab;

}
