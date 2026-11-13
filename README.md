# Snake, refactored

A refactoring exercise. The starting point is a console Snake game from Code Review Stack
Exchange: 170 lines in a single `Main` method, everything in one place. This repository keeps that
original next to a restructured version so the two can be read side by side.

The game itself is unchanged — same rules, same console rendering, same feel. Only the structure
is different.

## Origin and licence

The original code comes from [codereview.stackexchange.com/q/127515](https://codereview.stackexchange.com/q/127515),
posted by Wagacca and modified by the community, retrieved on 2026-02-12 and licensed
**CC BY-SA 3.0**.

This refactoring is a derivative work, so it is distributed under the same licence. See
[LICENSE](LICENSE). The attribution header is kept at the top of both `snake.cs` and
`Snake project/snakeCleaner.cs`.

## What changed

`snake.cs` does everything inside `Main`: console setup, input polling, collision checks, tail
management, drawing and the game-over screen, interleaved in one loop with the state held in local
variables.

The restructured version splits that into types with one responsibility each:

| Type | Responsibility |
| --- | --- |
| `Program` | Game loop and setup constants |
| `GameBounds` | Playfield dimensions, clamped to what the console can display |
| `Position` | An (x, y) pair |
| `Direction` | Up, down, left, right |
| `SnakeEntity` | Head position, tail, movement, tail trimming |
| `Berry` | The berry and where it spawns |
| `GameState` | Score and game-over flag |
| `InputHandler` | Key polling during a tick, direction validation |
| `Renderer` | All console drawing |

Two things came out of the split that are worth noting.

`InputHandler.CaptureDirectionForTick` polls for the whole duration of a tick instead of reading
one key at the top of the loop, and rejects a reversal into the snake's own neck. In the original
both behaviours were tangled into the main loop and easy to get wrong.

`GameBounds.Create` is a factory rather than a constructor because the requested size has to be
reconciled with the actual console window, which can refuse it. Making that a named operation puts
the negotiation somewhere visible instead of leaving it inline in `Main`.

## Running it

Requires the .NET SDK.

```bash
cd "Snake project"
dotnet run
```

Arrow keys to steer. The game ends on a wall or on the snake's own tail.

The original version is kept as `snake.cs` at the repository root for comparison; it is not part
of the build.
