using UnityEngine;

public class Player : MonoBehaviour
{
    public float punchHitboxHorizontal = 1f;
    public float punchHitboxVertical = 5f;
    public float xOffset = 0.5f;

    private Collider2D[] punchedNPCs;
    private LayerMask mask;
    private Vector3 point;
    private Vector2 size;

    private void Start()
    {
        ServiceLocator.Current.Get<EntityManager>().RegisterPlayer(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Punch();
        }
    }

    private void Punch()
    {
        mask = LayerMask.GetMask("NPC");
        point = transform.position + transform.right * (punchHitboxHorizontal/2 + xOffset);
        size = new Vector2(punchHitboxHorizontal, punchHitboxVertical);
        punchedNPCs = Physics2D.OverlapCapsuleAll(point, size, CapsuleDirection2D.Horizontal, 0f, mask);
        foreach (Collider2D collider in punchedNPCs)
        {
            //Debug.Log("punch!");
            ServiceLocator.Current.Get<AIManager>().ChangeState(collider.GetComponent<NPC>(), typeof(Entities.NPC.States.Consumer_Fly));
        }
    }
}
