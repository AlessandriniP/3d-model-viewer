import { Component, inject } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { UnityCommunicatorService } from '../../../services/unity-communicator.service';

@Component({
  selector: 'app-canvas-controls',
  imports: [MatButtonModule, MatIconModule, MatDividerModule],
  templateUrl: './canvas-controls.component.html',
  styleUrl: './canvas-controls.component.scss'
})
export class CanvasControlsComponent {
  private readonly unityCommunicatorService = inject(UnityCommunicatorService);

  previousObject(): void {
    this.unityCommunicatorService.showPreviousObject();
  }

  nextObject(): void {
    this.unityCommunicatorService.showNextObject();
  }

  focusObject(): void {
    this.unityCommunicatorService.resetView();
  }

  downloadObject(): void {
    console.log('Download the current object');
  }

  openProjectSource(): void {
    console.log('Open the project source in a new tab');
  }
}
