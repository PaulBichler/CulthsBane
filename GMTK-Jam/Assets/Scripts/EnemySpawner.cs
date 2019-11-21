using System.Collections.Generic;
using Models;
using TMPro;
using UIScripts;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    private List<Transform> enemySlots = new List<Transform>();
    private void Start()
    {
        foreach (Transform child in transform) enemySlots.Add(child);
    }


    public Slider enemyHealthBarPrefab;
    public TextMeshProUGUI shieldPrefab;
    

    public EnemyBehavior SpawnEnemy(Enemy enemyModel, int i)
    {
        foreach (Transform child in transform) enemySlots.Add(child);
        
        if (enemySlots[i].transform.childCount == 0)
        {
            var sprite = Resources.Load(enemyModel.Name, typeof(Sprite)) as Sprite;
            var enemy = Instantiate(enemyPrefab, enemySlots[i].position, enemySlots[i].rotation, enemySlots[i]);
            
            var image = enemy.AddComponent<Image>();
            image.sprite = Resources.Load(enemyModel.Name, typeof(Sprite)) as Sprite;
            image.SetNativeSize();
            image.GetComponent<RectTransform>().localScale = new Vector3(0.0125f, 0.0125f, 0.0125f);

            image.GetComponent<BoxCollider2D>().size = image.GetComponent<RectTransform>().sizeDelta;
            image.GetComponent<BoxCollider2D>().offset = Vector2.zero;
            
            
            enemy.name = enemyModel.Name;
            enemy.GetComponent<EnemyMouseEvents>().enemyIndex = i;
            var behavior = enemy.GetComponent<EnemyBehavior>();
            behavior.Init(enemyModel);


            var parent = enemy.transform.parent;
            var enemyHealthSlider = Instantiate(enemyHealthBarPrefab, parent);
            enemyHealthSlider.minValue = 0;
            enemyHealthSlider.maxValue = behavior.enemyData.Health;
            enemyHealthSlider.value = behavior.enemyData.Health;


            var sliderRectTransform = enemyHealthSlider.GetComponent<RectTransform>();
            sliderRectTransform.sizeDelta = new Vector2(150, 20);
            sliderRectTransform.localScale = new Vector3(0.0125f, 0.0125f, 0.0125f);
            sliderRectTransform.anchoredPosition3D = new Vector3(0, 8, 0);

            var text = sliderRectTransform.Find("Text").GetComponent<TextMeshProUGUI>();
            var startHealth = behavior.enemyData.Health;
            text.text = startHealth + " / " + behavior.enemyData.Health;

   
            var enemyShieldSlider = Instantiate(shieldPrefab, parent);
            enemyShieldSlider.GetComponent<RectTransform>().localScale = new Vector3(0.0125f, 0.0125f, 0.0125f);
            enemyShieldSlider.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0,6,0);
            enemyShieldSlider.text = "0";
            
            behavior.shieldbar = enemyShieldSlider;
            behavior.healthbar = enemyHealthSlider;
            
            
            return behavior;
        }
        return null;
    }
}