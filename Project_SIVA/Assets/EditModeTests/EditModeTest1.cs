using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class EditModeTest1
{
    // A Test behaves as an ordinary method
    [Test]
    public void EditModeTest1SimplePasses()
    {
        // Use the Assert class to test conditions
        int x = 5;
        
        Assert.AreEqual(x, 5);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator EditModeTest1WithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
