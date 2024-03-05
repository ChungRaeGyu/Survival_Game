using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreManager : MonoBehaviour
{
    public Text CoinText;
    int Potion=0; //10원
    int Bow=0; //20원
    int Diamond=0; //50원
    int sum =0;
    ItemGrid storeGrid;
    public PlayerData playerData;
    // Start is called before the first frame update
    void Start()
    {
        storeGrid = GameObject.Find("StoreGrid").GetComponent<ItemGrid>();
    }

    // Update is called once per frame
    void Update()
    {
        SetCoinText();
    }

    private void SetCoinText()
    {
        sum = Potion * 10 + Bow * 20 + Diamond * 50;
        CoinText.text = "" + sum;
    }

    //이 그리드에 물건이 올라 오면 자동으로 CoinText를 변경한다.
    //거래하기를 눌렀을 때 그리드안에 모든걸 삭제
    public void ItemUpdate(string name,int num){
        Debug.Log("아이템 업데이트 실행 ");
        switch(name){
            case "Potion" :
                Potion += num; 
                break;
            case "Bow":
                Bow+= num; break;
            case "Diamond":
                Diamond+= num; break;

        }
    }
    void Alldestory(){

    }
    public void SellButton(){
        storeGrid.CleanGridAll();
        playerData.Money+=sum;
        Potion=0;
        Bow =0;
        Diamond=0;
        //돈 지급 하는거 넣기 과연 
    }
    
}
