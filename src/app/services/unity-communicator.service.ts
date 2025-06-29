import { isPlatformBrowser } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, PLATFORM_ID, inject, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UnityCommunicatorService {
  modelsFetched = signal(false);
  canShowPreviousObject = signal(false);
  canShowNextObject = signal(false);
  objectTitle = signal('');
  objectUri = signal('');

  private readonly http = inject(HttpClient);
  private readonly communicatorServiceName = 'WebCommunicatorService';
  private readonly modelOverviewJsonUrl = './models/model-overview.json';

  private _unityInstance: any;

  constructor(@Inject(PLATFORM_ID) private platformId: Object) {
    if (isPlatformBrowser(this.platformId)) {
      (window as any).onUnityMessage =
        (param: string, value1: number | string, value2: number | string = '') => {
        this.handleUnityMessage(param, value1, value2);
      };
    }
  }

  init(instance: any): void {
    this._unityInstance = instance;

    this.sendModelOverviewJson();
    this.sendModelsPath();
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

  private sendModelsPath(): void {
    if (this._unityInstance) {
      this._unityInstance.SendMessage(this.communicatorServiceName, 'OnSendModelsPath',
        `./models/`);
    } else {
      console.error('Unity instance is not initialized. Cannot send models path.');
    }
  }

  private sendModelOverviewJson(): void {
    if (!this._unityInstance) {
      console.error('Unity instance is not initialized. Cannot send model overview JSON.');
      return;
    }

    this.http.get(this.modelOverviewJsonUrl, { responseType: 'text' }).subscribe({
      next: (jsonString: string) => {
        this._unityInstance.SendMessage(
          this.communicatorServiceName,
          'OnSendModelOverviewJson',
          jsonString
        );
      },
      error: (err) => {
        console.error('Error while loading the model overview JSON:', err);
      }
    });
  }

  private handleUnityMessage(param: string, value1: number | string,
                             value2: number | string = ''): void {
    switch (param) {
      case 'ModelsFetched':
        this.modelsFetched.set(true);
        break;
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
