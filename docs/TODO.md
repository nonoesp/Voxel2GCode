# TODO

- [ ] Fix `Force Top Layer` in `Slice Planes` component.

## Slicer additions (tentative)

```csharp
List<Plane> V2GBoundingFrames(GeometryBase g, double angles) {
    List<Plane> frames = new List<Plane>();
    // ...
    return frames;
}
```

```csharp
/// <summary>
/// Creates infill curves (weaved/stitched already?)
/// </summary>
/// <param name="g"></param>
/// <returns></returns>
/// Two possible approaches:
/// a. The surface has cavities cut already.
/// b. The surface is an outline, and cavities are later used to trim
///   (which would need different parameters on the function)
List<Curve> V2GInfillCurves(GeometryBase g)
{
    List<Curve> infill = new List<Curve>();
    // ...
    return infill;
}
```
