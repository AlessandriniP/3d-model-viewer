import { Routes } from '@angular/router';
import { UnityCanvasComponent } from './unity-canvas/unity-canvas.component';

export const routes: Routes = [
  {
    path: '',
    title: '3D Model Viewer',
    component: UnityCanvasComponent
  }
];
