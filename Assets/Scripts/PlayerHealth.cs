using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {
	
	public Slider energySlider;
	public GameObject Fill;
	public Sprite EmptyHeart;
	public Image[] HeartsArray;
	public float refillSpeed;

	private int startingEnergy = 100;
	private bool isDead = false;
	private int currentLife;
	private Color startColor;

	// Use this for initialization
	void Start () {
		energySlider.value = startingEnergy;
		currentLife = HeartsArray.Length;
		startColor = Fill.GetComponent<Image> ().color;
	}
	
	// Update is called once per frame
	void Update () {
		if (currentLife>0) {

			//Indicate to the user that they do not have enough energy to be granted a speed boost
			if(energySlider.value < startingEnergy/4)
				Fill.GetComponent<Image>().color = Color.yellow;
			else
				Fill.GetComponent<Image>().color = startColor;

			if (energySlider.value < startingEnergy) {
				energySlider.value += 5 * Time.deltaTime;
			}
		}
	}

	public void HitByEnemyBall(){
		HeartsArray [currentLife - 1].sprite = EmptyHeart;
		currentLife--;
	}

	public bool UseSpeedBoost(){
		if (energySlider.value >= 25) {
			energySlider.value-=25;
			return true;
		}
		return false;
	}

}
