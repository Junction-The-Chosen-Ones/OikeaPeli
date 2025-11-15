using UnityEngine;
using System.Collections.Generic;

public class NodeManager : MonoBehaviour
{
    int currentColumn = 0;
    int currentNode = 0;
    public static MapNode[][] nodes; //Array of arrays(columns of nodes)

    [SerializeField] private GameObject LevelNode;
    [SerializeField] private GameObject Pathline;

    public class MapNode
    {
        public int[] linkIndices;
        public Vector3 nodePos;
        public GameObject nodeObject;
        public GameObject[][] linkObjects;
        public MapNode(int[] linkindices, Vector3 nodepos, GameObject levelNode)
        {
            linkIndices = linkindices;
            nodePos = nodepos;
            nodeObject = Instantiate(levelNode, nodePos, Quaternion.identity);
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateMap(new Vector3(-4, 0, -1));
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < nodes[currentColumn][currentNode].linkIndices.Length; i++)
        {
            int linkedNodeIndex = nodes[currentColumn][currentNode].linkIndices[i];
            MapNode linkedNode = nodes[currentColumn + 1][linkedNodeIndex];
            if (linkedNode.nodeObject.GetComponent<BasicLevelClick>().GetClick()) //Click found on a linked nodes
            {
                for (int j = 0; j < nodes[currentColumn+1].Length; j++)
                {
                    SetNodeColor(nodes[currentColumn+1][j].nodeObject, Color.gray);
                }
                currentNode = linkedNodeIndex;
                currentColumn++;
                SetNodeColor(nodes[currentColumn][currentNode].nodeObject, Color.red);
                if (currentColumn == nodes.Length)
                {
                    GenerateMap(new Vector3(-4, 0, -1));
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

    private void SetNodeColor(GameObject myObject, Color color)
    {
        SpriteRenderer rend = myObject.GetComponent<SpriteRenderer>();
        rend.color = color;
    }

    private void GenerateMap(Vector3 startPos)
    {
        int rowHeight = 3;
        int columnAmount = Random.Range(2, 5)+2;
        int nodeAmount = Random.Range(2, 5);
        float columnOffset = Mathf.Abs(startPos.x + startPos.x)/(float)(columnAmount-1);

        int[] startNodeLinks = new int[nodeAmount];
        for (int i = 0; i < nodeAmount; i++) { startNodeLinks[i] = i; }
        MapNode[] startNodeColumn = { new MapNode(startNodeLinks, startPos, LevelNode) };
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
                MapNode node = new MapNode(linkindices, new Vector3(startPos.x + columnOffset * (i+1), startPos.y + rowHeight - (rowOffset * (j+1)), startPos.z), LevelNode);
                nodesOfColumn[j] = node;
            }
            nodes[i + 1] = nodesOfColumn;
            nodeAmount = nextNodeAmount;
        }
        MapNode[] endNodeColumn = { new MapNode(new int[]{}, startPos+new Vector3((columnAmount-1)*columnOffset,0,0), LevelNode) };
        nodes[columnAmount-1] = endNodeColumn;
        //Check that all nodes are connected to at least one other node. I am so sorry for this, I was tired
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
        SetNodeColor(nodes[0][0].nodeObject, Color.red);
        for (int i = 0; i < nodes[currentColumn][currentNode].linkIndices.Length; i++)
        {
            SetNodeColor(nodes[currentColumn + 1][nodes[currentColumn][currentNode].linkIndices[i]].nodeObject, Color.green);
        }
        DrawNodeMap();
    }

    private void DrawNodeMap()
    {
        currentColumn = 0;
        currentNode = 0;

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