using UnityEngine;
using UnityEngine.UI;

public class UI_EnemyHP : UI_Scene
{
    enum GameObjects
    {
        EnemyHPSlider,
    }

    Slider _enemyHpSlider;
    GameManager _gm;

    public override bool Init()
    {
        if (base.Init() == false) 
        {
            return false;
        }

        BindObjects(typeof(GameObjects));

        _enemyHpSlider = GetObject((int)GameObjects.EnemyHPSlider).GetComponent<Slider>();

        Debug.Log(_enemyHpSlider.name);


        return true;
    }

    private void Start()
    {
        _gm = GameManager.Instance;
        if (_gm != null)
        {
            var hp = _gm.EnemyHealth;
            if (hp != null)
            {
                _enemyHpSlider.maxValue = hp.MaxHealth;
                _enemyHpSlider.value = hp.CurrentHealth;
            }
            _gm.OnEnemyDamagedEvent += OnEnemyDamaged;
        }
    }

    private void OnDestroy()
    {
        if (_gm != null)
            _gm.OnEnemyDamagedEvent -= OnEnemyDamaged;
    }

    void OnEnemyDamaged(int damage)
    {
        if (_gm != null)
        {
            var hp = _gm.EnemyHealth;
            if (hp != null)
                _enemyHpSlider.value = hp.CurrentHealth;
        }
    }
}
