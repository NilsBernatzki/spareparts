using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour {

    public bool changingMat;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Transparent(float alpha) {
        Material material = GetComponent<MeshRenderer>().materials[0];

        ChangeModeTransparent(material);

        Color currentColor = GetComponent<MeshRenderer>().material.color;
        Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);
        
        StartCoroutine(LerpColor(currentColor, newColor, material));
    }
    public void Untransparent() {
        Material material = GetComponent<MeshRenderer>().materials[0];

        Color currentColor = GetComponent<MeshRenderer>().material.color;
        Color newColor = new Color(currentColor.r, currentColor.g, currentColor.b, 1f);

        StartCoroutine(LerpColor(currentColor, newColor, material));

        ChangeModeOpaque(material);
    }
    private IEnumerator LerpColor(Color start, Color end, Material mat) {
        changingMat = true;
        float t = 0;
        while (t < 1) {
            t += Time.deltaTime * 3f;
            mat.SetColor("_Color", Color.Lerp(start, end, t));
            yield return new WaitForEndOfFrame();
        }
        changingMat = false;
    }
    public void ChangeModeTransparent(Material material) {
        material.SetFloat("_Mode", 2);
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = 3000;
    }
    public void ChangeModeOpaque(Material material) {
        material.SetFloat("_Mode", 0);
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        material.SetInt("_ZWrite", 1);
        material.DisableKeyword("_ALPHATEST_OFF");
        material.EnableKeyword("_ALPHABLEND_OFF");
        material.DisableKeyword("_ALPHAPREMULTIPLY_OFF");
        material.renderQueue = 3000;
    }
}
