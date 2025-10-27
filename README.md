# ThorVG-Unity

✨ Bringing Lottie Animations to Unity using [ThorVG](https://github.com/thorvg/thorvg)!

![Melting](https://github.com/user-attachments/assets/847718be-41db-4579-863b-b8951bae1e1c)

## How to Use

- Add the `TvgPlayer` component to any object. It will then be rendered using `SpriteComponent`. It's that simple!

## Supported Platforms

- Windows
- MacOS
- Linux

## Development

### Compiling ThorVG

ThorVG has already been included in the plugin, but in case you want to build it yourself, here's how:

- Install [Meson](https://mesonbuild.com/Getting-meson.html) and [Ninja](https://ninja-build.org)

  - Windows: `pip install meson ninja`
  - MacOS: `brew install meson`
  - Linux: `sudo apt install meson`

- Download & Compile ThorVG:

```bash
git clone https://github.com/thorvg/thorvg.git --depth 1 --branch v1.0-pre30
cd thorvg
meson setup build -Dbindings=capi -Dloaders="lottie,svg,png,jpg,webp" -Dthreads=false -Dfile=false -Dpartial=false -Dextra= -Dbuildtype=release
meson compile -C build
```

- Copy the built library from `build/src` to `ThorVG-Unity/Plugins` in your Unity Project
