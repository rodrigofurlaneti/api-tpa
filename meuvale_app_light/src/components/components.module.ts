import { NgModule } from '@angular/core';
import { HeaderRetireComponent } from './header-retire/header-retire';
import { IonicModule } from 'ionic-angular';
import { ProductStructComponent } from './product/product-struct';

@NgModule({
	declarations: [
		HeaderRetireComponent,
		ProductStructComponent
	],
	imports: [
		IonicModule
	],
	exports: [	
		HeaderRetireComponent,
		ProductStructComponent
	]
})
export class ComponentsModule {}
