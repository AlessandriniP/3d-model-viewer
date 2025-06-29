import { Component, effect, inject, signal } from '@angular/core';
import { UnityCanvasComponent } from './unity-canvas/unity-canvas.component';
import { CanvasControlsComponent } from './canvas-controls/canvas-controls.component';
import { LoadingProgressHintComponent } from './loading-progress-hint/loading-progress-hint.component';
import { UnityCommunicatorService } from '../../services/unity-communicator.service';

@Component({
  selector: 'app-object-view-page',
  imports: [UnityCanvasComponent, CanvasControlsComponent, LoadingProgressHintComponent],
  templateUrl: './object-view-page.component.html',
  styleUrl: './object-view-page.component.scss'
})
export class ObjectViewPageComponent {
  canvasReady: boolean = false;
  hideFetchingHint = false;
  
  private readonly unityCommunicatorService = inject(UnityCommunicatorService);

  constructor() {
    effect(() => {
      if (this.unityCommunicatorService.modelsFetched()) {
        this.hideFetchingHint = true;
      }
    });
  }

  setCanvasReady(): void {
    this.canvasReady = true;
  }
}
