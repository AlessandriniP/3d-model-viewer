import { Component } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';

@Component({
  selector: 'app-canvas-controls',
  imports: [MatButtonModule, MatIconModule, MatDividerModule],
  templateUrl: './canvas-controls.component.html',
  styleUrl: './canvas-controls.component.scss'
})
export class CanvasControlsComponent {
  previousObject(): void {
    console.log('Show previous object');
  }

  nextObject(): void {
    console.log('Show next object');
  }

  focusObject(): void {
    console.log('Focus on the current object');
  }

  downloadObject(): void {
    console.log('Download the current object');
  }

  openProjectSource(): void {
    console.log('Open the project source in a new tab');
  }
}
