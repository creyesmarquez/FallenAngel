using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler
{
    public Text StackTxt;
    public Sprite slotEmpty;
    public Sprite slotHighlighted;

    private Stack<Item> items;

    public Stack<Item> Items
    {
        get
        {
            return items;
        }

        set
        {
            items = value;
        }
    }

    public bool IsEmpty
    {
        get { return Items.Count == 0; }
    }

    public bool IsAvailable
    {
        get { return CurrentItem.maxSize > Items.Count; }
    }

    public Item CurrentItem
    {
        get{ return Items.Peek(); }
    }

    void Start ()
    {
        Items = new Stack<Item>();
        RectTransform slotRect = GetComponent<RectTransform>();
        RectTransform txtRect = StackTxt.GetComponent<RectTransform>();

        int txtScaleFactor = (int)(slotRect.sizeDelta.x * 0.60);
        StackTxt.resizeTextMaxSize = txtScaleFactor;
        StackTxt.resizeTextMinSize = txtScaleFactor;

        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, slotRect.sizeDelta.x);
        txtRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, slotRect.sizeDelta.y);
    }
	
	void Update ()
    {
		
	}

    public void AddItem(Item item)
    {
        Items.Push(item);

        if ( Items.Count > 1)
        {
            StackTxt.text = Items.Count.ToString();
        }
        ChangeSprite(item.spriteNeutral, item.spriteHighlighted);
    }

    public void AddItems( Stack<Item> items )
    {
        this.Items = new Stack<Item>(items);
        StackTxt.text = items.Count > 1 ? items.Count.ToString() : string.Empty;
        ChangeSprite(CurrentItem.spriteNeutral, CurrentItem.spriteHighlighted);
    }

    private void ChangeSprite(Sprite neutral, Sprite highlight)
    {
        GetComponent<Image>().sprite = neutral;

        SpriteState st = new SpriteState();
        st.highlightedSprite = highlight;
        st.pressedSprite = neutral;

        GetComponent<Button>().spriteState = st;
    }

    private void UseItem()
    {
        if (!IsEmpty)
        {
            Items.Pop().Use();
            StackTxt.text = Items.Count > 1 ? Items.Count.ToString() : string.Empty;

            if (IsEmpty)
            {
                ChangeSprite(slotEmpty, slotHighlighted);
                Inventory.EmptySlots++;
            }
        }
    }

    public void ClearSlot()
    {
        items.Clear();
        ChangeSprite(slotEmpty, slotHighlighted);
        StackTxt.text = string.Empty;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            UseItem();
        }
    }
}
