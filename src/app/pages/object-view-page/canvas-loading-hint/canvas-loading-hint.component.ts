import { Component } from '@angular/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-canvas-loading-hint',
  imports: [MatProgressSpinnerModule],
  templateUrl: './canvas-loading-hint.component.html',
  styleUrl: './canvas-loading-hint.component.scss'
})
export class CanvasLoadingHintComponent {

}
