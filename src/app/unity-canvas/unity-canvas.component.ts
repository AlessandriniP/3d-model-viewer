import { AfterViewInit, Component } from '@angular/core';

@Component({
  selector: 'app-unity-canvas',
  imports: [],
  templateUrl: './unity-canvas.component.html',
  styleUrl: './unity-canvas.component.scss'
})
export class UnityCanvasComponent implements AfterViewInit {
  readonly unityCanvasId = 'unity-canvas';
  readonly unityBuildPath = 'unity-build/unity-build';

  ngAfterViewInit(): void {
    try {
      // @ts-ignore
      createUnityInstance(document.querySelector(`#${this.unityCanvasId}`), {
        dataUrl: `${this.unityBuildPath}.data`,
        frameworkUrl: `${this.unityBuildPath}.framework.js`,
        codeUrl: `${this.unityBuildPath}.wasm`,
        streamingAssetsUrl: 'StreamingAssets',
        companyName: 'AlessandriniP',
        productName: '3d-model-viewer',
        productVersion: '1.0'
      });
    }
    catch {
      // ignore exceptions
    }
  }
}
