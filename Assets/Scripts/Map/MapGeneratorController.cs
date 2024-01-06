using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorController : MonoBehaviour {

    private int hauteur = 15; // Hauteur max de la map générée
    private int largeur = 7;  // Largeur max de la map générée
    private int nbChemins = 6; // Nombre de chemins générés dans la map

    public GameObject nodePrefab; // Prefab de Noeud

    public List<GameObject> mapNodes = new List<GameObject>(); // Liste des gameobject noeuds de la map
    public List<int> usedNodes = new List<int>(); // Liste des numéros des noeuds utilisés

    public void GenerateNewMap () {
        ResetMap();
        GenerateNodes();
        GeneratePaths();
        DeleteUnusedNodes();
    }

    // On reset la map
    private void ResetMap() {
        this.mapNodes = new List<GameObject>();
        this.usedNodes = new List<int>();
        while (this.transform.childCount > 0) {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

    // Fonction qui genere une grille de gameobject noeud de taille hauteur*largeur
    private void GenerateNodes () {

        GameObject newFloor;
        GameObject newNode;
        
        for (int h = 0; h < this.hauteur; h++) {
        
            newFloor = new GameObject("Floor "+h);
            newFloor.transform.parent = this.gameObject.transform;
        
            for (int l = 0; l < this.largeur; l++) {
        
                Vector2 nodePosition = new Vector2(l+l*0.5f, h+h*0.5f);
                newNode = Instantiate(nodePrefab, nodePosition, Quaternion.identity, newFloor.gameObject.transform);
                newNode.name = "Node "+(h*largeur+l);
        
                Node node = (Node) newNode.GetComponent(typeof(Node));
                node.SetPosition(nodePosition);
        
                mapNodes.Add(newNode);
            }
        }
    }

    // Fonction qui va générer nbChemins de bas en haut
    private void GeneratePaths () {

        int randomFirstNodeNumber = 0;
        int currentNodeNumber = 0;
        int nextNodeNumber = 0;

        GameObject currentNode;
        GameObject nextNode;

        List<int> nodesSums = new List<int>();

        for (int i = 0; i < this.nbChemins; i++) {

            //On crée une nouvelle ligne
            var lineRenderer = new GameObject("Line "+ i).AddComponent<LineRenderer>();
            lineRenderer.transform.parent = this.gameObject.transform;
            lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.positionCount = 15;
            lineRenderer.useWorldSpace = true;
            lineRenderer.startColor = Color.blue;
            lineRenderer.endColor = Color.blue;

            for (int j = 0; j < this.hauteur; j++) {

                if (j == 0) {
                    randomFirstNodeNumber = Random.Range(0, largeur);
                    currentNodeNumber = randomFirstNodeNumber;
                    currentNode = GameObject.Find("Node "+ currentNodeNumber);
                    lineRenderer.SetPosition(j, new Vector2(currentNode.transform.position.x, currentNode.transform.position.y));
                } else {

                    var tmpValue = currentNodeNumber % this.largeur;
                    
                    switch (tmpValue) {
                        case 0:
                            nextNodeNumber = currentNodeNumber + largeur + Random.Range(0, 2);
                            break;
                        case 6:
                            nextNodeNumber = currentNodeNumber + largeur + Random.Range(-1, 1);
                            break;
                        default:
                            nextNodeNumber = currentNodeNumber + largeur + Random.Range(-1, 2);
                            break;
                    }

                    if (nodesSums.Contains(currentNodeNumber+nextNodeNumber)) {
                        nextNodeNumber = currentNodeNumber + largeur;
                    } else {
                        nodesSums.Add(currentNodeNumber+nextNodeNumber);
                    }

                    if (!usedNodes.Contains(currentNodeNumber)) {
                        this.usedNodes.Add(currentNodeNumber);
                    }

                    if (!usedNodes.Contains(nextNodeNumber)) {
                        this.usedNodes.Add(nextNodeNumber);
                    }

                    currentNode = GameObject.Find("Node "+ currentNodeNumber);
                    nextNode = GameObject.Find("Node "+ nextNodeNumber);
                    lineRenderer.SetPosition(j, new Vector2(nextNode.transform.position.x, nextNode.transform.position.y));

                    Node node = (Node) currentNode.GetComponent(typeof(Node));
                    node.AddNodeLinkedNextFloor(nextNode);

                    currentNodeNumber = nextNodeNumber;
                }

            }
        }
    }

    // Fonction qui supprime les noeuds qui ne servent pas
    private void DeleteUnusedNodes() {
        GameObject tmpNode;
        for(int i = 0; i < hauteur*largeur; i++) {
            if(!this.usedNodes.Contains(i)){
                tmpNode = GameObject.Find("Node "+ i);
                this.mapNodes.Remove(tmpNode);
                Destroy(tmpNode);
            }
        }
    }

}