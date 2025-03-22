using UnityEngine;
using System.Collections;

public class Dice : MonoBehaviour
{
    private Sprite[] diceSides;
    private SpriteRenderer rend; // get all the dice sides using the spirte renderer component
    private int whosTurn = 1; // whos tunturn it is 
    private bool coroutineAllowed = true; // to check if the coroutine is allowed to run or not

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");
        rend.sprite = diceSides[5];
    }
    private void OnMouseDown()
    {
        if (!GameControl.gameOver && coroutineAllowed)
            StartCoroutine("RollTheDice");
    }

    public void diceOnMouseDown(){ //using a button to roll the dice so would need to call this function
        OnMouseDown();
    }

    private IEnumerator RollTheDice()
    {
        coroutineAllowed = false;
        int randomDiceSide = 0;
        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = Random.Range(0, 6);
            rend.sprite = diceSides[randomDiceSide];
            yield return new WaitForSeconds(0.05f);
        }

        GameControl.diceSideThrown = randomDiceSide + 1;
        GameControl.diceSideThrown = 6;
        coroutineAllowed = true;
    }
}
