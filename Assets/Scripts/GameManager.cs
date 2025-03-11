using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    private GameBoard gameBoard;
    private List<Card> cardList;
    private int remainCardSet;

    private Card firstFlipedCard = null;
    private Card secondFlipedCard = null;

    private bool isReady = false;
    private bool isGameOver = false;
    private bool isResult;

    [SerializeField] private float timeLimit;
    private float currentTime;

    private Coroutine timeOutRoutine;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        gameBoard = FindObjectOfType<GameBoard>();
        cardList = gameBoard.GetCardList();
        remainCardSet = gameBoard.GetCardSet();
        currentTime = timeLimit;
        HUDManager.instance.UpdateTimeOutText((int)timeLimit);
        HUDManager.instance.UpdateTimeOutSlider(currentTime / timeLimit);
        StartCoroutine(FlipAllCardsRoutine());
    }

    IEnumerator FlipAllCardsRoutine() {
        yield return new WaitForSeconds(0.5f);
        FlipAllCards(); // 처음에 카드 보이게
        yield return new WaitForSeconds(2.5f); // 동물 카드를 보여주는 시간
        FlipAllCards(); // 카드 안 보이게
        yield return new WaitForSeconds(0.5f);
        isReady = true; // 게임 시작 준비 완료

        yield return timeOutRoutine = StartCoroutine(CountDownTimerRoutine());
    }

    private void FlipAllCards() {
        foreach (Card card in cardList) { // 모든 카드 Flip
            card.FlipCard();
        }
    }

    IEnumerator CountDownTimerRoutine() {
        while (currentTime > 0) { // 시간이 남아있으면
            currentTime -= Time.deltaTime;
            HUDManager.instance.UpdateTimeOutSlider(currentTime / timeLimit);
            HUDManager.instance.UpdateTimeOutText(Mathf.CeilToInt(currentTime));
            yield return null;
        }

        GameOver(false);
    }

    public bool GetIsReady() {
        return isReady;
    }

    public void SetIsReady(bool ready) {
        isReady = ready;
    }

    public void ClikedCard(Card card) {
        if (firstFlipedCard == null) { // 뒤집은 카드가 첫 번째라면
            firstFlipedCard = card;
        } else { // 이미 뒤집은 카드가 존재하면 현재 카드와 Match 확인
            SetIsReady(false);
            secondFlipedCard = card;
            StartCoroutine(MatchCheckRoutine());
        }
    }

    IEnumerator MatchCheckRoutine() {
        if (firstFlipedCard != null && secondFlipedCard != null) { // 2개 모두 뒤집혔다면
            if (firstFlipedCard.GetCardID() == secondFlipedCard.GetCardID()) { // 2개가 같은 카드라면
                firstFlipedCard.SetIsMatched();
                secondFlipedCard.SetIsMatched();
                remainCardSet--;
                if (remainCardSet == 0) { // 모든 카드 쌍을 다 찾았다면 게임 끝
                    GameOver(true);
                }
            } else { // Match되지 않았을 때는 되돌려야하므로 기다리는 시간 추가
                yield return new WaitForSeconds(0.7f);
                firstFlipedCard.FlipCard();
                secondFlipedCard.FlipCard();
                yield return new WaitForSeconds(0.5f);
            }
            firstFlipedCard = null;
            secondFlipedCard = null;
        }
        SetIsReady(true);
    }

    public void GameOver(bool result) { // result는 승패 여부, true면 승리, false면 패배
        if (!isGameOver) {
            isGameOver = true;
            isResult = result;
            StopCoroutine(timeOutRoutine); // 타이머 작동 중지
            Invoke("ShowGameOverPanel", 1.5f);
        }
    }

    public void ShowGameOverPanel() {
        HUDManager.instance.ShowGameOverPanel(isResult);
    }

    public bool GetIsGameOver() {
        return isGameOver;
    }

    public void LoadGameScene() {
        SceneManager.LoadScene("Scenes/GameScene");
    }

    public void LoadMenuScene() {
        SceneManager.LoadScene("Scenes/MenuScene");
    }
}
