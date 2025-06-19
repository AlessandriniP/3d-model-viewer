import { isPlatformBrowser } from '@angular/common';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UnityCommunicatorService {
  private readonly communicatorServiceName = 'WebCommunicatorService';

  private _unityInstance: any;

  constructor(@Inject(PLATFORM_ID) private platformId: Object) {
    if (isPlatformBrowser(this.platformId)) {
      (window as any).onUnityMessage = (message: string) => {
        this.handleUnityMessage(message);
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

  private handleUnityMessage(message: string): void {
    console.log('Received message from Unity:', message);
  }
}
