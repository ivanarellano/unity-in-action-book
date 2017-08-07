using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {

    public const int gridRows = 2;
    public const int gridCols = 4;
    public const float offsetX = 2f;
    public const float offsetY = 2.5f;

    [SerializeField] private MemoryCard originalCard;
    [SerializeField] private Sprite[] images;
    [SerializeField] private TextMesh scoreLabel;

    private MemoryCard firstRevealed;
    private MemoryCard secondRevealed;
    private int score;

    void Start () {
        Vector3 startPos = originalCard.transform.position;

        int[] numbers = { 0, 0, 1, 1, 2, 2, 3, 3 };
        numbers = ShuffleArray(numbers);

        for (int i = 0; i < gridCols; i++) {
            for (int j = 0; j < gridRows; j++) {
                MemoryCard card;
                if (i == 0 && j == 0) {
                    card = originalCard;
                } else {
                    card = Instantiate(originalCard) as MemoryCard;
                }

                int index = j * gridCols + i; /// y * width + x
                int id = numbers[index];
                card.SetCard(id, images[id]);

                float posX = (offsetX * i) + startPos.x;
                float posY = -(offsetY * j) + startPos.y;
                card.transform.position = new Vector3(posX, posY, startPos.z);
            } 
        }
	}

    // Knuth shuffle algorithm
    private int[] ShuffleArray(int[] numbers) {
        int[] newArr = numbers.Clone() as int[];
        for (int i = 0; i < newArr.Length; i++) {
            int tmp = newArr[i];
            int r = Random.Range(i, newArr.Length);
            newArr[i] = newArr[r];
            newArr[r] = tmp;
        }
        return newArr;
    }
	
    // Returns true when second hasn't been revealed
    public bool canReveal {
        get { return secondRevealed == null; }
    }

    public void CardRevealed(MemoryCard card) {
        if (firstRevealed == null) {
            firstRevealed = card;
        } else {
            secondRevealed = card;
            StartCoroutine(CheckMatch());
        }
    }

    public IEnumerator CheckMatch() {
        if (firstRevealed.id == secondRevealed.id) {
            score++;
            scoreLabel.text = "Score: " + score;
        } else {
            yield return new WaitForSeconds(.5f);
            firstRevealed.Unreveal();
            secondRevealed.Unreveal();
        }

        firstRevealed = null;
        secondRevealed = null;
    }

    public void Restart() {
        SceneManager.LoadScene("Scene1");
    }
}
