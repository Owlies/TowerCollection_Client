using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class TowerButton : MonoBehaviour, IPointerDownHandler {

	Button m_button;
	Text m_name;
	public Image m_halo;
	Image m_cost;

	LevelTower m_levelTower = null;
	SkillInfo m_towerSkill = null;
	GameObject m_towerPrefab = null;
	GameObject m_tower = null;
	GameObject m_skill = null;

	Color m_validColor = new Color(0,0,1,0.7f);		//blue
	Color m_inValidColor = new Color(1,0,0,0.7f);	//red

	float m_skillCooldown = 0;

	public Material m_prefabMaterial = null;

	public bool m_enabled = false;
	public bool m_moving = false;
	public bool m_mouse = false;
	public bool m_isSkill = false;
	public float m_curSkillTime = 0;

	public void Init(LevelTower levelTower)
	{
		m_levelTower = levelTower;
		m_name = GetComponentInChildren<Text>();
		m_button = GetComponentInChildren<Button>();
		m_cost = transform.GetChild(0).GetComponent<Image>();
		m_towerSkill = GameInfoManager.Instance.GetSkillInfo(levelTower.activeSkill);

		SetUIDisplay();
	}

	// Use this for initialization
	void Start () {

	}

	public void OnPointerDown (PointerEventData eventData) 
	{
		if(m_enabled == false)
			return;
		
		if(!m_isSkill && m_levelTower != null && !m_moving)
		{
			TowerInfo towerInfo = GameInfoManager.Instance.GetTowerInfo(m_levelTower.towerID);
			m_towerPrefab = GameObject.Instantiate(GameManager.Instance.GetResourceObject(GameConstant.MODEL_PATH + towerInfo.prefabName));

			// Disable collider to avoid raycast itself
			m_towerPrefab.GetComponentInChildren<CapsuleCollider>().enabled = false;

			// Apply my material inorder to change color
			MeshRenderer[] renders = m_towerPrefab.GetComponentsInChildren<MeshRenderer>();
			for(int i = 0;i<renders.Length;i++)
				renders[i].sharedMaterial = m_prefabMaterial;
			
			m_moving = true;
		}

		if(m_isSkill && !m_moving)
		{
			m_skill = GameObject.Instantiate(GameManager.Instance.GetResourceObject("Model/Prefabs/SkillCircle"));
			m_moving = true;
		}
	}

	// Update is called once per frame
	void Update () {

		if(m_isSkill)
		{
			SetCoolDown();
			ReleaseSkill();
			if(m_tower == null)
			{
				FlipUI();
				if(m_skill !=null)
					Destroy(m_skill);
			}
		}
		else
		{
			SetCost();
			SpawnTower();
		}
	}

	// Set the button's fill. Will change to spin later...
	private void SetCost()
	{

		float fill = 0;
		if(SceneManager.Instance.ElementCrystalNum >= m_towerSkill.skill.cost)
		{
			fill = 1;
			m_enabled = true;
		}
		else
		{
			fill = (float)SceneManager.Instance.ElementCrystalNum/m_towerSkill.skill.cost;
			m_enabled = false;
		}

		m_halo.gameObject.SetActive(m_enabled);
		m_halo.color = m_validColor;
		m_button.interactable = m_enabled;

		fill = (fill + m_cost.fillAmount)/2;
		m_cost.fillAmount = fill;

	}

	// Set button 
	private void SetCoolDown()
	{

		float fill = 0;
		m_skillCooldown += Time.deltaTime * 3;
		if(m_skillCooldown >= m_towerSkill.skill.coolDownTime)
		{
			fill = 1;
			m_skillCooldown = m_towerSkill.skill.coolDownTime;
			m_enabled = SceneManager.Instance.ElementCrystalNum >= m_towerSkill.skill.cost;
			m_halo.gameObject.SetActive(true);
			m_halo.color = m_enabled ? m_validColor : m_inValidColor;
		}
		else
		{
			fill = m_skillCooldown/m_towerSkill.skill.coolDownTime;
			m_enabled = false;
			m_halo.gameObject.SetActive(m_enabled);
		}

		m_button.interactable = m_enabled;
		fill = (fill + m_cost.fillAmount)/2;
		m_cost.fillAmount = fill;
	}

	// Logic for release skill
	private void ReleaseSkill()
	{

		m_mouse = Input.GetMouseButton(0);
		if(m_skill!=null)
		{
			if(m_moving && Input.GetMouseButton(0))
			{
				Vector3 mousePos = Input.mousePosition;

				Ray ray = Camera.main.ScreenPointToRay(mousePos);
				RaycastHit raycastInfo;
				if (Physics.Raycast(ray, out raycastInfo, GameConstant.MAX_DISTANCE, 1 << LayerMask.NameToLayer("Default")))
				{
					if(raycastInfo.collider.name == "Terrain")
					{
						m_skill.transform.position = new Vector3(raycastInfo.point.x, 0, raycastInfo.point.z);
					}
				}
			}

			if(!Input.GetMouseButton(0))
			{
				m_moving = false;
				m_skillCooldown = 0;
				// TODO :: play skill effects
				ArrayList skillInfo = new ArrayList();
				skillInfo.Add(m_skill.transform.position);
				skillInfo.Add(m_levelTower);
                skillInfo.Add(m_tower);
				EventManager.Instance.SendEventWithList(EVT_TYPE.EVT_TYPE_RELEASE_SKILL, skillInfo);
				EventManager.Instance.SendEvent(EVT_TYPE.EVT_TYPE_CHANGE_EC, -m_towerSkill.skill.cost);
				Destroy(m_skill);
			}
		}
	}

	// Logic for drag to spawn tower
	private void SpawnTower()
	{

		m_mouse = Input.GetMouseButton(0);
		if(m_towerPrefab!=null)
		{
			if(m_moving && Input.GetMouseButton(0))
			{
				Vector3 mousePos = Input.mousePosition;

				// Apply invalid color, will apply valid color if it's valid position
				m_prefabMaterial.color = m_inValidColor ;

				Ray ray = Camera.main.ScreenPointToRay(mousePos);
				RaycastHit raycastInfo;
				int layer = (1 << LayerMask.NameToLayer("Tower")) | (1 << LayerMask.NameToLayer("Default"));
				if (Physics.Raycast(ray, out raycastInfo, GameConstant.MAX_DISTANCE, layer ))
				{
					if(raycastInfo.collider.name == "Terrain")
					{
						m_towerPrefab.transform.position = new Vector3(raycastInfo.point.x, 0, raycastInfo.point.z);
						if(ValidRange(m_towerPrefab.transform.position))
						{
							// Where to adjust tower material
							m_prefabMaterial.color = m_validColor;
						}
					}
				}
			}
		}

		if(!Input.GetMouseButton(0))
		{
			// if released at correct place, create tower
			m_moving = false;
			if(m_towerPrefab != null && ValidRange(m_towerPrefab.transform.position))
			{
				m_levelTower.pos = m_towerPrefab.transform.position;
				m_tower = SceneManager.Instance.CreateTower(m_levelTower);
				EventManager.Instance.SendEvent(EVT_TYPE.EVT_TYPE_CHANGE_EC, -m_towerSkill.skill.cost);
				Destroy(m_towerPrefab);
				FlipUI();
			}
			else
			{
				Destroy(m_towerPrefab);
			}
		}
	}

	// make sure it's not too near, or too left or right
	bool ValidRange(Vector3 position)
	{
		return position.z > 50 && position.x > 100 && position.x < 220;
	}

	public void SetTower(LevelTower levelTower)
	{
		m_levelTower = levelTower;
	}

	// Called when tower is spawned or distroyed
	public void FlipUI()
	{
		m_isSkill = !m_isSkill;
		SetUIDisplay();
	}

	private void SetUIDisplay()
	{
		if(m_levelTower == null)
		{
			Debug.LogError("No level tower set!");
			return;
		}
		if(m_isSkill)
		{
			m_name.text = "Skill " + m_levelTower.towerID;
		}
		else
		{
			m_name.text = "Tower " + m_levelTower.towerID;
		}
	}

}
