import { Component, signal } from '@angular/core';
import { UnityCanvasComponent } from './unity-canvas/unity-canvas.component';
import { CanvasControlsComponent } from './canvas-controls/canvas-controls.component';
import { CanvasLoadingHintComponent } from './canvas-loading-hint/canvas-loading-hint.component';

@Component({
  selector: 'app-object-view-page',
  imports: [UnityCanvasComponent, CanvasControlsComponent, CanvasLoadingHintComponent],
  templateUrl: './object-view-page.component.html',
  styleUrl: './object-view-page.component.scss'
})
export class ObjectViewPageComponent {
  canvasReady: boolean = false;

  setCanvasReady(): void {
    this.canvasReady = true;
  }
}
