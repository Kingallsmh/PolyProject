using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    public static BrickManager Instance;

    public BreakBrick brickPrefab;
    public Vector3 playerAreaSize = new Vector3(1, 1, 1);
    public float playerEnd = 28f;

    List<BreakBrick> p1BrickList, p2BrickList;
    Vector3 brickDimensions = new Vector3(1, 1, 1);

    private void Awake()
    {
        Instance = this;
    }

    //private void Start()
    //{
    //    GeneratePlayerBricks(0, new Vector3(4, 1, 1), Color.green);
    //    GeneratePlayerBricks(1, new Vector3(4, 1, 1), Color.yellow);
    //}

    public void SetBrickDimensions(Vector3 _brickSize)
    {
        brickDimensions = _brickSize;
    }

    public void GeneratePlayerBricks(int _playerNum, Color _col)
    {        
        if(_playerNum == 0)
        {
            CreateBrickFence(-playerEnd, _col, p1BrickList);
        }
        else
        {
            CreateBrickFence(playerEnd, _col, p2BrickList);
        }
    }

    public void CreateBrickFence(float _zPos, Color _col, List<BreakBrick> _brickList)
    {
        _brickList = new List<BreakBrick>();
        float incX = brickDimensions.x / 2;
        float incZ = brickDimensions.z / 2;
        int rowAmount = (int)playerAreaSize.x / (int)brickDimensions.x;
        int colAmount = (int)playerAreaSize.z / (int)brickDimensions.z;
        
        for(int x = 0; x < rowAmount; x++)
        {
            for (int z = 0; z < colAmount; z++)
            {
                float xPos = incX + (-playerAreaSize.x / 2) + brickDimensions.x * x;
                float zPos = incZ + _zPos + (-playerAreaSize.z / 2) + brickDimensions.z * z;
                BreakBrick b = Instantiate(brickPrefab, new Vector3(xPos, 0, zPos), Quaternion.identity);
                b.CreateBrick(_col, brickDimensions);
                NetworkServer.Spawn(b.gameObject);
                _brickList.Add(b);
            }
        }
    }

    public void RemoveBricks(List<BreakBrick> _list)
    {
        for(int i = 0; i < _list.Count; i++)
        {
            NetworkServer.Destroy(_list[i].gameObject);
        }
    }
}
