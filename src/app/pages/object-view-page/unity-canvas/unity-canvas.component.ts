import { AfterViewInit, Component, Signal, input, output } from '@angular/core';

@Component({
  selector: 'app-unity-canvas',
  imports: [],
  templateUrl: './unity-canvas.component.html',
  styleUrl: './unity-canvas.component.scss'
})
export class UnityCanvasComponent implements AfterViewInit {
  canvasReady = output<void>();

  readonly unityCanvasId = 'unity-canvas';
  readonly unityBuildPath = 'unity-build/unity-build';

  private unityInstance: any; // TODO: use this to send messages to Unity

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
      }).then((instance: any) => {
        this.unityInstance = instance;
        this.canvasReady.emit();
      });
    }
    catch {
      // ignore exceptions
    }
  }
}
