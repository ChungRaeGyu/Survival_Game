using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InventoryHighlight : MonoBehaviour
{
    [SerializeField] RectTransform highlighter;

    public void Show(bool b){
        highlighter.gameObject.SetActive(b);
    }

    public void SetSize(InventoryItem targetItem){
        Vector2 size = new Vector2();
        size.x = targetItem.WIDTH * ItemGrid.tileSizeWidth;
        size.y = targetItem.HEIGHT * ItemGrid.tileSizeHeight;
        highlighter.sizeDelta = size;

    }

    //아이템의 크기와 위치를 받아서 highlighter의 위치를 정한다.
    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem)
    {
        Vector2 pos = targetGrid.CalculatePositionOnGrid(
            targetItem,
            targetItem.onGridPositionX,
            targetItem.onGridPositionY
        );

        highlighter.localPosition = pos;
    }

    public void SetParent(ItemGrid targetGrid)
    {
        if(targetGrid ==null){
            return;
        }
        highlighter.SetParent(targetGrid.GetComponent<RectTransform>());
    }

    //아이템을 잡았을 때 highlighter도 같이 따라다니게 만든다.
    public void SetPosition(ItemGrid targetGrid, InventoryItem targetItem, int posX, int posY){
        Vector2 pos = targetGrid.CalculatePositionOnGrid(
            targetItem,
            posX,
            posY
        );

        highlighter.localPosition = pos;
    }
}
