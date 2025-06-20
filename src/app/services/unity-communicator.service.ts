import { isPlatformBrowser } from '@angular/common';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UnityCommunicatorService {
  canShowPreviousObject = new BehaviorSubject<boolean>(false);
  canShowNextObject = new BehaviorSubject<boolean>(false);

  private readonly communicatorServiceName = 'WebCommunicatorService';

  private _unityInstance: any;

  constructor(@Inject(PLATFORM_ID) private platformId: Object) {
    if (isPlatformBrowser(this.platformId)) {
      (window as any).onUnityMessage = (param: string, value: number) => {
        this.handleUnityMessage(param, value);
      };
    }
  }

  set unityInstance(instance: any) {
    this._unityInstance = instance;
  }

  showNextObject(): void {
    if (this._unityInstance) {
      this._unityInstance.SendMessage(this.communicatorServiceName, 'OnShowNextObject');
    } else {
      console.error('Unity instance is not initialized. Cannot send message to show next object.');
    }
  }

  showPreviousObject(): void {
    if (this._unityInstance) {
      this._unityInstance.SendMessage(this.communicatorServiceName, 'OnShowPreviousObject');
    } else {
      console.error('Unity instance is not initialized. Cannot send message to show previous object.');
    }
  }

  resetView(): void {
    if (this._unityInstance) {
      this._unityInstance.SendMessage(this.communicatorServiceName, 'OnResetView');
    } else {
      console.error('Unity instance is not initialized. Cannot reset camera view.');
    }
  }

  private handleUnityMessage(param: string, value: number): void {
    switch (param) {
      case 'CanGoPrevious':
        this.canShowPreviousObject.next(value === 1);
        break;
      case 'CanGoNext':
        this.canShowNextObject.next(value === 1);
        break;
      default:
        console.error(`Unknown Unity message parameter: ${param}`);
    }
  }
}
