using UnityEngine;
using System.Collections;

public class BattleDeckController : MonoBehaviour {

	public GameObject mCardButtonPrefab;
	public GameObject mCardEmptyPrefab;
	private GameObject[] mMyCards;

	public GameObject mMyDeck;

	public RectTransform mTotalCardsTrans;
	public GameObject mDeckRowPrefab;

	private GameObject[] mDeckRows;
	private int mTotalCardNumber = 32;
	private GameObject[] mTotalCards;

	// Use this for initialization
	void Start () {
		int rowsCount = mTotalCardNumber / 5 + (mTotalCardNumber % 5 == 0 ? 0 : 1);
		int cardIndex = 0;
		mDeckRows = new GameObject[rowsCount];
		mTotalCards = new GameObject[rowsCount * 5];

		mTotalCardsTrans.sizeDelta = new Vector2(800, 175 * rowsCount);

		for(int i = 0; i<rowsCount; i++)
		{
			mDeckRows[i] = Instantiate<GameObject>(mDeckRowPrefab);
			mDeckRows[i].transform.SetParent(mTotalCardsTrans.transform, false);
			mDeckRows[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -175 * i);
			for(int j = 0;j<5;j++)
			{
				mTotalCards[cardIndex] = Instantiate<GameObject>(cardIndex < mTotalCardNumber ? mCardButtonPrefab : mCardEmptyPrefab);
				mTotalCards[cardIndex].transform.SetParent( mDeckRows[i].transform, false);
				cardIndex++;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
