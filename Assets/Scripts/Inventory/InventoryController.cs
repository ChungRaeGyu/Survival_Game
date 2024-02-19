using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    //[HideInInspector]
    public ItemGrid selectedItemGrid;

    InventoryHighlight inventoryHighlight;
    public ItemGrid SelectedItemGrid { 
        get => selectedItemGrid; 
        set {
            selectedItemGrid = value; 
            inventoryHighlight.SetParent(value);
        }
    }
    GameObject Inventory;
    [SerializeField] ItemGrid InventoryGrid;
    GameObject ItemBox;
    GameObject Storage;
    public bool InventoryOpen = false;

    
    [SerializeField] InventoryItem selectedItem;
    InventoryItem overlapItem;
    RectTransform rectTransform;

    [SerializeField] List<ItemData> items;
    [SerializeField] GameObject itemPrefeb;
    [SerializeField] Transform canvasTransform;    

    

    //ItemBox

    [SerializeField] ItemGrid ItmeBoxGird;
    
    //Don'tDestroyOnLoad
    private static InventoryController instance =null;

    private void Awake()
    {
        DontDestroyOnLoad_Singleton();
        
    }
    private void DontDestroyOnLoad_Singleton()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start() {
        SetItemBox();
        UISetting();
    }
    private void UISetting()
    {
        GameObject InvenCanvas = GameObject.Find("Inventory_Canvas");
        Inventory = InvenCanvas.transform.GetChild(0).gameObject;
        ItemBox = InvenCanvas.transform.GetChild(1).gameObject;
        Storage = InvenCanvas.transform.GetChild(2).gameObject;
        Inventory.SetActive(false);
        ItemBox.SetActive(false);
        Storage.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryOpenKey();
        }
        if (!InventoryOpen) { return; }     
        ItemIconDrag();

        if(Input.GetKeyDown(KeyCode.Q)){
            if(selectedItem ==null){
                CreateRandomItem();
            }
        }
        if(Input.GetKeyDown(KeyCode.R)){
            RotateItem();
        }

        if (selectedItemGrid == null) {
            inventoryHighlight.Show(false);
            return; 
        }

        HandleHightlight();

        if (Input.GetMouseButtonDown(0))
        {
            LeftMouseButtonPress();
        }
        if(Input.GetMouseButtonDown(1) && Input.GetKey(KeyCode.LeftShift)){
            //InsertRandomItem(); 추후 업데이트
        }
    }
    public void InventoryOpenKey()
    {
        if (!InventoryOpen)
        {
            Inventory.SetActive(true);
        }
        else
        {
            Inventory.SetActive(false);
            SelectedItemGrid = null;
        }
        InventoryOpen = !InventoryOpen;
    }

    private void RotateItem()
    {
        if(selectedItem == null){return;}
        selectedItem.Rotate();
    }
    private void SetItemBox()
    {
        inventoryHighlight = GetComponent<InventoryHighlight>();
        if (selectedItemGrid == null)
        {
            SelectedItemGrid = ItmeBoxGird;
        }
        CreateRandomItem();
        InventoryItem itemToInsert = selectedItem;
        selectedItem = null;
        InsertItem(itemToInsert);
        SelectedItemGrid = null;
    }
    private void InsertRandomItem()
    {
        //selectedItemGrid를 Default값을 하나 잡아 놔야겠는 걸? 현재 grid에 마우스를 올려 놔야만 아이템이 들어간다.
        if (selectedItemGrid == null) {
            SelectedItemGrid = InventoryGrid;
        }
        CreateRandomItem();
        InventoryItem itemToInsert = selectedItem;
        selectedItem = null;
        InsertItem(itemToInsert);
        
    }

    private void CreateRandomItem()
    {
        InventoryItem inventoryItem = Instantiate(itemPrefeb).GetComponent<InventoryItem>();
        selectedItem = inventoryItem;

        rectTransform = inventoryItem.GetComponent<RectTransform>();
        rectTransform.SetParent(canvasTransform);
        rectTransform.SetAsLastSibling(); //가장 상위 레이어로 이동

        int selectedItemID = UnityEngine.Random.Range(0, items.Count);
        inventoryItem.Set(items[selectedItemID]);
        Debug.Log(inventoryItem);
    }
    private void InsertItem(InventoryItem itemToInsert)
    {        
        Vector2Int? posOnGrid = selectedItemGrid.FindSpaceForObject(itemToInsert); //null허용
        if(posOnGrid ==null){return;}
        selectedItemGrid.PlaceItem(itemToInsert, posOnGrid.Value.x, posOnGrid.Value.y);
    }

    Vector2Int oldPosition;
    InventoryItem itemToHighlight;


    private void HandleHightlight()
    {
        Vector2Int positionOnGrid = GetTileGridPosition();
        if(oldPosition==positionOnGrid){return;} //같은 타일에 있을때 반복을 막기위해서
        oldPosition = positionOnGrid;
        if (selectedItem ==null){
            itemToHighlight = selectedItemGrid.GetItem(positionOnGrid.x,positionOnGrid.y);
            if(itemToHighlight!=null){
                inventoryHighlight.Show(true);
                inventoryHighlight.SetSize(itemToHighlight);
                inventoryHighlight.SetPosition(selectedItemGrid, itemToHighlight);
            }else{
                inventoryHighlight.Show(false);
            }
        }else{ //물건을 잡았을 때 이벤토리에 넣을 수 없으면 highlighter을 숨긴다.
            inventoryHighlight.Show(selectedItemGrid.boundryCheck(
                positionOnGrid.x,
                positionOnGrid.y,
                selectedItem.WIDTH,
                selectedItem.HEIGHT
            ));
            inventoryHighlight.SetSize(selectedItem);
            inventoryHighlight.SetPosition(selectedItemGrid, selectedItem,positionOnGrid.x,positionOnGrid.y);
        }
    }



    private void LeftMouseButtonPress()
    {
        Vector2Int tileGridPosition = GetTileGridPosition();

        if (selectedItem == null)
        {
            PickUpItem(tileGridPosition);
        }
        else
        {
            PlaceItem(tileGridPosition);
        }
    }

    private Vector2Int GetTileGridPosition()
    {
        Vector2 position = Input.mousePosition;

        if (selectedItem != null)
        {
            position.x -= (selectedItem.WIDTH - 1) * ItemGrid.tileSizeWidth / 2;
            position.y += (selectedItem.HEIGHT - 1) * ItemGrid.tileSizeHeight / 2;
        }
        
        return selectedItemGrid.GetTileGridPosition(position);
    }

    private void PlaceItem(Vector2Int tileGridPosition)
    {
        bool complete =selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y,ref overlapItem);
        if(complete){
            selectedItem = null;
            if(overlapItem !=null){ //들고 있던것과 원래 배치된것을 바꾼다. 이러기 위해서 overlapItem이 필요하다.
                selectedItem = overlapItem;
                overlapItem = null;
                rectTransform = selectedItem.GetComponent<RectTransform>();
                rectTransform.SetAsLastSibling();
            }
        }
        
    }

    private void PickUpItem(Vector2Int tileGridPosition)
    {
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
        if (selectedItem != null)
        {
            rectTransform = selectedItem.GetComponent<RectTransform>();
            rectTransform.SetAsLastSibling();
        }
    }

    private void ItemIconDrag()
    {
        if (selectedItem != null)
        {
            rectTransform.position = Input.mousePosition;
        }
    }
}
