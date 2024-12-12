using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MovementPoint : MonoBehaviour
{
  public Transform[] MovementPoints = new Transform[4];
  public GameObject obj;
  public Transform parentFolder;

  public void Update() {
    foreach(Transform t in MovementPoints) {
      if(t) {

        Debug.DrawRay(transform.position, -(transform.position-t.position), Color.red);
      }
    }
  }
  public void BuildPathUp() {
    Vector3 temp = new Vector3(-1,0,0);
    temp = transform.position+temp;
    MovementPoints[0] = Instantiate(obj, temp, Quaternion.identity, parentFolder).transform;
    MovementPoints[0].name = "Path";

    MovementPoints[0].GetComponent<MovementPoint>().MovementPoints[2] = transform;

    MovementPoints[0].GetComponent<MovementPoint>().MovementPoints[0] = null;
    MovementPoints[0].GetComponent<MovementPoint>().MovementPoints[1] = null;
    MovementPoints[0].GetComponent<MovementPoint>().MovementPoints[3] = null;

  }
  public void BuildPathRight() {
    Vector3 temp = new Vector3(0,0,1);
    temp = transform.position+temp;
    MovementPoints[1] = Instantiate(obj, temp, Quaternion.identity, parentFolder).transform;
    MovementPoints[1].name = "Path";

    MovementPoints[1].GetComponent<MovementPoint>().MovementPoints[3] = transform;

    MovementPoints[1].GetComponent<MovementPoint>().MovementPoints[0] = null;
    MovementPoints[1].GetComponent<MovementPoint>().MovementPoints[1] = null;
    MovementPoints[1].GetComponent<MovementPoint>().MovementPoints[2] = null;
      /*GameObject RightMovementPoint = Instantiate(obj, temp, Quaternion.identity);
      RightMovementPoint.gameObject.name = "Right";
      RightMovementPoint.transform.parent = gameObject.transform;*/
    }
    public void BuildPathDown() {
      Vector3 temp = new Vector3(1,0,0);
      temp = transform.position+temp;
      MovementPoints[2] = Instantiate(obj, temp, Quaternion.identity, parentFolder).transform;
      MovementPoints[2].name = "Path";

      MovementPoints[2].GetComponent<MovementPoint>().MovementPoints[0] = transform;

      MovementPoints[2].GetComponent<MovementPoint>().MovementPoints[1] = null;
      MovementPoints[2].GetComponent<MovementPoint>().MovementPoints[2] = null;
      MovementPoints[2].GetComponent<MovementPoint>().MovementPoints[3] = null;
    }
    public void BuildPathLeft() {
      Vector3 temp = new Vector3(0,0,-1);
      temp = transform.position+temp;
      MovementPoints[3] = Instantiate(obj, temp, Quaternion.identity, parentFolder).transform;
      MovementPoints[3].name = "Path";

      MovementPoints[3].GetComponent<MovementPoint>().MovementPoints[1] = transform;

      MovementPoints[3].GetComponent<MovementPoint>().MovementPoints[0] = null;
      MovementPoints[3].GetComponent<MovementPoint>().MovementPoints[2] = null;
      MovementPoints[3].GetComponent<MovementPoint>().MovementPoints[3] = null;
    }

    
    public Transform GetUp() {
      return MovementPoints[0];
    }
    public Transform GetRight() {
      return MovementPoints[1];
    }
    public Transform GetDown() {
      return MovementPoints[2];
    }
    public Transform GetLeft() {
      return MovementPoints[3];
    }

    public Transform GetPoint(int movementPoint) {
      return MovementPoints[movementPoint];
    }
}
