# Lottity
✨ Bringing Lottie Animations to Unity using [ThorVG](https://github.com/thorvg/thorvg)!

![Melting](https://github.com/user-attachments/assets/847718be-41db-4579-863b-b8951bae1e1c)

## How to Use

- Add the `LottieSprite` component to any object. It will then be rendered using `SpriteComponent`. It's that simple!

## Supported Platforms

- Windows
- MacOS

## Development

### Compiling ThorVG

ThorVG has already been included in the plugin, but in case you want to build it yourself, here's how:

- Install [Meson](https://mesonbuild.com/Getting-meson.html) and [Ninja](https://ninja-build.org)

  - Windows: `pip install meson ninja`
  - MacOS: `brew install meson`
  - Linux: `sudo apt install meson`

- Download ThorVG:

```bash
git clone https://github.com/thorvg/thorvg.git
cd thorvg
git checkout v1.0-pre8
```

- Compile ThorVG:
  
```bash
meson setup builddir -Dbindings="capi" --wipe
meson compile -C builddir
```

- Copy the built library from `builddir/src` to `Assets/Lottie/Plugins` in your Unity Project
