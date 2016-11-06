using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TempCard
{
	public int ID;
	public string name;
	public string discription;
	public int HP;
	public int attack;
	public string target;
	public float range;
	public string speed;

	public TempCard()
	{
		ID = 0;
		name = "Tower Card";
		discription = "This is a brief discription";
		HP = 800;
		attack = 100;
		target = "Ground";
		range = 5;
		speed = "-";
	}

	public string GetIntro()
	{
		return "HP :\t\t\t" + HP + "\nAttack :\t\t" + attack + "\nTarget :\t" + target + "\nRange :\t" + range + "\nSpeed :\t" + speed;
	}
}

public class CardInfoPopupController : ViewController {

	public Text mName;
	public Text mIntro;
	public Text mDiscription;
	public Image mImage;
	public Button mUpgrade;
	public Button mUse;

	public void Init(TempCard card)
	{
		mName.text = card.name;
		mIntro.text = card.GetIntro();
		mDiscription.text = card.discription;
		mUpgrade.onClick.AddListener(()=>{
			Debug.Log(card.name + " Upgrade!");
		});
		mUse.onClick.AddListener(()=>{
			Debug.Log(card.name + " Use!");
		});
	}
}
