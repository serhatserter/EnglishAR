using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MaterialSpawn : MonoBehaviour {
	public GameObject[] nesne, secilen;
	public List<GameObject> tiklanan;

	public int nesneSayisi = 5;
	bool bitir = false;
	bool kontrol = false;
	public string selected = "";
	public int length;
	public int seCount;
	int tikCount;
	float eksen;
	public float sure = 60.0f;
	int score;

	public Button yenileButton;
	public Button onaylaButton;
	public Button baslatButton;
	public Button yeniButton;

	public GameObject image;
	public GameObject timeOb;
	public GameObject scoreOb;

	public float newXaxis;
	public float newZaxis;

	

	void Start () {

		
		GetComponent<SelectItem>().secList.Clear();
		tiklanan = GetComponent<SelectItem>().secList;

		secilen = new GameObject[nesneSayisi];
		seCount = 0;
		
	
	}
	private void Update()
	{
		if (sure > 0 && image.active == false)
		{
			
			sure -= Time.deltaTime;
			timeOb.GetComponent<Text>().text = Mathf.Round(sure) + "";
			if (sure < 10) timeOb.GetComponent<Text>().DOColor(Color.red, 0.5f);
			if (sure <= 0) image.active = true;
		}
		else if(sure <= 0 && image.active == true)
		{
			timeOb.GetComponent<Text>().text = "-";
			GameObject.Find("Title").GetComponent<Text>().text = "Your Score: " + score;
			baslatButton.gameObject.active = false;
			yeniButton.gameObject.active = true;
			image.transform.DOMoveY(400f, 1.5f).OnComplete(GirisAc);
		}
		
	}


	public void Giris()
	{
		
		eksen = 5f;
		Uret();
		image.transform.FindChild("Button").GetComponent<Button>().interactable = false;
		image.transform.DOMoveY(1300f, 1.5f).SetEase(Ease.InOutCubic).OnComplete(GirisKaldir);
		
		


	}
	void GirisKaldir()
	{
		image.active = false;
	}

	void GirisAc(){
		GameObject.Find("Try").GetComponent<Button>().interactable = true;

	}
	public void Yeniden()
	{
		Application.LoadLevel(Application.loadedLevel);
	}

	void Uret()
	{
		
		for (int i = 0; i <= nesneSayisi - 1; i++)
		{


			length = nesne.Length;

			GameObject x = (GameObject)Instantiate(nesne[Random.Range(0, length)]);
			x.name = x.name + i;

			x.transform.parent = this.gameObject.transform;
			secilen[i] = x;


			Vector3 scale = new Vector3(0.1f, 0.1f, 0.1f);

			newXaxis = Random.Range(-eksen, eksen);
			newZaxis = Random.Range(-eksen, eksen);

			x.transform.position = new Vector3(newXaxis, 0, newZaxis);
			x.transform.localScale = new Vector3(0f, 0f, 0f);
			x.transform.DOScale(scale, 0.5f);
			Carpisma(x);

		}

		int sayi = Random.Range(0, nesneSayisi);
		selected = secilen[sayi].name;
		string[] sArray = selected.Split('(');
		GameObject.Find("Selected").GetComponent<Text>().DOText("Select " + sArray[0] + "!", 1f);
		GameObject.Find("Selected").transform.DOShakeScale(0.5f);

		Say(sArray[0]);
		eksen = 50f;



	}
	void Carpisma(GameObject x)
	{
		ExplosionDamage(x.transform.position, 1f);
		if (hitColliders.Length >= 2)
		{
			newXaxis = Random.Range(-eksen, eksen);
			newZaxis = Random.Range(-eksen, eksen);
			x.transform.position = new Vector3(newXaxis, 0, newZaxis);
			Carpisma(x);
	
		}
	}

	Collider[] hitColliders;

	void ExplosionDamage(Vector3 center, float radius)
	{
		hitColliders = Physics.OverlapSphere(center, radius);
		int i = 10;
		while (i < hitColliders.Length)
		{
			hitColliders[i].SendMessage("AddDamage");
			i++;
		}
	}


	void Say(string isim)
	{
		seCount = 0;
		foreach (Transform child in transform)
		{
			
			string[] splited = child.name.Split('(');

		if (splited[0] == isim)
		{
			seCount = seCount + 1;
		}
			yenileButton.interactable = true;
			onaylaButton.interactable = true;
		}
	}

	public void Karsilastir()
	{
		kontrol = false;
		onaylaButton.interactable = false;
		yenileButton.interactable = false;

		tiklanan = GetComponent<SelectItem>().secList;

		tikCount = 0;
		string[] sArray = selected.Split('(');

		for (int i=0; i<=tiklanan.Count-1; i++)
		{
			string[] tArray = tiklanan[i].name.Split('(');
			
			if ( tArray[0]== sArray[0])
			{

				tikCount++;
			}
			

		}
		if (seCount == tikCount && tikCount==tiklanan.Count)
		{
			GameObject.Find("Selected").GetComponent<Text>().text = "Perfect!!";

			GameObject green = GameObject.Find("GreenPoint");
			green.GetComponent<Image>().DOFade(0.5f, 0.2f).SetLoops(4,LoopType.Yoyo);

			score++;
			scoreOb.GetComponent<Text>().text = "Score: " + score;


		}
		else
		{
			GameObject.Find("Selected").GetComponent<Text>().text = "Wrong!!";

			GameObject red = GameObject.Find("RedPoint");
			red.GetComponent<Image>().DOFade(0.5f, 0.2f).SetLoops(4, LoopType.Yoyo);
		}

		StartCoroutine(YarimSaniye());



	}

	public void Sil()
	{
		GetComponent<SelectItem>().secList.Clear();
		tiklanan.Clear();
		bitir = false;
		onaylaButton.interactable = false;
		yenileButton.interactable = false;

		foreach (Transform child in transform)
		{
			Vector3 scale = new Vector3(0f, 0f, 0f);
			child.transform.DOScale(scale, 0.5f).OnComplete(SilAnim);
		}
		StartCoroutine(Bekle());

	}
	void SilAnim()
	{
		int childs = transform.childCount;
		for (int i = childs - 1; i >= 0; i--)
		{
			GameObject.Destroy(transform.GetChild(i).gameObject);
		}
	}


	IEnumerator Bekle()
	{
		bitir = true;
		yield return new WaitForSeconds(1);
		if (bitir == true)
		{
			Uret();
		}


	}

	IEnumerator YarimSaniye()
	{
		kontrol = true;
		yield return new WaitForSeconds(1);
		if (kontrol == true) {
			GetComponent<SelectItem>().secList.Clear();
			tiklanan.Clear();
			Sil();
		}



	}



}

