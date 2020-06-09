using UnityEngine;

public abstract class GameMonoBehaviour : MonoBehaviour
{
    /// <summary>
    /// 初始化
    /// </summary>
    protected virtual void Awake()
    {
        initModel();
        initView();
        initEvent();
    }

    protected virtual void initView()
    {

    }

    protected virtual void initEvent()
    {

    }

    protected virtual void initModel()
    {

    }

    //此处可以扩展全局自定义方法
    public virtual void render()
    {

    }

    /// <summary>
    /// 第一次Update前调用
    /// </summary>
    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
    }

    protected virtual void FixedUpdate()
    {

    }

    protected virtual void LateUpdate()
    {
    }

    public virtual void OnDisable()
    {
        Destroy(gameObject);
    }

    protected virtual void OnDestroy()
    {
    }
}
