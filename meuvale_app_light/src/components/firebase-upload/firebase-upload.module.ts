import { NgModule, ModuleWithProviders, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { _FirbaseUploadConfig, FirbaseUploadConfig } from './models';
import { HttpClientModule } from '@angular/common/http';
import { Camera } from '@ionic-native/camera';
import { FormsModule } from '@angular/forms';
import { FirebaseUploaderComponent } from './uploader/uploader';

@NgModule({
  declarations: [FirebaseUploaderComponent],
  exports: [FirebaseUploaderComponent],
  imports: [
    CommonModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [
      Camera
  ],
  schemas: [
    CUSTOM_ELEMENTS_SCHEMA 
  ]
})
export class FirebaseUploadModule { 
  static forRoot(config: _FirbaseUploadConfig): ModuleWithProviders{
    return {
      ngModule: FirebaseUploadModule,
      providers: [
        {
          provide: FirbaseUploadConfig,
          useValue: config
        }
      ]
    }
  }
}
