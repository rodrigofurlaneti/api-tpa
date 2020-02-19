import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ParceirosPage } from './parceiros';

@NgModule({
  declarations: [
    ParceirosPage,
  ],
  imports: [
    IonicPageModule.forChild(ParceirosPage),
  ],
})
export class ParceirosPageModule {}
