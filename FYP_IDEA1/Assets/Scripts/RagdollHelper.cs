using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
A helper component that enables blending from Mecanim animation to ragdolling and back

To use, do the following:

Add "GetUpFromBelly" and "GetUpFromBack" bool inputs to the Animator controller and corresponding transitions from any state to the get up animations
When the ragdoll mode is turned on, Mecanim stops where it was and it needs to transition to the get up state immediately when it is resumed
Therefore make sure that the blend times of the transitions to the get up animations are set to zero

TODO:
Make matching the ragdolled and animated root rotation and position more elegant
Now the transition works only if the ragdoll has stopped as the blending code will first wait for Mecanim to start playing the get up animation to obtain the animated hip position and rotation
Unfortunately Mecanim does not allow one to force an immediate transition in the same frame
Perhaps there could be an editor script that precomputes the needed information
*/

public class RagdollHelper : MonoBehaviour {
	// Public property that can be set to toggle between ragdolled and animated character
	public bool ragdolled
    {
		get
        {
			return state != RagdollState.animated;
		}
		set
        {
			if(value == true)
            {
				if(state == RagdollState.animated)
                {
					// Transition from animated to ragdolled
					setKinematic(false); //allow the ragdoll RigidBodies to react to the environment
					state = RagdollState.ragdolled;
				} 
			}
		} // Set
	} 

    // Possible states of the ragdoll
	enum RagdollState {
		animated, // Mecanim is fully in control
		ragdolled, // Mecanim turned off, physics controls the ragdoll
	}
	
	// Current state
	RagdollState state = RagdollState.animated;
	
	// How long to blend when transitioning from ragdolled to animated
	public float ragdollToMecanimBlendTime = 0.5f;
	float mecanimToGetUpTransitionTime = 0.05f;
	
	// A helper variable to store the time when transitioned from ragdolled to blendToAnim state
	float ragdollingEndTime =- 100;
	
	// Declare a class that will hold useful information for each body part
	public class BodyPart {
		public Transform transform;
		public Vector3 storedPosition;
		public Quaternion storedRotation;
	}

	// Additional vectors for storing the pose the ragdoll ended up in
	Vector3 ragdolledHipPosition, ragdolledHeadPosition, ragdolledFeetPosition;
	
	// Declare a list of body parts initialized in Start()
	List<BodyPart> bodyParts = new List<BodyPart>();
	
	// A helper function to set the isKinematc property of all RigidBodies in the children of the game object that this script is attached to
	void setKinematic(bool newValue) {
		// Get an array of components that are of type Rigidbody
		Component[] components=GetComponentsInChildren(typeof(Rigidbody));
		
		// For each of the components in the array, treat each component as a Rigidbody and set its isKinematic property
		foreach(Component component in components) {
			(component as Rigidbody).isKinematic = newValue;
		}
	}
	
	// Initialization on first frame of game
	void Start() {
		// Set all RigidBodies to be kinematic so that they can be controlled with Mecanim and there will be no glitches when transitioning to a ragdoll
		setKinematic(true);
		
		// Find all the transforms in the character, assuming that this script is attached to the root
		Component[] components = GetComponentsInChildren(typeof(Transform));
		
		// For each of the transforms, create a BodyPart instance and store the transform
		foreach(Component component in components) {
			BodyPart bodyPart = new BodyPart();
			bodyPart.transform = component as Transform;
			bodyParts.Add(bodyPart);
		}
	}
}