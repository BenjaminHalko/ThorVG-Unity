# ThorVG-Unity
✨ Bringing Lottie Animations to Unity using [ThorVG](https://github.com/thorvg/thorvg)!

![Melting](https://github.com/user-attachments/assets/847718be-41db-4579-863b-b8951bae1e1c)

## ⚠️ Disclaimer
This is to a Proof of Concept, there are still many features missing such as:
- Better animation controls
  - Speed
  - Loop Points
  - Pause / Resume
- Able to easily swap animations on the fly

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

## Demo

https://github.com/user-attachments/assets/ddbdddd8-1cba-4316-9e83-3bcccfc16cec
