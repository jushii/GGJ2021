public class Kid : NPC
{
    public bool isRescued;
    public override void Start()
    {
        ServiceLocator.Current.Get<EntityManager>().RegisterNPC(this);

        spawnPosition = transform.position;

        aiManager = ServiceLocator.Current.Get<AIManager>();
        aiManager.SetupNPC(this);

        stunned = false;
    }
}