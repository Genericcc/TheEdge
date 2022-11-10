using UnityEngine;
using System.Collections;

public class ScrollingUVs_Layers : MonoBehaviour 
{
	//public int materialIndex = 0;
	[SerializeField] private Vector2 uvAnimationRate = new Vector2( 0.1f, 0.0f );
	[SerializeField] private string textureName = "_MainTex";
	
	Vector2 uvOffset = Vector2.zero;
	
	void LateUpdate() 
	{
		uvOffset += ( uvAnimationRate * 0.01f * Time.deltaTime );
		if( GetComponent<Renderer>().enabled )
		{
			GetComponent<Renderer>().sharedMaterial.SetTextureOffset( textureName, uvOffset );
		}
	}
}