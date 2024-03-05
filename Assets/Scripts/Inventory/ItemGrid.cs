using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class ItemGrid : MonoBehaviour
{
    public const float tileSizeWidth = 32;  //const 변경할 수 없는 수
    public const float tileSizeHeight = 32;

    InventoryItem[,] inventoryItemSlot;

    RectTransform rectTransform; 

    [SerializeField] int girdSizeWidth = 20;
    [SerializeField] int girdSizeheight = 10;
    [SerializeField] Transform canvasTransform;


    void Awake()
    {
        rectTransform = GetComponent<RectTransform>(); //UI의 기준점 위치
        Init(girdSizeWidth, girdSizeheight);
    }
    private void Start()
    {
        //StartCoroutine(wait());
    }
    IEnumerator wait(){
        yield return new WaitForSecondsRealtime(0.1f);
        Debug.Log("실행");
        gameObject.SetActive(false);
    }
    //Set Inventroy size
    private void Init(int width, int height)
    {        
        inventoryItemSlot = new InventoryItem[width,height];
        Vector2 size = new Vector2(width * tileSizeWidth,height*tileSizeHeight);
        rectTransform.sizeDelta = size;
        Debug.Log("Itemgrid : " +this.name);
    }

    Vector2 positionOntheGrid = new Vector2();  //Inventory상의 기준점 설정
    Vector2Int tileGridePosition = new Vector2Int();
    //카메라 모드 오버레이일때 화면의 고정된 위치에 생성
    public Vector2Int GetTileGridPosition(Vector2 mousePosition)    
    {
        positionOntheGrid.x = mousePosition.x - rectTransform.position.x;
        positionOntheGrid.y = rectTransform.position.y - mousePosition.y;

        tileGridePosition.x = (int)(positionOntheGrid.x/tileSizeWidth);
        tileGridePosition.y = (int)(positionOntheGrid.y/tileSizeHeight);
        return tileGridePosition;
    }

    public bool PlaceItem(InventoryItem inventoryItem,int posX,int posY, ref InventoryItem overlapItem)
    {
        if (boundryCheck(posX, posY, inventoryItem.WIDTH, inventoryItem.HEIGHT) == false)
        {
            return false;
        }

        if (OverlapCheck(posX, posY, inventoryItem.WIDTH, inventoryItem.HEIGHT, ref overlapItem) == false)
        {
            overlapItem = null;
            return false;
        }

        if (overlapItem != null)
        {
            CleanGridReference(overlapItem);
        }

        PlaceItem(inventoryItem, posX, posY);

        return true;
    }

    public void PlaceItem(InventoryItem inventoryItem, int posX, int posY)
    {
        RectTransform rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(this.rectTransform);

        for (int x = 0; x < inventoryItem.WIDTH; x++)
        {
            for (int y = 0; y < inventoryItem.HEIGHT; y++)
            {
                inventoryItemSlot[posX + x, posY + y] = inventoryItem; //인벤토리의 어디에 위치해있는지
            }
        }

        inventoryItem.onGridPositionX = posX;
        inventoryItem.onGridPositionY = posY;

        Vector2 position = CalculatePositionOnGrid(inventoryItem, posX, posY);

        rectTransform.localPosition = position;
    }

    public Vector2 CalculatePositionOnGrid(InventoryItem inventoryItem, int posX, int posY)
    {
        Vector2 position = new Vector2();
        position.x = posX * tileSizeWidth + tileSizeWidth * inventoryItem.WIDTH / 2;
        position.y = -(posY * tileSizeHeight + tileSizeHeight * inventoryItem.HEIGHT / 2);
        return position;
    }

    private bool OverlapCheck(int posX, int posY, int width, int height, ref InventoryItem overlapItem)
    {
        for(int x =0; x < width; x++){
            for(int y=0; y<height; y++){
                if(inventoryItemSlot[posX+x,posY+y]!=null){ //!=null일때 return false하면 안되나?
                    if(overlapItem ==null){
                        overlapItem = inventoryItemSlot[posX + x, posY + y];
                    }else{
                        if(overlapItem != inventoryItemSlot[posX + x, posY + y]){
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }

    public InventoryItem PickUpItem(int x, int y)
    {
        InventoryItem toReturn = inventoryItemSlot[x, y];

        if (toReturn == null) { return null; }
        CleanGridReference(toReturn);
        return toReturn;
    }

    private void CleanGridReference(InventoryItem item)
    {
        for (int ix = 0; ix < item.WIDTH; ix++)
        {
            for (int iy = 0; iy < item.HEIGHT; iy++)
            {
                inventoryItemSlot[item.onGridPositionX + ix, item.onGridPositionY + iy] = null;
            }
        }
        item.GetComponent<Transform>().SetParent(canvasTransform);
    }

    bool PositionCheck(int posX,int posY){
        if(posX<0||posY<0){
            return false;
        }

        if(posX >= girdSizeWidth || posY >=girdSizeheight){
            return false;
        }
        return true;
    }

    public bool boundryCheck(int posX, int posY, int width,int height){
        if(PositionCheck(posX,posY)==false){return false;}

        posX+=width-1;
        posY+=height-1;
        if (PositionCheck(posX,posY)==false){return false;}        
        
        return true;
    }

    internal InventoryItem GetItem(int x, int y)
    {
        return inventoryItemSlot[x,y];
    }

    public Vector2Int? FindSpaceForObject(InventoryItem itemToInsert)
    {
        int height = girdSizeheight - itemToInsert.HEIGHT+1;
        int width = girdSizeWidth - itemToInsert.WIDTH+1;
        
        for (int y=0; y< height; y++){
            for (int x = 0; x < width; x++)
            {
                 if(CheckAvailableSpace(x,y,itemToInsert.WIDTH,itemToInsert.HEIGHT)==true){
                     return new Vector2Int(x,y);
                 }
            }
        }
        return null;
    }

    private bool CheckAvailableSpace(int posX, int posY, int width, int height)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (inventoryItemSlot[posX + x, posY + y] != null)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void CleanGridAll()
    {
        for (int ix = 0; ix < girdSizeWidth; ix++)
        {
            for (int iy = 0; iy < girdSizeheight; iy++)
            {
                inventoryItemSlot[ix,iy] = null;
            }
        }
        Transform[] RemoveObj = GetComponentsInChildren<Transform>(); ;
        for(int i=0; i<RemoveObj.Length;i++){
            if(RemoveObj[i].CompareTag("Item")){
                Destroy(RemoveObj[i].gameObject);
            }
        }
    }

}
