using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class PageTurner : MonoBehaviour {

	public BookText bookText;

	Vector3 startPos;

	const float MIN_SWIPE_MAGNITUDE = 0.1f;
	Vector3 turnDirection = Vector3.right;

	bool hasTurnedPage = false;
	bool isTurningPage = false;

	void Update () {
		if (isTurningPage) {

		}

		isTurningPage = false;
	}

	void HandHoverUpdate ( Hand hand ) {
		// this applies to either Vive controller, "Hand" can be abstracted as anything
		// on Vive controller, by default "StandardInteractionButton" is the trigger
		// lastly, we check AttachedObjects.Count to make sure you pick up only 1 thing at a time
		// if ( hand.GetStandardInteractionButton() == true && hand.AttachedObjects.Count == 0 ) { 
		// 	hand.AttachObject( gameObject );
		// }
		turnDirection = transform.right;
		if (hand.AttachedObjects.Count == 0) {
			if (!isTurningPage) {
				startPos = hand.transform.position;
				hasTurnedPage = false;
			}

			if (hand.GetStandardInteractionButton()) {
				isTurningPage = true;
				if (!hasTurnedPage) {
					if (isTurningPage(startPos, hand.transform.position)) {
						if (IsTurningForward(startPos, hand.transform.position)) {
							bookText.TurnPageForward();
						}
						else {
							bookText.TurnPageBack();
						}
					}
				}
			}
		} 
	}

	// void OnCollisionEnter (Collision c) {
	// 	if (c.gameObject.tag == "Hand") {
	// 		if (c.gameObject.name == "Hand1") {
	// 			hand1StartPos = c.gameObject.transform.localPosition;
	// 		}
	// 		else {
	// 			hand2StartPos = c.gameObject.transform.localPosition;
	// 		}
	// 	}
	// }

	// void OnCollisionStay (Collision c) {
	// 	if (!hasTurnedPage) {
	// 		if (c.gameObject.tag == "Hand") {
	// 			if (c.gameObject.name == "Hand1") {
	// 				if (IsTurningPage(hand1StartPos, c.gameObject.transform.localPosition)) {
	// 					if (IsTurningForward(hand1StartPos, c.gameObject.transform.localPosition)) {
	// 						bookText.TurnPageForward();
	// 					}
	// 					else {
	// 						bookText.TurnPageBack();
	// 					}
	// 				}
	// 			}
	// 			else {
	// 				if (IsTurningPage(hand2StartPos, c.gameObject.transform.localPosition)) {
	// 					if (IsTurningForward(hand2StartPos, c.gameObject.transform.localPosition)) {
	// 						bookText.TurnPageForward();
	// 					}
	// 					else {
	// 						bookText.TurnPageBack();
	// 					}
	// 				}
	// 			}
	// 		}
	// 	}
	// }

	// void OnCollisionExit (Collision c) {
	// 	if (c.gameObject.tag == "Hand") hasTurnedPage = false;
	// }

	bool IsTurningPage (Vector3 startPos, Vector3 currentPos) {
		Vector3 axisAlignedDirection = MultiplyVector3((currentPos - startPos), turnDirection);
		return axisAlignedDirection.magnitude >= MIN_SWIPE_MAGNITUDE;
		hasTurnedPage = true;
	}

	bool IsTurningForward (Vector3 startPos, Vector3 currentPos) {
		Vector3 axisAlignedDirection = MultiplyVector3((currentPos - startPos), turnDirection);
		return (Vector3.Dot(axisAlignedDirection, turnDirection) > 0);
	}

	Vector3 MultiplyVector3 (Vector3 v1, Vector3 v2, bool abs = false) {
		if (abs) {
			v1.x *= Mathf.Abs(v2.x);
			v1.y *= Mathf.Abs(v2.y);
			v1.z *= Mathf.Abs(v2.z);
		}
		else {
			v1.x *= v2.x;
			v1.y *= v2.y;
			v1.z *= v2.z;
		}

		return v1;
	}
}
