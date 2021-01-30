public class MallWorker : NPC
{
    public override void Start()
    {
        ServiceLocator.Current.Get<EntityManager>().RegisterNPC(this);

        spawnPosition = transform.position;

        aiManager = ServiceLocator.Current.Get<AIManager>();
        aiManager.SetupNPC(this);

        stunned = false;
    }

    public override void OnPunch()
    {
        aiManager.ChangeState(this, typeof(MallWorker_Fly));
    }
}