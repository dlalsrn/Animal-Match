using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Sprite[] cardSprites;

    private List<int> cardIDList = new List<int>();
    private List<Card> cardList = new List<Card>();

    void Start() {
        InitCardIDList();
        ShuffleCard();
        InitGameBoard();
    }

    private void InitCardIDList() {
        for (int i = 0; i < cardSprites.Length; i++) {
            cardIDList.Add(i);
            cardIDList.Add(i);
        }
    }

    private void ShuffleCard() { // 카드가 Random하게 생성되도록 ID를 Shuffle
        for (int i = 0; i < cardIDList.Count - 1; i++) {
            int randomIndex = Random.Range(i + 1, cardIDList.Count); // 자기 자신을 제외한 다른 ID 중 하나를 선택
            int t = cardIDList[randomIndex];
            cardIDList[randomIndex] = cardIDList[i];
            cardIDList[i] = t;
        }
    }

    private void InitGameBoard() {
        int cardIndex = 0; // 현재 생성된 카드 개수

        float paddingY = 1.8f;
        float paddingX = 1.3f;
        int rowCount = 5;
        int colCount = 4;

        for (int row = 0; row < rowCount; row++) {
            for (int col = 0; col < colCount; col++) {
                // 0, 0을 기준으로 카드를 배치
                Vector3 pos = new Vector3((col - colCount / 2) * paddingX + (paddingX / 2), (row - rowCount / 2) * paddingY, 0f);
                GameObject cardObject = Instantiate<GameObject>(cardPrefab, pos, Quaternion.identity);
                Card card = cardObject.GetComponent<Card>();
                card.SetCardID(cardIDList[cardIndex++]); // 0 - 9 사이의 Card ID로 Set
                card.SetAnimalSprite(cardSprites[card.GetCardID()]); // ID에 해당하는 Sprite로 Set
                cardList.Add(card);
            }
        }
    }

    public List<Card> GetCardList() {
        return cardList;
    }

    public int GetCardSet() { // 카드 쌍의 개수 return
        return cardSprites.Length;
    }
}
