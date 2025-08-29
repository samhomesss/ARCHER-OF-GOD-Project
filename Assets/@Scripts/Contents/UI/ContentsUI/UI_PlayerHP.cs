using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerHP : UI_Scene
{
    enum GameObjects
    {
        PlayerHPSlider,
    }

    Slider _playerHpSlider;
    GameManager _gm;

    public override bool Init()
    {
        if (base.Init() == false) 
            return false;

        BindObjects(typeof(GameObjects));

        _playerHpSlider = GetObject((int)GameObjects.PlayerHPSlider).GetComponent<Slider>();
        Debug.Log(_playerHpSlider.name);
         
        return true;
    }

    private void Start()
    {
        _gm = GameManager.Instance;
        if (_gm != null)
        {
            var hp = _gm.PlayerHealth;
            if (hp != null)
            {
                _playerHpSlider.maxValue = hp.MaxHealth;
                _playerHpSlider.value = hp.CurrentHealth;
            }
            _gm.OnPlayerDamagedEvent += OnPlayerDamaged;
        }
    }

    private void OnDestroy()
    {
        if (_gm != null)
            _gm.OnPlayerDamagedEvent -= OnPlayerDamaged;
    }

    void OnPlayerDamaged(int damage)
    {
        if (_gm != null)
        {
            var hp = _gm.PlayerHealth;
            Debug.Log(hp + "플레이어 체력");
            if (hp != null)
                _playerHpSlider.value = hp.CurrentHealth;
        }
    }
}
