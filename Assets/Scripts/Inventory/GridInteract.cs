using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridInteract : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    InventoryController inventoryController;
    ItemGrid itemGrid;

    private void Awake()
    {
        inventoryController = FindObjectOfType(typeof(InventoryController)) as InventoryController;
        itemGrid = GetComponent<ItemGrid>();
    }
    //마우스가 특정요소로 진입했을 때
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("들어옴");
        inventoryController.SelectedItemGrid = itemGrid;
    }

    ////마우스가 특정요소에서 나왔을 때
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("나감");
        inventoryController.SelectedItemGrid = null;
    }


}
