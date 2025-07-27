# 3D Model Viewer

[![Angular](https://img.shields.io/badge/Angular-20.0.3-red?logo=angular)](https://angular.dev/)
[![Unity](https://img.shields.io/badge/Unity-6000.0.53f1-blue?logo=unity)](https://unity.com/)

## Links

- [**GitHub Pages**](https://alessandrinip.github.io/3d-model-viewer/)
- [**Releases**](https://github.com/AlessandriniP/3d-model-viewer/releases)

## Project Structure

- **Unity project location:** [`unity-src/3d-model-viewer`](unity-src/3d-model-viewer)
- **Unity WebGL build:** Should be placed in `public/unity-build` (uncompressed)
    - For reference, see [`docs/unity-build`](docs/unity-build/)
- **GLTF models:** Should be placed in `public/models` with a corresponding `model-overview.json` file that keeps all model file names which should be viewable in the app
    - For reference, see [`docs/models`](docs/models/)

## Building and Running

Install all dependencies first and build the project:

1. **Install dependencies:**
```
npm install
```

2. **Build the project**
```
npm run build
```

<br>

Then, run the application:

- **Run web app:**
```
npm run serve
```

- **Run desktop app:**
```
npm run electron
```

- **Package desktop app executable:**
```
npm run package
```

---

For more details about the build artifacts, see [`docs`](docs) folder.
