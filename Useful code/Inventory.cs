using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private RectTransform inventoryRect;
    public GameObject slotPrefab;
    private static Slot from, to;
    public EventSystem eventSystem;
    public GameObject messagePanel;

    private float inventoryWidth, inventoryHeight;

    public int slots;
    public int rows;
    public float slotPaddingLeft, slotPaddingTop;
    public float slotSize;

    private List<GameObject> allSlots;
    private static int emptySlots;

    public GameObject iconPrefab;
    private static GameObject hoverObject;

    public Canvas canvas;
    private float hoverYOffset;


    public static int EmptySlots
    {
        get
        {
            return emptySlots;
        }

        set
        {
            emptySlots = value;
        }
    }

    void Start ()
    {
        CreateLayout();
	}
	
	void Update ()
    {

        if (Input.GetMouseButtonUp(0))
        {
            if (!eventSystem.IsPointerOverGameObject(-1) && from != null)
            {
                // change to drop or split
                from.GetComponent<Image>().color = Color.white;
                from.ClearSlot();
                Destroy(GameObject.Find("Hover"));
                to = null;
                from = null;
                hoverObject = null;
            }
        }

        if (hoverObject != null)
        {
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition,canvas.worldCamera, out position);
            position.Set(position.x, position.y - hoverYOffset);
            hoverObject.transform.position = canvas.transform.TransformPoint(position);
        }
	}

    private void CreateLayout()
    {
        allSlots = new List<GameObject>();

        hoverYOffset = slotSize * 0.01f;

        EmptySlots = slots;

        inventoryWidth = (slots / rows) * (slotSize + slotPaddingLeft) + slotPaddingLeft;
        inventoryHeight = rows * (slotSize + slotPaddingTop) + slotPaddingTop;

        inventoryRect = GetComponent<RectTransform>();
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, inventoryWidth);
        inventoryRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, inventoryHeight);

        int columns = slots / rows;
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                GameObject newSlot = (GameObject)Instantiate(slotPrefab);
                RectTransform slotRect = newSlot.GetComponent<RectTransform>();
                newSlot.name = "Slot";
                newSlot.transform.SetParent(GameObject.Find("Inv").transform);
                slotRect.localPosition = inventoryRect.localPosition + new Vector3(slotPaddingLeft * (x+1)+(slotSize*x), -slotPaddingTop * (y+1)-slotSize * y);

                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,slotSize);
                slotRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotSize);

                allSlots.Add(newSlot);

            }
        }
    }

    public bool AddItem(Item item)
    {
        if (item.maxSize == 1)
        {
            PlaceEmpty(item);
            return true;
        }
        else
        {
            foreach (GameObject slot in allSlots)
            {
                Slot tmp = slot.GetComponent<Slot>();
                if(!tmp.IsEmpty)
                {
                    if(tmp.CurrentItem.type == item.type && tmp.IsAvailable)
                    {
                        tmp.AddItem(item);
                        return true;
                    }
                }
            }
            if (EmptySlots > 0)
            {
                PlaceEmpty(item);
            }
        }
        return false;
    }

    private bool PlaceEmpty(Item item)
    {
        if (EmptySlots > 0)
        {
            foreach (GameObject slot in allSlots)
            {
                Slot tmp = slot.GetComponent<Slot>();

                if(tmp.IsEmpty)
                {
                    tmp.AddItem(item);
                    EmptySlots--;
                    return true;
                }
            }
        }
        return false;
    }

    public void MoveItem(GameObject clicked)
    {
        if (from == null)
        {
            if (!clicked.GetComponent<Slot>().IsEmpty)
            {
                from = clicked.GetComponent<Slot>();
                from.GetComponent<Image>().color = Color.gray;

                hoverObject = (GameObject)Instantiate(iconPrefab);
                hoverObject.GetComponent<Image>().sprite = clicked.GetComponent<Image>().sprite;
                hoverObject.name = "Hover";

                RectTransform hoverTransform = hoverObject.GetComponent<RectTransform>();
                RectTransform clikedTransform = clicked.GetComponent<RectTransform>();

                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, clikedTransform.sizeDelta.x);
                hoverTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clikedTransform.sizeDelta.y);

                hoverObject.transform.SetParent(GameObject.Find("UI").transform, true);
                hoverObject.transform.localScale = from.gameObject.transform.localScale;
            }
        }
        else if (to == null)
        {
            to = clicked.GetComponent<Slot>();
            Destroy(GameObject.Find("Hover"));
        }
        if (to != null && from != null)
        {
            Stack<Item> tmpTo = new Stack<Item>(to.Items);
            to.AddItems(from.Items);

            if (tmpTo.Count ==0)
            {
                from.ClearSlot();
            }
            else
            {
                from.AddItems(tmpTo);
            }
            from.GetComponent<Image>().color = Color.white;
            to = null;
            from = null;
            hoverObject = null;
        }
    }

    public void OpenMessagePanel(string text)
    {
        messagePanel.SetActive(true);
    }

    public void CloseMessagePanel()
    {
        messagePanel.SetActive(false);
    }
}
