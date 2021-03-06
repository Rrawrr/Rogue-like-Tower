using System.Collections;
using DG.Tweening;
using UnityEngine;

public abstract class AMovingObject : MonoBehaviour
{
    [HideInInspector] public bool isMoving;
    public float moveTime = 0.05f;
    public LayerMask blockingLayer;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rigidbody;
    //private float inverseMoveTime;


    protected bool canMove;

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        //inverseMoveTime = 1f / moveTime;
    }

    protected virtual void AttemptMove<T>(int xDir, int yDir) where T : Component
    {
        RaycastHit2D hit;
        canMove = Move(xDir, yDir, out hit);

        if (hit.transform == null) return;

        T hitComponent = hit.transform.GetComponent<T>();

        if (!canMove && hitComponent != null)
        {
            OnCantMove(hitComponent);
        }
    }

    protected bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDir, yDir);

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        if (hit.transform == null && !isMoving)
        {
            //StartCoroutine(SmoothMovement(end));
            SmoothMovement(end);
            return true;
        }

        return false;
    }

    protected abstract void OnCantMove<T>(T component) where T : Component;

    //protected IEnumerator SmoothMovement(Vector3 end)
    //{
    //    isMoving = true;
    //    float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

    //    while (sqrRemainingDistance > float.Epsilon)
    //    {
    //        Vector3 newPos = Vector3.MoveTowards(rigidbody.position,end,inverseMoveTime * Time.deltaTime);
    //        rigidbody.MovePosition(newPos);
    //        sqrRemainingDistance = (transform.position - end).sqrMagnitude;
    //        yield return null;
    //    }

    //    rigidbody.MovePosition(end);
    //    isMoving = false;
    //}

    private void SmoothMovement(Vector3 end)
    {
        transform.DOMove(end, moveTime, false).OnStart(OnStartMovementHandler).OnComplete(OnCompleteMovementHandler);
    }

    private void OnStartMovementHandler()
    {

    }

    private void OnCompleteMovementHandler()
    {

    }

}
