using UnityEngine;

public class CameraScript : MonoBehaviour
{
	public GameObject BoundaryObject;
	public GameObject Target;
	public Vector3 Offset;

	private float rightBound;
	private float leftBound;
	private float topBound;
	private float bottomBound;

	// Use this for initialization
	void Start()
	{
		var boundarySprite = BoundaryObject.GetComponent<SpriteRenderer>();

		float vertExtent = gameObject.GetComponent<Camera>().orthographicSize;
		float horzExtent = vertExtent * Screen.width / Screen.height;
		leftBound = (horzExtent - (BoundaryObject.transform.localScale.x * boundarySprite.sprite.bounds.size.x) / 2.0f);
		rightBound = ((BoundaryObject.transform.localScale.x * boundarySprite.sprite.bounds.size.x) / 2.0f - horzExtent);
		bottomBound = (vertExtent - BoundaryObject.transform.localScale.y * boundarySprite.sprite.bounds.size.y / 2.0f);
		topBound = (BoundaryObject.transform.localScale.y * boundarySprite.sprite.bounds.size.y / 2.0f - vertExtent);
	}

	// Update is called once per frame
	void Update()
	{
		var pos = new Vector3(Target.transform.position.x + Offset.x, Target.transform.position.y + Offset.y, Target.transform.position.z + Offset.z);
		pos.x = Mathf.Clamp(pos.x, leftBound, rightBound);
		pos.y = Mathf.Clamp(pos.y, bottomBound, topBound);
		transform.position = pos;
	}
}
