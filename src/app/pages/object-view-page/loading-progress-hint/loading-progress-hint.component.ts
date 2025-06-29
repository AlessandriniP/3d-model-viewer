import { Component, input } from '@angular/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-loading-progress-hint',
  imports: [MatProgressSpinnerModule],
  templateUrl: './loading-progress-hint.component.html',
  styleUrl: './loading-progress-hint.component.scss'
})
export class LoadingProgressHintComponent {
  readonly hintText = input.required<string>();
}
