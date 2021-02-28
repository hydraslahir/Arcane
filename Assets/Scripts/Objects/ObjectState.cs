using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectState : MonoBehaviour
{
    public enum State{
        MOVABLE,
        OBSERVABLE
    }
    [SerializeField] State state = State.MOVABLE;
    [SerializeField] public string OBJ_Reference;
    [SerializeField] public string OBJ_Name;
    // Start is called before the first frame update
    void Start()
    {
        if(state == State.MOVABLE){
            SetMovableState();
        }else{
            SetObservableState();
        }
    }

    public State GetState(){
        return state;
    }

    public void SetState(State s){
        if(s == State.MOVABLE){
            SetMovableState();
        }else{
            SetObservableState();
        }
    }

    public void SetMovableState(){
        this.GetComponent<MeshCollider>().enabled = false;
        this.GetComponent<Collider>().enabled = true;
        this.GetComponent<Rigidbody>().isKinematic = false;
    }

    public void SetObservableState(){
        this.GetComponent<MeshCollider>().enabled = true;
        this.GetComponent<Collider>().enabled = false;
        this.GetComponent<Rigidbody>().isKinematic = true;
    }

}
