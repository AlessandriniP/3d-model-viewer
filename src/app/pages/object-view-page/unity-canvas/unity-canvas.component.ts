import { AfterViewInit, Component, ElementRef, inject, NgZone, output, viewChild } from '@angular/core';
import { UnityCommunicatorService } from '../../../services/unity-communicator.service';

@Component({
  selector: 'app-unity-canvas',
  imports: [],
  templateUrl: './unity-canvas.component.html',
  styleUrl: './unity-canvas.component.scss'
})
export class UnityCanvasComponent implements AfterViewInit {
  canvasRef = viewChild.required<ElementRef<HTMLCanvasElement>>('unityCanvas');
  canvasReady = output<void>();

  readonly unityCanvasId = 'unity-canvas';
  readonly unityBuildPath = 'unity-build/unity-build';

  private readonly unityCommunicatorService = inject(UnityCommunicatorService);
  private readonly ngZone = inject(NgZone);

  ngAfterViewInit(): void {
    const unityCanvas = this.canvasRef().nativeElement;

    this.ngZone.runOutsideAngular(() => {
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
          this.ngZone.run(() => {
            this.unityCommunicatorService.init(instance);
            this.canvasReady.emit();
          });
        });
      }
      catch {
        // ignore exceptions
      }
    });
  }
}
