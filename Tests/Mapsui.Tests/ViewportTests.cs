﻿using NUnit.Framework;

namespace Mapsui.Tests;

[TestFixture]
public class ViewportTests
{
    [Test]
    public void SetCenterTest()
    {
        // Arrange
        var navigator = new Navigator();

        // Act
        navigator.SetCenter(10, 20);

        // Assert
        Assert.AreEqual(10, navigator.Viewport.CenterX);
        Assert.AreEqual(20, navigator.Viewport.CenterY);
    }

    [Test]
    public void SetTransformDeltaResolution1()
    {
        // Arrange
        var navigator = new Navigator();

        // Act
        navigator.SetCenter(10, 20);
        navigator.Pinch(new MPoint(10, 10), new MPoint(20, 20), 1);

        // Assert
        Assert.AreEqual(20, navigator.Viewport.CenterX);
        Assert.AreEqual(10, navigator.Viewport.CenterY);
    }

    [Test]
    public void SetTransformDeltaResolution2()
    {
        // Arrange
        var navigator = new Navigator();

        // Act
        navigator.SetCenter(10, 20);
        navigator.Pinch(new MPoint(10, 10), new MPoint(20, 20), 2);

        // Assert
        Assert.AreEqual(30, navigator.Viewport.CenterX);
        Assert.AreEqual(0, navigator.Viewport.CenterY);
    }
}
