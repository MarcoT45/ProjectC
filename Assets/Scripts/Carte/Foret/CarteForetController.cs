using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarteForetController : MonoBehaviour {

    public GameObject playerPrefab;
    private List<int> noeudsUtilises = new List<int>();
    private int currentPlayerNode;
    public GameObject popup;

    private Color blackColor = new Color (0f, 0f, 0f, 1f);
    private Color lightGreyColor = new Color (0.85f, 0.85f, 0.85f, 1f);
    private Color whiteColor = new Color (1f, 1f, 1f, 1f);
    private Color greenColor = new Color(0.7450981f, 0.7372549f, 0.4156863f, 1);

    private void Start() {
        List<Noeud> mapData = GameManager.Instance.GetMapData();

        if (mapData.Count == 0) {
            CreateNewMap();
        } else {
            LoadMapData(mapData);
        }

    }

    // On crée une nouvelle carte
    private void CreateNewMap() {
        CreateNewPaths();
        DeleteUnusedNodes();
        UpdateNodesEvent();
        DeleteDuplicates();
        ConnectStartingPoint();
        ConnectBoss();
        SetPlayerStartingPosition();
        UpdateMapStartingColor();
    }

    // On crée les chemins aléatoire de la carte
    private void CreateNewPaths() {
        int firstNode = 0;
        int secondNode = 0;
        List<int> nodesSums = new List<int>();
        List<int> firstNodesList = new List<int>();

        for (int i = 0; i < 4; i++) {

            firstNode = Random.Range(1, 6);

            while (firstNodesList.Contains(firstNode)) {
                firstNode = Random.Range(1, 6);
            }

            firstNodesList.Add(firstNode);

            for (int j = 1; j < 14; j++) {

                if (firstNode == (j-1)*5+1) {
                    secondNode = Random.Range(firstNode+5, firstNode+7);
                } else if (firstNode == j*5) {
                    secondNode = Random.Range(firstNode+4, firstNode+6);
                } else {
                    secondNode = Random.Range(firstNode+4, firstNode+7);
                }

                if (nodesSums.Contains(firstNode+secondNode)) {
                    secondNode = firstNode + 5;
                }
                    
                nodesSums.Add(firstNode+secondNode);

                if(GameObject.Find("Chemin "+firstNode+"-"+secondNode) == null) {
                    var lineRenderer = new GameObject("Chemin "+firstNode+"-"+secondNode).AddComponent<LineRenderer>();
                    lineRenderer.transform.parent = GameObject.Find("Chemins Carte Foret").transform;
                    lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                    lineRenderer.startWidth = 0.1f;
                    lineRenderer.endWidth = 0.1f;
                    lineRenderer.positionCount = 2;
                    lineRenderer.startColor = lightGreyColor;
                    lineRenderer.endColor = lightGreyColor;
                
                    GameObject node1 = GameObject.Find("Noeud "+ firstNode);
                    GameObject node2 = GameObject.Find("Noeud "+ secondNode);

                    NoeudController nds = (NoeudController) node1.GetComponent(typeof(NoeudController));
                    nds.noeud.AddNoeudSuivant(secondNode);

                    NoeudController ndp = (NoeudController) node2.GetComponent(typeof(NoeudController));
                    ndp.noeud.AddNoeudPrecedent(firstNode);

                    lineRenderer.SetPosition(0, node1.transform.position);
                    lineRenderer.SetPosition(1, node2.transform.position);

                    if(!noeudsUtilises.Contains(firstNode))
                        noeudsUtilises.Add(firstNode);
                    
                    if(!noeudsUtilises.Contains(secondNode))
                        noeudsUtilises.Add(secondNode);
                }

                firstNode = secondNode;
            }
        }
    }

    // On supprime les noeuds inutiles
    private void DeleteUnusedNodes() {
        GameObject noeudTemporaire;
        for(int i = 1; i < 71; i++) {
            if(!noeudsUtilises.Contains(i)) {
                noeudTemporaire = GameObject.Find("Noeud "+ i);
                Destroy(noeudTemporaire);
            }
        }
    }

    // On rajoute les évènements aux noeuds
    private void UpdateNodesEvent() {
        GameObject tmpNode;
        noeudsUtilises.Sort();

        foreach (var n in noeudsUtilises) {
            tmpNode = GameObject.Find("Noeud "+ n);
            NoeudController nds = (NoeudController) tmpNode.GetComponent(typeof(NoeudController));

            if (n<=5) {
                nds.ChangeNodeState(0);    // Regle 1
            } else if (n>=31 && n<=35) {
                nds.ChangeNodeState(6);    // Regle 2
            } else if (n>=66) {
                nds.ChangeNodeState(3);    // Regle 3
            } else {
                int randomEventValue = Random.Range(0, 101);
                
                if (randomEventValue < 64) { 
                    nds.ChangeNodeState(0); // Combat 64%
                } else if (randomEventValue < 71) {
                    nds.ChangeNodeState(1); // Evenement 7%
                } else if (randomEventValue < 78) {
                    nds.ChangeNodeState(3); // Repos 7%
                } else if (randomEventValue < 85) {
                    nds.ChangeNodeState(4); // Magasin 7%
                } else if (randomEventValue < 92) {
                    nds.ChangeNodeState(5); // Echange 7%
                } else if (randomEventValue < 100) {
                    nds.ChangeNodeState(6); // Coffre 7%
                } else {
                    nds.ChangeNodeState(2); // Elite 1%
                }
            }
        }
    }

    // On supprime/remplace les doublons de noeuds qui se suivent
    private void DeleteDuplicates() {
        GameObject tmpNode, previousNode;
        bool duplicate = false;

        List<int> remainingEvent = new List<int>() {1, 3, 4, 5, 6};

        foreach (var n in noeudsUtilises) {

            remainingEvent = new List<int>() {1, 3, 4, 5, 6};

            tmpNode = GameObject.Find("Noeud "+ n);
            NoeudController nds = (NoeudController) tmpNode.GetComponent(typeof(NoeudController));

            if (nds.noeud.eventNumber != 0 && nds.noeud.eventNumber != 2) {
                foreach (var p in nds.noeud.noeudsPrecedent) {
                    previousNode = GameObject.Find("Noeud "+ p);
                    NoeudController pnd = (NoeudController) previousNode.GetComponent(typeof(NoeudController));

                    if(nds.noeud.eventNumber == pnd.noeud.eventNumber) {
                        duplicate = true;
                    }

                    remainingEvent.Remove(pnd.noeud.eventNumber);
                }

                if(duplicate) {
                    nds.ChangeNodeState(remainingEvent[Random.Range(0, remainingEvent.Count)]);
                    duplicate = false;
                }
            }
        }
    }

    // On connecte le point de départ à la carte
    private void ConnectStartingPoint() {
        GameObject node1 = GameObject.Find("Noeud 0");

        foreach (var n in noeudsUtilises) {
            if(n<=5) {
                var lineRenderer = new GameObject("Chemin 0"+"-"+n).AddComponent<LineRenderer>();
                lineRenderer.transform.parent = GameObject.Find("Chemins Carte Foret").transform;
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.startWidth = 0.1f;
                lineRenderer.endWidth = 0.1f;
                lineRenderer.positionCount = 2;
                lineRenderer.startColor = whiteColor;
                lineRenderer.endColor = whiteColor;
                
                GameObject node2 = GameObject.Find("Noeud "+ n);

                NoeudController nds = (NoeudController) node1.GetComponent(typeof(NoeudController));
                nds.noeud.AddNoeudSuivant(n);

                NoeudController ndp = (NoeudController) node2.GetComponent(typeof(NoeudController));
                ndp.noeud.AddNoeudPrecedent(0);

                lineRenderer.SetPosition(0, node1.transform.position);
                lineRenderer.SetPosition(1, node2.transform.position);
            }
        }
    }

    // On connecte le boss à la carte
    private void ConnectBoss() {
        GameObject node2 = GameObject.Find("Noeud 71");
        NoeudController ndp = (NoeudController) node2.GetComponent(typeof(NoeudController));
        ndp.ChangeNodeState(7);
        ndp.UpdateCouleur();

        foreach (var n in noeudsUtilises) {
            if(n>=66) {
                var lineRenderer = new GameObject("Chemin "+n+"-71").AddComponent<LineRenderer>();
                lineRenderer.transform.parent = GameObject.Find("Chemins Carte Foret").transform;
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.startWidth = 0.1f;
                lineRenderer.endWidth = 0.1f;
                lineRenderer.positionCount = 2;
                lineRenderer.startColor = lightGreyColor;
                lineRenderer.endColor = lightGreyColor;
                
                GameObject node1 = GameObject.Find("Noeud "+ n);
                NoeudController nds = (NoeudController) node1.GetComponent(typeof(NoeudController));
                nds.noeud.AddNoeudSuivant(71);

                ndp.noeud.AddNoeudPrecedent(n);

                lineRenderer.SetPosition(0, node1.transform.position);
                lineRenderer.SetPosition(1, node2.transform.position);
            }
        }
    }

    // On place le joueur sur le noeud de départ et on active l'etat des noeuds voisins
    private void SetPlayerStartingPosition() {
        GameObject nodeDepart = GameObject.Find("Noeud 0");
        NoeudController ndd = (NoeudController) nodeDepart.GetComponent(typeof(NoeudController));
        ndd.UpdateEtatNoeud(EtatNoeud.Joueur);

        GameObject player = Instantiate(playerPrefab, nodeDepart.transform.position, Quaternion.identity, nodeDepart.transform);
        player.name = "Joueur";
        currentPlayerNode = 0;

        foreach (var n in noeudsUtilises) {
            if(n<=5) {      
                GameObject node = GameObject.Find("Noeud "+ n);
                NoeudController nd = (NoeudController) node.GetComponent(typeof(NoeudController));
                nd.UpdateEtatNoeud(EtatNoeud.Accessible);
            }
        }
    }

    // On met à jour les couleurs des noeuds/chemins sur la carte
    private void UpdateMapStartingColor() {
        foreach (var n in noeudsUtilises) {
            GameObject node = GameObject.Find("Noeud "+ n);
            NoeudController nd = (NoeudController) node.GetComponent(typeof(NoeudController));
            nd.UpdateCouleur();
        }
    }

    // On déplace le joueur sur le noeud cliqué
    public void MovePlayer(Noeud newPosition) {

        if(!popup.activeSelf) {

            GameObject currentNode = GameObject.Find("Noeud "+ currentPlayerNode);
            NoeudController cd = (NoeudController) currentNode.GetComponent(typeof(NoeudController));
            cd.UpdateEtatNoeud(EtatNoeud.Visite);

            foreach (var n in cd.noeud.noeudsSuivant) {
                if(n != newPosition.numero) {
                    GameObject node = GameObject.Find("Noeud "+ n);
                    NoeudController nd = (NoeudController) node.GetComponent(typeof(NoeudController));
                    nd.UpdateEtatNoeud(EtatNoeud.Inatteignable);

                    GameObject path = GameObject.Find("Chemin "+ currentPlayerNode +"-"+n);
                    LineRenderer p = (LineRenderer) path.GetComponent(typeof(LineRenderer));
                    p.startColor = lightGreyColor;
                    p.endColor = lightGreyColor;
                }
            }

            GameObject newNode = GameObject.Find("Noeud "+ newPosition.numero);
            NoeudController cdn = (NoeudController) newNode.GetComponent(typeof(NoeudController));
            cdn.UpdateEtatNoeud(EtatNoeud.Joueur);

            GameObject player = GameObject.Find("Joueur");
            player.transform.parent = newNode.gameObject.transform;
            CarteJoueurForetController pl = (CarteJoueurForetController) player.GetComponent(typeof(CarteJoueurForetController));
            pl.UpdateTargetPosition(newNode.transform.position);
            currentPlayerNode = newPosition.numero;

            GameObject commandCarte = GameObject.Find("CarteDeplacementController");
            CarteDeplacementController cmdC = (CarteDeplacementController) commandCarte.GetComponent(typeof(CarteDeplacementController));
            cmdC.UpdateCurrentNode(newPosition.numero);
        }
    }

    // On débloque les prochains chemins et on joue l'évenement du noeud
    public void UnlockPath() {

        GameObject currentNode = GameObject.Find("Noeud "+ currentPlayerNode);
        NoeudController cn = (NoeudController) currentNode.GetComponent(typeof(NoeudController));

        foreach (var n in cn.noeud.noeudsSuivant) {
            GameObject node = GameObject.Find("Noeud "+ n);
            NoeudController nd = (NoeudController) node.GetComponent(typeof(NoeudController));
            nd.UpdateEtatNoeud(EtatNoeud.Accessible);

            GameObject path = GameObject.Find("Chemin "+ cn.noeud.numero +"-"+n);
            LineRenderer p = (LineRenderer) path.GetComponent(typeof(LineRenderer));
            p.startColor = whiteColor;
            p.endColor = whiteColor;
        }

        PlayNodeEvent(cn.noeud);
    }

    // On joue l'evenement du node ici ?
    private void PlayNodeEvent(Noeud noeud){
        if (noeud.eventNumber == 0 || noeud.eventNumber == 2 || noeud.eventNumber == 7) {
            SendDataToManager();
            ControlsManager.Instance.UpdateState(400);
            SceneManager.LoadScene(3);
        } else {
            PopUpEventForetController pu = (PopUpEventForetController) popup.GetComponent(typeof(PopUpEventForetController));
            ControlsManager.Instance.UpdateState(302);
            pu.GeneratePopUpEvent(noeud);
        }
    }

    // On envoie les données de la map dans le GameManager juste avant un changement de scene combat
    private void SendDataToManager() {
        List<Noeud> listNoeuds = new List<Noeud>();

        foreach (var n in noeudsUtilises) {
            GameObject tmpNode = GameObject.Find("Noeud "+ n);
            NoeudController nds = (NoeudController) tmpNode.GetComponent(typeof(NoeudController));

            listNoeuds.Add(nds.noeud);
        }

        GameManager.Instance.SetMapData(listNoeuds);
    }

    // On charge les données de la carte et on déplace la caméra sur le joueur
    private void LoadMapData(List<Noeud> mapData) {

        GameObject nodeDepart = GameObject.Find("Noeud 0");
        GameObject nodeFinal = GameObject.Find("Noeud 71");

        foreach (var n in mapData) {
            GameObject nodeTmp = GameObject.Find("Noeud "+n.numero);
            NoeudController ndTmp = (NoeudController) nodeTmp.GetComponent(typeof(NoeudController));
            ndTmp.noeud = new Noeud(n);
            ndTmp.UpdateCouleur();
            ndTmp.ChangeNodeState(n.eventNumber);

            if (n.etat == EtatNoeud.Joueur) {
                GameObject player = Instantiate(playerPrefab, nodeTmp.transform.position, Quaternion.identity, nodeTmp.transform);
                player.name = "Joueur";
                currentPlayerNode = n.numero;

                GameObject commandCarte = GameObject.Find("CarteDeplacementController");
                CarteDeplacementController cmdC = (CarteDeplacementController) commandCarte.GetComponent(typeof(CarteDeplacementController));
                cmdC.UpdateCurrentNode(currentPlayerNode);

                GameObject camera = GameObject.Find("Main Camera");
                MoveCameraMapController cam = (MoveCameraMapController) camera.GetComponent(typeof(MoveCameraMapController));
                cam.SetCameraToPlayerPosition(nodeTmp.transform.position.y);
            }

            noeudsUtilises.Add(n.numero);
        }

        foreach (var n in mapData) {
            GameObject nodeTmp = GameObject.Find("Noeud "+n.numero);
            NoeudController ndTmp = (NoeudController) nodeTmp.GetComponent(typeof(NoeudController));
            ndTmp.noeud = new Noeud(n);
            ndTmp.UpdateCouleur();
            ndTmp.ChangeNodeState(n.eventNumber);

            foreach (var nextPath in n.noeudsSuivant) {
                var lineRenderer = new GameObject("Chemin "+n.numero+"-"+nextPath).AddComponent<LineRenderer>();
                lineRenderer.transform.parent = GameObject.Find("Chemins Carte Foret").transform;
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.startWidth = 0.1f;
                lineRenderer.endWidth = 0.1f;
                lineRenderer.positionCount = 2;

                GameObject nextNode = GameObject.Find("Noeud "+ nextPath);
                NoeudController nextTmp = (NoeudController) nextNode.GetComponent(typeof(NoeudController));

                if ( (nextTmp.noeud.etat == EtatNoeud.Accessible) && (ndTmp.noeud.etat == EtatNoeud.Visite || ndTmp.noeud.etat == EtatNoeud.Joueur) ) {
                    lineRenderer.startColor = whiteColor;
                    lineRenderer.endColor = whiteColor;
                } else if ( (nextTmp.noeud.etat == EtatNoeud.Visite || nextTmp.noeud.etat == EtatNoeud.Joueur) && (ndTmp.noeud.etat == EtatNoeud.Visite || ndTmp.noeud.etat == EtatNoeud.Joueur) ) {
                    lineRenderer.startColor = greenColor;
                    lineRenderer.endColor = greenColor;
                } else {
                    lineRenderer.startColor = lightGreyColor;
                    lineRenderer.endColor = lightGreyColor;
                }

                lineRenderer.SetPosition(0, nodeTmp.transform.position);
                lineRenderer.SetPosition(1, nextNode.transform.position);
            }
            
            if (n.numero <= 5) {
                var lineRenderer = new GameObject("Chemin 0"+"-"+n.numero).AddComponent<LineRenderer>();
                lineRenderer.transform.parent = GameObject.Find("Chemins Carte Foret").transform;
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.startWidth = 0.1f;
                lineRenderer.endWidth = 0.1f;
                lineRenderer.positionCount = 2;
                if (n.etat == EtatNoeud.Visite || n.etat == EtatNoeud.Joueur) {
                    lineRenderer.startColor = greenColor;
                    lineRenderer.endColor = greenColor;
                } else {
                    lineRenderer.startColor = lightGreyColor;
                    lineRenderer.endColor = lightGreyColor;
                }
                lineRenderer.SetPosition(0, nodeDepart.transform.position);
                lineRenderer.SetPosition(1, nodeTmp.transform.position);
            }

            if (n.numero >= 66) {
                var lineRenderer = new GameObject("Chemin "+n.numero+"-71").AddComponent<LineRenderer>();
                lineRenderer.transform.parent = GameObject.Find("Chemins Carte Foret").transform;
                lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
                lineRenderer.startWidth = 0.1f;
                lineRenderer.endWidth = 0.1f;
                lineRenderer.positionCount = 2;
                lineRenderer.startColor = lightGreyColor;
                lineRenderer.endColor = lightGreyColor;
                lineRenderer.SetPosition(0, nodeTmp.transform.position);
                lineRenderer.SetPosition(1, nodeFinal.transform.position);
            }
        }

        NoeudController ndfin = (NoeudController) nodeFinal.GetComponent(typeof(NoeudController));
        ndfin.UpdateCouleur();
        ndfin.ChangeNodeState(7);

        DeleteUnusedNodes();
    }

}