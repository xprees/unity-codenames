# Unity Codenames Package - cz.xprees.codenames

[![NPM Version](https://img.shields.io/npm/v/cz.xprees.codenames)](https://www.npmjs.com/package/cz.xprees.codenames)

Lightweight, deterministic codename generator in the style of Docker container names (`kind-edison`, `funny-whale`). Use
it to log or display a
stable label instead of free-text user input (for example a typed player name), so no personal data ends up in logs and
analytics.

- Plain C#, no dependencies, no Unity engine references (works on every platform incl. WebGL).
- Deterministic: the same seed always gives the same codename, on every platform and version.
- No allocations besides the result string, no assets, no files to load.
- 108 adjectives x 238 names = 25 704 combinations. Collisions are expected for large populations - a codename is a
  label, **not** an id.

## Installation

Install the package using npm scoped registry in `Project Settings > Package Manager > Scoped Registries` (For more
details
see [Unity Docs - Use a scoped registry in your project](https://docs.unity3d.com/6000.2/Documentation/Manual/upm-scoped-use.html))

`Packages/manifest.json`

```json
{
    "scopedRegistries": [
        {
            "name": "NPM - xprees",
            "url": "https://registry.npmjs.org",
            "scopes": [
                "cz.xprees"
            ]
        }
    ]
}
```

## Usage

```csharp
using Xprees.Codenames;

var codename = Codename.FromSeed(playerId); // e.g. "kind-edison"
```

A null or empty seed returns `Codename.Fallback` (`anonymous`).

## Privacy notes

- Seed it with a **random** id (e.g. a UUID). Never seed it with user input or data derived from it (name, email, form
  id): the word lists are
  small, so such a codename could be brute-forced back to the input.
- A codename derived from a stable id is pseudonymous (the same player is recognizable across sessions), not anonymous.
- The word lists are append-never: changing or reordering words renames every existing player.

## License

Apache-2.0, see `LICENSE.md`. The word lists are adapted from Docker (Moby), see `NOTICE.md`.
