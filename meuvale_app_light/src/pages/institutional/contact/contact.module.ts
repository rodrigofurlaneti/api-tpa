import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ContactPage } from './contact';
import { BrMaskerModule } from 'brmasker-ionic-3';

@NgModule({
  declarations: [
    ContactPage,
  ],
  imports: [
    BrMaskerModule,
    IonicPageModule.forChild(ContactPage),
  ],
})
export class ContactPageModule {}
