using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayModeTest1
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
    // A Test behaves as an ordinary method
    [Test]
    public void Tilemap_Population_Test_1()
    {
        Create_Square_Tilemap();

        Assert.AreEqual(tileList.Count, 100);
    }

    [Test]
    public void Tilemap_Pathfinding_Test_1()
    {
        Create_Square_Tilemap();

        OverlayTile start = tileList[0]; // Coordinate [0, 0]
        OverlayTile destination = tileList[99]; // Coordinate [9, 9]

        PathFinder testPF = new();
        List<OverlayTile> path = testPF.FindPath(start, destination, tileList, 1);

        Assert.AreEqual(path.Count, 18);
    }
    [Test]
    public void Tilemap_Pathfinding_Test_2()
    {
        

        OverlayTile start = tileList[5];

        PathFinder testPF = new();
        List<OverlayTile> path = testPF.FindPath(start, start, tileList, 1);

        Assert.AreEqual(path.Count, 0);
    }
    [Test]
    public void Tilemap_Pathfinding_Test_3()
    {
        Create_Diagonal_Tilemap();

        OverlayTile start = tileList[0];
        OverlayTile destination = tileList[9];
        PathFinder testPF = new();
        List<OverlayTile> path = testPF.FindPath(start, destination, tileList, 1);

        Assert.AreEqual(path.Count, 0);
    }
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator PlayModeTest1WithEnumeratorPasses()
    {
        // Use yield to skip a frame.
        yield return null;
    }
}
