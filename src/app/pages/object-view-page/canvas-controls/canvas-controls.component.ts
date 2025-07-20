import { Component, inject } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatDividerModule } from '@angular/material/divider';
import { MatTooltipModule } from '@angular/material/tooltip';
import { UnityCommunicatorService } from '../../../services/unity-communicator.service';

@Component({
  selector: 'app-canvas-controls',
  imports: [MatButtonModule, MatIconModule, MatDividerModule, MatTooltipModule],
  templateUrl: './canvas-controls.component.html',
  styleUrl: './canvas-controls.component.scss'
})
export class CanvasControlsComponent {
  readonly githubUrl = 'https://github.com/AlessandriniP/3d-model-viewer';
  readonly unityCommunicatorService = inject(UnityCommunicatorService);

  previousObject(): void {
    this.unityCommunicatorService.showPreviousObject();
  }

  nextObject(): void {
    this.unityCommunicatorService.showNextObject();
  }

  focusObject(): void {
    this.unityCommunicatorService.resetView();
  }
}
