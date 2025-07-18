import { AfterViewInit, Component, ElementRef, OnDestroy, Signal, ViewChild, inject, input, output, viewChild } from '@angular/core';
import { UnityCommunicatorService } from '../../../services/unity-communicator.service';

@Component({
  selector: 'app-unity-canvas',
  imports: [],
  templateUrl: './unity-canvas.component.html',
  styleUrl: './unity-canvas.component.scss'
})
export class UnityCanvasComponent implements AfterViewInit, OnDestroy {
  canvasRef = viewChild.required<ElementRef<HTMLCanvasElement>>('unityCanvas');
  canvasReady = output<void>();

  readonly unityCanvasId = 'unity-canvas';
  readonly unityBuildPath = 'unity-build/unity-build';

  private readonly unityCommunicatorService = inject(UnityCommunicatorService);
  private resizeHandler?: () => void;

  ngAfterViewInit(): void {
    const unityCanvas = this.canvasRef().nativeElement;

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
        unityCanvas.style.cursor = 'crosshair';
        this.unityCommunicatorService.init(instance);
        this.canvasReady.emit();

        this.registerWebGLToCanvasResizeHandler(instance);
      });
    }
    catch {
      // ignore exceptions
    }
  }

  ngOnDestroy(): void {
    if (this.resizeHandler) {
      window.removeEventListener('resize', this.resizeHandler);
    }
  }

  private registerWebGLToCanvasResizeHandler(unityInstance: any): void {
    const unityModule = unityInstance.Module;
    let resizeTimeout: number;

    this.resizeHandler = () => {
      if (unityModule) {
        unityModule.matchWebGLToCanvasSize = false;
      }
      clearTimeout(resizeTimeout);
      resizeTimeout = window.setTimeout(() => {
        if (unityModule) {
          unityModule.matchWebGLToCanvasSize = true;
        }
      }, 100);
    };

    window.addEventListener('resize', this.resizeHandler);
  }
}
