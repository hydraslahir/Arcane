using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SelectorNS{
    public class Selector : MonoBehaviour
    {
        [SerializeField] bool clipToGround = true;
        [SerializeField] Color_Picker colorPicker;
        string buttonType= "OBJECT";

        private GameObject selectedObject;
        public GameObject GetSelectedObject(){ 
            return selectedObject;
        }

        public void ChangeSelectionMode(string buttonName){
            this.buttonType = buttonName;
            if(selectedObject){
                //peu importe l'état dans lequel il était, garantit le comportement par defaut
                selectedObject.GetComponent<ObjectState>().SetState(ObjectState.State.MOVABLE);
            }
            Debug.Log("name :: " + name);
        }

        void Update()
        {
            switch(buttonType){
                case "OBJECT": {
                    moveObjetBehaviour();
                    break; 
                }
                case "FACE"  :
                    ChangeObjectColorOnClick();
                break;
            }      
        }

/**TODO #1
* La position de l'objet recule continuellement. //UPDATE; ça dépend de la position de la sourie et de l'orientation de la caméra
* L'objet sélectionné change lorsqu'un autre passe devant
* Offset de la souris n'est pas respecté
*/

    public void moveObjetBehaviour(){
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(!Input.GetMouseButton(0)) return;
        //Verifie si l'objet est derière un autre
        //si c'est le cas, conserve cet objet
        foreach(RaycastHit rhit in Physics.RaycastAll(ray)){
            if(selectedObject == rhit.transform.gameObject){
                var distanceCameraObject = Camera.main.WorldToScreenPoint(selectedObject.transform.position).z;
                var mouseWorldPosition = 
                    Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,distanceCameraObject)); 
                mouseWorldPosition.y = clipToGround? Mathf.Max(mouseWorldPosition.y,0) : mouseWorldPosition.y;
                selectedObject.transform.position = mouseWorldPosition;
                return;
            }
        }
        //Si hit n'est toujours pas initialisé
        if(Physics.Raycast(ray, out hit)){
            selectedObject = hit.transform.gameObject;
            var distanceCameraObject = Camera.main.WorldToScreenPoint(selectedObject.transform.position).z;
            var mouseWorldPosition = 
                Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,distanceCameraObject)); 
            mouseWorldPosition.y = clipToGround? Mathf.Max(mouseWorldPosition.y,0) : mouseWorldPosition.y;
            selectedObject.transform.position = mouseWorldPosition;         
        }
    }   
        
/** 
* Désactiver le Collider permet d'accéder au MeshCollider(nécessaire pour hit.trianglePosition)
* Un RigidBody sans Collider pose problème au niveua de la gravité
* Activer Rigidbody.isKinematic permet d'accéder à la modification des faces de l'objet & désactiver la gravité
* Puisqu'en mode FACE, le déplacement d'objet n'est pas activé, désactiver les mouvements ne pose pas de problèmes.
*/

    public void ChangeObjectColorOnClick(){
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Localiser l'objet
        if(Physics.Raycast(ray, out hit) && Input.GetMouseButtonDown(0)){
            selectedObject = hit.transform.gameObject;
            selectedObject.GetComponent<ObjectState>().SetState(ObjectState.State.OBSERVABLE);
            
            //Localiser le MeshCollider pour modifier hit
            Physics.Raycast(ray, out hit);
                Mesh mesh = selectedObject.GetComponent<MeshFilter>().mesh;

            
            //Permet d'ajouter dynamiquement
                Color desiredColor = colorPicker.GetColor();

                List<Vector3> vertices = new List<Vector3>(mesh.vertices);
                List<Color> colors = new List<Color>(mesh.colors);
                //NOTE:: La variation de triangles doit se faire par instance, mesh.triangles[x] = ... n'update pas la valeur
                var triangles = mesh.triangles;

                //Faire quelque chose si le vertex est partagé avec d'autres triangles
                //Triangle a 3 coins
                for(int index=0; index < 3; index++){
                    //vertex à changer
                    int vertex_index = mesh.triangles[hit.triangleIndex*3 + index];

                    //nombre d'occurrences
                    int nbOccurrences =0;
                    //Trouver le nombre d'occurrences d'un vertex donné dans mesh.triangles
                    foreach(var indexInTriangle in mesh.triangles){
                        if(vertex_index == indexInTriangle){
                            nbOccurrences++;
                        }
                    }
                    //Toujours 1 par défaut; si 2+ ==> plusieurs utilisations du même vertex
                    if(nbOccurrences > 1){
                        //Copie le vertex dans vertices à sa position(x,y,z)
                            vertices.Add(mesh.vertices[vertex_index]);

                        //Ajoute sa couleur
                            colors.Add(desiredColor);

                        //modifie le vertex lié dans triangles pour l'associer à la copie
                        //On change aussi le vertex_index pour sa nouvelle valeur
                            triangles[hit.triangleIndex*3+index] = vertices.Count-1;
                    }else{
                        //Besoin de changer la couleur du/des vertices seuls
                        colors[vertex_index] = desiredColor;
                    }
                }

                mesh.vertices = vertices.ToArray();
                mesh.colors = colors.ToArray();
                mesh.triangles = triangles;


            mesh.RecalculateNormals();

            //Rétablir l'équilibre
            selectedObject.GetComponent<ObjectState>().SetState(ObjectState.State.MOVABLE);           
        }
    }

    }
}
