# Release Notes

## 0.1.2 (master)

This is the current version in progress.

- [X] Change ZOffset default to 0.0.
- [X] [Core] Removed redundant G-code. (Coordinates and speeds only appear on G1 commands when they change, which reduces file sizes.)
- [X] [Core] Fixed resetting head comparison, now it checks for absolute different between Z coordinates rather than comparing them (this was returning false for a difference of 0.000000001 in coordinates).
- [X] [Core] EndPoint behavior fixed to incremental positioning (G91).

## 0.1.1

- [X] [Core] Reset E value at Z level change.
- [X] [Core] V2G Reset E value per Z.
- [X] [Core] Fixed bug were ZOffset was cumulative over points.
- [X] [GH] Verbose setting transferred to printing settings component.

## 0.1.0

- [X] First released version.
