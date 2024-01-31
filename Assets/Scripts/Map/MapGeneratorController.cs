using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapGeneratorController : MonoBehaviour {

    private int hauteur = 15;  // Hauteur max de la map générée
    private int largeur = 7;   // Largeur max de la map générée
    private int nbChemins = 6; // Nombre de chemins générés dans la map

    public GameObject nodePrefab; // Prefab de Noeud

    public List<GameObject> mapNodes = new List<GameObject>(); // Liste des gameobject noeuds de la map
    public List<int> usedNodes = new List<int>();              // Liste des numéros des noeuds utilisés

    private Vector3 originPos;       // Pour pouvoir scroll la carte
    private Vector3 targetPos;       // Pour pouvoir scroll la carte
    private float maxHeightBoss = 0; // Pour pouvoir scroll la carte

    public GameObject popUpInfos;                     // Pour afficher/cacher les infos
    public TextMeshProUGUI popUpTitre;                // Titre de la pop-up
    public TextMeshProUGUI popUpDescription;          // Description de la pop-up
    public GameObject encartCombat;                   // Encart combat fond
    public GameObject encartCombatTexte;              // Encart combat texte
    public TextMeshProUGUI encartCombatTexteTaille;   // Encart combat texte taille
    public TextMeshProUGUI encartCombatTexteEnnemis;  // Encart combat texte ennemis

    public GameObject popUpChest;                     // Pour afficher/cacher la pop-up Coffre
    public GameObject popUpEvent;                     // Pour afficher/cacher la pop-up Evenement
    public GameObject popUpRest;                      // Pour afficher/cacher la pop-up Repos
    public GameObject popUpTrade;                     // Pour afficher/cacher la pop-up Echange
    public GameObject popUpShop;                      // Pour afficher/cacher la pop-up Boutique
    private bool popUpOpened = false;                 // Pour savoir si une pop-up est ouverte ou non

    private void Start() {
        this.originPos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        this.targetPos = new Vector3(originPos.x, originPos.y, originPos.z);
    }

    private void Update() {
        while (this.transform.position != targetPos) {
            this.transform.position = Vector3.MoveTowards(this.transform.position, this.targetPos, 1f * Time.deltaTime);
        }

        if ( Input.GetAxis("Mouse ScrollWheel") != 0 ) {
            // On vérifie qu'on ne scroll pas trop loin
            if ( !(this.transform.position.y + 7*Input.GetAxis("Mouse ScrollWheel") > originPos.y) && !(this.transform.position.y + 7*Input.GetAxis("Mouse ScrollWheel") < -this.maxHeightBoss) ) {
                this.targetPos = new Vector3(originPos.x, this.transform.position.y + 7*Input.GetAxis("Mouse ScrollWheel"), originPos.z);
            }
        }
    }

    public void GenerateNewMap () {
        ResetMap();
        GenerateNodes();
        GeneratePaths();
        DeleteUnusedNodes();
        UpdateMapToEvent();
        ConnectBossToLastFloor();
        UpdateFirstNodeStatus();
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
            newFloor.transform.position = new Vector3(0, 0, 0);
        
            for (int l = 0; l < this.largeur; l++) {
        
                Vector2 nodePosition = new Vector2(l+l*0.5f-1.5f, h+h*0.5f-3);
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
            lineRenderer.startWidth = 0.05f;
            lineRenderer.endWidth = 0.05f;
            lineRenderer.positionCount = 15;
            lineRenderer.useWorldSpace = false;
            lineRenderer.startColor = Color.cyan;
            lineRenderer.endColor = Color.cyan;

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

    // Fonction qui crée les evenements pour la carte
    /*  REGLES
        1 - All the Rooms on the 1st Floor with Monsters.
        2 - Then all the Rooms on the 9th Floor with Treasure.
        3 - And all the Rooms on the 15th Floor with Rest Sites.
        4 - Elite and Rest Sites can’t be assigned below the 6th Floor.
        5 - Rest Site cannot be on the 14th Floor.
    */
    private void UpdateMapToEvent() {

        GameObject tmpNode;
        bool canRest = true;
        bool canElite = true;

        this.usedNodes.Sort();
        this.usedNodes.Reverse();

        foreach (var n in this.usedNodes) {

            tmpNode = GameObject.Find("Node "+ n);
            Node node = (Node) tmpNode.GetComponent(typeof(Node));

            if ( n < largeur ) { // REGLE 1
                node.ChangeNodeEvent(0); 
            } else if ( n >= 8*largeur && n < 9*largeur ) { // REGLE 2
                node.ChangeNodeEvent(2); 
            } else if ( n >= 14*largeur ) { // REGLE 3
                node.ChangeNodeEvent(4); 
            } else {

                if ( n < 35 ) { // REGLE 4
                    canRest = false;
                    canElite = false;
                } else {
                    canRest = true;
                    canElite = true;
                    if ( n >= 13 * largeur ) { // REGLE 5
                        canRest = false;
                    }
                }

                int randomEventValue = Random.Range(0, 101);
                
                if (randomEventValue < 45) { 
                    node.ChangeNodeEvent(0); // Ennemy 45%
                } else if (randomEventValue < 60 && canElite) {
                    node.ChangeNodeEvent(1); // Elite 15%
                } else if (randomEventValue < 68) {
                    node.ChangeNodeEvent(2); // Chest 8%
                } else if (randomEventValue < 76) {
                    node.ChangeNodeEvent(3); // Event 8%
                } else if (randomEventValue < 84 && canRest) {
                    node.ChangeNodeEvent(4); // Rest 8%
                } else if (randomEventValue < 92) {
                    node.ChangeNodeEvent(5); // Trade 8%
                } else {
                    node.ChangeNodeEvent(6); // Shop 8%
                }
            
            }     
        }
    }

    // Fonction qui crée et connecte le Node du Boss Final
    private void ConnectBossToLastFloor() {

        GameObject bossFloor;
        GameObject bossNode;
        
        bossFloor = new GameObject("Floor Boss");
        bossFloor.transform.parent = this.gameObject.transform;
        
        Vector2 bossNodePosition = new Vector2(((largeur-1)+(largeur-1)*0.5f)/2-1.5f, hauteur+hauteur*0.5f-1.5f);
        bossNode = Instantiate(nodePrefab, bossNodePosition, Quaternion.identity, bossFloor.gameObject.transform);
        bossNode.name = "Node Boss";
        this.maxHeightBoss = bossNodePosition.y;
        
        Node node = (Node) bossNode.GetComponent(typeof(Node));
        node.SetPosition(bossNodePosition);
        node.ChangeNodeEvent(7);
        node.UpdateNodeState(NodeState.Bloque);
        
        mapNodes.Add(bossNode);

        //On connecte les dernières lignes
        GameObject tmpNode;
        int i = 0;

        foreach (var n in this.usedNodes) {
            if ( n >= 14*largeur ) {
                tmpNode = GameObject.Find("Node "+ n);
                Node nodeToBoss = (Node) tmpNode.GetComponent(typeof(Node));
                nodeToBoss.AddNodeLinkedNextFloor(bossNode);

                //On crée une nouvelle ligne
                var lineRenderer = new GameObject("Boss Line "+ i).AddComponent<LineRenderer>();
                lineRenderer.transform.parent = this.gameObject.transform;
                lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
                lineRenderer.startWidth = 0.05f;
                lineRenderer.endWidth = 0.05f;
                lineRenderer.positionCount = 2;
                lineRenderer.useWorldSpace = false;
                lineRenderer.startColor = Color.cyan;
                lineRenderer.endColor = Color.cyan;

                lineRenderer.SetPosition(0, new Vector2(tmpNode.transform.position.x, tmpNode.transform.position.y));
                lineRenderer.SetPosition(1, new Vector2(bossNode.transform.position.x, bossNode.transform.position.y));
                i++;
            }
        }
    }

    // Fonction qui met les noeuds du 1ere étage en accessible
    private void UpdateFirstNodeStatus() {
        GameObject tmpNode;
        foreach (var n in this.usedNodes) {
            tmpNode = GameObject.Find("Node "+ n);
            Node node = (Node) tmpNode.GetComponent(typeof(Node));
            if ( n < largeur ) {
                node.UpdateNodeState(NodeState.Accessible);
            } else {
                node.UpdateNodeState(NodeState.Bloque);
            }
        }
    }

    // On affiche/cache la pop-up et on update les textes et la position de la pop-up
    public void DisplayPopUp(bool display, Node node) {
        // On n'affiche pas les pop-up d'information si une grosse pop-up est ouverte
        if (!popUpOpened) {
            this.popUpTitre.text = node.titre;
            this.popUpDescription.text = node.description;
            this.popUpInfos.transform.position = new Vector3(node.transform.position.x, node.transform.position.y + 1.5f, node.transform.position.z);
            this.popUpInfos.SetActive(display);

            if (node.titre == "Combat normal" || node.titre == "Combat d'élite") {

                switch (node.taille) {
                    case 2:
                        this.encartCombatTexteTaille.text = "Petite";
                        this.encartCombatTexteTaille.color = Color.blue;
                        break;
                    case 3:
                        this.encartCombatTexteTaille.text = "Moyenne";
                        this.encartCombatTexteTaille.color = Color.white;
                        break;
                    case 4:
                        this.encartCombatTexteTaille.text = "Grande";
                        this.encartCombatTexteTaille.color = Color.red;
                        break;
                }

                switch (node.nbEnnemis) {
                    case 2:
                        this.encartCombatTexteEnnemis.text = "Faible";
                        this.encartCombatTexteEnnemis.color = Color.blue;
                        break;
                    case 3:
                        this.encartCombatTexteEnnemis.text = "Moyen";
                        this.encartCombatTexteEnnemis.color = Color.cyan;
                        break;
                    case 4:
                        this.encartCombatTexteEnnemis.text = "Abondant";
                        this.encartCombatTexteEnnemis.color = Color.white;
                        break;
                    case 5:
                        this.encartCombatTexteEnnemis.text = "Légion";
                        this.encartCombatTexteEnnemis.color = Color.magenta;
                        break;
                    case 6:
                        this.encartCombatTexteEnnemis.text = "Mortel";
                        this.encartCombatTexteEnnemis.color = Color.red;
                        break;
                }

                this.encartCombat.SetActive(true);
                this.encartCombatTexte.SetActive(true);
            } else {
                this.encartCombat.SetActive(false);
                this.encartCombatTexte.SetActive(false);
            }
        }
    }

    // On ouvre la pop-up au clic sur le node
    public void OpenPopUp(Node node) {
        switch (node.titre) {
            case "Coffre":
                this.popUpChest.SetActive(true);
                this.popUpInfos.SetActive(false);
                this.popUpOpened = true;
                break;
            case "Événement":
                this.popUpEvent.SetActive(true);
                this.popUpInfos.SetActive(false);
                this.popUpOpened = true;
                break;
            case "Repos":
                this.popUpRest.SetActive(true);
                this.popUpInfos.SetActive(false);
                this.popUpOpened = true;
                break;
            case "Echange":
                this.popUpTrade.SetActive(true);
                this.popUpInfos.SetActive(false);
                this.popUpOpened = true;
                break;
            case "Magasin":
                this.popUpShop.SetActive(true);
                this.popUpInfos.SetActive(false);
                this.popUpOpened = true;
                break;
        }
    }

    // On met à jour la variable de la fermeture de la pop-up
    public void ClosedPopUpUpdate() {
        this.popUpOpened = false;
    }

}