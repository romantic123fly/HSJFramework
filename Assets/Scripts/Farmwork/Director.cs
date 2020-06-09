public class Director : Singleton<Director>
{
    protected override void Awake()
    {
        base.Awake();

        initManager();
    }

    private void initManager()
    {
        //NetworkManager.instance.setup();
        TimeManager.instance.setup();
        //LevelManager.instance.setup();
        PoolManager.instance.setup();
        //InputManager.instance.setup();
        //ItemManager.instance.setup();
        //SkillManager.instance.setup();
        //WindowManager.instance.setup();
        //GlobalManager.instance.setup();
        //LangManager.instance.setup();
        //PlayerManager.instance.setup();
        //QuestManager.instance.setup();
    }


    protected override void Update()
    {
        base.Update();

        //NetworkManager.instance.update();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        //NetworkManager.instance.disConnect();
    }
}
