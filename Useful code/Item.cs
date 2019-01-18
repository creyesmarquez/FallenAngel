using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType { MANA, HEALTH};

public class Item : MonoBehaviour
{

    public ItemType type;

    public Sprite spriteNeutral;
    public Sprite spriteHighlighted;
    public int maxSize;

    public void Use()
    {
        switch (type)
        {
            case ItemType.MANA:
                Debug.Log("Mana pot has been used.");
                break;
            case ItemType.HEALTH:
                Debug.Log("Health pot has been used.");
                break;
        }
    }
}
