using UnityEngine;
using UnityEngine.UI;

public class EnemiesContainer : MonoBehaviour
{
    public static EnemiesContainer Instance; // Singleton para acceso global

    [SerializeField] private Text enemiesAliveText;
    [SerializeField] private Text enemiesTotalText;

    private int enemiesAlive = 0;
    private int enemiesTotal = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddEnemy()
    {
        enemiesAlive++;
        enemiesTotal++;
        UpdateUI();
    }

    public void RemoveEnemy()
    {
        if (enemiesAlive > 0)
        {
            enemiesAlive--;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (enemiesAliveText != null)
            enemiesAliveText.text = "Enemigos Vivos: " + enemiesAlive;

        if (enemiesTotalText != null)
            enemiesTotalText.text = "Total de Enemigos: " + enemiesTotal;
    }
}

