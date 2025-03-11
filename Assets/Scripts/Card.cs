using UnityEngine;
using DG.Tweening;

public class Card : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite backSprite;
    private Sprite AnimalSprite;

    private bool isFliped = false; // 뒤집혀져 있나
    private bool isFliping = false; // 뒤집는 중인가
    private bool isMatched = false; // Match된 Card인가

    private int cardID; // 같은 Sprite의 카드는 같은 ID를 가짐

    private void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void FlipCard() {
        isFliping = true;
        Sequence flipSequence = DOTween.Sequence();
        flipSequence.Append(transform.DORotate(new Vector3(0, transform.eulerAngles.y + 90f, 0), 0.2f, RotateMode.Fast))
                    .AppendCallback(() => { // 90도 뒤집은 후 카드 Sprite 변경
                        isFliped = !isFliped; // 카드의 상태를 반대로 변경
                        spriteRenderer.sprite = (isFliped ? AnimalSprite : backSprite); // 카드의 상태에 따라 Sprite를 변경
                    })
                    .Append(transform.DORotate(new Vector3(0, transform.eulerAngles.y + 180f, 0), 0.2f, RotateMode.Fast)) // Sprite 변경 후 카드를 보이게
                    .OnComplete(() => {
                        isFliping = false;
                    });
    }

    private void OnMouseDown() {
        if (GameManager.instance.GetIsGameOver()) {
            return;
        }
        
        // Match된 카드이거나, 카드를 뒤집는 중이거나, 이미 뒤집혀져 있거나, 준비가 안 됐다면 클릭 Event 실행 X
        if (!isMatched && !isFliping && !isFliped && GameManager.instance.GetIsReady()) {
            FlipCard();
            GameManager.instance.ClikedCard(this); // Click한 Card의 Info를 넘겨줌
        }
    }

    public void SetAnimalSprite(Sprite sprite) {
        AnimalSprite = sprite;
    }

    public void SetCardID(int id) {
        cardID = id;
    }

    public int GetCardID() {
        return cardID;
    }

    public void SetIsMatched() {
        isMatched = true;
    }
}
