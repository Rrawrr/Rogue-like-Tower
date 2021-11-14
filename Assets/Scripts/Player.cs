using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : AMovingObject
{
    public int wallDamage = 1;
    public int poitnsPerFood = 10;
    public int pointsPerSoda = 20;
    public float restrtLevelDelay = 1f;
    public Text foodText;

    private Animator animator;
    private int food;

    protected override void Start()
    {
        animator = GetComponent<Animator>();

        food = GameManager.instance.playerFoodPoints;
        foodText.text = $"Food: {food}";

        base.Start();
    }

    private void OnDisable()
    {
        GameManager.instance.playerFoodPoints = food;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.isPlayerTurn) return;

        int horizontal = 0;
        int vertical = 0;

        horizontal = (int) Input.GetAxisRaw("Horizontal");
        vertical = (int) Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
        {
            vertical = 0;
        }

        if (horizontal != 0 || vertical != 0)
        {
            AttemptMove<Wall>(horizontal, vertical);
        }
    }

    protected override void AttemptMove<T>(int xDir, int yDir)
    {
        food--;
        foodText.text = $"Food: {food}";

        base.AttemptMove<T>(xDir, yDir);
        RaycastHit2D hit;

        if (Move(xDir, yDir,out hit))
        {

        }

        CheckIfGameOver();
        GameManager.instance.isPlayerTurn = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restrtLevelDelay);
            enabled = false;
        }
        else if (other.tag == "Food")
        {
            food += poitnsPerFood;
            foodText.text = $"+{poitnsPerFood} Food: {food}";
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            food += pointsPerSoda;
            foodText.text = $"+{pointsPerSoda} Food: {food}";
            other.gameObject.SetActive(false);
        }
    }

    protected override void OnCantMove<T>(T component)
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage);
        animator.SetTrigger("playerChop");
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoseFood(int loss)
    {
        animator.SetTrigger("playerHit");
        food -= loss;
        foodText.text = $"-{loss} Food: {food}";
        CheckIfGameOver();
    }

    private void CheckIfGameOver()
    {
        if (food <= 0)
        {
            GameManager.instance.GameOver();
        }
    }
}
