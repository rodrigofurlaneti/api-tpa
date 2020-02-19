import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ProductPlanOrderPage } from './product-plan-order';
import { PipesModule } from '../../../pipes/pipes.module';
import { BrMaskerModule } from 'brmasker-ionic-3';

@NgModule({
  declarations: [
    ProductPlanOrderPage,
  ],
  imports: [
    PipesModule,
    BrMaskerModule,
    IonicPageModule.forChild(ProductPlanOrderPage),
  ],
})
export class ProductPlanOrderPageModule {}
