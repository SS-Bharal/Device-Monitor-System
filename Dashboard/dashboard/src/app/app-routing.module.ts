import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ChartDataComponent } from './components/chart-data/chart-data.component';
import { CreateDeviceComponent } from './components/create-device/create-device.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { TableDataComponent } from './components/table-data/table-data.component';

const routes: Routes = [
  { path: '', component: TableDataComponent},
  { path: 'createdevice', component: CreateDeviceComponent},
  { path: 'chart', component: ChartDataComponent},
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
