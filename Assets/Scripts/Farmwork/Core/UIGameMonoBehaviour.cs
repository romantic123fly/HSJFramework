using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameMonoBehaviour : GameMonoBehaviour
{
    //是否显示
    protected bool _isActive;

    protected override void initView()
    {
        base.initView();

        _isActive = true;
    }

    public override void render()
    {
        base.render();
    }

    public virtual bool isActive
    {
        get
        {
            return _isActive;
        }
        set
        {
            _isActive = value;

            gameObject.SetActive(value);

            if (value)
            {
                render();
            }
        }
    }
}
