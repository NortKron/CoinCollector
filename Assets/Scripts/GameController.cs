using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour, IPointerDownHandler
{
    #region Serialize Fields

    [SerializeField]
    private GameObject prefabSpot;

    [SerializeField]
    private GameObject prefabLine;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject countCoins;

    [SerializeField]
    private GameObject panelStart;

    [SerializeField]
    private GameObject textGameEnd;

    [SerializeField]
    private GameObject textButton;

    [SerializeField]
    private ListCoins listCoins;

    [SerializeField]
    private ListSpikes listSpikes;

    [SerializeField]
    private Sprite spriteFailed;

    #endregion Serialize Fields

    public float speed = 4.0f;

    private List<GameObject> listSpots;
    private List<GameObject> listLines;
    
    private Vector3 startPosition;
    private Vector3 startPlayerPosition;

    private Sprite spriteMain;

    private int coins = 0;
    private bool isFIrstStart = true;

    private Coroutine coroutineMoving;
    private Coroutine coroutineCheckSpots;

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector3 spotPosition = new Vector3(
            Camera.main.ScreenToWorldPoint(eventData.position).x,
            Camera.main.ScreenToWorldPoint(eventData.position).y,
            1.0f
        );

        var spot = Instantiate(prefabSpot, spotPosition, Quaternion.identity);
        listSpots.Add(spot);

        var line = Instantiate(
            prefabLine, 
            new Vector3(0.0f, 0.0f, 0.0f), 
            Quaternion.identity);

        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPosition);
        lineRenderer.SetPosition(1, spotPosition);
        listLines.Add(line);

        startPosition = spotPosition;
    }

    private IEnumerator CheckSpots()
    {
        while (true)
        {
            GameObject activeSpot = listSpots.Where(s => s.gameObject.activeSelf == true).FirstOrDefault();

            if (activeSpot != null)
            {
                yield return coroutineMoving = StartCoroutine(MovingPlayer(activeSpot));
            }

            yield return new WaitForSeconds(1.0f);
        }
    }

    private IEnumerator MovingPlayer(GameObject spot)
    {
        while (player.transform.position != spot.transform.position)
        {
            player.transform.position = Vector3.MoveTowards(
                player.transform.position,
                spot.transform.position, 
                speed * Time.deltaTime);

            yield return null;
        }

        spot.gameObject.SetActive(false);
    }

    private void IncCoins(int index)
    {
        coins++;
        countCoins.GetComponent<TMPro.TextMeshProUGUI>().text = "Coin count : " + coins;
        listCoins.InactiveCoinByIndex(index);
    }

    private void FinishGame(bool win = true)
    {
        StopCoroutine(coroutineCheckSpots);
        StopCoroutine(coroutineMoving);

        Time.timeScale = 0;
        panelStart.SetActive(true);

        textGameEnd.GetComponent<TMPro.TextMeshProUGUI>().text = win ?  "You win! Again?" : "You lose :(";
        textButton.GetComponent<TMPro.TextMeshProUGUI>().text = win ? "Start" : "Restart";

        if (!win)
        {
            player.GetComponent<SpriteRenderer>().sprite = spriteFailed;
            player.transform.rotation = Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f));
        }
    }

    private void RestartGame()
    {
        foreach(var spot in listSpots)
        {
            Destroy(spot);
        }

        foreach (var line in listLines)
        {
            Destroy(line);
        }

        listSpots.Clear();
        listLines.Clear();

        coins = 0;
        startPosition = startPlayerPosition;
        player.transform.position = startPlayerPosition;

        player.GetComponent<SpriteRenderer>().sprite = spriteMain;
        countCoins.GetComponent<TMPro.TextMeshProUGUI>().text = "Coin count : " + coins;

        listCoins.ActivatingCoins();
        listSpikes.RandomRotateSpikes();

        coroutineCheckSpots = StartCoroutine(CheckSpots());
    }

    void Start()
    {
        Time.timeScale = 0;
        panelStart.SetActive(true);

        listSpots = new List<GameObject>();
        listLines = new List<GameObject>();

        startPosition = player.transform.position;
        startPlayerPosition = player.transform.position;

        spriteMain = player.GetComponent<SpriteRenderer>().sprite;
        coroutineCheckSpots = StartCoroutine(CheckSpots());
    }

    public void OnClickButton()
    {
        Time.timeScale = 1;
        panelStart.SetActive(false);

        if (!isFIrstStart) RestartGame();
        isFIrstStart = false;
    }
}
