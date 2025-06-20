import { isPlatformBrowser } from '@angular/common';
import { Inject, Injectable, PLATFORM_ID, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UnityCommunicatorService {
  canShowPreviousObject = signal(false);
  canShowNextObject = signal(false);
  objectTitle = signal('');
  objectUri = signal('');

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

  private handleUnityMessage(param: string, value1: number | string, value2: number | string = ''): void {
    switch (param) {
      case 'CanGoPrevious':
        this.canShowPreviousObject.set(value1 === 1);
        break;
      case 'CanGoNext':
        this.canShowNextObject.set(value1 === 1);
        break;
      case 'ObjectDescription':
        this.objectUri.set(value1 as string);
        this.objectTitle.set(value2 as string);
        break;
      default:
        console.error(`Unknown Unity message parameter: ${param}`);
    }
  }
}
