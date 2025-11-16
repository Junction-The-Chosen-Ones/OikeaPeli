using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

using static NetworkDataset;
public class MapManager : MonoBehaviour
{
    static int currentColumn = 0;
    static int currentNode = 0;
    static int EncounterCounter = 0; //Capped at 3, because image-generation takes time and money
    static List<Vector2> RedLane = new List<Vector2>(); //A temporary quick fix to map-regen issue
    int playerAnimCounter = 0;
    int playerAnimCoef= 1;
    int currentEvent = -1;
    bool battleToggle = true;

    public Sprite[] MapIconList;
    public static MapNode[][] nodes; //Array of arrays(columns of nodes)

    public List<Entity> enemies; // is_enemy = true
    public List<DialogEntry> dialogs; // dialogs

    public static List<Entity> enemiesStatic = new();
    public static List<DialogEntry> dialogStatic = new();

    [SerializeField] private GameObject LevelNode;
    [SerializeField] private GameObject Pathline;
    [SerializeField] private GameObject MapIcon;
    [SerializeField] private GameObject BattleButton;

    [SerializeField] TMP_Text textbox;

    [HideInInspector] public GameObject playerIcon;
    public class MapNode
    {
        public int[] linkIndices;
        public Vector3 nodePos;
        public GameObject nodeObject;
        public GameObject[][] linkObjects;
        public NodeEffect nodeEffect;

        public MapNode(int[] linkindices, Vector3 nodepos, GameObject levelNode, GameObject MapIcon, Sprite[] MapIconList, int nodeType = -1)
        {
            linkIndices = linkindices;
            nodePos = nodepos;
            nodeObject = Instantiate(levelNode, nodePos, Quaternion.identity);
            if (nodeType == -1)
            {
                nodeType = Random.Range(0, MapIconList.Length-1);
            }
            if (nodeType == 0)
            {
                if (EncounterCounter == 3) { nodeType = 1; }
                else { EncounterCounter += 1;}
            }
            GameObject nodeIcon = Instantiate(MapIcon, nodePos+new Vector3(0, 0.8f, -1), Quaternion.identity);
            nodeIcon.GetComponent<SpriteRenderer>().sprite = MapIconList[nodeType];
            nodeEffect = new NodeEffect();
            nodeEffect.NodeEffectIcon = nodeIcon;
            nodeEffect.EffectID = nodeType;
        }
        public void Redraw(GameObject levelNode, GameObject MapIcon, Sprite[] MapIconList)
        {
            nodeObject = Instantiate(levelNode, nodePos, Quaternion.identity);
            nodeEffect.NodeEffectIcon = Instantiate(MapIcon, nodePos + new Vector3(0, 0.8f, -1), Quaternion.identity);
            nodeEffect.NodeEffectIcon.GetComponent<SpriteRenderer>().sprite = MapIconList[nodeEffect.EffectID];
        }

        public void DrawLinks(int myColumn, GameObject pathLine)
        {
            List<GameObject[]> links = new List<GameObject[]>();
            for (int i = 0; i < linkIndices.Length; i++)
            {
                links.Add(DrawNodePath(nodePos, nodes[myColumn + 1][linkIndices[i]].nodePos, pathLine));
            }
            linkObjects = links.ToArray();
        }
    }

    public struct NodeEffect
    {
        public GameObject NodeEffectIcon;
        public int EffectID;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemiesStatic.AddRange(enemies.ToArray());
        dialogStatic.AddRange(dialogs.ToArray());
        if (currentColumn == 0)
        {
            GenerateMap(new Vector3(-6, 1, -1));
        }
        else
        {
            print($"Debug: {RedLane.Count}, {RedLane[0]}, {currentColumn}, {currentNode}");
            DrawNodeMap();
        }
        playerIcon = Instantiate(MapIcon, new Vector3(0, 0, 0), Quaternion.identity);
        print($"Debug: {playerIcon}, {nodes.Length}, {currentColumn}, {currentNode}");
    }

    // Update is called once per frame
    void Update()
    {
        //Player icon control:
        if(!playerIcon) playerIcon = Instantiate(MapIcon, new Vector3(0, 0, 0), Quaternion.identity);
        playerAnimCounter++;
        if (playerAnimCounter == 60) { playerAnimCounter = 0; playerAnimCoef = -playerAnimCoef; }
        Transform playertransformer = playerIcon.GetComponent<Transform>();
        playertransformer.position = nodes[currentColumn][currentNode].nodePos + new Vector3(0, 0.8f, 0) + new Vector3(0, -0.05f * playerAnimCoef, 0);

        //Node click-checker & Protocol
        if (currentEvent == -1)
        {
            for (int i = 0; i < nodes[currentColumn][currentNode].linkIndices.Length; i++)
            {
                int linkedNodeIndex = nodes[currentColumn][currentNode].linkIndices[i];
                MapNode linkedNode = nodes[currentColumn + 1][linkedNodeIndex];
                if (linkedNode.nodeObject.GetComponent<BasicLevelClick>().GetClick()) //Click found on a linked nodes
                {
                    for (int j = 0; j < nodes[currentColumn + 1].Length; j++)
                    {
                        SetNodeColor(nodes[currentColumn + 1][j].nodeObject, Color.gray);
                    }
                    currentNode = linkedNodeIndex;
                    currentColumn++;
                    SetNodeColor(nodes[currentColumn][currentNode].nodeObject, Color.red);
                    RedLane.Add(new Vector2(currentColumn, currentNode));
                    Destroy(nodes[currentColumn][currentNode].nodeEffect.NodeEffectIcon);
                    currentEvent = nodes[currentColumn][currentNode].nodeEffect.EffectID;
                    if (currentColumn == nodes.Length - 1)
                    {
                        ClearMap();
                        RedLane.Clear();
                        GenerateMap(new Vector3(-6, 1, -1));
                    }
                    else
                    {
                        for (int j = 0; j < nodes[currentColumn][currentNode].linkIndices.Length; j++)
                        {
                            SetNodeColor(nodes[currentColumn + 1][nodes[currentColumn][currentNode].linkIndices[j]].nodeObject, Color.green);
                        }
                    }
                }
            }
        }
        else
        {
            switch(currentEvent) //Different event effects
            {
                case 0: //Regular battle event
                    if (battleToggle)
                    {
                        if (enemiesStatic.Count != 0 && dialogStatic.Count != 0)
                        {
                            int TryCounter = 0;
                            int enemyIndex = 0;
                            while (true)
                            {
                                TryCounter++;
                                enemyIndex = Random.Range(0, enemiesStatic.Count);
                                if (!enemiesStatic[enemyIndex].is_boss || TryCounter == 20) { break; }
                            }
                            int enemyID = enemiesStatic[enemyIndex].id;
                            string enemyDialog = "Enemy Approaches, Time To Battle!";
                            foreach (var dialog in dialogStatic)
                            {
                                if (dialog.characterId == $"{enemyID}")
                                {
                                    enemyDialog = dialog.content;
                                }
                            }
                            Enemy.CurHP = enemiesStatic[enemyID].health;
                            textbox.text = enemyDialog;
                            BattleButton.SetActive(true);
                        }
                        else
                        {
                            textbox.text = "Enemy Approaches, Time To Battle!";
                            BattleButton.SetActive(true);
                        }
                    }
                    
                    break;
                case 1:
                    print("exclamation");
                    currentEvent = -1;
                    break;
                case 2:
                    print("heart");
                    currentEvent = -1;
                    break;
                case 3:
                    print("money");
                    currentEvent = -1;
                    break;
                case 4:
                    print("treasure");
                    currentEvent = -1;
                    break;
                case 5: //Question -> Boss battle
                    if (battleToggle)
                    {
                        bool foundEnemy = false;
                        int enemyIndex = 0;
                        int counter = 0;
                        foreach (var enemy in enemiesStatic)
                        {
                            counter++;
                            if (enemy.is_boss)
                            {
                                enemyIndex = counter;
                                foundEnemy = true;
                                Enemy.CurHP = enemy.health;
                                break;
                            }
                        }
                        string enemyDialog = "Strong Opponent is approaching...";
                        if (foundEnemy)
                        {
                            foreach (var dialog in dialogStatic)
                            {
                                if (dialog.characterId == $"{enemiesStatic[enemyIndex]}")
                                {
                                    enemyDialog = dialog.content;
                                }
                            }
                        }
                        textbox.text = enemyDialog;
                        BattleButton.SetActive(true);
                    }
                    else
                    {
                        textbox.text = "Enemy Approaches, Time To Battle!";
                        BattleButton.SetActive(true);
                    }
                    break;
            }
        }
    }
    private void SetNodeColor(GameObject myObject, Color color)
    {
        SpriteRenderer rend = myObject.GetComponent<SpriteRenderer>();
        rend.color = color;
    }
    private void ClearMap()
    {
        for (int i=0; i<nodes.Length;i++)
        {
            for (int j=0; j < nodes[i].Length;j++)
            {
                MapNode currentNode = nodes[i][j];
                Destroy(currentNode.nodeObject);
                Destroy(currentNode.nodeEffect.NodeEffectIcon);
                for (int k=0; k < currentNode.linkObjects.Length; k++)
                {
                    for (int l=0; l < currentNode.linkObjects[k].Length; l++)
                    {
                        Destroy(currentNode.linkObjects[k][l]);
                    }
                }
            }
        }
    }

    private void GenerateMap(Vector3 startPos)
    {
        EncounterCounter = 0;
        int rowHeight = 4;
        int columnAmount = Random.Range(3, 5)+2;
        int nodeAmount = Random.Range(2, 5);
        float columnOffset = Mathf.Abs(startPos.x + startPos.x)/(float)(columnAmount-1);

        int[] startNodeLinks = new int[nodeAmount];
        for (int i = 0; i < nodeAmount; i++) { startNodeLinks[i] = i; }
        MapNode[] startNodeColumn = { new MapNode(startNodeLinks, startPos, LevelNode, MapIcon, MapIconList) };
        nodes = new MapNode[columnAmount][];
        nodes[0] = startNodeColumn;

        for (int i=0; i<columnAmount-2; i++) //iterate over map columns
        {
            int nextNodeAmount = Random.Range(2, 5);
            float rowOffset = (rowHeight * 2)/(float)(nodeAmount+1);
            MapNode[] nodesOfColumn = new MapNode[nodeAmount];
            for (int j = 0; j < nodeAmount; j++) //iterate over map nodes
            {
                List<int> linkIndices = new List<int>();
                for (int k = 0; k < nextNodeAmount-1; k++) //iterate over links
                {
                    if (Random.Range(0, 2) == 0) linkIndices.Add(k);
                }
                int[] linkindices = linkIndices.ToArray();
                if (linkindices.Length == 0) { linkIndices.Add(Random.Range(0, nextNodeAmount)); linkindices = linkIndices.ToArray(); } //If no links were added
                if (i == columnAmount-3){ linkindices = new int[1]; linkindices[0] = 0; }
                MapNode node = new MapNode(linkindices, new Vector3(startPos.x + columnOffset * (i+1), startPos.y + rowHeight - (rowOffset * (j+1)), startPos.z), LevelNode, MapIcon, MapIconList);
                nodesOfColumn[j] = node;
            }
            nodes[i + 1] = nodesOfColumn;
            nodeAmount = nextNodeAmount;
        }
        MapNode[] endNodeColumn = { new MapNode(new int[]{}, startPos+new Vector3((columnAmount-1)*columnOffset,0,0), LevelNode, MapIcon, MapIconList, 5) };
        nodes[columnAmount-1] = endNodeColumn;
        //Check that all nodes are connected to at least one other node. I am so sorry for this spagetti, I was tired
        for (int i = 2; i < nodes.Length; i++) //Iterate through all columns
        {
            for (int j = 0; j < nodes[i].Length; j++) // Iterate through the nodes of the column
            {
                bool isConnected = false;
                for (int k = 0; k < nodes[i - 1].Length; k++) //Iterate through the nodes of the previous column
                {
                    for (int l = 0; l < nodes[i - 1][k].linkIndices.Length; l++) //Iterate through the linkindices of the precious-column nodes
                    {
                        if (nodes[i - 1][k].linkIndices[l] == j)
                        {
                            isConnected = true;
                            break;
                        }
                    }
                    if (isConnected) { break; }
                }
                if (!isConnected)
                {
                    MapNode randomNode = nodes[i - 1][Random.Range(0, nodes[i - 1].Length)];
                    List<int> tempList = new List<int>();
                    tempList.AddRange(randomNode.linkIndices);
                    tempList.Add(j);
                    randomNode.linkIndices = tempList.ToArray();
                }
            }
        }
        currentColumn = 0;
        currentNode = 0;
        DrawNodeMap();
    }

    private void DrawNodeMap()
    {
        if (currentColumn != 0) //This happens only when regenerating map
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                for (int j = 0; j < nodes[i].Length; j++)
                {
                    print("Redrawn");
                    nodes[i][j].Redraw(LevelNode, MapIcon, MapIconList);
                }
            }
            foreach (Vector2 nodecoords in RedLane)
            {
                SetNodeColor(nodes[(int)nodecoords.x][(int)nodecoords.y].nodeObject, Color.red);
                Destroy(nodes[(int)nodecoords.x][(int)nodecoords.y].nodeEffect.NodeEffectIcon);
            }
        }
        RedLane.Add(new Vector2(0, 0));
        SetNodeColor(nodes[0][0].nodeObject, Color.red);
        Destroy(nodes[0][0].nodeEffect.NodeEffectIcon);
        for (int i = 0; i < nodes[currentColumn][currentNode].linkIndices.Length; i++)
        {
            SetNodeColor(nodes[currentColumn + 1][nodes[currentColumn][currentNode].linkIndices[i]].nodeObject, Color.green);
        }

        //Draw nodes & columns
        for (int i = 0; i < nodes.Length; i++)
        {
            for (int j = 0; j < nodes[i].Length; j++)
            {
                nodes[i][j].DrawLinks(i, Pathline);
            }
        }
    }
    public static GameObject[] DrawNodePath(Vector3 nodePos1, Vector3 nodePos2, GameObject pathLine)
    {
        Vector3 nodeDirection = nodePos2 - nodePos1;
        float nodeDirectionAngleX = Vector3.Angle(new Vector3(1, 0, 0), nodeDirection);
        float nodeDirectionAngleY = Vector3.Angle(new Vector3(0, 1, 0), nodeDirection);
        if (nodeDirectionAngleY > 90) { nodeDirectionAngleX = -nodeDirectionAngleX; }
        float nodeDist = nodeDirection.magnitude;
        int lineAmount = (int)Mathf.Ceil(nodeDist / 0.5f);
        GameObject[] nodeToNodePath = new GameObject[lineAmount];
        for (int j = 0; j < lineAmount; j++)
        {
            Vector3 linePos = nodePos1 + nodeDirection.normalized * (0.125f + 0.5f * j);
            GameObject newPathLine = Instantiate(pathLine, linePos + new Vector3(0, 0, 0.1f), Quaternion.identity);
            Transform transformer = newPathLine.GetComponent<Transform>();
            transformer.eulerAngles = new Vector3(0, 0, nodeDirectionAngleX);
            nodeToNodePath[j] = newPathLine;
        }
        return nodeToNodePath;
    }
}