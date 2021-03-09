using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    //Vector Serialization
        [System.Serializable]
        public struct SerializableVector3{
            public float x,y,z;
        }
        public static SerializableVector3 SerializeVector(Vector3 vector){
            SerializableVector3 v3 = new SerializableVector3();
                v3.x = vector.x;
                v3.y = vector.y;
                v3.z = vector.z;
            return v3;
        }
        public static Vector3 DeserializeVector(SerializableVector3 v3){
            return new Vector3(v3.x,v3.y,v3.z);
        }
        public static SerializableVector3[] SerializeVector3Array(Vector3[] vectors){
            List<SerializableVector3> serializedVectors = new List<SerializableVector3>();
            foreach(Vector3 sv in vectors){
                serializedVectors.Add(SerializeVector(sv));
            }
            return serializedVectors.ToArray();
        }

        public static Vector3[] DeserializeVectorArray(SerializableVector3[] pSerializedArray){
            List<Vector3> vectors = new List<Vector3>();
            foreach(SerializableVector3 sv in pSerializedArray){
                vectors.Add(DeserializeVector(sv));
            }
            return vectors.ToArray();
        }

        public static Color[] DeserializeVectorArrayToColor(SerializableVector3[] pSerializedArray){
            List<Color> colors = new List<Color>();
            foreach(SerializableVector3 sv in pSerializedArray){
                Vector3 colorVector = DeserializeVector(sv);
                colors.Add(new Color(colorVector.x,colorVector.y,colorVector.z));
            }
            return colors.ToArray();
        }
    //End Vector Serialization

    //Mesh Serialization
        [System.Serializable]
        public struct SerializableMesh{
            public SerializableVector3[] colors;
            public SerializableVector3[] vertices;
            public int[] triangles;
        }
        public static SerializableMesh SerializeMesh(Mesh mesh){
            SerializableMesh sMesh = new SerializableMesh();
            sMesh.vertices = SerializeVector3Array(mesh.vertices);
            sMesh.triangles = mesh.triangles;

            //Colors
            List<SerializableVector3> liste = new List<SerializableVector3>();
            foreach(var c in mesh.colors){
                SerializableVector3 vec = new SerializableVector3();
                vec.x = c.r/1f;
                vec.y = c.g/1f;
                vec.z = c.b/1f;
                liste.Add(vec);
            }
            sMesh.colors = liste.ToArray();
            return sMesh;
        }
    //End Mesh

    [System.Serializable]
    public struct Entity{
        public string reference;
        public string name;
        public SerializableVector3 position;
        public SerializableVector3 scale;
        public float rx,ry,rz,rw;
        public SerializableMesh mesh;
    }

    public static void DeleteAllEntity(){
        if(Application.isPlaying){
            foreach(GameObject s in GameObject.FindGameObjectsWithTag("GO")){
                Destroy(s);
            }
        }else{
            foreach(GameObject s in GameObject.FindGameObjectsWithTag("GO")){
                DestroyImmediate(s);
            }
        }
    }

/** Utiliser un dictionnaire est un choix d'implémentation plus extensible qu'un conteneur basique
*   Il permet d'instantier des objets dynamiquement s'ils n'existent pas dans la scène.
*   Si l'objet existe, on peut utiliser la 'key' du dictionnaire dans la scène pour update un objet donné
*/
    public static Dictionary<string, object> GetAllEntity(){
        Dictionary<string,object> state = new Dictionary<string,object>();

            foreach(GameObject s in GameObject.FindGameObjectsWithTag("GO")){
                state[s.GetComponent<SaveableEntity>().GetUniqueID()] = EntityManager.GetState(s);
            }
            return state;
    }

    public static void RestaureAllEntity(Dictionary<string,object> state){
        foreach(var element in state){
            EntityManager.Restaure((EntityManager.Entity)element.Value);
        }
    }

    public static Entity GetState(GameObject obj){
        Entity entity = new Entity();
            entity.reference = obj.GetComponent<ObjectState>().OBJ_Reference;
            entity.name = obj.GetComponent<ObjectState>().OBJ_Name;

            entity.position = SerializeVector(obj.GetComponent<Transform>().position);
            entity.scale = SerializeVector(obj.GetComponent<Transform>().localScale);
            

            Quaternion rotation = obj.GetComponent<Transform>().rotation;
            entity.rx = rotation.x;
            entity.ry = rotation.y;
            entity.rz = rotation.z;
            entity.rw = rotation.w;

            entity.mesh = SerializeMesh(obj.GetComponent<MeshFilter>().mesh);

        return entity;
    }
    public static GameObject GenerateEntity(string name, GameObject basicGO, Material material){
        GameObject obj = Instantiate(basicGO);
            obj.AddComponent<MeshCollider>();
            obj.AddComponent<Rigidbody>();
            obj.AddComponent<ObjectState>();
                obj.GetComponent<ObjectState>().OBJ_Reference = "Objects/"+ basicGO.name;
                obj.GetComponent<ObjectState>().OBJ_Name = name;
                obj.name = name;

            obj.AddComponent<ObjectColorState>();
            obj.AddComponent<SaveableEntity>();
            
            obj.GetComponent<MeshRenderer>().material = material;
            obj.tag = "GO";
            return obj;
    }


    public static GameObject Restaure(Entity entity){
        Debug.Log(entity);

        GameObject obj = Instantiate(Resources.Load(entity.reference, typeof(GameObject))) as GameObject;
        //Transforms
            obj.transform.position = DeserializeVector(entity.position);
            obj.transform.localScale = DeserializeVector(entity.scale);
            obj.transform.rotation = new Quaternion(entity.rx, entity.ry, entity.rz, entity.rw);
        //Components
            obj.AddComponent<MeshCollider>();
            obj.AddComponent<Rigidbody>();
            obj.AddComponent<ObjectState>();
                obj.GetComponent<ObjectState>().OBJ_Reference = entity.reference;
                obj.GetComponent<ObjectState>().OBJ_Name = entity.name;
                obj.name = entity.name;
            obj.AddComponent<ObjectColorState>();
        //Saveable
            obj.AddComponent<SaveableEntity>();
            obj.GetComponent<MeshRenderer>().material = Resources.Load("Color_mat", typeof(Material)) as Material;
            obj.tag = "GO";

        //Mesh
        //TODO //Validator //SHARED_MESH
            Mesh mesh = obj.GetComponent<MeshFilter>().mesh;
            mesh.vertices = DeserializeVectorArray(entity.mesh.vertices);
            mesh.colors = DeserializeVectorArrayToColor(entity.mesh.colors);
            mesh.triangles = entity.mesh.triangles;
            mesh.RecalculateNormals();


        return obj;
    }
}
