using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PathfinderTests
{
    List<OverlayTile> tileList = new();
    private void Create_Square_Tilemap()
    {
        tileList.Clear();
        // Use the Assert class to test conditions.
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                GameObject overlayTileObject = new GameObject();

                OverlayTile overlayTile = overlayTileObject.AddComponent<OverlayTile>();

                overlayTile.gridLocation = new Vector3Int(i, j, 0);

                tileList.Add(overlayTile);
            }
        }
    }

    private void Create_Diagonal_Tilemap()
    {
        tileList.Clear();
        for (int i = 0; i < 10; i++)
        {
            GameObject overlayTileObject = new GameObject();

            OverlayTile overlayTile = overlayTileObject.AddComponent<OverlayTile>();

            overlayTile.gridLocation = new Vector3Int(i, i, 0);

            tileList.Add(overlayTile);
        }
    }

    private void Create_ZigZag_Tilemap()
    {
        tileList.Clear();
        for (int i = 0; i < 10; i++)
        {
            GameObject overlayTileObject1 = new GameObject();

            OverlayTile overlayTile1 = overlayTileObject1.AddComponent<OverlayTile>();

            overlayTile1.gridLocation = new Vector3Int(i, i, 0);

            if (!tileList.Contains(overlayTile1))
            {
                tileList.Add(overlayTile1);
            }
            GameObject overlayTileObject2 = new GameObject();

            OverlayTile overlayTile2 = overlayTileObject2.AddComponent<OverlayTile>();
            overlayTile2.gridLocation = new Vector3Int(i, i + 1, 0);

            if (!tileList.Contains(overlayTile2))
            {
                tileList.Add(overlayTile2);
            }
        }
    }

    private void Create_SquareBox_Tilemap()
    {
        tileList.Clear();

        for (int i = 0; i < 10; i++)
        {
            GameObject overlayTileObject1 = new GameObject();

            OverlayTile overlayTile1 = overlayTileObject1.AddComponent<OverlayTile>();

            overlayTile1.gridLocation = new Vector3Int(i, 0, 0);
            if (!tileList.Contains(overlayTile1))
            {
                tileList.Add(overlayTile1);
            }
            GameObject overlayTileObject2 = new GameObject();

            OverlayTile overlayTile2 = overlayTileObject2.AddComponent<OverlayTile>();
            overlayTile2.gridLocation = new Vector3Int(i, 9, 0);
            if (!tileList.Contains(overlayTile2))
            {
                tileList.Add(overlayTile2);
            }
           
        }

        for (int i = 1; i < 9; i++)
        {
            GameObject overlayTileObject1 = new GameObject();

            OverlayTile overlayTile1 = overlayTileObject1.AddComponent<OverlayTile>();

            overlayTile1.gridLocation = new Vector3Int(9, i, 0);
            if (!tileList.Contains(overlayTile1))
            {
                tileList.Add(overlayTile1);
            }
            GameObject overlayTileObject2 = new GameObject();

            OverlayTile overlayTile2 = overlayTileObject2.AddComponent<OverlayTile>();
            overlayTile2.gridLocation = new Vector3Int(0, i, 0);
            if (!tileList.Contains(overlayTile2))
            {
                tileList.Add(overlayTile2);
            }

        }
    }
    // A Test behaves as an ordinary method
    [Test]
    public void Tilemap_Population_Test_1()
    {
        Create_Square_Tilemap();

        Assert.AreEqual(tileList.Count, 100);
    }

    [Test]
    public void Pathfinding_Test_Square_1()
    {
        Create_Square_Tilemap();

        OverlayTile start = tileList[0]; // Coordinate [0, 0]
        OverlayTile destination = tileList[99]; // Coordinate [9, 9]

        PathFinder testPF = new();
        List<OverlayTile> path = testPF.FindPath(start, destination, tileList, 1);

        Assert.AreEqual(path.Count, 18);
    }

    [Test]
    public void Pathfinding_Test_NoPath()
    {


        OverlayTile start = tileList[5];

        PathFinder testPF = new();
        List<OverlayTile> path = testPF.FindPath(start, start, tileList, 1);

        Assert.AreEqual(path.Count, 0);
    }

    [Test]
    public void Pathfinding_Test_Diagonal_1()
    {
        Create_Diagonal_Tilemap();

        OverlayTile start = tileList[0];
        OverlayTile destination = tileList[9];
        PathFinder testPF = new();
        List<OverlayTile> path = testPF.FindPath(start, destination, tileList, 1);

        Assert.AreEqual(0, path.Count);
    }
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [Test]
    public void Pathfinding_Test_ZigZag_1()
    {
        Create_ZigZag_Tilemap();

        OverlayTile start = tileList[0];
        OverlayTile destination = tileList[tileList.Count - 1];
        PathFinder testPF = new();
        List<OverlayTile> path = testPF.FindPath(start, destination, tileList, 1);
        
        Assert.AreEqual(19, path.Count);
        
    }

    [Test]

    public void Pathfinding_Test_SquareBox_1()
    {
        Create_SquareBox_Tilemap();

        OverlayTile start = tileList.Find(x => x.gridLocation.Equals(new Vector3Int(3, 0)));
        OverlayTile destination = tileList.Find(x => x.gridLocation.Equals(new Vector3Int(3, 9)));

        PathFinder testPF = new();
        List<OverlayTile> path = testPF.FindPath(start, destination, tileList, 1);

        Assert.AreEqual(15, path.Count);
    }
}
